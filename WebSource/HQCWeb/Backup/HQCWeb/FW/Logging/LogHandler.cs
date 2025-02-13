using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Net.Http.Headers;
using System.Reflection;

namespace HQCWeb.FW.Logging
{
    public class LogHandler
    {
        private ILog logger;
        public LogHandler() { }
        public LogHandler(ILog iLog) {
            logger = iLog;
        }

        public void Info(string strMessage)
        {
            Info(string.Empty, string.Empty, strMessage);
        }
        public void Info(string strStation, string strEquipment, string strMessage)
        {
            logger.Info(string.Format("[Station:{0}][Equipment:{1}][{2}", strStation, strEquipment, strMessage));
        }

        public void Error(string strMessage)
        {
            Error(string.Empty, string.Empty, strMessage);
        }

        public void Error(string strStation, string strEquipment, string strMessage)
        {
            logger.Error(string.Format("[Station:{0}][Equipment:{1}][{2}", strStation, strEquipment, strMessage));
        }

        public void Warn(string strMessage)
        {
            Warn(string.Empty, string.Empty, strMessage);
        }

        public void Warn(string strStation, string strEquipment, string strMessage)
        {
            logger.Warn(string.Format("[Station:{0}][Equipment:{1}][{2}", strStation, strEquipment, strMessage));
        }

        public static log4net.ILog GetLogger2(string programName, string processName)
        {
            //LogManager.ResetConfiguration();
            var hierarchy = (Hierarchy)log4net.LogManager.GetRepository();
            var FileFormat = string.Format(@"{0}\{1}\{2}\{3}\", Environment.CurrentDirectory, "Log", programName, processName);
            var LayoutFormat = string.Format("%date][Program:{0}][Process:{1}][%level][%logger]%message%newline", programName, processName);
            
            var rollingFileAppender = new RollingFileAppender()
            {
                Name = programName + "_" + processName,
                //File = "%property{LogName}",
                File = FileFormat,
                //DatePattern = string.Format(@"yyyy\\\\MM\\\\dd\\\\['{0}'] yyyyMMddHHmm'.log'", programName),
                DatePattern = string.Format(@"yyyyMMdd\\\\yyyyMMddHHmm'.log'", programName),
                StaticLogFileName = false,
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Composite,
                //RollingStyle = RollingFileAppender.RollingMode.Size,
                //RollingStyle = RollingFileAppender.RollingMode.Date,
                MaxSizeRollBackups = -1,
                MaximumFileSize = "1MB",
                Layout = new PatternLayout(LayoutFormat),
            };
            
            var filter = new log4net.Filter.LoggerMatchFilter
            {
                LoggerToMatch = programName + "_" + processName,
                AcceptOnMatch = true
            };
            rollingFileAppender.AddFilter(filter);

            var filterDeny = new log4net.Filter.DenyAllFilter();
            rollingFileAppender.AddFilter(filterDeny);
            
            rollingFileAppender.ActivateOptions();
            hierarchy.Root.AddAppender(rollingFileAppender);
            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
            XmlConfigurator.Configure(hierarchy);

            return log4net.LogManager.GetLogger(rollingFileAppender.Name);
        }

        public static log4net.ILog GetLogger(string programName, string processName)
        {
            //LogManager.ResetConfiguration();
            var hierarchy = (Hierarchy)log4net.LogManager.GetRepository();
            var FileFormat = string.Format(@"{0}\{1}\{2}\{3}\", Environment.CurrentDirectory, "Log", programName, processName);
            var LayoutFormat = string.Format("%date|{0}|{1}|%level|%logger|%message%newline", programName, processName);

            var rollingFileAppender = new RollingMinuteFileAppender()
            {
                Name = programName + "_" + processName,
                Encoding = System.Text.Encoding.UTF8,
                AppendToFile = true,
                RollingStyle = RollingFileAppender.RollingMode.Date,
                DatePattern = string.Format(@"yyyyMMdd\\\\yyyyMMddHHmm'.log'"),
                StaticLogFileName = false,
                Layout = new PatternLayout(LayoutFormat),
                File = FileFormat,
                MaxSizeRollBackups = -1,
            };

            var filter = new log4net.Filter.LoggerMatchFilter
            {
                LoggerToMatch = programName + "_" + processName,
                AcceptOnMatch = true
            };
            rollingFileAppender.AddFilter(filter);

            var filterDeny = new log4net.Filter.DenyAllFilter();
            rollingFileAppender.AddFilter(filterDeny);

            rollingFileAppender.ActivateOptions();
            hierarchy.Root.AddAppender(rollingFileAppender);
            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
            XmlConfigurator.Configure(hierarchy);

            return log4net.LogManager.GetLogger(rollingFileAppender.Name);
        }

    }
}