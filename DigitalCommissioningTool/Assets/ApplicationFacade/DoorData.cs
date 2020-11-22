using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using UnityEngine;

namespace ApplicationFacade
{
    public class DoorData : GameObjectData
    {
        public delegate void DoorChangedEventHandler( DoorData door );

        public event DoorChangedEventHandler DoorChanged;

        public DoorType Type { get; protected set; }

        internal DoorData() : base( GameObjectDataType.Door )
        {
            Type = DoorType.Door;
        }

        internal DoorData( long id, DoorType type ) : base( GameObjectDataType.Door, id )
        {
            Type = type;
        }

        internal DoorData( long id, DoorType type, Vector3 position, Vector3 rotation, Vector3 scale ) : base( GameObjectDataType.Door, id, position, rotation, scale )
        {
            Type = type;
        }

        internal DoorData( long id, DoorType type, Vector3 position, Vector3 rotation, Vector3 scale, GameObject obj ) : base( GameObjectDataType.Door, id, position, rotation, scale, obj )
        {
            Type = type;
        }

        public void SetDoorType( DoorType type )
        {
            Type = type;
            OnChange( );
        }

        protected new virtual void OnChange()
        {
            base.OnChange( );
        }
    }
}
