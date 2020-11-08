using UnityEngine;
using SystemTools.ManagingResources;

namespace UnitTests
{
    internal class StringRessourceTestClass
    {
        public void Test()
        {
            StringResourceManager.StoreString( "OutputText1", "Das ist ein Test111.", true );
            StringResourceManager.StoreString( "OutputText2", "Das ist ein Test2." );
            StringResourceManager.StoreString( "OutputText3", "Das ist ein Test3." );
            
            Debug.Log( StringResourceManager.LoadString( "OutputText1" ) );
            Debug.Log( StringResourceManager.LoadString( "OutputText2" ) );
            Debug.Log( StringResourceManager.LoadString( "OutputText3" ) );
            
            StringResourceManager.StoreString( "OutputText1", "Das ist ein Test1.", true );
            StringResourceManager.StoreString( "OutputText4", "Das ist ein Test4." );
            StringResourceManager.StoreString( "OutputText5", "Das ist ein Test5." );
        }
    }
}