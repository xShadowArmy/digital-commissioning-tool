using System;
using System.Collections.Generic;
using SystemTools.Logging;
using System.IO;
using System.Xml;

namespace SystemTools.ManagingRessources
{
    /// <summary>
    /// Bietet Möglichkeiten zum Erstellen, speichern und laden von Configurations Daten.
    /// </summary>
    public class ConfigManager : IDisposable
    {
        /// <summary>
        /// Gibt an ob das Objekt bereits freigegeben wurde.
        /// </summary>
        private bool _disposed { get; set; }

        /// <summary>
        /// Die gepufferten Daten einer Configurations Datei.
        /// </summary>
        internal class ConfigBuffer
        {
            internal List<ConfigData> Data { get; private set; }

            public ConfigBuffer()
            {
                Data = new List<ConfigData>( );
            }
        }

        /// <summary>
        /// Der Name der aktuell geöffneten Konfigurations Datei.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gibt an ob ein offener Datenstream vorliegt.
        /// </summary>
        public bool OpenStream { get; set; }

        /// <summary>
        /// Gibt an ob die Daten automatisch in die Datei geschrieben werden sollen.
        /// </summary>
        public bool AutoFlush { get; set; }

        /// <summary>
        /// Wird verwendet um Daten zu speichern.
        /// </summary>
        private ConfigWriter Writer { get; set; }

        /// <summary>
        /// Wird verwendet um Daten zu lessen.
        /// </summary>
        private ConfigReader Reader { get; set; }

        /// <summary>
        /// Enthält die Datenelemente einer Konfigurationsdatei.
        /// </summary>
        private ConfigBuffer Buffer { get; set; }

        /// <summary>
        /// Die aktuell geöffnete Konfigurations Datei.
        /// </summary>
        private XmlDocument ConfigFile { get; set; }

        /// <summary>
        /// Der Systempfad an dem die Konfigurations Dateien gespeichert werden.
        /// </summary>
        private readonly string PATH;

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        public ConfigManager()
        {
            LogManager.WriteInfo( "Initialisierung eines ConfigManagers", "ConfigManager", "ConfigManager" );

            _disposed = false;
            OpenStream = false;
            AutoFlush = true;
#if DEBUG
            PATH = ".\\Output\\Ressources\\Data";
#else
            PATH = ".\\Ressources\\Data";
#endif
            ConfigFile = new XmlDocument( );

            CreateDir( );
        }

        /// <summary>
        /// Schließt bei Bedarf den noch geöffneten DatenStream.
        /// </summary>
        ~ConfigManager()
        {
            if ( OpenStream )
            {
                CloseConfigFile( );
            }
        }

        /// <summary>
        /// Speichert ein Array mit einem eindeutigen Schlüssel.
        /// </summary>
        /// <param name="key">Ein eindeutiger Schlüssel.</param>
        /// <param name="data">Die Daten die gespeichert werden sollen.</param>
        /// <param name="overwrite">Gibt an, ob die Daten überschrieben werden sollen, wenn der Schlüssel bereits verwendet wird.</param>
        /// <returns>Gibt true zurück, wenn die Daten erfolgreich gespeichert oder überschrieben wurden.</returns>
        public bool StoreData( string key, Array data, bool overwrite = true )
        {
            LogManager.WriteInfo( "Speichere Daten mit Schluessel: " + key, "ConfigManager", "StoreData" );

            if ( OpenStream )
            {
                return Writer.StoreData( key, data, overwrite, Buffer );
            }

            return false;
        }

        /// <summary>
        /// Speichert einen Wert mit einem eindeutigen Schlüssel.
        /// </summary>
        /// <param name="key">Ein eindeutiger Schlüssel.</param>
        /// <param name="data">Die Daten die gespeichert werden sollen.
        /// </param>Gibt an, ob die Daten überschrieben werden sollen, wenn der Schlüssel bereits verwendet wird.</param>
        /// <returns>Gibt true zurück, wenn die Daten erfolgreich gespeichert oder überschrieben wurden.</returns>
        public bool StoreData( string key, object data, bool overwrite = true )
        {
            LogManager.WriteInfo( "Speichere Daten mit Schluessel: " + key, "ConfigManager", "StoreData" );

            if ( OpenStream )
            {
                return Writer.StoreData( key, data, overwrite, Buffer );
            }

            return false;
        }

        /// <summary>
        /// Speicher ein Objekt mit eindeutigen Schlüssel.
        /// </summary>
        /// <param name="key">Ein eindeutiger Schlüssel.</param>
        /// <param name="data">Die Daten die gespeichert werden sollen.</param>
        /// <param name="overwrite"> Gibt an, ob die Daten überschrieben werden sollen, wenn der Schlüssel bereits verwendet wird.</param>
        /// <returns>Gibt true zurück, wenn die Daten erfolgreich gespeichert oder überschrieben wurden.</returns>
        public bool StoreData( string key, ISerialConfigData data, bool overwrite = true )
        {
            LogManager.WriteInfo( "Speichere Objekt: " + key, "ConfigManager", "StoreData" );

            if ( OpenStream )
            {
                return Writer.StoreData( key, data, overwrite, Buffer );
            }

            return false;
        }

        /// <summary>
        /// Lädt die Daten mit dem angegebenen Schlüssel.
        /// </summary>
        /// <param name="key">Der Schlüssel der Daten.</param>
        /// <returns>Gibt die geladenen Daten oder null zurück.</returns>
        public ConfigData LoadData( string key )
        {
            LogManager.WriteInfo( "Lade Daten mit Schluessel: " + key, "ConfigManager", "LoadData" );

            if ( OpenStream )
            {
                return Reader.LoadData( Buffer, key );
            }

            return null;
        }

        /// <summary>
        /// Lädt die Daten mit der angegebenen ID.
        /// </summary>
        /// <param name="id">Die ID der Daten.</param>
        /// <returns>Gibt die geladenen Daten oder null zurück.</returns>
        public ConfigData LoadData( long id )
        {
            LogManager.WriteInfo( "Speichere Daten mit ID: " + id, "ConfigManager", "LoadData" );

            if ( OpenStream )
            {
                return Reader.LoadData( Buffer, id );
            }

            return null;
        }

        /// <summary>
        /// Lädt das Objekt mit dem angegebenen Schlüssel.
        /// </summary>
        /// <param name="key">Der Schlüssel der Daten.</param>
        /// <param name="data">Das Objekt, in das die Daten geladen werden sollen.</param>         
        /// <returns>Gibt die geladenen Daten oder null zurück.</returns>
        public void LoadData( string key, ISerialConfigData data )
        {
            LogManager.WriteInfo( "Lade Objekt: " + key, "ConfigManager", "StoreData" );

            if ( OpenStream )
            {
                Reader.LoadData( Buffer, key, data );
            }
        }

        /// <summary>
        /// Entfernt die Daten mit dem angegebenen Schlüssel.
        /// </summary>
        /// <param name="key">Der Schlüssel der Daten.</param>
        /// <returns>Gibt true zurück, wenn die Daten erfolgreich entfernt werden konnten.</returns>
        public bool RemoveData( string key )
        {
            LogManager.WriteInfo( "Loesche Daten mit Schluessel: " + key, "ConfigManager", "RemoveData" );

            if ( OpenStream )
            {
                return Writer.RemoveData( key, Buffer );
            }

            return false;
        }

        /// <summary>
        /// Entfernt die Daten mit der angegebenen ID.
        /// </summary>
        /// <param name="id">Die ID der Daten.</param>
        /// <returns>Gibt true zurück, wenn die Daten erfolgreich entfernt werden konnten.</returns>
        public bool RemoveData( long id )
        {
            LogManager.WriteInfo( "Loesche Daten mit ID: " + id, "ConfigManager", "RemoveData" );

            if ( OpenStream )
            {
                return Writer.RemoveData( id, Buffer );
            }

            return false;
        }

        /// <summary>
        /// Öffnet eine Konfigurations Datei.
        /// </summary>
        /// <param name="name">Der Name der Konfigurationsdatei.</param>
        /// <param name="create">Gibt an ob die Konfigurations Datei erstellt werden soll.</param>
        public void OpenConfigFile( string name, bool create = false )
        {
            FileName = PATH + "\\" + name + ( ( name.EndsWith( ".xml" ) ? "" : ".xml" ) );

            LogManager.WriteInfo( "Oeffnen einer neuen ConfigDatei. Name: " + FileName, "ConfigManager", "OpenConfigFile" );

            if ( !File.Exists( FileName ) )
            {
                if ( !create )
                {
                    LogManager.WriteLog( "Die angegebene ConfigDatei konnte nicht gefunden werden! Pfad: " + FileName, LogLevel.Error, true, "ConfigManager", "OpenConfigFile" );
                }

                CreateFile( FileName );

                Buffer = new ConfigBuffer( );
                Reader = new ConfigReader( ConfigFile, Buffer, false );
                Writer = new ConfigWriter( ConfigFile );

                OpenStream = true;

                return;
            }

            InitializeConfigFile( false, FileName );

            File.Delete( FileName );

            CreateFile( FileName );

            OpenStream = true;
        }

        /// <summary>
        /// Schließt und Schreibt den Stream in die Datei.
        /// </summary>
        public void CloseConfigFile()
        {
            LogManager.WriteInfo( "Config Datei wird geschlossen.", "ConfigManager", "CloseConfigFile" );

            if ( OpenStream )
            {
                if ( AutoFlush )
                {
                    Flush( );
                }

                OpenStream = false;
            }
        }

        /// <summary>
        /// Schreibt den Stream in die Datei.
        /// </summary>
        public void Flush()
        {
            LogManager.WriteInfo( "Config Datei wird geschrieben.", "ConfigManager", "Flush" );

            Writer.WriteConfigFile( Buffer, FileName );
        }

        /// <summary>
        /// Erstellt eine neue Konfigurations Datei.
        /// </summary>
        /// <param name="file">Der Name der Datei die erstellt werden soll.</param>
        private void CreateFile( string file )
        {
            try
            {
                using ( StreamWriter writer = new StreamWriter( File.Create( file ) ) )
                {
                    writer.WriteLine( "<?xml version=\"1.0\" encoding=\"utf-8\"?>" );
                    writer.WriteLine( "<xs:Config xs:dataCount=\"0\" xmlns:xs=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Ressources/Data\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Ressources/Data ConfigSchema.xsd\">" );
                    writer.WriteLine( "</xs:Config>" );

                    writer.Flush( );
                }

                ConfigFile.Load( file );
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Die angegebene ConfigDatei konnte nicht erstellt werden! Pfad: " + file + " Fehler: " + e.Message, LogLevel.Error, true, "ConfigManager", "CreateFile" );
            }
        }

        /// <summary>
        /// Erstellt die Verzeichnisstruktur für die Konfigurations Dateien.
        /// </summary>
        private void CreateDir()
        {
            if ( !Directory.Exists( PATH ) )
            {
                try
                {
                    Directory.CreateDirectory( PATH );
                }

                catch ( Exception e )
                {
                    LogManager.WriteLog( "Verzeichnisstruktur fuer Konfigurations Dateien konnte nicht erstellt werden! Pfad: " + PATH + " Fehler: " + e.Message, LogLevel.Error, true, "ConfigManager", "CreateDir" );
                }
            }
        }

        /// <summary>
        /// Lädt die Daten einer Konfigurations Datei.
        /// </summary>
        /// <param name="newConfig">Gibt an ob die Konfigurations Datei gelesen werden muss.</param>
        /// <param name="file">Der Pfad der Konfigurationsdatei.</param>
        private void InitializeConfigFile( bool newConfig, string file = "" )
        {
            if ( !newConfig )
            {
                try
                {
                    ConfigFile.Load( file );
                }

                catch ( Exception e )
                {
                    LogManager.WriteLog( "Die angegebene ConfigDatei konnte nicht geoeffnet werden! Pfad: " + file + " Fehler: " + e.Message, LogLevel.Error, true, "ConfigManager", "InitializeConfigFile" );
                }

                Buffer = new ConfigBuffer( );
                Reader = new ConfigReader( ConfigFile, Buffer, false );
                Writer = new ConfigWriter( ConfigFile );
            }
        }

        /// <summary>
        /// Gibt Ressourcen wieder frei.
        /// </summary>
        public void Dispose()
        {
            Dispose( true );

            GC.SuppressFinalize( this );
        }

        /// <summary>
        /// Gibt Ressourcen wieder frei.
        /// </summary>
        protected virtual void Dispose( bool disposing )
        {
            if ( !_disposed )
            {
                if ( disposing )
                {
                    if ( OpenStream )
                    {
                        CloseConfigFile( );
                    }
                }

                _disposed = true;
            }
        }
    }
}
