using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using SystemTools.ManagingResources;
using UnityEngine;

namespace SystemTools.Handler
{
    /// <summary>
    /// Verwaltet String Resourcen und bietet Load and Store Architektur.
    /// </summary>
    public class StringResourceHandler
    {
        /// <summary>
        /// StringResourceReader Objekt mit dem die StringResourcen gelesen werden.
        /// </summary>
        private StringResourceReader Reader { get; set; }

        /// <summary>
        /// StringResourceWriter Objekt mit dem die StringResourcen geschrieben werden.
        /// </summary>
        private StringResourceWriter Writer { get; set; }
        
        /// <summary>
        /// Die Eingelesenen StringResource Daten.
        /// </summary>
        private List<StringResourceReader.StringResourceData> StringResources { get; set; }
        
        /// <summary>
        /// Repräsentiert die StringResource Xml Datei.
        /// </summary>
        private XmlDocument Doc { get; set; }

        /// <summary>
        /// Wird fuer das Schreiben von LogDateien verwendet.
        /// </summary>
        private LogHandler Logger;

        /// <summary>
        /// Initialisiert den StringRessourceHandler und lädt die Strings in den Speicher.
        /// </summary>
        public StringResourceHandler()
        {
            Logger = new LogHandler( );

            Logger.WriteInfo( "Initialisierung des StringResourceManagers", "StringRessourceHandler", "StringRessourceHandler" );

            try
            {
                Doc = new XmlDocument( );

                string tmp, StringResDir;
#if DEBUG
                tmp = new PathHandler().RetrievePath( "StringResourcePathDebug" );
#else
                tmp = new PathHandler().RetrievePath( "StringResourcePathRelease" );
#endif
                StringResDir = tmp + CultureInfo.InstalledUICulture.ThreeLetterISOLanguageName + ".xml";
                
                try
                {
                    StringResources = new List<StringResourceReader.StringResourceData>( );
                    Doc.Load( StringResDir );
                }

                catch ( FileNotFoundException e )
                {
                    Logger.WriteError( "StringResource Datei wurde nicht gefunden! Pfad: " + StringResDir + " Fehler: " + e.Message, "StringRessourceHandler", "StringRessourceHandler" );

                    Doc = null;
                }

                catch ( Exception e )
                {
                    Logger.WriteError( "StringResource Datei konnte nicht geoffnet werden! Pfad: " + StringResDir + " Fehler: " + e.Message, "StringRessourceHandler", "StringRessourceHandler" );

                    Doc = null;
                }

                Reader = new StringResourceReader( StringResDir, Doc, CultureInfo.InstalledUICulture, StringResources );
                Writer = new StringResourceWriter( StringResDir, Doc, CultureInfo.InstalledUICulture );
            }

            catch( Exception e )
            {
                Logger.WriteLog( "StringRessourceHandler konnte nicht initialisiert werden! Fehler: " + e.Message, 3, true, "StringRessourceHandler", "StringRessourceHandler" );
            }
        }
        
        /// <summary>
        /// Lädt eine StringResource anhand ihres Namens.
        /// </summary>
        /// <param name="name">Der Name der StringResource.</param>
        /// <returns>Die StringResource oder <see cref="string.Empty"> wenn die Resource nicht gefunden wurde</see>/></returns>
        public string LoadString( string name )
        {
            return Reader.LoadString( name, StringResources );
        }

        /// <summary>
        /// Lädt eine StringResource anhand ihrer ID.
        /// </summary>
        /// <param name="id">Die ID der StringResource.</param>
        /// <returns>Die StringResource oder <see cref="string.Empty"> wenn die Resource nicht gefunden wurde</see>/></returns>
        public string LoadString( long id )
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
        public bool StoreString( string name, string content, bool overwrite = false )
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
        public void WriteFile()
        {
            Writer.WriteResourceFile( StringResources );
        }

        /// <summary>
        /// Überprüft ob eine StringResource mit der angegebenen ID verfügbar ist.
        /// </summary>
        /// <param name="id">Die ID der zu suchenden Resource.</param>
        /// <returns>Gibt true zurück falls die Suche erfolgreich war.</returns>
        public bool Exists( long id )
        {
            return Reader.Exists( id, StringResources );
        } 

        /// <summary>
        /// Überprüft, ob eine StringResource mit dem angegebenen Namen verfügbar ist.
        /// </summary>
        /// <param name="name">Der Name der zu suchenden Resource.</param>
        /// <returns>Gibt true zurücl falls die Suche erfolgreich war.</returns>
        public bool Exists( string name )
        {
            return Reader.Exists( name, StringResources );
        }
    }
}
