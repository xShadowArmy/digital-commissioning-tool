using System;
using System.Xml;
using System.Xml.XPath;
using SystemTools.Logging;
using SystemTools.Handler;

namespace SystemTools.ManagingResources
{
    /// <summary>
    /// Ermöglicht das Lesen von Konfigurations Dateien.
    /// </summary>
    internal class ConfigReader
    {
        /// <summary>
        /// Die Konfigurationsdatei.
        /// </summary>
        private XmlDocument Doc { get; set; }

        /// <summary>
        /// Wird fuer das Schreiben von LogDateien verwendet.
        /// </summary>
        private LogHandler Logger;

        /// <summary>
        /// Erstellt eine neue Instanz und ließt die Daten in den Puffer.
        /// </summary>
        /// <param name="doc">Die Konfigurationsdatei.</param>
        /// <param name="buffer">Der Datenpuffer.</param>
        /// <param name="newFile">Gibt an ob die Datei neu erstellt wurde.</param>
        internal ConfigReader( XmlDocument doc, ConfigHandler.ConfigBuffer buffer, bool newFile )
        {
            Logger = new LogHandler( );
            Doc = doc;

            if ( newFile )
            {
                return;
            }

            FillBuffer( buffer );
        }

        /// <summary>
        /// Ließt Daten aus dem Puffer.
        /// </summary>
        /// <param name="buffer">Der Puffer aus dem gelesen werden soll.</param>
        /// <param name="name">Der Schlüssel der Daten die gelesen werden sollen.</param>
        /// <returns>Die gelesenen Daten.</returns>
        internal ConfigData LoadData( ConfigHandler.ConfigBuffer buffer, string name )
        {
            for( int i = 0; i < buffer.Data.Count; i++ )
            {
                if ( buffer.Data[i].Name.Equals( name ) )
                {
                    return buffer.Data[ i ];
                }
            }

            return null;
        }

        /// <summary>
        /// Ließt Daten aus dem Puffer.
        /// </summary>
        /// <param name="buffer">Der Puffer aus dem gelesen werden soll.</param>
        /// <param name="id">Die ID der Daten die gelesen werden sollen.</param>
        /// <returns>Die gelesenen Daten.</returns>
        internal ConfigData LoadData( ConfigHandler.ConfigBuffer buffer, long id )
        {
            for ( int i = 0; i < buffer.Data.Count; i++ )
            {
                if ( buffer.Data[ i ].ID == id )
                {
                    return buffer.Data[ i ];
                }
            }

            return null;
        }

        /// <summary>
        /// Ließt Daten aus dem Puffer.
        /// </summary>
        /// <param name="buffer">Der Puffer aus dem gelesen werden soll.</param>
        /// <param name="name">Der Schlüssel der gelesen werden soll.</param>
        /// <param name="data">Das Objekt das wiederhergestellt werden soll.</param>
        internal void LoadData( ConfigHandler.ConfigBuffer buffer, string name, ISerialConfigData data )
        {
            ConfigData cd = null;
            SerialConfigData scd;

            for ( int i = 0; i < buffer.Data.Count; i++ )
            {
                if ( buffer.Data[ i ].Name.Equals( name ) )
                {                    
                    cd = buffer.Data[ i ];

                    break;
                }
            }

            if ( cd == null || !cd.IsObject )
            {
                return;
            }

            scd = SerialConfigData.Initialize( );

            for( int i = 0; i < cd.ArrayLength; i++ )
            {
                SerialConfigData.AddData( scd, cd.GetValuesAsString( )[i], cd.Types[ i ] );
            }

            data.Restore( scd );
        }

        /// <summary>
        /// Füllt den Puffer mit den Daten der Konfigurations Datei.
        /// </summary>
        /// <param name="buffer">Der Puffer der gefüllt werden soll.</param>
        private void FillBuffer( ConfigHandler.ConfigBuffer buffer )
        {
            Logger.WriteInfo( "Einlesen von ConfigDaten", "ConfigReader", "FillBuffer" );

            string xmlns = "https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Resources/";

            XPathNavigator nav = Doc.CreateNavigator( );

            try
            {
                if ( nav.MoveToFirstChild() )
                {                 
                    if ( !long.TryParse( nav.GetAttribute( "dataCount", xmlns ), out long dataCount ) )
                    {
                        Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! DataCount attribut ist keine ganze zahl!", 3, true, "ConfigReader", "FilBuffer" );
                    }

                    if ( dataCount > 0 )
                    {
                        if ( !nav.MoveToFirstChild( ) )
                        {
                            Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! DataCount entspricht nicht der Anzahl an Daten!", 2, false, "ConfigReader", "FilBuffer" );
                        }
                    }

                    for ( long i = 0; i < dataCount; i++ )
                    {
                        switch ( nav.LocalName )
                        {
                            case "SimpleData":

                                ReadSimpleElement( nav, buffer, xmlns );
                                break;

                            case "ComplexData":

                                ReadComplexElement( nav, buffer, xmlns );
                                break;

                            default:

                                Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Unbekanntes Datenelement!", 2, false, "ConfigReader", "FilBuffer" );
                                break;
                        }

                        nav.MoveToNext( );
                    }
                }

                else
                {
                    Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! 'Config' Element konnte nicht gefunden werden!", 3, true, "ConfigReader", "FilBuffer" );
                }
            }

            catch( Exception e )
            {
                Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Fehler: " + e.Message, 3, true, "ConfigReader", "FilBuffer" );
            }
        }

        /// <summary>
        /// Ließt ein einfaches Datenelement in den Puffer-
        /// </summary>
        /// <param name="nav">Der Navigator an Position des einfachen Datenelements.</param>
        /// <param name="buffer">Der Puffer in den die Daten geschrieben werden sollen.</param>
        /// <param name="xmlns">Der Xml namespace.</param>
        private void ReadSimpleElement( XPathNavigator nav, ConfigHandler.ConfigBuffer buffer, string xmlns )
        {
            Logger.WriteLog( "Einlesen von simpleData Element", 1, false, "ConfigReader", "ReadSimpleElement" );

            try
            {
                string name = nav.GetAttribute( "name", xmlns );
                string type = nav.GetAttribute( "type", xmlns );
                string[ ] values;

                if ( name.Equals( string.Empty ) )
                {
                    Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Konnte 'name' nicht lesen! ", 3, true, "ConfigReader", "ReadSimpleElement" );
                }

                if ( type.Equals( string.Empty ) )
                {
                    Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Konnte 'type' nicht lesen! ", 3, true, "ConfigReader", "ReadSimpleElement" );
                }

                if ( !long.TryParse( nav.GetAttribute( "id", xmlns ), out long id ) )
                {
                    Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Konnte 'id; nicht lesen! ", 3, true, "ConfigReader", "ReadSimpleElement" );
                }

                if ( !long.TryParse( nav.GetAttribute( "elementCount", xmlns ), out long elementCount ) )
                {
                    Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Konnte 'elementCount' nicht lesen! ", 3, true, "ConfigReader", "ReadSimpleElement" );
                }

                if ( !bool.TryParse( nav.GetAttribute( "isArray", xmlns ), out bool isArray ) )
                {
                    Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Konnte 'isArray' nicht lesen! ", 3, true, "ConfigReader", "ReadSimpleElement" );
                }

                ConfigData data = ConfigData.Initialize( );

                values = new string[ elementCount ];

                if ( !nav.MoveToFirstChild() )
                {
                    Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Daten angelegt aber kein Wert! ", 3, true, "ConfigReader", "ReadSimpleElement" );
                }
                
                for( long i = 0; i < elementCount; i++ )
                {
                    values[ i ] = nav.Value;

                    nav.MoveToNext( );
                }

                nav.MoveToParent( );

                data.AddData( name, id, type, isArray, false, values );
                buffer.Data.Add( data );
            }

            catch( Exception e )
            {
                Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Konnte Simples Datenelement nicht lesen! Fehler: " + e.Message, 3, true, "ConfigReader", "ReadSimpleElement" );
            }
        }
        
        /// <summary>
        /// Ließt ein complexes Datenelement in den Puffer-
        /// </summary>
        /// <param name="nav">Der Navigator an Position des complexen Datenelements.</param>
        /// <param name="buffer">Der Puffer in den die Daten geschrieben werden sollen.</param>
        /// <param name="xmlns">Der Xml namespace.</param>
        private void ReadComplexElement( XPathNavigator nav, ConfigHandler.ConfigBuffer buffer, string xmlns )
        {
            Logger.WriteLog( "Einlesen von ComplexData Element", 1, false, "ConfigReader", "ReadComplexElement" );
        
            try
            {
                string name = nav.GetAttribute( "name", xmlns );
                string type = nav.GetAttribute( "type", xmlns );
                string[ ] values;
                string[ ] types;

                if ( name.Equals( string.Empty ) )
                {
                    Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Konnte 'name' nicht lesen! ", 3, true, "ConfigReader", "ReadComplexElement" );
                }

                if ( !type.Equals( "objectData" ) )
                {
                    Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Falscher Type fuer complexData! ", 3, true, "ConfigReader", "ReadComplexElement" );
                }

                if ( !long.TryParse( nav.GetAttribute( "id", xmlns ), out long id ) )
                {
                    Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Konnte 'id; nicht lesen! ", 3, true, "ConfigReader", "ReadComplexElement" );
                }

                if ( !nav.MoveToFirstChild() )
                {
                    Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! 'Values' element nicht vorhanden! ", 3, true, "ConfigReader", "ReadComplexElement" );
                }

                if ( !long.TryParse( nav.GetAttribute( "propertyCount", xmlns ), out long propertyCount ) )
                {
                    Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Konnte 'propertyCount; nicht lesen! ", 3, true, "ConfigReader", "ReadComplexElement" );
                }

                ConfigData data = ConfigData.Initialize( );

                values = new string[ propertyCount ];
                types  = new string[ propertyCount ];

                if ( !nav.MoveToFirstChild( ) )
                {
                    Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! complexElement hat keinen Wert! ", 3, true, "ConfigReader", "ReadComplexElement" );
                }

                for ( long i = 0; i < propertyCount; i++ )
                {
                    values[ i ] = nav.GetAttribute( "value", xmlns );
                    types[ i ]  = nav.GetAttribute( "type", xmlns );

                    nav.MoveToNext( );
                }

                nav.MoveToParent( );
                nav.MoveToParent( );

                data.AddData( name, id, types, false, values );
                buffer.Data.Add( data );
            }

            catch ( Exception e )
            {
                Logger.WriteLog( "Fehler beim Einlesen der ConfigDaten! Konnte Simples Datenelement nicht lesen! Fehler: " + e.Message, 3, true, "ConfigReader", "ReadComplexElement" );
            }
        }
    }
}
