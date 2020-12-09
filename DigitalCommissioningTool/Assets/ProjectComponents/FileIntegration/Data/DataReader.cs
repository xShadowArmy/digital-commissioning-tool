using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SystemFacade;
using ProjectComponents.Abstraction;
using System.Xml.XPath;
using System.Globalization;

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

                data.Name =  nav.Value;
                nav.MoveToNext( );

                data.FullPath = nav.Value;
                nav.MoveToNext( );

                data.DateCreated = DateTime.ParseExact( nav.Value, "dd/MM/yyyy", CultureInfo.CurrentCulture );
                nav.MoveToNext( );

                data.DateModified = DateTime.ParseExact( nav.Value, "dd/MM/yyyy", CultureInfo.CurrentCulture );
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Data.xml\" konnte nicht  werden! Fehler: " + e.Message, LogLevel.Error, true, "DataReader", "ReadFile" );
            }
        }
    }
}
