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
        public List<StorageData> ContainerData { get; private set; }

        internal InternalProjectContainer Data { get; private set; }

        public Container()
        {
            ContainerData = new List<StorageData>( );
            Data = new InternalProjectContainer( );
        }

        public StorageData CreateContainer( Vector3 position, Vector3 rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Mobiles Regal wird erstellt.", "ContainerData", "CreateContainer" );

            StorageData container = new StorageData( GetUniqueID( ContainerData.ToArray( ) ), position, rotation, scale );

            ContainerData.Add( container );

            Data.AddContainer( new ProjectStorageData( container.GetID( ), new ProjectTransformationData( position, rotation, scale ) ) );

            return container;
        }

        public void AddContainer( StorageData container )
        {
            LogManager.WriteInfo( "Mobiles Regal wird hinzugefuegt.", "Warehouse", "AddContainer" );

            ContainerData.Add( container );

            Data.AddContainer( new ProjectStorageData( container.GetID( ), new ProjectTransformationData( container.Position, container.Rotation, container.Scale ) ) );
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

        public StorageData GetStorageReck( GameObject obj )
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

        public ItemData CreateContainerItem( Vector3 position, Vector3 rotation, Vector3 scale, StorageData container )
        {
            LogManager.WriteInfo( "Ein ContainerItem wird erstellt.", "ContainerData", "CreateContainerItem" );

            ItemData item = new ItemData( 0, 0, position, rotation, scale );

            container.AddItem( item );

            for ( int i = 0; i < Data.Container.Count; i++ )
            {
                if ( Data.Container[ i ].ID == container.GetID( ) )
                {
                    Data.Container[ i ].AddItem( new ProjectItemData( 0, new ProjectTransformationData( position, rotation, scale ) ) );

                    return item;
                }
            }

            LogManager.WriteWarning( "Interne Regale stimmen nicht mit den Regaldaten ueberein!.", "Warehouse", "CreateStorageReckItem" );

            return item;
        }

        public void AddItemToContainer( StorageData container, ItemData item )
        {
            LogManager.WriteInfo( "Ein ContainerItem wird erstellt.", "ContainerData", "AddItemToContainer" );

            container.AddItem( item );

            for ( int i = 0; i < Data.Container.Count; i++ )
            {
                if ( Data.Container[ i ].ID == container.GetID( ) )
                {
                    Data.Container[ i ].AddItem( new ProjectItemData( item.GetID( ), new ProjectTransformationData( item.Position, item.Rotation, item.Scale ) ) );

                    return;
                }
            }

            LogManager.WriteWarning( "Interne ContainerData stimmen nicht mit den Containerdaten ueberein!.", "ContainerData", "AddItemToContainer" );
        }

        public bool RemoveItemFromContainer( StorageData storage, ItemData item )
        {
            LogManager.WriteInfo( "Ein ContainerItem wird entfernt.", "ContainerData", "RemoveItemFromContainer" );

            storage.RemoveItem( item );

            for ( int i = 0; i < Data.Container.Count; i++ )
            {
                if ( Data.Container[ i ].ID == storage.GetID( ) )
                {
                    for ( int j = 0; j < Data.Container[ i ].GetItems( ).Length; j++ )
                    {
                        if ( Data.Container[ i ].GetItems( )[ j ].IDRef == item.GetID( ) )
                        {
                            return Data.Container[ i ].RemoveItem( Data.Container[ i ].GetItems( )[ j ] );
                        }
                    }
                }
            }

            return false;
        }

        private long GetUniqueID( IDataIdentifier[ ] idUsed )
        {
            bool used = false;

            for ( int i = 0; ; i++ )
            {
                used = false;

                for ( int j = 0; j < idUsed.Length; j++ )
                {
                    if ( i + 1 == idUsed[ j ].GetID( ) )
                    {
                        used = true;
                        break;
                    }
                }

                if ( !used )
                {
                    return i + 1;
                }
            }
        }
    }
}
