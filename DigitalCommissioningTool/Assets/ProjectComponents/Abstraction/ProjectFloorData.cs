using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    /// <summary>
    /// Stellt einen Boden in der internen Projekt Xml Dateistruktur dar.
    /// </summary>
    public struct ProjectFloorData
    {
        /// <summary>
        /// Die ID des Bodens.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Die Transformationsdaten des Bodens.
        /// </summary>
        public ProjectTransformationData Transformation { get; private set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        /// <param name="transformation">Die Transformationsdaten des Objekts.</param>
        public ProjectFloorData( long id, ProjectTransformationData transformation )
        {
            Transformation = transformation;
            ID = id;
        }
    }
}
