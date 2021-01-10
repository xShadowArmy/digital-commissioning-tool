using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ProjectComponents.Abstraction;
using SystemFacade;

namespace ProjectComponents.FileIntegration
{
    /// <summary>
    /// Enthält Operationen zum Speichern und Laden von Allgemeinen Projektdaten.
    /// </summary>
    public class DataHandler
    {
        /// <summary>
        /// Objekt zum Lesen der Daten aus der Xml Datei.
        /// </summary>
        private DataReader Reader { get; set; }

        /// <summary>
        /// Objekt zum Speichern der Daten in die Xml Datei.
        /// </summary>
        private DataWriter Writer { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        public DataHandler()
        {
            Reader = new DataReader( new XmlDocument() );
            Writer = new DataWriter( new XmlDocument() );
        }

        /// <summary>
        /// Ließt die Daten aus der Datei.
        /// </summary>
        /// <returns>Objekt das die Daten enthält.</returns>
        public InternalProjectData LoadFile( )
        {
            LogManager.WriteInfo( "ProjektData Datei wird gelesen.", "DataHandler", "LoadFile" );

            InternalProjectData data = new InternalProjectData( );

            Reader.ReadFile( data );

            return data;
        }

        /// <summary>
        /// Speichert die Daten in die Datei.
        /// </summary>
        /// <param name="data">Objekt das die zu speichernden Daten enthält.</param>
        public void SaveFile( InternalProjectData data )
        {
            LogManager.WriteInfo( "ProjektData Datei wird geschrieben.", "DataHandler", "SaveFile" );
            
            Writer.WriteFile( data );
        }
    }
}
