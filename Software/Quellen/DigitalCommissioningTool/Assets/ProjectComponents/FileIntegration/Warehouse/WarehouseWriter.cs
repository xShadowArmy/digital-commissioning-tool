﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ProjectComponents.FileIntegration
{
    /// <summary>
    /// Schreibt die Daten des Projekt Lagerhauses in die Datei.
    /// </summary>
    internal class WarehouseWriter
    {
        /// <summary>
        /// Die Datei in die geschrieben werden soll.
        /// </summary>
        private XmlDocument Doc { get; set; }

        /// <summary>
        /// Initialisiert das Objekt.
        /// </summary>
        /// <param name="doc">Die Datei die verwendet werden soll.</param>
        internal WarehouseWriter( XmlDocument doc )
        {
            Doc = doc;
        }
        /// <summary>
        /// Schreibt die Datei.
        /// </summary>
        /// <param name="data">Enthält alle Daten die in die Datei geschrieben werden sollen.</param>
        internal void WriteFile( InternalProjectWarehouse data )
        {
            LogManager.WriteInfo( "Datei \"Warehouse.xml\" wird erstellt.", "WarehouseWriter", "WriteFile" );

            ReCreateFile( );

            string xmlns = "https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/";

            try
            {
                Doc.Load( Paths.TempPath + "Warehouse.xml" );

                XPathNavigator nav = Doc.CreateNavigator( );

                nav.MoveToFirstChild( );

                nav.AppendChildElement( "xs", "Floor", xmlns, "" );
                nav.MoveToChild( "Floor", xmlns );                
                WriteFloor( nav, data, xmlns );
                
                nav.InsertElementAfter( "xs", "Walls", xmlns, "" );
                nav.MoveToNext( );
                WriteWalls( nav, data, xmlns );
                
                nav.InsertElementAfter( "xs", "Windows", xmlns, "" );
                nav.MoveToNext( );
                WriteWindows( nav, data, xmlns );
                
                nav.InsertElementAfter( "xs", "Doors", xmlns, "" );
                nav.MoveToNext( );
                WriteDoors( nav, data, xmlns );

                nav.InsertElementAfter( "xs", "StorageRacks", xmlns, "" );
                nav.MoveToNext( );
                WriteStorageRecks( nav, data, xmlns );

                XmlTextWriter writer = new XmlTextWriter( Paths.TempPath + "Warehouse.xml", System.Text.Encoding.UTF8 )
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4
                };

                Doc.Save( writer );

                writer.Dispose( );
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Warehouse.xml\" konnte nicht erstellt werden! Fehler: " + e.Message, LogLevel.Error, true, "WarehouseWriter", "WriteFile" );
            }
        }

        /// <summary>
        /// Schreibt die Böden in die Datei.
        /// </summary>
        /// <param name="nav">Navigator mit Passender Position.</param>
        /// <param name="data">Die Daten die Geschrieben werden sollen.</param>
        /// <param name="xmlns">Der zu verwendende Xml Namespace.</param>
        private void WriteFloor( XPathNavigator nav, InternalProjectWarehouse data, string xmlns )
        {
            if ( nav.LocalName.Equals( "Floor" ) )
            {
                nav.CreateAttribute( "xs", "count", xmlns, data.Floor.Count.ToString( ) );

                if ( data.Floor.Count == 0 )
                {
                    return;
                }

                for ( int i = 0; i < data.Floor.Count; i++ )
                {
                    if ( i == 0 )
                    {
                        nav.AppendChildElement( "xs", "Floor", xmlns, "" );
                        nav.MoveToChild( "Floor", xmlns );
                        nav.CreateAttribute( "xs", "id", xmlns, data.Floor[ i ].ID.ToString( ) );
                    }

                    else
                    {
                        nav.InsertElementAfter( "xs", "Floor", xmlns, "" );
                        nav.MoveToNext( );
                        nav.CreateAttribute( "xs", "id", xmlns, data.Floor[ i ].ID.ToString( ) );
                    }

                    WriteTransformationData( nav, data.Floor[ i ].Transformation, xmlns );
                }

                nav.MoveToParent( );
            }

            else
            {
                LogManager.WriteLog( "Datei \"Warehouse.xml\" konnte nicht erstellt werden! \"<Floor>\" Element wurde nicht gefunden!", LogLevel.Error, true, "WarehouseWriter", "WriteFloor" );
            }
        }

        /// <summary>
        /// Schreibt die Wände in die Datei.
        /// </summary>
        /// <param name="nav">Navigator mit Passender Position.</param>
        /// <param name="data">Die Daten die Geschrieben werden sollen.</param>
        /// <param name="xmlns">Der zu verwendende Xml Namespace.</param>
        private void WriteWalls( XPathNavigator nav, InternalProjectWarehouse data, string xmlns )
        {
            if ( nav.LocalName.Equals( "Walls" ) )
            {
                nav.CreateAttribute( "xs", "count", xmlns, data.Walls.Count.ToString( ) );

                if ( data.Walls.Count == 0 )
                {
                    return;
                }

                for ( int i = 0; i < data.Walls.Count; i++ )
                {
                    if ( i == 0 )
                    {
                        nav.AppendChildElement( "xs", "Wall", xmlns, "" );
                        nav.MoveToChild( "Wall", xmlns );
                    }

                    else
                    {
                        nav.InsertElementAfter( "xs", "Wall", xmlns, "" );
                        nav.MoveToNext( );
                    }

                    nav.CreateAttribute( "xs", "face", xmlns, data.Walls[i].Face );
                    nav.CreateAttribute( "xs", "wallClass", xmlns, data.Walls[i].Class );
                    nav.CreateAttribute( "xs", "tag", xmlns, data.Walls[i].Tag );
                    nav.CreateAttribute( "xs", "id", xmlns, data.Walls[ i ].ID.ToString( ) );

                    WriteTransformationData( nav, data.Walls[i].Transformation, xmlns );
                }

                nav.MoveToParent( );
            }

            else
            {
                LogManager.WriteLog( "Datei \"Warehouse.xml\" konnte nicht erstellt werden! \"<Walls>\" Element wurde nicht gefunden!", LogLevel.Error, true, "WarehouseWriter", "WriteWalls" );
            }
        }

        /// <summary>
        /// Schreibt die Fenster in die Datei.
        /// </summary>
        /// <param name="nav">Navigator mit Passender Position.</param>
        /// <param name="data">Die Daten die Geschrieben werden sollen.</param>
        /// <param name="xmlns">Der zu verwendende Xml Namespace.</param>
        private void WriteWindows( XPathNavigator nav, InternalProjectWarehouse data, string xmlns )
        {
            if ( nav.LocalName.Equals( "Windows" ) )
            {
                nav.CreateAttribute( "xs", "count", xmlns, data.Windows.Count.ToString( ) );

                if ( data.Windows.Count == 0 )
                {
                    return;
                }

                for ( int i = 0; i < data.Windows.Count; i++ )
                {
                    if ( i == 0 )
                    {
                        nav.AppendChildElement( "xs", "Window", xmlns, "" );
                        nav.MoveToChild( "Window", xmlns );
                    }

                    else
                    {
                        nav.InsertElementAfter( "xs", "Window", xmlns, "" );
                        nav.MoveToNext( );
                    }

                    nav.CreateAttribute( "xs", "id", xmlns, data.Windows[ i ].ID.ToString( ) );
                    nav.CreateAttribute( "xs", "face", xmlns, data.Windows[i].Face );
                    nav.CreateAttribute( "xs", "class", xmlns, data.Windows[i].Class );

                    WriteTransformationData( nav, data.Windows[ i ].Transformation, xmlns );
                }

                nav.MoveToParent( );
            }

            else
            {
                LogManager.WriteLog( "Datei \"Warehouse.xml\" konnte nicht erstellt werden! \"<Windows>\" Element wurde nicht gefunden!", LogLevel.Error, true, "WarehouseWriter", "WriteWindows" );
            }
        }
        
        /// <summary>
        /// Schreibt die Türen in die Datei.
        /// </summary>
        /// <param name="nav">Navigator mit Passender Position.</param>
        /// <param name="data">Die Daten die Geschrieben werden sollen.</param>
        /// <param name="xmlns">Der zu verwendende Xml Namespace.</param>
        private void WriteDoors( XPathNavigator nav, InternalProjectWarehouse data, string xmlns )
        {
            if ( nav.LocalName.Equals( "Doors" ) )
            {
                nav.CreateAttribute( "xs", "count", xmlns, data.Doors.Count.ToString( ) );

                if ( data.Doors.Count == 0 )
                {
                    return;
                }

                for ( int i = 0; i < data.Doors.Count; i++ )
                {
                    if ( i == 0 )
                    {
                        nav.AppendChildElement( "xs", "Door", xmlns, "" );
                        nav.MoveToChild( "Door", xmlns );
                    }

                    else
                    {
                        nav.InsertElementAfter( "xs", "Door", xmlns, "" );
                        nav.MoveToNext( );
                    }

                    nav.CreateAttribute( "xs", "type", xmlns, data.Doors[ i ].Type );
                    nav.CreateAttribute( "xs", "id", xmlns, data.Doors[ i ].ID.ToString( ) );
                    nav.CreateAttribute( "xs", "face", xmlns, data.Doors[i].Face );
                    nav.CreateAttribute( "xs", "class", xmlns, data.Doors[i].Class );

                    WriteTransformationData( nav, data.Doors[ i ].Transformation, xmlns );
                }

                nav.MoveToParent( );
            }

            else
            {
                LogManager.WriteLog( "Datei \"Warehouse.xml\" konnte nicht erstellt werden! \"<Doors>\" Element wurde nicht gefunden!", LogLevel.Error, true, "WarehouseWriter", "WriteDoors" );
            }
        }

        /// <summary>
        /// Schreibt die Regale in die Datei.
        /// </summary>
        /// <param name="nav">Navigator mit Passender Position.</param>
        /// <param name="data">Die Daten die Geschrieben werden sollen.</param>
        /// <param name="xmlns">Der zu verwendende Xml Namespace.</param>
        private void WriteStorageRecks( XPathNavigator nav, InternalProjectWarehouse data, string xmlns )
        {
            if ( nav.LocalName.Equals( "StorageRacks" ) )
            {
                nav.CreateAttribute( "xs", "count", xmlns, data.StorageRacks.Count.ToString( ) );

                if ( data.StorageRacks.Count == 0 )
                {
                    return;
                }

                for ( int i = 0; i < data.StorageRacks.Count; i++ )
                {
                    if ( i == 0 )
                    {
                        nav.AppendChildElement( "xs", "Storage", xmlns, "" );
                        nav.MoveToChild( "Storage", xmlns );
                    }

                    else
                    {
                        nav.InsertElementAfter( "xs", "Storage", xmlns, "" );
                        nav.MoveToNext( );
                    }

                    nav.CreateAttribute( "xs", "id", xmlns, data.StorageRacks[ i ].ID.ToString( ) );
                    nav.CreateAttribute( "xs", "slotCount", xmlns, data.StorageRacks[i].SlotCount.ToString( ) );

                    nav.AppendChildElement( "xs", "Transform", xmlns, "" );
                    nav.MoveToChild( "Transform", xmlns );
                    
                    WriteTransformationData( nav, data.StorageRacks[ i ].Transformation, xmlns );

                    nav.InsertElementAfter( "xs", "Items", xmlns, "" );
                    nav.MoveToNext( );
                    nav.CreateAttribute( "xs", "count", xmlns, data.StorageRacks[ i ].Items.Length.ToString( ) );
                    
                    for( int j = 0; j < data.StorageRacks[i].Items.Length; j++ )
                    {
                        if ( j == 0 )
                        {
                            nav.AppendChildElement( "xs", "Item", xmlns, "" );
                            nav.MoveToChild( "Item", xmlns );
                        }

                        else
                        {
                            nav.InsertElementAfter( "xs", "Item", xmlns, "" );
                            nav.MoveToNext( );
                        }

                        if ( data.StorageRacks[i].Items[j] != null )
                        {
                            nav.CreateAttribute( "xs", "itemName", xmlns, data.StorageRacks[i].Items[j].Name );
                            nav.CreateAttribute( "xs", "itemWeight", xmlns, data.StorageRacks[i].Items[j].Weight.ToString( ) );
                            nav.CreateAttribute( "xs", "itemCount", xmlns, data.StorageRacks[i].Items[j].Count.ToString( ) );
                            nav.CreateAttribute( "xs", "slot", xmlns, j.ToString( ) );
                            nav.CreateAttribute( "xs", "id", xmlns, data.StorageRacks[i].Items[j].ID.ToString( ) );
                            nav.CreateAttribute( "xs", "idRef", xmlns, data.StorageRacks[i].Items[j].IDRef.ToString( ) );
                            nav.CreateAttribute( "xs", "inQueue", xmlns, data.StorageRacks[i].Items[j].InQueue.ToString() );
                            nav.CreateAttribute( "xs", "posQueue", xmlns, data.StorageRacks[i].Items[j].QueuePosition.ToString( ) );

                            WriteTransformationData( nav, data.StorageRacks[i].Items[j].Transformation, xmlns );
                        }

                        else
                        {
                            nav.CreateAttribute( "xs", "itemName", xmlns, "null" );
                        }
                    }

                    nav.MoveToParent( );
                    nav.MoveToParent( );
                }

                nav.MoveToParent( );
            }

            else
            {
                LogManager.WriteLog( "Datei \"Warehouse.xml\" konnte nicht erstellt werden! \"<StorageRacks>\" Element wurde nicht gefunden!", LogLevel.Error, true, "WarehouseWriter", "WriteStorageRecks" );
            }
        }

        /// <summary>
        /// Schreibt die Transformationsdaten in die Datei.
        /// </summary>
        /// <param name="nav">Navigator mit Passender Position.</param>
        /// <param name="data">Die Daten die Geschrieben werden sollen.</param>
        /// <param name="xmlns">Der zu verwendende Xml Namespace.</param>
        private void WriteTransformationData( XPathNavigator nav, ProjectTransformationData data, string xmlns )
        {
            nav.AppendChildElement( "xs", "Position", xmlns, "" );
            nav.MoveToChild( "Position", xmlns );
            nav.CreateAttribute( "xs", "x", xmlns, data.Position.x.ToString( ) );
            nav.CreateAttribute( "xs", "y", xmlns, data.Position.y.ToString( ) );
            nav.CreateAttribute( "xs", "z", xmlns, data.Position.z.ToString( ) );

            nav.InsertElementAfter( "xs", "Rotation", xmlns, "" );
            nav.MoveToNext( );
            nav.CreateAttribute( "xs", "x", xmlns, data.Rotation.eulerAngles.x.ToString( ) );
            nav.CreateAttribute( "xs", "y", xmlns, data.Rotation.eulerAngles.y.ToString( ) );
            nav.CreateAttribute( "xs", "z", xmlns, data.Rotation.eulerAngles.z.ToString( ) );

            nav.InsertElementAfter( "xs", "Scale", xmlns, "" );
            nav.MoveToNext( );
            nav.CreateAttribute( "xs", "x", xmlns, data.Scale.x.ToString( ) );
            nav.CreateAttribute( "xs", "y", xmlns, data.Scale.y.ToString( ) );
            nav.CreateAttribute( "xs", "z", xmlns, data.Scale.z.ToString( ) );

            nav.MoveToParent( );
        }

        /// <summary>
        /// Erstellt die Datei neu.
        /// </summary>
        private void ReCreateFile()
        {
            try
            {
                if ( File.Exists( Paths.TempPath + "Warehouse.xml" ) )
                {
                    File.Delete( Paths.TempPath + "Warehouse.xml" );
                }

                using ( StreamWriter writer = new StreamWriter( File.Create( Paths.TempPath + "Warehouse.xml" ) ) )
                {
                    writer.WriteLine( "<?xml version=\"1.0\" encoding=\"utf-8\"?>" );
                    writer.WriteLine( "<xs:ProjectWarehouse xmlns:xs=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/ ProjectWarehouseSchema.xsd\">" );
                    writer.WriteLine( "</xs:ProjectWarehouse>" );

                    writer.Flush( );
                }
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Warehouse.xml\" konnte nicht erstellt werden! Fehler: " + e.Message, LogLevel.Error, true, "WarehouseReader", "ReCreateFile" );
            }
        }
    }
}