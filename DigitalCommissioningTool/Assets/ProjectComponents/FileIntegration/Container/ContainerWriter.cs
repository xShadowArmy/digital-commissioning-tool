using System;
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
    /// Schreibt Container Daten in eine Xml Datei.
    /// </summary>
    internal class ContainerWriter
    {
        /// <summary>
        /// Die Datei in die geschrieben wird.
        /// </summary>
        private XmlDocument Doc { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="doc">Die Datei in die geschrieben werden soll.</param>
        internal ContainerWriter( XmlDocument doc )
        {
            Doc = doc;
        }
        
        /// <summary>
        /// Schreibt die Daten in die Datei.
        /// </summary>
        /// <param name="data">Die Daten die gespeichert werden sollen.</param>
        internal void WriteFile( InternalProjectContainer data )
        {
            LogManager.WriteInfo( "Datei \"Container.xml\" wird erstellt.", "ContainerWriter", "WriteFile" );

            ReCreateFile( );

            string xmlns = "https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/";

            try
            {
                Doc.Load( Paths.TempPath + "Container.xml" );

                XPathNavigator nav = Doc.CreateNavigator( );

                nav.MoveToFirstChild( );

                nav.MoveToAttribute( "count", xmlns );
                nav.SetValue( data.Container.Count.ToString() );
                nav.MoveToParent( );

                if ( data.Container.Count > 0 )
                {
                    nav.AppendChildElement( "xs", "Container", xmlns, "" );
                    nav.MoveToFirstChild( );

                    for ( int i = 0; i < data.Container.Count; i++ )
                    {
                        if ( i > 0 )
                        {
                            nav.InsertElementAfter( "xs", "Container", xmlns, "" );
                            nav.MoveToNext( );
                        }

                        nav.CreateAttribute( "xs", "slotCount", xmlns, data.Container[i].SlotCount.ToString() );
                        nav.CreateAttribute( "xs", "id", xmlns, data.Container[ i ].ID.ToString() );
                        
                        nav.AppendChildElement( "xs", "Transform", xmlns, "" );
                        nav.MoveToChild( "Transform", xmlns );
                        WriteTransformationData( nav, data.Container[i].Transformation, xmlns );

                        nav.InsertElementAfter( "xs", "Items", xmlns, "" );
                        nav.MoveToNext( );
                        nav.CreateAttribute( "xs", "count", xmlns, data.Container[i].Items.Length.ToString() );

                        if ( data.Container[i].Items.Length > 0 )
                        {
                            for( int j = 0; j < data.Container[i].Items.Length; j++ )
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

                                if ( data.Container[i].Items[j] != null )
                                {
                                    nav.CreateAttribute( "xs", "itemName", xmlns, data.Container[i].Items[j].Name );
                                    nav.CreateAttribute( "xs", "itemWeight", xmlns, data.Container[i].Items[j].Weight.ToString( ) );
                                    nav.CreateAttribute( "xs", "itemCount", xmlns, data.Container[i].Items[j].Count.ToString( ) );
                                    nav.CreateAttribute( "xs", "slot", xmlns, j.ToString( ) );
                                    nav.CreateAttribute( "xs", "id", xmlns, data.Container[i].Items[j].ID.ToString( ) );
                                    nav.CreateAttribute( "xs", "idRef", xmlns, data.Container[i].Items[j].IDRef.ToString( ) );
                                    WriteTransformationData( nav, data.Container[i].Items[j].Transformation, xmlns );
                                }

                                else
                                {
                                    nav.CreateAttribute( "xs", "itemName", xmlns, "null" );
                                }
                            }

                            nav.MoveToParent( );
                        }

                        nav.MoveToParent( );
                    }                    
                }
                
                XmlTextWriter writer = new XmlTextWriter( Paths.TempPath + "Container.xml", System.Text.Encoding.UTF8 )
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4
                };

                Doc.Save( writer );

                writer.Dispose( );
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Container.xml\" konnte nicht erstellt werden! Fehler: " + e.Message, LogLevel.Error, true, "WarehouseWriter", "WriteFile" );
            }
        }
        
        /// <summary>
        /// Schreibt Transformationsdaten in die Daeti.
        /// </summary>
        /// <param name="nav">Navigator mit passender Position.</param>
        /// <param name="data">Daten die gespeichert werden sollen.</param>
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
            nav.CreateAttribute( "xs", "x", xmlns, data.Rotation.x.ToString( ) );
            nav.CreateAttribute( "xs", "y", xmlns, data.Rotation.y.ToString( ) );
            nav.CreateAttribute( "xs", "z", xmlns, data.Rotation.z.ToString( ) );

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
                if ( File.Exists( Paths.TempPath + "Container.xml" ) )
                {
                    File.Delete( Paths.TempPath + "Container.xml" );
                }

                using ( StreamWriter writer = new StreamWriter( File.Create( Paths.TempPath + "Container.xml" ) ) )
                {
                    writer.WriteLine( "<?xml version=\"1.0\" encoding=\"utf-8\"?>" );
                    writer.WriteLine( "<xs:ProjectContainer xs:count=\"0\" xmlns:xs=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/ ProjectContainerSchema.xsd\">" );
                    writer.WriteLine( "</xs:ProjectContainer>" );

                    writer.Flush( );
                }
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Container.xml\" konnte nicht erstellt werden! Fehler: " + e.Message, LogLevel.Error, true, "ContainerWriter", "ReCreateFile" );
            }
        }
    }
}