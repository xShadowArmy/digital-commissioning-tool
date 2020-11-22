using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using UnityEngine;

namespace ApplicationFacade
{
    public class WindowData : GameObjectData
    {
        public delegate void WindowChangedEventHandler( WindowData window );

        public event WindowChangedEventHandler WindowChanged;

        internal WindowData() : base( GameObjectDataType.Window )
        {
        }

        internal WindowData( long id ) : base( GameObjectDataType.Window, id )
        {
        }

        internal WindowData( long id, Vector3 position, Vector3 rotation, Vector3 scale ) : base( GameObjectDataType.Window, id, position, rotation, scale )
        {
        }

        internal WindowData( long id, Vector3 position, Vector3 rotation, Vector3 scale, GameObject obj ) : base( GameObjectDataType.Window, id, position, rotation, scale, obj )
        {
        }

        protected new virtual void OnChange()
        {
            base.OnChange( );
        }
    }
}
