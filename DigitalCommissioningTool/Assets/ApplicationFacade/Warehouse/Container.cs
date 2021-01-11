using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Warehouse;
using ApplicationFacade.Application;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ApplicationFacade.Warehouse
{
    public class Container
    {
        public delegate void ContainerChangedEventHandler( StorageData storage );

        public event ContainerChangedEventHandler ContainerCreated;

        public event ContainerChangedEventHandler ContainerDeleted;

        public List<StorageData> ContainerData { get; private set; }

        internal InternalProjectContainer Data { get; private set; }

        /// <summary>
        /// Gibt alle mobilen Regale zurueck.
        /// </summary>
        public StorageData[] StorageRacks
        {
            get
            {
                return ContainerData.ToArray( );
            }
        }

        public Container()
        {
            ContainerData = new List<StorageData>( );
            Data = new InternalProjectContainer( );
        }

        ~Container()
        {
            DestroyContainer( );
        }

        public StorageData CreateContainer( )
        {
            LogManager.WriteInfo( "Mobiles Regal wird erstellt.", "ContainerData", "CreateContainer" );
            
            StorageData container = new StorageData( Warehouse.GetUniqueID( ContainerData.ToArray( ) ), GameManager.GameWarehouse.ObjectSpawn.transform.position, GameManager.GameWarehouse.ObjectSpawn.transform.rotation, GameManager.GameWarehouse.ObjectSpawn.transform.localScale )
            {
                IsContainer = true
            };
            
            container.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableContainer" ), GameManager.GameWarehouse.ObjectSpawn.transform.position, GameManager.GameWarehouse.ObjectSpawn.transform.rotation, GameObject.FindGameObjectWithTag( "ContainerDefinition" ).transform ) );
            
            container.Object.name = "Container" + container.GetID( );

            ContainerData.Add( container );

            container.ChangeSlotCount( new StorageSlotCalculation() );

            Data.Container.Add( new ProjectStorageData( container.GetID( ), container.GetItems.Length, new ProjectTransformationData( GameManager.GameWarehouse.ObjectSpawn.transform.position, GameManager.GameWarehouse.ObjectSpawn.transform.rotation, GameManager.GameWarehouse.ObjectSpawn.transform.localScale ) ) );

            OnSContainerCreated( container );
            
            return container;
        }
        
        public StorageData CreateContainer( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Mobiles Regal wird erstellt.", "ContainerData", "CreateContainer" );

            StorageData container = new StorageData( Warehouse.GetUniqueID( ContainerData.ToArray( ) ), position, rotation, scale )
            {
                IsContainer = true
            };

            container.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableContainer" ), position, rotation, GameObject.FindGameObjectWithTag( "ContainerDefinition" ).transform ) );

            container.Object.name = "Container" + container.GetID( );

            ContainerData.Add( container );

            container.ChangeSlotCount( new StorageSlotCalculation( ) );

            Data.Container.Add( new ProjectStorageData( container.GetID( ), container.GetItems.Length, new ProjectTransformationData( position, rotation, scale ) ) );

            OnSContainerCreated( container );

            return container;
        }

        internal void AddContainer( StorageData container )
        {
            LogManager.WriteInfo( "Mobiles Regal wird hinzugefuegt.", "Warehouse", "AddContainer" );

            container.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableContainer" ), container.Position, container.Rotation, GameObject.FindGameObjectWithTag( "ContainerDefinition" ).transform ) );

            container.Object.name = "Container" + container.GetID( );

            ContainerData.Add( container );

            container.ChangeSlotCount( new StorageSlotCalculation() );

            Data.Container.Add( new ProjectStorageData( container.GetID( ), container.GetItems.Length, new ProjectTransformationData( container.Position, container.Rotation, container.Scale ) ) );

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

        internal void DestroyContainer()
        {
            foreach ( StorageData container in ContainerData )
            {
                container.Destroy( );
            }
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
