using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationFacade.Application;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ApplicationFacade.Warehouse
{
    public class DoorData : WallObjectData
    {
        public DoorType Type { get; internal set; }

        internal DoorData() : base( )
        {
            Type = DoorType.Door;
        }

        internal DoorData( long id ) : base( id )
        {
            Type = DoorType.Door;
        }

        internal DoorData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( id, position, rotation, scale )
        {
            Type = DoorType.Door;
        }
                                  
        internal DoorData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
            Type = DoorType.Door;
        }

        protected override void ObjectChanged( )
        {
            LogManager.WriteInfo( "Aktualisiere DoorData.", "DoorData", "ObjectChanged" );

            for ( int i = 0; i < GameManager.GameWarehouse.Data.Doors.Count; i++ )
            {
                if ( GetID( ) == GameManager.GameWarehouse.Data.Doors[i].ID )
                {
                    GameManager.GameWarehouse.Data.Doors.Remove( GameManager.GameWarehouse.Data.Doors[i] );
                    GameManager.GameWarehouse.Data.Doors.Insert( i, new ProjectDoorData( GetID( ), Face.ToString(), Class.ToString(), Type.ToString( ), new ProjectTransformationData( Position, Rotation, Scale ) ) );

                    return;
                }
            }
        }
    }
}
