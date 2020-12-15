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

        public int SlotCount { get; internal set; }

        public static int DefaultSlotCount
        {
            get
            {
                return 12;
            }
        }

        private ItemData[] Data { get; set; }

        public ItemData[] GetItems
        {
            get
            {
                return Data.ToArray( );
            }
        }

        internal StorageData( ) : base( GameObjectDataType.StorageReck )
        {
            Data = new ItemData[DefaultSlotCount];
            SlotCount = DefaultSlotCount;
        }

        internal StorageData( long id ) : base( GameObjectDataType.StorageReck, id )
        {
            Data = new ItemData[DefaultSlotCount];
            SlotCount = DefaultSlotCount;
        }

        internal StorageData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( GameObjectDataType.StorageReck, id, position, rotation, scale )
        {
            Data = new ItemData[DefaultSlotCount];
            SlotCount = DefaultSlotCount;
        }

        internal StorageData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( GameObjectDataType.StorageReck, id, position, rotation, scale, obj )
        {
            Data = new ItemData[DefaultSlotCount];
            SlotCount = DefaultSlotCount;
        }

        internal void AddItem( ItemData item, int slot )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            if ( slot > SlotCount )
            {
                LogManager.WriteWarning( "Ein Objekt soll auf ein Slot abgelegt werden der nicht existiert!", "StorageData", "AddItem" );

                return;
            }

            if ( Data[slot] != null )
            {
                LogManager.WriteWarning( "Ein Objekt soll auf ein Slot abgelegt werden der bereits belegt ist!", "StorageData", "AddItem" );

                return;
            }

            Data[slot] = item;
            OnChange( this );
        }

        internal bool RemoveItem( ItemData item )
        {
            if ( IsDestroyed( ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( Data[i].GetID() == item.GetID() )
                {
                    Data[i] = null;

                    OnChange( this );

                    return true;
                }
            }

            return false;
        }

        internal bool ContainsItem( ItemData item )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( Data[i].GetID() == item.GetID() )
                {
                    return true;
                }
            }

            return false;
        }

        public ItemData GetItem( int slot )
        {
            if ( IsDestroyed( ) )
            {
                return null;
            }

            if ( slot > SlotCount )
            {
                LogManager.WriteWarning( "Ein Objekt soll aus einem Slot abgefragt werden der nicht existiert!", "StorageData", "AddItem" );

                return null;
            }

            if ( Data[slot] == null )
            {
                return null;
            }

            return Data[slot];
        }

        public ItemData GetItem( long id )
        {
            if ( IsDestroyed( ) )
            {
                return null;
            }

            for ( int i = 0; i < Data.Length; i++ )
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
            if ( IsDestroyed( ) )
            {
                return null;
            }

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( Data[i].Object == obj )
                {
                    return Data[ i ];
                }
            }

            return null;
        }
        
        public int GetSlot( GameObject obj )
        {
            if ( IsDestroyed( ) )
            {
                return -1;
            }

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( Data[i].Object == obj )
                {
                    return i;
                }
            }

            return -1;
        }

        public int GetSlot( ItemData item )
        {
            if ( IsDestroyed( ) )
            {
                return -1;
            }

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( Data[i].GetID() == item.GetID() )
                {
                    return i;
                }
            }

            return -1;
        }

        public void ChangeSlotCount( int slots )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            ItemData[] tmp = Data;

            SlotCount = slots;

            Data = new ItemData[slots];

            for( int i = 0; i < Data.Length; i++ )
            {
                if ( i < tmp.Length )
                {
                    Data[i] = tmp[i];
                }
            }

            OnChange( );
        }
        
        protected virtual void OnChange( StorageData data )
        {
            base.OnChange( );
            StorageChanged?.Invoke( data );
        }
    }
}
