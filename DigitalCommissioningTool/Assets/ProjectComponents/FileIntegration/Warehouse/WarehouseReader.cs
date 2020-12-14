using System;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ProjectComponents.FileIntegration
{
    internal class WarehouseReader
    {
        private XmlDocument Doc { get; set; }

        internal WarehouseReader( XmlDocument doc )
        {
            Doc = doc;
        }
        
        internal InternalProjectWarehouse ReadFile( )
        {
            LogManager.WriteInfo( "Datei \"Warehouse.xml\" wird gelesen.", "WarehouseReader", "ReadFile" );
            
            string xmlns = "https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/";

            InternalProjectWarehouse warehouse = new InternalProjectWarehouse( );

            try
            {
                Doc.Load( Paths.TempPath + "Warehouse.xml" );

                XPathNavigator nav = Doc.CreateNavigator( );

                nav.MoveToFirstChild( );

                nav.MoveToFirstChild( );
                ReadFloor( nav, warehouse, xmlns );

                nav.MoveToNext( );
                ReadWalls( nav, warehouse, xmlns );

                nav.MoveToNext( );
                ReadWindows( nav, warehouse, xmlns );

                nav.MoveToNext( );
                ReadDoors( nav, warehouse, xmlns );

                nav.MoveToNext( );
                ReadStorageRecks( nav, warehouse, xmlns );
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Warehouse.xml\" konnte nicht gelesen werden! Fehler: " + e.Message, LogLevel.Error, true, "WarehouseReader", "Readfile" );
            }
            
            return warehouse;
        }

        private void ReadFloor( XPathNavigator nav, InternalProjectWarehouse warehouse, string xmlns )
        {
            LogManager.WriteInfo( "Der Boden wird gelesen.", "WarehouseReader", "ReadFloor" );

            try
            {
                long count = long.Parse( nav.GetAttribute( "count", xmlns ), NumberStyles.Integer );

                if ( count <= 0 )
                {
                    return;
                }

                nav.MoveToFirstChild( );

                ProjectFloorData data;

                for ( int i = 0; i < count; i++ )
                {
                    data = new ProjectFloorData( long.Parse( nav.GetAttribute( "id", xmlns ), NumberStyles.Integer ), ReadTransformation( nav, xmlns ) );

                    nav.MoveToNext( );

                    warehouse.Floor.Add( data );
                }

                nav.MoveToParent( );
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( e.Message, LogLevel.Error, true, "WarehouseReader", "ReadFloor" );
            }
        }

        private void ReadWalls( XPathNavigator nav, InternalProjectWarehouse warehouse, string xmlns )
        {
            LogManager.WriteInfo( "Die Waende werden gelesen.", "WarehouseReader", "ReadWalls" );

            try
            {
                long count = long.Parse( nav.GetAttribute( "count", xmlns ), NumberStyles.Integer );

                if ( count <= 0 )
                {
                    return;
                }

                nav.MoveToFirstChild( );

                ProjectWallData data;

                for( int i = 0; i < count; i++ )
                {
                    data = new ProjectWallData( long.Parse( nav.GetAttribute( "id", xmlns ), NumberStyles.Integer ), nav.GetAttribute( "face", xmlns ), nav.GetAttribute( "wallClass", xmlns ), ReadTransformation( nav, xmlns ) );
                    
                    nav.MoveToNext( );

                    warehouse.Walls.Add( data );
                }

                nav.MoveToParent( );
            }

            catch( Exception e )
            {
                LogManager.WriteLog( e.Message, LogLevel.Error, true, "WarehouseReader", "ReadWalls" );
            }
        }

        private void ReadWindows( XPathNavigator nav, InternalProjectWarehouse warehouse, string xmlns )
        {
            LogManager.WriteInfo( "Die Fenster werden gelesen.", "WarehouseReader", "ReadWindows" );

            try
            {
                long count = long.Parse( nav.GetAttribute( "count", xmlns ), NumberStyles.Integer );

                if ( count <= 0 )
                {
                    return;
                }

                nav.MoveToFirstChild( );

                ProjectWindowData data;

                for ( int i = 0; i < count; i++ )
                {
                    data = new ProjectWindowData( long.Parse( nav.GetAttribute( "id", xmlns ), NumberStyles.Integer ), ReadTransformation( nav, xmlns ) );

                    nav.MoveToNext( );

                    warehouse.Windows.Add( data );
                }

                nav.MoveToParent( );
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( e.Message, LogLevel.Error, true, "WarehouseReader", "ReadWindows" );
            }
        }

        private void ReadDoors( XPathNavigator nav, InternalProjectWarehouse warehouse, string xmlns )
        {
            LogManager.WriteInfo( "Die Tueren werden gelesen.", "WarehouseReader", "ReadDoors" );

            try
            {
                long count = long.Parse( nav.GetAttribute( "count", xmlns ), NumberStyles.Integer );

                if ( count <= 0 )
                {
                    return;
                }

                nav.MoveToFirstChild( );

                ProjectDoorData data;

                for ( int i = 0; i < count; i++ )
                {
                    data = new ProjectDoorData( long.Parse( nav.GetAttribute( "id", xmlns ), NumberStyles.Integer ), nav.GetAttribute( "type", xmlns ), ReadTransformation( nav, xmlns ) );
                    
                    nav.MoveToNext( );

                    warehouse.Doors.Add( data );
                }

                nav.MoveToParent( );
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( e.Message, LogLevel.Error, true, "WarehouseReader", "ReadDoors" );
            }
        }

        private void ReadStorageRecks( XPathNavigator nav, InternalProjectWarehouse warehouse, string xmlns )
        {
            LogManager.WriteInfo( "Die Regale werden gelesen.", "WarehouseReader", "ReadStorageRecks" );

            try
            {
                long storageCount = long.Parse( nav.GetAttribute( "count", xmlns ), NumberStyles.Integer );
                long itemCount;

                if ( storageCount <= 0 )
                {
                    return;
                }

                nav.MoveToFirstChild( );

                ProjectStorageData data;

                for ( int i = 0; i < storageCount; i++ )
                {
                    if ( i > 0 )
                    {
                        nav.MoveToNext( );
                    }

                    long id = long.Parse( nav.GetAttribute( "id", xmlns ), NumberStyles.Integer );
                    int slotCount = int.Parse( nav.GetAttribute( "slotCount", xmlns ) );

                    nav.MoveToFirstChild( );

                    data = new ProjectStorageData( id, slotCount, ReadTransformation( nav, xmlns ) );

                    nav.MoveToNext( );

                    itemCount = long.Parse( nav.GetAttribute( "count", xmlns ), NumberStyles.Integer );

                    if ( itemCount <= 0 )
                    {
                        nav.MoveToParent( );
                        
                        warehouse.StorageRacks.Add( data );

                        continue;
                    }

                    nav.MoveToFirstChild( );

                    ProjectItemData item;

                    for ( int j = 0; j < itemCount; j++ )
                    {
                        long idRef  = long.Parse( nav.GetAttribute( "idRef", xmlns ), NumberStyles.Integer );
                        string name = nav.GetAttribute( "itemName", xmlns );
                        double weight = double.Parse( nav.GetAttribute( "itemWeight", xmlns ), NumberStyles.Number );
                        int count = int.Parse( nav.GetAttribute( "itemCount", xmlns ) );

                        item = new ProjectItemData( idRef, count, weight, name, ReadTransformation( nav, xmlns ) );
                        
                        nav.MoveToNext( );

                        data.Items.Add( item );
                    }

                    nav.MoveToParent( );

                    warehouse.StorageRacks.Add( data );
                }

                nav.MoveToParent( );
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( e.Message, LogLevel.Error, true, "WarehouseReader", "ReadWalls" );
            }
        }

        private ProjectTransformationData ReadTransformation( XPathNavigator nav, string xmlns )
        {
            ProjectTransformationData data;

            try
            {
                nav.MoveToChild( "Position", xmlns );

                Vector3 position = new Vector3( float.Parse( nav.GetAttribute( "x", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "y", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "z", xmlns ), NumberStyles.Float ) );

                nav.MoveToNext( );
                Vector3 rotation = new Vector3( float.Parse( nav.GetAttribute( "x", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "y", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "z", xmlns ), NumberStyles.Float ) );

                nav.MoveToNext( );
                Vector3 scale = new Vector3( float.Parse( nav.GetAttribute( "x", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "y", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "z", xmlns ), NumberStyles.Float ) );

                data = new ProjectTransformationData( position, Quaternion.Euler( rotation ), scale );

                nav.MoveToParent( );

                return data;
            }

            catch( Exception e )
            {
                LogManager.WriteLog( "Datei \"Warehouse.xml\" konnte nicht gelesen werden! Fehler: " + e.Message, LogLevel.Error, true, "WarehouseReader", "ReadTransformation" );

                return new ProjectTransformationData( );
            }
        }
    }
}