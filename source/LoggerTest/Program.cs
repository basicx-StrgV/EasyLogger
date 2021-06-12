﻿using System;
using System.Collections.Generic;
using System.IO;
using BasicxLogger;
using BasicxLogger.Databases;

namespace LoggerTest
{
#pragma warning disable
    class Program
    {
        //-Logger-------------------------------------------------------------

        //File Logger TXT file
        private FileLogger _txtFileLogger;
        //File Logger LOG file
        private FileLogger _logFileLogger;
        //File Logger xml file
        private FileLogger _xmlFileLogger;
        //File Logger JSON file
        private FileLogger _jsonFileLogger;

        //Json Logger
        private JsonLogger<JsonLoggerTestModel> _jsonLogger;

        //MySql Logger
        private MySqlLogger _mySqlLogger;

        //Multi Logger
        private MultiLogger _multiLogger;

        //--------------------------------------------------------------------

        static void Main(string[] args)
        {
            new Program();
        }
        Program()
        {
            Run();
        }

        private void Run()
        {
            if (Directory.Exists(DirConfig.TestOutputDir))
            {
                Directory.CreateDirectory(DirConfig.TestOutputDir);
            }

            DefaultTest();

            Console.WriteLine("\n--------------------------------------------\n");

            CustomTest();
        }

        private void CustomTest()
        {
            List<string> test = new List<string>();
            test.Add("test");
            test.Add("test");
            test.Add("test");
            test.Add("test");
            test.Add("test");
            test.Add("test");
            //---Test-code-for-any-sort-of-tests-goes-here---
            FileLogger fileLogger = new FileLogger(
                        new XmlLogFile(String.Format("{0}/{1}/", DirConfig.TestOutputDir, "CustomTest"), "Log"));
            fileLogger.Log("Test");
            fileLogger.UseId = false;
            fileLogger.Log("Test");
            fileLogger.UseId = true;

            JsonLogger<testJson> jsonLogger = new JsonLogger<testJson>(
                String.Format("{0}/{1}/", DirConfig.TestOutputDir, "CustomTest"), "JsonLoggerTest.json");
            jsonLogger.Log(new testJson() { test = "Test"});

            FileLogger fileLogger2 = new FileLogger(
                        new TxtLogFile(String.Format("{0}/{1}/", DirConfig.TestOutputDir, "CustomTest"), "Log"));
            MySqlLogger mySqlLogger = new MySqlLogger(new MySqlDatabase("", "", "", ""), "");
            MultiLogger multiLogger = new MultiLogger();
            multiLogger.Add(fileLogger);
            multiLogger.Add(fileLogger2);
            foreach (ILogger i in multiLogger)
            {
                i.Log("123321");
            }
            string[] ids = multiLogger.Log("Multilogger Entry");
        }

        private void DefaultTest()
        {
            InitalizeLogger();

            MultiLoggerSetup();

            //-Test all logger ----------------------------------------------

            Console.Write("\nFileLogger(.txt): ");
            OutputTestStatus(new FileLoggerTester(_txtFileLogger).Test());

            Console.Write("\nFileLogger(.log): ");
            OutputTestStatus(new FileLoggerTester(_logFileLogger).Test());

            Console.Write("\nFileLogger(.json): ");
            OutputTestStatus(new FileLoggerTester(_jsonFileLogger).Test());

            //---------------------------------------------------------------
        }

        private void InitalizeLogger()
        {
            //File Logger TXT file
            _txtFileLogger = new FileLogger(
                        new TxtLogFile(String.Format("{0}/{1}/", DirConfig.TestOutputDir, "FileLoggerTestOutput"), "Log"));
            //File Logger LOG file
            _logFileLogger = new FileLogger(
                        new LogLogFile(String.Format("{0}/{1}/", DirConfig.TestOutputDir, "FileLoggerTestOutput"), "Log"));
            //File Logger xml file
            _xmlFileLogger = new FileLogger(
                        new XmlLogFile(String.Format("{0}/{1}/", DirConfig.TestOutputDir, "FileLoggerTestOutput"), "Log"));
            //File Logger JSON file
            _jsonFileLogger = new FileLogger(
                        new JsonLogFile(String.Format("{0}/{1}/", DirConfig.TestOutputDir, "FileLoggerTestOutput"), "Log"));

            //Json Logger
            _jsonLogger = new JsonLogger<JsonLoggerTestModel>(
                        String.Format("{0}/{1}/", DirConfig.TestOutputDir, "JsonLoggerTestOutput"), "JsonLoggerTest.json");


            //MySql Logger
            try
            {
                _mySqlLogger = new MySqlLogger(new MySqlDatabase("localhost", "test", "root", "admin"), "logTest");
            }
            catch (Exception)
            {
                _multiLogger = null;
            }

            //Multi Logger
            _multiLogger = new MultiLogger();
        }

        private void MultiLoggerSetup()
        {
            _multiLogger.Add(_txtFileLogger);
            _multiLogger.Add(_logFileLogger);
            _multiLogger.Add(_xmlFileLogger);
            _multiLogger.Add(_jsonFileLogger);
        }

        private void OutputTestStatus(bool testSuccess)
        {
            if (testSuccess)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("PASS\n");
                Console.ResetColor();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("FAIL\n");
                Console.ResetColor();
            }
        }
    }

    class testJson
    {
        public string test { get; set; }
    }
}
