using System.Xml;
using ProjectComponents.Abstraction;
using SystemFacade;

namespace ProjectComponents.FileIntegration
{
    /// <summary>
    /// Enthält Methoden zum Schreiben und Lesen von Projekteinstellungen.
    /// </summary>
    public class SettingsHandler
    {
        /// <summary>
        /// Objekt zum Lesen der Dateien.
        /// </summary>
        private SettingsReader Reader { get; set; }

        /// <summary>
        /// Objekt zum Schreiben der Dateien.
        /// </summary>
        private SettingsWriter Writer { get; set; }

        /// <summary>
        /// Initialisiert das Objekt.
        /// </summary>
        public SettingsHandler()
        {
            Reader = new SettingsReader( new XmlDocument() );
            Writer = new SettingsWriter( new XmlDocument() );
        }

        /// <summary>
        /// Ließt die Daten aus der Datei.
        /// </summary>
        /// <returns>Objekt das alle Daten enthält-</returns>
        public InternalProjectSettings LoadFile( )
        {
            LogManager.WriteInfo( "ProjektSettings Datei wird gelesen.", "SettingsHandler", "LoadFile" );

            InternalProjectSettings tmp = new InternalProjectSettings( );

            Reader.ReadFile( tmp );

            return tmp;
        }

        /// <summary>
        /// Schreibt die Daten in die Datei.
        /// </summary>
        /// <param name="settings">Die Daten die Gespeichert werden sollen.</param>
        public void SaveFile( InternalProjectSettings settings )
        {
            LogManager.WriteInfo( "ProjektSettings Datei wird geschrieben.", "SettingsHandler", "SaveFile" );
            Writer.WriteFile( settings );
        }
    }
}
