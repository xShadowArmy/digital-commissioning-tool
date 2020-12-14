using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using SystemFacade;
using SystemTools;
using UnityEngine;

namespace ApplicationFacade
{
    public class ItemData : GameObjectData, ISerialConfigData
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
                if ( ChildItems.Count != 0 )
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
        
        public void SetItemCount( int itemCount )
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

        public void SetItemWeight( double itemWeight )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "ItemData", "SetItemName" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return;
            }

            Weight = itemWeight;
            OnChange( );
        }

        public void Serialize( SerialConfigData storage )
        {
            storage.AddData( Name );
            storage.AddData( Weight );
            storage.AddData( Count );
        }

        public void Restore( SerialConfigData storage )
        {
            Name = storage.GetValueAsString( );
            Weight = storage.GetValueAsDouble( );
            Count = storage.GetValueAsInt( );
        }

        public static ItemData AddItemToStock( string name, int count = 1, double weight = 0 )
        {
            ItemData item = new ItemData( Warehouse.GetUniqueID( ItemStock.ToArray( ) ) )
            {
                Name = name,
                Count = count,
                Weight = weight,
            };

            ItemStock.Add( item );

            return item;
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

        public static ItemData RequestStockItem( string name, int count )
        {
            for ( int i = 0; i < ItemStock.Count; i++ )
            {
                if ( ItemStock[i].Name == name )
                {
                    if ( count > ItemStock[i].Count )
                    {
                        LogManager.WriteWarning( "Es werden mehr kopien eines Objekts angefordert als vorhanden sind!", "ItemData", "GetStockItem" );
                        Debug.LogWarning( "Es werden mehr kopien eines Objekts angefordert als vorhanden sind!" );

                        return null;
                    }

                    ItemData data = new ItemData( ItemStock[i].GetID( ) )
                    {
                        Count = count,
                        Name = name,
                        Weight = ItemStock[i].Weight,
                        ParentItem = ItemStock[i]
                    };

                    ItemStock[i].Count -= count;
                    ItemStock[i].ChildItems.Add( data );

                    data.ItemChanged += ItemDataChanged;

                    return data;
                }
            }

            return null;
        }

        public static ItemData RequestStockItem( long id, int count )
        {
            for ( int i = 0; i < ItemStock.Count; i++ )
            {
                if ( count > ItemStock[i].Count )
                {
                    LogManager.WriteWarning( "Es werden mehr kopien eines Objekts angefordert als vorhanden sind!", "ItemData", "GetStockItem" );
                    Debug.LogWarning( "Es werden mehr kopien eines Objekts angefordert als vorhanden sind!" );

                    return null;
                }

                ItemData data = new ItemData( ItemStock[i].GetID( ) )
                {
                    Count = count,
                    Name = ItemStock[i].Name,
                    Weight = ItemStock[i].Weight,
                    ParentItem = ItemStock[i]
                };

                ItemStock[i].Count -= count;
                ItemStock[i].ChildItems.Add( data );

                data.ItemChanged += ItemDataChanged;
            }

            return null;
        }

        public static bool ReturnStockItem( ItemData item )
        {
            if ( item.ParentItem != null )
            {
                return false;
            }

            int cnt = GetStockCount( item );

            item.Count = cnt;

            foreach( ItemData data in item.ChildItems )
            {
                RemoveStockItem( data );
                item.ChildItems.Remove( data );
            }

            return true;
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
        
        internal void SetParent( StorageData storage )
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

        private static void ItemDataChanged( ItemData item )
        {

        }
    }
}