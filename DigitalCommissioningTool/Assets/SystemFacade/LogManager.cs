using System;
using System.IO;
using SystemTools.Handler;

namespace SystemFacade
{
    /// <summary>
    /// Ermöglicht das Schreiben von LogDateien.
    /// </summary>
    public static class LogManager
    {
        /// <summary>
        /// Objekt das die Facade überdeckt.
        /// </summary>
        private static LogHandler Handler;

        /// <summary>
        /// Initialisiert den LogHandler.
        /// </summary>
        static LogManager()
        {
            Handler = new LogHandler( );
        }

        /// <summary>
        /// Schreibt eine Info Nachricht in die LogDatei.
        /// </summary>
        /// <param name="message">Die Nachricht die geschrieben werden soll.</param>
        /// <param name="className">Der Name der Klasse für die etwas dokumentiert werden soll.</param>
        /// <param name="methodName">Der Name der Methode in der etwas dokumentiert werden soll.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht geöffnet und beschrieben werden kann.</exception>
        public static void WriteInfo( string message, string className = "", string methodName = "" )
        {
            Handler.WriteInfo( message, className, methodName );
        }

        /// <summary>
        /// Schreibt eine Warn Nachricht in die LogDatei.
        /// </summary>
        /// <param name="message">Die Nachricht die geschrieben werden soll.</param>
        /// <param name="className">Der Name der Klasse für die etwas dokumentiert werden soll.</param>
        /// <param name="methodName">Der Name der Methode in der etwas dokumentiert werden soll.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht geöffnet und beschrieben werden kann.</exception>
        public static void WriteWarning( string message, string className = "", string methodName = "" )
        {
            Handler.WriteWarning( message, className, methodName );
        }

        /// <summary>
        /// Schreibt eine Error Nachricht in die LogDatei.
        /// </summary>
        /// <param name="message">Die Nachricht die geschrieben werden soll.</param>
        /// <param name="className">Der Name der Klasse für die etwas dokumentiert werden soll.</param>
        /// <param name="methodName">Der Name der Methode in der etwas dokumentiert werden soll.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht geöffnet und beschrieben werden kann.</exception>
        public static void WriteError( string message, string className = "", string methodName = "" )
        {
            Handler.WriteError( message, className, methodName );
        }

        /// <summary>
        /// Schreibt eine Log Nachricht und bietet die Möglichkeit, gleichzeitig eine Exception zu werfen.
        /// </summary>
        /// <param name="msg">Die Nachricht die geloggt werden soll.</param>
        /// <param name="lvl">Die Priorität der Nachricht.</param>
        /// <param name="throwException">Gibt an ob mit der angegebenen Nachricht eine Exception geworfen werden soll.</param>
        /// <param name="className">Name der Klasse die eine Lognachricht schreibt.</param>
        /// <param name="methodName">Name der MEthode die eine Lognachricht schreibt.</param>
        /// <exception cref="Exception"></exception>
        public static void WriteLog( string msg, LogLevel lvl, bool throwException, string className = "", string methodName = "" )
        {
            Handler.WriteLog( msg, (int)lvl, throwException, className, methodName );
        }
    }
}
