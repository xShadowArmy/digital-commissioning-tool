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
    public class Container
    {
        public delegate void ContainerChangedEventHandler( StorageData storage );

        public event ContainerChangedEventHandler ContainerCreated;

        public event ContainerChangedEventHandler ContainerDeleted;

        public List<StorageData> ContainerData { get; private set; }

        internal InternalProjectContainer Data { get; private set; }

        public Container()
        {
            ContainerData = new List<StorageData>( );
            Data = new InternalProjectContainer( );
        }

        public StorageData CreateContainer( )
        {
            LogManager.WriteInfo( "Mobiles Regal wird erstellt.", "ContainerData", "CreateContainer" );
            
            StorageData container = new StorageData( Warehouse.GetUniqueID( ContainerData.ToArray( ) ), GameManager.GameWarehouse.ObjectSpawn.transform.position, GameManager.GameWarehouse.ObjectSpawn.transform.rotation, GameManager.GameWarehouse.ObjectSpawn.transform.localScale );
            
            container.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableStorage" ), GameManager.GameWarehouse.ObjectSpawn.transform.position, GameManager.GameWarehouse.ObjectSpawn.transform.rotation, GameObject.FindGameObjectWithTag( "StorageRackDefinition" ).transform ) );
            
            container.GameObjectDataChanged += GameObjectHasChanged;
            container.StorageChanged += ContainerHasChanged;

            container.Object.name = "Container" + container.GetID( );

            ContainerData.Add( container );

            Data.Container.Add( new ProjectStorageData( container.GetID( ), container.SlotCount, new ProjectTransformationData( GameManager.GameWarehouse.ObjectSpawn.transform.position, GameManager.GameWarehouse.ObjectSpawn.transform.rotation, GameManager.GameWarehouse.ObjectSpawn.transform.localScale ) ) );

            OnSContainerCreated( container );
            
            return container;
        }
        
        public StorageData CreateContainer( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Mobiles Regal wird erstellt.", "ContainerData", "CreateContainer" );

            StorageData container = new StorageData( Warehouse.GetUniqueID( ContainerData.ToArray( ) ), position, rotation, scale );

            container.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableStorage" ), position, rotation, GameObject.FindGameObjectWithTag( "StorageRackDefinition" ).transform ) );

            container.GameObjectDataChanged += GameObjectHasChanged;
            container.StorageChanged += ContainerHasChanged;

            container.Object.name = "Container" + container.GetID( );

            ContainerData.Add( container );

            Data.Container.Add( new ProjectStorageData( container.GetID( ), container.SlotCount, new ProjectTransformationData( position, rotation, scale ) ) );

            OnSContainerCreated( container );

            return container;
        }

        internal void AddContainer( StorageData container )
        {
            LogManager.WriteInfo( "Mobiles Regal wird hinzugefuegt.", "Warehouse", "AddContainer" );

            container.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableStorage" ), GameManager.GameWarehouse.ObjectSpawn.transform.position, GameManager.GameWarehouse.ObjectSpawn.transform.rotation, GameObject.FindGameObjectWithTag( "StorageRackDefinition" ).transform ) );

            container.GameObjectDataChanged += GameObjectHasChanged;
            container.StorageChanged += ContainerHasChanged;

            container.Object.name = "Container" + container.GetID( );

            ContainerData.Add( container );

            Data.Container.Add( new ProjectStorageData( container.GetID( ), container.SlotCount, new ProjectTransformationData( GameManager.GameWarehouse.ObjectSpawn.transform.position, GameManager.GameWarehouse.ObjectSpawn.transform.rotation, GameManager.GameWarehouse.ObjectSpawn.transform.localScale ) ) );

            OnSContainerCreated( container );
        }

        public bool RemoveContainer( StorageData container )
        {
            LogManager.WriteInfo( "Mobiles Regal wird entfernt.", "ContainerData", "RemoveContainer" );

            if ( !ContainerData.Remove( container ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.Container.Count; i++ )
            {
                if ( Data.Container[ i ].ID == container.GetID( ) )
                {
                    container.GameObjectDataChanged -= GameObjectHasChanged;
                    container.StorageChanged -= ContainerHasChanged;

                    Data.Container.Remove( Data.Container[ i ] );
                                        
                    OnContainerDeleted( container );

                    container.Destroy( );

                    return true;
                }
            }

            return false;
        }

        public StorageData GetContainer( long id )
        {
            LogManager.WriteInfo( "Mobiles Regal wird abgefragt.", "ContainerData", "GetContainer" );

            for ( int i = 0; i < ContainerData.Count; i++ )
            {
                if ( ContainerData[ i ].GetID( ) == id )
                {
                    return ContainerData[ i ];
                }
            }

            return null;
        }

        public StorageData GetContainer( GameObject obj )
        {
            LogManager.WriteInfo( "Mobiles Regal wird abgefragt.", "ContainerData", "GetContainer" );

            for ( int i = 0; i < ContainerData.Count; i++ )
            {
                if ( ContainerData[ i ].Object == obj )
                {
                    return ContainerData[ i ];
                }
            }

            return null;
        }
        
        public void AddItemToContainer( StorageData container, ItemData item, int slot )
        {
            LogManager.WriteInfo( "Ein ContainerItem wird erstellt.", "ContainerData", "AddItemToContainer" );

            if ( container.Slots[slot] != null )
            {
                if ( container.RemoveItem( container.GetItems[slot] ) )
                {
                    container.GetItems[slot].ReturnItem( );
                }
            }

            container.AddItem( item, slot );
            
            item.GameObjectDataChanged += GameObjectHasChanged;
            item.ItemChanged += ContainerItemHasChanged;

            foreach ( ProjectStorageData data in Data.Container )
            {
                if ( data.ID == container.GetID( ) )
                {
                    data.Items.Add( new ProjectItemData( item.GetID( ), item.Count, item.Weight, item.Name, new ProjectTransformationData( item.Position, item.Rotation, item.Scale ) ) );
                }
            }
        }

        public bool RemoveItemFromContainer( StorageData container, ItemData item )
        {
            LogManager.WriteInfo( "Ein ContainerItem wird entfernt.", "ContainerData", "RemoveItemFromContainer" );

            if ( container.GetItems[0] == null )
            {
                return false;
            }

            if ( container.RemoveItem( item ) )
            {
                item.ReturnItem( );
                
                item.GameObjectDataChanged -= GameObjectHasChanged;
                item.ItemChanged -= ContainerItemHasChanged;

                foreach ( ProjectStorageData data in Data.Container )
                {
                    if ( data.ID == container.GetID( ) )
                    {
                        foreach( ProjectItemData idata in data.Items )
                        {
                            if ( idata.IDRef == item.GetID() )
                            {
                                return data.Items.Remove( idata );
                            }
                        }
                    }
                }

                return false;
            }

            return false;
        }
        
        private void GameObjectHasChanged( GameObjectData obj, GameObjectDataType type )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere GameObjectData. Type=" + type.ToString( ), "Container", "GameObjectHasChanged" );

            switch ( type )
            {
                case GameObjectDataType.StorageReck:

                    ContainerHasChanged( obj as StorageData );
                    break;

                case GameObjectDataType.Item:

                    ContainerItemHasChanged( obj as ItemData );
                    break;

                default:

                    LogManager.WriteWarning( "[Event] Falscher Typ in EventSystem referenziert!", "Container", "GameObjectHasChanged" );
                    break;
            }
        }

        private void ContainerHasChanged( StorageData storage )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere StorageData.", "Warehouse", "StorageRackHasChanged" );

            for ( int i = 0; i < Data.Container.Count; i++ )
            {
                if ( storage.GetID( ) == Data.Container[ i ].ID )
                {
                    if ( storage.GetItems.Length > Data.Container[ i ].GetItems.Length )
                    {
                        ProjectTransformationData data = new ProjectTransformationData( storage.GetItems[ storage.GetItems.Length - 1 ].Position,
                                                                                        storage.GetItems[ storage.GetItems.Length - 1 ].Rotation,
                                                                                        storage.GetItems[ storage.GetItems.Length - 1 ].Scale );

                        Data.Container[i].Items.Add( new ProjectItemData( storage.GetItems[storage.GetItems.Length - 1].GetID( ),
                                                       storage.GetItems[storage.GetItems.Length - 1].Count,
                                                       storage.GetItems[storage.GetItems.Length - 1].Weight,
                                                       storage.GetItems[storage.GetItems.Length - 1].Name,
                                                       data ) );

                        break;
                    }

                    else if ( storage.GetItems.Length < Data.Container[ i ].GetItems.Length )
                    {
                        if ( storage.GetItems.Length == 1 )
                        {
                            Data.Container[ i ].Items.Remove( Data.Container[ i ].GetItems[ 0 ] );

                            break;
                        }

                        for ( int j = 0; j < Data.Container[ i ].GetItems.Length; j++ )
                        {
                            if ( Data.Container[ i ].GetItems[ j ].IDRef != storage.GetItems[ j ].GetID( ) )
                            {
                                Data.Container[ i ].Items.Remove( Data.Container[ i ].GetItems[ j ] );

                                break;
                            }
                        }

                        break;
                    }

                    else
                    {
                        ProjectItemData[ ] items = Data.Container[ i ].GetItems;
                        ProjectStorageData data = new ProjectStorageData( storage.GetID( ), Data.Container[i].SlotCount, new ProjectTransformationData( storage.Position, storage.Rotation, storage.Scale ) );

                        foreach ( ProjectItemData item in items )
                        {
                            data.Items.Add( item );
                        }

                        Data.Container.Remove( Data.Container[ i ] );
                        Data.Container.Insert( i, data );

                        break;
                    }
                }
            }
        }

        private void ContainerItemHasChanged( ItemData item )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere ItemData.", "Warehouse", "StorageRackItemHasChanged" );
            
        }  
        
        protected virtual void OnSContainerCreated( StorageData data )
        {
            ContainerCreated?.Invoke( data );
        }

        protected virtual void OnContainerDeleted( StorageData data )
        {
            ContainerDeleted?.Invoke( data );
        }
    }
}
