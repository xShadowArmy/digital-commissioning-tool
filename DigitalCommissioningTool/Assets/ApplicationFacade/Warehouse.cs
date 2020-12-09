using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using AppData.Warehouse;
using SystemFacade;
using UnityEngine;

namespace ApplicationFacade
{
    /// <summary>
    /// Stellt ein Lagerhaus in der 3D-Umgebung dar.
    /// </summary>
    public class Warehouse
    {
        /// <summary>
        /// EventHandler Definition für StorageRack events.
        /// </summary>
        /// <param name="data">Das Regal das verändert wurde.</param>
        public delegate void StorageRackModifiedEventHandler( StorageData data );

        /// <summary>
        /// Event das ausgelöst wird, wenn ein Regal erstellt wurde.
        /// </summary>
        public event StorageRackModifiedEventHandler StorageRackCreated;

        /// <summary>
        /// Event das ausgelöst wird, wenn ein Regal entfernt wurde.
        /// </summary>
        public event StorageRackModifiedEventHandler StorageRackDeleted;
        
        internal List<FloorData> Floor { get; private set; }

        internal List<WallData> Walls { get; private set; }

        internal List<WindowData> Windows { get; private set; }

        internal List<DoorData> Doors { get; private set; }

        internal List<StorageData> StorageRacks { get; private set; }

        internal InternalProjectWarehouse Data { get; private set; }

        internal Warehouse()
        {
            Floor   = new List<FloorData>( );
            Walls   = new List<WallData>( );
            Windows = new List<WindowData>( );
            Doors   = new List<DoorData>( );
            StorageRacks = new List<StorageData>( );
            Data = new InternalProjectWarehouse( );
        }

        public FloorData CreateFloor( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Lagehausboden wird erstellt.", "Warehouse", "CreateFloor" );
            
            FloorData floor = new FloorData( GetUniqueID( Floor.ToArray() ), position, rotation, scale );
                       
            floor.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableFloor" ), position, rotation, GameObject.FindWithTag( "FloorDefinition" ).transform ) );
            
            floor.Object.name = "Floor" + floor.GetID( );
            
            floor.GameObjectDataChanged += GameObjectHasChanged;
            floor.FloorChanged += FloorHasChanged;
            
            Floor.Add( floor );
            
            Data.Floor.Add( new ProjectFloorData( floor.GetID( ), new ProjectTransformationData( position, rotation, scale ) ) );

            return floor;
        }

        internal void AddFloor( FloorData floor )
        {
            LogManager.WriteInfo( "Lagerhausboden wird hinzugefuegt.", "Warehouse", "AddFloor" );

            floor.GameObjectDataChanged += GameObjectHasChanged;
            floor.FloorChanged += FloorHasChanged;

            Floor.Add( floor );

            Data.Floor.Add( new ProjectFloorData( floor.GetID( ), new ProjectTransformationData( floor.Position, floor.Rotation, floor.Scale ) ) );
        }

        public bool RemoveFloor( FloorData floor )
        {
            LogManager.WriteInfo( "Lagerhausboden wird entfernt.", "Warehouse", "RemoveFloor" );

            if ( !Floor.Remove( floor ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.Floor.Count; i++ )
            {
                if ( Data.Floor[ i ].ID == floor.GetID( ) )
                {
                    Data.Floor.Remove( Data.Floor[ i ] );

                    floor.GameObjectDataChanged -= GameObjectHasChanged;
                    floor.FloorChanged -= FloorHasChanged;

                    floor.Destroy( );

                    return true;
                }
            }

            return false;
        }

        public FloorData GetFloor( long id )
        {
            LogManager.WriteInfo( "Lagerhausboden wird abgefragt.", "Warehouse", "GetFloor" );

            for ( int i = 0; i < Floor.Count; i++ )
            {
                if ( Floor[ i ].GetID( ) == id )
                {
                    return Floor[ i ];
                }
            }

            return null;
        }

        public FloorData GetFloor( GameObject obj )
        {
            LogManager.WriteInfo( "Lagerhausboden wird abgefragt.", "Warehouse", "GetFloor" );

            for ( int i = 0; i < Walls.Count; i++ )
            {
                if ( Floor[ i ].Object == obj )
                {
                    return Floor[ i ];
                }
            }

            return null;
        }
        
        public WallData CreateWall( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Lagehauswand wird erstellt.", "Warehouse", "CreateWall" );
            
            WallData wall = new WallData( GetUniqueID( Walls.ToArray() ), position, rotation, scale );

            wall.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableWall" ), position, rotation, GameObject.FindGameObjectWithTag( "WallDefinition" ).transform ) );
            
            wall.Object.name = "Wall" + wall.GetID();

            wall.GameObjectDataChanged += GameObjectHasChanged;
            wall.WallChanged += WallHasChanged;

            Walls.Add( wall );

            Data.Walls.Add( new ProjectWallData( wall.GetID(), new ProjectTransformationData( position, rotation, scale ) ) );
            
            return wall;
        }

        internal void AddWall( WallData wall )
        {
            LogManager.WriteInfo( "Lagerhauswand wird hinzugefuegt.", "Warehouse", "AddWall" );

            wall.GameObjectDataChanged += GameObjectHasChanged;
            wall.WallChanged += WallHasChanged;

            Walls.Add( wall );

            Data.Walls.Add( new ProjectWallData( wall.GetID( ), new ProjectTransformationData( wall.Position, wall.Rotation, wall.Scale ) ) );
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

                    wall.Destroy( );

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

        public WindowData CreateWindow( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Lagerhausfenster wird erstellt.", "Warehouse", "CreateWindow" );

            WindowData window = new WindowData( GetUniqueID( Windows.ToArray( ) ), position, rotation, scale );

            window.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableWindow" ), position, rotation, GameObject.FindGameObjectWithTag( "WindowDefinition" ).transform ) );

            window.Object.name = "Window" + window.GetID( );

            window.GameObjectDataChanged += GameObjectHasChanged;
            window.WindowChanged += WindowHasChanged;

            Windows.Add( window );

            Data.Windows.Add( new ProjectWindowData( window.GetID( ), new ProjectTransformationData( position, rotation, scale ) ) );

            return window;
        }

        internal void AddWindow( WindowData window )
        {
            LogManager.WriteInfo( "Lagehaus Fenster wird hinzugefuegt.", "Warehouse", "AddWindow" );

            window.GameObjectDataChanged += GameObjectHasChanged;
            window.WindowChanged += WindowHasChanged;

            Windows.Add( window );

            Data.Windows.Add( new ProjectWindowData( window.GetID( ), new ProjectTransformationData( window.Position, window.Rotation, window.Scale ) ) );
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

                    window.Destroy( );

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

        public DoorData CreateDoor( Vector3 position, Quaternion rotation, Vector3 scale, DoorType type )
        {
            LogManager.WriteInfo( "Lagerhaustuer wird erstellt.", "Warehouse", "CreateDoor" );

            DoorData door = new DoorData( GetUniqueID( Doors.ToArray( ) ), type, position, rotation, scale );

            door.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableDoor" ), position, rotation, GameObject.FindGameObjectWithTag( "DoorDefinition" ).transform ) );

            door.Object.name = "Door" + door.GetID( );

            door.GameObjectDataChanged += GameObjectHasChanged;
            door.DoorChanged += DoorHasChanged;

            Doors.Add( door );

            Data.Doors.Add( new ProjectDoorData( door.GetID( ), type.ToString(), new ProjectTransformationData( position, rotation, scale ) ) );

            return door;
        }

        internal void AddDoor( DoorData door )
        {
            LogManager.WriteInfo( "Lagehaustuer wird hinzugefuegt.", "Warehouse", "AddDoor" );

            door.GameObjectDataChanged += GameObjectHasChanged;
            door.DoorChanged += DoorHasChanged;

            Doors.Add( door );

            Data.Doors.Add( new ProjectDoorData( door.GetID( ), door.Type.ToString(), new ProjectTransformationData( door.Position, door.Rotation, door.Scale ) ) );
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

                    door.Destroy( );

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

        public StorageData CreateStorageRack( )
        {
            LogManager.WriteInfo( "Lagerhausregal wird erstellt.", "Warehouse", "CreateStorageRack" );
            
            GameObject spawn = GameObject.Find( "ObjectSpawn" );
            
            StorageData storage = new StorageData( GetUniqueID( StorageRacks.ToArray( ) ), spawn.transform.position, spawn.transform.rotation, spawn.transform.localScale );

            storage.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableStorage" ), spawn.transform.position, Quaternion.Euler( 0, 90, 0 ), GameObject.FindGameObjectWithTag( "StorageRackDefinition" ).transform ) );

            storage.Object.name = "Storage" + storage.GetID( );

            storage.GameObjectDataChanged += GameObjectHasChanged;
            storage.StorageChanged += StorageRackHasChanged;

            StorageRacks.Add( storage );

            Data.StorageRacks.Add( new ProjectStorageData( storage.GetID( ), new ProjectTransformationData( spawn.transform.position, spawn.transform.rotation, spawn.transform.localScale ) ) );

            OnStorageRackModified( 0, storage );

            return storage;
        }

        internal void AddStorageRack( StorageData storage )
        {
            LogManager.WriteInfo( "Lagerhausregal wird hinzugefuegt.", "Warehouse", "AddStorageRack" );

            storage.GameObjectDataChanged += GameObjectHasChanged;
            storage.StorageChanged += StorageRackHasChanged;

            StorageRacks.Add( storage );

            Data.StorageRacks.Add( new ProjectStorageData( storage.GetID( ), new ProjectTransformationData( storage.Position, storage.Rotation, storage.Scale ) ) );
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

                    OnStorageRackModified( 1, storage );

                    storage.Destroy( );

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
        
        public ItemData CreateStorageRackItem( Vector3 position, Quaternion rotation, Vector3 scale, StorageData storage )
        {
            LogManager.WriteInfo( "Ein RegalItem wird erstellt.", "Warehouse", "CreateStorageRackItem" );

            ItemData item = new ItemData( 0, 0, position, rotation, scale );

            item.GameObjectDataChanged += GameObjectHasChanged;
            item.ItemChanged += StorageRackItemHasChanged;

            item.SetParent( storage );
            storage.AddItem( item );

            // item.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableStorage" ), spawn.transform.position, Quaternion.Euler( 0, 90, 0 ), GameObject.FindGameObjectWithTag( "StorageRackDefinition" ).transform ) );

            //item.Object.name = "Item_" + item.Name + "_" + item.GetID( );

            for ( int i = 0; i < Data.StorageRacks.Count; i++ )
            {
                if ( Data.StorageRacks[ i ].ID == storage.GetID() )
                {
                    Data.StorageRacks[ i ].Items.Add( new ProjectItemData( 0, new ProjectTransformationData( position, rotation, scale ) ) );
                                       
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
                    Data.StorageRacks[ i ].Items.Add( new ProjectItemData( item.GetID(), new ProjectTransformationData( item.Position, item.Rotation, item.Scale ) ) );

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
                    for( int j = 0; j < Data.StorageRacks[i].GetItems.Length; j++ )
                    {
                        if ( Data.StorageRacks[i].GetItems[j].IDRef == item.GetID() )
                        {
                            Data.StorageRacks[ i ].Items.Remove( Data.StorageRacks[ i ].GetItems[ j ] );

                            item.GameObjectDataChanged -= GameObjectHasChanged;
                            item.ItemChanged -= StorageRackItemHasChanged;

                            item.Destroy( );

                            return true;
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

            for ( int i = 0; i < Data.Floor.Count; i++ )
            {
                if ( floor.GetID( ) == Data.Floor[ i ].ID )
                {
                    Data.Floor.Remove( Data.Floor[ i ] );
                    Data.Floor.Insert( i, new ProjectFloorData( floor.GetID( ), new ProjectTransformationData( floor.Position, floor.Rotation, floor.Scale ) ) );

                    return;
                }
            }
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
                    if ( storage.GetItems.Length > Data.StorageRacks[i].GetItems.Length )
                    {
                        ProjectTransformationData data = new ProjectTransformationData( storage.GetItems[ storage.GetItems.Length - 1 ].Position,
                                                                                        storage.GetItems[ storage.GetItems.Length - 1 ].Rotation,
                                                                                        storage.GetItems[ storage.GetItems.Length - 1 ].Scale );
                        
                        Data.StorageRacks[ i ].Items.Add( new ProjectItemData( storage.GetItems[ storage.GetItems.Length - 1 ].GetID( ), data ) );

                        break;
                    }

                    else if ( storage.GetItems.Length < Data.StorageRacks[ i ].GetItems.Length )
                    {
                        if ( storage.GetItems.Length == 1 )
                        {
                            Data.StorageRacks[ i ].Items.Remove( Data.StorageRacks[ i ].GetItems[ 0 ] );

                            break;
                        }

                        for ( int j = 0; j < Data.StorageRacks[i].GetItems.Length; j++ )
                        {
                            if ( Data.StorageRacks[i].GetItems[j].IDRef != storage.GetItems[j].GetID() )
                            {
                                Data.StorageRacks[ i ].Items.Remove( Data.StorageRacks[i].GetItems[j] );

                                break;
                            }
                        }

                        break;
                    }

                    else
                    {
                        ProjectItemData[ ] items = Data.StorageRacks[ i ].GetItems;
                        ProjectStorageData data = new ProjectStorageData( storage.GetID(), new ProjectTransformationData( storage.Position, storage.Rotation, storage.Scale ) );

                        foreach( ProjectItemData item in items )
                        {
                            data.Items.Add( item );
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
                    for( int j = 0; j < Data.StorageRacks[i].GetItems.Length; j++ )
                    {
                        if ( Data.StorageRacks[i].GetItems[j].IDRef == item.GetID() )
                        {
                            ProjectTransformationData data = new ProjectTransformationData( item.Position, item.Rotation, item.Scale );

                            Data.StorageRacks[ i ].Items.Remove( Data.StorageRacks[ i ].GetItems[ j ] );
                            Data.StorageRacks[ i ].Items.Insert( j, new ProjectItemData( item.GetID(), new ProjectTransformationData( item.Position, item.Rotation, item.Scale ) ) );

                            break;
                        }
                    }

                    break;
                }
            }
        }

        private void OnStorageRackModified( int mode, StorageData data )
        {
            if ( mode == 0 )
            {
                StorageRackCreated?.Invoke( data );
            }

            else
            {
                StorageRackDeleted?.Invoke( data );
            }
        }
    }
}
