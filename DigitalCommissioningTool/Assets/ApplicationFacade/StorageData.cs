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
        private List<ItemData> Data { get; set; }

        public StorageData() : base()
        {
            Data = new List<ItemData>( );
        }

        public StorageData( long id ) : base( id )
        {
            Data = new List<ItemData>( );
        }

        public StorageData( long id, Vector3 position, Vector3 rotation, Vector3 scale ) : base( id, position, rotation, scale )
        {
            Data = new List<ItemData>( );
        }

        public StorageData( long id, Vector3 position, Vector3 rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
            Data = new List<ItemData>( );
        }

        public void AddItem( ItemData item )
        {
            Data.Add( item );
        }

        public bool RemoveItem( ItemData item )
        {
            for( int i = 0; i < Data.Count; i++ )
            {
                if ( Data[i].GetID() == item.GetID() )
                {
                    return Data.Remove( Data[ i ] );
                }
            }

            return false;
        }

        public bool ContainsItem( ItemData item )
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
    }
}
