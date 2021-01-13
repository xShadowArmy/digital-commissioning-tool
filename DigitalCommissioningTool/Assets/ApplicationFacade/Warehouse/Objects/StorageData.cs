using System;
using System.Collections.Generic;
using AppData.Warehouse;
using ApplicationFacade.Application;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ApplicationFacade.Warehouse
{
    /// <summary>
    /// Repräsentiert ein Regal in der Umgebung.
    /// </summary>
    public class StorageData : GameObjectData
    {
        /// <summary>
        /// Gibt an ob das Regal ein Container ist.
        /// </summary>
        public bool IsContainer { get; internal set; }

        /// <summary>
        /// Die Anzahl der Slots.
        /// </summary>
        public int SlotCount { get; internal set; }

        /// <summary>
        /// Die Items von dem Container.
        /// </summary>
        private ItemData[] Data { get; set; }

        /// <summary>
        /// Die Kisten Objekte von dem Regal.
        /// </summary>
        private GameObject[] Slots { get; set; }

        /// <summary>
        /// Gibt die Items von dem Regal zurück.
        /// </summary>
        public ItemData[] GetItems
        {
            get
            {
                return Data;
            }
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        internal StorageData( ) : base( )
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Regals.</param>
        internal StorageData( long id ) : base( id )
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Regals.</param>
        /// <param name="position">Die Position des Regals.</param>
        /// <param name="rotation">Die Rotation des Regals.</param>
        /// <param name="scale">Die Skalierung des Regals.</param>
        internal StorageData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( id, position, rotation, scale )
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Regals.</param>
        /// <param name="position">Die Position des Regals.</param>
        /// <param name="rotation">Die Rotation des Regals.</param>
        /// <param name="scale">Die Skalierung des Regals.</param>
        /// <param name="obj">Das GameObject des Regals.</param>
        internal StorageData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
        }

        /// <summary>
        /// Gibt das Item aus dem angegebenen Slot zurück.
        /// </summary>
        /// <param name="slot">Der Slot des Items.</param>
        /// <returns>Gibt das Item zurück.</returns>
        public GameObject GetItem( int slot = -1 )
        {
            if ( IsDestroyed( ) )
            {
                return null;
            }

            if ( slot >= Slots.Length )
            {
                LogManager.WriteWarning( "Ein Objekt soll aus einem Slot abgefragt werden der nicht existiert!", "StorageData", "AddItem" );

                return null;
            }

            if ( slot <= -1 )
            {
                for( int i = 0; i < Data.Length; i++ )
                {
                    if ( Data[i] == null )
                    {
                        return Slots[i];
                    }
                }

                return null;
            }

            return Slots[slot];
        }

        /// <summary>
        /// Gibt das Item zurück mit der angegebenen ID.
        /// </summary>
        /// <param name="id">Die ID des Items.</param>
        /// <returns>Gibt das Item mit der passenden ID zurück.</returns>
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

        /// <summary>
        /// Gibt das Item zurück mit dem angegebenen GameObject.
        /// </summary>
        /// <param name="obj">Das GameObject nach dem gesucht werden soll.</param>
        /// <returns>Gibt das Item mit dem passenden GameObject zurück.</returns>
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

        /// <summary>
        /// Gibt den Slot zurück auf dem das Item mit dem passenden GameObject abgelegt wurde.
        /// </summary>
        /// <param name="obj">Das GameObject das gesucht werden soll.</param>
        /// <returns>Gibt die Nummer des Slots zurück.</returns>
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

        /// <summary>
        /// Gibt den Slot zurück auf dem das Item abgelegt wurde.
        /// </summary>
        /// <param name="item">Das Item dessen Slot gesucht werden soll.</param>
        /// <returns>Gibt die Nummer des Slots zurück.</returns>
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

        /// <summary>
        /// Legt ein Item auf das Regal.
        /// </summary>
        /// <param name="item">Das Item das auf das Regal gelegt werden soll.</param>
        /// <param name="slot">Der Slot auf dem das Item abgelegt werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
        public bool AddItem( ItemData item, int slot = -1 )
        {
            LogManager.WriteInfo( "Ein RegalItem wird hinzugefuegt.", "Warehouse", "AddItemToStorageRack" );

            if ( IsDestroyed( ) )
            {
                return false;
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
                    return false;
                }
            }

            else
            {
                if ( slot >= Slots.Length )
                {
                    LogManager.WriteWarning( "Ein Objekt soll auf ein Slot abgelegt werden der nicht existiert!", "StorageData", "AddItem" );

                    return false;
                }
            }

            if ( item.Object != null )
            {
                GameObject.Destroy( item.Object );
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
                    data.Items[slot] = new ProjectItemData( item.IDRef, item.GetID(), item.Count, item.Weight, item.Name, item.InQueue, item.QueuePosition, new ProjectTransformationData( item.Position, item.Rotation, item.Scale ) );

                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// Legt ein Item auf das Regal.
        /// </summary>
        /// <param name="item">Das Item das auf das Regal gelegt werden soll.</param>
        /// <param name="warehouse">Das Lager in dem die änderungen gespeichert werden sollen.</param>
        /// <param name="slot">Der Slot auf dem das Item gelegt wird.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
        internal bool AddItem( ItemData item, ref Warehouse warehouse, int slot = -1 )
        {
            LogManager.WriteInfo( "Ein RegalItem wird hinzugefuegt.", "Warehouse", "AddItemToStorageRack" );

            if ( IsDestroyed( ) )
            {
                return false;
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
                    return false;
                }
            }

            else
            {
                if ( slot >= Slots.Length )
                {
                    LogManager.WriteWarning( "Ein Objekt soll auf ein Slot abgelegt werden der nicht existiert!", "StorageData", "AddItem" );

                    return false;
                }
            }

            if ( item.Object != null )
            {
                GameObject.Destroy( item.Object );
            }

            Data[slot] = item;

            item.SetID( Warehouse.GetUniqueID( Data ) );

            item.ChangeGameObject( Slots[slot] );
            item.Object.name = item.Name;
            item.ParentStorage = this;

            Data[slot].Object.SetActive( true );

            Data[slot].Position = Slots[slot].transform.position;
            Data[slot].Rotation = Slots[slot].transform.rotation;
            Data[slot].Scale = Slots[slot].transform.localScale;

            foreach ( ProjectStorageData data in warehouse.Data.StorageRacks )
            {
                if ( data.ID == GetID( ) )
                {
                    data.Items[slot] = new ProjectItemData( item.IDRef, item.GetID( ), item.Count, item.Weight, item.Name, item.InQueue, item.QueuePosition, new ProjectTransformationData( item.Position, item.Rotation, item.Scale ) );

                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// Entfernt ein Item von dem Regal.
        /// </summary>
        /// <param name="item">Das Item das Entfernt werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
        public bool RemoveItem( ItemData item )
        {
            if ( IsDestroyed( ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.Length; i++ )
            {
                if ( Data[i] != null && Data[i].GetID() == item.GetID() )
                {
                    item.ChangeGameObject( null );

                    Slots[i].SetActive( false );

                    Data[i] = null;

                    if ( item.ParentItem != null )
                    {
                        if ( item.ParentItem.IsRoot )
                        {
                            item.DecreaseItemCount( 1 );
                        }

                        else
                        {
                            item.IncreaseItemCount( 1 );
                        }
                    }

                    item.ParentStorage = null;

                    foreach ( ProjectStorageData data in GameManager.GameWarehouse.Data.StorageRacks )
                    {
                        if ( data.ID == GetID( ) )
                        {
                            for( int j = 0; j < data.Items.Length; j++ )
                            {
                                if ( data.Items[j] != null && data.Items[j].ID == item.GetID() )
                                {
                                    data.Items[j] = null;

                                    break;
                                }
                            }

                            break;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Überprüft ob ein Item auf dem Regal liegt.
        /// </summary>
        /// <param name="item">Das Item nach dem gesucht werden soll.</param>
        /// <returns>Gibt true zurück wenn das Regal das Item enthält.</returns>
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

        /// <summary>
        /// Zerstört das Objekt.
        /// </summary>
        public new void Destroy()
        {
            base.Destroy( );

            foreach( ItemData item in Data )
            {
                if ( item != null )
                {
                    item.ReturnItem( );
                }
            }
        }

        /// <summary>
        /// Ändert die Anzahl der Slots.
        /// </summary>
        /// <param name="strategie">Der Algorithmus der für die Berechnung der Slots verwendet werden soll.</param>
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

        /// <summary>
        /// Wird aufgerufen wenn das Objekt verändert wurde um die Änderungen zu Speichern.
        /// </summary>
        protected override void ObjectChanged()
        {
            if ( !IsContainer )
            {
                for ( int i = 0; i < GameManager.GameWarehouse.Data.StorageRacks.Count; i++ )
                {
                    if ( GameManager.GameWarehouse.Data.StorageRacks[i].ID == GetID( ) )
                    {
                        GameManager.GameWarehouse.Data.StorageRacks.Remove( GameManager.GameWarehouse.Data.StorageRacks[i] );

                        ProjectStorageData storage = new ProjectStorageData( GetID(), SlotCount, new ProjectTransformationData( Position, Rotation, Scale ) );

                        for ( int j = 0; j < Data.Length; j++ )
                        {
                            if ( Data[j] != null )
                            {
                                storage.Items[j] = new ProjectItemData( Data[j].IDRef, Data[j].GetID( ), Data[j].Count, Data[j].Weight, Data[j].Name, Data[j].InQueue, Data[j].QueuePosition, new ProjectTransformationData( Data[j].Position, Data[j].Rotation, Data[j].Scale ) );
                            }
                        }

                        GameManager.GameWarehouse.Data.StorageRacks.Insert( i, storage );

                        break;
                    }
                }
            }

            else
            {
                for ( int i = 0; i < GameManager.GameContainer.Data.Container.Count; i++ )
                {
                    if ( GameManager.GameContainer.Data.Container[i].ID == GetID( ) )
                    {
                        GameManager.GameContainer.Data.Container.Remove( GameManager.GameContainer.Data.Container[i] );

                        ProjectStorageData storage = new ProjectStorageData( GetID(), SlotCount, new ProjectTransformationData( Position, Rotation, Scale ) );

                        for ( int j = 0; j < Data.Length; j++ )
                        {
                            if ( Data[j] != null )
                            {
                                storage.Items[j] = new ProjectItemData( Data[j].IDRef, Data[j].GetID( ), Data[j].Count, Data[j].Weight, Data[j].Name, Data[j].InQueue, Data[j].QueuePosition, new ProjectTransformationData( Data[j].Position, Data[j].Rotation, Data[j].Scale ) );
                            }
                        }

                        GameManager.GameContainer.Data.Container.Insert( i, storage );

                        break;
                    }
                }
            }
        }
    }
}
