//using log4net;
//using log4net.Appender;
//using log4net.Config;
//using log4net.Core;
//using log4net.Layout;
//using log4net.Repository.Hierarchy;
using System;
using System.IO;
using System.Reflection;
using log4net;
using loggerWrapper;

namespace VerifyLogging
{
    class Program
    {

        static void Main(string[] args)

        {

            Stream st = File.OpenRead(@"C:\Users\tarun.tank\source\repos\log\VerifyLogging\log4net.config");
            CustomLogger logger = new CustomLogger(st, "MYCustomLoggerRepo");
            //logger.InitializeLogger("ClassLibrary1");

            logger.InitializeLoggerWithDynamicFile("MYCustomLogger", "MYCustomLogFile");
            logger.LogInfo(LogLevelWrapper.DEBUG, "MY log", "DEBUG 1 {0}--->{1}", new object[] { "firstArg", "SecondArg" });
            logger.changeConfiguration("MYCustomLoggerRepo");
            logger.LogInfo(LogLevelWrapper.ERROR, "MY log", "DEBU 2 {0}--->{1}", new object[] { "firstArg", "SecondArg" });

            //st = File.OpenRead(@"C:\Users\tarun.tank\source\repos\log\VerifyLogging\log4net.config");
            //CustomLogger loggerOther = new CustomLogger(st, "OtherCustomLoggerRepo");
            //loggerOther.InitializeLoggerWithDynamicFile("OtherCustomLogger", "OtherCustomLoggerFile");
            //loggerOther.LogInfo(LogLevelWrapper.ERROR, "Other log", " {0}--->{1}", new object[] { "firstArg", "SecondArg" });


            //logger.CloseRepository();
            //loggerOther.CloseRepository();


            //st = File.OpenRead(@"C:\Users\tarun.tank\source\repos\log\VerifyLogging\log4net.config");
            //CustomLogger loggerSeparate = new CustomLogger(st, "SeparateCustomLoggerRepo");
            //loggerSeparate.InitializeLoggerWithDynamicFile("SeparateCustomLogger", "SeparateCustomLoggerFile");
            //loggerSeparate.LogInfo(LogLevelWrapper.ERROR, "Separate log", " {0}--->{1}", new object[] { "firstArg", "SecondArg" });


            //logger.LogInfo(LogLevelWrapper.WARN, "MY log WARN log without format");
            //loggerOther.LogInfo(LogLevelWrapper.WARN, "Other log WARN log without format");
            //loggerSeparate.LogInfo(LogLevelWrapper.WARN, "Separate log WARN log without format");


            //logger.LogInfo(LogLevelWrapper.ERROR, "ERROR log format msg WITHOUT EX", " {0}--->{1}", new object[] { "firstArg", "SecondArg" });
            //logger.LogInfo(LogLevelWrapper.ERROR, "ERROR log without format msg WITHOUT EX");
            //logger.LogInfo(LogLevelWrapper.ERROR, new NullReferenceException(), "ERROR log format msg", " {0}--->{1}", new object[] { "firstArg", "SecondArg" });
            //logger.LogInfo(LogLevelWrapper.ERROR, new NullReferenceException(), "ERROR log without format msg");
            Console.WriteLine();
        }
        #region commented code
        //logger.Initialize(st,"log1");
        //logger.logInformation("debug", "debug issue", new ArgumentOutOfRangeException(), new { User = "Farhan", Environment = "Production" });
        //logger.logInformation("error", "error issue", null, new { User = "Tarun", Environment = "Dev" });
        //logger.logInformation("information", "error information", new Exception(), new { User = "Nirav", Environment = "QA" });


        //st = File.OpenRead(@"C:\Users\tarun.tank\source\repos\log\VerifyLogging\log4net1.config");

        //logger.Initialize(st, "log1");
        //logger.logInformation("debug", "debug issue", new ArgumentOutOfRangeException(), new { User = "Farhan", Environment = "Production" });
        //logger.logInformation("error", "error issue", null, new { User = "Tarun", Environment = "Dev" });
        //logger.logInformation("information", "error information", new Exception(), new { User = "Nirav", Environment = "QA" });

        //Console.ReadLine();

        //var logRepository =LogManager.CreateRepository("LogFolderOther");
        ////Hierarchy hierarchy = (Hierarchy)logRepository;
        ////hierarchy.Root.Additivity = false;

        //////Add appenders you need: here I need a rolling file and a memoryappender
        ////var rollingAppender = GetRollingAppender("LogFolderOther");
        ////hierarchy.Root.AddAppender(rollingAppender);
        ////BasicConfigurator.Configure(logRepository);

        ////var logRepository = LogManager.GetRepository(@"netcoreapp2.1");
        //XmlConfigurator.ConfigureAndWatch(logRepository,new FileInfo(@"C:\Users\tarun.tank\source\repos\log\VerifyLogging\log4net.config") );
        //ILog log = LogManager.GetLogger("LogFolderOther", "p");
        //log.Debug("Starting up");            
        //log.Warn("Starting up",new ArgumentException("arg not present"));
        ////log.Debug("Shutting down");
        //private static IAppender GetRollingAppender(string arg)
        //{
        //    var level = Level.All;

        //    var rollingFileAppenderLayout = new PatternLayout("%date{HH:mm:ss,fff}|T%2thread|%25.25logger|%5.5level| %message%newline");
        //    rollingFileAppenderLayout.ActivateOptions();

        //    var rollingFileAppenderName = string.Format("{0}{1}", "Rolling", arg);

        //    var rollingFileAppender = new RollingFileAppender();
        //    rollingFileAppender.Name = rollingFileAppenderName;
        //    rollingFileAppender.Threshold = level;
        //    rollingFileAppender.CountDirection = 0;
        //    rollingFileAppender.AppendToFile = true;
        //    rollingFileAppender.LockingModel = new FileAppender.MinimalLock();
        //    rollingFileAppender.StaticLogFileName = true;
        //    rollingFileAppender.RollingStyle = RollingFileAppender.RollingMode.Date;
        //    rollingFileAppender.DatePattern = ".yyyy-MM-dd'.log'";
        //    rollingFileAppender.Layout = rollingFileAppenderLayout;
        //    rollingFileAppender.File = string.Format("{0}.{1}", "log", arg);
        //    rollingFileAppender.ActivateOptions();

        //    return rollingFileAppender;
        //}
        #endregion
    }




}
