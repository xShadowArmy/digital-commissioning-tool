using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using SystemTools.Handler;

namespace SystemTools.ManagingResources
{
    /// <summary>
    /// Wird zum Lesen von StringResource Dateien verwendet.
    /// </summary>
    internal class StringResourceReader
    {
        /// <summary>
        /// Repräsentiert die StringResource Xml Datei.
        /// </summary>
        private XmlDocument Doc { get; set; }

        /// <summary>
        /// Wird fuer das Schreiben von LogDateien verwendet.
        /// </summary>
        private LogHandler Logger;

        /// <summary>
        /// Repräsentiert einen Eintrag in einer StringResourceDatei.
        /// </summary>
        internal struct StringResourceData
        {
            public string Name  { get; set; }
            public string Value { get; set; }
            public long   ID    { get; set; }
        }
        
        /// <summary>
        /// Informationen zur aktuellen Systemsprache.
        /// </summary>
        private CultureInfo LangInfo { get; set; }

        /// <summary>
        /// Der Pfad an dem die StringResourcen liegen.
        /// </summary>
        private string Path { get; set; }

        /// <summary>
        /// Initialisiert den StringResourceReader und ließt die Resourcen für die aktuelle Systemsprache in den Speicher.
        /// </summary>
        /// <param name="path">Pfad an dem die StringResourcen liegen.</param>
        /// <param name="info">Informationen über die Systemsprache.</param>
        /// <param name="stringResources">Das Pufferobjekt.</param>
        internal StringResourceReader( string path, XmlDocument doc, CultureInfo info, List<StringResourceData> stringResources )
        {
            Logger = new LogHandler( );

            Logger.WriteInfo( "Initialisierung des StringResourceReader", "StringResourceReader", "StringResourceReader" );

            Path     = path;
            LangInfo = info;
            Doc      = doc;

            ReadStringResources( stringResources );
        }

        /// <summary>
        /// Lädt eine StringResource anhand ihres Namens.
        /// </summary>
        /// <param name="name">Der Name der StringResource.</param>
        /// <returns>Die StringResource oder <see cref="string.Empty"> wenn die Resource nicht gefunden wurde</see>/></returns>
        internal string LoadString( string name, List<StringResourceData> stringResources )
        {
            name = RemoveQualifier( name );
            
            string tmp = "MISSING RESOURCE";

            foreach( StringResourceData data in stringResources )
            {
                if ( data.Name.Equals( name ) )
                {
                    tmp = data.Value;
                }
            }

            return tmp;
        }

        /// <summary>
        /// Lädt eine StringResource anhand ihrer ID.
        /// </summary>
        /// <param name="id">Die ID der StringResource.</param>
        /// <param name="stringResources">Das Pufferobjekt.</param>
        /// <returns>Die StringResource oder <see cref="string.Empty"> wenn die Resource nicht gefunden wurde</see>/></returns>
        internal string LoadString( long id, List<StringResourceData> stringResources )
        {
            string tmp = "MISSING RESOURCE";

            foreach ( StringResourceData data in stringResources )
            {
                if ( data.ID == id )
                {
                    tmp = data.Value;
                }
            }

            return tmp;
        }

        /// <summary>
        /// Überprüft ob eine StringResource mit der angegebenen ID verfügbar ist.
        /// </summary>
        /// <param name="id">Die ID der zu suchenden Resource.</param>
        /// <param name="stringResources">Das Pufferobjekt.</param>
        /// <returns>Gibt true zurück falls die Suche erfolgreich war.</returns>
        internal bool Exists( long id, List<StringResourceData> stringResources )
        {
            foreach ( StringResourceData data in stringResources )
            {
                if ( data.ID == id )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Überprüft, ob eine StringResource mit dem angegebenen Namen verfügbar ist.
        /// </summary>
        /// <param name="name">Der Name der zu suchenden Resource.</param>
        /// <param name="stringResources">Das Pufferobjekt.</param>
        /// <returns>Gibt true zurücl falls die Suche erfolgreich war.</returns>
        internal bool Exists( string name, List<StringResourceData> stringResources )
        {
            name = RemoveQualifier( name );

            foreach ( StringResourceData data in stringResources )
            {
                if ( data.Name.Equals( name ) )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Lädt die StringResourcen in den Speicher.
        /// </summary>
        /// <param name="stringResources">Das Pufferobjekt in das Geschrieben wird.</param>
        private void ReadStringResources( List<StringResourceData> stringResources )
        {
            Logger.WriteInfo( "Einlesen der StringResourcen", "StringResourceReader", "ReadStringResources" );

            string xmlns = "https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/";

            if ( stringResources != null && Doc != null )
            {
                try
                {
                    Logger.WriteInfo( "Erstellen des Navigators", "StringResourceReader", "ReadStringResources" );

                    XPathNavigator nav = Doc.CreateNavigator( );

                    if ( nav.MoveToFirstChild( ) )
                    {
                        Logger.WriteInfo( "Lesen des RootTags", "StringResourceReader", "ReadStringResources" );

                        if ( nav.LocalName == "StringResources" )
                        {
                            if ( !nav.GetAttribute( "lang", xmlns ).Equals( LangInfo.ThreeLetterISOLanguageName ) )
                            {
                                Logger.WriteError( "Fehler beim Lesen der StringResource headers!Sprache ist nicht identisch zur Systemsprache", "StringResourceReader", "ReadStringResources" );
                                throw new Exception( "Fehler beim Lesen der StringResource headers! Sprache ist nicht identisch zur Systemsprache" );
                            }

                            if ( nav.MoveToFirstChild() )
                            {
                                Logger.WriteInfo( "Lesen der Resourcen", "StringResourceReader", "ReadStringResources" );

                                do
                                {
                                    StringResourceData data = new StringResourceData( );

                                    if ( !long.TryParse( nav.GetAttribute( "id", xmlns ), out long tmpID ))
                                    {
                                        Logger.WriteInfo( "Fehler beim Interpretieren einer StringID! ID ist keine ganze Zahl", "StringResourceReader", "ReadStringResources" );
                                    }
                                                                        
                                    data.Name  = nav.GetAttribute( "name", xmlns );
                                    data.ID    = tmpID;

                                    if ( nav.MoveToFirstChild() )
                                    {
                                        data.Value = nav.Value;

                                        nav.MoveToParent( );
                                    }

                                    stringResources.Add( data );

                                } while ( nav.MoveToNext( ) );

                                for( int i = 0; i < stringResources.Count; i++ )
                                {
                                    StringResourceData tmp = stringResources[ i ];
                                    stringResources.Remove( tmp );

                                    tmp.ID = i + 1;
                                    stringResources.Insert( i, tmp );
                                }
                            }

                            else
                            {
                                Logger.WriteWarning( "Fehler beim Einlesen der StringResourcen! Keine Kindelemente in StringResources", "StringResourceReader", "ReadStringResources" );
                            }
                        }
                        
                        else
                        {
                            Logger.WriteError( "Fehler beim Einlesen der StringResourcen! Element StringResources konnte nicht gefunden werden!", "StringResourceReader", "ReadStringResources" );
                            throw new Exception( "Fehler beim Einlesen der StringResourcen! Element StringResources konnte nicht gefunden werden!" );
                        }
                    }
                }

                catch( Exception e )
                {
                    Logger.WriteError( "Fehler beim Einlesen der StringResourcen! Fehler: " + e.Message , "StringResourceReader", "ReadStringResources" );
                    throw new Exception( "Fehler beim Einlesen der StringResourcen! Fehler: " + e.Message );
                }
            }
        }

        /// <summary>
        /// Entfernt das @ bei den StringIDs.
        /// </summary>
        /// <param name="name">Der name der StringIDs von dem der Qualifier entfernt werden soll.</param>
        /// <returns>Die StringID ohne Qualifier.</returns>
        private string RemoveQualifier( string name )
        {
            if ( name.StartsWith( "@" ) )
            {
                return name.Substring( 1 );
            }

            return name;
        }       
    }
}
