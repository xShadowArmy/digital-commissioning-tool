using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ProjectComponents.FileIntegration
{
    internal class ContainerReader
    {
        private XmlDocument Doc { get; set; }

        internal ContainerReader( XmlDocument doc )
        {
            Doc = doc;
        }

        internal InternalProjectContainer ReadFile()
        {
            LogManager.WriteInfo( "Datei \"Container.xml\" wird gelesen.", "ContainerReader", "ReadFile" );

            string xmlns = "https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/";

            InternalProjectContainer container = new InternalProjectContainer( );

            try
            {
                Doc.Load( Paths.TempPath + "Container.xml" );

                XPathNavigator nav = Doc.CreateNavigator( );

                nav.MoveToFirstChild( );

                long storageCount = long.Parse( nav.GetAttribute( "count", xmlns ), NumberStyles.Integer );
                long itemCount;

                if ( storageCount <= 0 )
                {
                    return container;
                }

                nav.MoveToFirstChild( );

                ProjectStorageData data;

                for ( int i = 0; i < storageCount; i++ )
                {
                    data = new ProjectStorageData( );

                    data.SetID( long.Parse( nav.GetAttribute( "id", xmlns ), NumberStyles.Integer ) );

                    nav.MoveToFirstChild( );

                    data.SetTransformation( ReadTransformation( nav, xmlns ) );

                    nav.MoveToNext( );

                    itemCount = long.Parse( nav.GetAttribute( "count", xmlns ), NumberStyles.Integer );

                    if ( itemCount <= 0 )
                    {
                        nav.MoveToParent( );
                        container.AddContainer( data );

                        continue;
                    }

                    nav.MoveToFirstChild( );

                    ProjectItemData item;

                    for ( int j = 0; j < itemCount; j++ )
                    {
                        item = new ProjectItemData( );

                        item.SetIDRef( long.Parse( nav.GetAttribute( "idRef", xmlns ), NumberStyles.Integer ) );

                        item.SetTransformation( ReadTransformation( nav, xmlns ) );

                        nav.MoveToNext( );

                        data.AddItem( item );
                    }

                    nav.MoveToParent( );

                    container.AddContainer( data );

                    nav.MoveToParent( );
                } 
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Container.xml\" konnte nicht gelesen werden! Fehler: " + e.Message, LogLevel.Error, true, "ContainerReader", "Readfile" );
            }

            return container;
        }
        
        private ProjectTransformationData ReadTransformation( XPathNavigator nav, string xmlns )
        {
            ProjectTransformationData data = new ProjectTransformationData( );

            try
            {
                nav.MoveToChild( "Position", xmlns );

                data.SetPosition( new Vector3( float.Parse( nav.GetAttribute( "x", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "y", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "z", xmlns ), NumberStyles.Float ) ) );

                nav.MoveToNext( );
                data.SetRotation( new Vector3( float.Parse( nav.GetAttribute( "x", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "y", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "z", xmlns ), NumberStyles.Float ) ) );

                nav.MoveToNext( );
                data.SetScale( new Vector3( float.Parse( nav.GetAttribute( "x", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "y", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "z", xmlns ), NumberStyles.Float ) ) );

                nav.MoveToParent( );
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Warehouse.xml\" konnte nicht gelesen werden! Fehler: " + e.Message, LogLevel.Error, true, "WarehouseReader", "ReadTransformation" );
            }

            return data;
        }
    }
}
