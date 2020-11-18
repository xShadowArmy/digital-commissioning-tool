using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ProjectComponents.Abstraction;
using SystemFacade;

namespace ProjectComponents.FileIntegration
{
    public class DataHandler
    {
        private DataReader Reader { get; set; }
        private DataWriter Writer { get; set; }

        public DataHandler()
        {
            Reader = new DataReader( new XmlDocument() );
            Writer = new DataWriter( new XmlDocument() );
        }

        public void LoadFile( InternalProjectData data )
        {
            LogManager.WriteInfo( "ProjektData Datei wird gelesen.", "DataHandler", "LoadFile" );
            Reader.ReadFile( data );
        }

        public void SaveFile( InternalProjectData data )
        {
            LogManager.WriteInfo( "ProjektData Datei wird geschrieben.", "DataHandler", "SaveFile" );
            Writer.WriteFile( data );
        }
    }
}
