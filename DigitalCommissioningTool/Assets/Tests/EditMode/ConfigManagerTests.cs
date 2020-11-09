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

        private string TestConfigFileName { get; set; }
        private string TestConfigFilePath { get; set; }

        public ConfigManagerTests()
        {
            TestConfigFileName = "TestConfigFile2345234523.xml";
            TestConfigFilePath = ".\\Output\\Resources\\Data\\TestConfigFile2345234523.xml";
        }


        [Test]
        public void creates_config_files()
        {
            using (ConfigManager cman = new ConfigManager())
            {
                cman.OpenConfigFile(TestConfigFileName, true);
            }

            Assert.IsTrue(File.Exists(TestConfigFilePath));
            File.Delete(TestConfigFilePath);
        }

        [Test]
        public void stores_and_loads_config_data()
        {
            string key1 = "configDataKey45634958439543";
            string value = "TestData1242314324";

            string key2 = "configObjectKey3205439058433";
            ConfigTestClass configTestObject = new ConfigTestClass();

            string configData1 = "";
            ConfigTestClass configData2 = new ConfigTestClass();


            using (ConfigManager cman = new ConfigManager())
            {
                cman.OpenConfigFile(TestConfigFileName, true);
                cman.StoreData(key1, value);
                cman.StoreData(key2, configTestObject);

                configData1 = cman.LoadData(key1).GetValueAsString();
                cman.LoadData(key2, configData2);
            }

            Assert.IsTrue(value.Equals(configData1) && configTestObject.Equals(configData2));
            File.Delete(TestConfigFilePath);
        }

        [Test]
        public void overrides_and_loads_config_data()
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
                cman.OpenConfigFile(TestConfigFileName, true);
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

        [Test]
        public void deletes_config_data()
        {
            string key1 = "configDataKey45634958439543";
            string value = "TestData1242314324";

            string key2 = "configObjectKey3205439058433";
            ConfigTestClass configTestObject = new ConfigTestClass();

            ConfigData configData1 = null;
            ConfigData configData2 = null;

            using (ConfigManager cman = new ConfigManager())
            {
                //create
                cman.OpenConfigFile(TestConfigFileName, true);
                cman.StoreData(key1, value);
                cman.StoreData(key2, configTestObject);
                //remove
                cman.RemoveData(key1);
                cman.RemoveData(key2);
                //load
                configData1 = cman.LoadData(key1);
                configData2 = cman.LoadData(key2);
            }

            Assert.IsTrue(configData1 is null && configData2 is null);
        }
    }
}