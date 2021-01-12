using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    /// <summary>
    /// Stellt die Transformationendaten dar die in den XMl Projektdateien verwendet werden.
    /// </summary>
    public struct ProjectTransformationData
    {
        /// <summary>
        /// Die Position eines Objekts.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Die Rotation eines Objekts.
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// Die Skalierung eines Objekts.
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="position">Die Positionsdaten des Objekts.</param>
        /// <param name="rotation">Die Rotationsdaten des Objekts.</param>
        /// <param name="scale">Die Skalierung des Objekts.</param>
        public ProjectTransformationData( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            Position = position;
            Rotation = rotation;
            Scale    = scale;
        }
    }
}
