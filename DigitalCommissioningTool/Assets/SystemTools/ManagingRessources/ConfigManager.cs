using System;
using System.Collections.Generic;
using SystemTools.Logging;
using System.IO;
using System.Xml;

namespace SystemTools.ManagingRessources
{
    public class ConfigManager
    {
        internal class ConfigBuffer
        {
            internal List<ConfigData> Data { get; private set; }

            public ConfigBuffer()
            {
                Data = new List<ConfigData>( );
            }
        }

        public bool OpenStream { get; set; }
        public bool AutoFlush { get; set; }
        private ConfigWriter Writer { get; set; }
        private ConfigReader Reader { get; set; }
        private ConfigBuffer Buffer { get; set; }
        private XmlDocument ConfigFile { get; set; }
        private readonly string PATH;
        private string FileName { get; set; }

        public ConfigManager()
        {
            LogManager.WriteInfo( "Initialisierung eines ConfigManagers", "ConfigManager", "ConfigManager" );

            OpenStream = false;
            AutoFlush  = true;
#if DEBUG
            PATH = ".\\Output\\Ressources\\Data";
#else
            PATH = ".\\Ressources\\Data";
#endif
            ConfigFile = new XmlDocument( );

            CreateDir( );
        }

        ~ConfigManager()
        {
            if ( OpenStream )
            {
                CloseConfigFile( );
            }
        }

        public bool StoreData( string key, Array data, bool overwrite = true )
        {
            LogManager.WriteInfo( "Speichere Daten mit Schluessel: " + key, "ConfigManager", "StoreData");

            if ( OpenStream )
            {
                return Writer.StoreData( key, data, overwrite, Buffer );
            }

            return false;
        }

        public bool StoreData( string key, object data, bool overwrite = true )
        {
            LogManager.WriteInfo( "Speichere Daten mit Schluessel: " + key, "ConfigManager", "StoreData" );

            if ( OpenStream )
            {
                return Writer.StoreData( key, data, overwrite, Buffer );
            }

            return false;
        }
        
        public bool StoreData( string key, ISerialConfigData data, bool overwrite = true )
        {
            LogManager.WriteInfo( "Speichere Objekt: " + key, "ConfigManager", "StoreData" );

            if ( OpenStream )
            {
                return Writer.StoreData( key, data, overwrite, Buffer );
            }

            return false;
        }

        public ConfigData LoadData( string key )
        {
            LogManager.WriteInfo( "Lade Daten mit Schluessel: " + key, "ConfigManager", "LoadData" );

            if ( OpenStream )
            {
                return Reader.LoadData( Buffer, key );
            }

            return null;
        }

        public ConfigData LoadData( long id )
        {
            LogManager.WriteInfo( "Speichere Daten mit ID: " + id, "ConfigManager", "LoadData" );

            if ( OpenStream )
            {
                return Reader.LoadData( Buffer, id );
            }

            return null;
        }

        public void LoadData( string key, ISerialConfigData data )
        {
            LogManager.WriteInfo( "Lade Objekt: " + key, "ConfigManager", "StoreData" );

            if ( OpenStream )
            {
                Reader.LoadData( Buffer, key, data );
            }
        }

        public bool RemoveData( string key )
        {
            LogManager.WriteInfo( "Loesche Daten mit Schluessel: " + key, "ConfigManager", "RemoveData" );

            if ( OpenStream )
            {
                return Writer.RemoveData( key, Buffer );
            }

            return false;
        }

        public bool RemoveData( long id )
        {
            LogManager.WriteInfo( "Loesche Daten mit ID: " + id, "ConfigManager", "RemoveData" );

            if ( OpenStream )
            {
                return Writer.RemoveData( id, Buffer );
            }

            return false;
        }

        public void OpenConfigFile( string name, bool create = false )
        {
            FileName = PATH + "\\" + name + ( ( name.EndsWith( ".xml" ) ? "" : ".xml" ) );

            LogManager.WriteInfo( "Oeffnen einer neuen ConfigDatei. Name: " + FileName, "ConfigManager", "OpenConfigFile" );

            if ( !File.Exists( FileName ) )
            {
                if ( !create )
                {
                    LogManager.WriteError( "Die angegebene ConfigDatei konnte nicht gefunden werden! Pfad: " + FileName, "ConfigManager", "OpenConfigFile" );
                    throw new Exception( "Die angegebene ConfigDatei konnte nicht gefunden werden! Pfad: " + FileName );
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
        
        public void CloseConfigFile()
        {
            LogManager.WriteInfo( "Config Datei wird geschlossen.", "ConfigManager", "CloseCOnfigFile" );

            if ( OpenStream )
            {
                if ( AutoFlush )
                {
                    Flush( );
                }

                OpenStream = false;
            }
        }

        public void Flush()
        {
            LogManager.WriteInfo( "Config Datei wird geschrieben.", "ConfigManager", "Flush" );

            Writer.WriteConfigFile( Buffer, FileName );
        }

        private void CreateFile( string file )
        {
            try
            {
                using ( StreamWriter writer = new StreamWriter( File.Create( file ) ) )
                {
                    writer.WriteLine( "<?xml version=\"1.0\" encoding=\"utf-8\"?>" );
                    writer.WriteLine( "<xs:Config xs:dataCount=\"0\" xmlns:xs=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Ressources/Data\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Ressources/Data ConfigSchema.xsd\">");
                    writer.WriteLine( "</xs:Config>");

                    writer.Flush( );
                }

                ConfigFile.Load( file );
            }

            catch( Exception e )
            {
                LogManager.WriteLog( "Die angegebene ConfigDatei konnte nicht erstellt werden! Pfad: " + file + " Fehler: " + e.Message, LogLevel.Error, true, "ConfigManager", "CreateFile" );
            }
        }

        private void CreateDir( )
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
    }
}
