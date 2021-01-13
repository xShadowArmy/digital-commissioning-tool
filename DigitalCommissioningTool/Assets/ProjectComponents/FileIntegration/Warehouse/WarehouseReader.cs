using System;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ProjectComponents.FileIntegration
{
    /// <summary>
    /// Ließt Projekt Lagerhaus Daten aus einer Xml Datei.
    /// </summary>
    internal class WarehouseReader
    {
        /// <summary>
        /// Datei aus der gelesen wird.
        /// </summary>
        private XmlDocument Doc { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="doc">Die Datei aus der die Daten gelesen werden.</param>
        internal WarehouseReader( XmlDocument doc )
        {
            Doc = doc;
        }
        
        /// <summary>
        /// Ließt die Daten aus der Datei.
        /// </summary>
        /// <returns>Objekt das die gelesenen Daten enthält.</returns>
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

        /// <summary>
        /// Ließt die Daten über die Böden aus der Datei.
        /// </summary>
        /// <param name="nav">Der Navigator mit passender Position.</param>
        /// <param name="warehouse">Objekt in das die gelesenen Daten gespeichert werden.</param>
        /// <param name="xmlns">Der zu verwendende Xml Namespace.</param>
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

                nav.MoveToChild( "Floor", xmlns );

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
                LogManager.WriteLog( "Der Boden konnte nicht gelesen werden! " + e.Message, LogLevel.Error, true, "WarehouseReader", "ReadFloor" );
            }
        }

        /// <summary>
        /// Ließt die Daten über die Wände aus der Datei.
        /// </summary>
        /// <param name="nav">Der Navigator mit passender Position.</param>
        /// <param name="warehouse">Objekt in das die gelesenen Daten gespeichert werden.</param>
        /// <param name="xmlns">Der zu verwendende Xml Namespace.</param>
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
                    data = new ProjectWallData( long.Parse( nav.GetAttribute( "id", xmlns ), NumberStyles.Integer ), nav.GetAttribute( "tag", xmlns ), nav.GetAttribute( "face", xmlns ), nav.GetAttribute( "wallClass", xmlns ), ReadTransformation( nav, xmlns ) );
                    
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

        /// <summary>
        /// Ließt die Daten über die Fenster aus der Datei.
        /// </summary>
        /// <param name="nav">Der Navigator mit passender Position.</param>
        /// <param name="warehouse">Objekt in das die gelesenen Daten gespeichert werden.</param>
        /// <param name="xmlns">Der zu verwendende Xml Namespace.</param>
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
                    data = new ProjectWindowData( long.Parse( nav.GetAttribute( "id", xmlns ), NumberStyles.Integer ), nav.GetAttribute( "face", xmlns ), nav.GetAttribute( "class", xmlns ), ReadTransformation( nav, xmlns ) );

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

        /// <summary>
        /// Ließt die Daten über die Türen aus der Datei.
        /// </summary>
        /// <param name="nav">Der Navigator mit passender Position.</param>
        /// <param name="warehouse">Objekt in das die gelesenen Daten gespeichert werden.</param>
        /// <param name="xmlns">Der zu verwendende Xml Namespace.</param>
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
                    data = new ProjectDoorData( long.Parse( nav.GetAttribute( "id", xmlns ), NumberStyles.Integer ), nav.GetAttribute( "face", xmlns ), nav.GetAttribute( "class", xmlns ), nav.GetAttribute( "type", xmlns ), ReadTransformation( nav, xmlns ) );
                    
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

        /// <summary>
        /// Ließt die Daten über die Regale aus der Datei.
        /// </summary>
        /// <param name="nav">Der Navigator mit passender Position.</param>
        /// <param name="warehouse">Objekt in das die gelesenen Daten gespeichert werden.</param>
        /// <param name="xmlns">Der zu verwendende Xml Namespace.</param>
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
                        string name = nav.GetAttribute( "itemName", xmlns );

                        if ( !name.Equals( "null" ) )
                        {
                            long idRef  = long.Parse( nav.GetAttribute( "idRef", xmlns ), NumberStyles.Integer );
                            long iid  = long.Parse( nav.GetAttribute( "id", xmlns ), NumberStyles.Integer );
                            double weight = double.Parse( nav.GetAttribute( "itemWeight", xmlns ), NumberStyles.Number );
                            int count = int.Parse( nav.GetAttribute( "itemCount", xmlns ), NumberStyles.Integer );
                            bool inQueue = bool.Parse( nav.GetAttribute( "inQueue", xmlns ) );
                            int posQueue = int.Parse( nav.GetAttribute( "posQueue", xmlns ), NumberStyles.Integer );
                            
                            item = new ProjectItemData( idRef, iid, count, weight, name, inQueue, posQueue, ReadTransformation( nav, xmlns ) );

                            data.Items[int.Parse( nav.GetAttribute( "slot", xmlns ), NumberStyles.Integer )] = item;
                        }

                        nav.MoveToNext( );
                    }

                    nav.MoveToParent( );
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

        /// <summary>
        /// Ließt die Transformationsdaten aus der Datei.
        /// </summary>
        /// <param name="nav">Der Navigator mit passender Position.</param>
        /// <param name="xmlns">Der zu verwendende Xml Namespace.</param>
        private ProjectTransformationData ReadTransformation( XPathNavigator nav, string xmlns )
        {
            ProjectTransformationData data;
            Vector3 position;
            Vector3 rotation;
            Vector3 scale;

            try
            {
                nav.MoveToChild( "Position", xmlns );

                string x = nav.GetAttribute( "x", xmlns );
                string y = nav.GetAttribute( "y", xmlns );
                string z = nav.GetAttribute( "z", xmlns );

                try
                {
                    position = new Vector3( float.Parse( x, NumberStyles.Float, new CultureInfo( "en-US" ) ), float.Parse( y, NumberStyles.Float, new CultureInfo( "en-US" ) ), float.Parse( z, NumberStyles.Float, new CultureInfo( "en-US" ) ) );
                }

                catch( Exception )
                {
                    x = x.Replace( ',', '.' );
                    y = y.Replace( ',', '.' );
                    z = z.Replace( ',', '.' );

                    position = new Vector3( float.Parse( x, NumberStyles.Float, new CultureInfo( "en-US" ) ), float.Parse( y, NumberStyles.Float, new CultureInfo( "en-US" ) ), float.Parse( z, NumberStyles.Float, new CultureInfo( "en-US" ) ) );
                }

                nav.MoveToNext( );

                x = nav.GetAttribute( "x", xmlns );
                y = nav.GetAttribute( "y", xmlns );
                z = nav.GetAttribute( "z", xmlns );

                try
                {
                    rotation = new Vector3( float.Parse( x, NumberStyles.Float, new CultureInfo( "en-US" ) ), float.Parse( y, NumberStyles.Float, new CultureInfo( "en-US" ) ), float.Parse( z, NumberStyles.Float, new CultureInfo( "en-US" ) ) );
                }

                catch ( Exception )
                {
                    x = x.Replace( ',', '.' );
                    y = y.Replace( ',', '.' );
                    z = z.Replace( ',', '.' );

                    rotation = new Vector3( float.Parse( x, NumberStyles.Float, new CultureInfo( "en-US" ) ), float.Parse( y, NumberStyles.Float, new CultureInfo( "en-US" ) ), float.Parse( z, NumberStyles.Float, new CultureInfo( "en-US" ) ) );
                }

                nav.MoveToNext( );

                x = nav.GetAttribute( "x", xmlns );
                y = nav.GetAttribute( "y", xmlns );
                z = nav.GetAttribute( "z", xmlns );

                try
                {
                    scale = new Vector3( float.Parse( x, NumberStyles.Float, new CultureInfo( "en-US" ) ), float.Parse( y, NumberStyles.Float, new CultureInfo( "en-US" ) ), float.Parse( z, NumberStyles.Float, new CultureInfo( "en-US" ) ) );
                }

                catch ( Exception )
                {
                    x = x.Replace( ',', '.' );
                    y = y.Replace( ',', '.' );
                    z = z.Replace( ',', '.' );

                    scale = new Vector3( float.Parse( x, NumberStyles.Float, new CultureInfo( "en-US" ) ), float.Parse( y, NumberStyles.Float, new CultureInfo( "en-US" ) ), float.Parse( z, NumberStyles.Float, new CultureInfo( "en-US" ) ) );
                }

                nav.MoveToParent( );

                data = new ProjectTransformationData( position, Quaternion.Euler( rotation ), scale );

                return data;
            }

            catch( Exception e )
            {
                LogManager.WriteLog( "Datei \"Warehouse.xml\" konnte nicht gelesen werden! Fehlerhafte Transformationsdaten! Fehler: " + e.Message, LogLevel.Error, true, "WarehouseReader", "ReadTransformation" );

                return new ProjectTransformationData( );
            }
        }
    }
}