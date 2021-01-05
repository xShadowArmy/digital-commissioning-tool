using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using SystemFacade;
using ApplicationFacade.Application;
using UnityEngine;

namespace ApplicationFacade.Warehouse
{
    public class WallData : WallObjectData
    {       
        internal WallData() : base( )
        {
        }

        internal WallData( long id ) : base( id )
        {
        }

        internal WallData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( id, position, rotation, scale )
        {
        }

        internal WallData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
        }
        
        protected override void ObjectChanged()
        {
            LogManager.WriteInfo( "Aktualisiere WallData.", "WallData", "ObjectChanged" );

            for ( int i = 0; i < GameManager.GameWarehouse.Data.Walls.Count; i++ )
            {
                if ( GetID( ) == GameManager.GameWarehouse.Data.Walls[i].ID )
                {
                    GameManager.GameWarehouse.Data.Walls.Remove( GameManager.GameWarehouse.Data.Walls[i] );
                    GameManager.GameWarehouse.Data.Walls.Insert( i, new ProjectWallData( GetID( ), Object.tag, Face.ToString( ), Class.ToString( ), new ProjectTransformationData( Position, Rotation, Scale ) ) );

                    return;
                }
            }
        }
    }
}
