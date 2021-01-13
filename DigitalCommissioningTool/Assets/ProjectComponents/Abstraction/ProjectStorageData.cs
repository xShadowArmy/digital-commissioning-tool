using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    /// <summary>
    /// Stellt ein Regal in den Projekt Dateien dar.
    /// </summary>
    public class ProjectStorageData
    {
        /// <summary>
        /// Die ID des Regals.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Die Anzahl der Slots des Regals.
        /// </summary>
        public int SlotCount { get; set; }

        /// <summary>
        /// Die Transformationsdaten des Regals.
        /// </summary>
        public ProjectTransformationData Transformation { get; set; }

        /// <summary>
        /// Gibt die Items des Regals zurueck.
        /// </summary>
        public ProjectItemData[] Items { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Regals.</param>
        /// <param name="slotCount">Die Anzahl der Slots.</param>
        /// <param name="transformation">Die Transformationsdaten.</param>
        public ProjectStorageData( long id, int slotCount, ProjectTransformationData transformation )
        {
            ID = id;
            SlotCount = slotCount;
            Transformation = transformation;
            Items = new ProjectItemData[slotCount];
        }        
    }
}
