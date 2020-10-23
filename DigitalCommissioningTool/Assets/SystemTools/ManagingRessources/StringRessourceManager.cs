using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTools.Logging;

namespace SystemTools.ManagingRessources
{
    /// <summary>
    /// Verwaltet String Ressourcen und bietet Load and Store Architektur.
    /// </summary>
    public static class StringRessourceManager
    {
        private static string StringResDir { get; set; }
        private static CultureInfo LangInfo { get; set; }
        private static StringRessourceReader Reader { get; set; }
        private static StringRessourceWriter Writer { get; set; }

        /// <summary>
        /// Initialisiert den StringRessourceManager und lädt die Strings in den Speicher.
        /// </summary>
        static StringRessourceManager()
        {
            LogManager.WriteInfo( "Initialisierung des StringRessourceManagers", "StringRessourceManager", "StringRessourceManager" );

            // Lesen der aktuellen Systemsprache.
            LangInfo = CultureInfo.InstalledUICulture;

            StringResDir = "Output\\Ressources\\Strings\\" + LangInfo.ThreeLetterISOLanguageName + ".xml";

            Reader = new StringRessourceReader( StringResDir, LangInfo );
            Writer = new StringRessourceWriter( StringResDir, LangInfo );

            // Einmaliges einlesen der String zu Programmstart.
            ReadStringRessources( );
        }
        
        /// <summary>
        /// Lädt eine StringRessource anhand ihres Namens.
        /// </summary>
        /// <param name="name">Der Name der StringRessource.</param>
        /// <returns>Die StringRessource oder <see cref="string.Empty"> wenn die Ressource nicht gefunden wurde</see>/></returns>
        public static string LoadString( string name )
        {
            return Reader.LoadString( name );
        }

        /// <summary>
        /// Lädt eine StringRessource anhand ihrer ID.
        /// </summary>
        /// <param name="id">Die ID der StringRessource.</param>
        /// <returns>Die StringRessource oder <see cref="string.Empty"> wenn die Ressource nicht gefunden wurde</see>/></returns>
        public static string LoadString( long id )
        {
            return Reader.LoadString( id );
        }

        /// <summary>
        /// Lädt eine StringRessource anhand ihres Namens.
        /// </summary>
        /// <param name="name">Der Name der StringRessource.</param>
        /// <returns>Die StringRessource oder <see cref="string.Empty"> wenn die Ressource nicht gefunden wurde</see>/></returns>
        public static string LoadString( this string str, string name )
        {
            return Reader.LoadString( name );
        }

        /// <summary>
        /// Lädt eine StringRessource anhand ihrer ID.
        /// </summary>
        /// <param name="id">Die ID der StringRessource.</param>
        /// <returns>Die StringRessource oder <see cref="string.Empty"> wenn die Ressource nicht gefunden wurde</see>/></returns>
        public static string LoadString( this string str, long id )
        {
            return Reader.LoadString( id );
        }

        /// <summary>
        /// Noch nicht implementiert.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static bool StoreString( string name, string content, bool overwrite = false )
        {
            return false;
        }

        /// <summary>
        /// Noch nicht implementiert.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static bool StoreString( long id, string content, bool overwrite = false )
        {
            return false;
        }

        /// <summary>
        /// Überprüft ob eine StringRessource mit der angegebenen ID verfügbar ist.
        /// </summary>
        /// <param name="id">Die ID der zu suchenden Ressource.</param>
        /// <returns>Gibt true zurück falls die Suche erfolgreich war.</returns>
        public static bool Exists( long id )
        {
            return Reader.Exists( id );
        } 

        /// <summary>
        /// Überprüft, ob eine StringRessource mit dem angegebenen Namen verfügbar ist.
        /// </summary>
        /// <param name="name">Der Name der zu suchenden Ressource.</param>
        /// <returns>Gibt true zurücl falls die Suche erfolgreich war.</returns>
        public static bool Exists( string name )
        {
            return Reader.Exists( name );
        }

        /// <summary>
        /// Überprüft ob eine StringRessource mit der angegebenen ID verfügbar ist.
        /// </summary>
        /// <param name="id">Die ID der zu suchenden Ressource.</param>
        /// <returns>Gibt true zurück falls die Suche erfolgreich war.</returns>
        public static bool Exists( this string str, long id )
        {
            return Reader.Exists( id );
        }

        /// <summary>
        /// Überprüft, ob eine StringRessource mit dem angegebenen Namen verfügbar ist.
        /// </summary>
        /// <param name="name">Der Name der zu suchenden Ressource.</param>
        /// <returns>Gibt true zurücl falls die Suche erfolgreich war.</returns>
        public static bool Exists( this string str, string name )
        {
            return Reader.Exists( name );
        }

        /// <summary>
        /// Lädt die StringRessourcen in den Speicher.
        /// </summary>
        private static void ReadStringRessources()
        {
            Reader.ReadStringRessources( );
        }

        /// <summary>
        /// Schreibt die StringRessourcen in die passende Ressourcendatei.
        /// </summary>
        private static void WriteStringRessource()
        {

        }
    }
}
