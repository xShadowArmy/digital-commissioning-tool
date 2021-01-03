using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AppData.Warehouse
{
    public struct ObjectTransformation
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public ObjectTransformation( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            Position = position;
            Rotation = rotation;
            Scale    = scale;
        }
    }
}
