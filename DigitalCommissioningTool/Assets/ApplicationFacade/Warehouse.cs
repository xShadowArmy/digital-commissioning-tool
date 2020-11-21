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
    public class Warehouse
    {
        public FloorData Floor { get; private set; }

        public List<WallData> Walls { get; private set; }

        public List<WindowData> Windows { get; private set; }

        public List<DoorData> Doors { get; private set; }

        public List<StorageData> StorageRecks { get; private set; }

        internal InternalProjectWarehouse Data { get; private set; }

        internal Warehouse()
        {
            Floor   = new FloorData( );
            Walls   = new List<WallData>( );
            Windows = new List<WindowData>( );
            Doors   = new List<DoorData>( );
            StorageRecks = new List<StorageData>( );
            Data = new InternalProjectWarehouse( );
        }
        
        public void AdjustFloor( Vector3 position, Vector3 rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Lagerhausboden wird angepasst.", "Warehouse", "AdjustFloor" );

            Floor.SetPosition( position );
            Floor.SetRotation( rotation );
            Floor.SetScale( scale );

            Data.UpdateFloor( new ProjectFloorData( new ProjectTransformationData( position, rotation, scale ) ) );
        }

        public WallData CreateWall( Vector3 position, Vector3 rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Lagehauswand wird erstellt.", "Warehouse", "CreateWall" );
            
            WallData wall = new WallData( GetUniqueID( Walls.ToArray() ), position, rotation, scale );

            Walls.Add( wall );

            Data.AddWall( new ProjectWallData( wall.GetID(), new ProjectTransformationData( position, rotation, scale ) ) );

            return wall;
        }

        public void AddWall( WallData wall )
        {
            LogManager.WriteInfo( "Lagehauswand wird hinzugefuegt.", "Warehouse", "AddWall" );

            Walls.Add( wall );

            Data.AddWall( new ProjectWallData( wall.GetID( ), new ProjectTransformationData( wall.Position, wall.Rotation, wall.Scale ) ) );
        }

        public bool RemoveWall( WallData wall )
        {
            LogManager.WriteInfo( "Lagehauswand wird entfernt.", "Warehouse", "RemoveWall" );

            if (! Walls.Remove( wall ) )
            {
                return false;
            }

            for( int i = 0; i < Data.Walls.Count; i++ )
            {
                if ( Data.Walls[i].ID == wall.GetID() )
                {
                    Data.Walls.Remove( Data.Walls[ i ] );

                    return true;
                }
            }

            return false;
        }

        public WallData GetWall( long id )
        {
            LogManager.WriteInfo( "Lagehauswand wird abgefragt.", "Warehouse", "GetWall" );

            for ( int i = 0; i < Walls.Count; i++ )
            {
                if ( Walls[ i ].GetID() == id )
                {
                    return Walls[ i ];
                }
            }

            return null;
        }

        public WallData GetWall( GameObject obj )
        {
            LogManager.WriteInfo( "Lagehauswand wird abgefragt.", "Warehouse", "GetWall" );

            for ( int i = 0; i < Walls.Count; i++ )
            {
                if ( Walls[ i ].Object == obj )
                {
                    return Walls[ i ];
                }
            }

            return null;
        }

        public WindowData CreateWindow( Vector3 position, Vector3 rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Lagerhausfenster wird erstellt.", "Warehouse", "CreateWindow" );

            WindowData window = new WindowData( GetUniqueID( Windows.ToArray( ) ), position, rotation, scale );

            Windows.Add( window );

            Data.AddWindow( new ProjectWindowData( window.GetID( ), new ProjectTransformationData( position, rotation, scale ) ) );

            return window;
        }

        public void AddWindow( WindowData window )
        {
            LogManager.WriteInfo( "Lagehaus Fenster wird hinzugefuegt.", "Warehouse", "AddWindow" );
            
            Windows.Add( window );

            Data.AddWindow( new ProjectWindowData( window.GetID( ), new ProjectTransformationData( window.Position, window.Rotation, window.Scale ) ) );
        }

        public bool RemoveWindow( WindowData window )
        {
            LogManager.WriteInfo( "Lagehaus Fenster wird entfernt.", "Warehouse", "RemoveWindow" );
            
            if ( !Windows.Remove( window ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.Windows.Count; i++ )
            {
                if ( Data.Windows[ i ].ID == window.GetID( ) )
                {
                    Data.Windows.Remove( Data.Windows[ i ] );

                    return true;
                }
            }

            return false;
        }

        public WindowData GetWindow( long id )
        {
            LogManager.WriteInfo( "Lagerhaufenster wird abgefragt.", "Warehouse", "GetWindow" );

            for ( int i = 0; i < Windows.Count; i++ )
            {
                if ( Windows[ i ].GetID( ) == id )
                {
                    return Windows[ i ];
                }
            }

            return null;
        }

        public WindowData GetWindow( GameObject obj )
        {
            LogManager.WriteInfo( "Lagerhaufenster wird abgefragt.", "Warehouse", "GetWindow" );

            for ( int i = 0; i < Windows.Count; i++ )
            {
                if ( Windows[ i ].Object == obj )
                {
                    return Windows[ i ];
                }
            }

            return null;
        }

        public DoorData CreateDoor( Vector3 position, Vector3 rotation, Vector3 scale, DoorType type )
        {
            LogManager.WriteInfo( "Lagerhaustuer wird erstellt.", "Warehouse", "CreateDoor" );

            DoorData Door = new DoorData( GetUniqueID( Doors.ToArray( ) ), type, position, rotation, scale );

            Doors.Add( Door );

            Data.AddDoor( new ProjectDoorData( Door.GetID( ), type.ToString(), new ProjectTransformationData( position, rotation, scale ) ) );

            return Door;
        }

        public void AddDoor( DoorData door )
        {
            LogManager.WriteInfo( "Lagehaustuer wird hinzugefuegt.", "Warehouse", "AddDoor" );

            Doors.Add( door );

            Data.AddDoor( new ProjectDoorData( door.GetID( ), door.Type.ToString(), new ProjectTransformationData( door.Position, door.Rotation, door.Scale ) ) );
        }

        public bool RemoveDoor( DoorData door )
        {
            LogManager.WriteInfo( "Lagerhaustuer wird entfernt.", "Warehouse", "RemoveDoor" );

            if ( !Doors.Remove( door ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.Doors.Count; i++ )
            {
                if ( Data.Doors[ i ].ID == door.GetID( ) )
                {
                    Data.Doors.Remove( Data.Doors[ i ] );

                    return true;
                }
            }

            return false;
        }

        public DoorData GetDoor( long id )
        {
            LogManager.WriteInfo( "Lagerhaustuer wird abgefragt.", "Warehouse", "GetDoor" );

            for ( int i = 0; i < Doors.Count; i++ )
            {
                if ( Doors[ i ].GetID( ) == id )
                {
                    return Doors[ i ];
                }
            }

            return null;
        }

        public DoorData GetDoor( GameObject obj )
        {
            LogManager.WriteInfo( "Lagerhaustuer wird abgefragt.", "Warehouse", "GetDoor" );

            for ( int i = 0; i < Doors.Count; i++ )
            {
                if ( Doors[ i ].Object == obj )
                {
                    return Doors[ i ];
                }
            }

            return null;
        }

        public StorageData CreateStorageReck( Vector3 position, Vector3 rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Lagerhausregal wird erstellt.", "Warehouse", "CreateStorageReck" );

            StorageData storage = new StorageData( GetUniqueID( StorageRecks.ToArray( ) ), position, rotation, scale );

            StorageRecks.Add( storage );

            Data.AddStorageReck( new ProjectStorageData( storage.GetID( ), new ProjectTransformationData( position, rotation, scale ) ) );

            return storage;
        }

        public void AddStorageReck( StorageData storage )
        {
            LogManager.WriteInfo( "Lagerhausregal wird hinzugefuegt.", "Warehouse", "AddStorageReck" );

            StorageRecks.Add( storage );

            Data.AddStorageReck( new ProjectStorageData( storage.GetID( ), new ProjectTransformationData( storage.Position, storage.Rotation, storage.Scale ) ) );
        }

        public bool RemoveStorageReck( StorageData storage )
        {
            LogManager.WriteInfo( "Lagerhausregal wird entfernt.", "Warehouse", "RemoveStorageReck" );

            if ( !StorageRecks.Remove( storage ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.StorageRecks.Count; i++ )
            {
                if ( Data.StorageRecks[ i ].ID == storage.GetID( ) )
                {
                    Data.StorageRecks.Remove( Data.StorageRecks[ i ] );

                    return true;
                }
            }

            return false;
        }

        public StorageData GetStorageReck( long id )
        {
            LogManager.WriteInfo( "Lagerhausregal wird abgefragt.", "Warehouse", "GetStorageReck" );

            for ( int i = 0; i < StorageRecks.Count; i++ )
            {
                if ( StorageRecks[ i ].GetID( ) == id )
                {
                    return StorageRecks[ i ];
                }
            }

            return null;
        }

        public StorageData GetStorageReck( GameObject obj )
        {
            LogManager.WriteInfo( "Lagerhausregal wird abgefragt.", "Warehouse", "GetStorageReck" );

            for ( int i = 0; i < StorageRecks.Count; i++ )
            {
                if ( StorageRecks[ i ].Object == obj )
                {
                    return StorageRecks[ i ];
                }
            }

            return null;
        }
        
        public ItemData CreateStorageReckItem( Vector3 position, Vector3 rotation, Vector3 scale, StorageData storage )
        {
            LogManager.WriteInfo( "Ein RegalItem wird erstellt.", "Warehouse", "CreateStorageReckItem" );

            ItemData item = new ItemData( 0, 0, position, rotation, scale );

            storage.AddItem( item );

            for ( int i = 0; i < Data.StorageRecks.Count; i++ )
            {
                if ( Data.StorageRecks[ i ].ID == storage.GetID() )
                {
                    Data.StorageRecks[ i ].AddItem( new ProjectItemData( 0, new ProjectTransformationData( position, rotation, scale ) ) );

                    return item;
                }
            }

            LogManager.WriteWarning( "Interne Regale stimmen nicht mit den Regaldaten ueberein!.", "Warehouse", "CreateStorageReckItem" );
            
            return item;
        }

        public void AddItemToStorageReck( StorageData storage, ItemData item )
        {
            LogManager.WriteInfo( "Ein RegalItem wird hinzugefuegt.", "Warehouse", "AddItemToStorageReck" );
            
            storage.AddItem( item );

            for ( int i = 0; i < Data.StorageRecks.Count; i++ )
            {
                if ( Data.StorageRecks[ i ].ID == storage.GetID( ) )
                {
                    Data.StorageRecks[ i ].AddItem( new ProjectItemData( item.GetID(), new ProjectTransformationData( item.Position, item.Rotation, item.Scale ) ) );

                    return;
                }
            }

            LogManager.WriteWarning( "Interne Regale stimmen nicht mit den Regaldaten ueberein!.", "Warehouse", "AddItemToStorageReck" );
        }

        public bool RemoveItemFromStorageReck( StorageData storage, ItemData item )
        {
            LogManager.WriteInfo( "Ein RegalItem wird entfernt.", "Warehouse", "RemoveItemFromStorageReck" );

            storage.RemoveItem( item );

            for ( int i = 0; i < Data.StorageRecks.Count; i++ )
            {
                if ( Data.StorageRecks[ i ].ID == storage.GetID( ) )
                {
                    for( int j = 0; j < Data.StorageRecks[i].GetItems().Length; j++ )
                    {
                        if ( Data.StorageRecks[i].GetItems()[j].IDRef == item.GetID() )
                        {
                            return Data.StorageRecks[ i ].RemoveItem( Data.StorageRecks[ i ].GetItems( )[ j ] );
                        }
                    }
                }
            }

            return false;
        }

        private long GetUniqueID( IDataIdentifier[] idUsed )
        {
            bool used = false;

            for ( int i = 0; ; i++ )
            {
                used = false;

                for ( int j = 0; j < idUsed.Length; j++ )
                {
                    if ( i + 1 == idUsed[ j ].GetID() )
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
