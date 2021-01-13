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
    /// <summary>
    /// Stellt ein Item auf einem Regal in der Umgebung dar.
    /// </summary>
    public class ItemData : GameObjectData
    {
        /// <summary>
        /// EventHandler für das StockChanged Event.
        /// </summary>
        /// <param name="item">Das betroffene Item.</param>
        public delegate void StockChangedEventHandler( ItemData item );

        /// <summary>
        /// EventHandler für das ItemChanged Event.
        /// </summary>
        /// <param name="item">Das betroffene Item.</param>
        public delegate void ItemChangedEventHandler( ItemData item );

        /// <summary>
        /// Wird ausgelöst wenn der Lagerbestand veändert wurde.
        /// </summary>
        public static event StockChangedEventHandler StockChanged;

        /// <summary>
        /// Wird ausgelöst wenn ein Item veändert wurde.
        /// </summary>
        public static event ItemChangedEventHandler ItemChanged;

        /// <summary>
        /// Die Anzahl des Items.
        /// </summary>
        public int Count { get; internal set; }

        /// <summary>
        /// Das Gewicht des Items.
        /// </summary>
        public double Weight { get; internal set; }

        /// <summary>
        /// Der Name des Items.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gibt an ob sich das Objekt in der Queue befindet.
        /// </summary>
        internal bool InQueue { get ; set; }

        /// <summary>
        /// Gibt die Position in der Queue an.
        /// </summary>
        internal int QueuePosition { get; set; }

        /// <summary>
        /// Das Regal auf dem das Item liegt.
        /// </summary>
        public StorageData ParentStorage { get; internal set; }

        /// <summary>
        /// Gibt an ob die Item Instanz die Wurzel des Baumes ist.
        /// </summary>
        internal bool IsRoot { get; set; }

        /// <summary>
        /// Das Item von dem dieses Item eine Teilmenge ist.
        /// </summary>
        internal ItemData ParentItem { get; private set; }

        /// <summary>
        /// Die Kinder des aktuellen Items.
        /// </summary>
        public List<ItemData> ChildItems { get; private set; }

        /// <summary>
        /// Die ID Referenz des Items.
        /// </summary>
        internal long IDRef { get; set; }

        /// <summary>
        /// Eine Liste mit dem Lagerbestand.
        /// </summary>
        internal static List<ItemData> ItemStock { get; set; }

        /// <summary>
        /// Gibt den Lagerbestand zurück.
        /// </summary>
        public static ItemData[] GetStock
        {
            get
            {
                return ItemStock.ToArray( );
            }
        }

        /// <summary>
        /// Gibt den kompletten Lagerbestand des Items zurück.
        /// </summary>
        public int StockCount
        {
            get
            {
                ItemData tmp = this;

                while ( tmp.ParentItem != null )
                {
                    tmp = tmp.ParentItem;
                }

                return tmp.Count;
            }
        }

        /// <summary>
        /// Gibt an ob das Objekt eine Wurzel im Baum ist.
        /// </summary>
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

        /// <summary>
        /// Erstellt den Lagerbestand.
        /// </summary>
        static ItemData()
        {
            ItemStock = new List<ItemData>( );
        }
        
        /// <summary>
        /// Zerstört das Objekt.
        /// </summary>
        ~ItemData()
        {
            Destroy( );
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        internal ItemData() : base( )
        {
            Count = 0;
            Weight = 0;
            Name = string.Empty;
            ParentStorage = null;
            ParentItem = null;
            ChildItems = new List<ItemData>( );
            QueuePosition = 0;
            InQueue = false;
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Items.</param>
        internal ItemData( long id ) : base( id )
        {
            Count = 0;
            Weight = 0;
            Name = string.Empty;
            ParentStorage = null;
            ParentItem = null;
            ChildItems = new List<ItemData>( );
            QueuePosition = 0;
            InQueue = false;
        }
        
        /// <summary>
        /// Erhöt die Anzahl des Items.
        /// </summary>
        /// <param name="itemCount">Die Anzahl um die die Teilmenge erhöt werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
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
                Count      += itemCount;
                ItemChanged?.Invoke(this);
                ItemChanged?.Invoke(data);
            }
                        
            return true;
        }

        /// <summary>
        /// Verringert die Anzahl des Items.
        /// </summary>
        /// <param name="itemCount">Die Anzahl um die die Teilmenge verringert werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
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
                Count      -= itemCount;
                ItemChanged?.Invoke(this);
                ItemChanged?.Invoke(data);
            }

            return true;
        }

        /// <summary>
        /// Ändert den Namen des Items.
        /// </summary>
        /// <param name="itemName">Der neue Name.</param>
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
            ItemChanged?.Invoke(this);

            ObjectChanged( );
        }

        /// <summary>
        /// Verändert das Gewicht des Items.
        /// </summary>
        /// <param name="itemWeight">Das neue Gewicht.</param>
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
            ItemChanged?.Invoke(this);

            ObjectChanged( );
        }
                
        /// <summary>
        /// Frägt eine Teilmenge aus dem Item ab.
        /// </summary>
        /// <param name="count">Die Anzahl der Teilmenge die Abgefragt werden soll.</param>
        /// <returns>Ein Item mit der Teilmenge.</returns>
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

            if ( ParentItem == null )
            {
                Count += count;
            }

            else
            {
                Count -= count;
            }

            ChildItems.Add( data );
            ItemChanged?.Invoke(data);
            ItemChanged?.Invoke(this);

            return data;
        }

        /// <summary>
        /// Frägt eine Teilmenge aus dem Item ab und klont das GameObject.
        /// </summary>
        /// <param name="count">Die Anzahl der Teilmenge die Abgefragt werden soll.</param>
        /// <returns>Ein Item mit der Teilmenge.</returns>
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

            if ( ParentItem == null )
            {
                Count += count;
            }

            else
            {
                Count -= count;
            }

            data.ChangeGameObject( GameObject.Instantiate( Object, Object.transform.parent ) );
            data.Object.transform.SetParent( GameObject.FindWithTag( "Player" ).transform );

            ChildItems.Add( data );

            ItemChanged?.Invoke(this);

            return data;
        }

        /// <summary>
        /// Frägt eine Teilmenge aus dem Item ab und klont das GameObject.
        /// </summary>
        /// <param name="count">Die Anzahl der Teilmenge die Abgefragt werden soll.</param>
        /// <param name="position">Die Position des geklonten Objekts.</param>
        /// <param name="rotation">Die Rotation des geklonten Objekts.</param>
        /// <returns>Ein Item mit der Teilmenge.</returns>
        public ItemData RequestCopyItem( int count, Vector3 position, Quaternion rotation )
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

            if ( ParentItem == null )
            {
                Count += count;
            }

            else
            {
                Count -= count;
            }

            data.ChangeGameObject( GameObject.Instantiate( Object, position, rotation, Object.transform.parent ) );

            ChildItems.Add( data );
            ItemChanged?.Invoke(this);

            return data;
        }

        /// <summary>
        /// Die Teilmenge wird wieder zum Parent hinzugefügt.
        /// </summary>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
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
            ItemChanged?.Invoke(ParentItem.ParentItem);

            Destroy( );

            return true;
        }

        /// <summary>
        /// Aendert den Status ob sich das Item in der Queue befindet.
        /// </summary>
        /// <param name="value">Der neue Wert.</param>
        public void SetQueueStatus( bool value )
        {
            InQueue = value;

            if ( ParentStorage != null )
            {
                if ( ParentStorage.IsContainer )
                {
                    foreach( ProjectStorageData storage in GameManager.GameContainer.Data.Container )
                    {
                        if ( storage.ID == ParentStorage.GetID() )
                        {
                            for( int i = 0; i < storage.Items.Length; i++ )
                            {
                                if ( storage.Items[i].ID == GetID() )
                                {
                                    storage.Items[i].InQueue = value;

                                    break;
                                }
                            }

                            break;
                        }
                    }
                }

                else
                {
                    foreach ( ProjectStorageData storage in GameManager.GameWarehouse.Data.StorageRacks )
                    {
                        if ( storage.ID == ParentStorage.GetID( ) )
                        {
                            for ( int i = 0; i < storage.Items.Length; i++ )
                            {
                                if ( storage.Items[i].ID == GetID( ) )
                                {
                                    storage.Items[i].InQueue = value;

                                    break;
                                }
                            }

                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Aendert die Queue Position.
        /// </summary>
        /// <param name="position">Die neue Position des Items.</param>
        public void SetQueuePosition( int position )
        {
            QueuePosition = position;

            if ( ParentStorage != null )
            {
                if ( ParentStorage.IsContainer )
                {
                    foreach ( ProjectStorageData storage in GameManager.GameContainer.Data.Container )
                    {
                        if ( storage.ID == ParentStorage.GetID( ) )
                        {
                            for ( int i = 0; i < storage.Items.Length; i++ )
                            {
                                if ( storage.Items[i].ID == GetID( ) )
                                {
                                    storage.Items[i].QueuePosition = position;

                                    break;
                                }
                            }

                            break;
                        }
                    }
                }

                else
                {
                    foreach ( ProjectStorageData storage in GameManager.GameWarehouse.Data.StorageRacks )
                    {
                        if ( storage.ID == ParentStorage.GetID( ) )
                        {
                            for ( int i = 0; i < storage.Items.Length; i++ )
                            {
                                if ( storage.Items[i].ID == GetID( ) )
                                {
                                    storage.Items[i].QueuePosition = position;

                                    break;
                                }
                            }

                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Fügt ein Item zum Lagerbestand hinzu.
        /// </summary>
        /// <param name="name">Der Name des Items.</param>
        /// <param name="weight">Das Gewicht des Items.</param>
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

        /// <summary>
        /// Fügt ein Item zum Lagerbestand hinzu.
        /// </summary>
        /// <param name="id">Die ID des Items.</param>
        /// <param name="name">Der Name des Items.</param>
        /// <param name="count">Die Anzahl des Items.</param>
        /// <param name="weight">Das Gewicht des Items.</param>
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

        /// <summary>
        /// Entfernt das Item vom Lagerbestand.
        /// </summary>
        /// <param name="item">Das Item das Entfernt werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
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

        /// <summary>
        /// Entfernt das Item vom Lagerbestand.
        /// </summary>
        /// <param name="idRef">Die ID des Items das entfernt werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
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

        /// <summary>
        /// Entfernt das Item vom Lagerbestand.
        /// </summary>
        /// <param name="name">Der Name des Items das Entfernt werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
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

        /// <summary>
        /// Überprüft ob der Lagerbestand ein Item enthält.
        /// </summary>
        /// <param name="name">Der Name des Items das gesucht werden soll.</param>
        /// <returns>Gibt true zurück wenn das Item existiert.</returns>
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

        /// <summary>
        /// Überprüft ob der Lagerbestand ein Item enthält.
        /// </summary>
        /// <param name="idRef">Die IDReferenz des Items das gesucht werden soll.</param>
        /// <returns>Gibt true zurück wenn das Item existiert.</returns>
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
        
        /// <summary>
        /// Frägt ein Item aus dem Lagerbestand ab.
        /// </summary>
        /// <param name="Name">Der Name des Items das abgefragt werden soll.</param>
        /// <returns>Gibt das Item zurück oder null.</returns>
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
        
        /// <summary>
        /// Ändert das Eltern Regal dem das Item zugewiesen ist.
        /// </summary>
        /// <param name="storage">Das Regal das als Parent gesetzt werden soll.</param>
        internal void SetParentStorage( StorageData storage )
        {
            if ( IsDestroyed() )
            {
                return;
            }

            ParentStorage = storage;
        }

        /// <summary>
        /// Entfernt ein Item aus dem Lagerbestand rekursiv.
        /// </summary>
        /// <param name="data">Das Item das entfernt werden soll.</param>
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
        
        /// <summary>
        /// Wird aufgerufen wenn ein Item geändert wurde und ändert den kompletten hierarchie Baum rekursiv ab.
        /// </summary>
        /// <param name="src">Das Item mit den änderungen.</param>
        /// <param name="item">Das Item das geändert wird.</param>
        private void ObjectChanged( ItemData src, ItemData item )
        {
            if ( item.ChildItems.Count == 0 )
            {
                item.Name   = src.Name;
                item.Weight = src.Weight;

                if ( item.ParentStorage != null )
                {
                    if ( item.ParentStorage.IsContainer )
                    {
                        for ( int i = 0; i < GameManager.GameContainer.Data.Container.Count; i++ )
                        {
                            if ( item.ParentStorage.GetID( ) == GameManager.GameContainer.Data.Container[i].ID )
                            {
                                GameManager.GameContainer.Data.Container.Remove( GameManager.GameContainer.Data.Container[i] );

                                ProjectStorageData storage = new ProjectStorageData( GetID(), item.ParentStorage.SlotCount, new ProjectTransformationData( item.ParentStorage.Position, item.ParentStorage.Rotation, item.ParentStorage.Scale ) );

                                for ( int j = 0; j < item.ParentStorage.GetItems.Length; j++ )
                                {
                                    if ( item.ParentStorage.GetItems[j] != null )
                                    {
                                        storage.Items[j] = new ProjectItemData( item.IDRef, item.GetID( ), item.Count, item.Weight, item.Name, item.InQueue, item.QueuePosition, new ProjectTransformationData( item.Position, item.Rotation, item.Scale ) );
                                    }
                                }

                                GameManager.GameContainer.Data.Container.Insert( i, storage );

                                break;
                            }
                        }
                    }

                    else
                    {
                        for ( int i = 0; i < GameManager.GameWarehouse.Data.StorageRacks.Count; i++ )
                        {
                            if ( item.ParentStorage.GetID( ) == GameManager.GameWarehouse.Data.StorageRacks[i].ID )
                            {
                                GameManager.GameWarehouse.Data.StorageRacks.Remove( GameManager.GameWarehouse.Data.StorageRacks[i] );

                                ProjectStorageData storage = new ProjectStorageData( GetID(), item.ParentStorage.SlotCount, new ProjectTransformationData( item.ParentStorage.Position, item.ParentStorage.Rotation, item.ParentStorage.Scale ) );

                                for ( int j = 0; j < item.ParentStorage.GetItems.Length; j++ )
                                {
                                    if ( item.ParentStorage.GetItems[j] != null )
                                    {
                                        storage.Items[j] = new ProjectItemData( item.IDRef, item.GetID( ), item.Count, item.Weight, item.Name, item.InQueue, item.QueuePosition, new ProjectTransformationData( item.Position, item.Rotation, item.Scale ) );
                                    }
                                }

                                GameManager.GameWarehouse.Data.StorageRacks.Insert( i, storage );

                                break;
                            }
                        }
                    }
                }
            }

            else
            {
                foreach( ItemData child in item.ChildItems )
                {
                    ObjectChanged( src, child );
                }
            }
        }

        /// <summary>
        /// Wird aufgerufen wenn ein Item geändert wird.
        /// </summary>
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