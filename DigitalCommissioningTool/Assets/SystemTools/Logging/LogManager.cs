using System.IO;

namespace SystemTools.Logging
{
    /// <summary>
    /// Ermöglicht das Schreiben von LogDateien.
    /// </summary>
    public static class LogManager
    {
        /// <summary>
        /// Ermöglicht das Schreiben von LogDateien.
        /// </summary>
        private static LogWriter Writer { get; set; }

        /// <summary>
        /// Initialisiert den LogManager.
        /// </summary>
        static LogManager()
        {
            Writer  = new LogWriter( );
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
#if DEBUG
            Writer.WriteInfo( message, className, methodName );
#endif
        }

        /// <summary>
        /// Schreibt eine Warn Nachricht in die LogDatei.
        /// </summary>
        /// <param name="message">Die Nachricht die geschrieben werden soll.</param>
        /// <param name="className">Der Name der Klasse für die etwas dokumentiert werden soll.</param>
        /// <param name="methodName">Der Name der Methode in der etwas dokumentiert werden soll.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht geöffnet und beschrieben werden kann.</exception>
        public static void WriteWarning( string message, string className, string methodName )
        {
            Writer.WriteWarning( message, className, methodName );
        }

        /// <summary>
        /// Schreibt eine Error Nachricht in die LogDatei.
        /// </summary>
        /// <param name="message">Die Nachricht die geschrieben werden soll.</param>
        /// <param name="className">Der Name der Klasse für die etwas dokumentiert werden soll.</param>
        /// <param name="methodName">Der Name der Methode in der etwas dokumentiert werden soll.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht geöffnet und beschrieben werden kann.</exception>
        public static void WriteError( string message, string className, string methodName )
        {
            Writer.WriteError( message, className, methodName );
        }
    }
}
