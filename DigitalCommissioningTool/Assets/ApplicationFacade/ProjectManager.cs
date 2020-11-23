using System.Collections;
using System.Collections.Generic;
using ProjectComponents.FileIntegration;
using ProjectComponents.Abstraction;
using SystemFacade;
using System;
using UnityEngine;
using System.IO;

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

        private DataHandler      DHandler { get; set; }
        private SettingsHandler  SHandler { get; set; }
        private WarehouseHandler WHandler { get; set; }
        private ContainerHandler CHandler { get; set; }
        
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

        public void OpenProject( string name, ref Warehouse warehouse, ref Container container )
        {
            if ( !File.Exists( Paths.ProjectsPath + name + InternalProjectData.Extension ) )
            {
                warehouse = null;
                container = null;

                return;
            }

            ArchiveManager.ExtractArchive( Paths.ProjectsPath + name + InternalProjectData.Extension, Paths.TempPath );
            
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

        public void CreateProject( string name, ref Warehouse warehouse, ref Container container )
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

            Settings = new ProjectSettings( );

            SaveProject( name, GetDefaultWarehouse(), GetDefaultContainer() );

            InternalProjectData idata = DHandler.LoadFile( );
            ReadProjectData( idata );

            InternalProjectSettings isettings = SHandler.LoadFile( );
            ReadProjectSettings( isettings );

            InternalProjectWarehouse iwarehouse = WHandler.LoadFile( );
            ReadProjectWarehouse( iwarehouse, warehouse );

            InternalProjectContainer icontainer = CHandler.LoadFile( );
            ReadProjectContainer( icontainer, container );
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
            warehouse.AdjustFloor( iwarehouse.Floor.Transformation.Position, iwarehouse.Floor.Transformation.Rotation, iwarehouse.Floor.Transformation.Scale );

            for( int i = 0; i < iwarehouse.Walls.Count; i++ )
            {
                WallData wall = new WallData( iwarehouse.Walls[i].ID );

                wall.SetPosition( iwarehouse.Walls[ i ].Transformation.Position );
                wall.SetRotation( iwarehouse.Walls[ i ].Transformation.Rotation );
                wall.SetScale( iwarehouse.Walls[ i ].Transformation.Scale );

                warehouse.AddWall( wall );
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

                //if ( iwarehouse.StorageRacks[i].GetItems() == null )
                //{
                //    continue;
                //}

                for ( int j = 0; j < iwarehouse.StorageRacks[i].GetItems().Length; j++ )
                {
                    ItemData item = new ItemData( iwarehouse.StorageRacks[ i ].GetItems( )[ j ].IDRef, 0 );
                    
                    item.SetPosition( iwarehouse.StorageRacks[ i ].GetItems()[j].Transformation.Position );
                    item.SetRotation( iwarehouse.StorageRacks[ i ].GetItems( )[ j ].Transformation.Rotation );
                    item.SetScale( iwarehouse.StorageRacks[ i ].GetItems( )[ j ].Transformation.Scale );

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

                for ( int j = 0; j < icontainer.Container[ i ].GetItems( ).Length; j++ )
                {
                    ItemData item = new ItemData( icontainer.Container[ i ].GetItems( )[ j ].IDRef, 0 );

                    item.SetPosition( icontainer.Container[ i ].GetItems( )[ j ].Transformation.Position );
                    item.SetRotation( icontainer.Container[ i ].GetItems( )[ j ].Transformation.Rotation );
                    item.SetScale( icontainer.Container[ i ].GetItems( )[ j ].Transformation.Scale );

                    container.AddItemToContainer( data, item );
                }
            }
        }

        private Warehouse GetDefaultWarehouse()
        {
            Warehouse warehouse = new Warehouse( );

            return warehouse;
        }

        private Container GetDefaultContainer()
        {
            Container container = new Container( );

            return container;
        }
    }
}