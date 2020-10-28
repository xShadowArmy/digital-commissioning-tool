using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using SystemTools.Logging;

namespace SystemTools.ManagingRessources
{
    /// <summary>
    /// Wird zum Lesen von StringRessource Dateien verwendet.
    /// </summary>
    internal class StringRessourceReader
    {
        /// <summary>
        /// Repräsentiert einen Eintrag in einer StringRessourceDatei.
        /// </summary>
        internal struct StringRessourceData
        {
            public string Name  { get; set; }
            public string Value { get; set; }
            public long   ID    { get; set; }
        }

        /// <summary>
        /// Die Eingelesenen StringRessource Daten.
        /// </summary>
        internal List<StringRessourceData> StringRessources { get; private set; }

        /// <summary>
        /// Repräsentiert die StringRessource Xml Datei.
        /// </summary>
        private XmlDocument Doc { get; set; }

        /// <summary>
        /// Informationen zur aktuellen Systemsprache.
        /// </summary>
        private CultureInfo LangInfo { get; set; }

        /// <summary>
        /// Der Pfad an dem die StringRessourcen liegen.
        /// </summary>
        private string Path { get; set; }

        /// <summary>
        /// Initialisiert den StringRessourceReader und ließt die Ressourcen für die aktuelle Systemsprache in den Speicher.
        /// </summary>
        /// <param name="path">Pfad an dem die StringRessourcen liegen.</param>
        /// <param name="info">Informationen über die Systemsprache.</param>
        internal StringRessourceReader( string path, CultureInfo info )
        {
            LogManager.WriteInfo( "Initialisierung des StringRessourceReader", "StringRessourceReader", "StringRessourceReader" );

            Path     = path;
            LangInfo = info;

            Doc = new XmlDocument( );

            try
            {
                Doc.Load( Path );
                ReadStringRessources();
            }

            catch ( FileNotFoundException e )
            {
                LogManager.WriteError( "StringRessource Datei wurde nicht gefunden! Pfad: " + path + " Fehler: " + e.Message, "StringRessourceReader", "StringRessourceReader" );

                Doc = null;
            }

            catch ( Exception e )
            {
                LogManager.WriteError( "StringRessource Datei konnte nicht geoffnet werden! Pfad: " + path + " Fehler: " + e.Message, "StringRessourceReader", "StringRessourceReader" );

                Doc = null;
            }
        }

        /// <summary>
        /// Lädt eine StringRessource anhand ihres Namens.
        /// </summary>
        /// <param name="name">Der Name der StringRessource.</param>
        /// <returns>Die StringRessource oder <see cref="string.Empty"> wenn die Ressource nicht gefunden wurde</see>/></returns>
        internal string LoadString( string name )
        {
            name = RemoveQualifier( name );
            
            string tmp = "MISSING RESSOURCE";

            foreach( StringRessourceData data in StringRessources )
            {
                if ( data.Name.Equals( name ) )
                {
                    tmp = data.Value;
                }
            }

            return tmp;
        }

        /// <summary>
        /// Lädt eine StringRessource anhand ihrer ID.
        /// </summary>
        /// <param name="id">Die ID der StringRessource.</param>
        /// <returns>Die StringRessource oder <see cref="string.Empty"> wenn die Ressource nicht gefunden wurde</see>/></returns>
        internal string LoadString( long id )
        {
            string tmp = "MISSING RESSOURCE";

            foreach ( StringRessourceData data in StringRessources )
            {
                if ( data.ID == id )
                {
                    tmp = data.Value;
                }
            }

            return tmp;
        }

        /// <summary>
        /// Überprüft ob eine StringRessource mit der angegebenen ID verfügbar ist.
        /// </summary>
        /// <param name="id">Die ID der zu suchenden Ressource.</param>
        /// <returns>Gibt true zurück falls die Suche erfolgreich war.</returns>
        internal bool Exists( long id )
        {
            foreach ( StringRessourceData data in StringRessources )
            {
                if ( data.ID == id )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Überprüft, ob eine StringRessource mit dem angegebenen Namen verfügbar ist.
        /// </summary>
        /// <param name="name">Der Name der zu suchenden Ressource.</param>
        /// <returns>Gibt true zurücl falls die Suche erfolgreich war.</returns>
        internal bool Exists( string name )
        {
            name = RemoveQualifier( name );

            foreach ( StringRessourceData data in StringRessources )
            {
                if ( data.Name.Equals( name ) )
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Lädt die StringRessourcen in den Speicher.
        /// </summary>
        private void ReadStringRessources()
        {
            LogManager.WriteInfo( "Einlesen der StringRessourcen", "StringRessourceReader", "ReadStringRessources" );

            string xmlns = "https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Ressources/Strings";

            if ( StringRessources == null && Doc != null )
            {
                StringRessources = new List<StringRessourceData>( );

                try
                {
                    LogManager.WriteInfo( "Erstellen des Navigators", "StringRessourceReader", "ReadStringRessources" );

                    XPathNavigator nav = Doc.CreateNavigator( );

                    if ( nav.MoveToFirstChild( ) )
                    {
                        LogManager.WriteInfo( "Lesen des RootTags", "StringRessourceReader", "ReadStringRessources" );

                        if ( nav.LocalName == "StringRessources" )
                        {
                            if ( !nav.GetAttribute( "lang", xmlns ).Equals( LangInfo.ThreeLetterISOLanguageName ) )
                            {
                                throw new Exception( "Fehler beim Lesen der StringRessource headers! Sprache ist nicht identisch zur Systemsprache" );
                            }

                            if ( nav.MoveToFirstChild() )
                            {
                                LogManager.WriteInfo( "Lesen der Ressourcen", "StringRessourceReader", "ReadStringRessources" );

                                do
                                {
                                    StringRessourceData data = new StringRessourceData( );

                                    if ( !long.TryParse( nav.GetAttribute( "id", xmlns ), out long tmpID ))
                                    {
                                        throw new Exception( "Fehler beim Interpretieren einer StringID! ID ist keine ganze Zahl" );
                                    }
                                                                        
                                    data.Name  = nav.GetAttribute( "name", xmlns );
                                    data.ID    = tmpID;

                                    if ( nav.MoveToFirstChild() )
                                    {
                                        data.Value = nav.Value;

                                        nav.MoveToParent( );
                                    }

                                    StringRessources.Add( data );

                                } while ( nav.MoveToNext( ) );
                            }

                            else
                            {
                                LogManager.WriteWarning( "Fehler beim Einlesen der StringRessourcen! Keine Kindelemente in StringRessources", "StringRessourceReader", "ReadStringRessources" );
                            }
                        }
                        
                        else
                        {
                            LogManager.WriteWarning( "Fehler beim Einlesen der StringRessourcen! Element StringRessources konnte nicht gefunden werden!", "StringRessourceReader", "ReadStringRessources" );
                        }
                    }
                }

                catch( Exception e )
                {
                    LogManager.WriteError( "Fehler beim Einlesen der StringRessourcen! Fehler: " + e.Message , "StringRessourceReader", "ReadStringRessources" );
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
