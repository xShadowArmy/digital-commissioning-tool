using System;
using SystemTools.Logging;

namespace SystemTools.ManagingRessources
{
    public class ConfigData
    {
        public string Name { get; private set; }
        public long ID { get; private set; }
        public bool IsArray { get; private set; }
        public bool IsObject { get; private set; }
        
        public long ArrayLength
        {
            get
            {
                return Data.Length;
            }
        }

        internal bool IsBuffered { get; private set; }
        internal string[ ] Types { get; private set; }
        private string[ ] Data { get; set; }

        private ConfigData()
        {
            Data = null;
            ID = 0;
            Types = null;
            IsArray = false;
            Data = null;
        }

        public string GetValueAsString()
        {
            return Data[0];
        }

        public short GetValueAsShort()
        {
            CheckType( "int16" );

            return short.Parse( Data[0] );
        }

        public long GetValueAsLong()
        {
            CheckType( "int64" );

            return long.Parse( Data[ 0 ] );
        }

        public int GetValueAsInt()
        {
            CheckType( "int32" );

            return int.Parse( Data[ 0 ] );
        }

        public float GetValueAsFloat()
        {
            CheckType( "float" );

            return float.Parse( Data[ 0 ] );
        }

        public double GetValueAsDouble()
        {
            CheckType( "double" );

            return double.Parse( Data[ 0 ] );
        }

        public bool GetValueAsBool()
        {
            CheckType( "boolean" );

            return bool.Parse( Data[ 0 ] );
        }

        public string[] GetValuesAsString()
        {
            string[ ] tmp = new string[ Data.Length ];

            Data.CopyTo( tmp, 0 );

            return tmp;
        }

        public short[] GetValuesAsShort()
        {
            CheckType( "int16" );

            short[ ] tmp = new short[ Data.Length ];

            for( int i = 0; i < Data.Length; i++ )
            {
                tmp[ i ] = short.Parse( Data[ i ] );
            }

            return tmp;
        }

        public long[] GetValuesAsLong()
        {
            CheckType( "int64" );

            long[ ] tmp = new long[ Data.Length ];

            for ( int i = 0; i < Data.Length; i++ )
            {
                tmp[ i ] = long.Parse( Data[ i ] );
            }

            return tmp;
        }

        public int[] GetValuesAsInt()
        {
            CheckType( "int32" );

            int[ ] tmp = new int[ Data.Length ];

            for ( int i = 0; i < Data.Length; i++ )
            {
                tmp[ i ] = int.Parse( Data[ i ] );
            }

            return tmp;
        }

        public float[] GetValuesAsFloat()
        {
            CheckType( "float" );

            float[ ] tmp = new float[ Data.Length ];

            for ( int i = 0; i < Data.Length; i++ )
            {
                tmp[ i ] = float.Parse( Data[ i ] );
            }

            return tmp;
        }

        public double[] GetValuesAsDouble()
        {
            CheckType( "double" );

            double[ ] tmp = new double[ Data.Length ];

            for ( int i = 0; i < Data.Length; i++ )
            {
                tmp[ i ] = double.Parse( Data[ i ] );
            }

            return tmp;
        }

        public bool[] GetValuesAsBool()
        {
            CheckType( "boolean" );

            bool[ ] tmp = new bool[ Data.Length ];

            for ( int i = 0; i < Data.Length; i++ )
            {
                tmp[ i ] = bool.Parse( Data[ i ] );
            }

            return tmp;
        }
        
        internal static ConfigData Initialize()
        {
            return new ConfigData( );
        }

        internal void AddData( string name, long id, string type, bool isArray, bool isNew, params string[ ] data )
        {
            LogManager.WriteInfo( "Speichern von einfachen ConfigDaten.", "ConfigData", "AddData" );

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

        internal void AddData( string name, long id, string[ ] types, bool isNew, params string[ ] data )
        {
            LogManager.WriteInfo( "Speichern von complexen ConfigDaten.", "ConfigData", "AddData" );

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

        private void CheckType( string typeName )
        {
            if ( IsObject )
            {
                LogManager.WriteError( "Kann Serialisiertes Objekt nicht in einfachen Typen umwandeln!", "ConfigData", "CheckType" );
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

                LogManager.WriteError( "Kann Daten nicht in angegebenen Typ umwandeln! '" + Types[ 0 ] + "' zu '" + typeName + "'", "ConfigData", "CheckType" );
                throw new InvalidCastException( "Kann Daten nicht in angegebenen Typ umwandeln! '" + Types[0] + "' zu '" + typeName + "'" );
            }
        }
    }
}
