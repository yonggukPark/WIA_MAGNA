using System.Data;

using HQCWeb.FW;
using HQCWeb.FW.Data;
using HQCWeb.FW.Rule;

namespace HQCWeb.Biz.SystemManagement
{
    public class MessageMgt
    {
        DataTable dt = new DataTable();
        DataSet dsList = new DataSet();

        SqlMapper Mapper = null;

        MesRuleBase MRB = new MesRuleBase();

        #region MessageMgt
        public MessageMgt()
        {
            
        }
        #endregion

        #region GetMessageList
        public DataSet GetMessageList(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;

            //jDoc = JsonUtils.MakeBaseMessage("MessageData.Get_MessageList", "0", string.Empty);

            //JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            //JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "MessageData.Get_MessageList");
            //JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

            //jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            //JsonUtils.AddChildTextNode(jParams, "MSG_ID",    pValue[0]);
            //JsonUtils.AddChildTextNode(jParams, "MSG_NM",    pValue[1]);
        }
        #endregion        

        #region GetMessageData
        public DataSet GetMessageData(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;

            //jDoc = JsonUtils.MakeBaseMessage("MessageData.Get_MessageInfo", "0", string.Empty);

            //JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME", strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            //JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID", "MessageData.Get_MessageInfo");
            //JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE", "0");        // 0:조회,1:입력,2:수정,3:삭제

            //jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            //JsonUtils.AddChildTextNode(jParams, "MSG_ID", pValue[0]);
        }
        #endregion

        #region GetMessageIDValChk
        public string GetMessageIDValChk(string strDBName, string strQueryID, Parameters sParam)
        {
            string strRtnCode = string.Empty;

            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            //jDoc = JsonUtils.MakeBaseMessage("MessageData.Get_MessageID_ValChk", "0", string.Empty);

            //JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            //JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "MessageData.Get_MessageID_ValChk");
            //JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

            //jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            //JsonUtils.AddChildTextNode(jParams, "MSG_ID", pValue[0]);
            
            strRtnCode = dt.Rows[0]["VAL_CHK"].ToString();
            
            return strRtnCode;
        }
        #endregion

        #region SetMessageInfo
        public int SetMessageInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;

            //jDoc = JsonUtils.MakeBaseMessage("MessageData.Set_MessageInfo", "0", string.Empty);

            //JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            //JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "MessageData.Set_MessageInfo");
            //JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "1");        // 0:조회,1:입력,2:수정,3:삭제

            //jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            //JsonUtils.AddChildTextNode(jParams, "MSG_ID",    pValue[0]);
            //JsonUtils.AddChildTextNode(jParams, "MSG_TXT_KR", pValue[1]);
            //JsonUtils.AddChildTextNode(jParams, "MSG_TXT_EN", pValue[2]);
            //JsonUtils.AddChildTextNode(jParams, "MSG_TXT_LO", pValue[3]);
            //JsonUtils.AddChildTextNode(jParams, "REG_ID",    pValue[4]);            
        }
        #endregion

        #region DelMessageInfo
        public int DelMessageInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;

            //jDoc = JsonUtils.MakeBaseMessage("MessageData.Set_MessageInfoDel", "0", string.Empty);

            //JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            //JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "MessageData.Set_MessageInfoDel");
            //JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "3");        // 0:조회,1:입력,2:수정,3:삭제

            //jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            //JsonUtils.AddChildTextNode(jParams, "MSG_ID", pValue[0]);            
        }
        #endregion
    }
}