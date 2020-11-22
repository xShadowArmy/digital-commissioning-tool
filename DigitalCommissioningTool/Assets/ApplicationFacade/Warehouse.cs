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

            wall.GameObjectDataChanged += GameObjectHasChanged;
            wall.WallChanged += WallHasChanged;

            Walls.Add( wall );

            Data.AddWall( new ProjectWallData( wall.GetID(), new ProjectTransformationData( position, rotation, scale ) ) );

            return wall;
        }

        public void AddWall( WallData wall )
        {
            LogManager.WriteInfo( "Lagehauswand wird hinzugefuegt.", "Warehouse", "AddWall" );

            wall.GameObjectDataChanged += GameObjectHasChanged;
            wall.WallChanged += WallHasChanged;

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

        public StorageData CreateStorageReck( Vector3 position, Vector3 rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Lagerhausregal wird erstellt.", "Warehouse", "CreateStorageReck" );

            StorageData storage = new StorageData( GetUniqueID( StorageRecks.ToArray( ) ), position, rotation, scale );

            storage.GameObjectDataChanged += GameObjectHasChanged;
            storage.StorageChanged += StorageReckHasChanged;

            StorageRecks.Add( storage );

            Data.AddStorageReck( new ProjectStorageData( storage.GetID( ), new ProjectTransformationData( position, rotation, scale ) ) );

            return storage;
        }

        public void AddStorageReck( StorageData storage )
        {
            LogManager.WriteInfo( "Lagerhausregal wird hinzugefuegt.", "Warehouse", "AddStorageReck" );

            storage.GameObjectDataChanged += GameObjectHasChanged;
            storage.StorageChanged += StorageReckHasChanged;

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

                    storage.GameObjectDataChanged -= GameObjectHasChanged;
                    storage.StorageChanged -= StorageReckHasChanged;

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

            item.GameObjectDataChanged += GameObjectHasChanged;
            item.ItemChanged += StorageReckItemHasChanged;

            item.SetParent( storage );
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

            item.GameObjectDataChanged += GameObjectHasChanged;
            item.ItemChanged += StorageReckItemHasChanged;

            item.SetParent( storage );
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
            
            item.GameObjectDataChanged -= GameObjectHasChanged;
            item.ItemChanged -= StorageReckItemHasChanged;

            item.SetParent( null );
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

                    StorageReckHasChanged( obj as StorageData );
                    break;

                case GameObjectDataType.Item:

                    StorageReckItemHasChanged( obj as ItemData );
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

        private void StorageReckHasChanged( StorageData storage )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere StorageData.", "Warehouse", "StorageReckHasChanged" );

            for ( int i = 0; i < Data.StorageRecks.Count; i++ )
            {
                if ( storage.GetID( ) == Data.StorageRecks[ i ].ID )
                {
                    if ( storage.GetItems().Length > Data.StorageRecks[i].GetItems().Length )
                    {
                        ProjectTransformationData data = new ProjectTransformationData( storage.GetItems( )[ storage.GetItems( ).Length - 1 ].Position,
                                                                                        storage.GetItems( )[ storage.GetItems( ).Length - 1 ].Rotation,
                                                                                        storage.GetItems( )[ storage.GetItems( ).Length - 1 ].Scale );
                        
                        Data.StorageRecks[ i ].AddItem( new ProjectItemData( storage.GetItems( )[ storage.GetItems( ).Length - 1 ].GetID( ), data ) );

                        break;
                    }

                    else if ( storage.GetItems( ).Length < Data.StorageRecks[ i ].GetItems( ).Length )
                    {
                        if ( storage.GetItems( ).Length == 1 )
                        {
                            Data.StorageRecks[ i ].RemoveItem( Data.StorageRecks[ i ].GetItems( )[ 0 ] );

                            break;
                        }

                        for ( int j = 0; j < Data.StorageRecks[i].GetItems().Length; j++ )
                        {
                            if ( Data.StorageRecks[i].GetItems()[j].IDRef != storage.GetItems()[j].GetID() )
                            {
                                Data.StorageRecks[ i ].RemoveItem( Data.StorageRecks[i].GetItems()[j] );

                                break;
                            }
                        }

                        break;
                    }

                    else
                    {
                        ProjectItemData[ ] items = Data.StorageRecks[ i ].GetItems( );
                        ProjectStorageData data = new ProjectStorageData( storage.GetID(), new ProjectTransformationData( storage.Position, storage.Rotation, storage.Scale ) );

                        foreach( ProjectItemData item in items )
                        {
                            data.AddItem( item );
                        }

                        Data.StorageRecks.Remove( Data.StorageRecks[ i ] );
                        Data.StorageRecks.Insert( i, data );

                        break;
                    }
                }
            }
        }

        private void StorageReckItemHasChanged( ItemData item )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere ItemData.", "Warehouse", "StorageReckItemHasChanged" );
            
            for( int i = 0; i < StorageRecks.Count; i++ )
            {
                if ( item.Parent.GetID() == StorageRecks[i].GetID() )
                {
                    for( int j = 0; j < Data.StorageRecks[i].GetItems().Length; j++ )
                    {
                        if ( Data.StorageRecks[i].GetItems()[j].IDRef == item.GetID() )
                        {
                            ProjectTransformationData data = new ProjectTransformationData( item.Position, item.Rotation, item.Scale );

                            Data.StorageRecks[ i ].RemoveItem( Data.StorageRecks[ i ].GetItems( )[ j ] );
                            Data.StorageRecks[ i ].AddItem( j, new ProjectItemData( item.GetID(), new ProjectTransformationData( item.Position, item.Rotation, item.Scale ) ) );

                            break;
                        }
                    }

                    break;
                }
            }
        }
    }
}
