using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using UnityEngine;

namespace ApplicationFacade
{
    public class WallData : GameObjectData
    {
        public delegate void WallChangedEventHandler( WallData wall );

        public event WallChangedEventHandler WallChanged;

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
