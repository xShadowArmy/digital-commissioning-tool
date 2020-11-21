using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SystemTools.Handler
{
    public class PathHandler
    {
        private Dictionary<string,string> Table;
        private LogHandler Logger;

        public PathHandler()
        {
            Logger = new LogHandler( );

            Logger.WriteInfo( "PathHandler wird initialisiert", "PathHandler", "PathHandler" );

            Table  = new Dictionary<string, string>( );

            ReadPaths( );
        }

        public string RetrievePath( string name )
        {
            Logger.WriteInfo( "Pfad \"" + name + "\" wird gesucht", "PathHandler", "RetrievePath" );

            foreach ( KeyValuePair<string,string> tmp in Table )
            {
                if ( name.Equals( tmp.Key ) )
                {
                    return tmp.Value;
                }
            }

            return string.Empty;
        }

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
