using System;
using System.Xml;
using System.Xml.XPath;
using SystemTools.Logging;

namespace SystemTools.ManagingRessources
{
    internal class ConfigWriter
    {
        private XmlDocument Doc { get; set; }

        internal ConfigWriter( XmlDocument doc )
        {
            Doc = doc;
        }

        internal bool StoreData( string key, object data, bool overwrite, ConfigManager.ConfigBuffer buffer )
        {
            ConfigData newData;
            
            bool tmp = IsUniqueName( key, buffer );

            if ( !overwrite )
            {
                if ( !tmp )
                {
                    return false;
                }
            }

            foreach ( ConfigData cd in buffer.Data )
            {
                if ( cd.Name.Equals( key ) )
                {
                    newData = cd;
                    
                    newData.AddData( key, newData.ID, data.GetType( ).Name, false, false, data.ToString( ) );

                    return true;
                }
            }

            newData = ConfigData.Initialize( );

            newData.AddData( key, GetNextID( buffer ), data.GetType( ).Name, false, true, data.ToString( ) );

            buffer.Data.Add( newData );

            return true;
        }

        internal bool StoreData( string key, Array data, bool overwrite, ConfigManager.ConfigBuffer buffer )
        {
            ConfigData newData;
            
            string[ ] values = new string[ data.Length ];
            bool tmp = IsUniqueName( key, buffer );

            for ( int i = 0; i < data.Length; i++ )
            {
                values[ i ] = data.GetValue( i ).ToString( );
            }

            if ( !overwrite )
            {
                if ( !tmp )
                {
                    return false;
                }
            }

            foreach ( ConfigData cd in buffer.Data )
            {
                if ( cd.Name.Equals( key ) )
                {
                    newData = cd;

                    newData.AddData( key, newData.ID, data.GetValue( 0 ).GetType( ).Name, true, false, values );

                    return true;
                }
            }

            newData = ConfigData.Initialize( );

            newData.AddData( key, GetNextID( buffer ), data.GetValue( 0 ).GetType( ).Name, true, true, values );

            buffer.Data.Add( newData );

            return true;
        }

        internal bool StoreData( string key, ISerialConfigData data, bool overwrite, ConfigManager.ConfigBuffer buffer )
        {
            SerialConfigData newSData = SerialConfigData.Initialize( );
            ConfigData newData;
            string[ ] values;
            string[ ] types;
            bool tmp = IsUniqueName( key, buffer );

            if ( !overwrite )
            {
                if ( !tmp )
                {
                    return false;
                }
            }
            
            data.Serialize( newSData );

            values = new string[ SerialConfigData.RetrieveSerialData( newSData ).Count ];
            types  = new string[ SerialConfigData.RetrieveSerialData( newSData ).Count ];
            
            for ( int i = 0; i < values.Length; i++ )
            {
                ConfigData cd = SerialConfigData.RetrieveSerialData( newSData ).Pop( );

                values[ i ] = cd.GetValueAsString( );
                types[ i ]  = cd.Types[ 0 ];
            }

            foreach ( ConfigData cd in buffer.Data )
            {
                if ( cd.Name.Equals( key ) )
                {
                    newData = cd;

                    newData.AddData( key, newData.ID, types, true, values );

                    return true;
                }
            }
            
            newData = ConfigData.Initialize( );

            newData.AddData( key, GetNextID( buffer ), types, true, values );

            buffer.Data.Add( newData );

            return true;
        }

        internal bool RemoveData( string key, ConfigManager.ConfigBuffer buffer )
        {
            foreach ( ConfigData cd in buffer.Data )
            {
                if ( cd.Name.Equals( key ) )
                {
                    buffer.Data.Remove( cd );

                    return true;
                }
            }
            
            return false;
        }

        internal bool RemoveData( long id, ConfigManager.ConfigBuffer buffer )
        {
            foreach ( ConfigData cd in buffer.Data )
            {
                if ( cd.ID == id )
                {
                    buffer.Data.Remove( cd );

                    return true;
                }
            }

            return false;
        }
               
        internal void WriteConfigFile( ConfigManager.ConfigBuffer buffer, string path )
        {
            LogManager.WriteInfo( "Schreiben von ConfigDaten", "ConfigWriter", "WriteConfigFile" );

            string xmlns = "https://github.com/xShadowArmy/digital-commissioning-tool/tree/main/DigitalCommissioningTool/Output/Ressources/Data";

            XPathNavigator nav = Doc.CreateNavigator( );

            try
            {
                if ( nav.MoveToFirstChild( ) )
                {
                    nav.MoveToAttribute( "dataCount", xmlns );
                    nav.SetValue( buffer.Data.Count.ToString( ) );
                    nav.MoveToParent( );

                    while ( nav.MoveToNext(  ) );

                    for( int i = 0; i < buffer.Data.Count; i++ )
                    {
                        if ( buffer.Data[i].IsArray )
                        {
                            WriteArray( nav, buffer.Data[ i ], xmlns, (i == 0) ? true : false );
                        }

                        else
                        {
                            if ( buffer.Data[i].IsObject )
                            {
                                WriteObject( nav, buffer.Data[ i ], xmlns, ( i == 0 ) ? true : false );
                            }

                            else
                            {
                                WriteData( nav, buffer.Data[ i ], xmlns, ( i == 0 ) ? true : false );
                            }
                        }
                    }

                    XmlTextWriter writer = new XmlTextWriter( path, System.Text.Encoding.UTF8 )
                    {
                        Formatting = Formatting.Indented,
                        Indentation = 4
                    };

                    Doc.Save( writer );
                }

                else
                {
                    LogManager.WriteLog( "Fehler beim Schreiben der ConfigDaten! 'Config' Element konnte nicht gefunden werden!", LogLevel.Error, true, "ConfigWriter", "WriteConfigData" );
                }
            }

            catch ( Exception e )
            {
                LogManager.WriteLog( "Fehler beim Schreiben der ConfigDaten! Fehler: " + e.Message, LogLevel.Error, true, "ConfigWriter", "WriteConfigFile" );
            }
        }

        private void WriteData( XPathNavigator nav, ConfigData data, string xmlns, bool isFirst )
        {
            // DatenTyp festlegen
            if ( isFirst )
            {
                nav.AppendChildElement( "xs", "simpleData", xmlns, "" );
                nav.MoveToFirstChild();
            }

            else
            {
                nav.InsertElementAfter( "xs", "simpleData", xmlns, "" );
                nav.MoveToNext( );
            }

            // Attribute fuer Datentyp schreiben
            nav.CreateAttribute( "xs", "name", xmlns, data.Name );
            nav.CreateAttribute( "xs", "id", xmlns, data.ID.ToString() );
            nav.CreateAttribute( "xs", "type", xmlns, data.Types[0] );
            nav.CreateAttribute( "xs", "isArray", xmlns, data.IsArray.ToString().ToLower() );
            nav.CreateAttribute( "xs", "elementCount", xmlns, data.ArrayLength.ToString() );

            // Wert einfuegen
            nav.AppendChildElement( "xs", "value", xmlns, data.GetValueAsString( ) );
        }

        private void WriteArray( XPathNavigator nav, ConfigData data, string xmlns, bool isFirst )
        {
            // DatenTyp festlegen
            if ( isFirst )
            {
                nav.AppendChildElement( "xs", "simpleData", xmlns, "" );
                nav.MoveToFirstChild( );
            }

            else
            {
                nav.InsertElementAfter( "xs", "simpleData", xmlns, "" );
                nav.MoveToNext( );
            }

            // Attribute fuer Datentyp schreiben
            nav.CreateAttribute( "xs", "name", xmlns, data.Name );
            nav.CreateAttribute( "xs", "id", xmlns, data.ID.ToString( ) );
            nav.CreateAttribute( "xs", "type", xmlns, data.Types[ 0 ] );
            nav.CreateAttribute( "xs", "isArray", xmlns, data.IsArray.ToString( ).ToLower() );
            nav.CreateAttribute( "xs", "elementCount", xmlns, data.ArrayLength.ToString( ) );

            // Werte einfuegen
            for( int i = 0; i < data.ArrayLength; i++ )
            {
                nav.AppendChildElement( "xs", "value", xmlns, data.GetValuesAsString()[i] );
            }
        }

        private void WriteObject( XPathNavigator nav, ConfigData data, string xmlns, bool isFirst )
        {
            // DatenTyp festlegen
            if ( isFirst )
            {
                nav.AppendChildElement( "xs", "complexData", xmlns, "" );
                nav.MoveToFirstChild( );
            }

            else
            {
                nav.InsertElementAfter( "xs", "complexData", xmlns, "" );
                nav.MoveToNext( );
            }

            // Attribute fuer Datentyp schreiben
            nav.CreateAttribute( "xs", "name", xmlns, data.Name );
            nav.CreateAttribute( "xs", "id", xmlns, data.ID.ToString( ) );
            nav.CreateAttribute( "xs", "type", xmlns, "objectData" );
            nav.CreateAttribute( "xs", "isArray", xmlns, false.ToString() );
            nav.CreateAttribute( "xs", "elementCount", xmlns, 1.ToString() );

            // Werte einfuegen
            nav.AppendChildElement( "xs", "values", xmlns, "" );
            nav.MoveToChild( "values", xmlns );
            nav.CreateAttribute( "xs", "propertyCount", xmlns, data.ArrayLength.ToString() );

            for ( int i = 0; i < data.ArrayLength; i++ )
            {
                nav.AppendChildElement( "xs", "property", xmlns, "" );
            }

            nav.MoveToChild( "property", xmlns );

            for ( int i = 0; i < data.ArrayLength; i++ )
            {
                nav.CreateAttribute( "xs", "type", xmlns, data.Types[i] );
                nav.CreateAttribute( "xs", "value", xmlns, data.GetValuesAsString( )[ i ] );
                nav.MoveToNext( );
            }

            nav.MoveToParent( );
            nav.MoveToParent( );
        }            

        private bool IsUniqueName( string key, ConfigManager.ConfigBuffer buffer )
        {
            foreach( ConfigData data in buffer.Data )
            {
                if ( data.Name.Equals( key ) )
                {
                    return false;
                }                
            }

            return true;
        }

        private long GetNextID( ConfigManager.ConfigBuffer buffer )
        {
            bool used = false;
            long[ ] ids = new long[ buffer.Data.Count ];

            for( int i = 0; i < ids.Length; i++ )
            {
                ids[ i ] = buffer.Data[ i ].ID;
            }

            for( int i = 0; ; i++ )
            {
                used = false;

                for ( int j = 0; j < ids.Length; j++ )
                {
                    if ( i + 1 == ids[j] )
                    {
                        used = true;
                        break;
                    }
                }

                if ( !used )
                {
                    return i + 1;
                }
            }
        }
    }
}
