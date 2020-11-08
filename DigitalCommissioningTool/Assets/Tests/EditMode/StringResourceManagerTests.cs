using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SystemTools.ManagingResources;

namespace Tests
{
    public class StringResourceManagerTests
    {
        [Test]
        public void overrides_string_resources()
        {
            string key = "testResource13425346564765";
            StringResourceManager.StoreString(key, "unchanged content");
            StringResourceManager.StoreString(key, "overwritten content", true);

            StringResourceManager.StoreString( "test1", "unchanged content" );
            StringResourceManager.StoreString( "test2", "unchanged content" );
            StringResourceManager.StoreString( "test3", "unchanged content" );
            StringResourceManager.StoreString( "test4", "unchanged content" );
            StringResourceManager.StoreString( "test5", "unchanged content" );
            StringResourceManager.StoreString( "test6", "unchanged content" );
            StringResourceManager.StoreString( "test7", "unchanged content" );
            StringResourceManager.StoreString( "test8", "unchanged content" );
            StringResourceManager.StoreString( "test9", "unchanged content" );

            string stringResource = StringResourceManager.LoadString("@" + key);

            Assert.AreEqual("overwritten content", stringResource);
            


        }
    }
}
