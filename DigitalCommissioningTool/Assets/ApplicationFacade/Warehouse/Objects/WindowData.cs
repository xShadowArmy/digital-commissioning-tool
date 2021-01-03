using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;
using ApplicationFacade.Application;

namespace ApplicationFacade.Warehouse
{
    public class WindowData : WallObjectData
    {
        internal WindowData() : base( )
        {
        }

        internal WindowData( long id ) : base( id )
        {
        }

        internal WindowData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( id, position, rotation, scale )
        {
        }

        internal WindowData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
        }

        protected override void ObjectChanged( )
        {
            LogManager.WriteInfo( "Aktualisiere WindowData.", "WindowData", "ObjectChanged" );

            for ( int i = 0; i < GameManager.GameWarehouse.Data.Windows.Count; i++ )
            {
                if ( GetID( ) == GameManager.GameWarehouse.Data.Windows[i].ID )
                {
                    GameManager.GameWarehouse.Data.Windows.Remove( GameManager.GameWarehouse.Data.Windows[i] );
                    GameManager.GameWarehouse.Data.Windows.Add( new ProjectWindowData( GetID( ), Face.ToString( ), Class.ToString( ), new ProjectTransformationData( Position, Rotation, Scale ) ) );

                    return;
                }
            }
        }
    }
}
