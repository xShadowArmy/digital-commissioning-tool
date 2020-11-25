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

        public List<StorageData> StorageRacks { get; private set; }

        internal InternalProjectWarehouse Data { get; private set; }

        internal Warehouse()
        {
            Floor   = new FloorData( );
            Walls   = new List<WallData>( );
            Windows = new List<WindowData>( );
            Doors   = new List<DoorData>( );
            StorageRacks = new List<StorageData>( );
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

            wall.GameObjectDataChanged += GameObjectHasChanged;
            wall.WallChanged += WallHasChanged;

            Walls.Add( wall );

            Data.AddWall( new ProjectWallData( wall.GetID(), new ProjectTransformationData( position, rotation, scale ) ) );

            return wall;
        }

        public void AddWall( WallData wall )
        {
            LogManager.WriteInfo( "Lagerhauswand wird hinzugefuegt.", "Warehouse", "AddWall" );

            wall.GameObjectDataChanged += GameObjectHasChanged;
            wall.WallChanged += WallHasChanged;

            Walls.Add( wall );

            Data.AddWall( new ProjectWallData( wall.GetID( ), new ProjectTransformationData( wall.Position, wall.Rotation, wall.Scale ) ) );
        }

        public bool RemoveWall( WallData wall )
        {
            LogManager.WriteInfo( "Lagerhauswand wird entfernt.", "Warehouse", "RemoveWall" );

            if (! Walls.Remove( wall ) )
            {
                return false;
            }

            for( int i = 0; i < Data.Walls.Count; i++ )
            {
                if ( Data.Walls[i].ID == wall.GetID() )
                {
                    Data.Walls.Remove( Data.Walls[ i ] );
                    
                    wall.GameObjectDataChanged -= GameObjectHasChanged;
                    wall.WallChanged -= WallHasChanged;

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

            window.GameObjectDataChanged += GameObjectHasChanged;
            window.WindowChanged += WindowHasChanged;

            Windows.Add( window );

            Data.AddWindow( new ProjectWindowData( window.GetID( ), new ProjectTransformationData( position, rotation, scale ) ) );

            return window;
        }

        public void AddWindow( WindowData window )
        {
            LogManager.WriteInfo( "Lagehaus Fenster wird hinzugefuegt.", "Warehouse", "AddWindow" );

            window.GameObjectDataChanged += GameObjectHasChanged;
            window.WindowChanged += WindowHasChanged;

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

                    window.GameObjectDataChanged -= GameObjectHasChanged;
                    window.WindowChanged -= WindowHasChanged;

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

            DoorData door = new DoorData( GetUniqueID( Doors.ToArray( ) ), type, position, rotation, scale );

            door.GameObjectDataChanged += GameObjectHasChanged;
            door.DoorChanged += DoorHasChanged;

            Doors.Add( door );

            Data.AddDoor( new ProjectDoorData( door.GetID( ), type.ToString(), new ProjectTransformationData( position, rotation, scale ) ) );

            return door;
        }

        public void AddDoor( DoorData door )
        {
            LogManager.WriteInfo( "Lagehaustuer wird hinzugefuegt.", "Warehouse", "AddDoor" );

            door.GameObjectDataChanged += GameObjectHasChanged;
            door.DoorChanged += DoorHasChanged;

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

                    door.GameObjectDataChanged -= GameObjectHasChanged;
                    door.DoorChanged -= DoorHasChanged;

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

        public StorageData CreateStorageRack( Vector3 position, Vector3 rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Lagerhausregal wird erstellt.", "Warehouse", "CreateStorageRack" );

            StorageData storage = new StorageData( GetUniqueID( StorageRacks.ToArray( ) ), position, rotation, scale );

            storage.GameObjectDataChanged += GameObjectHasChanged;
            storage.StorageChanged += StorageRackHasChanged;

            StorageRacks.Add( storage );

            Data.AddStorageRack( new ProjectStorageData( storage.GetID( ), new ProjectTransformationData( position, rotation, scale ) ) );

            return storage;
        }

        public void AddStorageRack( StorageData storage )
        {
            LogManager.WriteInfo( "Lagerhausregal wird hinzugefuegt.", "Warehouse", "AddStorageRack" );

            storage.GameObjectDataChanged += GameObjectHasChanged;
            storage.StorageChanged += StorageRackHasChanged;

            StorageRacks.Add( storage );

            Data.AddStorageRack( new ProjectStorageData( storage.GetID( ), new ProjectTransformationData( storage.Position, storage.Rotation, storage.Scale ) ) );
        }

        public bool RemoveStorageRack( StorageData storage )
        {
            LogManager.WriteInfo( "Lagerhausregal wird entfernt.", "Warehouse", "RemoveStorageRack" );

            if ( !StorageRacks.Remove( storage ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.StorageRacks.Count; i++ )
            {
                if ( Data.StorageRacks[ i ].ID == storage.GetID( ) )
                {
                    Data.StorageRacks.Remove( Data.StorageRacks[ i ] );

                    storage.GameObjectDataChanged -= GameObjectHasChanged;
                    storage.StorageChanged -= StorageRackHasChanged;

                    return true;
                }
            }

            return false;
        }

        public StorageData GetStorageRack( long id )
        {
            LogManager.WriteInfo( "Lagerhausregal wird abgefragt.", "Warehouse", "GetStorageRack" );

            for ( int i = 0; i < StorageRacks.Count; i++ )
            {
                if ( StorageRacks[ i ].GetID( ) == id )
                {
                    return StorageRacks[ i ];
                }
            }

            return null;
        }

        public StorageData GetStorageRack( GameObject obj )
        {
            LogManager.WriteInfo( "Lagerhausregal wird abgefragt.", "Warehouse", "GetStorageRack" );

            for ( int i = 0; i < StorageRacks.Count; i++ )
            {
                if ( StorageRacks[ i ].Object == obj )
                {
                    return StorageRacks[ i ];
                }
            }

            return null;
        }
        
        public ItemData CreateStorageRackItem( Vector3 position, Vector3 rotation, Vector3 scale, StorageData storage )
        {
            LogManager.WriteInfo( "Ein RegalItem wird erstellt.", "Warehouse", "CreateStorageRackItem" );

            ItemData item = new ItemData( 0, 0, position, rotation, scale );

            item.GameObjectDataChanged += GameObjectHasChanged;
            item.ItemChanged += StorageRackItemHasChanged;

            item.SetParent( storage );
            storage.AddItem( item );
                       
            for ( int i = 0; i < Data.StorageRacks.Count; i++ )
            {
                if ( Data.StorageRacks[ i ].ID == storage.GetID() )
                {
                    Data.StorageRacks[ i ].AddItem( new ProjectItemData( 0, new ProjectTransformationData( position, rotation, scale ) ) );
                                       
                    return item;
                }
            }

            LogManager.WriteWarning( "Interne Regale stimmen nicht mit den Regaldaten ueberein!.", "Warehouse", "CreateStorageRackItem" );
            
            return item;
        }

        public void AddItemToStorageRack( StorageData storage, ItemData item )
        {
            LogManager.WriteInfo( "Ein RegalItem wird hinzugefuegt.", "Warehouse", "AddItemToStorageRack" );

            item.GameObjectDataChanged += GameObjectHasChanged;
            item.ItemChanged += StorageRackItemHasChanged;

            item.SetParent( storage );
            storage.AddItem( item );

            for ( int i = 0; i < Data.StorageRacks.Count; i++ )
            {
                if ( Data.StorageRacks[ i ].ID == storage.GetID( ) )
                {
                    Data.StorageRacks[ i ].AddItem( new ProjectItemData( item.GetID(), new ProjectTransformationData( item.Position, item.Rotation, item.Scale ) ) );

                    return;
                }
            }

            LogManager.WriteWarning( "Interne Regale stimmen nicht mit den Regaldaten ueberein!.", "Warehouse", "AddItemToStorageRack" );
        }

        public bool RemoveItemFromStorageRack( StorageData storage, ItemData item )
        {
            LogManager.WriteInfo( "Ein RegalItem wird entfernt.", "Warehouse", "RemoveItemFromStorageRack" );
            
            item.SetParent( null );
            storage.RemoveItem( item );

            for ( int i = 0; i < Data.StorageRacks.Count; i++ )
            {
                if ( Data.StorageRacks[ i ].ID == storage.GetID( ) )
                {
                    for( int j = 0; j < Data.StorageRacks[i].GetItems().Length; j++ )
                    {
                        if ( Data.StorageRacks[i].GetItems()[j].IDRef == item.GetID() )
                        {
                            item.GameObjectDataChanged -= GameObjectHasChanged;
                            item.ItemChanged -= StorageRackItemHasChanged;

                            return Data.StorageRacks[ i ].RemoveItem( Data.StorageRacks[ i ].GetItems( )[ j ] );
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

        private void GameObjectHasChanged( GameObjectData obj, GameObjectDataType type )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere GameObjectData. Type=" + type.ToString( ), "Warehouse", "GameObjectHasChanged" );

            switch ( type )
            {
                case GameObjectDataType.Floor:

                    FloorHasChanged( obj as FloorData );
                    break;

                case GameObjectDataType.Wall:

                    WallHasChanged( obj as WallData );
                    break;

                case GameObjectDataType.Window:

                    WindowHasChanged( obj as WindowData );
                    break;

                case GameObjectDataType.Door:

                    DoorHasChanged( obj as DoorData );
                    break;

                case GameObjectDataType.StorageReck:

                    StorageRackHasChanged( obj as StorageData );
                    break;

                case GameObjectDataType.Item:

                    StorageRackItemHasChanged( obj as ItemData );
                    break;

                default:

                    LogManager.WriteWarning( "[Event] Falscher Typ in EventSystem referenziert!", "Warehouse", "GameObjectHasChanged" );
                    break;
            }
        }

        private void FloorHasChanged( FloorData floor )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere FloorData.", "Warehouse", "FloorHasChanged" );

            Data.UpdateFloor( new ProjectFloorData( new ProjectTransformationData( floor.Position, floor.Rotation, floor.Scale ) ) );
        }

        private void WallHasChanged( WallData wall )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere WallData.", "Warehouse", "WallHasChanged" );
            
            for ( int i = 0; i < Data.Walls.Count; i++ )
            {
                if ( wall.GetID() == Data.Walls[i].ID )
                {
                    Data.Walls.Remove( Data.Walls[ i ] );
                    Data.Walls.Insert( i, new ProjectWallData( wall.GetID(), new ProjectTransformationData( wall.Position, wall.Rotation, wall.Scale ) ) );

                    return;
                }
            }
        }

        private void WindowHasChanged( WindowData window )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere WindowData.", "Warehouse", "WindowHasChanged" );

            for ( int i = 0; i < Data.Windows.Count; i++ )
            {
                if ( window.GetID( ) == Data.Windows[ i ].ID )
                {
                    Data.Windows.Remove( Data.Windows[i] );
                    Data.Windows.Add( new ProjectWindowData( window.GetID(), new ProjectTransformationData( window.Position, window.Rotation, window.Scale ) ) );

                    return;
                }
            }
        }

        private void DoorHasChanged( DoorData door )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere DoorData.", "Warehouse", "DoorHasChanged" );

            for ( int i = 0; i < Data.Doors.Count; i++ )
            {
                if ( door.GetID( ) == Data.Doors[ i ].ID )
                {
                    Data.Doors.Remove( Data.Doors[ i ] );
                    Data.Doors.Insert(i, new ProjectDoorData( door.GetID(), door.Type.ToString(), new ProjectTransformationData( door.Position, door.Rotation, door.Scale ) ) );
                    Data.Doors[ i ].SetType( door.Type.ToString( ) );

                    return;
                }
            }
        }

        private void StorageRackHasChanged( StorageData storage )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere StorageData.", "Warehouse", "StorageRackHasChanged" );

            for ( int i = 0; i < Data.StorageRacks.Count; i++ )
            {
                if ( storage.GetID( ) == Data.StorageRacks[ i ].ID )
                {
                    if ( storage.GetItems().Length > Data.StorageRacks[i].GetItems().Length )
                    {
                        ProjectTransformationData data = new ProjectTransformationData( storage.GetItems( )[ storage.GetItems( ).Length - 1 ].Position,
                                                                                        storage.GetItems( )[ storage.GetItems( ).Length - 1 ].Rotation,
                                                                                        storage.GetItems( )[ storage.GetItems( ).Length - 1 ].Scale );
                        
                        Data.StorageRacks[ i ].AddItem( new ProjectItemData( storage.GetItems( )[ storage.GetItems( ).Length - 1 ].GetID( ), data ) );

                        break;
                    }

                    else if ( storage.GetItems( ).Length < Data.StorageRacks[ i ].GetItems( ).Length )
                    {
                        if ( storage.GetItems( ).Length == 1 )
                        {
                            Data.StorageRacks[ i ].RemoveItem( Data.StorageRacks[ i ].GetItems( )[ 0 ] );

                            break;
                        }

                        for ( int j = 0; j < Data.StorageRacks[i].GetItems().Length; j++ )
                        {
                            if ( Data.StorageRacks[i].GetItems()[j].IDRef != storage.GetItems()[j].GetID() )
                            {
                                Data.StorageRacks[ i ].RemoveItem( Data.StorageRacks[i].GetItems()[j] );

                                break;
                            }
                        }

                        break;
                    }

                    else
                    {
                        ProjectItemData[ ] items = Data.StorageRacks[ i ].GetItems( );
                        ProjectStorageData data = new ProjectStorageData( storage.GetID(), new ProjectTransformationData( storage.Position, storage.Rotation, storage.Scale ) );

                        foreach( ProjectItemData item in items )
                        {
                            data.AddItem( item );
                        }

                        Data.StorageRacks.Remove( Data.StorageRacks[ i ] );
                        Data.StorageRacks.Insert( i, data );

                        break;
                    }
                }
            }
        }

        private void StorageRackItemHasChanged( ItemData item )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere ItemData.", "Warehouse", "StorageRackItemHasChanged" );
            
            for( int i = 0; i < StorageRacks.Count; i++ )
            {
                if ( item.Parent.GetID() == StorageRacks[i].GetID() )
                {
                    for( int j = 0; j < Data.StorageRacks[i].GetItems().Length; j++ )
                    {
                        if ( Data.StorageRacks[i].GetItems()[j].IDRef == item.GetID() )
                        {
                            ProjectTransformationData data = new ProjectTransformationData( item.Position, item.Rotation, item.Scale );

                            Data.StorageRacks[ i ].RemoveItem( Data.StorageRacks[ i ].GetItems( )[ j ] );
                            Data.StorageRacks[ i ].AddItem( j, new ProjectItemData( item.GetID(), new ProjectTransformationData( item.Position, item.Rotation, item.Scale ) ) );

                            break;
                        }
                    }

                    break;
                }
            }
        }
    }
}
