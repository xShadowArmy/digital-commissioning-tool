using System;
using System.IO;
using System.Text;

namespace SystemTools.Logging
{
    /// <summary>
    /// Erstellt und Schreibt LogDateien.
    /// </summary>
    internal class LogWriter
    {
        /// <summary>
        /// Der Pfad, an dem die LogDateien erstellt werden.               
        /// </summary>
        internal string LogPath { get; private set; }

        /// <summary>
        /// Initialisiert den LogWriter und erstellt eine neue LogDatei mit Header.
        /// </summary>
        internal LogWriter()
        {
            StringBuilder builder = new StringBuilder( string.Empty );

            builder.Append( "Output" );
            builder.Append( "\\Logs" );
            
            if ( !Directory.Exists( builder.ToString( ) ) )
            {
                Directory.CreateDirectory( builder.ToString() );
            }

            builder.Append( "\\Log_" );
            builder.Append( DateTime.Now.ToFileTime( ) );
            builder.Append( ".log" );

            LogPath = builder.ToString( );

            if ( !File.Exists( LogPath ) )
            {
                try
                {
                    using ( StreamWriter writer = File.CreateText( LogPath ) )
                    {
                        PrintFileHeader( writer );
                    }
                }

                catch ( Exception e )
                {
                    throw new Exception( "Log Datei konnte nicht erstellt werden in Pfad: " + LogPath + " Fehler: " + e.Message );
                }
            }
        }

        /// <summary>
        /// Schreibt eine Info Nachricht in die LogDatei.
        /// </summary>
        /// <param name="message">Die Nachricht die geschrieben werden soll.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht geöffnet und beschrieben werden kann.</exception>
        public void WriteInfo( string message, string className, string methodName )
        {
            PrintToFile( "[INFO] ", "[" + className + "][" + methodName + "] " + message );
        }

        /// <summary>
        /// Schreibt eine Warn Nachricht in die LogDatei.
        /// </summary>
        /// <param name="message">Die Nachricht die geschrieben werden soll.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht geöffnet und beschrieben werden kann.</exception>
        public void WriteWarning( string message, string className, string methodName )
        {
            PrintToFile( "[WARN] ", "[" + className + "][" + methodName + "] " + message );
        }

        /// <summary>
        /// WSchreibt eine Error Nachricht in die LogDatei.
        /// </summary>
        /// <param name="message">Die Nachricht die geschrieben werden soll.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht geöffnet und beschrieben werden kann.</exception>
        public void WriteError( string message, string className, string methodName )
        {
            PrintToFile( "[ERROR] ", "[" + className + "][" + methodName + "] " + message );
        }
        
        /// <summary>
        /// Schreibt die Daten in die LogDatei.
        /// </summary>
        /// <param name="tag">Priorität der Nachricht.</param>
        /// <param name="message">Die Nachricht.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht beschrieben werden kann.</exception>
        private void PrintToFile( string tag, string message )
        {
            try
            {
                using ( StreamWriter writer = new StreamWriter( LogPath, true ) )
                {
                    if ( writer != null )
                    {
                        if ( writer.BaseStream != null )
                        {
                            try
                            {
                                writer.WriteLine( tag + message );

                                writer.Flush( );
                            }

                            catch ( Exception e )
                            {
                                throw new IOException( "Log Datei konnte nicht geschrieben werden Pfad: " + LogPath + " Fehler: " + e.Message );
                            }
                        }
                    }
                }
            }

            catch ( Exception e )
            {
                throw new IOException( "Log Datei konnte nicht geoeffnet und geschrieben werden Pfad: " + LogPath + " Fehler: " + e.Message );
            }
        }

        /// <summary>
        /// Schreibt den Header in die LogDatei.
        /// </summary>
        /// <param name="writer">StreamWriter objekt mit verfügbarem FileStream.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht beschrieben werden kann.</exception>
        private void PrintFileHeader( StreamWriter writer )
        {
            try
            {
                writer.WriteLine( "--------------------------------------------------------------------" );
                writer.WriteLine( "- Automatisch generierte Log Datei." );
                writer.WriteLine( "- " + DateTime.Now.ToShortDateString( ) + " " + DateTime.Now.ToLongTimeString( ) );
                writer.WriteLine( "--------------------------------------------------------------------" + Environment.NewLine );

                writer.Flush( );
            }

            catch ( Exception e )
            {
                throw new IOException("Der Header der Log Datei konnte nicht in die Datei geschrieben werden: " + LogPath + " Fehler: " + e.Message );
            }
        }
    }
}
