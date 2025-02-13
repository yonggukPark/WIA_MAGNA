using HQC.FW.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HQCWeb.FW
{
    public class MessageMgt
    {

        public MessageType m_sendMessageType = MessageType.JSON;

        #region SendTibcoMessage_FMB
        public bool SendTibcoMessage_FMB(string targetSubjectName, string message, ref string rtnMessage, bool isRequest)
        {
            bool bResult = false;
            try
            {
                //foreach (TibModule tibModule in TibcoService.m_tibcoSendServices_FMB)
                //{
                //    if (tibModule.ServiceType == TibcoServiceType.Send)
                //    {
                //        if (string.IsNullOrEmpty(targetSubjectName))
                //        {
                //            targetSubjectName = tibModule.ConnectionInfo.Subject;
                //            if (isRequest)
                //            {
                //                rtnMessage = tibModule.SendRequest(tibModule.ConnectionInfo.Subject, message, m_sendMessageType);
                //            }
                //            else
                //            {
                //                tibModule.Send(tibModule.ConnectionInfo.Subject, message, m_sendMessageType);
                //            }
                //        }
                //        else
                //        {
                //            if (isRequest)
                //            {
                //                rtnMessage = tibModule.SendRequest(targetSubjectName, message, m_sendMessageType);
                //            }
                //            else
                //            {
                //                tibModule.Send(targetSubjectName, message, m_sendMessageType);
                //            }
                //        }
                //        bResult = true;
                //    }
                //}
                if (!bResult) rtnMessage = string.Format("Tibco Send Subject Not found{Subject:{0}", targetSubjectName);
                return bResult;
            }
            catch (Exception ex)
            {
                rtnMessage = string.Format("Tibco Service Send Error[Subject:{0}] : {1}", targetSubjectName, ex.Message);
                return false;
            }
        }
        #endregion

        #region SendTibcoMessage_WEB
        public bool SendTibcoMessage_WEB(string targetSubjectName, string message, ref string rtnMessage, bool isRequest)
        {
            bool bResult = false;
            try
            {
                //foreach (TibModule tibModule in TibcoService.m_tibcoSendServices_WEB)
                //{
                //    if (tibModule.ServiceType == TibcoServiceType.Send)
                //    {
                //        if (string.IsNullOrEmpty(targetSubjectName))
                //        {
                //            targetSubjectName = tibModule.ConnectionInfo.Subject;
                //            if (isRequest)
                //            {
                //                rtnMessage = tibModule.SendRequest(tibModule.ConnectionInfo.Subject, message, m_sendMessageType);
                //            }
                //            else
                //            {
                //                tibModule.Send(tibModule.ConnectionInfo.Subject, message, m_sendMessageType);
                //            }
                //        }
                //        else
                //        {
                //            if (isRequest)
                //            {
                //                rtnMessage = tibModule.SendRequest(targetSubjectName, message, m_sendMessageType);
                //            }
                //            else
                //            {
                //                tibModule.Send(targetSubjectName, message, m_sendMessageType);
                //            }
                //        }
                //        bResult = true;
                //    }
                //}
                if (!bResult) rtnMessage = string.Format("Tibco Send Subject Not found{Subject:{0}", targetSubjectName);
                return bResult;
            }
            catch (Exception ex)
            {
                rtnMessage = string.Format("Tibco Service Send Error[Subject:{0}] : {1}", targetSubjectName, ex.Message);
                return false;
            }
        }
        #endregion

    }
}