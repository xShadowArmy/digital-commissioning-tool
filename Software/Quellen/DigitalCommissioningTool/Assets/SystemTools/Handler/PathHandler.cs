using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SystemTools.Handler
{
    /// <summary>
    /// Uebernimmt das Speichern und Laden fuer die verwendeten Pfade.
    /// </summary>
    public class PathHandler
    {
        /// <summary>
        /// Tabelle mit den Pfaden und den passenden Namen als Schluessel.
        /// </summary>
        private Dictionary<string,string> Table;

        /// <summary>
        /// Objekt fuer das Schreiben von Logs.
        /// </summary>
        private LogHandler Logger;

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        public PathHandler()
        {
            Logger = new LogHandler( );

            Logger.WriteInfo( "PathHandler wird initialisiert", "PathHandler", "PathHandler" );

            Table  = new Dictionary<string, string>( );

            ReadPaths( );
        }

        /// <summary>
        /// Gibt den Pfad mit dem angegebenen Schluessel zurueck.
        /// </summary>
        /// <param name="name">Der Schluessel des Pfads.</param>
        /// <returns>Der gespeicherte Pfad.</returns>
        public string RetrievePath( string name )
        {
            foreach ( KeyValuePair<string,string> tmp in Table )
            {
                if ( name.Equals( tmp.Key ) )
                {
                    return tmp.Value;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Fuegt einen neuen Pfad der Tabelle hinzu.
        /// </summary>
        /// <param name="name">Der Key unter dem der Pfad gespeichert werden soll.</param>
        /// <param name="path">Der Pfad der gespeichert werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
        public bool AddPath( string name, string path )
        {
            Logger.WriteInfo( "Pfad wird honzugefuegt", "PathHandler", "AddPath" );

            using ( ConfigHandler con = new ConfigHandler( ) )
            {
                con.OpenConfigFile( "Paths" );

                Table.Add( name, path );

                return con.StoreData( name, path, true );
            }
        }

        /// <summary>
        /// Entfernt einen Pfad aus der Tabelle.
        /// </summary>
        /// <param name="name">Der Key des Pfads der entfernt werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
        public bool RemovePath( string name )
        {
            Logger.WriteInfo( "Pfad wird entfernt", "PathHandler", "RemovePath" );

            using( ConfigHandler con = new ConfigHandler() )
            {
                con.OpenConfigFile( "Paths" );

                Table.Remove( name );

                return con.RemoveData( name );
            }
        }

        /// <summary>
        /// Löscht alle Datein aus dem Temp Verzeichnis.
        /// </summary>
        public void ClearTempPath()
        {
#if DEBUG
            string temp = RetrievePath( "TempPathDebug" );
#else
            string temp = RetrievePath( "TempPathRelease" );
#endif
            try
            {
                foreach( string tmp in Directory.GetFiles( temp ) )
                {
                    File.Delete( tmp );
                }
            }

            catch( Exception e )
            {
                Logger.WriteWarning( "Temp Pfad konnte nicht geleert werden! Fehler: " + e.Message, "PathHandler", "ClearTempPath" );
            }
        }

        /// <summary>
        /// Liest die Pfade aus der Datei.
        /// </summary>
        private void ReadPaths()
        {
            string[ ] keys;
            string[ ] vals;

            using( ConfigHandler con = new ConfigHandler() )
            {
                con.OpenConfigFile( "Paths", true );

                ConfigData datak = con.LoadData( "keys" );
                ConfigData datav = con.LoadData( "vals" );

                if ( datak == null || datav == null )
                {
                    Logger.WriteWarning( "Konnte Pfade nicht lesen!", "PathHandler", "ReadPaths" );

                    return;
                }

                keys = datak.GetValuesAsString( );
                vals = datav.GetValuesAsString( );

                for( int i = 0; i < keys.Length; i++ )
                {
                    Table.Add( keys[ i ], vals[ i ] );

                    Logger.WriteInfo( "Ueberpruefe ob Verzeichnis \"" + vals[ i ] + "\" existiert.", "PathHandler", "ReadPaths" );

#if DEBUG
                    if ( keys[i].Contains( "Debug" ) && !Directory.Exists( vals[i] ) )
                    {
                        try
                        {
                            Logger.WriteWarning( "Verzeichnis \"" + vals[ i ] + "\" wird erstellt.", "PathHandler", "ReadPaths" );
                            Directory.CreateDirectory( vals[ i ] );
                        }

                        catch( Exception e )
                        {
                            Logger.WriteLog( "Verzeichnis \"" + vals[ i ] + "\" konnte nicht erstellt werden! Fehler: " + e.Message, 3, true, "PathHandler", "ReadPaths" );
                        }
                    }
#else
                    if ( keys[i].Contains( "Release" ) && !Directory.Exists( vals[i] ) )
                    {
                        try
                        {
                            Logger.WriteWarning( "Verzeichnis \"" + vals[ i ] + "\" wird erstellt.", "PathHandler", "ReadPaths" );
                            Directory.CreateDirectory( vals[ i ] );
                        }

                        catch( Exception e )
                        {
                            Logger.WriteLog( "Verzeichnis \"" + vals[ i ] + "\" konnte nicht erstellt werden! Fehler: " + e.Message, 3, true, "PathHandler", "ReadPaths" );
                        }
                    }
#endif
                    else
                    {
                        Logger.WriteInfo( "Verzeichnis \"" + vals[ i ] + "\" existiert!", "PathHandler", "ReadPaths" );
                    }
                }
            }
        }
    }
}
