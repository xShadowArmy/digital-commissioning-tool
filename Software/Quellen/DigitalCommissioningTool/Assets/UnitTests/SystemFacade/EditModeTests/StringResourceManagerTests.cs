using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SystemFacade;
using System.Xml;
using System.Linq;

namespace UnitTests.SystemFacade
{
    public class StringResourceManagerTests
    {
        [Test]
        public void overrides_string_resources()
        {
            StringResourceManager.SelectLanguage( SystemLanguage.German );

            string key = "testResource13425346564765";
            StringResourceManager.StoreString(key, "unchanged content");
            StringResourceManager.StoreString(key, "overwritten content", true);

            StringResourceManager.StoreString("test1", "unchanged content");
            StringResourceManager.StoreString("test2", "unchanged content");
            StringResourceManager.StoreString("test3", "unchanged content");
            StringResourceManager.StoreString("test4", "unchanged content");
            StringResourceManager.StoreString("test5", "unchanged content");
            StringResourceManager.StoreString("test6", "unchanged content");
            StringResourceManager.StoreString("test7", "unchanged content");
            StringResourceManager.StoreString("test8", "unchanged content");
            StringResourceManager.StoreString("test9", "unchanged content");

            string stringResource = StringResourceManager.LoadString("@" + key);

            Assert.AreEqual("overwritten content", stringResource);
        }

        [Test]
        public void sets_unique_german_string_resource_ids()
        {
            bool isUnique = true;
            HashSet<string> set = new HashSet<string>();

            string germanStringResourcesPath = Paths.StringResourcePath + "German.xml";
            XmlDocument stringResourcesDocument = new XmlDocument();
            stringResourcesDocument.Load(germanStringResourcesPath);

            foreach (XmlNode node in stringResourcesDocument.ChildNodes)
            {
                foreach (XmlNode innerNode in node.ChildNodes)
                {
                    if (!set.Add(innerNode.Attributes["xs:id"]?.InnerText))
                    {
                        isUnique = false;
                    }
                }
            }

            Assert.IsTrue(isUnique);
        }

        [Test]
        public void sets_unique_english_string_resource_ids()
        {
            bool isUnique = true;
            HashSet<string> set = new HashSet<string>();

            string englishStringResourcesPath = Paths.StringResourcePath + "English.xml";
            XmlDocument stringResourcesDocument = new XmlDocument();
            stringResourcesDocument.Load(englishStringResourcesPath);

            foreach (XmlNode node in stringResourcesDocument.ChildNodes)
            {
                foreach (XmlNode innerNode in node.ChildNodes)
                {
                    if (!set.Add(innerNode.Attributes["xs:id"]?.InnerText))
                    {
                        isUnique = false;
                    }
                }
            }

            Assert.IsTrue(isUnique);
        }

        [Test]
        public void stores_and_loads_string_resources()
        {
            string key = "testResource5462345634573567";
            string value = "testStringResource";
            StringResourceManager.StoreString(key, value);
            string stringResource = StringResourceManager.LoadString(key);
            Assert.AreEqual(value, stringResource);
        }
    }
}