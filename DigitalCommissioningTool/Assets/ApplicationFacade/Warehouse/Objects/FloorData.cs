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
    public class FloorData : GameObjectData
    {
        internal FloorData() : base( )
        {
        }

        internal FloorData( long id ) : base( id )
        {
        }

        internal FloorData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( id, position, rotation, scale )
        {
        }

        internal FloorData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
        }

        protected override void ObjectChanged( )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere FloorData.", "Warehouse", "FloorHasChanged" );

            for ( int i = 0; i < GameManager.GameWarehouse.Data.Floor.Count; i++ )
            {
                if ( GetID( ) == GameManager.GameWarehouse.Data.Floor[i].ID )
                {
                    GameManager.GameWarehouse.Data.Floor.Remove( GameManager.GameWarehouse.Data.Floor[i] );
                    GameManager.GameWarehouse.Data.Floor.Insert( i, new ProjectFloorData( GetID( ), new ProjectTransformationData( Position, Rotation, Scale ) ) );

                    return;
                }
            }
        }
    }
}
