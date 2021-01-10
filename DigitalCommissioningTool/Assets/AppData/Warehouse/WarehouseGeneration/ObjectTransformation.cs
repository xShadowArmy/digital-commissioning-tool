using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AppData.Warehouse
{
    /// <summary>
    /// Transformationensdaten die fuer die Lagerhaus berechnen verwendet werden.
    /// </summary>
    public struct ObjectTransformation
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
        /// <param name="position">Die Position des Objekts.</param>
        /// <param name="rotation">Die Rotation des Objekts.</param>
        /// <param name="scale">Die Skalierung des Objekts.</param>
        public ObjectTransformation( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            Position = position;
            Rotation = rotation;
            Scale    = scale;
        }
    }
}
