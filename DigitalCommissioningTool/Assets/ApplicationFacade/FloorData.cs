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

        internal FloorData( Vector3 position, Vector3 rotation, Vector3 scale ) : base( GameObjectDataType.Floor, 0, position, rotation, scale )
        {
        }

        internal FloorData( Vector3 position, Vector3 rotation, Vector3 scale, GameObject obj ) : base( GameObjectDataType.Floor, 0, position, rotation, scale, obj )
        {
        }

        public new void SetID( long id )
        {
            ID = 0;
        }

        protected new virtual void OnChange()
        {
            base.OnChange( );
        }
    }
}
