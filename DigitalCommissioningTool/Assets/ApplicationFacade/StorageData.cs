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
    public class StorageData : GameObjectData
    {
        public delegate void StorageChangedEventHandlder( StorageData storage );

        public event StorageChangedEventHandlder StorageChanged;

        private List<ItemData> Data { get; set; }

        public ItemData[] GetItems
        {
            get
            {
                return Data.ToArray( );
            }
        }

        internal StorageData() : base( GameObjectDataType.StorageReck )
        {
            Data = new List<ItemData>( );
        }

        internal StorageData( long id ) : base( GameObjectDataType.StorageReck, id )
        {
            Data = new List<ItemData>( );
        }

        internal StorageData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( GameObjectDataType.StorageReck, id, position, rotation, scale )
        {
            Data = new List<ItemData>( );
        }

        internal StorageData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( GameObjectDataType.StorageReck, id, position, rotation, scale, obj )
        {
            Data = new List<ItemData>( );
        }

        internal void AddItem( ItemData item )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "StorageData", "AddItem" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return;
            }

            Data.Add( item );
            OnChange( );
        }

        internal bool RemoveItem( ItemData item )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "StorageData", "AddItem" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return false;
            }

            for ( int i = 0; i < Data.Count; i++ )
            {
                if ( Data[i].GetID() == item.GetID() )
                {
                    bool res = Data.Remove( Data[ i ] );

                    OnChange( );

                    return res;
                }
            }

            return false;
        }

        internal bool ContainsItem( ItemData item )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "StorageData", "AddItem" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return false;
            }

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
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "StorageData", "GetItem" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return null;
            }

            for ( int i = 0; i < Data.Count; i++ )
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
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "StorageData", "GetItem" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return null;
            }

            for ( int i = 0; i < Data.Count; i++ )
            {
                if ( Data[ i ].Object == obj )
                {
                    return Data[ i ];
                }
            }

            return null;
        }

        protected new virtual void OnChange()
        {
            base.OnChange( );
        }
    }
}
