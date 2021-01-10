using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTools.Handler;

namespace SystemFacade
{
    /// <summary>
    /// Facade für Archiv Operationen.
    /// </summary>
    public static class ArchiveManager
    {
        /// <summary>
        /// Objekt das überdeckt wird.
        /// </summary>
        private static ArchiveHandler Handler;

        /// <summary>
        /// Initialisiert die Facade.
        /// </summary>
        static ArchiveManager()
        {
            Handler = new ArchiveHandler( );
        }

        /// <summary>
        /// ARchiviert ein Verzeichnis.
        /// </summary>
        /// <param name="src">Der Pfad des Verzeichnisses das Archiviert werden soll.</param>
        /// <param name="dst">Pfad unter dem das Archiv gespeichert werden soll.</param>
        public static void ArchiveDirectory( string src, string dst )
        {
            Handler.ArchiveDirectory( src, dst );
        }

        /// <summary>
        /// Entpackt ein Archiv.
        /// </summary>
        /// <param name="src">Der Pfad des Archivs.</param>
        /// <param name="dst">Der Pfad unter dem das Archiv entpackt werden soll.</param>
        public static void ExtractArchive( string src, string dst )
        {
            Handler.ExtractArchive( src, dst ); 
        }
    }
}
