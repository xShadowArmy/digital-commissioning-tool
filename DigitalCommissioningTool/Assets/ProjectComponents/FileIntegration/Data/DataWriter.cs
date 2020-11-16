using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using ProjectComponents.Abstraction;
using SystemFacade;

namespace ProjectComponents.FileIntegration
{
    internal class DataWriter
    {
        private XmlDocument Doc { get; set; }

        internal DataWriter( XmlDocument doc )
        {
            Doc = doc;
        }

        internal void WriteFile( ProjectData data )
        {
            LogManager.WriteInfo( "Datei \"Data.xml\" wird erstellt." );

            ReCreateFile( );

            string xmlns = "https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/";

            try
            {
                Doc.Load( Paths.TempPath + "Data.xml" );

                XPathNavigator nav = Doc.CreateNavigator( );

                nav.MoveToFirstChild( );
                nav.AppendChildElement( "xs", "Name", xmlns, data.Name );
                nav.AppendChildElement( "xs", "Path", xmlns, data.FullPath );
                nav.AppendChildElement( "xs", "DateCreated", xmlns, data.DateCreated.ToString() );
                nav.AppendChildElement( "xs", "DateModified", xmlns, data.DateModified.ToString() );

                XmlTextWriter writer = new XmlTextWriter( Paths.TempPath + "Data.xml", System.Text.Encoding.UTF8 )
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4
                };

                Doc.Save( writer );

                writer.Dispose( );
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Data.xml\" konnte nicht erstellt werden! Fehler: " + e.Message, LogLevel.Error, true, "DataWriter", "WriteFile" );
            }
        }

        private void ReCreateFile()
        {
            try
            {
                if ( File.Exists( Paths.TempPath + "Data.xml" ) )
                {
                    File.Delete( Paths.TempPath + "Data.xml" );
                }

                using( StreamWriter writer = new StreamWriter( File.Create( Paths.TempPath + "Data.xml" ) ) )
                {
                    writer.WriteLine( "<?xml version=\"1.0\" encoding=\"utf-8\"?>" );
                    writer.WriteLine( "<xs:ProjectData xmlns:xs=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/ ProjectDataSchema.xsd\">" );
                    writer.WriteLine( "</xs:ProjectData>" );

                    writer.Flush( );
                }
            }

            catch( Exception e )
            {
                LogManager.WriteLog( "Datei \"Data.xml\" konnte nicht erstellt werden! Fehler: " + e.Message, LogLevel.Error, true, "DataWriter", "ReCreateFile" );
            }
        }
    }
}
