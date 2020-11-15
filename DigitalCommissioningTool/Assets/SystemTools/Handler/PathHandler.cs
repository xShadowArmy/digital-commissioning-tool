using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                }
            }
        }
    }
}
