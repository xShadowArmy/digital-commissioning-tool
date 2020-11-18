using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SystemFacade;
using ProjectComponents.Abstraction;
using System.Xml.XPath;

namespace ProjectComponents.FileIntegration
{
    internal class DataReader
    {
        private XmlDocument Doc { get; set; }

        internal DataReader( XmlDocument doc )
        {
            Doc = doc;
        }

        internal void ReadFile( InternalProjectData data )
        {
            LogManager.WriteInfo( "Datei \"Data.xml\" wird gelesen.", "DataReader", "ReadFile" );

            try
            {
                Doc.Load( Paths.TempPath + "Data.xml" );

                XPathNavigator nav = Doc.CreateNavigator( );

                nav.MoveToFirstChild( );
                nav.MoveToFirstChild( );

                data.ChangeProjectName( nav.LocalName );
                nav.MoveToNext( );

                data.ChangeProjectPath( nav.LocalName );
                nav.MoveToNext( );

                data.ChangeDateCreated( DateTime.Parse( nav.LocalName ) );
                nav.MoveToNext( );

                data.ChangeDateModified( DateTime.Parse( nav.LocalName ) );
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Data.xml\" konnte nicht gelesen werden! Fehler: " + e.Message, LogLevel.Error, true, "DataReader", "ReadFile" );
            }
        }
    }
}
