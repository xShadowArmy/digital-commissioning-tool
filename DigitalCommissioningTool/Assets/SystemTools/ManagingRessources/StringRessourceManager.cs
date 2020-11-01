using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using SystemTools.Logging;

namespace SystemTools.ManagingRessources
{
    /// <summary>
    /// Verwaltet String Ressourcen und bietet Load and Store Architektur.
    /// </summary>
    public static class StringRessourceManager
    {
        /// <summary>
        /// StringRessourceReader Objekt mit dem die StringRessourcen gelesen werden.
        /// </summary>
        private static StringRessourceReader Reader { get; set; }

        /// <summary>
        /// StringRessourceWriter Objekt mit dem die StringRessourcen geschrieben werden.
        /// </summary>
        private static StringRessourceWriter Writer { get; set; }
        
        /// <summary>
        /// Die Eingelesenen StringRessource Daten.
        /// </summary>
        private static List<StringRessourceReader.StringRessourceData> StringRessources { get; set; }
        
        /// <summary>
        /// Repräsentiert die StringRessource Xml Datei.
        /// </summary>
        private static XmlDocument Doc { get; set; }

        /// <summary>
        /// Initialisiert den StringRessourceManager und lädt die Strings in den Speicher.
        /// </summary>
        static StringRessourceManager()
        {
            LogManager.WriteInfo( "Initialisierung des StringRessourceManagers", "StringRessourceManager", "StringRessourceManager" );

            ConfigManager man = new ConfigManager( );
            Doc = new XmlDocument( );

            man.OpenConfigFile( "Paths", true );

#if DEBUG
            string tmp = man.LoadData( "StringRessourcePathDebug" ).GetValueAsString( );
#else
            string tmp = man.LoadData( "StringRessourcePathRelease" ).GetValueAsString( );
#endif

            string StringResDir = tmp + CultureInfo.InstalledUICulture.ThreeLetterISOLanguageName + ".xml";
            
            try
            {
                StringRessources = new List<StringRessourceReader.StringRessourceData>( );
                Doc.Load( StringResDir );
            }

            catch ( FileNotFoundException e )
            {
                LogManager.WriteError( "StringRessource Datei wurde nicht gefunden! Pfad: " + StringResDir + " Fehler: " + e.Message, "StringRessourceManager", "StringRessourceManager" );

                Doc = null;
            }

            catch ( Exception e )
            {
                LogManager.WriteError( "StringRessource Datei konnte nicht geoffnet werden! Pfad: " + StringResDir + " Fehler: " + e.Message, "StringRessourceManager", "StringRessourceManager" );

                Doc = null;
            }

            Reader = new StringRessourceReader( StringResDir, Doc, CultureInfo.InstalledUICulture, StringRessources );
            Writer = new StringRessourceWriter( StringResDir, Doc, CultureInfo.InstalledUICulture );
        }
        
        /// <summary>
        /// Lädt eine StringRessource anhand ihres Namens.
        /// </summary>
        /// <param name="name">Der Name der StringRessource.</param>
        /// <returns>Die StringRessource oder <see cref="string.Empty"> wenn die Ressource nicht gefunden wurde</see>/></returns>
        public static string LoadString( string name )
        {
            return Reader.LoadString( name, StringRessources );
        }

        /// <summary>
        /// Lädt eine StringRessource anhand ihrer ID.
        /// </summary>
        /// <param name="id">Die ID der StringRessource.</param>
        /// <returns>Die StringRessource oder <see cref="string.Empty"> wenn die Ressource nicht gefunden wurde</see>/></returns>
        public static string LoadString( long id )
        {
            return Reader.LoadString( id, StringRessources );
        }

        /// <summary>
        /// Lädt eine StringRessource anhand ihres Namens.
        /// </summary>
        /// <param name="name">Der Name der StringRessource.</param>
        /// <returns>Die StringRessource oder <see cref="string.Empty"> wenn die Ressource nicht gefunden wurde</see>/></returns>
        public static string LoadString( this string str, string name )
        {
            return Reader.LoadString( name, StringRessources);
        }

        /// <summary>
        /// Lädt eine StringRessource anhand ihrer ID.
        /// </summary>
        /// <param name="id">Die ID der StringRessource.</param>
        /// <returns>Die StringRessource oder <see cref="string.Empty"> wenn die Ressource nicht gefunden wurde</see>/></returns>
        public static string LoadString( this string str, long id )
        {
            return Reader.LoadString( id, StringRessources );
        }

        /// <summary>
        /// Speichert eine StringRessource mit dem angegebenen Schluessel.
        /// </summary>
        /// <param name="name">Der Schluessel der Ressource.</param>
        /// <param name="content">Die StringRessource.</param>
        /// <param name="overwrite">Gibt an, ob vorhandene Daten ueberschrieben werden sollen.</param>
        /// <returns>Gibt true zurueck, wenn Erfolgreich.</returns>
        public static bool StoreString( string name, string content, bool overwrite = false )
        {
            return Writer.StoreString( name, content, overwrite, StringRessources );
        }
        
        /// <summary>
        /// Speichert eine StringRessource mit dem angegebenen Schluessel.
        /// </summary>
        /// <param name="name">Der Schluessel der Ressource.</param>
        /// <param name="content">Die StringRessource.</param>
        /// <param name="overwrite">Gibt an, ob vorhandene Daten ueberschrieben werden sollen.</param>
        /// <returns>Gibt true zurueck, wenn Erfolgreich.</returns>
        public static bool StoreString( this string str, string name, string content, bool overwrite = false )
        {
            return Writer.StoreString( name, content, overwrite, StringRessources );
        }
        
        /// <summary>
        /// Schreibt hinzugefügte StringRessourcen in die Datei. Darf nicht nach jedem StoreData aufgerufen werden!
        /// </summary>
        public static void WriteFile()
        {
            Writer.WriteRessourceFile( StringRessources );
        }

        /// <summary>
        /// Überprüft ob eine StringRessource mit der angegebenen ID verfügbar ist.
        /// </summary>
        /// <param name="id">Die ID der zu suchenden Ressource.</param>
        /// <returns>Gibt true zurück falls die Suche erfolgreich war.</returns>
        public static bool Exists( long id )
        {
            return Reader.Exists( id, StringRessources );
        } 

        /// <summary>
        /// Überprüft, ob eine StringRessource mit dem angegebenen Namen verfügbar ist.
        /// </summary>
        /// <param name="name">Der Name der zu suchenden Ressource.</param>
        /// <returns>Gibt true zurücl falls die Suche erfolgreich war.</returns>
        public static bool Exists( string name )
        {
            return Reader.Exists( name, StringRessources );
        }

        /// <summary>
        /// Überprüft ob eine StringRessource mit der angegebenen ID verfügbar ist.
        /// </summary>
        /// <param name="id">Die ID der zu suchenden Ressource.</param>
        /// <returns>Gibt true zurück falls die Suche erfolgreich war.</returns>
        public static bool Exists( this string str, long id )
        {
            return Reader.Exists( id, StringRessources );
        }

        /// <summary>
        /// Überprüft, ob eine StringRessource mit dem angegebenen Namen verfügbar ist.
        /// </summary>
        /// <param name="name">Der Name der zu suchenden Ressource.</param>
        /// <returns>Gibt true zurücl falls die Suche erfolgreich war.</returns>
        public static bool Exists( this string str, string name )
        {
            return Reader.Exists( name, StringRessources );
        }
    }
}
