using System;
using SystemTools.Handler;

namespace SystemTools
{
    /// <summary>
    /// Repräsentiert gespeicherte Configurations Daten.
    /// </summary>
    public class ConfigData
    {
        /// <summary>
        /// Der gespeicherte Schlüssel.
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Die ID der Daten.
        /// </summary>
        public long ID { get; private set; }

        /// <summary>
        /// Gibt an ob die gespeicherten Daten ein Array von Daten sind.
        /// </summary>
        public bool IsArray { get; private set; }

        /// <summary>
        /// Gibt an ob die gespeicherten Daten ein Objekt sind.
        /// </summary>
        public bool IsObject { get; private set; }
        
        /// <summary>
        /// Gibt die Anzahl der gespeicherten Daten zurück.
        /// </summary>
        public long ArrayLength
        {
            get
            {
                return Data.Length;
            }
        }

        /// <summary>
        /// Gibt an ob die gespeicherten Daten neu hinzugefügt wurden.
        /// </summary>
        internal bool IsBuffered { get; private set; }

        /// <summary>
        /// Gibt die Typnamen der einzelnen Daten zurück.
        /// </summary>
        internal string[ ] Types { get; private set; }

        /// <summary>
        /// Die gespeicherten Daten.
        /// </summary>
        private string[ ] Data { get; set; }

        /// <summary>
        /// Wird fuer das Schreiben von Logdateien verwendet.
        /// </summary>
        private LogHandler Logger;

        /// <summary>
        /// Erstellt eine neue leere Instanz.
        /// </summary>
        protected ConfigData()
        {
            Data = null;
            ID = 0;
            Types = null;
            IsArray = false;
            Data = null;
            Logger = new LogHandler( );
        }

        /// <summary>
        /// Gibt den gespeicherten Wert als String zurück.
        /// </summary>
        /// <returns>Der gespeicherte Wert.</returns>
        public string GetValueAsString()
        {
            return Data[0];
        }

        /// <summary>
        /// Gibt den gespeicherten Wert als Short zurück.
        /// </summary>
        /// <exception cref="InvalidCastException">Wird geworfen wenn der Typ nicht mit dem Typ der gespeicherten Daten übereinstimmt.</exception>
        /// <returns>Der gespeicherte Wert.</returns>
        public short GetValueAsShort()
        {
            CheckType( "int16" );

            return short.Parse( Data[0] );
        }

        /// <summary>
        /// Gibt den gespeicherten Wert als Long zurück.
        /// </summary>
        /// <exception cref="InvalidCastException">Wird geworfen wenn der Typ nicht mit dem Typ der gespeicherten Daten übereinstimmt.</exception>
        /// <returns>Der gespeicherte Wert.</returns>
        public long GetValueAsLong()
        {
            CheckType( "int64" );

            return long.Parse( Data[ 0 ] );
        }

        /// <summary>
        /// Gibt den gespeicherten Wert als Int zurück.
        /// </summary>
        /// <exception cref="InvalidCastException">Wird geworfen wenn der Typ nicht mit dem Typ der gespeicherten Daten übereinstimmt.</exception>
        /// <returns>Der gespeicherte Wert.</returns>
        public int GetValueAsInt()
        {
            CheckType( "int32" );

            return int.Parse( Data[ 0 ] );
        }

        /// <summary>
        /// Gibt den gespeicherten Wert als Float zurück.
        /// </summary>
        /// <exception cref="InvalidCastException">Wird geworfen wenn der Typ nicht mit dem Typ der gespeicherten Daten übereinstimmt.</exception>
        /// <returns>Der gespeicherte Wert.</returns>
        public float GetValueAsFloat()
        {
            CheckType( "float" );

            return float.Parse( Data[ 0 ] );
        }

        /// <summary>
        /// Gibt den gespeicherten Wert als Double zurück.
        /// </summary>
        /// <exception cref="InvalidCastException">Wird geworfen wenn der Typ nicht mit dem Typ der gespeicherten Daten übereinstimmt.</exception>
        /// <returns>Der gespeicherte Wert.</returns>
        public double GetValueAsDouble()
        {
            CheckType( "double" );

            return double.Parse( Data[ 0 ] );
        }

        /// <summary>
        /// Gibt den gespeicherten Wert als Bool zurück.
        /// </summary>
        /// <exception cref="InvalidCastException">Wird geworfen wenn der Typ nicht mit dem Typ der gespeicherten Daten übereinstimmt.</exception>
        /// <returns>Der gespeicherte Wert.</returns>
        public bool GetValueAsBool()
        {
            CheckType( "boolean" );

            return bool.Parse( Data[ 0 ] );
        }

        /// <summary>
        /// Gibt die gespeicherten Werte als StringArray zurück.
        /// </summary>
        /// <returns>Die gespeicherten Werte.</returns>
        public string[ ] GetValuesAsString()
        {
            string[ ] tmp = new string[ Data.Length ];

            Data.CopyTo( tmp, 0 );

            return tmp;
        }

        /// <summary>
        /// Gibt die gespeicherten Werte als ShortArray zurück.
        /// </summary>
        /// <exception cref="InvalidCastException">Wird geworfen wenn der Typ nicht mit dem Typ der gespeicherten Daten übereinstimmt.</exception>
        /// <returns>Die gespeicherten Werte.</returns>
        public short[ ] GetValuesAsShort()
        {
            CheckType( "int16" );

            short[ ] tmp = new short[ Data.Length ];

            for( int i = 0; i < Data.Length; i++ )
            {
                tmp[ i ] = short.Parse( Data[ i ] );
            }

            return tmp;
        }
        
        /// <summary>
        /// Gibt die gespeicherten Werte als LongArray zurück.
        /// </summary>
        /// <exception cref="InvalidCastException">Wird geworfen wenn der Typ nicht mit dem Typ der gespeicherten Daten übereinstimmt.</exception>
        /// <returns>Die gespeicherten Werte.</returns>
        public long[ ] GetValuesAsLong()
        {
            CheckType( "int64" );

            long[ ] tmp = new long[ Data.Length ];

            for ( int i = 0; i < Data.Length; i++ )
            {
                tmp[ i ] = long.Parse( Data[ i ] );
            }

            return tmp;
        }

        /// <summary>
        /// Gibt die gespeicherten Werte als IntArray zurück.
        /// </summary>
        /// <exception cref="InvalidCastException">Wird geworfen wenn der Typ nicht mit dem Typ der gespeicherten Daten übereinstimmt.</exception>
        /// <returns>Die gespeicherten Werte.</returns>
        public int[ ] GetValuesAsInt()
        {
            CheckType( "int32" );

            int[ ] tmp = new int[ Data.Length ];

            for ( int i = 0; i < Data.Length; i++ )
            {
                tmp[ i ] = int.Parse( Data[ i ] );
            }

            return tmp;
        }
        
        /// <summary>
        /// Gibt die gespeicherten Werte als FloatArray zurück.
        /// </summary>
        /// <exception cref="InvalidCastException">Wird geworfen wenn der Typ nicht mit dem Typ der gespeicherten Daten übereinstimmt.</exception>
        /// <returns>Die gespeicherten Werte.</returns>
        public float[ ] GetValuesAsFloat()
        {
            CheckType( "float" );

            float[ ] tmp = new float[ Data.Length ];

            for ( int i = 0; i < Data.Length; i++ )
            {
                tmp[ i ] = float.Parse( Data[ i ] );
            }

            return tmp;
        }
        
        /// <summary>
        /// Gibt die gespeicherten Werte als DoubleArray zurück.
        /// </summary>
        /// <exception cref="InvalidCastException">Wird geworfen wenn der Typ nicht mit dem Typ der gespeicherten Daten übereinstimmt.</exception>
        /// <returns>Die gespeicherten Werte.</returns>
        public double[ ] GetValuesAsDouble()
        {
            CheckType( "double" );

            double[ ] tmp = new double[ Data.Length ];

            for ( int i = 0; i < Data.Length; i++ )
            {
                tmp[ i ] = double.Parse( Data[ i ] );
            }

            return tmp;
        }
        
        /// <summary>
        /// Gibt die gespeicherten Werte als BoolArray zurück.
        /// </summary>
        /// <exception cref="InvalidCastException">Wird geworfen wenn der Typ nicht mit dem Typ der gespeicherten Daten übereinstimmt.</exception>
        /// <returns>Die gespeicherten Werte.</returns>
        public bool[ ] GetValuesAsBool()
        {
            CheckType( "boolean" );

            bool[ ] tmp = new bool[ Data.Length ];

            for ( int i = 0; i < Data.Length; i++ )
            {
                tmp[ i ] = bool.Parse( Data[ i ] );
            }

            return tmp;
        }
        
        /// <summary>
        /// Gibt eine neue Instanz zurück.
        /// </summary>
        /// <returns>Eine neue Instanz der ConfigDaten.</returns>
        internal static ConfigData Initialize()
        {
            return new ConfigData( );
        }

        /// <summary>
        /// Speichert die angegebenen Daten (SimpleData).
        /// </summary>
        /// <param name="name">Der verwendete Schlüssel.</param>
        /// <param name="id">Die eindeutige ID der Daten</param>
        /// <param name="type">Der Typ der Daten.</param>
        /// <param name="isArray">Gibt an ob die Daten ein Array sind.</param>
        /// <param name="isNew">Gibt an ob die Daten neu hinzugefügt wurden.</param>
        /// <param name="data">Die zu speichernden Daten.</param>
        internal void AddData( string name, long id, string type, bool isArray, bool isNew, params string[ ] data )
        {
            Logger.WriteInfo( "Puffern von einfachen ConfigDaten.", "ConfigData", "AddData" );

            Name = name;
            ID = id;
            IsArray = isArray;
            IsObject = false;
            IsBuffered = isNew;

            Types = new string[ data.Length ];

            for ( long i = 0; i < data.Length; i++ )
            {
                Types[ i ] = type;
            }

            Data = new string[ data.Length ];
            data.CopyTo( Data, 0 );
        }

        /// <summary>
        /// Speichert die angegebenen Daten (ComplexData).
        /// </summary>
        /// <param name="name">Der verwendete Schlüssel.</param>
        /// <param name="id">Die eindeutige ID der Daten.</param>
        /// <param name="types">Die Typen der einzelnen Daten.</param>
        /// <param name="isNew">Gibt an ob die Daten neu hinzugefügt wurden.</param>
        /// <param name="data">Die Daten des zu speichernden Objekts.</param>
        internal void AddData( string name, long id, string[ ] types, bool isNew, params string[ ] data )
        {
            Logger.WriteInfo( "Puffern von complexen ConfigDaten.", "ConfigData", "AddData" );

            Name = name;
            ID = id;
            IsArray = false;
            IsObject = true;
            IsBuffered = isNew;

            Types = new string[ data.Length ];

            for ( long i = 0; i < data.Length; i++ )
            {
                Types[ i ] = types[ i ];
            }

            Data = new string[ data.Length ];
            data.CopyTo( Data, 0 );
        }

        /// <summary>
        /// Überprüft den Typ der gespeicherten Daten mit dem angegebenen Typ.
        /// </summary>
        /// <param name="typeName">Der Name des Types in den die Daten umgewandelt werden sollen.</param>
        /// <exception cref="InvalidCastException">Wird geworfen wenn der Typ der Daten nicht mit dem angegebenen Typ übereinstimmt.</exception>
        private void CheckType( string typeName )
        {
            if ( IsObject )
            {
                Logger.WriteError( "Kann Serialisiertes Objekt nicht in einfachen Typen umwandeln!", "ConfigData", "CheckType" );
                throw new InvalidCastException( "Kann Serialisiertes Objekt nicht in einfachen Typen umwandeln!" );
            }

            if ( !typeName.ToLower().Equals( Types[0].ToLower() ) )
            {
                if ( typeName.Equals( "string" ) && Types[0].ToLower().Equals( "char" ) )
                {
                    return;
                }

                if ( typeName.Equals( "float" ) && Types[ 0 ].ToLower( ).Equals( "single" ) )
                {
                    return;
                }

                Logger.WriteError( "Kann Daten nicht in angegebenen Typ umwandeln! '" + Types[ 0 ] + "' zu '" + typeName + "'", "ConfigData", "CheckType" );
                throw new InvalidCastException( "Kann Daten nicht in angegebenen Typ umwandeln! '" + Types[0] + "' zu '" + typeName + "'" );
            }
        }
    }
}
