using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    /// <summary>
    /// Stellt ein Fenster in der internen Projekt Dateistruktur dar.
    /// </summary>
    public struct ProjectWindowData
    {
        /// <summary>
        /// Die ID des Fensters.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Die Ausrichtung des Fensters.
        /// </summary>
        public string Face { get; set; }

        /// <summary>
        /// Die Kategorie des Fensters.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Die Transformationsdaten des Fenster.
        /// </summary>
        public ProjectTransformationData Transformation { get; set; }
        
        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Fenster.</param>
        /// <param name="face">Die Ausrichtung des Fensters.</param>
        /// <param name="wClass">Die Kategorie des Fensters.</param>
        /// <param name="transformation">Die Transformationsdaten des Fensters.</param>
        public ProjectWindowData( long id, string face, string wClass, ProjectTransformationData transformation )
        {
            ID    = id;
            Face  = face;
            Class = wClass;
            Transformation = transformation;
        }        
    }
}
