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
    /// <summary>
    /// Speichert Informationen über alle Container in der Umgebung.
    /// </summary>
    public class Container
    {
        /// <summary>
        /// Eventhandler für Create und Delete Events.
        /// </summary>
        /// <param name="storage">Das Regal welches mit dem Event in Verbindung gebracht werden kann.</param>
        public delegate void ContainerChangedEventHandler( StorageData storage );

        /// <summary>
        /// Wird ausgelöst wenn ein neues mobiles Regal erstellt wurde.
        /// </summary>
        public event ContainerChangedEventHandler ContainerCreated;

        /// <summary>
        /// Wird ausgeläst wenn ein mobiles Regal gelöscht wurde.
        /// </summary>
        public event ContainerChangedEventHandler ContainerDeleted;

        /// <summary>
        /// Liste mit allen mobilen Regalen.
        /// </summary>
        internal List<StorageData> ContainerData { get; private set; }

        /// <summary>
        /// Internes Projektobjekt das für das Speichern der Container verantwortlich ist.
        /// </summary>
        internal InternalProjectContainer Data { get; private set; }

        /// <summary>
        /// Gibt alle mobilen Regale zurueck.
        /// </summary>
        public StorageData[] StorageContainer
        {
            get
            {
                return ContainerData.ToArray( );
            }
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        public Container()
        {
            ContainerData = new List<StorageData>( );
            Data = new InternalProjectContainer( );
        }

        ~Container()
        {
            DestroyContainer( );
        }

        /// <summary>
        /// Erstellt einen neuen Container am default Spawnpunkt.
        /// </summary>
        /// <returns>Objekt das den Container repräsentiert.</returns>
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
        
        /// <summary>
        /// Erstellt einen neuen Container.
        /// </summary>
        /// <param name="position">Die Position des Containers.</param>
        /// <param name="rotation">Die Rotation des Containers.</param>
        /// <param name="scale">Die Skalierung des Containres.</param>
        /// <returns>Objekt das den Container repräsentiert.</returns>
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

        /// <summary>
        /// Fügt einen geladenen Container zur Containerverwaltung hinzu.
        /// </summary>
        /// <param name="container">Der geladene Container der Verwaltet werden soll.</param>
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

        /// <summary>
        /// Entfernt einen Container aus der Liste und der Umgebung.
        /// </summary>
        /// <param name="container">Der Container der Entfernt werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
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

        /// <summary>
        /// Gibt den Container mit der angegebenen ID zurück.
        /// </summary>
        /// <param name="id">Die ID des Containers.</param>
        /// <returns>Gibt den Container oder null zurück.</returns>
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

        /// <summary>
        /// Gibt den Container mit dem angegebenen GameObject zurück.
        /// </summary>
        /// <param name="obj">Das GameObject des Containers..</param>
        /// <returns>Gibt den Container oder null zurück.</returns>
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

        /// <summary>
        /// Zerstört alle Container in der Umgebung.
        /// </summary>
        internal void DestroyContainer()
        {
            foreach ( StorageData container in ContainerData )
            {
                container.Destroy( );
            }
        }

        /// <summary>
        /// Löst das ContainerCreated Event aus.
        /// </summary>
        /// <param name="data">Der Container der erstellt wurde.</param>
        protected virtual void OnSContainerCreated( StorageData data )
        {
            ContainerCreated?.Invoke( data );
        }

        /// <summary>
        /// Löst das ContainerDeleted Event aus.
        /// </summary>
        /// <param name="data">Der Container der gelöscht wurde.</param>
        protected virtual void OnContainerDeleted( StorageData data )
        {
            ContainerDeleted?.Invoke( data );
        }
    }
}
