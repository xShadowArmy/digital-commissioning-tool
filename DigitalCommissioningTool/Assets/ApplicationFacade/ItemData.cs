using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using UnityEngine;

namespace ApplicationFacade
{
    public class ItemData : GameObjectData
    {
        public long ItemCount { get; protected set; }

        internal ItemData() : base( )
        {
            ItemCount = 0;
        }

        internal ItemData( long id, long itemCount ) : base( id )
        {
            ItemCount = itemCount;
        }

        internal ItemData( long id, long itemCount, Vector3 position, Vector3 rotation, Vector3 scale ) : base( id, position, rotation, scale )
        {
            ItemCount = itemCount;
        }

        internal ItemData( long id, long itemCount, Vector3 position, Vector3 rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
            ItemCount = itemCount;
        }

        public void SetItemCount( long itemCount )
        {
            ItemCount = itemCount;
        }
    }
}