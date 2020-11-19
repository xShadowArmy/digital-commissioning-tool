using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using UnityEngine;

namespace ApplicationFacade
{
    public struct TransformationData
    {
        public Vector3 Position
        {
            get
            {
                return Data.Position;
            }

            set
            {
                Data.SetPosition( value );
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return Data.Position;
            }

            set
            {
                Data.SetPosition( value );
            }
        }

        public Vector3 Scale
        {
            get
            {
                return Data.Position;
            }

            set
            {
                Data.SetPosition( value );
            }
        }

        private ProjectTransformationData Data;
        
        public TransformationData( Vector3 position, Vector3 rotation, Vector3 scale )
        {
            Data = new ProjectTransformationData( position, rotation, scale );
        }
    }
}
