using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    public struct ProjectTransformationData
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public ProjectTransformationData( Vector3 position, Vector3 rotation, Vector3 scale )
        {
            Position = position;
            Rotation = rotation;
            Scale    = scale;
        }

        public void SetPosition( Vector3 position )
        {
            Position = position;
        }

        public void SetRotation( Vector3 rotation )
        {
            Rotation = rotation;
        }

        public void SetScale( Vector3 scale )
        {
            Scale = scale;
        }
    }
}
