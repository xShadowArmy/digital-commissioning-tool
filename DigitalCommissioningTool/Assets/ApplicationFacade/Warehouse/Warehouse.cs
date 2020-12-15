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

        /// <summary>
        /// Objekt das eine Position vorgibt an dem die neue Objekte gespawnt werden.
        /// </summary>
        public GameObject ObjectSpawn { get; internal set; }

        /// <summary>
        /// Enthält alle Böden die aktuell in der Umgebung dargestellt werden.
        /// </summary>
        internal List<FloorData> Floor { get; private set; }

        /// <summary>
        /// Enthält alle Wände die aktuell in der Umgebung dargestellt werden.
        /// </summary>
        internal List<WallData> Walls { get; private set; }

        /// <summary>
        /// Enthält alle Fenster die aktuell in der Umgebung dargestellt werden.
        /// </summary>
        internal List<WindowData> Windows { get; private set; }

        /// <summary>
        /// Enthält alle Türen die aktuell in der Umgebung dargestellt werden.
        /// </summary>
        internal List<DoorData> Doors { get; private set; }

        /// <summary>
        /// Enthält alle Lagerregale die aktuell in der Umgebung dargestellt werden.
        /// </summary>
        internal List<StorageData> StorageRackList { get; private set; }

        /// <summary>
        /// Interne Struktur die alle Objekte protokolliert.
        /// </summary>
        internal InternalProjectWarehouse Data { get; private set; }

        public StorageData[] StorageRacks
        {
            get
            {
                return StorageRackList.ToArray( );
            }
        }

        /// <summary>
        /// Erstellt eine neue Instanz und initialisiert alle Objekte.
        /// </summary>
        internal Warehouse()
        {
            Floor = new List<FloorData>( );
            Walls = new List<WallData>( );
            Windows = new List<WindowData>( );
            Doors = new List<DoorData>( );
            StorageRackList = new List<StorageData>( );
            Data = new InternalProjectWarehouse( );

            ObjectSpawn = GameObject.FindGameObjectWithTag( "Respawn" );
        }

        // Floor

        /// <summary>
        /// Erstellt einen Boden an dem Objektspawn.
        /// </summary>
        /// <returns>Ein <see cref="FloorData"/> Objekt, dass den Boden er erstellt wurde darstellt.</returns>
        public FloorData CreateFloor( )
        {
            LogManager.WriteInfo( "Lagehausboden wird erstellt.", "Warehouse", "CreateFloor" );

            FloorData floor = new FloorData( GetUniqueID( Floor.ToArray( ) ), ObjectSpawn.transform.position, ObjectSpawn.transform.rotation, ObjectSpawn.transform.localScale );

            CreateFloorObject( floor );

            return floor;
        }

        /// <summary>
        /// Erstellt einen Boden an der gegebenen Position.
        /// </summary>
        /// <param name="position">Die Position an der der Boden erstellt wird.</param>
        /// <param name="rotation">Die Rotation des Bodens.</param>
        /// <param name="scale">Die Skalierung des Bodens.</param>
        /// <returns>Ein <see cref="FloorData"/> Objekt, dass den Boden er erstellt wurde darstellt.</returns>
        public FloorData CreateFloor( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Lagehausboden wird erstellt.", "Warehouse", "CreateFloor" );
            
            FloorData floor = new FloorData( GetUniqueID( Floor.ToArray() ), position, rotation, scale );

            CreateFloorObject( floor );

            return floor;
        }

        /// <summary>
        /// Fügt einen Boden zur Protokollierung hinzu und stellt diesen in der Umgebung dar.
        /// </summary>
        /// <param name="floor">Der Boden der Protokolliert und dargestellt werden soll.</param>
        internal void AddFloor( FloorData floor )
        {
            LogManager.WriteInfo( "Lagerhausboden wird hinzugefuegt.", "Warehouse", "AddFloor" );

            CreateFloorObject( floor );
        }

        /// <summary>
        /// Zerstört den angegebenen Boden und entfernt diesen aus der Umgebung.
        /// </summary>
        /// <param name="floor">Der Boden der zerstört werden soll.</param>
        /// <returns>Gibt true zurück wenn der Boden erfolgreich gefunden und entfernt wurde.</returns>
        public bool RemoveFloor( FloorData floor )
        {
            LogManager.WriteInfo( "Lagerhausboden wird entfernt.", "Warehouse", "RemoveFloor" );
            
            return DestroyFloorObject( floor );
        }

        /// <summary>
        /// Gibt den Boden mit der angegebenen ID zurück.
        /// </summary>
        /// <param name="id">Die ID des Bodens.</param>
        /// <returns>Der Boden mit der ID oder null.</returns>
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

        /// <summary>
        /// Gibt den Boden mit dem angegebenen GameObject zurück
        /// </summary>
        /// <param name="obj">Das Objekt eines Bodens.</param>
        /// <returns>Der Boden mit dem GameObjekt oder null.</returns>
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

        // Wall

        
        public WallData CreateWall( Vector3 position, Quaternion rotation, Vector3 scale, WallFace face, WallClass wClass )
        {
            LogManager.WriteInfo( "Lagehauswand wird erstellt.", "Warehouse", "CreateWall" );

            WallData wall = new WallData( GetUniqueID( Walls.ToArray( ) ), position, rotation, scale )
            {
                Face = face,
                Class = wClass
            };

            CreateWallObject( wall );

            return wall;
        }

        internal void AddWall( WallData wall )
        {
            LogManager.WriteInfo( "Lagerhauswand wird hinzugefuegt.", "Warehouse", "AddWall" );

            CreateWallObject( wall );
        }

        public bool RemoveWall( WallData wall )
        {
            LogManager.WriteInfo( "Lagerhauswand wird entfernt.", "Warehouse", "RemoveWall" );

            return DestroyWallObject( wall );
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

        // Window

        
        public WindowData CreateWindow()
        {
            LogManager.WriteInfo( "Lagerhausfenster wird erstellt.", "Warehouse", "CreateWindow" );

            WindowData window = new WindowData( GetUniqueID( Windows.ToArray( ) ), ObjectSpawn.transform.position, ObjectSpawn.transform.rotation, ObjectSpawn.transform.localScale );

            CreateWindowObject( window );

            return window;
        }

        public WindowData CreateWindow( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Lagerhausfenster wird erstellt.", "Warehouse", "CreateWindow" );

            WindowData window = new WindowData( GetUniqueID( Windows.ToArray( ) ), position, rotation, scale );

            CreateWindowObject( window );

            return window;
        }

        internal void AddWindow( WindowData window )
        {
            LogManager.WriteInfo( "Lagehaus Fenster wird hinzugefuegt.", "Warehouse", "AddWindow" );

            CreateWindowObject( window );
        }

        public bool RemoveWindow( WindowData window )
        {
            LogManager.WriteInfo( "Lagehaus Fenster wird entfernt.", "Warehouse", "RemoveWindow" );
            
            return DestroyWindowObject( window );
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

        // Door

        
        public DoorData CreateDoor( DoorType type )
        {
            LogManager.WriteInfo( "Lagerhaustuer wird erstellt.", "Warehouse", "CreateDoor" );

            DoorData door = new DoorData( GetUniqueID( Doors.ToArray( ) ), ObjectSpawn.transform.position, ObjectSpawn.transform.rotation, ObjectSpawn.transform.localScale )
            {
                Type = type
            };

            door.SetDoorType( type );

            CreateDoorObject( door );

            return door;
        }

        public DoorData CreateDoor( Vector3 position, Quaternion rotation, Vector3 scale, DoorType type )
        {
            LogManager.WriteInfo( "Lagerhaustuer wird erstellt.", "Warehouse", "CreateDoor" );

            DoorData door = new DoorData( GetUniqueID( Doors.ToArray( ) ), position, rotation, scale )
            {
                Type = type
            };

            door.SetDoorType( type );

            CreateDoorObject( door );

            return door;
        }

        internal void AddDoor( DoorData door )
        {
            LogManager.WriteInfo( "Lagehaustuer wird hinzugefuegt.", "Warehouse", "AddDoor" );

            CreateDoorObject( door );
        }

        public bool RemoveDoor( DoorData door )
        {
            LogManager.WriteInfo( "Lagerhaustuer wird entfernt.", "Warehouse", "RemoveDoor" );
            
            return DestroyDoorObject( door );
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

        // StorageRack

        
        public StorageData CreateStorageRack( )
        {
            LogManager.WriteInfo( "Lagerhausregal wird erstellt.", "Warehouse", "CreateStorageRack" );

            StorageData storage = new StorageData( GetUniqueID( StorageRacks.ToArray( ) ), ObjectSpawn.transform.position, ObjectSpawn.transform.rotation, ObjectSpawn.transform.localScale )
            {                
            };

            CreateStorageRackObject( storage );

            return storage;
        }

        public StorageData CreateStorageRack( int slotCount )
        {
            LogManager.WriteInfo( "Lagerhausregal wird erstellt.", "Warehouse", "CreateStorageRack" );

            StorageData storage = new StorageData( GetUniqueID( StorageRacks.ToArray( ) ), ObjectSpawn.transform.position, ObjectSpawn.transform.rotation, ObjectSpawn.transform.localScale )
            {
                SlotCount = slotCount
            };

            CreateStorageRackObject( storage );

            return storage;
        }

        public StorageData CreateStorageRack( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Lagerhausregal wird erstellt.", "Warehouse", "CreateStorageRack" );
                        
            StorageData storage = new StorageData( GetUniqueID( StorageRacks.ToArray( ) ), position, rotation, scale );

            CreateStorageRackObject( storage );

            return storage;
        }

        public StorageData CreateStorageRack( Vector3 position, Quaternion rotation, Vector3 scale, int slotCount )
        {
            LogManager.WriteInfo( "Lagerhausregal wird erstellt.", "Warehouse", "CreateStorageRack" );

            StorageData storage = new StorageData( GetUniqueID( StorageRacks.ToArray( ) ), position, rotation, scale )
            {
                SlotCount = slotCount
            };

            CreateStorageRackObject( storage );

            return storage;
        }

        internal void AddStorageRack( StorageData storage )
        {
            LogManager.WriteInfo( "Lagerhausregal wird hinzugefuegt.", "Warehouse", "AddStorageRack" );

            CreateStorageRackObject( storage );
        }

        public bool RemoveStorageRack( StorageData storage )
        {
            LogManager.WriteInfo( "Lagerhausregal wird entfernt.", "Warehouse", "RemoveStorageRack" );

            DestroyStorageRackObject( storage );

            return false;
        }

        public StorageData GetStorageRack( long id )
        {
            LogManager.WriteInfo( "Lagerhausregal wird abgefragt.", "Warehouse", "GetStorageRack" );

            for ( int i = 0; i < StorageRackList.Count; i++ )
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

            for ( int i = 0; i < StorageRackList.Count; i++ )
            {
                if ( StorageRacks[ i ].Object == obj )
                {
                    return StorageRacks[ i ];
                }
            }

            return null;
        }
        
        // StorageRackItem
         
        public void AddItemToStorageRack( StorageData storage, ItemData item, int slot )
        {
            LogManager.WriteInfo( "Ein RegalItem wird hinzugefuegt.", "Warehouse", "AddItemToStorageRack" );
            
        }

        public bool RemoveItemFromStorageRack( StorageData storage, ItemData item )
        {
            LogManager.WriteInfo( "Ein RegalItem wird entfernt.", "Warehouse", "RemoveItemFromStorageRack" );
            

            return false;
        }
        
        public ItemData GetStorageRackItem( GameObject obj )
        {
            LogManager.WriteInfo( "Lagerhausregal wird abgefragt.", "Warehouse", "GetStorageRack" );

            for ( int i = 0; i < StorageRackList.Count; i++ )
            {
                for ( int j = 0; j < StorageRackList[i].GetItems.Length; j++ )
                {
                    if ( StorageRackList[i].GetItems[j].Object == obj )
                    {
                        return StorageRackList[i].GetItems[j];
                    }
                }
            }

            return null;
        }

        // Implementierungen

        internal static long GetUniqueID( IDataIdentifier[] idUsed )
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
        
        private void CreateFloorObject( FloorData data )
        {
            data.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableFloor" ), data.Position, data.Rotation, GameObject.FindGameObjectWithTag( "FloorDefinition" ).transform ) );

            data.Object.name = "Floor" + data.GetID( );

            data.GameObjectDataChanged += GameObjectHasChanged;
            data.FloorChanged += FloorHasChanged;

            Floor.Add( data );

            Data.Floor.Add( new ProjectFloorData( data.GetID( ), new ProjectTransformationData( data.Position, data.Rotation, data.Scale ) ) );
        }

        private bool DestroyFloorObject( FloorData data )
        {
            if ( !Floor.Remove( data ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.Floor.Count; i++ )
            {
                if ( Data.Floor[i].ID == data.GetID( ) )
                {
                    Data.Floor.Remove( Data.Floor[i] );

                    data.GameObjectDataChanged -= GameObjectHasChanged;
                    data.FloorChanged -= FloorHasChanged;

                    data.Destroy( );

                    return true;
                }
            }

            return false;
        }

        private void CreateWallObject( WallData data )
        {
            data.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableWall" ), data.Position, data.Rotation, GameObject.FindGameObjectWithTag( data.Face.ToString( ) + "Wall" ).transform ) );

            data.Object.name = "Wall" + data.GetID( );

            data.GameObjectDataChanged += GameObjectHasChanged;
            data.WallChanged += WallHasChanged;

            Walls.Add( data );

            Data.Walls.Add( new ProjectWallData( data.GetID( ), data.Face.ToString( ), data.Class.ToString( ), new ProjectTransformationData( data.Position, data.Rotation, data.Scale ) ) );

            if ( data.Class == WallClass.Outer )
            {
                switch ( data.Face )
                {
                    case WallFace.North:

                        WallData.NorthWallLength += 1;
                        break;

                    case WallFace.East:

                        WallData.EastWallLength += 1;
                        break;

                    case WallFace.South:

                        WallData.SouthWallLength += 1;
                        break;

                    case WallFace.West:

                        WallData.WesthWallLength += 1;
                        break;

                    case WallFace.Undefined:

                        LogManager.WriteWarning( "Es wird eine undefinierte Wandseite verwendet!", "Warehouse", "CreateWallObject" );
                        break;
                }
            }
        }

        private bool DestroyWallObject( WallData data )
        {
            if ( !Walls.Remove( data ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.Walls.Count; i++ )
            {
                if ( Data.Walls[i].ID == data.GetID( ) )
                {
                    Data.Walls.Remove( Data.Walls[i] );

                    data.GameObjectDataChanged -= GameObjectHasChanged;
                    data.WallChanged -= WallHasChanged;

                    switch ( data.Face )
                    {
                        case WallFace.North:

                            WallData.NorthWallLength -= 1;
                            break;

                        case WallFace.East:

                            WallData.EastWallLength -= 1;
                            break;

                        case WallFace.South:

                            WallData.SouthWallLength -= 1;
                            break;

                        case WallFace.West:

                            WallData.WesthWallLength -= 1;
                            break;

                        case WallFace.Undefined:

                            LogManager.WriteWarning( "Es wird eine undefinierte Wandseite verwendet!", "Warehouse", "CreateWallObject" );
                            break;
                    }

                    data.Destroy( );

                    return true;
                }
            }

            return false;
        }

        private void CreateWindowObject( WindowData data )
        {
            data.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableWindow" ), data.Position, data.Rotation, GameObject.FindGameObjectWithTag( "WindowDefinition" ).transform ) );

            data.Object.name = "Window" + data.GetID( );

            data.GameObjectDataChanged += GameObjectHasChanged;
            data.WindowChanged += WindowHasChanged;

            Windows.Add( data );

            Data.Windows.Add( new ProjectWindowData( data.GetID( ), new ProjectTransformationData( data.Position, data.Rotation, data.Scale ) ) );
        }

        private bool DestroyWindowObject( WindowData data )
        {
            if ( !Windows.Remove( data ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.Windows.Count; i++ )
            {
                if ( Data.Windows[i].ID == data.GetID( ) )
                {
                    Data.Windows.Remove( Data.Windows[i] );

                    data.GameObjectDataChanged -= GameObjectHasChanged;
                    data.WindowChanged -= WindowHasChanged;

                    data.Destroy( );

                    return true;
                }
            }

            return false;
        }

        private void CreateDoorObject( DoorData data )
        {
            data.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableDoor" ), data.Position, data.Rotation, GameObject.FindGameObjectWithTag( "DoorDefinition" ).transform ) );

            data.Object.name = "Door" + data.GetID( );

            data.GameObjectDataChanged += GameObjectHasChanged;
            data.DoorChanged += DoorHasChanged;

            Doors.Add( data );

            Data.Doors.Add( new ProjectDoorData( data.GetID( ), data.Type.ToString( ), new ProjectTransformationData( data.Position, data.Rotation, data.Scale ) ) );
        }

        private bool DestroyDoorObject( DoorData data )
        {
            if ( !Doors.Remove( data ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.Doors.Count; i++ )
            {
                if ( Data.Doors[i].ID == data.GetID( ) )
                {
                    Data.Doors.Remove( Data.Doors[i] );

                    data.GameObjectDataChanged -= GameObjectHasChanged;
                    data.DoorChanged -= DoorHasChanged;

                    data.Destroy( );

                    return true;
                }
            }

            return false;
        }

        private void CreateStorageRackObject( StorageData data )
        {
            data.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableStorage" ), data.Position, data.Rotation, GameObject.FindGameObjectWithTag( "StorageRackDefinition" ).transform ) );

            data.Object.name = "Storage" + data.GetID( );

            data.GameObjectDataChanged += GameObjectHasChanged;
            data.StorageChanged += StorageRackHasChanged;

            StorageRackList.Add( data );

            Data.StorageRacks.Add( new ProjectStorageData( data.GetID( ), data.SlotCount, new ProjectTransformationData( data.Position, data.Rotation, data.Scale ) ) );

            OnStorageRackModified( 0, data );
        }

        private bool DestroyStorageRackObject( StorageData data )
        {
            if ( !StorageRackList.Remove( data ) )
            {
                return false;
            }

            for ( int i = 0; i < Data.StorageRacks.Count; i++ )
            {
                if ( Data.StorageRacks[i].ID == data.GetID( ) )
                {
                    Data.StorageRacks.Remove( Data.StorageRacks[i] );

                    data.GameObjectDataChanged -= GameObjectHasChanged;
                    data.StorageChanged -= StorageRackHasChanged;

                    OnStorageRackModified( 1, data );

                    data.Destroy( );

                    return true;
                }
            }

            return false;
        }

        // Eventhandler

        
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
                    Data.Walls.Insert( i, new ProjectWallData( wall.GetID(), wall.Face.ToString(), wall.Class.ToString(), new ProjectTransformationData( wall.Position, wall.Rotation, wall.Scale ) ) );

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
                        
                        Data.StorageRacks[ i ].Items.Add( new ProjectItemData( storage.GetItems[ storage.GetItems.Length - 1 ].GetID( ),
                                                                               storage.GetItems[storage.GetItems.Length - 1].Count,
                                                                               storage.GetItems[storage.GetItems.Length - 1].Weight,
                                                                               storage.GetItems[storage.GetItems.Length - 1].Name,
                                                                               data ) );

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
                        ProjectStorageData data = new ProjectStorageData( storage.GetID(), Data.StorageRacks[i].SlotCount, new ProjectTransformationData( storage.Position, storage.Rotation, storage.Scale ) );

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

            List<ItemData> items = new List<ItemData>( );

            foreach( StorageData sdata in StorageRacks )
            {
                foreach( ItemData idata in sdata.GetItems )
                {
                    if ( idata.GetID() == item.GetID() )
                    {

                        items.Add( idata );
                    }
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
