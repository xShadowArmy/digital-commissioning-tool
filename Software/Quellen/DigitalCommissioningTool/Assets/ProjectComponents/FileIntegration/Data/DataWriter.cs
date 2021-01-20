using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using ProjectComponents.Abstraction;
using SystemFacade;

namespace ProjectComponents.FileIntegration
{
    /// <summary>
    /// Schreibt allgemeine Projektdaten in eine Xml Datei.
    /// </summary>
    internal class DataWriter
    {
        /// <summary>
        /// Die Datei in die geschrieben werden soll.
        /// </summary>
        private XmlDocument Doc { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="doc">Die Datei in die geschrieben werden soll.</param>
        internal DataWriter( XmlDocument doc )
        {
            Doc = doc;
        }

        /// <summary>
        /// Schreibt die Daten in die Datei.
        /// </summary>
        /// <param name="data">Die Daten die gespeichert werden sollen.</param>
        internal void WriteFile( InternalProjectData data )
        {
            LogManager.WriteInfo( "Datei \"Data.xml\" wird erstellt.", "DataWriter", "WriteFile" );

            ReCreateFile( );

            string xmlns = "https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/";

            try
            {
                Doc.Load( Paths.TempPath + "Data.xml" );

                XPathNavigator nav = Doc.CreateNavigator( );

                nav.MoveToFirstChild( );
                nav.AppendChildElement( "xs", "Name", xmlns, data.Name );
                nav.AppendChildElement( "xs", "Path", xmlns, data.FullPath );
                nav.AppendChildElement( "xs", "DateCreated", xmlns, data.DateCreated.ToString( "dd/MM/yyyy" ) );
                nav.AppendChildElement( "xs", "DateModified", xmlns, data.DateModified.ToString( "dd/MM/yyyy" ) );

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

        /// <summary>
        /// Erstellt die Datei neu.
        /// </summary>
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
