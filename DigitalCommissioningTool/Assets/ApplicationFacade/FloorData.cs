using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using UnityEngine;

namespace ApplicationFacade
{
    public class FloorData : GameObjectData
    {
        internal FloorData() : base( )
        {
        }

        internal FloorData( Vector3 position, Vector3 rotation, Vector3 scale ) : base( 0, position, rotation, scale )
        {
        }

        internal FloorData( Vector3 position, Vector3 rotation, Vector3 scale, GameObject obj ) : base( 0, position, rotation, scale, obj )
        {
        }

        public new void SetID( long id )
        {
            ID = 0;
        }
    }
}
