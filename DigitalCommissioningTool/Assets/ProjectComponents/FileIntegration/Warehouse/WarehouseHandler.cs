using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ProjectComponents.Abstraction;

namespace ProjectComponents.FileIntegration
{
    /// <summary>
    /// Enthält Methoden zum Schreiben und Lesen von dem Projekt Lagerhaus.
    /// </summary>
    public class WarehouseHandler
    {
        /// <summary>
        /// Objekt das zum Lesen der Datei verwendet wird.
        /// </summary>
        private WarehouseReader Reader;

        /// <summary>
        /// Objekt das zum Schreiben der Datei verwendet wird.
        /// </summary>
        private WarehouseWriter Writer;

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        public WarehouseHandler()
        {
            Reader = new WarehouseReader( new XmlDocument() );
            Writer = new WarehouseWriter( new XmlDocument() );
        }

        /// <summary>
        /// Schreibt die Daten in die Datei.
        /// </summary>
        /// <param name="warehouse">Die Daten die Gespeichert werden sollen.</param>
        public void SaveFile( InternalProjectWarehouse warehouse)
        {
            Writer.WriteFile( warehouse );
        }

        /// <summary>
        /// L#dt die Daten aus der Datei.
        /// </summary>
        /// <returns>Objekt das die geladenen Daten enthält.</returns>
        public InternalProjectWarehouse LoadFile()
        {
            return Reader.ReadFile( );
        }
    }
}
