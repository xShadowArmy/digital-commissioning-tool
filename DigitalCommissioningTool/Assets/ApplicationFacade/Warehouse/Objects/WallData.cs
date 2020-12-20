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

        public static int NorthWallLength { get; internal set; }

        public static int EastWallLength { get; internal set; }

        public static int SouthWallLength { get; internal set; }

        public static int WestWallLength { get; internal set; }
        
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
        
        protected new virtual void OnChange()
        {
            base.OnChange( );
        }
    }
}
