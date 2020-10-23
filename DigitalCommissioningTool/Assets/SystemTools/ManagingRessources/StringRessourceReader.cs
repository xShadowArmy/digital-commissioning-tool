using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using SystemTools.Logging;

namespace SystemTools.ManagingRessources
{
    internal class StringRessourceReader
    {
        internal class StringRessourceData
        {
            public string Name  { get; set; }
            public string Value { get; set; }
            public long   ID    { get; set; }
        }

        internal List<StringRessourceData> StringRessources { get; private set; }
        private XmlDocument Doc { get; set; }
        private CultureInfo LangInfo { get; set; }
        private string Path { get; set; }

        internal StringRessourceReader( string path, CultureInfo info )
        {
            LogManager.WriteInfo( "Initialisierung des StringRessourceReader", "StringRessourceReader", "StringRessourceReader" );

            Path     = path;
            LangInfo = info;

            ValidateXml( );

            Doc = new XmlDocument( );

            try
            {
                Doc.Load( Path );
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

        internal string LoadString( string name )
        {
            name = RemoveQualifier( name );
            
            string tmp = string.Empty;

            foreach( StringRessourceData data in StringRessources )
            {
                if ( data.Name.Equals( name ) )
                {
                    tmp = data.Value;
                }
            }

            return tmp;
        }

        internal string LoadString( long id )
        {
            string tmp = string.Empty;

            foreach ( StringRessourceData data in StringRessources )
            {
                if ( data.ID == id )
                {
                    tmp = data.Value;
                }
            }

            return tmp;
        }

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

        internal void ReadStringRessources()
        {
            LogManager.WriteInfo( "Einlesen der StringRessourcen", "StringRessourceReader", "ReadStringRessources" );

            if ( StringRessources == null && Doc != null )
            {
                StringRessources = new List<StringRessourceData>( );

                try
                {
                    LogManager.WriteInfo( "Erstellen des Navigators", "StringRessourceReader", "ReadStringRessources" );

                    XPathNavigator nav = Doc.CreateNavigator( );

                    if ( nav.MoveToFirstChild() )
                    {
                        LogManager.WriteInfo( "Lesen des RootTags", "StringRessourceReader", "ReadStringRessources" );

                        if ( nav.Name == "StringRessources" )
                        {
                            if ( !nav.GetAttribute( "lang", "" ).Equals( LangInfo.ThreeLetterISOLanguageName ) )
                            {
                                throw new Exception( "Fehler beim Lesen der StringRessource headers! Sprache ist nicht identisch zur Systemsprache" );
                            }

                            if ( nav.MoveToFirstChild() )
                            {
                                StringRessourceData data = null;
                                
                                LogManager.WriteInfo( "Lesen der Ressourcen", "StringRessourceReader", "ReadStringRessources" );

                                do
                                {
                                    data = new StringRessourceData( );

                                    if ( !long.TryParse( nav.GetAttribute( "id", "" ), out long tmpID ))
                                    {
                                        throw new Exception( "Fehler beim Interpretieren einer StringID! ID ist keine ganze Zahl" );
                                    }
                                                                        
                                    data.Name  = nav.GetAttribute( "name", "" );
                                    data.ID    = tmpID;

                                    if ( nav.MoveToFirstChild() )
                                    {
                                        data.Value = nav.Value;

                                        nav.MoveToParent( );
                                    }

                                    StringRessources.Add( data );

                                } while ( nav.MoveToNext( ) );
                            }
                        }
                    }
                }

                catch( Exception e )
                {
                    LogManager.WriteError( "Fehler beim Einlesen der StringRessourcen! Fehler: " + e.Message , "StringRessourceReader", "ReadStringRessources" );
                }
            }
        }

        private string RemoveQualifier( string name )
        {
            if ( name.StartsWith( "@" ) )
            {
                return name.Substring( 1 );
            }

            return name;
        }

        private void ValidateXml( )
        {
            XmlReaderSettings settings = new XmlReaderSettings( );

            settings.ValidationType   = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += new ValidationEventHandler( ValidationCallBack );
            
            XmlReader reader = XmlReader.Create( Path, settings );
            
            while ( reader.Read( ) );

            reader.Close( );
            reader.Dispose( );
        }

        private static void ValidationCallBack( object sender, ValidationEventArgs args )
        {
            if ( args.Severity == XmlSeverityType.Warning )
            {
                LogManager.WriteInfo( "XmlSchema nicht gefunden. Fehler: " + args.Message, "StringRessourceReader", "ValidationCallBack" ) ;
            }

            else
            {
                LogManager.WriteInfo( "Xml passt nicht zum XmlSchema. Fehler: " + args.Message, "StringRessourceReader", "ValidationCallBack" ) ;
            }
        }
    }
}
