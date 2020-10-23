using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SystemTools.Logging
{
    /// <summary>
    /// Used to create and write Log files.
    /// </summary>
    public static class LogManager
    {
        public static string LogPath { get; private set; }

        /// <summary>
        /// Initializes the log manager.
        /// </summary>
        static LogManager()
        {
            StringBuilder builder = new StringBuilder( string.Empty );
            
            builder.Append( "Output" );
            builder.Append( "\\Logs" );
            
            if ( !Directory.Exists( builder.ToString() ) )
            {
                throw new DirectoryNotFoundException( "Unable to locate log directory in: " + builder.ToString() );
            }

            builder.Append( "\\Log_" );            
            builder.Append( DateTime.Now.ToFileTime() );
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

                catch( Exception e )
                {
                    throw new Exception( "Unable to create log file in: " + LogPath + " Error: " + e.Message );
                }
            }
        }

        /// <summary>
        /// Writes an info message into the log file while in DEBUG mode.
        /// </summary>
        /// <param name="message">The info message which should be written.</param>
        /// <exception cref="IOException">Will be thrown if the system is unable to open and write into the log file.</exception>
        public static void WriteInfo( string message, string className, string methodName )
        {
#if DEBUG

            PrintToFile( "[Info]  ", message );
#endif
        }

        /// <summary>
        /// Writes a warning message into the log file.
        /// </summary>
        /// <param name="message">The warning message which should be written.</param>
        /// <exception cref="IOException">Will be thrown if the system is unable to open and write into the log file.</exception>
        public static void WriteWarning( string message, string classeName, string methodName )
        {
            PrintToFile( "[WARN]  ", message );
        }

        /// <summary>
        /// Writes an error message into the log file.
        /// </summary>
        /// <param name="message">The error message which should be written.</param>
        /// <exception cref="IOException">Will be thrown if the system is unable to open and write into the log file.</exception>
        public static void WriteError( string message, string className, string methodName )
        {
            PrintToFile( "[ERROR] ", message );
        }

        /// <summary>
        /// Prints available data into the log file.
        /// </summary>
        /// <param name="tag">Message priority.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="IOException">Will be thrown if the system is unable to open and write into the log file.</exception>
        private static void PrintToFile( string tag, string message )
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

                            catch( Exception e )
                            {
                                throw new IOException( "Couldn't writer into log file at: " + LogPath + " Error: " + e.Message );
                            }
                        }
                    }
                }
            }

            catch( Exception e )
            {
                throw new IOException( "Unable to open and write log file in: " + LogPath + " Error: " + e.Message );
            }
        }

        /// <summary>
        /// Prints the log file header.
        /// </summary>
        /// <param name="writer">StreamWriter object with open filestream.</param>
        /// <exception cref="IOException">Will be thrown if the system is unable to open and write into the log file.</exception>
        private static void PrintFileHeader( StreamWriter writer )
        {
            try
            {
                writer.WriteLine( "--------------------------------------------------------------------" );
                writer.WriteLine( "- Automatically generated Log file" );
                writer.WriteLine( "- " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() );
                writer.WriteLine( "--------------------------------------------------------------------" + Environment.NewLine );

                writer.Flush( );
            }

            catch ( Exception e )
            {
                throw new IOException( "COuldn't writer file header into log file at: " + LogPath + " Error: " + e.Message );
            }
        }
    }
}
