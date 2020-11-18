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

        public void LoadFile( InternalProjectSettings settings )
        {
            LogManager.WriteInfo( "ProjektSettings Datei wird gelesen.", "SettingsHandler", "LoadFile" );
            Reader.ReadFile( settings );
        }

        public void SaveFile( InternalProjectSettings settings )
        {
            LogManager.WriteInfo( "ProjektSettings Datei wird gelesen.", "SettingsHandler", "SaveFile" );
            Writer.WriteFile( settings );
        }
    }
}
