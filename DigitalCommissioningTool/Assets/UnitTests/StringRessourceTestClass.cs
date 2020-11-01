using UnityEngine;
using SystemTools.ManagingRessources;

namespace UnitTests
{
    internal class StringRessourceTestClass
    {
        public void Test()
        {
            StringRessourceManager.StoreString( "OutputText1", "Das ist ein Test111.", true );
            StringRessourceManager.StoreString( "OutputText2", "Das ist ein Test2." );
            StringRessourceManager.StoreString( "OutputText3", "Das ist ein Test3." );
            
            StringRessourceManager.WriteFile( );

            Debug.Log( StringRessourceManager.LoadString( "OutputText1" ) );
            Debug.Log( StringRessourceManager.LoadString( "OutputText2" ) );
            Debug.Log( StringRessourceManager.LoadString( "OutputText3" ) );
            
            StringRessourceManager.StoreString( "OutputText1", "Das ist ein Test1.", true );
            StringRessourceManager.StoreString( "OutputText4", "Das ist ein Test4." );
            StringRessourceManager.StoreString( "OutputText5", "Das ist ein Test5." );

            StringRessourceManager.WriteFile( );
        }
    }
}