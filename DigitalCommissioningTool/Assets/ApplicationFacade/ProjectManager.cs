using System.Collections;
using System.Collections.Generic;
using ProjectComponents.FileIntegration;
using ProjectComponents.Abstraction;
using SystemFacade;
using System;
using UnityEngine;
using System.IO;
using AppData.Warehouse;

namespace ApplicationFacade
{
    public class ProjectManager
    {
        public string ProjectName
        {
            get
            {
                if ( Data == null )
                {
                    return null;
                }

                return Data.ProjectName;
            }

            private set
            {
                Data.ProjectName = value;
            }
        }

        public ProjectData Data { get; private set; }
        public ProjectSettings Settings { get; private set; }

        internal DataHandler      DHandler { get; set; }
        internal SettingsHandler  SHandler { get; set; }
        internal WarehouseHandler WHandler { get; set; }
        internal ContainerHandler CHandler { get; set; }
        
        public ProjectManager()
        {
        }

        ~ProjectManager()
        {
            if ( !ProjectName.Equals( string.Empty ) )
            {
                CloseProject( );
            }
        }

        public void LoadProject( string name, ref Warehouse warehouse, ref Container container )
        {
            if ( !File.Exists( Paths.ProjectsPath + name + ".prj" ) )
            {
                warehouse = null;
                container = null;

                return;
            }

            ArchiveManager.ExtractArchive( Paths.ProjectsPath + name + ".prj", Paths.TempPath );
            
            DHandler = new DataHandler( );
            SHandler = new SettingsHandler( );
            WHandler = new WarehouseHandler( );
            CHandler = new ContainerHandler( );

            InternalProjectData idata = DHandler.LoadFile( );
            Data = new ProjectData( );
            ReadProjectData( idata );

            InternalProjectSettings isettings = SHandler.LoadFile( );
            Settings = new ProjectSettings( );
            ReadProjectSettings( isettings );

            InternalProjectWarehouse iwarehouse = WHandler.LoadFile( );
            ReadProjectWarehouse( iwarehouse, warehouse );
            
            InternalProjectContainer icontainer = CHandler.LoadFile( );
            ReadProjectContainer( icontainer, container );
        }

        public void SaveProject( string name, Warehouse warehouse, Container container )
        {
            if ( name != null )
            {
                if ( !name.Equals( string.Empty ) )
                {
                    ProjectName = name;
                }
            }

            WriteProjectData( );
            WriteProjectSettings( );
            WriteProjectWarehouse( warehouse );
            WriteProjectContainer( container );

            if ( File.Exists( Data.ProjectPath + Data.ProjectName + InternalProjectData.Extension ) )
            {
                File.Delete( Data.ProjectPath + Data.ProjectName + InternalProjectData.Extension );
            }

            ArchiveManager.ArchiveDirectory( Paths.TempPath, Data.ProjectPath + ProjectName + InternalProjectData.Extension );
        }

        public void CloseProject()
        {
            Data = null;
            Settings = null;

            DHandler = new DataHandler( );
            WHandler = new WarehouseHandler( );
            SHandler = new SettingsHandler( );
            CHandler = new ContainerHandler( );

            Paths.ClearTempPath( );
        }

        public void CreateProject( string name, WarehouseSize size, ref Warehouse warehouse, ref Container container )
        {
            if ( ProjectName != null )
            {
                CloseProject( );
            }

            if ( File.Exists( Paths.ProjectsPath + name + InternalProjectData.Extension ) )
            {
                warehouse = null;
                container = null;

                return;
            }
            
            DHandler = new DataHandler( );
            SHandler = new SettingsHandler( );
            WHandler = new WarehouseHandler( );
            CHandler = new ContainerHandler( );

            Data = new ProjectData( );
            ProjectName = name;
            Data.ProjectPath = Paths.ProjectsPath;

            Settings  = new ProjectSettings( );

            container = GetDefaultContainer( );

            switch ( size )
            {
                case WarehouseSize.Small:
                    
                    warehouse = GetDefaultWarehouse( new SmallWarehouseStrategie() );
                    break;

                case WarehouseSize.Medium:

                    warehouse = GetDefaultWarehouse( new MediumWarehouseStrategie() );
                    break;

                case WarehouseSize.Large:

                    warehouse = GetDefaultWarehouse( new LargeWarehouseStrategie() );
                    break;
            }

            SaveProject( name, warehouse, GetDefaultContainer( ) );
        }

        public void DeleteProject( string name )
        {
            if( ProjectName != null && ProjectName.Equals( name ) )
            {
                CloseProject( );
            }

            if ( File.Exists( Paths.ProjectsPath + name + InternalProjectData.Extension ) )
            {
                File.Delete( Paths.ProjectsPath + name + InternalProjectData.Extension );
            }
        }

        private void WriteProjectData( )
        {
            DHandler.SaveFile( Data.Data );
        }

        private void WriteProjectSettings( )
        {
            SHandler.SaveFile( Settings.Data );
        }

        private void WriteProjectWarehouse( Warehouse warehouse )
        {
            WHandler.SaveFile( warehouse.Data );
        }

        private void WriteProjectContainer( Container container )
        {
            CHandler.SaveFile( container.Data );
        }

        private void ReadProjectData( InternalProjectData data )
        {
            Data.DateCreated  = data.DateCreated;
            Data.DateModified = DateTime.Today;
            Data.ProjectName  = data.Name;
            Data.ProjectPath  = data.FullPath;
        }

        private void ReadProjectSettings( InternalProjectSettings settings )
        {

        }

        private void ReadProjectWarehouse( InternalProjectWarehouse iwarehouse, Warehouse warehouse )
        {
            for( int i = 0; i < iwarehouse.Floor.Count; i++ )
            {
                FloorData floor = new FloorData( iwarehouse.Floor[ i ].ID );

                floor.SetPosition( iwarehouse.Floor[ i ].Transformation.Position );
                floor.SetRotation( iwarehouse.Floor[ i ].Transformation.Rotation );
                floor.SetScale( iwarehouse.Floor[ i ].Transformation.Scale );

                warehouse.CreateFloor( floor.Position, floor.Rotation, floor.Scale );
            }

            for( int i = 0; i < iwarehouse.Walls.Count; i++ )
            {
                WallData wall = new WallData( iwarehouse.Walls[i].ID );

                wall.SetPosition( iwarehouse.Walls[ i ].Transformation.Position );
                wall.SetRotation( iwarehouse.Walls[ i ].Transformation.Rotation );
                wall.SetScale( iwarehouse.Walls[ i ].Transformation.Scale );

                warehouse.CreateWall( wall.Position, wall.Rotation, wall.Scale );
            }

            for ( int i = 0; i < iwarehouse.Windows.Count; i++ )
            {
                WindowData window = new WindowData( iwarehouse.Windows[ i ].ID );

                window.SetPosition( iwarehouse.Windows[ i ].Transformation.Position );
                window.SetRotation( iwarehouse.Windows[ i ].Transformation.Rotation );
                window.SetScale( iwarehouse.Windows[ i ].Transformation.Scale );

                warehouse.AddWindow( window );
            }

            for ( int i = 0; i < iwarehouse.Doors.Count; i++ )
            {
                DoorData door;

                if ( iwarehouse.Doors[i].Type.ToLower().Equals( "door" ) )
                {
                    door = new DoorData( iwarehouse.Doors[ i ].ID, DoorType.Door );
                }

                else
                {
                    door = new DoorData( iwarehouse.Doors[ i ].ID, DoorType.Gate );
                }

                door.SetPosition( iwarehouse.Doors[ i ].Transformation.Position );
                door.SetRotation( iwarehouse.Doors[ i ].Transformation.Rotation );
                door.SetScale( iwarehouse.Doors[ i ].Transformation.Scale );

                warehouse.AddDoor( door );
            }

            for ( int i = 0; i < iwarehouse.StorageRacks.Count; i++ )
            {
                StorageData data = new StorageData( iwarehouse.StorageRacks[ i ].ID );

                data.SetPosition( iwarehouse.StorageRacks[ i ].Transformation.Position );
                data.SetRotation( iwarehouse.StorageRacks[ i ].Transformation.Rotation );
                data.SetScale( iwarehouse.StorageRacks[ i ].Transformation.Scale );

                warehouse.AddStorageRack( data );

                for ( int j = 0; j < iwarehouse.StorageRacks[i].GetItems.Length; j++ )
                {
                    ItemData item = new ItemData( iwarehouse.StorageRacks[ i ].GetItems[ j ].IDRef, 0 );
                    
                    item.SetPosition( iwarehouse.StorageRacks[ i ].GetItems[j].Transformation.Position );
                    item.SetRotation( iwarehouse.StorageRacks[ i ].GetItems[ j ].Transformation.Rotation );
                    item.SetScale( iwarehouse.StorageRacks[ i ].GetItems[ j ].Transformation.Scale );

                    warehouse.AddItemToStorageRack( data, item );
                }
            }
        }

        private void ReadProjectContainer( InternalProjectContainer icontainer, Container container )
        {
            for( int i = 0; i < icontainer.Container.Count; i++ )
            {
                StorageData data = new StorageData( icontainer.Container[i].ID );

                data.SetPosition( icontainer.Container[ i ].Transformation.Position );
                data.SetRotation( icontainer.Container[ i ].Transformation.Rotation );
                data.SetScale( icontainer.Container[ i ].Transformation.Scale );

                container.AddContainer( data );

                for ( int j = 0; j < icontainer.Container[ i ].GetItems.Length; j++ )
                {
                    ItemData item = new ItemData( icontainer.Container[ i ].GetItems[ j ].IDRef, 0 );

                    item.SetPosition( icontainer.Container[ i ].GetItems[ j ].Transformation.Position );
                    item.SetRotation( icontainer.Container[ i ].GetItems[ j ].Transformation.Rotation );
                    item.SetScale( icontainer.Container[ i ].GetItems[ j ].Transformation.Scale );

                    container.AddItemToContainer( data, item );
                }
            }
        }

        private Warehouse GetDefaultWarehouse( IWarehouseStrategie size )
        {
            Warehouse warehouse = new Warehouse( );

            size.StartGeneration( );

            foreach( ObjectTransformation data in size.GetFloor() )
            {
                warehouse.CreateFloor( data.Position, data.Rotation, data.Scale );
            }

            foreach ( ObjectTransformation data in size.GetWalls( ) )
            {
                warehouse.CreateWall( data.Position, data.Rotation, data.Scale );
            }

            foreach ( ObjectTransformation data in size.GetWindows( ) )
            {
                warehouse.CreateWindow( data.Position, data.Rotation, data.Scale );
            }

            foreach ( ObjectTransformation data in size.GetDoors( ) )
            {
                warehouse.CreateDoor( data.Position, data.Rotation, data.Scale, DoorType.Door );
            }

            foreach ( ObjectTransformation data in size.GetStorageRacks( ) )
            {
                //warehouse.CreateStorageRack( data.Position, data.Rotation, data.Scale );
            }

            return warehouse;
        }

        private Container GetDefaultContainer()
        {
            Container container = new Container( );

            return container;
        }
    }
}