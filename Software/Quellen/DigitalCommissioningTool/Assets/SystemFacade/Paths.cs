using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTools.Handler;

namespace SystemFacade
{
    /// <summary>
    /// Facade für die vom Programm verwendeten Pfade.
    /// </summary>
    public static class Paths
    {
        /// <summary>
        /// Pfad in dem die LogDateien liegen.
        /// </summary>
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

        /// <summary>
        /// Pfad in dem die String Ressourcen liegen.
        /// </summary>
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

        /// <summary>
        /// Pfad in dem die Projekte gespeichert werden.
        /// </summary>
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

        /// <summary>
        /// Pfad in dem die Konfigurations Dateien gespeichert werden.
        /// </summary>
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

        /// <summary>
        /// Pfad in dem die Xml Schemata liegen.
        /// </summary>
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

        /// <summary>
        /// Pfad in dem temporäre Dateien für die Projekte liegen.
        /// </summary>
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
        
        /// <summary>
        /// Helper Objekt das von der Facade überdeckt wird.
        /// </summary>
        private static PathHandler Handler;
        
        /// <summary>
        /// Initialisiert das helper Objekt.
        /// </summary>
        static Paths()
        {
            Handler = new PathHandler( );
        }

        /// <summary>
        /// Fügt einen neuen Pfad hinzu.
        /// </summary>
        /// <param name="name">Der Schlüssel des neuen Pfads.</param>
        /// <param name="path">Der Pfad.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
        public static bool AddPath( string name, string path )
        {
            return Handler.AddPath( name, path );
        }

        /// <summary>
        /// Frägt einen vorhandenen Pfad ab.
        /// </summary>
        /// <param name="name">Der Schlüssel des Pfads.</param>
        /// <returns>Gibt den Pfad zurück wenn Erfolgreich oder einen leeren String.</returns>
        public static string RetrievePath( string name )
        {
            return Handler.RetrievePath( name );
        }

        /// <summary>
        /// Entfernt einen vorhandenen Pfad.
        /// </summary>
        /// <param name="name">Der Schlüssel des Pfads der entfernt werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
        public static bool RemovePath( string name )
        {
            return Handler.RemovePath( name );
        }

        /// <summary>
        /// Löscht alle Dateien aus dem Temp verzeichnis.
        /// </summary>
        public static void ClearTempPath()
        {
            Handler.ClearTempPath( );
        }
    }
}
