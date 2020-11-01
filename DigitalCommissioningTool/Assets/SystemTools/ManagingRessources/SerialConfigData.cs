using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTools.Logging;

namespace SystemTools.ManagingRessources
{
    public class SerialConfigData
    {
        private Stack<ConfigData> Data { get; set; }

        private SerialConfigData()
        {
            Data = new Stack<ConfigData>();
        }

        public void AddData( object data )
        {
            ConfigData newData = ConfigData.Initialize( );

            newData.AddData( string.Empty, 0, data.GetType( ).Name, false, true, data.ToString() );

            Data.Push( newData );
        }
        
        public string GetValueAsString()
        {
            return Data.Pop().GetValueAsString( );
        }

        public short GetValueAsShort()
        {
            return Data.Pop( ).GetValueAsShort( );
        }

        public long GetValueAsLong()
        {
            return Data.Pop( ).GetValueAsLong( );
        }

        public int GetValueAsInt()
        {
            return Data.Pop( ).GetValueAsInt( );
        }

        public float GetValueAsFloat()
        {
            return Data.Pop( ).GetValueAsFloat( );
        }

        public double GetValueAsDouble()
        {
            return Data.Pop( ).GetValueAsDouble( );
        }

        public bool GetValueAsBool()
        {
            return Data.Pop( ).GetValueAsBool( );
        }
        
        internal static void AddData( SerialConfigData obj, object data, string type )
        {
            ConfigData newData = ConfigData.Initialize( );

            newData.AddData( string.Empty, 0, type, false, true, data.ToString( ) );

            obj.Data.Push( newData );
        }

        internal static Stack<ConfigData> RetrieveSerialData( SerialConfigData data )
        {
            return data.Data;
        }

        internal static SerialConfigData Initialize()
        {
            return new SerialConfigData( );
        }
    }
}
