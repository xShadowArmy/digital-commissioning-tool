using UnityEngine;
using SystemTools.ManagingRessources;
using System;

namespace UnitTests
{
    class ConfigTestClass : ISerialConfigData
    {
        private string TestString { get; set; }
        private double TestValue  { get; set; }
        private int[] Data { get; set; }

        public void Restore( SerialConfigData storage )
        {
            TestString = storage.GetValueAsString( );
            TestValue  = storage.GetValueAsDouble( );
        }

        public void Serialize( SerialConfigData storage )
        {
            storage.AddData( TestString );
            storage.AddData( TestValue  );
        }

        public void Test()
        {
            ConfigManager cman = new ConfigManager( );

            cman.OpenConfigFile( "settings.xml", true );

            // Testen ob Daten korrekt entfernt werden.
            cman.RemoveData( "WorkingDirectory" );

            // Ausgabe von gelesenen Daten zum Testen ob diese korrekt sind.
            Debug.Log( cman.LoadData( "ExampleText1" ).GetValueAsString() );
            Debug.Log( cman.LoadData( "ExampleText2" ).GetValueAsString() );
            Debug.Log( cman.LoadData( "ExampleText3" ).GetValueAsString() );
            Debug.Log( cman.LoadData( "ExampleText4" ).GetValueAsString() );
            Debug.Log( cman.LoadData( "ExampleText5" ).GetValueAsString() );

            //Data = cman.LoadData( "TestValues" ).GetValuesAsInt( );
            //
            //for ( int i = 1; i < 10; i++ )
            //{
            //    Debug.Log( Data[ i ] );
            //}

            //cman.LoadData( "TestClass", this );

            //Debug.Log( TestString );
            //Debug.Log( TestValue  );

            // Testen ob Daten korrekt gespeichert werden.
            cman.StoreData( "ExampleText1", "Das ist ein sehr langer Text! mit einigen Satzeichen, der beim Testen hilft?", true );
            cman.StoreData( "ExampleText2", 123456789, true );
            cman.StoreData( "ExampleText3", 145346.67, true );
            cman.StoreData( "ExampleText4", '@', true );
            cman.StoreData( "ExampleText5", false, true );
            cman.StoreData( "WorkingDirectory", Environment.CurrentDirectory, true );

            //cman.StoreData( "TestClass", this, true );
        }
    }
}
