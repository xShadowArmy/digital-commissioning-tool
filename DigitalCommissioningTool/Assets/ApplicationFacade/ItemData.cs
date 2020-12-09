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
    public class ItemData : GameObjectData
    {
        public delegate void ItemChangedEventHandler( ItemData item );

        public event ItemChangedEventHandler ItemChanged;

        public long Count { get; protected set; }

        public string Name { get; protected set; }

        public StorageData Parent { get; private set; }

        internal ItemData() : base( GameObjectDataType.Item )
        {
            Count = 0;
            Name = string.Empty;
        }

        internal ItemData( long id, long itemCount ) : base( GameObjectDataType.Item, id )
        {
            Count = itemCount;
            Name = string.Empty;
        }

        internal ItemData( long id, long itemCount, Vector3 position, Quaternion rotation, Vector3 scale ) : base( GameObjectDataType.Item, id, position, rotation, scale )
        {
            Count = itemCount;
            Name = string.Empty;
        }

        internal ItemData( long id, long itemCount, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( GameObjectDataType.Item, id, position, rotation, scale, obj )
        {
            Count = itemCount;
            Name = string.Empty;
        }

        internal void SetParent( StorageData storage )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "ItemData", "SetParent" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return;
            }

            Parent = storage;
        }

        public void SetItemCount( long itemCount )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "ItemData", "SetItemCount" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return;
            }

            Count = itemCount;
            OnChange( );
        }

        public void SetItemName( string itemName )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "ItemData", "SetItemName" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return;
            }

            Name = itemName;
            OnChange( );
        }

        protected new virtual void OnChange()
        {
            base.OnChange( );
        }
    }
}