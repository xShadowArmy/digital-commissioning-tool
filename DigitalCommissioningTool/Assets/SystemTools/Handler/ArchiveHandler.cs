using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using SystemTools.Handler;

namespace SystemTools.Handler
{
    public class ArchiveHandler
    {
        private LogHandler Logger;

        public ArchiveHandler()
        {
            Logger = new LogHandler( );
            Logger.WriteInfo( "ArchiveHandler wird initialisiert.", "ArchiveHandler", "ArchiveHandler" );
        }

        public void ArchiveDirectory( string src, string dst )
        {
            try
            {
                Logger.WriteInfo( "Archiviere " + src + " nach: " + dst + " ", "ArchiveHandler", "ArchiveDirectory" );
                ZipFile.CreateFromDirectory( src, dst );
            }

            catch( Exception e )
            {
                Logger.WriteLog( "Archive konnte nicht erstellt werden! Pfad: " + src + " ZielPfad: " + dst + " Error: " + e.Message, 2, false, "ArchiveHandler", "ArchiveDirectory" );
            }
        }

        public void ExtractArchive( string src, string dst )
        {
            try
            {
                Logger.WriteInfo( "Extrahiere " + src + " nach: " + dst + " ", "ArchiveHandler", "ExtractArchive" );
                ZipFile.ExtractToDirectory( src, dst );
            }

            catch ( Exception e )
            {
                Logger.WriteLog( "Archive konnte nicht extrahiert werden! Pfad: " + src + " ZielPfad: " + dst + " Error: " + e.Message, 2, false, "ArchiveHandler", "ExtractArchive" );
            }
        }
    }
}
