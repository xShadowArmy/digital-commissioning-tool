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
    /// <summary>
    /// Ließt Container Daten aus einer Xml Datei.
    /// </summary>
    internal class ContainerReader
    {
        /// <summary>
        /// Die Datei aus der die Daten gelesen werden.
        /// </summary>
        private XmlDocument Doc { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="doc">Die Datei aus der gelesen werden soll.</param>
        internal ContainerReader( XmlDocument doc )
        {
            Doc = doc;
        }

        /// <summary>
        /// Ließt die Daten aus der Datei.
        /// </summary>
        /// <returns>Objekt das die gelesenen Daten enthält.</returns>
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
                        string name = nav.GetAttribute( "itemName", xmlns );

                        if ( !name.Equals( "null" ) )
                        {
                            long idRef  = long.Parse( nav.GetAttribute( "idRef", xmlns ), NumberStyles.Integer );
                            long iid  = long.Parse( nav.GetAttribute( "id", xmlns ), NumberStyles.Integer );
                            double weight = double.Parse( nav.GetAttribute( "itemWeight", xmlns ), NumberStyles.Number );
                            int count = int.Parse( nav.GetAttribute( "itemCount", xmlns ) );
                            bool inQueue = bool.Parse( nav.GetAttribute( "inQueue", xmlns ) );
                            int posQueue = int.Parse( nav.GetAttribute( "posQueue", xmlns ), NumberStyles.Integer );
                            long parentItemID = long.Parse( nav.GetAttribute( "parentItemID", xmlns ), NumberStyles.Integer );
                            long parentStorageID = long.Parse( nav.GetAttribute( "parentStorageID", xmlns ), NumberStyles.Integer );

                            item = new ProjectItemData( idRef, iid, count, weight, name, inQueue, posQueue, parentItemID, parentStorageID, ReadTransformation( nav, xmlns ) );

                            data.Items[int.Parse( nav.GetAttribute( "slot", xmlns ), NumberStyles.Integer )] = item;
                        }

                        nav.MoveToNext( );
                    }

                    nav.MoveToParent( );

                    container.Container.Add( data );

                    nav.MoveToParent( );
                    nav.MoveToNext( "Container", xmlns);
                } 
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Container.xml\" konnte nicht gelesen werden! Fehler: " + e.Message, LogLevel.Error, true, "ContainerReader", "Readfile" );
            }

            return container;
        }
        
        /// <summary>
        /// Ließt die Transformationsdaten aus der Datei.
        /// </summary>
        /// <param name="nav">Navigator mit passender Position.</param>
        /// <param name="xmlns">Xml Namespace der verwendet werden soll.</param>
        /// <returns>Die gelesenen Transformationsdaten.</returns>
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

                catch ( Exception )
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

            catch ( Exception e )
            {
                LogManager.WriteLog( "Datei \"Warehouse.xml\" konnte nicht gelesen werden! Fehler: " + e.Message, LogLevel.Error, true, "WarehouseReader", "ReadTransformation" );

                return new ProjectTransformationData( );
            }
        }
    }
}
