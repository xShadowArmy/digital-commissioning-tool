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
        public delegate void FloorChangedEventHandler( FloorData floor );

        public event FloorChangedEventHandler FloorChanged;

        internal FloorData() : base( GameObjectDataType.Floor )
        {
        }

        internal FloorData( long id ) : base( GameObjectDataType.Floor, id )
        {
        }

        internal FloorData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( GameObjectDataType.Floor, id, position, rotation, scale )
        {
        }

        internal FloorData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( GameObjectDataType.Floor, id, position, rotation, scale, obj )
        {
        }

        protected new virtual void OnChange()
        {
            base.OnChange( );
        }
    }
}
