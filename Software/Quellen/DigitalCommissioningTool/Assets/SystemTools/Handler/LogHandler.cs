using System;
using System.IO;
using SystemTools.Logging;

namespace SystemTools.Handler 
{
    /// <summary>
    /// Ermöglicht das Schreiben von LogDateien.
    /// </summary>
    public class LogHandler
    {
        /// <summary>
        /// Ermöglicht das Schreiben von LogDateien.
        /// </summary>
        private LogWriter Writer { get; set; }

        /// <summary>
        /// Initialisiert den LogHandler.
        /// </summary>
        public LogHandler()
        {
            Writer = new LogWriter( );
        }

        /// <summary>
        /// Schreibt eine Info Nachricht in die LogDatei.
        /// </summary>
        /// <param name="message">Die Nachricht die geschrieben werden soll.</param>
        /// <param name="className">Der Name der Klasse für die etwas dokumentiert werden soll.</param>
        /// <param name="methodName">Der Name der Methode in der etwas dokumentiert werden soll.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht geöffnet und beschrieben werden kann.</exception>
        public void WriteInfo( string message, string className = "", string methodName = "" )
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
        public void WriteWarning( string message, string className = "", string methodName = "" )
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
        public void WriteError( string message, string className = "", string methodName = "" )
        {
            Writer.WriteError( message, className, methodName );
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
        public void WriteLog( string msg, int lvl, bool throwException, string className = "", string methodName = "" )
        {
            switch ( lvl )
            {
                case 1:

                    WriteInfo( msg, className, methodName );
                    break;

                case 2:

                    WriteWarning( msg, className, methodName );
                    break;

                case 3:

                    WriteError( msg, className, methodName );
                    break;
            }

            if ( throwException )
            {
                throw new Exception( msg );
            }
        }

        public void Flush()
        {
            Writer.PrintToFile( );
        }
    }
}
