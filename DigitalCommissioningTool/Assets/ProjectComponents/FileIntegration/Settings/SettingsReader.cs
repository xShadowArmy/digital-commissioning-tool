using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ProjectComponents.FileIntegration
{
    /// <summary>
    /// Ließt die Projekteinstellungen aus der Xml Datei.
    /// </summary>
    internal class SettingsReader
    {
        /// <summary>
        /// Die Datei aus der die Daten gelesen werden.
        /// </summary>
        private XmlDocument Doc { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="doc">Die Datei aus der gelesen werden soll.</param>
        internal SettingsReader( XmlDocument doc )
        {
            Doc = doc;
        }

        /// <summary>
        /// Ließt die Daten aus der Datei.
        /// </summary>
        /// <param name="settings">Objekt das zum Speichern der Daten verwendet wird.</param>
        internal void ReadFile( InternalProjectSettings settings )
        {
            LogManager.WriteInfo( "Datei \"Settings.xml\" wird gelesen.", "SettingsReader", "ReadFile" );

            try
            {
                Doc.Load( Paths.TempPath + "Settings.xml" );

                XPathNavigator nav = Doc.CreateNavigator( );

                nav.MoveToFirstChild( );
                nav.MoveToFirstChild( );
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Settings.xml\" konnte nicht gelesen werden! Fehler: " + e.Message, LogLevel.Error, true, "SettingsReader", "ReadFile" );
            }
        }
    }
}