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
    public class WallData : GameObjectData
    {
        public delegate void WallChangedEventHandler( WallData wall );

        public event WallChangedEventHandler WallChanged;

        public WallFace Face { get; internal set; }

        public WallClass Class { get; internal set; }

        public static long NorthWallLength { get; internal set; }

        public static long EastWallLength { get; internal set; }

        public static long SouthWallLength { get; internal set; }

        public static long WesthWallLength { get; internal set; }
        
        internal WallData() : base( GameObjectDataType.Wall )
        {
        }

        internal WallData( long id ) : base( GameObjectDataType.Wall, id )
        {
        }

        internal WallData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( GameObjectDataType.Wall, id, position, rotation, scale )
        {
        }

        internal WallData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( GameObjectDataType.Wall, id, position, rotation, scale, obj )
        {
        }

        public void SetFace( WallFace face )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "WallData", "SetFace" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return;
            }

            Face = face;
            OnChange( );
        }

        public void SetClass( WallClass wClass )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "WallData", "SetClass" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return;
            }

            Class = wClass;
            OnChange( );
        }
        
        protected new virtual void OnChange()
        {
            base.OnChange( );
        }
    }
}
