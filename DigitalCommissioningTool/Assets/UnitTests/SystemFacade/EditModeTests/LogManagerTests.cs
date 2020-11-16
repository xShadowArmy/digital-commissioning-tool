using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SystemFacade;
using System.IO;
using System.Linq;
using SystemTools;

namespace UnitTests.SystemFacade
{
    public class LogManagerTests
    {
        [Test]
        public void writes_log_message()
        {
            string testString = "ABCDEFG123456789";

            bool infoLogMessageFound = false;
            bool errorLogMessageFound = false;

            LogManager.WriteLog(testString, LogLevel.Info, false, "LogManagerTests", "writes_log_message");
            LogManager.WriteError(testString, "LogManagerTests", "writes_log_message");

            DirectoryInfo logDirectory = new DirectoryInfo(Paths.LogPath);
            FileInfo logFile = logDirectory.GetFiles().OrderByDescending(f => f.LastWriteTime).First();

            using (StreamReader sr = logFile.OpenText())
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    if (s.Equals("[INFO] [LogManagerTests][writes_log_message] " + testString))
                    {
                        infoLogMessageFound = true;
                    }
                    else if (s.Equals("[ERROR] [LogManagerTests][writes_log_message] " + testString))
                    {
                        errorLogMessageFound = true;
                    }
                }
            }

            Assert.IsTrue(infoLogMessageFound && errorLogMessageFound);
        }
    }
}