using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SystemTools.ManagingResources;
using UnityEngine;
using UnityEngine.TestTools;
using Object = System.Object;

namespace Tests
{
    public class ConfigManagerTests
    {
        internal class ConfigTestClass : ISerialConfigData
        {
            public int TestValue { get; set; }
            public string TestString { get; set; }

            internal ConfigTestClass()
            {
                TestValue = 23452354;
                TestString = "TestString3425345";
            }

            public void Serialize(SerialConfigData storage)
            {
                storage.AddData(TestValue);
                storage.AddData(TestString);
            }

            public void Restore(SerialConfigData storage)
            {
                TestValue = storage.GetValueAsInt();
                TestString = storage.GetValueAsString();
            }

            public override bool Equals(object obj)
            {
                var item = obj as ConfigTestClass;

                if (item == null)
                {
                    return false;
                }

                return this.TestValue.Equals(item.TestValue) && this.TestString.Equals(item.TestString);
            }

            public override int GetHashCode()
            {
                return this.TestValue.GetHashCode() ^ this.TestString.GetHashCode();
            }
        }

        public ConfigManagerTests()
        {
            TestConfigFilePath = ".\\Output\\Resources\\Data\\TestConfigFile2345234523.xml";
        }

        private string TestConfigFilePath { get; set; }

        [Test]
        public void creates_config_files()
        {
            using (ConfigManager cman = new ConfigManager())
            {
                cman.OpenConfigFile("TestConfigFile2345234523", true);
            }

            Assert.IsTrue(File.Exists(TestConfigFilePath));
            File.Delete(TestConfigFilePath);
        }

        [Test]
        public void stores_config_data()
        {
            string key1 = "configDataKey45634958439543";
            string value = "TestData1242314324";

            string key2 = "configObjectKey3205439058433";
            ConfigTestClass configTestObject = new ConfigTestClass();

            string configData1 = "";
            ConfigTestClass configData2 = new ConfigTestClass();


            using (ConfigManager cman = new ConfigManager())
            {
                cman.OpenConfigFile("TestConfigFile2345234523", true);
                cman.StoreData(key1, value);
                cman.StoreData(key2, configTestObject);

                configData1 = cman.LoadData(key1).GetValueAsString();
                cman.LoadData(key2, configData2);
            }

            Assert.IsTrue(value.Equals(configData1) && configTestObject.Equals(configData2));
            File.Delete(TestConfigFilePath);
        }

        [Test]
        public void overrides_config_data()
        {
            string key1 = "configDataKey45634958439543";
            string origValue = "TestData1242314324";
            string newValue = "NewTestData783474359";

            string key2 = "configObjectKey3205439058433";
            int newTestValue = 87978534;
            string newTestString = "NewTestString8904586456";

            ConfigTestClass origConfigTestObject = new ConfigTestClass();
            ConfigTestClass newConfigTestObject = new ConfigTestClass();
            newConfigTestObject.TestString = newTestString;
            newConfigTestObject.TestValue = newTestValue;

            string configData1 = "";
            ConfigTestClass configData2 = new ConfigTestClass();

            using (ConfigManager cman = new ConfigManager())
            {
                cman.OpenConfigFile("TestConfigFile2345234523", true);
                //create
                cman.StoreData(key1, origValue);
                cman.StoreData(key2, origConfigTestObject);
                //override
                cman.StoreData(key1, newValue);
                cman.StoreData(key2, newConfigTestObject);
                //load
                configData1 = cman.LoadData(key1).GetValueAsString();
                cman.LoadData(key2, configData2);
            }

            Assert.IsTrue(newValue.Equals(configData1) && newConfigTestObject.Equals(configData2));
            File.Delete(TestConfigFilePath);
        }
    }
}