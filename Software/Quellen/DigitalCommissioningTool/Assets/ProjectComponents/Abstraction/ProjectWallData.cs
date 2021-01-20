using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    /// <summary>
    /// Stellt eine Wand in der internen Xml Projekt Dateistruktur dar.
    /// </summary>
    public struct ProjectWallData
    {
        /// <summary>
        /// Die ID der Wand.
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Die Ausrichtung der Wand.
        /// </summary>
        public string Face { get; set; }

        /// <summary>
        /// Die Kategorie der Wand.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Der Tag der Wand.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Die Transformationsdaten der Wand.
        /// </summary>
        public ProjectTransformationData Transformation { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID der Wand.</param>
        /// <param name="tag"></param>
        /// <param name="face"></param>
        /// <param name="wClass"></param>
        /// <param name="transformation"></param>
        public ProjectWallData( long id, string tag, string face, string wClass, ProjectTransformationData transformation )
        {
            Tag = tag;
            ID = id;
            Face = face;
            Class = wClass;
            Transformation = transformation;
        }
    }
}
