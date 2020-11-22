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
        public delegate void ItemChangedEventHandler( ItemData item );

        public event ItemChangedEventHandler ItemChanged;
                
        public long ItemCount { get; protected set; }

        public StorageData Parent { get; private set; }

        internal ItemData() : base( GameObjectDataType.Item )
        {
            ItemCount = 0;
        }

        internal ItemData( long id, long itemCount ) : base( GameObjectDataType.Item, id )
        {
            ItemCount = itemCount;
        }

        internal ItemData( long id, long itemCount, Vector3 position, Vector3 rotation, Vector3 scale ) : base( GameObjectDataType.Item, id, position, rotation, scale )
        {
            ItemCount = itemCount;
        }

        internal ItemData( long id, long itemCount, Vector3 position, Vector3 rotation, Vector3 scale, GameObject obj ) : base( GameObjectDataType.Item, id, position, rotation, scale, obj )
        {
            ItemCount = itemCount;
        }

        internal void SetParent( StorageData storage )
        {
            Parent = storage;
        }

        public void SetItemCount( long itemCount )
        {
            OnChange( );
            ItemCount = itemCount;
        }

        protected new virtual void OnChange()
        {
            base.OnChange( );
        }
    }
}