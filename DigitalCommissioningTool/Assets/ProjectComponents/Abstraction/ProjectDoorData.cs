using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    /// <summary>
    /// Stellt eine Tür in der Xml Dateistruktur eines Projekts dar.
    /// </summary>
    public struct ProjectDoorData
    {
        /// <summary>
        /// Die ID der Tür.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Die Art der Tür.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Die Ausrichtung der Tür.
        /// </summary>
        public string Face { get; set; }

        /// <summary>
        /// Die Kategorie der Tür.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Die Transformationsdaten der Tür.
        /// </summary>
        public ProjectTransformationData Transformation { get; private set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID der Tür.</param>
        /// <param name="face">Die Ausrichtung der Tür.</param>
        /// <param name="wClass">Die Kategorie der Tür.</param>
        /// <param name="type">Die Art der Tür.</param>
        /// <param name="transformation">Die Transformationsdaten der Tür.</param>
        public ProjectDoorData( long id, string face, string wClass, string type, ProjectTransformationData transformation )
        {
            ID    = id;
            Face  = face;
            Class = wClass;
            Type  = type;
            Transformation = transformation;
        }
    }
}
