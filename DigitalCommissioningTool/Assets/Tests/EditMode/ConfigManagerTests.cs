using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using SystemTools.ManagingResources;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ConfigManagerTests
    {
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

    }
}