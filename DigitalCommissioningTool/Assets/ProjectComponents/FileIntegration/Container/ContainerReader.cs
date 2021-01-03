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
                    long id = long.Parse( nav.GetAttribute( "id", xmlns ), NumberStyles.Integer );
                    int slotCount = int.Parse( nav.GetAttribute( "slotCount", xmlns ) );

                    nav.MoveToFirstChild( );

                    data = new ProjectStorageData( id, slotCount, ReadTransformation( nav, xmlns ) );

                    nav.MoveToNext( );

                    itemCount = long.Parse( nav.GetAttribute( "count", xmlns ), NumberStyles.Integer );

                    if ( itemCount <= 0 )
                    {
                        nav.MoveToParent( );
                        container.Container.Add( data );

                        continue;
                    }

                    nav.MoveToFirstChild( );

                    ProjectItemData item;

                    for ( int j = 0; j < itemCount; j++ )
                    {
                        long idRef = long.Parse( nav.GetAttribute( "idRef", xmlns ), NumberStyles.Integer );
                        long iid = long.Parse( nav.GetAttribute( "id", xmlns ), NumberStyles.Integer );
                        string itemName = nav.GetAttribute( "itemName", xmlns );
                        double weight = double.Parse( nav.GetAttribute( "itemWeight", xmlns ), NumberStyles.Number );
                        int count = int.Parse( nav.GetAttribute( "itemCount", xmlns ), NumberStyles.Integer );

                        item = new ProjectItemData( idRef, iid, count, weight, itemName, ReadTransformation( nav, xmlns ) );
                        
                        data.Items[int.Parse( nav.GetAttribute( "slot", xmlns ), NumberStyles.Integer )] = item;

                        nav.MoveToNext( );
                    }

                    nav.MoveToParent( );

                    container.Container.Add( data );

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
            ProjectTransformationData data;

            try
            {
                nav.MoveToChild( "Position", xmlns );

                Vector3 position = new Vector3( float.Parse( nav.GetAttribute( "x", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "y", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "z", xmlns ), NumberStyles.Float ) );

                nav.MoveToNext( );
                Vector3 rotation =  new Vector3( float.Parse( nav.GetAttribute( "x", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "y", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "z", xmlns ), NumberStyles.Float ) );

                nav.MoveToNext( );
                Vector3 scale = new Vector3( float.Parse( nav.GetAttribute( "x", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "y", xmlns ), NumberStyles.Float ), float.Parse( nav.GetAttribute( "z", xmlns ), NumberStyles.Float ) );

                data = new ProjectTransformationData( position, Quaternion.Euler( rotation ), scale );

                nav.MoveToParent( );

                return data;
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Warehouse.xml\" konnte nicht gelesen werden! Fehler: " + e.Message, LogLevel.Error, true, "WarehouseReader", "ReadTransformation" );

                return new ProjectTransformationData( );
            }
        }
    }
}
