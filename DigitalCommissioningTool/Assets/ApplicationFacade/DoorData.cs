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
        public DoorType Type { get; protected set; }

        internal DoorData() : base( )
        {
            Type = DoorType.Door;
        }

        internal DoorData( long id, DoorType type ) : base( id )
        {
            Type = type;
        }

        internal DoorData( long id, DoorType type, Vector3 position, Vector3 rotation, Vector3 scale ) : base( id, position, rotation, scale )
        {
            Type = type;
        }

        internal DoorData( long id, DoorType type, Vector3 position, Vector3 rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
            Type = type;
        }

        public void SetDoorType( DoorType type )
        {
            Type = type;
        }
    }
}
