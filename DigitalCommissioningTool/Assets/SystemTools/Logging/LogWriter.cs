using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace SystemTools.Logging
{
    /// <summary>
    /// Erstellt und Schreibt LogDateien.
    /// </summary>
    internal class LogWriter
    {
        /// <summary>
        /// Text der in die LogDatei geschrieben werden soll.
        /// </summary>
        private static List<string> Buffer = null;

        /// <summary>
        /// Gibt an wie viele Objekte der Klasse vorhanden sind.
        /// </summary>
        private static long RefCnt = 0;

        /// <summary>
        /// Der Pfad, an dem die LogDateien erstellt werden.               
        /// </summary>
        private string LogPath { get; set; }

        /// <summary>
        /// Initialisiert den LogWriter und erstellt eine neue LogDatei mit Header.
        /// </summary>
        internal LogWriter()
        {
#if DEBUG
            LogPath = ".\\Output\\Logs\\";
#else
            LogPath = ".\\Logs\\";
#endif

            RefCnt += 1;

            if ( !Directory.Exists( LogPath ) )
            {
                Directory.CreateDirectory( LogPath );
            }

            LogPath += "Log_";
            LogPath += DateTime.Now.ToFileTime( );
            LogPath += ".log";

            if ( Buffer == null )
            {
                Buffer = new List<string>( );
            }
        }

        /// <summary>
        /// Schreibt den Puffer in die Datei.
        /// </summary>
        ~LogWriter()
        {
            RefCnt -= 1;
            Debug.Log( "LogRefCount: " + RefCnt  );

            if ( RefCnt == 0 )
            {
                Debug.Log( "FlushLogs" );
                PrintToFile( );
            }
        }

        /// <summary>
        /// Schreibt eine Info Nachricht in die LogDatei.
        /// </summary>
        /// <param name="message">Die Nachricht die geschrieben werden soll.</param>
        /// <param name="className">Der Name der Klasse für die etwas dokumentiert werden soll.</param>
        /// <param name="methodName">Der Name der Methode in der etwas dokumentiert werden soll.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht geöffnet und beschrieben werden kann.</exception>
        internal void WriteInfo( string message, string className, string methodName )
        {
            AddToBuffer( "[INFO] ", "[" + className + "][" + methodName + "] " + message );
        }

        /// <summary>
        /// Schreibt eine Warn Nachricht in die LogDatei.
        /// </summary>
        /// <param name="message">Die Nachricht die geschrieben werden soll.</param>
        /// <param name="className">Der Name der Klasse für die etwas dokumentiert werden soll.</param>
        /// <param name="methodName">Der Name der Methode in der etwas dokumentiert werden soll.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht geöffnet und beschrieben werden kann.</exception>
        internal void WriteWarning( string message, string className, string methodName )
        {
            AddToBuffer( "[WARN] ", "[" + className + "][" + methodName + "] " + message );
        }

        /// <summary>
        /// WSchreibt eine Error Nachricht in die LogDatei.
        /// </summary>
        /// <param name="message">Die Nachricht die geschrieben werden soll.</param>
        /// <param name="className">Der Name der Klasse für die etwas dokumentiert werden soll.</param>
        /// <param name="methodName">Der Name der Methode in der etwas dokumentiert werden soll.</param>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht geöffnet und beschrieben werden kann.</exception>
        internal void WriteError( string message, string className, string methodName )
        {
            AddToBuffer( "[ERROR] ", "[" + className + "][" + methodName + "] " + message );
        }

        /// <summary>
        /// Schreibt den Buffer in die LogDatei.
        /// </summary>
        /// <exception cref="IOException">Wird geworfen wenn die Datei nicht beschrieben werden kann.</exception>
        internal void PrintToFile()
        {
            try
            {
                using ( StreamWriter writer = (( !File.Exists( LogPath ) ) ? File.CreateText( LogPath ) : new StreamWriter( LogPath ) ) )
                {
                    if ( writer != null )
                    {
                        if ( writer.BaseStream != null )
                        {
                            try
                            {
                                PrintFileHeader( writer );

                                if ( Buffer.Count > 0 )
                                {
                                    foreach ( string s in Buffer )
                                    {
                                        writer.WriteLine( s );
                                    }
                                }

                                writer.Flush( );
                            }

                            catch ( Exception e )
                            {
                                throw new IOException( "Log Datei konnte nicht geschrieben werden Pfad: " + LogPath + " Fehler: " + e.Message );
                            }
                        }
                    }
                }

                Buffer.Clear( );
            }

            catch ( Exception e )
            {
                throw new IOException( "Log Datei konnte nicht geoeffnet und geschrieben werden Pfad: " + LogPath + " Fehler: " + e.Message );
            }
        }

        /// <summary>
        /// Schreibt die Daten in den Buffer.
        /// </summary>
        /// <param name="tag">Priorität der Nachricht.</param>
        /// <param name="message">Die Nachricht.</param>
        private void AddToBuffer( string tag, string message )
        {
            Buffer.Add( tag + message );
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
