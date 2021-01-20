using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using SystemTools.Handler;
using UnityEngine;

namespace SystemFacade
{
    /// <summary>
    /// Verwaltet String Resourcen und bietet Load and Store Architektur.
    /// </summary>
    public static class StringResourceManager
    {
        /// <summary>
        /// Objekt das die Facade verdeckt.
        /// </summary>
        private static StringResourceHandler Handler;

        /// <summary>
        /// Initialisiert den StringRessourceHandler und lädt die Strings in den Speicher.
        /// </summary>
        static StringResourceManager( )
        {
            Handler = new StringResourceHandler( Application.systemLanguage );
        }

        /// <summary>
        /// Lädt eine StringResource anhand ihres Namens.
        /// </summary>
        /// <param name="name">Der Name der StringResource.</param>
        /// <returns>Die StringResource oder <see cref="string.Empty"> wenn die Resource nicht gefunden wurde</see>/></returns>
        public static string LoadString( string name )
        {
            return Handler.LoadString( name );
        }

        /// <summary>
        /// Lädt eine StringResource anhand ihrer ID.
        /// </summary>
        /// <param name="id">Die ID der StringResource.</param>
        /// <returns>Die StringResource oder <see cref="string.Empty"> wenn die Resource nicht gefunden wurde</see>/></returns>
        public static string LoadString( long id )
        {
            return Handler.LoadString( id );
        }

        /// <summary>
        /// Lädt eine StringResource anhand ihres Namens.
        /// </summary>
        /// <param name="name">Der Name der StringResource.</param>
        /// <returns>Die StringResource oder <see cref="string.Empty"> wenn die Resource nicht gefunden wurde</see>/></returns>
        public static string LoadString( this string content, string name )
        {
            return Handler.LoadString( name );
        }

        /// <summary>
        /// Speichert eine StringResource mit dem angegebenen Schluessel.
        /// </summary>
        /// <param name="name">Der Schluessel der Resource.</param>
        /// <param name="content">Die StringResource.</param>
        /// <param name="overwrite">Gibt an, ob vorhandene Daten ueberschrieben werden sollen.</param>
        /// <returns>Gibt true zurueck, wenn Erfolgreich.</returns>
        public static bool StoreString( this string content, string name, bool overwrite = false )
        {
            return Handler.StoreString( name, content, overwrite );
        }

        /// <summary>
        /// Schreibt hinzugefügte StringResourcen in die Datei. Darf nicht nach jedem StoreData aufgerufen werden!
        /// </summary>
        public static void WriteFile()
        {
            Handler.WriteFile( );
        }

        /// <summary>
        /// Überprüft ob eine StringResource mit der angegebenen ID verfügbar ist.
        /// </summary>
        /// <param name="id">Die ID der zu suchenden Resource.</param>
        /// <returns>Gibt true zurück falls die Suche erfolgreich war.</returns>
        public static bool Exists( long id )
        {
            return Handler.Exists( id );
        }

        /// <summary>
        /// Überprüft, ob eine StringResource mit dem angegebenen Namen verfügbar ist.
        /// </summary>
        /// <param name="name">Der Name der zu suchenden Resource.</param>
        /// <returns>Gibt true zurücl falls die Suche erfolgreich war.</returns>
        public static bool Exists( string name )
        {
            return Handler.Exists( name );
        }

        /// <summary>
        /// Wählt die passenden StringResourcen für die jeweilige Sprache aus.
        /// </summary>
        /// <param name="lang">Die Sprache die verwendet werden soll.</param>
        public static void SelectLanguage( SystemLanguage lang )
        {
            Handler = new StringResourceHandler( lang );
        }
    }
}
