using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Appender;
using log4net.Core;

namespace HQCWeb.FW.Logging
{
    public class RollingMinuteFileAppender : RollingFileAppender
    {
        #region 변수
        private int _maxMinute = 10;
        private DateTime _minuteDate;
        #endregion

        #region 생성자
        public RollingMinuteFileAppender()
        {
            CalcNextDate();
        }
        #endregion

        #region 10분 주기 파일 생성
        private void CalcNextDate()
        {
            _minuteDate = Convert.ToDateTime(DateTime.Now.AddMinutes(_maxMinute).ToString("yyyy-MM-dd HH:mm:00"));
        }

        protected override void AdjustFileBeforeAppend()
        {
            DateTime dtNow = DateTime.Now;

            if (dtNow >= _minuteDate)
            {
                CalcNextDate();

                base.AdjustFileBeforeAppend();
            }
        }
        #endregion

        #region Modify Custrom Log Format
        protected override void Append(LoggingEvent loggingEvent)
        {
            var val = ConvertMessage(loggingEvent);
            base.Append(val);
        }

        protected override void Append(LoggingEvent[] loggingEvents)
        {
            var vals = loggingEvents.Select(ConvertMessage).ToArray();
            base.Append(vals);
        }

        private LoggingEvent ConvertMessage(LoggingEvent loggingEvent)
        {
            var eventData = loggingEvent.GetLoggingEventData();
            eventData.Message = ModifyCustomLogFormat(eventData.Message);
            var val = new LoggingEvent(eventData);
            return val;
        }

        private string ModifyCustomLogFormat(string val)
        {
            string result = string.Empty;

            if (val != null)
            {
                result = val.Replace("\r\n", "    ").Replace("\r", "  ").Replace("\n", "  ");
            }

            return result;
        }
        #endregion
    }
}
