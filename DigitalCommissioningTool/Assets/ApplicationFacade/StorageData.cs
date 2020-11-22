using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using UnityEngine;

namespace ApplicationFacade
{
    public class StorageData : GameObjectData
    {
        public delegate void StorageChangedEventHandlder( StorageData storage );

        public event StorageChangedEventHandlder StorageChanged;

        private List<ItemData> Data { get; set; }

        internal StorageData() : base( GameObjectDataType.StorageReck )
        {
            Data = new List<ItemData>( );
        }

        internal StorageData( long id ) : base( GameObjectDataType.StorageReck, id )
        {
            Data = new List<ItemData>( );
        }

        internal StorageData( long id, Vector3 position, Vector3 rotation, Vector3 scale ) : base( GameObjectDataType.StorageReck, id, position, rotation, scale )
        {
            Data = new List<ItemData>( );
        }

        internal StorageData( long id, Vector3 position, Vector3 rotation, Vector3 scale, GameObject obj ) : base( GameObjectDataType.StorageReck, id, position, rotation, scale, obj )
        {
            Data = new List<ItemData>( );
        }

        internal void AddItem( ItemData item )
        {
            OnChange( );
            Data.Add( item );
        }

        internal bool RemoveItem( ItemData item )
        {
            for( int i = 0; i < Data.Count; i++ )
            {
                if ( Data[i].GetID() == item.GetID() )
                {
                    OnChange( );
                    return Data.Remove( Data[ i ] );
                }
            }

            return false;
        }

        internal bool ContainsItem( ItemData item )
        {
            for ( int i = 0; i < Data.Count; i++ )
            {
                if ( Data[ i ].GetID() == item.GetID() )
                {
                    return true;
                }
            }

            return false;
        }

        public ItemData GetItem( long id )
        {
            for( int i = 0; i < Data.Count; i++ )
            {
                if ( Data[i].GetID() == id )
                {
                    return Data[i];
                }
            }

            return null;
        }

        public ItemData GetItem( GameObject obj )
        {
            for ( int i = 0; i < Data.Count; i++ )
            {
                if ( Data[ i ].Object == obj )
                {
                    return Data[ i ];
                }
            }

            return null;
        }

        public ItemData[ ] GetItems()
        {
            return Data.ToArray( );
        }

        protected new virtual void OnChange()
        {
            base.OnChange( );
        }
    }
}
