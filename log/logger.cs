using System;
using System.IO;
using log4net;
using log4net.Config;
using log4net.Repository;
using log4net.Core;
using System.Reflection;
using log4net.Repository.Hierarchy;
using log4net.Appender;

namespace loggerWrapper
{
    //     ALL     DEBUG   INFO    WARN    ERROR   FATAL   OFF
    //
    //    •All                                     
    //    •DEBUG  •DEBUG                           
    //    •INFO   •INFO   •INFO                    
    //    •WARN   •WARN   •WARN   •WARN            
    //    •ERROR  •ERROR  •ERROR  •ERROR  •ERROR      
    //    •FATAL  •FATAL  •FATAL  •FATAL  •FATAL  •FATAL  
    //    •OFF    •OFF    •OFF    •OFF    •OFF    •OFF    •OFF

    //public static class logger
    //{
    //    private static bool isInitialized=false;
    //    private static readonly string logRepositoryName = "LogInfo";
    //    private static ILoggerRepository logRepository;
    //    private static ILog log;
    //    static logger() {
    //        logRepository = LogManager.CreateRepository(logRepositoryName);
    //    }

    //    public static void Initialize(Stream configStream,string loggerName="logData")
    //    {
    //        try
    //        {
    //            //if (!isInitialized)
    //            //{
    //                //if (!String.IsNullOrEmpty(configStream))
    //                if(configStream!=null)
    //                {
    //                    XmlConfigurator.Configure(logRepository,configStream);
    //                    //XmlConfigurator.ConfigureAndWatch(logRepository, new FileInfo(configFilePath));
    //                    log = LogManager.GetLogger(logRepositoryName, loggerName);
    //                   // isInitialized = true;
    //                    //return isInitialized;
    //                }
    //                //else
    //                //{
    //                //    isInitialized = false;
    //                //    return isInitialized;
    //                //}
    //            //}
    //            //else
    //            //{
    //            //    return isInitialized;
    //            //}
    //        }
    //        catch (Exception ex) {
    //            throw new Exception("Error in initialization of logger.", ex);
    //        }

    //    }

    //    public static void logInformation(string logLevel,string logMessage,Exception exception,object loggingProperties) {
    //        try
    //        {
    //            if (log != null)
    //            {


    //                if (!string.IsNullOrEmpty(logLevel))
    //                {
    //                    logLevel = logLevel.ToLower();


    //                        if (loggingProperties != null)
    //                        {
    //                            Type attrType = loggingProperties.GetType();
    //                            PropertyInfo[] properties = attrType.GetProperties(
    //                                           BindingFlags.Public | BindingFlags.Instance);
    //                            for (int i = 0; i < properties.Length; i++)
    //                            {
    //                                object value = properties[i].GetValue(loggingProperties, null);
    //                                if (value != null)
    //                                    ThreadContext.Stacks[properties[i].Name].Push(value.ToString());
    //                            }
    //                        }

    //                    switch (logLevel)
    //                    {

    //                        case "critical":
    //                            log.Fatal(logMessage, exception);
    //                            break;
    //                        case "debug":
    //                        case "trace":
    //                            //object[] obj =  { "dbg", "msg", new DateTime() };

    //                            //log.DebugFormat("today msg is  {0},{1}===>{2}", obj);

    //                            log.Debug(logMessage, exception);
    //                            log.Error(logMessage, exception);

    //                            break;
    //                        case "error":
    //                            log.Error(logMessage, exception);
    //                            break;
    //                        case "information":
    //                            log.Info(logMessage, exception);
    //                            break;
    //                        case "warning":
    //                            log.Warn(logMessage, exception);
    //                            break;
    //                        default:
    //                            log.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
    //                            log.Info(logMessage);
    //                            break;
    //                    }
    //                    if (loggingProperties != null)
    //                    {
    //                        Type attrType = loggingProperties.GetType();
    //                        PropertyInfo[] properties = attrType.GetProperties(
    //                                       BindingFlags.Public | BindingFlags.Instance);
    //                        for (int i = 0; i < properties.Length; i++)
    //                        {
    //                            object value = properties[i].GetValue(loggingProperties, null);
    //                            if (value != null)
    //                                ThreadContext.Stacks[properties[i].Name].Pop();
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex) {
    //            throw new Exception("Error in logging.", ex);
    //        }
    //    }
    //}

    /// <summary>
    /// enum:Define levele of logging through LogLevelWrapper
    /// </summary>
    public enum LogLevelWrapper { ALL, DEBUG, INFO, WARN, ERROR, FATAL, OFF };
    /// <summary>
    /// Interface to implement for Custom logging for log4net library(dll)
    /// </summary>
    public interface ILoggerWrapper
    {
        /// <summary>
        /// log4net required Repository to config and create logger, will not create actual repository.
        /// </summary>
        string LogRepositoryName { get; set; }
        /// <summary>
        /// log4net required Strem of config file to configure logger
        /// </summary>
        Stream configFileStream { get; set; }
        /// <summary>
        /// Initialize the logger with uniq name so you can have multiple logger instance
        /// </summary>
        /// <param name="loggerName"></param>
        void InitializeLogger(string loggerName);
        /// <summary>
        /// log the message according to loglevel set in config file        
        /// </summary>
        /// <param name="logLevel">pass the loglevel of message</param>
        /// <param name="logMessage">"message to be logged"</param>
        /// <param name="messageFormat">"message with format"</param>
        /// <param name="obj">"array of object that to be used by message format"</param>
        void LogInfo(LogLevelWrapper logLevel, string logMessage, string messageFormat = null, params object[] obj);
        /// <summary>
        /// log the message according to loglevel set in config file        
        /// </summary>
        /// <param name="logLevel">pass the loglevel of message</param>
        /// <param name="ex">Exception object need to pass for log</param>
        /// <param name="logMessage">"message to be logged"</param>
        /// <param name="messageFormat">"message with format"</param>
        /// <param name="obj">"array of object that to be used by message format"</param>
        void LogInfo(LogLevelWrapper logLevel, Exception ex, string logMessage, string messageFormat = null, params object[] obj);

    }


    /// <summary>
    /// CustomLogger implement the Actual functionality
    /// </summary>
    public class CustomLogger : ILoggerWrapper
    {
        private string logRepositoryName = "";
        private Stream _configFileStream = null;
        private ILog log;
        private ILoggerRepository loggerRepository;
        /// <summary>
        /// create the repository of name provided or find the repository if alreday created
        /// </summary>
        /// <param name="configFileStream">Stream of config file to configure logger</param>
        /// <param name="logReositoryName">Repository name; Default value is "log"</param>
        public CustomLogger(Stream configFileStream, string logReositoryName = "log")
        {
            //intialize repository name and stream of config file
            this.logRepositoryName = logReositoryName;
            this.configFileStream = configFileStream;
            //verify if stream of config file alreday initilize or not
            if (configFileStream != null)
            {
                
                loggerRepository = null;
                //get all repository 
                var repositories = LogManager.GetAllRepositories();
                //If any is repository present then use first one
                if (repositories.Length > 0)
                {
                    foreach (var repo in repositories)
                    {
                        if (repo.Name == logReositoryName) {
                            loggerRepository = repo;
                            break;
                        }
                    }
                    
                }
                //If no any repository then create new one
                if (loggerRepository == null)
                {
                    loggerRepository = LogManager.CreateRepository(logRepositoryName);                    
                }
            }
        }

        public string LogRepositoryName  // read-write instance property
        {
            get
            {
                return logRepositoryName;
            }
            set
            {
                logRepositoryName = value;
            }
        }

        public Stream configFileStream  // read-write instance property
        {
            get
            {
                return _configFileStream;
            }
            set
            {
                _configFileStream = value;
            }
        }

        /// <summary>
        /// Initialize the logger with config stream provided; If logger is created already then use created logger to log data/info/error.
        /// </summary>
        /// <param name="loggerName"></param>
        public void InitializeLogger(string loggerName="DefaultLogger")
        {   
            try
            {
                //if repository not null
                if (loggerRepository != null)
                {
                    //configure logger with provided repository and config file stream
                    XmlConfigurator.Configure(loggerRepository, configFileStream);
                    //get the log instance and initialize to private log variable to log the data/info/error.
                    log = LogManager.GetLogger(logRepositoryName, loggerName);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error in initialization of logger.", ex);
            }

        }

        public void InitializeLoggerWithDynamicFile(string loggerName, string logFileName) {
            log = LogManager.GetLogger(logRepositoryName, loggerName);
            log4net.GlobalContext.Properties["LogFileName"] = "..\\"+logFileName+"\\" + logFileName;
            XmlConfigurator.Configure(loggerRepository, configFileStream);
            
            
            
        }

        /// <summary>
        /// log the message according to loglevel set in config file        
        /// </summary>
        /// <param name="logLevel">pass the loglevel of message</param>
        /// <param name="logMessage">"message to be logged"</param>
        /// <param name="messageFormat">"message with format"</param>
        /// <param name="obj">"array of object that to be used by message format"</param>
        public void LogInfo(LogLevelWrapper logLevel, string logMessage, string messageFormat = null, params object[] obj)
        {
            if (log != null)
                logger(logLevel, null, logMessage, messageFormat, obj);
        }
        /// <summary>
        /// log the message according to loglevel set in config file        
        /// </summary>
        /// <param name="logLevel">pass the loglevel of message</param>
        /// <param name="ex">Exception object need to pass for log</param>
        /// <param name="logMessage">"message to be logged"</param>
        /// <param name="messageFormat">"message with format"</param>
        /// <param name="obj">"array of object that to be used by message format"</param>
        public void LogInfo(LogLevelWrapper logLevel, Exception ex, string logMessage, string messageFormat = null, params object[] obj)
        {
            if (log != null)
                logger(logLevel, ex, logMessage, messageFormat, obj);
        }
        /// <summary>
        /// private common function to log the message according to loglevel set in config file        
        /// </summary>
        /// <param name="logLevel">pass the loglevel of message</param>
        /// <param name="ex">Exception object need to pass for log</param>
        /// <param name="logMessage">"message to be logged"</param>
        /// <param name="messageFormat">"message with format"</param>
        /// <param name="obj">"array of object that to be used by message format"</param>
        private void logger(LogLevelWrapper logLevel, Exception ex, string logMessage, string messageFormat = null, params object[] obj)
        {
            //if obj is null initilize with empty arry of object
            if (obj == null)
            {
                obj = new object[] { };
            }
            //verify log level
            switch (logLevel)
            {
                case LogLevelWrapper.ALL:
                case LogLevelWrapper.DEBUG:
                    //verify if logger is configured to log the Debug level message
                    if (log.IsDebugEnabled)
                    {
                        //if message format is provided then log both messages
                        if (!string.IsNullOrEmpty(messageFormat))
                        {
                            log.Debug(logMessage, ex);
                            log.DebugFormat(messageFormat, obj);                  
                        }
                        //else log the message only
                        else
                        {
                            log.Debug(logMessage, ex);
                        }
                    }
                    break;
                case LogLevelWrapper.INFO:
                    if (log.IsInfoEnabled)
                    {
                        if (!string.IsNullOrEmpty(messageFormat))
                        {
                            log.Info(logMessage, ex);
                            log.InfoFormat(messageFormat, obj);
                        }
                        else
                        {
                            log.Info(logMessage, ex);
                        }                       
                    }
                    break;
                case LogLevelWrapper.WARN:
                    if (log.IsWarnEnabled)
                    {
                        if (!string.IsNullOrEmpty(messageFormat))
                        {
                            log.Warn(logMessage, ex);
                            log.WarnFormat(messageFormat, obj);
                        }
                        else
                        {
                            log.Warn(logMessage, ex);
                        }
                    }
                    break;
                case LogLevelWrapper.ERROR:

                    if (log.IsErrorEnabled)
                    {
                        if (!string.IsNullOrEmpty(messageFormat))
                        {
                            log.Error(logMessage, ex);
                            log.ErrorFormat(messageFormat, obj);
                        }
                        else
                        {
                            log.Error(logMessage, ex);
                        }                        
                    }

                    break;
                case LogLevelWrapper.FATAL:
                    if (log.IsFatalEnabled)
                    {
                        if (!string.IsNullOrEmpty(messageFormat))
                        {
                            log.Fatal(logMessage, ex);
                            log.FatalFormat(messageFormat, obj);
                        }
                        else
                        {
                            log.Fatal(logMessage, ex);
                        }                      
                    }
                    break;
                default:

                    break;
            }

        }

        public void CloseRepository() {
            try
            {
                LogManager.ShutdownRepository(logRepositoryName);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public void changeConfiguration(string repoName) {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository(repoName);
            var appenders = hierarchy.GetAppenders();
            RollingFileAppender rollingFileAppender = (RollingFileAppender)appenders[0];
            log4net.Layout.PatternLayout patternLayout = new log4net.Layout.PatternLayout("%-5p %c %logger ===> %m%n");

            //((log4net.Layout.PatternLayout)rollingFileAppender.Layout).ConversionPattern = "%-5p %c %logger ===> %m%n";
            rollingFileAppender.Layout = patternLayout;
            rollingFileAppender.ActivateOptions();

            hierarchy.Root.Level = Level.Error;
            hierarchy.Configured = true;
        }
    }
}
