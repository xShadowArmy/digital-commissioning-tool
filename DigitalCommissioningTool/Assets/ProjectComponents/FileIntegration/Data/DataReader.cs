using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using SystemFacade;
using ProjectComponents.Abstraction;
using System.Xml.XPath;
using System.Globalization;

namespace ProjectComponents.FileIntegration
{
    /// <summary>
    /// Ließt allgemeine Projektdaten aus einer Xml Datei.
    /// </summary>
    internal class DataReader
    {
        /// <summary>
        /// Die Datei aus der gelesen werden soll.
        /// </summary>
        private XmlDocument Doc { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="doc">Die Datei aus der gelesen werden soll.</param>
        internal DataReader( XmlDocument doc )
        {
            Doc = doc;
        }

        /// <summary>
        /// Ließt die Daten aus der Datei.
        /// </summary>
        /// <param name="data">Objekt das zum Speichern der Daten verwendet wird.</param>
        internal void ReadFile( InternalProjectData data )
        {
            LogManager.WriteInfo( "Datei \"Data.xml\" wird gelesen.", "DataReader", "ReadFile" );

            try
            {
                Doc.Load( Paths.TempPath + "Data.xml" );

                XPathNavigator nav = Doc.CreateNavigator( );

                nav.MoveToFirstChild( );
                nav.MoveToFirstChild( );

                data.Name =  nav.Value;
                nav.MoveToNext( );

                data.FullPath = nav.Value;
                nav.MoveToNext( );

                try
                {
                    data.DateCreated = DateTime.ParseExact( nav.Value, "dd/MM/yyyy", CultureInfo.CurrentCulture );
                    nav.MoveToNext( );

                    data.DateModified = DateTime.ParseExact( nav.Value, "dd/MM/yyyy", CultureInfo.CurrentCulture );
                }
                
                catch( Exception )
                {
                    try
                    {
                        data.DateCreated = DateTime.ParseExact( nav.Value, "dd.MM.yyyy", CultureInfo.CurrentCulture );
                        nav.MoveToNext( );

                        data.DateModified = DateTime.ParseExact( nav.Value, "dd.MM.yyyy", CultureInfo.CurrentCulture );
                    }

                    catch( Exception )
                    {
                        data.DateCreated  = DateTime.Now;
                        data.DateModified = DateTime.Now;
                    }
                }
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Data.xml\" konnte nicht  werden! Fehler: " + e.Message, LogLevel.Error, true, "DataReader", "ReadFile" );
            }
        }
    }
}
