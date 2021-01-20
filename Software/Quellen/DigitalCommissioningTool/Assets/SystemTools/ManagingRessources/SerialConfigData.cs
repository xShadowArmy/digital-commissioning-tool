using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemTools.Logging;

namespace SystemTools
{
    /// <summary>
    /// Enthält Daten über ein serialisiertes Objekt.
    /// </summary>
    public class SerialConfigData
    {
        /// <summary>
        /// Die "serialisierten" Daten.
        /// </summary>
        private Stack<ConfigData> Data { get; set; }

        /// <summary>
        /// Erstellt eine neue leere Instanz.
        /// </summary>
        protected SerialConfigData()
        {
            Data = new Stack<ConfigData>();
        }

        /// <summary>
        /// Fügt ein Wert zu den Serialisierten Daten hinzu.
        /// </summary>
        /// <param name="data">Die Daten die serialisiert werden sollen.</param>
        public void AddData( object data )
        {
            ConfigData newData = ConfigData.Initialize( );

            newData.AddData( string.Empty, 0, data.GetType( ).Name, false, true, data.ToString() );

            Data.Push( newData );
        }

        /// <summary>
        /// Gibt einen serialisierten Wert als String zurück.
        /// </summary>
        /// <returns>Der Wert als string.</returns>
        public string GetValueAsString()
        {
            return Data.Pop().GetValueAsString( );
        }

        /// <summary>
        /// Gibt einen serialisierten Wert als Short zurück.
        /// </summary>
        /// <returns>Der Wert als short.</returns>
        public short GetValueAsShort()
        {
            return Data.Pop( ).GetValueAsShort( );
        }

        /// <summary>
        /// Gibt einen serialisierten Wert als Long zurück.
        /// </summary>
        /// <returns>Der Wert als long.</returns>
        public long GetValueAsLong()
        {
            return Data.Pop( ).GetValueAsLong( );
        }

        /// <summary>
        /// Gibt einen serialisierten Wert als Int zurück.
        /// </summary>
        /// <returns>Der Wert als int.</returns>
        public int GetValueAsInt()
        {
            return Data.Pop( ).GetValueAsInt( );
        }

        /// <summary>
        /// Gibt einen serialisierten Wert als Float zurück.
        /// </summary>
        /// <returns>Der Wert als Float.</returns>
        public float GetValueAsFloat()
        {
            return Data.Pop( ).GetValueAsFloat( );
        }

        /// <summary>
        /// Gibt einen serialisierten Wert als Double zurück.
        /// </summary>
        /// <returns>Der Wert als Double.</returns>
        public double GetValueAsDouble()
        {
            return Data.Pop( ).GetValueAsDouble( );
        }

        /// <summary>
        /// Gibt einen serialisierten Wert als Bool zurück.
        /// </summary>
        /// <returns>Der Wert als bool.</returns>
        public bool GetValueAsBool()
        {
            return Data.Pop( ).GetValueAsBool( );
        }

        /// <summary>
        /// Gibt einen serialisierten Wert als Long zurück.
        /// </summary>
        /// <returns>Der Wert als long.</returns>
        internal static void AddData( SerialConfigData obj, object data, string type )
        {
            ConfigData newData = ConfigData.Initialize( );

            newData.AddData( string.Empty, 0, type, false, true, data.ToString( ) );

            obj.Data.Push( newData );
        }

        /// <summary>
        /// Gibt die serialisierten Daten zurück.
        /// </summary>
        /// <param name="data">Das Objekt dessen serialisierte Daten abgefragt werden sollen.</param>
        /// <returns>Die "serialisierten" Daten.</returns>
        internal static Stack<ConfigData> RetrieveSerialData( SerialConfigData data )
        {
            return data.Data;
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <returns>Die neue Instanz.</returns>
        internal static SerialConfigData Initialize()
        {
            return new SerialConfigData( );
        }
    }
}
