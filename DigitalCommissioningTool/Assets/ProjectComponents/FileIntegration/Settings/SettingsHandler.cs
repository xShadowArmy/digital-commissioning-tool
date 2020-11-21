using System.Xml;
using ProjectComponents.Abstraction;
using SystemFacade;

namespace ProjectComponents.FileIntegration
{
    public class SettingsHandler
    {
        private SettingsReader Reader { get; set; }
        private SettingsWriter Writer { get; set; }

        public SettingsHandler()
        {
            Reader = new SettingsReader( new XmlDocument() );
            Writer = new SettingsWriter( new XmlDocument() );
        }

        public InternalProjectSettings LoadFile( )
        {
            LogManager.WriteInfo( "ProjektSettings Datei wird gelesen.", "SettingsHandler", "LoadFile" );

            InternalProjectSettings tmp = new InternalProjectSettings( );

            Reader.ReadFile( tmp );

            return tmp;
        }

        public void SaveFile( InternalProjectSettings settings )
        {
            LogManager.WriteInfo( "ProjektSettings Datei wird geschrieben.", "SettingsHandler", "SaveFile" );
            Writer.WriteFile( settings );
        }
    }
}
