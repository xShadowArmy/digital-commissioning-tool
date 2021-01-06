using System;
using System.Collections.Generic;
using AppData.Warehouse;
using ApplicationFacade.Application;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ApplicationFacade.Warehouse
{
    public class StorageData : GameObjectData
    {
        public bool IsContainer { get; internal set; }

        public int SlotCount { get; internal set; }

        private ItemData[] Data { get; set; }

        private GameObject[] Slots { get; set; }

        public ItemData[] GetItems
        {
            get
            {
                return Data;
            }
        }

        internal StorageData( ) : base( )
        {
        }

        internal StorageData( long id ) : base( id )
        {
        }

        internal StorageData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( id, position, rotation, scale )
        {
        }

        internal StorageData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
        }
        
        public ItemData GetItem( int slot )
        {
            if ( IsDestroyed( ) || slot == -1 )
            {
                return null;
            }

            if ( slot >= Slots.Length )
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
                if ( Data[i] != null && Data[i].Object == obj )
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

        public void AddItem( ItemData item, int slot = -1 )
        {
            LogManager.WriteInfo( "Ein RegalItem wird hinzugefuegt.", "Warehouse", "AddItemToStorageRack" );

            if ( IsDestroyed( ) )
            {
                return;
            }

            if ( slot < 0 )
            {
                for ( int i = 0; i < SlotCount; i++ )
                {
                    if ( Data[i] == null )
                    {
                        slot = i;
                        break;
                    }
                }

                if ( slot < 0 )
                {
                    return;
                }
            }

            else
            {
                if ( slot >= Slots.Length )
                {
                    LogManager.WriteWarning( "Ein Objekt soll auf ein Slot abgelegt werden der nicht existiert!", "StorageData", "AddItem" );

                    return;
                }
            }

            if ( item.Object != null )
            {
                GameObject.Destroy( Slots[slot] );
                Slots[slot] = item.Object;
            }

            Data[slot] = item;

            item.SetID( Warehouse.GetUniqueID( Data ) );

            item.ChangeGameObject( Slots[slot] );
            item.Object.name = item.Name;
            item.ParentStorage = this;

            Data[slot].Object.SetActive( true );

            Data[slot].Position = Slots[slot].transform.position;
            Data[slot].Rotation = Slots[slot].transform.rotation;
            Data[slot].Scale    = Slots[slot].transform.localScale;

            foreach ( ProjectStorageData data in GameManager.GameWarehouse.Data.StorageRacks )
            {
                if ( data.ID == GetID( ) )
                {
                    data.Items[slot] = new ProjectItemData( item.IDRef, item.GetID(), item.Count, item.Weight, item.Name, new ProjectTransformationData( item.Position, item.Rotation, item.Scale ) );

                    break;
                }
            }

            ObjectChanged( );
        }

        public bool RemoveItem( ItemData item )
        {
            if ( IsDestroyed( ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( Data[i].GetID() == item.GetID() )
                {
                    Data[i].Object.SetActive( false );
                    Data[i] = null;

                    item.ParentStorage = null;

                    ObjectChanged( );

                    return true;
                }
            }

            return false;
        }

        public bool ContainsItem( ItemData item )
        {
            if ( IsDestroyed( ) )
            {
                return false;
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

        public new void Destroy()
        {
            base.Destroy( );

            foreach( ItemData item in Data )
            {
                if ( item != null )
                {
                    item.Destroy( );
                }
            }
        }

        internal void ChangeSlotCount( ISlotCalcStrategie strategie )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            strategie.StartGeneration( );

            SlotCount = strategie.GetSlotCount( );

            if ( Data == null )
            {
                Data = new ItemData[SlotCount];
            }

            else
            {
                ItemData[] tmp = new ItemData[SlotCount];

                Data.CopyTo( tmp, 0 );
            }

            bool change = false;

            if ( Slots == null )
            {
                Slots = new GameObject[SlotCount];
            }

            else
            {
                foreach( GameObject obj in Slots )
                {
                    GameObject.Destroy( obj );
                }

                change = true;
            }                

            List<GameObject> layers = new List<GameObject>();

            foreach( Transform data in Object.transform )
            {
                if ( data.gameObject.CompareTag( "StorageRackLayer" ) )
                {
                    layers.Add( data.gameObject );
                }
            }

            for ( int i = 0; i < SlotCount; i++ )
            {
                switch ( i % 3 )
                {
                    case 0:

                        Slots[i] = GameObject.Instantiate( GameObject.FindGameObjectWithTag( "StorageBoxOpen" ), layers[i / ( SlotCount / strategie.GetLayerCount())].transform );
                        break;

                    case 1:

                        Slots[i] = GameObject.Instantiate( GameObject.FindGameObjectWithTag( "StorageBoxPartialOpen" ), layers[i / ( SlotCount / strategie.GetLayerCount( ) )].transform );
                        break;

                    case 2:

                        Slots[i] = GameObject.Instantiate( GameObject.FindGameObjectWithTag( "StorageBoxClosed" ), layers[i / ( SlotCount / strategie.GetLayerCount( ) )].transform );
                        break;
                }

                Slots[i].transform.localPosition = strategie.GetPositionData( )[i];
                Slots[i].transform.localScale    = strategie.GetScaleData()[i];
                Slots[i].SetActive( false );
            }

            if ( change )
            {
                for ( int i = 0; i < Slots.Length ; i++ )
                {
                    if ( Data[i] != null )
                    {
                        Data[i].Object = Slots[i];
                    }
                }
            }

            ObjectChanged( );
        }

        protected override void ObjectChanged()
        {
            foreach ( ProjectStorageData data in GameManager.GameWarehouse.Data.StorageRacks )
            {
                if ( data.ID == GetID( ) )
                {
                    GameManager.GameWarehouse.Data.StorageRacks.Remove( data );

                    ProjectStorageData storage = new ProjectStorageData( GetID(), SlotCount, new ProjectTransformationData( Position, Rotation, Scale ) );

                    for( int i = 0; i < Data.Length; i++ )
                    {
                        if ( Data[i] != null )
                        {
                            storage.Items[i] = new ProjectItemData( Data[i].IDRef, Data[i].GetID(), Data[i].Count, Data[i].Weight, Data[i].Name, new ProjectTransformationData( Data[i].Position, Data[i].Rotation, Data[i].Scale ) );
                        }
                    }

                    GameManager.GameWarehouse.Data.StorageRacks.Add( storage );

                    break;
                }
            }
        }
    }
}
