using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SystemTools.ManagingRessources;

namespace Tests
{
    public class StringResourceManagerTests
    {
        [Test]
        public void overrides_string_resources()
        {
            string key = "testResource13425346564765";
            StringRessourceManager.StoreString(key, "unchanged content");
            StringRessourceManager.StoreString(key, "overwritten content", true);
            
            string stringResource = StringRessourceManager.LoadString("@" + key);

            Assert.AreEqual("overwritten content", stringResource);
            


        }
    }
}
