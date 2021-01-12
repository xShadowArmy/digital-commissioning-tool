using System.Collections.Generic;
using System.Linq;
using AppData.Warehouse;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ApplicationFacade.Warehouse
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
        /// Enthält alle Objekte aus der Nordwand.
        /// </summary>
        internal List<WallObjectData> NorthWall { get; set; }

        /// <summary>
        /// Enthält alle Objekte aus der Ostwand.
        /// </summary>
        internal List<WallObjectData> EastWall { get; set; }

        /// <summary>
        /// Enthält alle Objekte aus der Südwand.
        /// </summary>
        internal List<WallObjectData> SouthWall { get; set; }

        /// <summary>
        /// Enthält alle Objekte aus der Westwand.
        /// </summary>
        internal List<WallObjectData> WestWall { get; set; }

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
        
        /// <summary>
        /// Gibt alle Regal Objekte die aktuell im Lagerhaus stehen zurück.
        /// </summary>
        public StorageData[] StorageRacks
        {
            get
            {
                return StorageRackList.ToArray( );
            }
        }

        /// <summary>
        /// Gibt alle Objekte aus der Nordwand zurück.
        /// </summary>
        public WallObjectData[] NorthWallObjects
        {
            get
            {
                return NorthWall.ToArray();
            }
        }

        /// <summary>
        /// Gibt alle Objekte aus der Ostwand zurück.
        /// </summary>
        public WallObjectData[] EastWallObjects
        {
            get
            {
                return EastWall.ToArray( );
            }
        }

        /// <summary>
        /// Gibt alle Objekte aus der Südwand zurück.
        /// </summary>
        public WallObjectData[] SouthWallObjects
        {
            get
            {
                return SouthWall.ToArray( );
            }
        }

        /// <summary>
        /// Gibt alle Objekte aus der Westwand zuück.
        /// </summary>
        public WallObjectData[] WestWallObjects
        {
            get
            {
                return WestWall.ToArray( );
            }
        }

        /// <summary>
        /// Gibt an ob das Lager zerstört wurde.
        /// </summary>
        private bool Destroyed;

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

            NorthWall = new List<WallObjectData>( );
            EastWall  = new List<WallObjectData>( );
            SouthWall = new List<WallObjectData>( );
            WestWall  = new List<WallObjectData>( );

            Destroyed = false;

            ObjectSpawn = GameObject.FindGameObjectWithTag( "Respawn" );
        }

        ~Warehouse()
        {
            DestroyWarehouse( );
        }

        // --- Floor ---

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

            if ( Floor.Contains( floor ) )
            {
                return;
            }

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
            for ( int i = 0; i < Walls.Count; i++ )
            {
                if ( Floor[ i ].Object == obj )
                {
                    return Floor[ i ];
                }
            }

            return null;
        }

        // --- Wall ---

        /// <summary>
        /// Erstellt eine Wand mit den angegebenen Eigenschaften.
        /// </summary>
        /// <param name="position">Die Position der Wand.</param>
        /// <param name="rotation">Die Rotation der Wand.</param>
        /// <param name="scale">Die Skalierung der Wand.</param>
        /// <param name="face">Die Ausrichtung der Wand.</param>
        /// <param name="wClass">Gibt an ob die Wand das Lagerhaus oder einen internen Raum definiert.</param>
        /// <returns>Das <see cref="WallData"/> Objekt das die Wand repräsentiert.</returns>
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

        /// <summary>
        /// Erstellt eine Wand mit den angegebenen Eigenschaften.
        /// </summary>
        /// <param name="position">Die Position der Wand.</param>
        /// <param name="rotation">Die Rotation der Wand.</param>
        /// <param name="scale">Die Skalierung der Wand.</param>
        /// <param name="face">Die Ausrichtung der Wand.</param>
        /// <param name="wClass">Gibt an ob die Wand das Lagerhaus oder einen internen Raum definiert.</param>
        /// <param name="tag">Der Tag den das GameObjekt bekommen soll</param>
        /// <returns>Das <see cref="WallData"/> Objekt das die Wand repräsentiert.</returns>
        public WallData CreateWall( Vector3 position, Quaternion rotation, Vector3 scale, WallFace face, WallClass wClass, string tag )
        {
            LogManager.WriteInfo( "Lagehauswand wird erstellt.", "Warehouse", "CreateWall" );

            WallData wall = new WallData( GetUniqueID( Walls.ToArray( ) ), position, rotation, scale )
            {
                Face = face,
                Class = wClass
            };

            CreateWallObject( wall, tag );

            return wall;
        }

        /// <summary>
        /// Fügt ein Wandobjekt zur Lagerhalle hinzu.
        /// </summary>
        /// <param name="wall">Das Objekt das die Wand repräsentiert.</param>
        internal void AddWall( WallData wall, string tag )
        {
            LogManager.WriteInfo( "Lagerhauswand wird hinzugefuegt.", "Warehouse", "AddWall" );

            if ( tag == null || tag.Equals( string.Empty ) )
            {
                tag = "SelectableWall";
            }

            CreateWallObject( wall, tag );

            if ( wall.Class == WallClass.Inner )
            {
                wall.Object.transform.localScale = wall.Scale;
            }
        }

        /// <summary>
        /// Entfernt die angegebene Wand.
        /// </summary>
        /// <param name="wall">Die Wand die entfernt werden soll.</param>
        /// <returns>Gibt true zurück wenn erfolgreich.</returns>
        public bool RemoveWall( WallData wall )
        {
            LogManager.WriteInfo( "Lagerhauswand wird entfernt.", "Warehouse", "RemoveWall" );

            return DestroyWallObject( wall );
        }

        /// <summary>
        /// SUcht ein Wand Objekt anhand der gegebenen ID.
        /// </summary>
        /// <param name="id">Die ID des Wand Objekts.</param>
        /// <returns>Gibt das <see cref="WallData"/> Objekt das die Wand repräsentiert zurück oder null.</returns>
        public WallData GetWall( long id )
        {
            for ( int i = 0; i < Walls.Count; i++ )
            {
                if ( Walls[ i ].GetID() == id )
                {
                    return Walls[ i ];
                }
            }

            return null;
        }
  
        /// <summary>
        /// SUcht ein Wand Objekt anhand des GameObjects.
        /// </summary>
        /// <param name="obj">Das GameObject der Wand.</param>
        /// <returns>Gibt das <see cref="WallData"/> Objekt das die Wand repräsentiert zurück oder null.</returns>
        public WallData GetWall( GameObject obj )
        {
            for ( int i = 0; i < Walls.Count; i++ )
            {
                if ( Walls[ i ].Object == obj )
                {
                    return Walls[ i ];
                }
            }

            return null;
        }

        // --- Window ---

        /// <summary>
        /// Erstellt ein neues Fenster am default Spawnpunkt.
        /// </summary>
        /// <returns>Das <see cref="WindowData"/> Objekt dass das Fenster repräsentiert.</returns>
        public WindowData CreateWindow()
        {
            LogManager.WriteInfo( "Lagerhausfenster wird erstellt.", "Warehouse", "CreateWindow" );

            WindowData window = new WindowData( GetUniqueID( Windows.ToArray( ) ), ObjectSpawn.transform.position, ObjectSpawn.transform.rotation, ObjectSpawn.transform.localScale );

            CreateWindowObject( window );

            return window;
        }

        /// <summary>
        /// Erstellt ein neues Fenster mit den angegebenen Eigenschaften.
        /// </summary>
        /// <param name="position">Die Position des Fensters.</param>
        /// <param name="rotation">Die Rotation des Fensters.</param>
        /// <param name="scale">Die Skalierung des Fensters.</param>
        /// <returns>Gibt das <see cref="WindowData"/> Objekt zurück dass das Fenster repräsentiert.</returns>
        public WindowData CreateWindow( Vector3 position, Quaternion rotation, Vector3 scale, WallFace face, WallClass wClass )
        {
            LogManager.WriteInfo( "Lagerhausfenster wird erstellt.", "Warehouse", "CreateWindow" );

            WindowData window = new WindowData( GetUniqueID( Windows.ToArray( ) ), position, rotation, scale )
            {
                Face  = face,
                Class = wClass
            };

            CreateWindowObject( window );

            return window;
        }

        /// <summary>
        /// Fügt ein Fenster Objekt zur Lagerhalle hinzu.
        /// </summary>
        /// <param name="window">Das Fenster das hinzugefügt werden soll.</param>
        internal void AddWindow( WindowData window )
        {
            LogManager.WriteInfo( "Lagehaus Fenster wird hinzugefuegt.", "Warehouse", "AddWindow" );

            if ( Windows.Contains( window ) )
            {
                return;
            }

            CreateWindowObject( window );
        }

        /// <summary>
        /// Entfernt das angegebene Fenster.
        /// </summary>
        /// <param name="window">Das Fenster das entfernt werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
        public bool RemoveWindow( WindowData window )
        {
            LogManager.WriteInfo( "Lagehaus Fenster wird entfernt.", "Warehouse", "RemoveWindow" );
            
            return DestroyWindowObject( window );
        }
        
        /// <summary>
        /// SUcht ein Fenster Objekt anhand der gegebenen ID.
        /// </summary>
        /// <param name="id">Die ID des Fenster Objekts.</param>
        /// <returns>Gibt das <see cref="WindowData"/> Objekt das das Fenster repräsentiert zurück oder null.</returns>
        public WindowData GetWindow( long id )
        {
            for ( int i = 0; i < Windows.Count; i++ )
            {
                if ( Windows[ i ].GetID( ) == id )
                {
                    return Windows[ i ];
                }
            }

            return null;
        }
        
        /// <summary>
        /// SUcht ein Fenster Objekt anhand des GameObjects.
        /// </summary>
        /// <param name="obj">Das GameObject des Fenster Objekts.</param>
        /// <returns>Gibt das <see cref="WindowData"/> Objekt das das Fenster repräsentiert zurück oder null.</returns>
        public WindowData GetWindow( GameObject obj )
        {
            for ( int i = 0; i < Windows.Count; i++ )
            {
                if ( Windows[ i ].Object == obj )
                {
                    return Windows[ i ];
                }
            }

            return null;
        }

        // --- Door ---
        
        /// <summary>
        /// Erstellt eine neue Tür am default Spawnpunkt.
        /// </summary>
        /// <param name="type">Die Türart.</param>
        /// <returns>Das <see cref="DoorData"/> Objekt dass die Tür repräsentiert.</returns>
        public DoorData CreateDoor( DoorType type )
        {
            LogManager.WriteInfo( "Lagerhaustuer wird erstellt.", "Warehouse", "CreateDoor" );

            DoorData door = new DoorData( GetUniqueID( Doors.ToArray( ) ), ObjectSpawn.transform.position, ObjectSpawn.transform.rotation, ObjectSpawn.transform.localScale )
            {
                Type = type
            };

            CreateDoorObject( door );

            return door;
        }
        
        /// <summary>
        /// Erstellt eine neue Tür mit den angegebenen Eigenschaften.
        /// </summary>
        /// <param name="position">Die Position der Tür.</param>
        /// <param name="rotation">Die Rotation der Tür.</param>
        /// <param name="scale">Die Skalierung der Tür.</param>
        /// <param name="type">Die Türart.</param>
        /// <returns>Gibt das <see cref="DoorData"/> Objekt zurück dass die Tür repräsentiert.</returns>
        public DoorData CreateDoor( Vector3 position, Quaternion rotation, Vector3 scale, DoorType type, WallFace face, WallClass wClass )
        {
            LogManager.WriteInfo( "Lagerhaustuer wird erstellt.", "Warehouse", "CreateDoor" );

            DoorData door = new DoorData( GetUniqueID( Doors.ToArray( ) ), position, rotation, scale )
            {
                Type  = type,
                Face  = face,
                Class = wClass
            };

            CreateDoorObject( door );

            return door;
        }

        /// <summary>
        /// Fügt ein Tür Objekt zur Lagerhalle hinzu.
        /// </summary>
        /// <param name="door">Die Tür die hinzugefügt werden soll.</param>
        internal void AddDoor( DoorData door )
        {
            LogManager.WriteInfo( "Lagehaustuer wird hinzugefuegt.", "Warehouse", "AddDoor" );

            if ( Doors.Contains( door ) )
            {
                return;
            }

            CreateDoorObject( door );
        }
        
        /// <summary>
        /// Entfernt die angegebene Tür.
        /// </summary>
        /// <param name="door">Die Tür die entfernt werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
        public bool RemoveDoor( DoorData door )
        {
            LogManager.WriteInfo( "Lagerhaustuer wird entfernt.", "Warehouse", "RemoveDoor" );
            
            return DestroyDoorObject( door );
        }
        
        /// <summary>
        /// SUcht ein Tür Objekt anhand der gegebenen ID.
        /// </summary>
        /// <param name="id">Die ID des Tür Objekts.</param>
        /// <returns>Gibt das <see cref="DoorData"/> Objekt das die Tür repräsentiert zurück oder null.</returns>
        public DoorData GetDoor( long id )
        {
            for ( int i = 0; i < Doors.Count; i++ )
            {
                if ( Doors[ i ].GetID( ) == id )
                {
                    return Doors[ i ];
                }
            }

            return null;
        }
        
        /// <summary>
        /// SUcht ein Tür Objekt anhand des GameObjects.
        /// </summary>
        /// <param name="obj">Das GameObject des Tür Objekts.</param>
        /// <returns>Gibt das <see cref="DoorData"/> Objekt das die Tür repräsentiert zurück oder null.</returns>
        public DoorData GetDoor( GameObject obj )
        {
            for ( int i = 0; i < Doors.Count; i++ )
            {
                if ( Doors[ i ].Object == obj )
                {
                    return Doors[ i ];
                }
            }

            return null;
        }

        // --- StorageRack ---

        /// <summary>
        /// Erstellt ein neues Regal am default Spawnpunkt.
        /// </summary>
        /// <returns>Das <see cref="StorageData"/> Objekt das das Regal repräsentiert.</returns>
        public StorageData CreateStorageRack( )
        {
            LogManager.WriteInfo( "Lagerhausregal wird erstellt.", "Warehouse", "CreateStorageRack" );

            StorageData storage = new StorageData( GetUniqueID( StorageRacks.ToArray( ) ), ObjectSpawn.transform.position, ObjectSpawn.transform.rotation, ObjectSpawn.transform.localScale )
            {                
                IsContainer = false
            };

            CreateStorageRackObject( storage );

            return storage;
        }

        /// <summary>
        /// Erstellt ein neues Regal mit den angegebenen Eigenschaften.
        /// </summary>
        /// <param name="position">Die Position des Regals.</param>
        /// <param name="rotation">Die Rotation des Regals</param>
        /// <param name="scale">Die Skalierung desRegals.</param>
        /// <returns>Das <see cref="StorageData"/> Objekt das das Regal repräsentiert.</returns>
        public StorageData CreateStorageRack( Vector3 position, Quaternion rotation, Vector3 scale )
        {
            LogManager.WriteInfo( "Lagerhausregal wird erstellt.", "Warehouse", "CreateStorageRack" );
                        
            StorageData storage = new StorageData( GetUniqueID( StorageRacks.ToArray( ) ), position, rotation, scale )
            {
                IsContainer = false
            };

            CreateStorageRackObject( storage );

            return storage;
        }

        /// <summary>
        /// Fügt ein Regal Objekt zur Lagerhalle hinzu.
        /// </summary>
        /// <param name="storage">Das Regal das hinzugefügt werden soll.</param>
        internal void AddStorageRack( StorageData storage )
        {
            LogManager.WriteInfo( "Lagerhausregal wird hinzugefuegt.", "Warehouse", "AddStorageRack" );

            CreateStorageRackObject( storage );
        }

        /// <summary>
        /// Entfernt das angegebene Regal.
        /// </summary>
        /// <param name="storage">Das Regal das entfernt werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
        public bool RemoveStorageRack( StorageData storage )
        {
            LogManager.WriteInfo( "Lagerhausregal wird entfernt.", "Warehouse", "RemoveStorageRack" );

            DestroyStorageRackObject( storage );

            return false;
        }

        /// <summary>
        /// SUcht ein Regal Objekt anhand der gegebenen ID.
        /// </summary>
        /// <param name="id">Die ID des Regal Objekts.</param>
        /// <returns>Gibt das <see cref="StorageData"/> Objekt das das Regal repräsentiert zurück oder null.</returns>
        public StorageData GetStorageRack( long id )
        {
            for ( int i = 0; i < StorageRackList.Count; i++ )
            {
                if ( StorageRacks[ i ].GetID( ) == id )
                {
                    return StorageRacks[ i ];
                }
            }

            return null;
        }

        /// <summary>
        /// SUcht ein Regal Objekt anhand des GameObjects.
        /// </summary>
        /// <param name="obj">Das GameObject des Regal Objekts.</param>
        /// <returns>Gibt das <see cref="StorageData"/> Objekt das das Regal repräsentiert zurück oder null.</returns>
        public StorageData GetStorageRack( GameObject obj )
        {
            for ( int i = 0; i < StorageRackList.Count; i++ )
            {
                if ( StorageRacks[ i ].Object == obj )
                {
                    return StorageRacks[ i ];
                }
            }

            return null;
        }
        
        // --- StorageRackItem ---

        /// <summary>
        /// Sucht anhand des <see cref="GameObject"/> ein Regalitem.
        /// </summary>
        /// <param name="obj">Das <see cref="GameObject"/> mit dem gesucht wird.</param>
        /// <param name="storage">Das Regal in dem das Item gesucht werden soll.</param>
        /// <returns>Gibt das passende Item oder null zurück.</returns>
        public ItemData GetStorageRackItem( StorageData storage, GameObject obj )
        {
            ItemData data = storage.GetItem( obj );

            return null;
        }

        /// <summary>
        /// Sucht anhand des <see cref="GameObject"/> ein Regalitem.
        /// </summary>
        /// <param name="obj">Das <see cref="GameObject"/> mit dem gesucht wird.</param>
        /// <returns>Gibt das passende Item oder null zurück.</returns>
        public ItemData GetStorageRackItem( GameObject obj )
        {
            for ( int i = 0; i < StorageRackList.Count; i++ )
            {
                ItemData data = StorageRackList[i].GetItem( obj );

                if ( data != null )
                {
                    return data;
                }
            }

            return null;
        }

        /// <summary>
        /// Zerstört alle Lagerhaus Objekte in der Umgebung.
        /// </summary>
        internal void DestroyWarehouse()
        {
            if ( !Destroyed )
            {
                foreach ( var data in Floor )
                {
                    data.Destroy( );
                }

                foreach ( var data in Walls )
                {
                    data.Destroy( );
                }

                foreach ( var data in Windows )
                {
                    data.Destroy( );
                }

                foreach ( var data in Doors )
                {
                    data.Destroy( );
                }

                foreach ( var data in StorageRacks )
                {
                    OnStorageRackModified( 1, data );
                    data.Destroy( );
                }

                Destroyed = true;
            }
        }

        // --- Implementierungen ---

        /// <summary>
        /// Erzeugt eine für die Objektgruppe eindeutige ID.
        /// </summary>
        /// <param name="idUsed">Enthält alle IDs die bereits in der Objektgruppe vorhanden sind.</param>
        /// <returns>Eine für die Objektgruppe eindeutige ID.</returns>
        internal static long GetUniqueID( IDataIdentifier[] idUsed )
        {
            bool used;

            for ( int i = 0; ; i++ )
            {
                used = false;

                for ( int j = 0; j < idUsed.Length; j++ )
                {
                    if ( idUsed[j] != null && i + 1 == idUsed[ j ].GetID() )
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

        /// <summary>
        /// Erstellt ein Tür Objekt und lädt es in die Umgebung.
        /// </summary>
        /// <param name="data">Das Objekt das in die Umgebung geladen werden soll.</param>
        private void CreateFloorObject( FloorData data )
        {
            data.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableFloor" ), data.Position, data.Rotation, GameObject.FindGameObjectWithTag( "FloorDefinition" ).transform ) );

            data.Object.name = "Floor" + data.GetID( );

            Floor.Add( data );

            Data.Floor.Add( new ProjectFloorData( data.GetID( ), new ProjectTransformationData( data.Position, data.Rotation, data.Scale ) ) );
        }

        /// <summary>
        /// Zerstört ein Boden Objekt und entfernt es aus der Umgebung.
        /// </summary>
        /// <param name="data">Das Objekt das Zerstört werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
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

                    Object.Destroy( data.Object );
                    data.Destroy( );

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Erstellt ein Wand Objekt und lädt es in die Umgebung.
        /// </summary>
        /// <param name="data">Das Objekt das in die Umgebung geladen werden soll.</param>
        private void CreateWallObject( WallData data, string tag = "SelectableWall" )
        {
            if ( data.Class == WallClass.Outer )
            {
                data.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectsWithTag( "SelectableWall" )[0], data.Position, data.Rotation, GameObject.FindGameObjectWithTag( data.Face.ToString( ) + "Wall" ).transform ) );

                switch ( data.Face )
                {
                    case WallFace.North:

                        NorthWall.Add( data );
                        break;

                    case WallFace.East:

                        EastWall.Add( data );
                        break;

                    case WallFace.South:

                        SouthWall.Add( data );
                        break;

                    case WallFace.West:

                        WestWall.Add( data );
                        break;

                    case WallFace.Undefined:

                        LogManager.WriteWarning( "Es wird eine undefinierte Wandseite verwendet!", "Warehouse", "CreateWallObject" );
                        break;
                }
            }

            else
            {
                data.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableWall" ), data.Position, data.Rotation, GameObject.FindGameObjectWithTag( "InnerWall" ).transform ) );
            }

            data.Object.name = "Wall" + data.GetID( );
            data.Object.tag = tag;

            Walls.Add( data );

            Data.Walls.Add( new ProjectWallData( data.GetID( ), data.Object.tag, data.Face.ToString( ), data.Class.ToString( ), new ProjectTransformationData( data.Position, data.Rotation, data.Scale ) ) );
        }

        /// <summary>
        /// Zerstört ein Wand Objekt und entfernt es aus der Umgebung.
        /// </summary>
        /// <param name="data">Das Objekt das Zerstört werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
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

                    if ( data.Class == WallClass.Outer )
                    {
                        switch ( data.Face )
                        {
                            case WallFace.North:

                                NorthWall.Remove( data );
                                break;

                            case WallFace.East:

                                EastWall.Remove( data );
                                break;

                            case WallFace.South:

                                SouthWall.Remove( data );
                                break;

                            case WallFace.West:

                                WestWall.Remove( data );
                                break;

                            case WallFace.Undefined:

                                LogManager.WriteWarning( "Es wird eine undefinierte Wandseite verwendet!", "Warehouse", "CreateWallObject" );
                                break;
                        }
                    }

                    data.Destroy( );

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Erstellt ein Fenster Objekt und lädt es in die Umgebung.
        /// </summary>
        /// <param name="data">Das Objekt das in die Umgebung geladen werden soll.</param>
        private void CreateWindowObject( WindowData data )
        {
            data.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableWindow" ), data.Position, data.Rotation, GameObject.FindGameObjectWithTag( "WindowDefinition" ).transform ) );

            data.Object.name = "Window" + data.GetID( );

            Windows.Add( data );

            if ( data.Class == WallClass.Outer )
            {
                switch ( data.Face )
                {
                    case WallFace.North:

                        NorthWall.Add( data );
                        break;

                    case WallFace.East:

                        EastWall.Add( data );
                        break;

                    case WallFace.South:

                        SouthWall.Add( data );
                        break;

                    case WallFace.West:

                        WestWall.Add( data );
                        break;

                    case WallFace.Undefined:

                        LogManager.WriteWarning( "Es wird eine undefinierte Wandseite verwendet!", "Warehouse", "CreateWallObject" );
                        break;
                }
            }

            Data.Windows.Add( new ProjectWindowData( data.GetID( ), data.Face.ToString(), data.Class.ToString(), new ProjectTransformationData( data.Position, data.Rotation, data.Scale ) ) );
        }

        /// <summary>
        /// Zerstört ein Fenster Objekt und entfernt es aus der Umgebung.
        /// </summary>
        /// <param name="data">Das Objekt das Zerstört werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
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

                    if ( data.Class == WallClass.Outer )
                    {
                        switch ( data.Face )
                        {
                            case WallFace.North:

                                NorthWall.Remove( data );
                                break;

                            case WallFace.East:

                                EastWall.Remove( data );
                                break;

                            case WallFace.South:

                                SouthWall.Remove( data );
                                break;

                            case WallFace.West:

                                WestWall.Remove( data );
                                break;

                            case WallFace.Undefined:

                                LogManager.WriteWarning( "Es wird eine undefinierte Wandseite verwendet!", "Warehouse", "CreateWallObject" );
                                break;
                        }
                    }

                    data.Destroy( );

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Erstellt ein Tür Objekt und lädt es in die Umgebung.
        /// </summary>
        /// <param name="data">Das Objekt das in die Umgebung geladen werden soll.</param>
        private void CreateDoorObject( DoorData data )
        {
            data.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableDoor" ), data.Position, data.Rotation, GameObject.FindGameObjectWithTag( "DoorDefinition" ).transform ) );

            data.Object.name = "Door" + data.GetID( );

            Doors.Add( data );

            if ( data.Class == WallClass.Outer )
            {
                switch ( data.Face )
                {
                    case WallFace.North:

                        NorthWall.Add( data );
                        break;

                    case WallFace.East:

                        EastWall.Add( data );
                        break;

                    case WallFace.South:

                        SouthWall.Add( data );
                        break;

                    case WallFace.West:

                        WestWall.Add( data );
                        break;

                    case WallFace.Undefined:

                        LogManager.WriteWarning( "Es wird eine undefinierte Wandseite verwendet!", "Warehouse", "CreateWallObject" );
                        break;
                }
            }

            Data.Doors.Add( new ProjectDoorData( data.GetID( ), data.Face.ToString( ), data.Class.ToString( ), data.Type.ToString( ), new ProjectTransformationData( data.Position, data.Rotation, data.Scale ) ) );
        }

        /// <summary>
        /// Zerstört ein Tür Objekt und entfernt es aus der Umgebung.
        /// </summary>
        /// <param name="data">Das Objekt das Zerstört werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
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

                    if ( data.Class == WallClass.Outer )
                    {
                        switch ( data.Face )
                        {
                            case WallFace.North:

                                NorthWall.Remove( data );
                                break;

                            case WallFace.East:

                                EastWall.Remove( data );
                                break;

                            case WallFace.South:

                                SouthWall.Remove( data );
                                break;

                            case WallFace.West:

                                WestWall.Remove( data );
                                break;

                            case WallFace.Undefined:

                                LogManager.WriteWarning( "Es wird eine undefinierte Wandseite verwendet!", "Warehouse", "CreateWallObject" );
                                break;
                        }
                    }

                    data.Destroy( );

                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Erstellt ein Regal Objekt und lädt es in die Umgebung.
        /// </summary>
        /// <param name="data">Das Objekt das in die Umgebung geladen werden soll.</param>
        private void CreateStorageRackObject( StorageData data )
        {
            data.ChangeGameObject( GameObject.Instantiate( GameObject.FindGameObjectWithTag( "SelectableStorage" ), data.Position, data.Rotation, GameObject.FindGameObjectWithTag( "StorageRackDefinition" ).transform ) );

            data.Object.name = "Storage" + data.GetID( );

            StorageRackList.Add( data );

            data.ChangeSlotCount( new StorageSlotCalculation( ) );

            Data.StorageRacks.Add( new ProjectStorageData( data.GetID( ), data.SlotCount, new ProjectTransformationData( data.Position, data.Rotation, data.Scale ) ) );

            OnStorageRackModified( 0, data );
        }

        /// <summary>
        /// Zerstört ein Regal Objekt und entfernt es aus der Umgebung.
        /// </summary>
        /// <param name="data">Das Objekt das Zerstört werden soll.</param>
        /// <returns>Gibt true zurück wenn Erfolgreich.</returns>
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

                    OnStorageRackModified( 1, data );

                    data.Destroy( );

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Wird aufgerufen wenn ein Regal hinzugefügt oder entfernt wurde und löst das passende Event aus.
        /// </summary>
        /// <param name="mode">Gibt an ob ein Regal hinzugefügt oder entfernt wurde. 0 = Hinzugefügt, !0 = Entfernt</param>
        /// <param name="data">Das betroffene Regal.</param>
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
