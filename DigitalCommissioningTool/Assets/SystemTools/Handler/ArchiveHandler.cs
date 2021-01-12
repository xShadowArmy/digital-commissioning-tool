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
    /// <summary>
    /// Helper Methoden für das Archivieren und Entpacken von .Zip Verzeichnissen.
    /// </summary>
    public class ArchiveHandler
    {
        /// <summary>
        /// Objekt zum Schreiben von Log Dateien.
        /// </summary>
        private LogHandler Logger;

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        public ArchiveHandler()
        {
            Logger = new LogHandler( );
            Logger.WriteInfo( "ArchiveHandler wird initialisiert.", "ArchiveHandler", "ArchiveHandler" );
        }

        /// <summary>
        /// Archiviert alle Dateien eines Pfads.
        /// </summary>
        /// <param name="src">Der Pfad der Archiviert werden soll.</param>
        /// <param name="dst">Der Zielpfad unter dem das Archiv gespeichert werden soll.</param>
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

        /// <summary>
        /// Entpackt ein Archiv.
        /// </summary>
        /// <param name="src">Der Pfad des Archivs.</param>
        /// <param name="dst">Der Pfad in dem das Archiv entpackt werden soll.</param>
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
