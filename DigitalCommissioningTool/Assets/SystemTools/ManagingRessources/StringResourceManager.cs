using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using SystemTools.Logging;

namespace SystemTools.ManagingResources
{
    /// <summary>
    /// Verwaltet String Resourcen und bietet Load and Store Architektur.
    /// </summary>
    public static class StringResourceManager
    {
        /// <summary>
        /// StringResourceReader Objekt mit dem die StringResourcen gelesen werden.
        /// </summary>
        private static StringResourceReader Reader { get; set; }

        /// <summary>
        /// StringResourceWriter Objekt mit dem die StringResourcen geschrieben werden.
        /// </summary>
        private static StringResourceWriter Writer { get; set; }
        
        /// <summary>
        /// Die Eingelesenen StringResource Daten.
        /// </summary>
        private static List<StringResourceReader.StringResourceData> StringResources { get; set; }
        
        /// <summary>
        /// Repräsentiert die StringResource Xml Datei.
        /// </summary>
        private static XmlDocument Doc { get; set; }

        /// <summary>
        /// Initialisiert den StringResourceManager und lädt die Strings in den Speicher.
        /// </summary>
        static StringResourceManager()
        {
            LogManager.WriteInfo( "Initialisierung des StringResourceManagers", "StringResourceManager", "StringResourceManager" );

            try
            {
                Doc = new XmlDocument( );

                string tmp, StringResDir;

                using ( ConfigManager man = new ConfigManager( ) )
                {
                    man.OpenConfigFile( "Paths", true );
#if DEBUG
                    tmp = man.LoadData( "StringResourcePathDebug" ).GetValueAsString( );
#else
                    tmp = man.LoadData( "StringResourcePathRelease" ).GetValueAsString( );
#endif
                    StringResDir = tmp + CultureInfo.InstalledUICulture.ThreeLetterISOLanguageName + ".xml";
                }

                try
                {
                    StringResources = new List<StringResourceReader.StringResourceData>( );
                    Doc.Load( StringResDir );
                }

                catch ( FileNotFoundException e )
                {
                    LogManager.WriteError( "StringResource Datei wurde nicht gefunden! Pfad: " + StringResDir + " Fehler: " + e.Message, "StringResourceManager", "StringResourceManager" );

                    Doc = null;
                }

                catch ( Exception e )
                {
                    LogManager.WriteError( "StringResource Datei konnte nicht geoffnet werden! Pfad: " + StringResDir + " Fehler: " + e.Message, "StringResourceManager", "StringResourceManager" );

                    Doc = null;
                }

                Reader = new StringResourceReader( StringResDir, Doc, CultureInfo.InstalledUICulture, StringResources );
                Writer = new StringResourceWriter( StringResDir, Doc, CultureInfo.InstalledUICulture );
            }

            catch( Exception e )
            {
                LogManager.WriteLog( "StringResourceManager konnte nicht initialisiert werden! Fehler: " + e.Message, LogLevel.Error, true, "StringResourceManager", "StringResourceManager" );
            }
        }
        
        /// <summary>
        /// Lädt eine StringResource anhand ihres Namens.
        /// </summary>
        /// <param name="name">Der Name der StringResource.</param>
        /// <returns>Die StringResource oder <see cref="string.Empty"> wenn die Resource nicht gefunden wurde</see>/></returns>
        public static string LoadString( string name )
        {
            return Reader.LoadString( name, StringResources );
        }

        /// <summary>
        /// Lädt eine StringResource anhand ihrer ID.
        /// </summary>
        /// <param name="id">Die ID der StringResource.</param>
        /// <returns>Die StringResource oder <see cref="string.Empty"> wenn die Resource nicht gefunden wurde</see>/></returns>
        public static string LoadString( long id )
        {
            return Reader.LoadString( id, StringResources );
        }

        /// <summary>
        /// Lädt eine StringResource anhand ihres Namens.
        /// </summary>
        /// <param name="name">Der Name der StringResource.</param>
        /// <returns>Die StringResource oder <see cref="string.Empty"> wenn die Resource nicht gefunden wurde</see>/></returns>
        public static string LoadString( this string str, string name )
        {
            return Reader.LoadString( name, StringResources);
        }

        /// <summary>
        /// Lädt eine StringResource anhand ihrer ID.
        /// </summary>
        /// <param name="id">Die ID der StringResource.</param>
        /// <returns>Die StringResource oder <see cref="string.Empty"> wenn die Resource nicht gefunden wurde</see>/></returns>
        public static string LoadString( this string str, long id )
        {
            return Reader.LoadString( id, StringResources );
        }

        /// <summary>
        /// Speichert eine StringResource mit dem angegebenen Schluessel.
        /// </summary>
        /// <param name="name">Der Schluessel der Resource.</param>
        /// <param name="content">Die StringResource.</param>
        /// <param name="overwrite">Gibt an, ob vorhandene Daten ueberschrieben werden sollen.</param>
        /// <returns>Gibt true zurueck, wenn Erfolgreich.</returns>
        public static bool StoreString( string name, string content, bool overwrite = false )
        {
            bool tmp = Writer.StoreString( name, content, overwrite, StringResources );

            if ( tmp )
            {
                WriteFile( );
            }

            return tmp;
        }
        
        /// <summary>
        /// Speichert eine StringResource mit dem angegebenen Schluessel.
        /// </summary>
        /// <param name="name">Der Schluessel der Resource.</param>
        /// <param name="content">Die StringResource.</param>
        /// <param name="overwrite">Gibt an, ob vorhandene Daten ueberschrieben werden sollen.</param>
        /// <returns>Gibt true zurueck, wenn Erfolgreich.</returns>
        public static bool StoreString( this string str, string name, string content, bool overwrite = false )
        {
            bool tmp = Writer.StoreString( name, content, overwrite, StringResources );

            if ( tmp )
            {
                WriteFile( );
            }

            return tmp;
        }
        
        /// <summary>
        /// Schreibt hinzugefügte StringResourcen in die Datei. Darf nicht nach jedem StoreData aufgerufen werden!
        /// </summary>
        public static void WriteFile()
        {
            Writer.WriteResourceFile( StringResources );
        }

        /// <summary>
        /// Überprüft ob eine StringResource mit der angegebenen ID verfügbar ist.
        /// </summary>
        /// <param name="id">Die ID der zu suchenden Resource.</param>
        /// <returns>Gibt true zurück falls die Suche erfolgreich war.</returns>
        public static bool Exists( long id )
        {
            return Reader.Exists( id, StringResources );
        } 

        /// <summary>
        /// Überprüft, ob eine StringResource mit dem angegebenen Namen verfügbar ist.
        /// </summary>
        /// <param name="name">Der Name der zu suchenden Resource.</param>
        /// <returns>Gibt true zurücl falls die Suche erfolgreich war.</returns>
        public static bool Exists( string name )
        {
            return Reader.Exists( name, StringResources );
        }

        /// <summary>
        /// Überprüft ob eine StringResource mit der angegebenen ID verfügbar ist.
        /// </summary>
        /// <param name="id">Die ID der zu suchenden Resource.</param>
        /// <returns>Gibt true zurück falls die Suche erfolgreich war.</returns>
        public static bool Exists( this string str, long id )
        {
            return Reader.Exists( id, StringResources );
        }

        /// <summary>
        /// Überprüft, ob eine StringResource mit dem angegebenen Namen verfügbar ist.
        /// </summary>
        /// <param name="name">Der Name der zu suchenden Resource.</param>
        /// <returns>Gibt true zurücl falls die Suche erfolgreich war.</returns>
        public static bool Exists( this string str, string name )
        {
            return Reader.Exists( name, StringResources );
        }
    }
}
