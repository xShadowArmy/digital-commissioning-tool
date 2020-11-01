using UnityEngine;
using SystemTools.ManagingRessources;
using System;

namespace UnitTests
{
    internal class ConfigTestClass : ISerialConfigData
    {
        private string TestString { get; set; }
        private double TestValue  { get; set; }
        private int[] Data { get; set; }

        internal ConfigTestClass()
        {
            TestString = "Hallo Welt!";
            TestValue  = 0815;
        }

        public void Restore( SerialConfigData storage )
        {
            TestString = storage.GetValueAsString( );
            TestValue  = storage.GetValueAsDouble( );
            Debug.Log( TestString );
            Debug.Log( TestValue  );
        }

        public void Serialize( SerialConfigData storage )
        {
            storage.AddData( TestString );
            storage.AddData( TestValue  );

            TestString = string.Empty;
            TestValue  = 0;
        }

        public void Test()
        {
            ConfigManager cman = new ConfigManager( );

            cman.OpenConfigFile( "settings.xml", true );

            cman.LoadData( "TestClass", this );

            // Testen ob Daten korrekt gespeichert werden.
            cman.StoreData( "ExampleText1", "Das ist ein sehr langer Text! mit einigen Satzeichen, der beim Testen hilft?", true );
            cman.StoreData( "ExampleText2", 123456789, true );
            cman.StoreData( "ExampleText3", 145346.67, true );
            cman.StoreData( "ExampleText4", '@', true );
            cman.StoreData( "ExampleText5", false, true );
            cman.StoreData( "WorkingDirectory", Environment.CurrentDirectory, true );

            Debug.Log( cman.LoadData( "ExampleText1" ).GetValueAsString() );

            cman.StoreData( "TestClass", this, true );
        }
    }
}
