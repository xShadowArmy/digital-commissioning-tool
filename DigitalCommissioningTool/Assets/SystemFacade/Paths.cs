using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTools.Handler;

namespace SystemFacade
{
    public static class Paths
    {
        public static string LogPath
        {
            get
            {
#if DEBUG
                return Handler.RetrievePath( "LogPathDebug" );
#else
                return Handler.RetrievePath( "LogPathRelease" );
#endif
            }
        }

        public static string StringResourcePath
        {
            get
            {
#if DEBUG
                return Handler.RetrievePath( "StringResourcePathDebug" );
#else
                return Handler.RetrievePath( "StringResourcePathRelease" );
#endif
            }
        }

        public static string ProjectsPath
        {
            get
            {
#if DEBUG
                return Handler.RetrievePath( "ProjectsPathDebug" );
#else
                return Handler.RetrievePath( "ProjectsPathRelease" );
#endif
            }
        }

        public static string DataResourcePath
        {
            get
            {
#if DEBUG
                return Handler.RetrievePath( "DataResourcePathDebug" );
#else
                return Handler.RetrievePath( "DataResourcePathRelease" );
#endif
            }
        }

        public static string ResourcePath
        {
            get
            {
#if DEBUG
                return Handler.RetrievePath( "ResourcePathDebug" );
#else
                return Handler.RetrievePath( "ResourcePathRelease" );
#endif
            }
        }

        public static string TempPath
        {
            get
            {
#if DEBUG
                return Handler.RetrievePath( "TempPathDebug" );
#else
                return Handler.RetrievePath( "TempPathRelease" );
#endif
            }
        }
        
        private static PathHandler Handler;
        
        static Paths()
        {
            Handler = new PathHandler( );
        }

        public static bool AddPath( string name, string path )
        {
            return Handler.AddPath( name, path );
        }

        public static string RetrievePath( string name )
        {
            return Handler.RetrievePath( name );
        }

        public static bool RemovePath( string name )
        {
            return Handler.RemovePath( name );
        }

        public static void ClearTempPath()
        {
            Handler.ClearTempPath( );
        }
    }
}
