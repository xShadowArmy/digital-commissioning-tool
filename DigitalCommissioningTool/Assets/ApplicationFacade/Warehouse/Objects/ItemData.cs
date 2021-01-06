using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationFacade.Application;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ApplicationFacade.Warehouse
{
    public class ItemData : GameObjectData
    {
        public delegate void StockChangedEventHandler( ItemData item );

        public static event StockChangedEventHandler StockChanged;

        public int Count { get; internal set; }

        public double Weight { get; internal set; }

        public string Name { get; internal set; }

        public StorageData ParentStorage { get; internal set; }

        internal bool IsRoot { get; set; }

        internal ItemData ParentItem { get; private set; }

        internal List<ItemData> ChildItems { get; private set; }

        internal long IDRef { get; set; }

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
                ItemData tmp = this;

                while( tmp.ParentItem != null )
                {
                    tmp = tmp.ParentItem;
                }

                return tmp.Count;
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

        internal ItemData() : base( )
        {
            Count = 0;
            Weight = 0;
            Name = string.Empty;
            ParentStorage = null;
            ParentItem = null;
            ChildItems = new List<ItemData>( );
        }

        internal ItemData( long id ) : base( id )
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

            if ( ParentItem != null )
            {
                ItemData data = this;

                while( data.ParentItem != null )
                {
                    data = data.ParentItem;
                }

                data.Count += itemCount;
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

            if ( ParentItem != null )
            {
                ItemData data = this;

                while ( data.ParentItem != null )
                {
                    data = data.ParentItem;
                }

                data.Count -= itemCount;
            }

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

            ObjectChanged( );
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

            ObjectChanged( );
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

            ItemData data = new ItemData(  )
            {
                Count = count,
                Name = Name,
                Weight = Weight,
                ParentItem = this,
                IDRef = IDRef
            };

            Count += count;
            ChildItems.Add( data );

            return data;
        }

        public ItemData RequestCopyItem( int count )
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
                return null;
            }

            ItemData data = new ItemData(  )
            {
                Count = count,
                Name = Name,
                Weight = Weight,
                ParentItem = this,
                IDRef = IDRef
            };

            Count -= count;

            data.ChangeGameObject( GameObject.Instantiate( Object ) );

            ChildItems.Add( data );

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

            if ( ParentItem.ParentItem == null )
            {
                ParentItem.ParentItem.Count -= Count;
            }

            else
            {
                ParentItem.ParentItem.Count += Count;
            }

            Destroy( );

            return true;
        }

        public static void AddItemToStock( string name, double weight = 0 )
        {
            ItemData item = new ItemData( Warehouse.GetUniqueID( ItemStock.ToArray( ) ) )
            {
                Name   = name,
                Count  = 0,
                Weight = weight
            };

            item.IDRef = item.ID;

            ItemStock.Add( item );

            StockChanged?.Invoke( item );
        }
        
        internal static void AddItemToStock( long id, string name, int count = 1, double weight = 0 )
        {
            ItemData item = new ItemData( id )
            {
                Name = name,
                Count = count,
                Weight = weight,
            };

            item.IDRef = id;

            ItemStock.Add( item );

            StockChanged?.Invoke( item );
        }

        public static bool RemoveItemFromStock( ItemData item )
        {
            if ( !item.IsStockItem )
            {
                return false;
            }

            RemoveStockItem( item );

            bool res = ItemStock.Remove( item );

            StockChanged?.Invoke( item );

            return res;
        }

        public static bool RemoveItemFromStock( long idRef )
        {
            for ( int i = 0; i < ItemStock.Count; i++ )
            {
                if ( ItemStock[i].IDRef == idRef )
                {
                    RemoveStockItem( ItemStock[i] );

                    bool res = ItemStock.Remove( ItemStock[i] );

                    StockChanged?.Invoke( ItemStock[i] );

                    return res;
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

                    bool res = ItemStock.Remove( ItemStock[i] );

                    StockChanged?.Invoke( ItemStock[i] );

                    return res;
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

        public static bool StockContainsItem( long idRef )
        {
            for ( int i = 0; i < ItemStock.Count; i++ )
            {
                if ( ItemStock[i].IDRef == idRef )
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
            if ( IsDestroyed() )
            {
                return;
            }

            ParentStorage = storage;
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
        
        private void ObjectChanged( ItemData src, ItemData item )
        {
            if ( item.ChildItems.Count == 0 )
            {
                item.Name   = src.Name;
                item.Weight = src.Weight;
            }

            else
            {
                foreach( ItemData child in item.ChildItems )
                {
                    ObjectChanged( src, child );
                }
            }
        }

        protected override void ObjectChanged()
        {
            ItemData tmp = this;

            while ( tmp.ParentItem != null )
            {
                tmp = tmp.ParentItem;
            }

            ObjectChanged( this, tmp );
        }
    }
}