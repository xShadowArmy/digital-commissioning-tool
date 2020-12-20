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

        public int Count { get; internal set; }

        public double Weight { get; internal set; }

        public string Name { get; internal set; }

        public StorageData ParentStorage { get; internal set; }

        internal ItemData ParentItem { get; private set; }

        internal List<ItemData> ChildItems { get; private set; }

        internal static List<ItemData> ItemStock { get; set; }

        public static ItemData[] GetStock
        {
            get
            {
                return ItemStock.ToArray( );
            }
        }

        public int StockCount
        {
            get
            {
                return GetStockCount( this );
            }
        }

        public bool IsStockItem
        {
            get
            {
                if ( ChildItems.Count != 0 || ParentItem != null )
                {
                    return false;
                }

                return true;
            }
        }

        static ItemData()
        {
            ItemStock = new List<ItemData>( );
        }

        internal ItemData() : base( GameObjectDataType.Item )
        {
            Count = 0;
            Weight = 0;
            Name = string.Empty;
            ParentStorage = null;
            ParentItem = null;
            ChildItems = new List<ItemData>( );
        }

        internal ItemData( long id ) : base( GameObjectDataType.Item, id )
        {
            Count = 0;
            Weight = 0;
            Name = string.Empty;
            ParentStorage = null;
            ParentItem = null;
            ChildItems = new List<ItemData>( );
        }
        
        public bool IncreaseItemCount( int itemCount )
        {
            if ( IsDestroyed() )
            {
                return false;
            }

            if ( IsReadonly( ) )
            {
                return false;
            }

            if ( ParentItem == null )
            {
                Count += itemCount;

                return true;
            }

            else
            {
                if ( ParentItem.Count > itemCount )
                {
                    ParentItem.Count -= itemCount;
                    Count += itemCount;
                }

                else
                {
                    return false;
                }
            }
                        
            return true;
        }

        public bool DecreaseItemCount( int itemCount )
        {
            if ( IsDestroyed( ) )
            {
                return false;
            }

            if ( IsReadonly( ) )
            {
                return false;
            }

            if ( itemCount >= Count )
            {
                return false;
            }

            if ( ParentItem != null )
            {
                ParentItem.Count += Count;
            }

            Count -= itemCount;

            return true;
        }

        public void SetItemName( string itemName )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            if ( IsReadonly( ) )
            {
                return;
            }

            Name = itemName;
            OnChange( this );
        }

        public void SetItemWeight( double itemWeight )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            if ( IsReadonly( ) )
            {
                return;
            }

            Weight = itemWeight;
            OnChange( this );
        }
                
        public ItemData RequestItem( int count )
        {
            if ( IsDestroyed( ) )
            {
                return null;
            }

            if ( IsReadonly( ) )
            {
                return null;
            }

            if ( count > Count )
            {
                LogManager.WriteWarning( "Es werden mehr kopien eines Objekts angefordert als vorhanden sind!", "ItemData", "GetStockItem" );
                Debug.LogWarning( "Es werden mehr kopien eines Objekts angefordert als vorhanden sind!" );

                return null;
            }

            ItemData data = new ItemData( ID )
            {
                Count = count,
                Name = Name,
                Weight = Weight,
                ParentItem = this
            };

            Count -= count;
            ChildItems.Add( data );

            data.ItemChanged += ItemDataChanged;

            return data;
        }

        public bool ReturnItem( )
        {
            if ( IsDestroyed( ) )
            {
                return false;
            }

            if ( IsReadonly( ) )
            {
                return false;
            }

            if ( ParentItem == null )
            {
                return false;
            }

            ParentItem.Count += Count;

            if ( ChildItems.Count > 0 )
            {
                for( int i = 0; i < ChildItems.Count; i++ )
                {
                    ParentItem.ChildItems.Add( ChildItems[i] );
                }
            }

            ParentItem.ChildItems.Remove( this );

            ParentItem = null;

            ItemChanged -= ItemDataChanged;

            Destroy( );

            return true;
        }

        public static void AddItemToStock( string name, int count = 1, double weight = 0 )
        {
            ItemData item = new ItemData( Warehouse.GetUniqueID( ItemStock.ToArray( ) ) )
            {
                Name = name,
                Count = count,
                Weight = weight,
            };

            ItemStock.Add( item );
        }
        
        internal static void AddItemToStock( long idRef, string name, int count = 1, double weight = 0 )
        {
            ItemData item = new ItemData( idRef )
            {
                Name = name,
                Count = count,
                Weight = weight,
            };

            ItemStock.Add( item );
        }

        public static bool RemoveItemFromStock( ItemData item )
        {
            if ( !item.IsStockItem )
            {
                return false;
            }

            RemoveStockItem( item );

            return ItemStock.Remove( item );
        }

        public static bool RemoveItemFromStock( long id )
        {
            for ( int i = 0; i < ItemStock.Count; i++ )
            {
                if ( ItemStock[i].GetID( ) == id )
                {
                    RemoveStockItem( ItemStock[i] );

                    return ItemStock.Remove( ItemStock[i] );
                }
            }

            return false;
        }

        public static bool RemoveItemFromStock( string name )
        {
            for ( int i = 0; i < ItemStock.Count; i++ )
            {
                if ( ItemStock[i].Name == name )
                {
                    RemoveStockItem( ItemStock[i] );

                    return ItemStock.Remove( ItemStock[i] );
                }
            }

            return false;
        }

        public static bool StockContainsItem( string name )
        {
            for ( int i = 0; i < ItemStock.Count; i++ )
            {
                if ( ItemStock[i].Name == name )
                {
                    return true;
                }
            }

            return false;
        }

        public static bool StockContainsItem( long id )
        {
            for ( int i = 0; i < ItemStock.Count; i++ )
            {
                if ( ItemStock[i].GetID( ) == id )
                {
                    return true;
                }
            }

            return false;
        }
        
        public static ItemData RequestStockItem( string Name )
        {
            foreach( ItemData item in ItemStock )
            {
                if ( item.Name.Equals( Name ) )
                {
                    return item;
                }
            }

            return null;
        }
        
        internal void SetParentStorage( StorageData storage )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "ItemData", "SetParent" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return;
            }

            ParentStorage = storage;
        }

        protected virtual void OnChange( ItemData data )
        {
            base.OnChange( );
            ItemChanged?.Invoke( data );
        }

        private static void RemoveStockItem( ItemData data )
        {
            if ( data.ChildItems.Count == 0 )
            {
                data.Destroy( );

                return;
            }

            else
            {
                foreach( ItemData item in data.ChildItems )
                {
                    RemoveStockItem( item );
                    data.ChildItems.Remove( item );
                    item.Destroy( );
                }
            }
        }

        private static int GetStockCount( ItemData data )
        {
            int count = 0;

            if ( data.ChildItems.Count == 0 )
            {
                count = data.Count;
            }

            else
            {
                foreach ( ItemData item in data.ChildItems )
                {
                    count += GetStockCount( item );

                    count += item.Count;
                }
            }

            return count;
        } 
        
        private void UpdateItemData( ItemData changed, ItemData data )
        {
            if ( data == null )
            {
                if ( changed.ChildItems != null )
                {
                    foreach( ItemData item in changed.ChildItems )
                    {
                        UpdateItemData( changed, item );
                    }
                }

                else
                {
                    data.Name   = changed.Name;
                    data.Weight = changed.Weight;
                }
            }

            else
            {
                if ( data.ChildItems != null )
                {
                    foreach ( ItemData item in data.ChildItems )
                    {
                        UpdateItemData( changed, item );
                    }
                }

                else
                {
                    data.Name   = changed.Name;
                    data.Weight = changed.Weight;
                }
            }
        }

        private void ItemDataChanged( ItemData item )
        {
            if ( item.ParentItem != null )
            {
                ItemDataChanged( item.ParentItem );
            }

            UpdateItemData( item, null );
        }
    }
}