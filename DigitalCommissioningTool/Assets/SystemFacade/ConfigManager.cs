using System;
using SystemTools.ManagingResources;
using SystemTools;
using SystemTools.Handler;

namespace SystemFacade
{
    /// <summary>
    /// Bietet Möglichkeiten zum Erstellen, speichern und laden von Configurations Daten.
    /// </summary>
    public class ConfigManager : IDisposable
    {
        /// <summary>
        /// Der Name der aktuell geöffneten Konfigurations Datei.
        /// </summary>
        public string FileName
        {
            get
            {
                return Handler.FileName;
            }
        }

        /// <summary>
        /// Gibt an ob ein offener Datenstream vorliegt.
        /// </summary>
        public bool OpenStream
        {
            get
            {
                return Handler.OpenStream;
            }
        }

        /// <summary>
        /// Gibt an ob die Daten automatisch in die Datei geschrieben werden sollen.
        /// </summary>
        public bool AutoFlush
        {
            get
            {
                return Handler.AutoFlush;
            }

            set
            {
                Handler.AutoFlush = value;
            }
        }

        /// <summary>
        /// Objekt das von der Facade verdeckt wird.
        /// </summary>
        private ConfigHandler Handler;

        /// <summary>
        /// Gibt an ob das Objekt bereits freigegeben wurde.
        /// </summary>
        private bool _disposed { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        public ConfigManager()
        {
            _disposed = false;
            Handler = new ConfigHandler( );
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
            return Handler.StoreData( key, data, overwrite );
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
            return Handler.StoreData( key, data, overwrite );
        }

        /// <summary>
        /// Speichert ein Objekt mit eindeutigen Schlüssel.
        /// </summary>
        /// <param name="key">Ein eindeutiger Schlüssel.</param>
        /// <param name="data">Die Daten die gespeichert werden sollen.</param>
        /// <param name="overwrite"> Gibt an, ob die Daten überschrieben werden sollen, wenn der Schlüssel bereits verwendet wird.</param>
        /// <returns>Gibt true zurück, wenn die Daten erfolgreich gespeichert oder überschrieben wurden.</returns>
        public bool StoreData( string key, ISerialConfigData data, bool overwrite = true )
        {
            return Handler.StoreData( key, data, overwrite );
        }
        
        /// <summary>
        /// Lädt die Daten mit dem angegebenen Schlüssel.
        /// </summary>
        /// <param name="key">Der Schlüssel der Daten.</param>
        /// <returns>Gibt die geladenen Daten oder null zurück.</returns>
        public ConfigData LoadData( string key )
        {
            return Handler.LoadData( key );
        }

        /// <summary>
        /// Lädt die Daten mit der angegebenen ID.
        /// </summary>
        /// <param name="id">Die ID der Daten.</param>
        /// <returns>Gibt die geladenen Daten oder null zurück.</returns>
        public ConfigData LoadData( long id )
        {
            return Handler.LoadData( id );
        }

        /// <summary>
        /// Lädt das Objekt mit dem angegebenen Schlüssel.
        /// </summary>
        /// <param name="key">Der Schlüssel der Daten.</param>
        /// <param name="data">Das Objekt, in das die Daten geladen werden sollen.</param>         
        /// <returns>Gibt die geladenen Daten oder null zurück.</returns>
        public void LoadData( string key, ISerialConfigData data )
        {
            Handler.LoadData( key, data );
        }

        /// <summary>
        /// Entfernt die Daten mit dem angegebenen Schlüssel.
        /// </summary>
        /// <param name="key">Der Schlüssel der Daten.</param>
        /// <returns>Gibt true zurück, wenn die Daten erfolgreich entfernt werden konnten.</returns>
        public bool RemoveData( string key )
        {
            return Handler.RemoveData( key );
        }

        /// <summary>
        /// Entfernt die Daten mit der angegebenen ID.
        /// </summary>
        /// <param name="id">Die ID der Daten.</param>
        /// <returns>Gibt true zurück, wenn die Daten erfolgreich entfernt werden konnten.</returns>
        public bool RemoveData( long id )
        {
            return Handler.RemoveData( id );
        }

        /// <summary>
        /// Öffnet eine Konfigurations Datei.
        /// </summary>
        /// <param name="name">Der Name der Konfigurationsdatei.</param>
        /// <param name="create">Gibt an ob die Konfigurations Datei erstellt werden soll.</param>
        public void OpenConfigFile( string name, bool create = false )
        {
            Handler.OpenConfigFile( name, create );
        }

        /// <summary>
        /// Öffnet eine Konfigurations Datei.
        /// </summary>
        /// <param name="name">Der Name der Konfigurationsdatei.</param>
        /// <param name="create">Gibt an ob die Konfigurations Datei erstellt werden soll.</param>
        public void OpenConfigFile( string path, string name, bool create = false )
        {
            Handler.OpenConfigFile( path, name, create );
        }

        /// <summary>
        /// Schließt und Schreibt den Stream in die Datei.
        /// </summary>
        public void CloseConfigFile()
        {
            Handler.CloseConfigFile( );
        }

        /// <summary>
        /// Schreibt den Stream in die Datei.
        /// </summary>
        public void Flush()
        {
            Handler.Flush( );
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
                    Handler.Dispose( );
                }

                _disposed = true;
            }
        }
    }
}

