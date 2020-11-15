using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTools.Handler;

namespace SystemFacade
{
    public static class ArchiveManager
    {
        private static ArchiveHandler Handler;

        static ArchiveManager()
        {
            Handler = new ArchiveHandler( );
        }

        public static void ArchiveDirectory( string src, string dst )
        {
            Handler.ArchiveDirectory( src, dst );
        }

        public static void ExtractArchive( string src, string dst )
        {
            Handler.ExtractArchive( src, dst ); 
        }
    }
}
