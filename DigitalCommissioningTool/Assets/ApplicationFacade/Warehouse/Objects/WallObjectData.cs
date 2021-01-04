using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ApplicationFacade.Warehouse
{
    public abstract class WallObjectData : GameObjectData
    {
        public WallFace Face { get; internal set; }

        public WallClass Class { get; internal set; }

        internal WallObjectData(  ) : base( )
        {
        }

        internal WallObjectData( long id ) : base( id )
        {
        }

        internal WallObjectData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( id, position, rotation, scale )
        {
        }

        internal WallObjectData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
        }

        public void SetFace( WallFace face )
        {
            Face = face;

            ObjectChanged( );
        }

        public void SetClass( WallClass wClass )
        {
            Class = wClass;

            ObjectChanged( );
        }

        protected abstract override void ObjectChanged();
    }
}
