using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ApplicationFacade
{
    public class DoorData : GameObjectData
    {
        public delegate void DoorChangedEventHandler( DoorData door );

        public event DoorChangedEventHandler DoorChanged;

        public DoorType Type { get; internal set; }

        internal DoorData() : base( GameObjectDataType.Door )
        {
            Type = DoorType.Door;
        }

        internal DoorData( long id ) : base( GameObjectDataType.Door, id )
        {
            Type = DoorType.Door;
        }

        internal DoorData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( GameObjectDataType.Door, id, position, rotation, scale )
        {
            Type = DoorType.Door;
        }
                                  
        internal DoorData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( GameObjectDataType.Door, id, position, rotation, scale, obj )
        {
            Type = DoorType.Door;
        }

        public void SetDoorType( DoorType type )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            Type = type;
            OnChange( );
        }

        protected new virtual void OnChange()
        {
            base.OnChange( );
        }
    }
}
