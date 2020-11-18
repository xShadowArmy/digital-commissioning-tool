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
    internal class SettingsReader
    {
        private XmlDocument Doc { get; set; }

        internal SettingsReader( XmlDocument doc )
        {
            Doc = doc;
        }

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