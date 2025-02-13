using HQC.FW.Common;
using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

using HQCWeb.FW.Data;
using HQCWeb.FW.Rule;

using System.Collections.Generic;


namespace HQCWeb.Biz.SystemManagement
{
    public class UserMgt
    {
        DataTable dt = new DataTable();
        DataSet dsList = new DataSet();
        BasePage bp = new BasePage();

        SqlMapper Mapper = null;

        MesRuleBase MRB = new MesRuleBase();

        public string strPlant = string.Empty;
        
        #region UserMgt
        public UserMgt()
        {
            strPlant = "GPDB";
        }
        #endregion

        #region GetUserInfoList
        public DataSet GetUserInfoList(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion
        
        #region GetUserInfo
        public DataSet GetUserInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion

        #region GetUserIDValChk
        public string GetUserIDValChk(string strDBName, string strQueryID, Parameters sParam)
        {
            string strRtnCode = string.Empty;

            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            strRtnCode = dt.Rows[0]["VAL_CHK"].ToString();

            return strRtnCode;
        }
        #endregion

        #region SetUserInfo
        public int SetUserInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion

        #region DelUserInfo
        public int DelUserInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion

        #region SetUserPwdUpdate
        public string SetUserPwdUpdate(string[] pValue)
        {
            //jDoc = JsonUtils.MakeBaseMessage("UserInfoData.Set_UserPwdUpdate", "0", string.Empty);

            string strRtnCode = string.Empty;
            //DataSet dsList = new DataSet();
            //DataTable dt = new DataTable();

            //JObject jDoc;
            //JObject jParams;

            //string receiveResult = string.Empty;
            //string receiveFlag = string.Empty;

            //JObject jReceiveResult;
            //JToken jReturnTable;
            //DataTable dtReturnTable;
            //string strRtnMsg = string.Empty;

            //jDoc = JsonUtils.MakeBaseMessage("UserInfoData.Set_UserPwdUpdate", "0", string.Empty);

            //JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            //JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "UserInfoData.Set_UserPwdUpdate");
            //JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "3");        // 0:조회,1:입력,2:수정,3:삭제

            //jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            //JsonUtils.AddChildTextNode(jParams, "userid",   pValue[0]);
            //JsonUtils.AddChildTextNode(jParams, "userpwd",  pValue[1]);

            //receiveResult = string.Empty;
            //TibcoService.SendTibcoMessage_WEB(strTargetName, jDoc.ToString(), ref receiveResult, true);  // 호출 대상 서비스이름

            //jReceiveResult = (JObject)JsonConvert.DeserializeObject(receiveResult);
            //jReturnTable = JsonUtils.GetNode(jReceiveResult, "//message/return/returnTable");
            //dtReturnTable = JsonUtils.ConvertJsonToDataTable(jReturnTable);
            //strRtnCode = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returncode");
            //strRtnMsg = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returnmessage");

            //if (strRtnCode == "")
            //{
            //    TibcoService.TIBCO_Open();
            //    TibcoService.WEB_Conn();

            //    strRtnCode = "Tibco Service Error";
            //}

            return strRtnCode;
        }
        #endregion

        #region GetLoginData
        public DataSet GetLoginData(string strDBName, string strQueryID, Parameters sParam)
        {

            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion

        #region SetPwdChange
        public int SetPwdChange(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion

        #region GetUserMenuSettingInfoList
        public DataSet GetUserMenuSettingInfoList(string strDBName, string strQueryID, Parameters sParam)
        {
            dsList = new DataSet();

            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion

        #region SetUserMenuSettingInfoList
        public int SetUserMenuSettingInfoList(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion

        #region SetUserLoginDTUpdate
        public int SetUserLoginDTUpdate(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion

        #region SetUserLoginFaultCntUpdate
        public int SetUserLoginFaultCntUpdate(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion


        #region SetUserLoginFaultInfoUpdate
        public int SetUserLoginFaultInfoUpdate(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion

        #region SetUserTempPWDUpdate
        public int SetUserTempPWDUpdate(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion



        #region SendTempPWD
        public string SendTempPWD(string strNumber, string strTempPWD)
        {
            string strRtn = string.Empty;

            // 한번 요청으로 1만건의 알림톡 발송이 가능합니다.
            // 등록되어 있는 템플릿의 변수 부분을 제외한 나머지 부분(상수)은 100% 일치해야 합니다.
            // 템플릿 내용이 "#{이름}님 가입을 환영합니다."으로 등록되어 있는 경우 변수 #{이름}을 홍길동으로 치환하여 "홍길동님 가입을 환영합니다."로 입력해 주세요.
            // 버튼은 5개까지 입력 가능합니다.

            MessagingLib.Messages messages = new MessagingLib.Messages();

            //string strTest = "AKJDSFKJASKDJF";

            // #{변수} 에 값을 입력합니다. (템플릿의 모든 변수값을 입력해야 오류 발생하지 않습니다.)
            //Dictionary<string, string> variables = new Dictionary<string, string> { { "#{otpnumber}", strTempPWD } };
            Dictionary<string, string> variables = new Dictionary<string, string> { { "#{temppassword}", strTempPWD } };

            // variables에 변수값을 입력하여 요청하면 알림톡의 내용은 서버쪽에서 자동으로 채워져 발송됩니다.
            messages.Add(new MessagingLib.Message()
            {
                to = strNumber,
                from = "029302266",
                kakaoOptions = new MessagingLib.KakaoOptions()
                {
                    pfId = "KA01PF240219011728428NZDqHClvCQz",
                    templateId = "KA01TP240401075156127HI3UxhIE7ip", // 템플릿 등록 후 발급받은 값을 사용해 주세요
                    //templateId = "KA01TP240219012410044BeWX0MQklXu", // 템플릿 등록 후 발급받은 값을 사용해 주세요
                    variables = variables
                }
            });

            MessagingLib.Response response = MessagingLib.SendMessages(messages);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                strRtn = "OK";
            }
            else
            {
                strRtn = response.ErrorMessage.ToString();
            }

            return strRtn;
        }
        #endregion
    }
}