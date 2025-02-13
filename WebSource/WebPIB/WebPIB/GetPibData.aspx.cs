using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Script.Serialization;

namespace WebPIB
{
    public partial class GetPibData : PageBase
    {
        protected string strWorkType = string.Empty;
        protected string strClientAddr = string.Empty;
        protected string strMid = string.Empty;
        protected string strPid = string.Empty;
        protected string strVM = string.Empty;
        protected string strShop1 = string.Empty;
        protected string strLine1 = string.Empty;
        protected string strShop2 = string.Empty;
        protected string strLine2 = string.Empty;

        #region 기준가동률
        //private double iStandard_YF1 = 80;
        //private double iStandard_YF2 = 80;
        //private double iStandard_LF = 80;
        //private double iStandard_JF = 80;
        //private double iStandard_PHEV = 80;
        //private double iStandard_12V = 80;
        //private double iStandard_AEDE1 = 80;
        //private double iStandard_AEDE2 = 80;
        //private double iStandard_AEEV = 80;
        //private double iStandard_AEEV1 = 80;
        //private double iStandard_WG = 80;

        private Hashtable htiStandard = new Hashtable();
        private double iStandardLine1 = 80;
        private double iStandardLine2 = 80;
        #endregion

        #region 전역변수 - 소규모 시스템이어서 전역변수를 구성해서 데이터,상태정보 유지
        public static Hashtable s_htGetDeliveryData = new Hashtable();        // 출하 현황판데이터 조회시간
        public static Hashtable s_dsGetDeliveryData = new Hashtable();          // 출하 현황판데이터

        public static Hashtable s_htGetStockData = new Hashtable();       // 재고 현황판데이터 조회시간
        public static Hashtable s_dsGetStockData = new Hashtable();         // 재고 현황판데이터

        public static DateTime s_dtGetProdData = DateTime.Now.AddSeconds(-10);  // 사무실 현황판데이터 조회시간
        public static string s_strGetProdData = string.Empty;                   // 사무실 현황판데이터

        public static DateTime s_dtGetShipData = DateTime.Now.AddSeconds(-10);  // 출하 모니터링 데이터 조회시간
        public static string s_strGetShipData = string.Empty;                   // 출하 모니터링 현황판데이터

        public static DateTime s_dtGetProdDataT = DateTime.Now.AddSeconds(-10); // 사무실 현황판데이터 조회시간
        public static string s_strGetProdDataT = string.Empty;          // 사무실 현황판데이터

        public static DateTime s_dtGetStationMonitorData = DateTime.Now.AddSeconds(-10);  // 공정 모니터링 데이터 조회시간
        public static string s_strGetStationMonitorData = string.Empty;                   // 공정 모니터링 현황판데이터

        public static DateTime s_dtGetStationMonitorData2 = DateTime.Now.AddSeconds(-10);  // 공정 모니터링 데이터 조회시간
        public static string s_strGetStationMonitorData2 = string.Empty;                   // 공정 모니터링 현황판데이터

        public static DateTime s_dtGetStationMonitorData3 = DateTime.Now.AddSeconds(-10);  // 공정 모니터링 데이터 조회시간
        public static string s_strGetStationMonitorData3 = string.Empty;                   // 공정 모니터링 현황판데이터

        public static Hashtable s_htGetLineProdData = new Hashtable();      // 라인 현황판데이터 조회시간
        public static Hashtable s_dsGetLineProdData = new Hashtable();        // 라인 현황판데이터

        public static Hashtable s_htGetAlarmData = new Hashtable();       // 알람데이터 조회시간
        public static Hashtable s_dsGetAlarmData = new Hashtable();         // 알람데이터

        // @@ KDK add S
        public static bool s_bBeforeStock = false;                          // 전일 마감 Data 생성 여부
        public static DateTime s_dtGetDailyClosingData = DateTime.Now.AddSeconds(-10);  // 일마감 현황판데이터 조회시간
        public static string s_strGetDailyClosingData = string.Empty;                       // 일마감 현황판 데이터

        public static DateTime s_dtGetDailyTotData = DateTime.Now.AddSeconds(-10);      // 실시간 지표종합 현황판 데이터 조회시간
        public static string s_strGetDailyTotData = string.Empty;                       // 실시간 지표종합 현황판 데이터
        
        public static string s_strGetSubContPIBData = string.Empty;                       // 신규 현황판 데이터
                                                                                            // @@ KDK add E


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                strWorkType = Request.Params.Get("mode");
                strMid = Request.Params.Get("mid");
                strPid = Request.Params.Get("pid");
                strVM = Request.Params.Get("vm");
                strShop1 = Request.Params.Get("shop1");
                strLine1 = Request.Params.Get("line1");
                strShop2 = Request.Params.Get("shop2");
                strLine2 = Request.Params.Get("line2");
                strClientAddr = Request.UserHostAddress;


                if (strWorkType != null && strWorkType.Length > 0)
                {
                    // 사무실현황판
                    if (strWorkType == "board")
                    {
                        GetMonitorInfo(strMid);
                    }
                    // 사무실현황판
                    else if (strWorkType == "prod")
                    {
                        getProdData();
                    }
                    // 사무실현황판
                    else if (strWorkType == "tprod")
                    {
                        getTotalProdData();
                    }
                    // 사무실현황판 EV
                    else if(strWorkType == "tprodEv")
                    {
                        getTotalProdDataEv();
                    }
                    // 사무실현황판 PHEV
                    else if (strWorkType == "tprodPhev")
                    {
                        getTotalProdDataPhev();
                    }
                    // 라인현황판
                    else if (strWorkType == "lineProd")
                    {
                        getLineProdData();
                    }
                    // 재고현황판
                    else if (strWorkType == "stock")
                    {
                        getStockData();
                    }
                    // 출하현황판
                    else if (strWorkType == "delivery")
                    {
                        getDeliveryData();
                    }
                    //출하차량모니터링
                    else if (strWorkType == "shipment")
                    {
                        getShipmentData();
                    }
                    //
                    else if (strWorkType == "stationMonitor")
                    {
                        getStationMonitorData();
                    }
                    else if (strWorkType == "stationMonitor2")
                    {
                        getStationMonitorData2();
                    }
                    else if (strWorkType == "stationMonitor4")
                    {
                        getStationMonitorData3();
                    }
                    // @@ KDK add S
                    else if (strWorkType == "dailyClosing")   // 일마감
                    {
                        getDailyClosingData();
                    }
                    else if (strWorkType == "dailyTot") // 지표 종합
                    {
                        getDailyTotData();
                    }
                    else if (strWorkType == "stationMonitor5")
                    {
                        getStationMonitorData4();
                    }
                    else if (strWorkType == "stationMonitor6")
                    {
                        getStationMonitorData5();
                    }
                    // 신규 PIB 생성용
                    else if (strWorkType == "subContPIB")// 사외창고
                    {
                        getSubContPIB();
                    }

                    // @@ KDK add E
                }

                getAlarmData();
            }
            finally
            {
                GC.Collect();
            }
        }

        private void SELECT_Standard()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {

                ds = SELECT_GD(null, null);


                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                else
                {
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][2].ToString() != null)
                    {
                        htiStandard[ds.Tables[0].Rows[0][0].ToString() + ds.Tables[0].Rows[0][1].ToString()] = ds.Tables[0].Rows[0][2].ToString();
                    }
                }

                ds.Clear();
                dt.Clear();
            }
            catch
            {
            }
            finally
            {
                ds.Dispose();
                dt.Dispose();
                ds = null;
                dt = null;
                GC.Collect();
            }
        }

        private void SELECT_StandardForLine(string shop1, string line1, string shop2, string line2)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                ds = SELECT_GD(shop1, line1);


                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                else
                {
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() != null)
                    {
                        //if (strMid == "PDP05")
                        //{
                        iStandardLine1 = Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString());
                        //}
                        //else if (strMid == "PDP02")
                        //{
                        //	iStandardLine_LF = Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString());
                        //}
                        //else if (strMid == "PDP06")
                        //{
                        //	iStandardLine_AEDE1 = Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString());
                        //}
                        //else if (strMid == "PDP07")
                        //{
                        //	iStandardLine_AEEV1 = Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString());
                        //}
                    }
                }

                ds.Clear();
                dt.Clear();

                //ds = this.m_clsDb.SELECT_GD("BPA", "02");
                ds = SELECT_GD(shop2, line2);

                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                else
                {
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    if (ds.Tables[0].Rows[0][0].ToString() != null)
                    {
                        //if (strMid == "PDP05")
                        //{
                        iStandardLine2 = Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString());
                        //}
                        //else if (strMid == "PDP02")
                        //{
                        //	iStandardLine_JF = Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString());
                        //}
                        //else if (strMid == "PDP06")
                        //{
                        //	iStandardLine_AEDE2 = Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString());
                        //}
                        //else if (strMid == "PDP07")
                        //{
                        //	iStandardLine_WG = Convert.ToDouble(ds.Tables[0].Rows[0][0].ToString());
                        //}
                    }
                }

                ds.Clear();
                dt.Clear();

            }
            catch (Exception EX)
            {
            }
            finally
            {
                ds.Dispose();
                dt.Dispose();
                ds = null;
                dt = null;
                GC.Collect();
            }
        }

        private DataSet SELECT_GD(string strShop, string strLine)
        {
            bool bResult = true;
            DataSet ds = new DataSet();
            StringBuilder QueryBuilder = new StringBuilder();

            try
            {
                if (strShop != null && strLine != null)
                {
                    QueryBuilder.AppendFormat(" SELECT SHOP_CD, LINE_CD, DISP_GD FROM {2}C_LINE_CD WITH ( NOLOCK ) WHERE SHOP_CD = '{0}' AND LINE_CD = '{1}' ", strShop, strLine, strLinkedServer);
                }
                else
                {
                    QueryBuilder.AppendFormat(" SELECT SHOP_CD, LINE_CD, DISP_GD FROM {0}C_LINE_CD WITH ( NOLOCK ) WHERE USE_YN='Y' AND DISP_GD > 0 ", strLinkedServer);
                }
                bResult = GetDataSet(QueryBuilder.ToString(), null, CommandType.Text, ref ds, ref m_strErrCode, ref m_strErrText);

            }
            catch
            {

            }
            return ds;
        }

        private DataSet SELECT_Message(string strMoniter_CD)
        {
            bool bResult = true;
            DataSet ds = new DataSet();
            StringBuilder QueryBuilder = new StringBuilder();

            try
            {
                QueryBuilder.AppendFormat(" SELECT TOP 1 MESSAGE FROM {1}C_MONITOR_CD WITH ( NOLOCK ) WHERE MON_CD = '{0}' AND USE_FLG = 1  ", strMoniter_CD, strLinkedServer);
                bResult = GetDataSet(QueryBuilder.ToString(), null, CommandType.Text, ref ds, ref m_strErrCode, ref m_strErrText);
            }
            catch
            {

            }
            finally
            {
            }
            return ds;
        }

        #region 출하현황판 데이터 조회
        private void getDeliveryData()
        {
            //string jsonDs = string.Empty;
            bool bResult = true;
            DataSet ds = new DataSet();
            string strGetDeliveryData = string.Empty;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                // strVM정보가 없으면 데이터를 가져오지 않는다.
                if (strVM != null && strVM.Length > 0)
                {
                    // 전역변수에서 strVM로 검색된 기록이 있는지 찾는다.
                    if (s_htGetDeliveryData.ContainsKey(strVM))
                    {

                        // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 이전에 전역변수에 저장해 두었는 데이터를 가져온다...
                        if ((DateTime)s_htGetDeliveryData[strVM] > DateTime.Now.AddSeconds(-5))
                        {
                            strGetDeliveryData = jss.Serialize(GetList2(s_dsGetDeliveryData));
                            return;
                        }
                        // 마지막으로 검색한 시간을 갱신한다.
                        s_htGetDeliveryData[strVM] = DateTime.Now;
                    }
                    else
                    {
                        // 마지막으로 검색한 시간을 갱신한다.
                        s_htGetDeliveryData[strVM] = DateTime.Now;
                    }
                }
                else
                {
                    return;
                }

                // 데이터 취득
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@VIEW_MODE", strVM); // 지역
                bResult = GetDataSet(strLinkedServer + "SP_PIB_GET_DELIVERY_DATA", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtResult = ds.Tables[0].Copy();
                    dtResult.TableName = strVM.Replace(",", "").Replace(";", "");
                    // 전역변수에 데이터가 있으면 갱신하고 없으면 추가 한다.
                    s_dsGetDeliveryData[dtResult.TableName] = dtResult.Copy();
                    /*
                              if (s_dsGetDeliveryData != null && s_dsGetDeliveryData.Tables.Count > 0)
                              {
                                  if (s_dsGetDeliveryData.Tables.Contains(dtResult.TableName))
                                  {
                                      try
                                      {
                                          if (s_dsGetDeliveryData.Tables.CanRemove(s_dsGetDeliveryData.Tables[dtResult.TableName]))
                                          {
                                              s_dsGetDeliveryData.Tables.Remove(dtResult.TableName);
                                              if (dtResult.Rows.Count > 0)
                                              {
                                                  //s_dsGetDeliveryData.Tables[dtResult.TableName].Merge(dtResult.Copy(), true);
                                                  s_dsGetDeliveryData.Tables.Add(dtResult.Copy());
                                              }
                                          }
                                      }
                                      catch
                                      {
                                      }
                                      //s_dsGetDeliveryData.Tables[dtResult.TableName].Rows.Clear();
                                      //if (dtResult.Rows.Count > 0)
                                      //{
                                      //	//s_dsGetDeliveryData.Tables[dtResult.TableName].Merge(dtResult.Copy(), true);
                                      //	s_dsGetDeliveryData.Tables.Add(dtResult.Copy());
                                      //}
                                  }
                                  else
                                  {
                                      if (dtResult.Rows.Count > 0)
                                      {
                                          s_dsGetDeliveryData.Tables.Add(dtResult.Copy());
                                      }
                                  }
                              }
                              else
                              {
                                  if (dtResult.Rows.Count > 0)
                                  {
                                      s_dsGetDeliveryData.Tables.Add(dtResult.Copy());
                                  }
                              }
                               * */
                }
                else
                {
                    if (s_dsGetDeliveryData.ContainsKey(strVM.Replace(",", "").Replace(";", "")))
                    {
                        s_dsGetDeliveryData.Remove(strVM.Replace(",", "").Replace(";", ""));
                    }
                    /*
                              if (s_dsGetDeliveryData != null && s_dsGetDeliveryData.Tables.Count > 0)
                              {
                                  if (s_dsGetDeliveryData.Tables.Contains(strVM.Replace(",", "").Replace(";", "")))
                                  {
                                      try
                                      {
                                          if (s_dsGetDeliveryData.Tables.CanRemove(s_dsGetDeliveryData.Tables[strVM.Replace(",", "").Replace(";", "")]))
                                          {
                                              s_dsGetDeliveryData.Tables.Remove(strVM.Replace(",", "").Replace(";", ""));
                                          }
                                      }
                                      catch
                                      {
                                      }
                                      //s_dsGetDeliveryData.Tables[strVM.Replace(",", "").Replace(";", "")].Rows.Clear();//.Remove(strVM.Replace(",", "").Replace(";", ""));
                                  }
                              }
                               * */
                }

                //리턴해줄 데이터를 JSON문자열로 변환한다.
                strGetDeliveryData = jss.Serialize(GetList2(s_dsGetDeliveryData));
            }
            catch { }
            finally
            {
                ds.Dispose();
                ds = null;
                GC.Collect();

                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                // 결과를 리턴한다.
                Response.ContentType = "text/plain";
                Response.Write(strGetDeliveryData);
                Response.End();
            }
        }
        #endregion

        #region 출하모니터링
        private void getShipmentData()
        {
            //string jsonDs = string.Empty;
            bool bResult = true;
            DataSet ds = new DataSet();
            try
            {
                // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 데이터 검색안함...
                if (s_dtGetShipData > DateTime.Now.AddSeconds(-5))
                {
                    return;
                }

                // 전역변수에 마지막으로 검색한 시간을 갱신한다.
                s_dtGetShipData = DateTime.Now;


                //      SELECT_Standard(); // 기준 가동율 조회

                // 데이터 취득
                SqlParameter[] param = null;// new SqlParameter[1];
                                            //param[0] = new SqlParameter("@VIEW_MODE", strWorkType);
                                            //  bResult = GetDataSet(strLinkedServer + "SP_SHIPMONO_T", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);
                bResult = GetDataSet(strLinkedServer + "SP_PIB_SHIPCARMONI", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);
                DataSet dsResult = EditDispData_ship(ds);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                s_strGetShipData = jss.Serialize(GetList2(dsResult));

            }
            catch
            {
            }
            finally
            {
                ds.Dispose();
                ds = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                Response.BufferOutput = true;
                Response.ContentType = "text/plain";
                Response.Write(s_strGetShipData);
                Response.End();
            }
        }
        #endregion

        #region 재고현황판 데이터 조회
        private void getStockData()
        {
            //string jsonDs = string.Empty;
            bool bResult = true;
            DataSet ds = new DataSet();
            string strGetStockData = string.Empty;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                // strVM정보가 없으면 데이터를 가져오지 않는다.
                if (strVM != null && strVM.Length > 0)
                {
                    // 전역변수에서 strVM로 검색된 기록이 있는지 찾는다.
                    if (s_htGetStockData.ContainsKey(strVM))
                    {

                        // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 이전에 전역변수에 저장해 두었는 데이터를 가져온다...
                        if ((DateTime)s_htGetStockData[strVM] > DateTime.Now.AddSeconds(-5))
                        {
                            strGetStockData = jss.Serialize(GetList2(s_dsGetStockData));
                            return;
                        }
                        // 마지막으로 검색한 시간을 갱신한다.
                        s_htGetStockData[strVM] = DateTime.Now;
                    }
                    else
                    {
                        // 마지막으로 검색한 시간을 갱신한다.
                        s_htGetStockData[strVM] = DateTime.Now;
                    }
                }
                else
                {
                    return;
                }

                // 데이터 취득
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@VIEW_MODE", strVM); // 지역
                bResult = GetDataSet(strLinkedServer + "SP_PIB_GET_STOCK_DATA", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dtResult = ds.Tables[0].Copy();
                    dtResult.TableName = strVM.Replace(",", "").Replace(";", "");
                    s_dsGetStockData[dtResult.TableName] = dtResult.Copy();
                    // 전역변수에 데이터가 있으면 갱신하고 없으면 추가 한다.
                    /*
                              if (s_dsGetStockData != null && s_dsGetStockData.Tables.Count > 0)
                              {
                                  if (s_dsGetStockData.Tables.Contains(dtResult.TableName))
                                  {
                                      try
                                      {
                                          if (s_dsGetStockData.Tables.CanRemove(s_dsGetStockData.Tables[dtResult.TableName]))
                                          {
                                              s_dsGetStockData.Tables.Remove(dtResult.TableName);
                                              if (dtResult.Rows.Count > 0)
                                              {
                                                  //s_dsGetStockData.Tables[dtResult.TableName].Merge(dtResult.Copy(), true);
                                                  s_dsGetStockData.Tables.Add(dtResult.Copy());
                                              }
                                          }
                                      }
                                      catch
                                      {
                                      }
                                      //s_dsGetStockData.Tables[dtResult.TableName].Rows.Clear();
                                      //if (dtResult.Rows.Count > 0)
                                      //{
                                      //	//s_dsGetStockData.Tables[dtResult.TableName].Merge(dtResult.Copy(), true);
                                      //	s_dsGetStockData.Tables.Add(dtResult.Copy());
                                      //}
                                  }
                                  else
                                  {
                                      if (dtResult.Rows.Count > 0)
                                      {
                                          s_dsGetStockData.Tables.Add(dtResult.Copy());
                                      }
                                  }
                              }
                              else
                              {
                                  if (dtResult.Rows.Count > 0)
                                  {
                                      s_dsGetStockData.Tables.Add(dtResult.Copy());
                                  }
                              }
                               * */
                }
                else
                {
                    if (s_dsGetStockData.ContainsKey(strVM.Replace(",", "").Replace(";", "")))
                    {
                        s_dsGetStockData.Remove(strVM.Replace(",", "").Replace(";", ""));
                    }
                    /*
                              if (s_dsGetStockData != null && s_dsGetStockData.Tables.Count > 0)
                              {
                                  try
                                  {
                                      if (s_dsGetStockData.Tables.CanRemove(s_dsGetStockData.Tables[strVM.Replace(",", "").Replace(";", "")]))
                                      {
                                          s_dsGetStockData.Tables.Remove(strVM.Replace(",", "").Replace(";", ""));
                                      }
                                  }
                                  catch
                                  {
                                  }
                                  //if (s_dsGetStockData.Tables.Contains(strVM.Replace(",", "").Replace(";", "")))
                                  //{
                                  //	s_dsGetStockData.Tables[strVM.Replace(",", "").Replace(";", "")].Rows.Clear();//.Remove(strVM.Replace(",", "").Replace(";", ""));
                                  //}
                              }
                              */
                }

                //리턴해줄 데이터를 JSON문자열로 변환한다.
                strGetStockData = jss.Serialize(GetList2(s_dsGetStockData));
            }
            catch { }
            finally
            {
                ds.Dispose();
                ds = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                // 결과를 리턴한다.
                Response.ContentType = "text/plain";
                Response.Write(strGetStockData);
                Response.End();
            }
        }
        #endregion

        #region 사무실현황판 데이터 조회
        private void getProdData()
        {
            //string jsonDs = string.Empty;
            bool bResult = true;
            DataSet ds = new DataSet();
            try
            {
                // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 데이터 검색안함...
                if (s_dtGetProdData > DateTime.Now.AddSeconds(-5))
                {
                    return;
                }

                // 전역변수에 마지막으로 검색한 시간을 갱신한다.
                s_dtGetProdData = DateTime.Now;


                SELECT_Standard(); // 기준 가동율 조회

                // 데이터 취득
                SqlParameter[] param = null;// new SqlParameter[1];
                                            //param[0] = new SqlParameter("@VIEW_MODE", strWorkType);
                bResult = GetDataSet(strLinkedServer + "C_SP_PDP_GETDATA", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);
                DataSet dsResult = EditDispData(ds);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                s_strGetProdData = jss.Serialize(GetList2(dsResult));

            }
            catch { }
            finally
            {
                ds.Dispose();
                ds = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                Response.BufferOutput = true;
                Response.ContentType = "text/plain";
                Response.Write(s_strGetProdData);
                Response.End();
            }
        }
        #endregion

        #region 사무실현황판 데이터 조회
        private void getTotalProdData()
        {
            //string jsonDs = string.Empty;
            bool bResult = true;
            DataSet dsResult = new DataSet();
            try
            {
                // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 데이터 검색안함...
                if (s_dtGetProdDataT > DateTime.Now.AddSeconds(-5))
                {
                    return;
                }

                // 전역변수에 마지막으로 검색한 시간을 갱신한다.
                s_dtGetProdDataT = DateTime.Now;


                //SELECT_Standard(); // 기준 가동율 조회

                // 데이터 취득
                SqlParameter[] param = null;// new SqlParameter[1];
                                            //param[0] = new SqlParameter("@VIEW_MODE", strWorkType);
                bResult = GetDataSet(strLinkedServer + "SP_PIB_GET_PRODDATA", param, CommandType.StoredProcedure, ref dsResult, ref m_strErrCode, ref m_strErrText);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                s_strGetProdDataT = jss.Serialize(GetList2(dsResult));

            }
            catch { }
            finally
            {
                dsResult.Dispose();
                dsResult = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                Response.BufferOutput = true;
                Response.ContentType = "text/plain";
                Response.Write(s_strGetProdDataT);
                Response.End();
            }
        }

        // EV 데이터 조회
        private void getTotalProdDataEv()
        {
            //string jsonDs = string.Empty;
            bool bResult = true;
            DataSet dsResult = new DataSet();
            try
            {
                // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 데이터 검색안함...
                if (s_dtGetProdDataT > DateTime.Now.AddSeconds(-5))
                {
                    return;
                }

                // 전역변수에 마지막으로 검색한 시간을 갱신한다.
                s_dtGetProdDataT = DateTime.Now;


                //SELECT_Standard(); // 기준 가동율 조회

                // 데이터 취득
                SqlParameter[] param = null;// new SqlParameter[1];
                                            //param[0] = new SqlParameter("@VIEW_MODE", strWorkType);
                bResult = GetDataSet(strLinkedServer + "SP_PIB_GET_PRODDATA_EV", param, CommandType.StoredProcedure, ref dsResult, ref m_strErrCode, ref m_strErrText);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                s_strGetProdDataT = jss.Serialize(GetList2(dsResult));

            }
            catch { }
            finally
            {
                dsResult.Dispose();
                dsResult = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                Response.BufferOutput = true;
                Response.ContentType = "text/plain";
                Response.Write(s_strGetProdDataT);
                Response.End();
            }
        }

        // PHEV 데이터 조회
        private void getTotalProdDataPhev()
        {
            //string jsonDs = string.Empty;
            bool bResult = true;
            DataSet dsResult = new DataSet();
            try
            {
                // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 데이터 검색안함...
                if (s_dtGetProdDataT > DateTime.Now.AddSeconds(-5))
                {
                    return;
                }

                // 전역변수에 마지막으로 검색한 시간을 갱신한다.
                s_dtGetProdDataT = DateTime.Now;


                //SELECT_Standard(); // 기준 가동율 조회

                // 데이터 취득
                SqlParameter[] param = null;// new SqlParameter[1];
                                            //param[0] = new SqlParameter("@VIEW_MODE", strWorkType);
                bResult = GetDataSet(strLinkedServer + "SP_PIB_GET_PRODDATA_PHEV", param, CommandType.StoredProcedure, ref dsResult, ref m_strErrCode, ref m_strErrText);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                s_strGetProdDataT = jss.Serialize(GetList2(dsResult));

            }
            catch { }
            finally
            {
                dsResult.Dispose();
                dsResult = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                Response.BufferOutput = true;
                Response.ContentType = "text/plain";
                Response.Write(s_strGetProdDataT);
                Response.End();
            }
        }
        #endregion


        // @@ KDK add S
        #region 신규 PIB 데이터 조회
        private void getSubContPIB()
        {
            string strErrCod = string.Empty;
            string strErrMsg = string.Empty;
            bool bResult = true;
            DataSet dsResult = new DataSet();
            try
            {
                SqlParameter[] param = null;

                // 데이터 취득
                bResult = GetDataSet(strLinkedServer + "SP_PIB_SUBCONT_DATA", param, CommandType.StoredProcedure, ref dsResult, ref m_strErrCode, ref m_strErrText);

                JavaScriptSerializer jss = new JavaScriptSerializer();
                s_strGetSubContPIBData = jss.Serialize(GetList2(dsResult));

            }
            catch { }

            finally
            {
                dsResult.Dispose();
                dsResult = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                Response.BufferOutput = true;
                Response.ContentType = "text/plain";
                Response.Write(s_strGetSubContPIBData);
                Response.End();
            }
        }
        #endregion

        #region 일마감 현황 데이터 조회
        private void getDailyClosingData()
        {
            string strErrCod = string.Empty;
            string strErrMsg = string.Empty;
            bool bResult = true;
            DataSet ds = new DataSet();
            try
            {
                // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 데이터 검색안함...
                if (s_dtGetDailyClosingData > DateTime.Now.AddSeconds(-5))
                {
                    return;
                }

                // 전역변수에 마지막으로 검색한 시간을 갱신한다.
                s_dtGetDailyClosingData = DateTime.Now;

                string strYMD_FROM = string.Empty;
                string strYMD_TO = string.Empty;
                string strYYYYMM_FROM = string.Empty;
                string strYYYYMM_TO = string.Empty;
                string strYYYY_FROM = string.Empty;
                string strYYYY_TO = string.Empty;

                DateTime dtFrom = DateTime.Now;
                DateTime dtTo = DateTime.Now;

                // 일별 Data 기준
                // 8시 이전 -2일 까지 5일치 ,8시 이후 금일 -1일 까지 5일치
                if (DateTime.Now.Hour > 7)  // 현재시각이 8시 부터
                {
                    // 일별
                    dtFrom = dtFrom.AddDays(-5);
                    dtTo = dtTo.AddDays(-1);

                    strYMD_FROM = dtFrom.ToString("yyyyMMdd");
                    strYMD_TO = dtTo.ToString("yyyyMMdd");
                }
                else // 현재시각이 8시 이전 이면
                {
                    dtFrom = dtFrom.AddDays(-5 - 1);
                    dtTo = dtTo.AddDays(-1 - 1);

                    strYMD_FROM = dtFrom.ToString("yyyyMMdd");
                    strYMD_TO = dtTo.ToString("yyyyMMdd");
                }

                // 월누적, 년누적 기준
                // 월누적:일자별 To의 월 1일 08시00분00초 ~ +다음달 1일 07분59분59초 까지
                strYYYYMM_FROM = dtTo.ToString("yyyyMM") + "01080000";
                strYYYYMM_TO = dtTo.AddMonths(1).ToString("yyyyMM") + "01075959";

                // 년누적:일자별 To의 년 1월 1일 08시00분00초 ~ +다음해 1월1일 07분59분59초 까지
                strYYYY_FROM = dtTo.ToString("yyyy") + "0101080000";
                strYYYY_TO = dtTo.AddYears(1).ToString("yyyy") + "0101075959";

                SqlParameter[] param = new SqlParameter[6];
                param[0] = new SqlParameter("@YMD_FROM", strYMD_FROM);
                param[1] = new SqlParameter("@YMD_TO", strYMD_TO);
                param[2] = new SqlParameter("@YYYYMM_FROM", strYYYYMM_FROM);
                param[3] = new SqlParameter("@YYYYMM_TO", strYYYYMM_TO);
                param[4] = new SqlParameter("@YYYY_FROM", strYYYY_FROM);
                param[5] = new SqlParameter("@YYYY_TO", strYYYY_TO);

                // 데이터 취득
                bResult = GetDataSet(strLinkedServer + "SP_PIB_DAY_CLOSING_DATA", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);
                DataSet dsResult = EditDailyClosingData(ds);

                JavaScriptSerializer jss = new JavaScriptSerializer();
                s_strGetDailyClosingData = jss.Serialize(GetList2(dsResult));

            }
            catch { }

            finally
            {
                ds.Dispose();
                ds = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                Response.BufferOutput = true;
                Response.ContentType = "text/plain";
                Response.Write(s_strGetDailyClosingData);
                Response.End();
            }
        }
        #endregion

        #region 일마감 현황 데이터  편집
        private DataSet EditDailyClosingData(DataSet p_ds)
        {
            int nRowIdx = 1;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //DataTable dt2 = new DataTable();
            DateTime MyDateTime = DateTime.Now;
            string[] strObj = new String[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
            dt.Columns.Add("c1", System.Type.GetType("System.String"));
            dt.Columns.Add("c2", System.Type.GetType("System.String"));
            dt.Columns.Add("c3", System.Type.GetType("System.String"));
            dt.Columns.Add("c4", System.Type.GetType("System.String"));
            dt.Columns.Add("c5", System.Type.GetType("System.String"));
            dt.Columns.Add("c6", System.Type.GetType("System.String"));
            dt.Columns.Add("c7", System.Type.GetType("System.String"));
            dt.Columns.Add("c8", System.Type.GetType("System.String"));

            // 현황판 데이터 저장할 공간을 10개만 먼저 만든다.
            for (int i = 0; i < 10; i++) dt.Rows.Add(strObj);

            if (p_ds != null && p_ds.Tables.Count > 0)
            {
                for (int i = 0; i < p_ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        string strDt = p_ds.Tables[0].Rows[i]["DT"].ToString();
                        if (strDt.Length == 8)
                        {
                            nRowIdx++;
                            dt.Rows[nRowIdx][0] = strDt.Substring(4, 2) + "/" + strDt.Substring(6, 2);
                            dt.Rows[nRowIdx][1] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PL_QTY"].ToString())).ToString();
                            dt.Rows[nRowIdx][2] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["DL_QTY"].ToString())).ToString();
                            dt.Rows[nRowIdx][3] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["MP_QTY"].ToString())).ToString();
                            dt.Rows[nRowIdx][4] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["VZ_QTY"].ToString())).ToString();
                            dt.Rows[nRowIdx][5] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["IN_ST"].ToString())).ToString();
                            dt.Rows[nRowIdx][6] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["OUT_ST"].ToString())).ToString();
                            dt.Rows[nRowIdx][7] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["ST_QTY"].ToString())).ToString();
                        }
                        else if (strDt.Length == 6)
                        {
                            dt.Rows[7][0] = strDt.Substring(4, 2) + "월누적";
                            dt.Rows[7][1] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PL_QTY"].ToString())).ToString();
                            dt.Rows[7][2] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["DL_QTY"].ToString())).ToString();
                            dt.Rows[7][3] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["MP_QTY"].ToString())).ToString();
                            dt.Rows[7][4] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["VZ_QTY"].ToString())).ToString();
                        }
                        else if (strDt.Length == 4)
                        {
                            dt.Rows[8][0] = strDt.Substring(0, 2) + "년누적";
                            dt.Rows[8][1] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PL_QTY"].ToString())).ToString();
                            dt.Rows[8][2] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["DL_QTY"].ToString())).ToString();
                            dt.Rows[8][3] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["MP_QTY"].ToString())).ToString();
                            dt.Rows[8][4] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["VZ_QTY"].ToString())).ToString();
                        }
                        else
                        {

                            continue;
                        }
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                }
            }

            ds.Tables.Add(dt);
            return ds;
        }
        #endregion

        #region 실시간 지표 종합 데이터 조회
        private void getDailyTotData()
        {
            string strErrCod = string.Empty;
            string strErrMsg = string.Empty;
            bool bResult = true;
            DataSet ds = new DataSet();
            try
            {
                // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 데이터 검색안함...
                if (s_dtGetDailyTotData > DateTime.Now.AddSeconds(-5))
                {
                    return;
                }


                // 전역변수에 마지막으로 검색한 시간을 갱신한다.
                s_dtGetDailyTotData = DateTime.Now;

                string strYMD_ST = string.Empty;          // 전일 재고
                string strYMD_PL = string.Empty;          // 금일 생산
                string strYYYYMMDD_FROM = string.Empty;   // 금일 출하 From
                string strYYYYMMDD_TO = string.Empty;     // 금일 출하 To
                string strYYYYMM_FROM = string.Empty;     // 월 누적 출하 From
                string strYYYYMM_TO = string.Empty;       // 월 누적 출하 To
                string strYYYY_FROM = string.Empty;       // 년 누적 출하 From
                string strYYYY_TO = string.Empty;         // 년 누적 출하 To

                DateTime dtFrom = DateTime.Now; // 금일 출하 실적 From
                DateTime dtTo = DateTime.Now;   // 금일 출하 실적 To

                if (DateTime.Now.Hour > 7)
                {
                    strYMD_ST = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");  // 전일 재고
                    strYMD_PL = DateTime.Now.ToString("yyyyMMdd");              // 금일 생산

                    dtTo = dtTo.AddDays(1); // 금일 출하 실적 To
                }
                else
                {
                    strYMD_ST = DateTime.Now.AddDays(-2).ToString("yyyyMMdd");  // 전일 재고
                    strYMD_PL = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");  // 금일 생산

                    dtFrom = dtFrom.AddDays(-1); // 금일 출하 실적 From
                }

                strYYYYMMDD_FROM = dtFrom.ToString("yyyyMMdd") + "080000";          // 출하 금일 From
                strYYYYMMDD_TO = dtTo.ToString("yyyyMMdd") + "075959";              // 출하 금일 To
                strYYYYMM_FROM = dtFrom.ToString("yyyyMM") + "01080000";            // 출하 월 누계 From 
                strYYYYMM_TO = dtFrom.AddMonths(1).ToString("yyyyMM") + "01075959"; // 출하 월 누계 To
                strYYYY_FROM = dtFrom.ToString("yyyy") + "0101080000";              // 출하 년 누계 From
                strYYYY_TO = dtFrom.AddYears(1).ToString("yyyy") + "0101075959";    // 출하 년 누계 To


                SqlParameter[] param = new SqlParameter[8];
                param[0] = new SqlParameter("@YMD_ST", strYMD_ST);
                param[1] = new SqlParameter("@YMD_PL", strYMD_PL);
                param[2] = new SqlParameter("@YYYYMMDD_FROM", strYYYYMMDD_FROM);
                param[3] = new SqlParameter("@YYYYMMDD_TO", strYYYYMMDD_TO);
                param[4] = new SqlParameter("@YYYYMM_FROM", strYYYYMM_FROM);
                param[5] = new SqlParameter("@YYYYMM_TO", strYYYYMM_TO);
                param[6] = new SqlParameter("@YYYY_FROM", strYYYY_FROM);
                param[7] = new SqlParameter("@YYYY_TO", strYYYY_TO);

                // 데이터 취득
                bResult = GetDataSet(strLinkedServer + "SP_PIB_DAY_CLOSING_TOT_DATA", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);
                DataSet dsResult = EditDailyTotData(ds);

                JavaScriptSerializer jss = new JavaScriptSerializer();
                s_strGetDailyTotData = jss.Serialize(GetList2(dsResult));
            }
            catch { }

            finally
            {
                ds.Dispose();
                ds = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                Response.BufferOutput = true;
                Response.ContentType = "text/plain";
                Response.Write(s_strGetDailyTotData);
                Response.End();
            }
        }
        #endregion

        #region 실시간 지표 종합 데이터  편집
        private DataSet EditDailyTotData(DataSet p_ds)
        {
            int nRowIdx = 0;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            //DataTable dt2 = new DataTable();
            DateTime MyDateTime = DateTime.Now;
            string[] strObj = new String[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
            dt.Columns.Add("c1", System.Type.GetType("System.String"));
            dt.Columns.Add("c2", System.Type.GetType("System.String"));
            dt.Columns.Add("c3", System.Type.GetType("System.String"));
            dt.Columns.Add("c4", System.Type.GetType("System.String"));
            dt.Columns.Add("c5", System.Type.GetType("System.String"));
            dt.Columns.Add("c6", System.Type.GetType("System.String"));

            // 현황판 데이터 저장할 공간을 30개만 먼저 만든다.
            for (int i = 0; i < 10; i++) dt.Rows.Add(strObj);

            if (p_ds != null && p_ds.Tables.Count > 0)
            {
                for (int i = 0; i < p_ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        string strDay = s_dtGetDailyTotData.ToString("MM") + "/" + s_dtGetDailyTotData.ToString("dd");

                        if (s_dtGetDailyTotData.Hour < 8)
                            strDay = s_dtGetDailyTotData.AddDays(-1).ToString("MM") + "/" + s_dtGetDailyTotData.AddDays(-1).ToString("dd");

                        string strDt = p_ds.Tables[0].Rows[i]["DT"].ToString();
                        string strR1 = p_ds.Tables[0].Rows[i]["R1"].ToString();

                        if (strR1 == "EV") nRowIdx = 1;
                        else if (strR1 == "HEV") nRowIdx = 2;
                        else if (strR1 == "PHEV") nRowIdx = 3;
                        else if (strR1 == "합계") nRowIdx = 4;
                        else if (strR1 == "금일") nRowIdx = 5;
                        else if (strR1 == "월간") nRowIdx = 6;
                        else if (strR1 == "년간") nRowIdx = 7;

                        if (nRowIdx > 0)
                        {
                            dt.Rows[nRowIdx][0] = strDay; // 쿼리결과 일자 사용 안하고 호출시 일자 사용
                            dt.Rows[nRowIdx][1] = p_ds.Tables[0].Rows[i]["R1"].ToString();
                            dt.Rows[nRowIdx][2] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["R2"].ToString())).ToString();
                            dt.Rows[nRowIdx][3] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["R3"].ToString())).ToString();
                            dt.Rows[nRowIdx][4] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["R4"].ToString())).ToString();
                            dt.Rows[nRowIdx][5] = String.Format("{0:#,0}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["R5"].ToString())).ToString();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            ds.Tables.Add(dt);
            return ds;
        }
        #endregion
        // @@ KDK add E

        #region 라인현황판 데이터 조회
        private void getLineProdData()
        {
            bool bResult = true;
            DataSet ds = new DataSet();
            string strGetLineProdData = string.Empty;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            //string mShop1 = string.Empty;
            //string mLine1 = string.Empty;
            //string mShop2 = string.Empty;
            //string mLine2 = string.Empty;
            try
            {
                // shop,line정보가 없으면 데이터를 가져오지 않는다.
                if (strShop1 != null && strLine1 != null && strShop2 != null && strLine2 != null
                  && strShop1.Length > 0 && strLine1.Length > 0 && strShop2.Length > 0 && strLine2.Length > 0)
                {
                    // 전역변수에서 strShop1 + strLine1 + strShop2 + strLine2로 검색된 기록이 있는지 찾는다.
                    if (s_htGetLineProdData.ContainsKey(strShop1 + strLine1 + strShop2 + strLine2))
                    {

                        // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 이전에 전역변수에 저장해 두었는 데이터를 가져온다...
                        if ((DateTime)s_htGetLineProdData[strShop1 + strLine1 + strShop2 + strLine2] > DateTime.Now.AddSeconds(-5))
                        {
                            strGetLineProdData = jss.Serialize(GetList2(s_dsGetLineProdData));
                            return;
                        }
                        // 마지막으로 검색한 시간을 갱신한다.
                        s_htGetLineProdData[strShop1 + strLine1 + strShop2 + strLine2] = DateTime.Now;
                    }
                    else
                    {
                        // 마지막으로 검색한 시간을 갱신한다.
                        s_htGetLineProdData[strShop1 + strLine1 + strShop2 + strLine2] = DateTime.Now;
                    }
                }
                else
                {
                    return;
                }

                SELECT_StandardForLine(strShop1, strLine1, strShop2, strLine2);

                // 데이터 취득
                SqlParameter[] param = null;// new SqlParameter[1];
                                            //param[0] = new SqlParameter("@VIEW_MODE", strWorkType);
                bResult = GetDataSet(strLinkedServer + "C_SP_PDP_GETDATA_LINE", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);

                // 라인현황판용 데이터를 편집한다.
                DataTable dtResult = EditLineDispData(ds, strShop1, strLine1, strShop2, strLine2);

                // 전역변수에 데이터가 있으면 갱신하고 없으면 추가 한다.
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    s_dsGetLineProdData[dtResult.TableName] = dtResult.Copy();
                }
                else
                {
                    if (s_dsGetLineProdData.ContainsKey(strShop1 + strLine1 + strShop2 + strLine2))
                    {
                        s_dsGetLineProdData.Remove(strShop1 + strLine1 + strShop2 + strLine2);
                    }
                }
                //if (s_dsGetLineProdData != null && s_dsGetLineProdData.Count > 0)
                //{
                //	if (s_dsGetLineProdData.Tables.Contains(dtResult.TableName))
                //	{
                //		try
                //		{
                //			if (s_dsGetLineProdData.Tables.CanRemove(s_dsGetLineProdData.Tables[dtResult.TableName]))
                //			{
                //				s_dsGetLineProdData.Tables.Remove(dtResult.TableName); 
                //				if (dtResult.Rows.Count > 0)
                //				{
                //					s_dsGetLineProdData.Tables.Add(dtResult.Copy());
                //				}
                //			}
                //		}
                //		catch
                //		{
                //		}
                //	}
                //	else
                //	{
                //		if (dtResult.Rows.Count > 0)
                //		{
                //			s_dsGetLineProdData.Tables.Add(dtResult.Copy());
                //		}
                //	}
                //}
                //else
                //{
                //	if (dtResult.Rows.Count > 0)
                //	{
                //		s_dsGetLineProdData.Tables.Add(dtResult.Copy());
                //	}
                //}

                //리턴해줄 데이터를 JSON문자열로 변환한다.
                strGetLineProdData = jss.Serialize(GetList2(s_dsGetLineProdData));
            }
            catch { }
            finally
            {
                ds.Dispose();
                ds = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                // 결과를 리턴한다.
                Response.ContentType = "text/plain";
                Response.Write(strGetLineProdData);
                Response.End();
            }
        }
        #endregion

        #region 공정 모니터링 데이터 조회
        private void getStationMonitorData()
        {
            //string jsonDs = string.Empty;
            bool bResult = true;
            DataSet ds = new DataSet();
            try
            {
                // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 데이터 검색안함...
                if (s_dtGetStationMonitorData > DateTime.Now.AddSeconds(-5))
                {
                    return;
                }

                // 전역변수에 마지막으로 검색한 시간을 갱신한다.
                s_dtGetStationMonitorData = DateTime.Now;


                //      SELECT_Standard(); // 기준 가동율 조회

                // 데이터 취득
                SqlParameter[] param = null;

                bResult = GetDataSet(strLinkedServer + "SP_PIB_STATION_MONITOR", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);
                DataSet dsResult = EditDispData_StationMonitor(ds);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                s_strGetStationMonitorData = jss.Serialize(GetList2(dsResult));

            }
            catch
            {
            }
            finally
            {
                ds.Dispose();
                ds = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                Response.BufferOutput = true;
                Response.ContentType = "text/plain";
                Response.Write(s_strGetStationMonitorData);
                Response.End();
            }
        }
        #endregion
        private void getStationMonitorData2()
        {
            //string jsonDs = string.Empty;
            bool bResult = true;
            DataSet ds = new DataSet();
            try
            {
                // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 데이터 검색안함...
                if (s_dtGetStationMonitorData > DateTime.Now.AddSeconds(-5))
                {
                    return;
                }

                // 전역변수에 마지막으로 검색한 시간을 갱신한다.
                s_dtGetStationMonitorData = DateTime.Now;


                //      SELECT_Standard(); // 기준 가동율 조회

                // 데이터 취득
                SqlParameter[] param = null;

                bResult = GetDataSet(strLinkedServer + "SP_PIB_STATION_MONITOR2", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);
                DataSet dsResult = EditDispData_StationMonitor2(ds);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                s_strGetStationMonitorData2 = jss.Serialize(GetList2(dsResult));

            }
            catch
            {
            }
            finally
            {
                ds.Dispose();
                ds = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                Response.BufferOutput = true;
                Response.ContentType = "text/plain";
                Response.Write(s_strGetStationMonitorData2);
                Response.End();
            }
        }

        private void getStationMonitorData3()
        {
            //string jsonDs = string.Empty;
            bool bResult = true;
            DataSet ds = new DataSet();
            try
            {
                // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 데이터 검색안함...
                if (s_dtGetStationMonitorData > DateTime.Now.AddSeconds(-5))
                {
                    return;
                }

                // 전역변수에 마지막으로 검색한 시간을 갱신한다.
                s_dtGetStationMonitorData = DateTime.Now;


                //      SELECT_Standard(); // 기준 가동율 조회

                // 데이터 취득
                SqlParameter[] param = null;

                bResult = GetDataSet(strLinkedServer + "SP_PIB_STATION_MONITOR3", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);
                DataSet dsResult = EditDispData_StationMonitor3(ds);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                s_strGetStationMonitorData3 = jss.Serialize(GetList2(dsResult));

            }
            catch
            {
            }
            finally
            {
                ds.Dispose();
                ds = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                Response.BufferOutput = true;
                Response.ContentType = "text/plain";
                Response.Write(s_strGetStationMonitorData3);
                Response.End();
            }
        }

        private void getStationMonitorData4()
        {
            //string jsonDs = string.Empty;
            bool bResult = true;
            DataSet ds = new DataSet();
            try
            {
                // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 데이터 검색안함...
                if (s_dtGetStationMonitorData > DateTime.Now.AddSeconds(-5))
                {
                    return;
                }

                // 전역변수에 마지막으로 검색한 시간을 갱신한다.
                s_dtGetStationMonitorData = DateTime.Now;


                //      SELECT_Standard(); // 기준 가동율 조회

                // 데이터 취득
                SqlParameter[] param = null;

                bResult = GetDataSet(strLinkedServer + "SP_PIB_STATION_MONITOR4", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);
                DataSet dsResult = EditDispData_StationMonitor4(ds);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                s_strGetStationMonitorData3 = jss.Serialize(GetList2(dsResult));

            }
            catch
            {
            }
            finally
            {
                ds.Dispose();
                ds = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                Response.BufferOutput = true;
                Response.ContentType = "text/plain";
                Response.Write(s_strGetStationMonitorData3);
                Response.End();
            }
        }

        private void getStationMonitorData5()
        {
            //string jsonDs = string.Empty;
            bool bResult = true;
            DataSet ds = new DataSet();
            try
            {
                // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면 데이터 검색안함...
                if (s_dtGetStationMonitorData > DateTime.Now.AddSeconds(-5))
                {
                    return;
                }

                // 전역변수에 마지막으로 검색한 시간을 갱신한다.
                s_dtGetStationMonitorData = DateTime.Now;


                //      SELECT_Standard(); // 기준 가동율 조회

                // 데이터 취득
                SqlParameter[] param = null;

                bResult = GetDataSet(strLinkedServer + "SP_PIB_STATION_MONITOR5", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);
                DataSet dsResult = EditDispData_StationMonitor4(ds);
                JavaScriptSerializer jss = new JavaScriptSerializer();
                s_strGetStationMonitorData3 = jss.Serialize(GetList2(dsResult));

            }
            catch
            {
            }
            finally
            {
                ds.Dispose();
                ds = null;
                GC.Collect();
                try
                {
                    // 현황판 상태 갱신...
                    UpdateHeartBit(strMid, strPid, strClientAddr);
                }
                finally
                {

                }
                Response.BufferOutput = true;
                Response.ContentType = "text/plain";
                Response.Write(s_strGetStationMonitorData3);
                Response.End();
            }
        }

        #region 알람 데이터 조회 
        private void getAlarmData()
        {
            StringBuilder QueryBuilder = new StringBuilder();
            bool bResult = true;
            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();
            DataSet ds4 = new DataSet();
            string strGetAlarmData = string.Empty;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string strTableName = string.Empty;
            try
            {
                //// 마지막으로 검색한 시간이 현재 검색한 시간보다 2초 전이면...
                //if (s_dtGetAlarmData > DateTime.Now.AddSeconds(-2))
                //{
                //	strGetAlarmData = "[{\"STATE\":\"WAIT\"}]";
                //	return;
                //}

                //// 마지막으로 검색한 시간을 갱신한다.
                //s_dtGetAlarmData = DateTime.Now;

                // shop,line정보가 없으면 데이터를 가져오지 않는다.
                if (strShop1 != null && strLine1 != null && strShop2 != null && strLine2 != null
                  && strShop1.Length > 0 && strLine1.Length > 0 && strShop2.Length > 0 && strLine2.Length > 0)
                {
                    strTableName = strShop1 + strLine1 + strShop2 + strLine2;
                    if (s_htGetAlarmData.ContainsKey(strShop1 + strLine1 + strShop2 + strLine2))
                    {

                        // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면...
                        if ((DateTime)s_htGetAlarmData[strShop1 + strLine1 + strShop2 + strLine2] > DateTime.Now.AddSeconds(-2))
                        {
                            strGetAlarmData = jss.Serialize(GetList2(s_dsGetAlarmData));
                            return;
                        }
                        // 마지막으로 검색한 시간을 갱신한다.
                        s_htGetAlarmData[strShop1 + strLine1 + strShop2 + strLine2] = DateTime.Now;
                    }
                    else
                    {
                        // 마지막으로 검색한 시간을 갱신한다.
                        s_htGetAlarmData.Add(strShop1 + strLine1 + strShop2 + strLine2, DateTime.Now);
                    }
                }
                // 모니터 정보가 없으면 데이터를 가져오지 않는다.
                else if (strMid != null && strMid.Length > 0)
                {
                    strTableName = strMid;
                    if (s_htGetAlarmData.ContainsKey(strMid))
                    {

                        // 마지막으로 검색한 시간이 현재 검색한 시간보다 5초 전이면...
                        if ((DateTime)s_htGetAlarmData[strMid] > DateTime.Now.AddSeconds(-2))
                        {
                            strGetAlarmData = jss.Serialize(GetList2(s_dsGetAlarmData));
                            return;
                        }
                        // 마지막으로 검색한 시간을 갱신한다.
                        s_htGetAlarmData[strMid] = DateTime.Now;
                    }
                    else
                    {
                        s_htGetAlarmData.Add(strMid, DateTime.Now);
                    }
                }
                else
                {
                    return;
                }


                SqlParameter[] param = null;
                param = null;
                QueryBuilder = new StringBuilder();
                QueryBuilder.AppendFormat(" SELECT MSG FROM {1}C_PIB_MONITOR_MA WITH ( NOLOCK ) WHERE MONI_ID = '{0}' ", strMid, strLinkedServer);
                bResult = GetDataSet(QueryBuilder.ToString(), param, CommandType.Text, ref ds3, ref m_strErrCode, ref m_strErrText);

                if (strWorkType == "stockAlm" || strWorkType == "deliveryAlm")
                {
                    param = null;
                    QueryBuilder = new StringBuilder();
                    QueryBuilder.AppendFormat(" SELECT DISTINCT STUFF((SELECT ',' + CASE WHEN MSG IS NULL OR DATALENGTH(MSG) < 1 THEN NULL ELSE MSG END FROM {2}C_PIB_MONITOR_IF WITH (NOLOCK) WHERE MONI_ID=A.MONI_ID FOR XML PATH('')),1,1,'') AS MSG FROM {2}C_PIB_MONITOR_IF A WITH ( NOLOCK ) WHERE MONI_ID = '{0}' ", strMid, strPid, strLinkedServer);
                    bResult = GetDataSet(QueryBuilder.ToString(), param, CommandType.Text, ref ds4, ref m_strErrCode, ref m_strErrText);
                }
                else
                {

                    // 메세지 데이터 취득 - 기존소스
                    param = null;// new SqlParameter[1];
                    QueryBuilder = new StringBuilder();
                    //param[0] = new SqlParameter("@VIEW_MODE", strWorkType);
                    QueryBuilder.AppendFormat(" SELECT TOP 1 MESSAGE FROM {1}C_MONITOR_CD WITH ( NOLOCK ) WHERE MON_CD = '{0}' AND USE_FLG = 1  ", strMid, strLinkedServer);
                    bResult = GetDataSet(QueryBuilder.ToString(), param, CommandType.Text, ref ds, ref m_strErrCode, ref m_strErrText);

                    // 알람 데이터 취득 - 기존소스
                    param = new SqlParameter[4];
                    param[0] = new SqlParameter("@SHOP_CD", "");
                    param[1] = new SqlParameter("@LINE_CD", "");
                    param[2] = new SqlParameter("@STN_CD", "");
                    param[3] = new SqlParameter("@TYPE", "PDP");
                    bResult = GetDataSet(strLinkedServer + "C_SP_PPC_ALARM_CHECK", param, CommandType.StoredProcedure, ref ds2, ref m_strErrCode, ref m_strErrText);
                }

                //DataSet dsResult = new DataSet();
                DataTable dtResult = new DataTable(strTableName);
                dtResult.Columns.Add("c1");
                dtResult.Columns.Add("BackColor");
                // 우선순위 1 - 현황판에 알릴 메세지
                if (ds4 != null && ds4.Tables.Count > 0 && ds4.Tables[0].Rows.Count > 0 && ds4.Tables[0].Rows[0]["MSG"].ToString() != "")
                {
                    dtResult.Rows.Add(new string[] { ds4.Tables[0].Rows[0]["MSG"].ToString(), "White" });
                }
                else
                {
                    if (ds3 != null && ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0 && ds3.Tables[0].Rows[0]["MSG"].ToString() != "")
                    {
                        dtResult.Rows.Add(new string[] { ds3.Tables[0].Rows[0]["MSG"].ToString(), "White" });
                    }
                    else
                    {
                        // 우선순위 2 - 알람 메세지
                        if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0 && ds2.Tables[0].Rows[0]["ALM_MSG"].ToString() != "")
                        {
                            dtResult.Rows.Add(new string[] { ds2.Tables[0].Rows[0]["ALM_MSG"].ToString(), "yellow" });
                        }
                        else
                        {
                            // 우선순위 3 - 작업시간...
                            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["MESSAGE"].ToString() != "")
                            {
                                dtResult.Rows.Add(new string[] { ds.Tables[0].Rows[0]["MESSAGE"].ToString(), "White" });
                            }
                        }
                    }
                }

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    s_dsGetAlarmData[strTableName] = dtResult.Copy();
                }
                else
                {
                    if (s_dsGetAlarmData.ContainsKey(strTableName))
                    {
                        s_dsGetAlarmData.Remove(strTableName);
                    }
                }

                //if (s_dsGetAlarmData != null)
                //{
                //	if (s_dsGetAlarmData.Tables.Contains(dtResult.TableName))
                //	{
                //		if (s_dsGetAlarmData.Tables.CanRemove(s_dsGetAlarmData.Tables[dtResult.TableName]))
                //		{
                //			s_dsGetAlarmData.Tables.Remove(dtResult.TableName);
                //			//s_dsGetAlarmData.Tables[dtResult.TableName].Rows.Clear();
                //			if (dtResult.Rows.Count > 0)
                //			{
                //				s_dsGetAlarmData.Tables.Add(dtResult.Copy());
                //				//s_dsGetAlarmData.Tables[dtResult.TableName].Merge(dtResult.Copy(), true);
                //			}
                //		}
                //	}
                //	else
                //	{
                //		if (dtResult.Rows.Count > 0)
                //		{
                //			s_dsGetAlarmData.Tables.Add(dtResult.Copy());
                //		}
                //	}
                //}
                //else
                //{
                //	if (dtResult.Rows.Count > 0)
                //	{
                //		s_dsGetAlarmData.Tables.Add(dtResult.Copy());
                //	}
                //}

                strGetAlarmData = jss.Serialize(GetList2(s_dsGetAlarmData));
            }
            catch { }
            finally
            {
                ds.Dispose();
                ds = null;
                ds2.Dispose();
                ds2 = null;
                ds3.Dispose();
                ds3 = null;
                ds4.Dispose();
                ds4 = null;
                GC.Collect();
                Response.ContentType = "text/plain";
                Response.Write(strGetAlarmData);
                Response.End();
            }
        }
        #endregion

        #region 출하차량 모니터링 편집
        private DataSet EditDispData_ship(DataSet p_ds)
        {
            int nRowIdx = 0;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DateTime MyDateTime = DateTime.Now;
            string[] strObj = new String[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, String.Format("{0:yyyy'/'MM'/'dd ddd}", MyDateTime) + "요일", String.Format("{0:HH:mm:ss}", MyDateTime) };
            dt.Columns.Add("c1", System.Type.GetType("System.String"));
            dt.Columns.Add("c2", System.Type.GetType("System.String"));
            dt.Columns.Add("c3", System.Type.GetType("System.String"));
            dt.Columns.Add("c4", System.Type.GetType("System.String"));
            dt.Columns.Add("c5", System.Type.GetType("System.String"));
            dt.Columns.Add("c6", System.Type.GetType("System.String"));
            dt.Columns.Add("c7", System.Type.GetType("System.String"));
            dt.Columns.Add("c8", System.Type.GetType("System.String"));
            dt.Columns.Add("c9", System.Type.GetType("System.String"));
            dt.Columns.Add("CurrDate", System.Type.GetType("System.String"));
            dt.Columns.Add("CurrTime", System.Type.GetType("System.String"));

            dt2.Columns.Add("c1", System.Type.GetType("System.String"));
            dt2.Columns.Add("c2", System.Type.GetType("System.String"));
            dt2.Columns.Add("c3", System.Type.GetType("System.String"));
            dt2.Columns.Add("c4", System.Type.GetType("System.String"));
            dt2.Columns.Add("c5", System.Type.GetType("System.String"));
            dt2.Columns.Add("c6", System.Type.GetType("System.String"));
            dt2.Columns.Add("c7", System.Type.GetType("System.String"));
            dt2.Columns.Add("c8", System.Type.GetType("System.String"));
            dt2.Columns.Add("c9", System.Type.GetType("System.String"));
            dt2.Columns.Add("CurrDate", System.Type.GetType("System.String"));
            dt2.Columns.Add("CurrTime", System.Type.GetType("System.String"));

            // 현황판 데이터 저장할 공간을 30개만 먼저 만든다.
            for (int i = 0; i < 100; i++) dt.Rows.Add(strObj);

            if (p_ds != null && p_ds.Tables.Count > 0)
            {


                for (int i = 0; i < p_ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        if (p_ds.Tables[0].Rows[i]["CAR_NM"].ToString() != "")
                        {
                            int k = i + 1;
                            nRowIdx++;
                            dt.Rows[nRowIdx][0] = k;
                            dt.Rows[nRowIdx][1] = p_ds.Tables[0].Rows[i]["CAR_NM"].ToString();
                            dt.Rows[nRowIdx][2] = String.Format("{0:HH:mm}", Convert.ToDateTime(p_ds.Tables[0].Rows[i]["INTIME"].ToString())).ToString();
                            dt.Rows[nRowIdx][3] = String.Format("{0:HH:mm}", Convert.ToDateTime(p_ds.Tables[0].Rows[i]["OUTTIME"].ToString())).ToString();
                            dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["PART_NO"].ToString();
                            dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["CD_NM"].ToString();
                            dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["ALC"].ToString();
                            dt.Rows[nRowIdx][7] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["CNT"].ToString())).ToString();
                            dt.Rows[nRowIdx][8] = p_ds.Tables[0].Rows[i]["COMMENT"].ToString();
                            //dt.Rows[nRowIdx][2] = String.Format("{0:HH:mm}", Convert.ToDateTime(p_ds.Tables[0].Rows[i]["INTIME"].ToString())).ToString();
                            //dt.Rows[nRowIdx][3] = String.Format("{0:HH:mm}", Convert.ToDateTime(p_ds.Tables[0].Rows[i]["OUTTIME"].ToString())).ToString();

                        }
                    }

                    catch (Exception ex)
                    {

                    }
                }
                // 30개 중에서 빈것은 지운다....
                for (int i = 100; i > 0; i--)
                {
                    if (dt.Rows[i - 1][0].ToString() == null || dt.Rows[i - 1][0].ToString().Length == 0)
                    {
                        dt.Rows.RemoveAt(i - 1);
                    }
                }

                // 설정값이 이상한 데이터를 보여줄 데이터에 추가한다.
                if (dt2.Rows.Count > 0)
                {
                    DataRow[] drTemp = dt2.Select();
                    foreach (DataRow row in drTemp)
                    {
                        //object[] obj = row.ItemArray;
                        dt.Rows.Add(row.ItemArray);
                    }
                }
            }
            ds.Tables.Add(dt);
            return ds;
        }
        #endregion

        #region 사무실현황판 데이터 편집
        private DataSet EditDispData(DataSet p_ds)
        {
            int nRowIdx = 0;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DateTime MyDateTime = DateTime.Now;
            string[] strObj = new String[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, String.Format("{0:yyyy'/'MM'/'dd ddd}", MyDateTime) + "요일", String.Format("{0:HH:mm:ss}", MyDateTime) };
            dt.Columns.Add("c1", System.Type.GetType("System.String"));
            dt.Columns.Add("c2", System.Type.GetType("System.String"));
            dt.Columns.Add("c3", System.Type.GetType("System.String"));
            dt.Columns.Add("c4", System.Type.GetType("System.String"));
            dt.Columns.Add("c5", System.Type.GetType("System.String"));
            dt.Columns.Add("c6", System.Type.GetType("System.String"));
            dt.Columns.Add("c7", System.Type.GetType("System.String"));
            dt.Columns.Add("CurrDate", System.Type.GetType("System.String"));
            dt.Columns.Add("CurrTime", System.Type.GetType("System.String"));

            dt2.Columns.Add("c1", System.Type.GetType("System.String"));
            dt2.Columns.Add("c2", System.Type.GetType("System.String"));
            dt2.Columns.Add("c3", System.Type.GetType("System.String"));
            dt2.Columns.Add("c4", System.Type.GetType("System.String"));
            dt2.Columns.Add("c5", System.Type.GetType("System.String"));
            dt2.Columns.Add("c6", System.Type.GetType("System.String"));
            dt2.Columns.Add("c7", System.Type.GetType("System.String"));
            dt2.Columns.Add("CurrDate", System.Type.GetType("System.String"));
            dt2.Columns.Add("CurrTime", System.Type.GetType("System.String"));

            // 현황판 데이터 저장할 공간을 30개만 먼저 만든다.
            for (int i = 0; i < 30; i++) dt.Rows.Add(strObj);

            if (p_ds != null && p_ds.Tables.Count > 0)
            {
                // LF / JF HEV 1
                // LF / JF HEV 2
                // AE/DE HEV 1

                for (int i = 0; i < p_ds.Tables[0].Rows.Count; i++)
                {
                    bool bLineExist = false;
                    string strLineInfo = string.Empty;
                    string strLineTitle = string.Empty;
                    string[] arrLineInfo = null;
                    try
                    {
                        // Web.config에서 라인정보를 가져온다.
                        strLineInfo = ConfigurationManager.AppSettings.Get(p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() + ";" + p_ds.Tables[0].Rows[i]["LINE_CD"].ToString());
                        // 라인정보가 있을경우
                        if (strLineInfo != null && strLineInfo.Length > 0)
                        {
                            // 라인정보를 ';'문자로 분리
                            arrLineInfo = strLineInfo.Split(';');
                            if (arrLineInfo != null && arrLineInfo.Length > 0)
                            {
                                // 라인명 취득
                                strLineTitle = arrLineInfo[0];

                                // 라인표시순서가 있으면....
                                if (arrLineInfo.Length > 1)
                                {
                                    // 표시순서 취득
                                    if (!Int32.TryParse(arrLineInfo[1], out nRowIdx))
                                    {
                                        // 표시순서가 잘못되었으면 0으로...
                                        nRowIdx = 0;
                                    }

                                    // 순서가 범위를 벗어났으면...
                                    if (nRowIdx < 0 || nRowIdx >= 30) nRowIdx = 0;

                                }
                                // 라인 정보가 없으면....
                                else
                                {
                                    nRowIdx = 0;
                                }
                            }
                            else
                            {
                                strLineTitle = p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() + "_" + p_ds.Tables[0].Rows[i]["LINE_CD"].ToString();
                                nRowIdx = 0;
                            }

                            bLineExist = true;
                        }
                    }
                    catch
                    {
                    }

                    //switch (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() + p_ds.Tables[0].Rows[i]["LINE_CD"].ToString())
                    //{

                    //	case "BPA03":
                    //		nRowIdx = 0;
                    //		dt.Rows[nRowIdx][0] = strLineTitle;
                    //		bLineExist = true;
                    //		break;
                    //	case "BPA04":
                    //		nRowIdx = 1;
                    //		dt.Rows[nRowIdx][0] = strLineTitle;
                    //		bLineExist = true;
                    //		break;
                    //	case "BPA07":
                    //		nRowIdx = 2;
                    //		dt.Rows[nRowIdx][0] = strLineTitle;
                    //		bLineExist = true;
                    //		break;
                    //	case "BPA08":
                    //		nRowIdx = 3;
                    //		dt.Rows[nRowIdx][0] = strLineTitle;
                    //		bLineExist = true;
                    //		break;
                    //	case "BPA01":
                    //		nRowIdx = 4;
                    //		dt.Rows[nRowIdx][0] = strLineTitle;
                    //		bLineExist = true;
                    //		break;
                    //	case "BPA05":
                    //		nRowIdx = 5;
                    //		dt.Rows[nRowIdx][0] = strLineTitle;
                    //		bLineExist = true;
                    //		break;
                    //	case "CMA09":
                    //		nRowIdx = 6;
                    //		dt.Rows[nRowIdx][0] = strLineTitle;
                    //		bLineExist = true;
                    //		break;
                    //	case "BPA10":
                    //		nRowIdx = 7;
                    //		dt.Rows[nRowIdx][0] = strLineTitle;
                    //		bLineExist = true;
                    //		break;

                    //	default: 
                    //		dt.Rows.Add(strObj);
                    //		nRowIdx = dt.Rows.Count-1;
                    //		dt.Rows[nRowIdx][0] = strLineTitle;
                    //		bLineExist = true;
                    //		break;

                    //}

                    if (bLineExist)
                    {
                        // 순서가 이상할 경우....
                        if (nRowIdx == 0)
                        {
                            dt2.Rows.Add(strObj);
                            dt2.Rows[dt2.Rows.Count - 1][0] = strLineTitle;

                            dt2.Rows[dt2.Rows.Count - 1][1] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString())).ToString();
                            dt2.Rows[dt2.Rows.Count - 1][2] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["TARGET_QTY"].ToString())).ToString();
                            dt2.Rows[dt2.Rows.Count - 1][3] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["COMP_QTY"].ToString())).ToString();

                            if (Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString()) > 0)
                            {
                                dt2.Rows[dt2.Rows.Count - 1][4] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                                dt2.Rows[dt2.Rows.Count - 1][5] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";
                                dt2.Rows[dt2.Rows.Count - 1][6] = p_ds.Tables[0].Rows[i]["DIRECT_RATE"].ToString() + "%";

                            }
                            else
                            {
                                dt2.Rows[dt2.Rows.Count - 1][4] = string.Empty;
                                dt2.Rows[dt2.Rows.Count - 1][5] = string.Empty;
                                dt2.Rows[dt2.Rows.Count - 1][6] = string.Empty;
                            }

                            double diStandard = 0;
                            if (htiStandard.ContainsKey(p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() + p_ds.Tables[0].Rows[i]["LINE_CD"].ToString()))
                            {
                                diStandard = (double)htiStandard[p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() + p_ds.Tables[0].Rows[i]["LINE_CD"].ToString()];
                            }

                            if (Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) > diStandard ||
                              Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) == 0)
                            {
                                dt2.Rows[dt2.Rows.Count - 1][5] = "<span class=\"RateState1\">" + dt2.Rows[dt2.Rows.Count - 1][5].ToString() + "</span>";
                            }
                            else
                            {
                                dt2.Rows[dt2.Rows.Count - 1][5] = "<span class=\"RateState2\">" + dt2.Rows[dt2.Rows.Count - 1][5].ToString() + "</span>";
                            }
                        }
                        // 순서가 정상일 경우
                        else
                        {
                            dt.Rows[nRowIdx][0] = strLineTitle;
                            dt.Rows[nRowIdx][1] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString())).ToString();
                            dt.Rows[nRowIdx][2] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["TARGET_QTY"].ToString())).ToString();
                            dt.Rows[nRowIdx][3] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["COMP_QTY"].ToString())).ToString();

                            if (Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString()) > 0)
                            {
                                dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                                dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";
                                dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["DIRECT_RATE"].ToString() + "%";

                            }
                            else
                            {
                                dt.Rows[nRowIdx][4] = string.Empty;
                                dt.Rows[nRowIdx][5] = string.Empty;
                                dt.Rows[nRowIdx][6] = string.Empty;
                            }

                            double diStandard = 0;
                            if (htiStandard.ContainsKey(p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() + p_ds.Tables[0].Rows[i]["LINE_CD"].ToString()))
                            {
                                diStandard = (double)htiStandard[p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() + p_ds.Tables[0].Rows[i]["LINE_CD"].ToString()];
                            }

                            if (Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) > diStandard ||
                              Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) == 0)
                            {
                                dt.Rows[nRowIdx][5] = "<span class=\"RateState1\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                            }
                            else
                            {
                                dt.Rows[nRowIdx][5] = "<span class=\"RateState2\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                            }
                        }
                    }

                    #region old
                    //if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() == "BPA" &&
                    //	p_ds.Tables[0].Rows[i]["LINE_CD"].ToString() == "03")
                    //{
                    //	nRowIdx = 0;
                    //	dt.Rows[nRowIdx][1] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][2] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["TARGET_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][3] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["COMP_QTY"].ToString())).ToString();

                    //	if (Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString()) > 0)
                    //	{
                    //		dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["DIRECT_RATE"].ToString() + "%";

                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][4] = string.Empty;
                    //		dt.Rows[nRowIdx][5] = string.Empty;
                    //		dt.Rows[nRowIdx][6] = string.Empty;
                    //	}

                    //	if (Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) > iStandard_LF ||
                    //		Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) == 0)
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState1\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState2\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}

                    //}
                    //else if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() == "BPA" &&
                    //		p_ds.Tables[0].Rows[i]["LINE_CD"].ToString() == "04")
                    //{
                    //	nRowIdx = 1;
                    //	dt.Rows[nRowIdx][1] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][2] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["TARGET_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][3] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["COMP_QTY"].ToString())).ToString();

                    //	if (Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString()) > 0)
                    //	{
                    //		dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["DIRECT_RATE"].ToString() + "%";

                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][4] = string.Empty;
                    //		dt.Rows[nRowIdx][5] = string.Empty;
                    //		dt.Rows[nRowIdx][6] = string.Empty;
                    //	}

                    //	if (Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) > iStandard_JF ||
                    //		Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) == 0)
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState1\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState2\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //}
                    //else if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() == "BPA" &&
                    //		p_ds.Tables[0].Rows[i]["LINE_CD"].ToString() == "07")
                    //{
                    //	nRowIdx = 2;

                    //	if (Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString()) > 0)
                    //	{
                    //		dt.Rows[nRowIdx][1] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString())).ToString();
                    //		dt.Rows[nRowIdx][2] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["TARGET_QTY"].ToString())).ToString();
                    //		dt.Rows[nRowIdx][3] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["COMP_QTY"].ToString())).ToString();
                    //		dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["DIRECT_RATE"].ToString() + "%";

                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][1] = string.Empty;
                    //		dt.Rows[nRowIdx][2] = string.Empty;
                    //		dt.Rows[nRowIdx][3] = string.Empty;
                    //		dt.Rows[nRowIdx][4] = string.Empty;
                    //		dt.Rows[nRowIdx][5] = string.Empty;
                    //		dt.Rows[nRowIdx][6] = string.Empty;
                    //	}

                    //	if (Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) > iStandard_AEDE1 ||
                    //		Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) == 0)
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState1\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState2\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //}
                    //else if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() == "BPA" &&
                    //		p_ds.Tables[0].Rows[i]["LINE_CD"].ToString() == "08")
                    //{
                    //	nRowIdx = 3;

                    //	if (Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString()) > 0)
                    //	{
                    //		dt.Rows[nRowIdx][1] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString())).ToString();
                    //		dt.Rows[nRowIdx][2] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["TARGET_QTY"].ToString())).ToString();
                    //		dt.Rows[nRowIdx][3] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["COMP_QTY"].ToString())).ToString();
                    //		dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["DIRECT_RATE"].ToString() + "%";

                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][1] = string.Empty;
                    //		dt.Rows[nRowIdx][2] = string.Empty;
                    //		dt.Rows[nRowIdx][3] = string.Empty;
                    //		dt.Rows[nRowIdx][4] = string.Empty;
                    //		dt.Rows[nRowIdx][5] = string.Empty;
                    //		dt.Rows[nRowIdx][6] = string.Empty;
                    //	}

                    //	if (Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) > iStandard_AEDE2 ||
                    //		Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) == 0)
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState1\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState2\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //}
                    //else if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() == "BPA" && 
                    //		p_ds.Tables[0].Rows[i]["LINE_CD"].ToString() == "01")
                    //{
                    //	nRowIdx = 4;
                    //	dt.Rows[nRowIdx][1] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][2] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["TARGET_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][3] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["COMP_QTY"].ToString())).ToString();

                    //	if (Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString()) > 0)
                    //	{
                    //		dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["DIRECT_RATE"].ToString() + "%";

                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][4] = string.Empty;
                    //		dt.Rows[nRowIdx][5] = string.Empty;
                    //		dt.Rows[nRowIdx][6] = string.Empty;
                    //	}

                    //	if (Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) > iStandard_YF1 ||
                    //		Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) == 0)
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState1\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState2\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //}
                    //else if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() == "BPA" &&
                    //		p_ds.Tables[0].Rows[i]["LINE_CD"].ToString() == "05")
                    //{
                    //	nRowIdx = 5;
                    //	dt.Rows[nRowIdx][1] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][2] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["TARGET_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][3] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["COMP_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                    //	dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";
                    //	dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["DIRECT_RATE"].ToString() + "%";

                    //	if (Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString()) > 0)
                    //	{
                    //		dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["DIRECT_RATE"].ToString() + "%";

                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][4] = string.Empty;
                    //		dt.Rows[nRowIdx][5] = string.Empty;
                    //		dt.Rows[nRowIdx][6] = string.Empty;
                    //	}

                    //	if (Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) > iStandard_PHEV ||
                    //		Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) == 0)
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState1\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState2\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //}
                    //else if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() == "CMA" &&
                    //		p_ds.Tables[0].Rows[i]["LINE_CD"].ToString() == "09")
                    //{
                    //	nRowIdx = 6;
                    //	dt.Rows[nRowIdx][1] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][2] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["TARGET_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][3] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["COMP_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                    //	dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";
                    //	dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["DIRECT_RATE"].ToString() + "%";

                    //	if (Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString()) > 0)
                    //	{
                    //		dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["DIRECT_RATE"].ToString() + "%";

                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][4] = string.Empty;
                    //		dt.Rows[nRowIdx][5] = string.Empty;
                    //		dt.Rows[nRowIdx][6] = string.Empty;
                    //	}

                    //	if (Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) > iStandard_12V ||
                    //		Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) == 0)
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState1\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState2\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //}
                    //else if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() == "BPA" &&
                    //		p_ds.Tables[0].Rows[i]["LINE_CD"].ToString() == "10")
                    //{
                    //	nRowIdx = 7;
                    //	dt.Rows[nRowIdx][1] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][2] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["TARGET_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][3] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["COMP_QTY"].ToString())).ToString();
                    //	dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                    //	dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";
                    //	dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["DIRECT_RATE"].ToString() + "%";

                    //	if (Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString()) > 0)
                    //	{
                    //		dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";
                    //		dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["DIRECT_RATE"].ToString() + "%";

                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][4] = string.Empty;
                    //		dt.Rows[nRowIdx][5] = string.Empty;
                    //		dt.Rows[nRowIdx][6] = string.Empty;
                    //	}

                    //	if (Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) > iStandard_PHEV ||
                    //		Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) == 0)
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState1\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //	else
                    //	{
                    //		dt.Rows[nRowIdx][5] = "<span class=\"RateState2\">" + dt.Rows[nRowIdx][5].ToString() + "</span>";
                    //	}
                    //}
                    #endregion
                }

                // 30개 중에서 빈것은 지운다....
                for (int i = 30; i > 0; i--)
                {
                    if (dt.Rows[i - 1][0].ToString() == null || dt.Rows[i - 1][0].ToString().Length == 0)
                    {
                        dt.Rows.RemoveAt(i - 1);
                    }
                }

                // 설정값이 이상한 데이터를 보여줄 데이터에 추가한다.
                if (dt2.Rows.Count > 0)
                {
                    DataRow[] drTemp = dt2.Select();
                    foreach (DataRow row in drTemp)
                    {
                        //object[] obj = row.ItemArray;
                        dt.Rows.Add(row.ItemArray);
                    }
                }
            }
            ds.Tables.Add(dt);
            return ds;
        }
        #endregion

        #region 라인현황판 데이터 편집
        private DataTable EditLineDispData(DataSet p_ds, string shop1, string line1, string shop2, string line2)
        {
            int nRowIdx = 0;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(shop1 + line1 + shop2 + line2);
            DateTime MyDateTime = DateTime.Now;
            string[] strObj = new String[] { string.Empty, string.Empty, string.Empty, string.Empty };
            dt.Columns.Add("c1", System.Type.GetType("System.String"));
            dt.Columns.Add("c2", System.Type.GetType("System.String"));
            dt.Columns.Add("c3", System.Type.GetType("System.String"));
            dt.Columns.Add("c4", System.Type.GetType("System.String"));
            //dt.Columns.Add("CurrDate");
            //dt.Columns.Add("CurrTime");
            dt.Rows.Add(strObj);
            dt.Rows.Add(strObj);
            dt.Rows.Add(strObj);
            dt.Rows.Add(strObj);
            dt.Rows.Add(strObj);
            dt.Rows.Add(strObj);

            try
            {
                string strLineTitle = ConfigurationManager.AppSettings.Get(shop1 + ";" + line1 + ";" + shop2 + ";" + line2);
                if (strLineTitle != null && strLineTitle.Length > 0)
                {
                    string[] arrLineTitle = strLineTitle.Split(';');
                    if (arrLineTitle != null && arrLineTitle.Length > 0)
                    {
                        dt.Rows[0][1] = arrLineTitle[0];
                        dt.Rows[0][0] = arrLineTitle[0];

                        if (arrLineTitle.Length > 1)
                        {
                            dt.Rows[0][2] = arrLineTitle[1];
                            dt.Rows[0][3] = arrLineTitle[1];
                        }
                        else
                        {
                            dt.Rows[0][2] = "UNKNOWN";
                            dt.Rows[0][3] = "UNKNOWN";
                        }
                    }
                    else
                    {
                        dt.Rows[0][1] = "UNKNOWN";
                        dt.Rows[0][0] = "UNKNOWN";
                        dt.Rows[0][2] = "UNKNOWN";
                        dt.Rows[0][3] = "UNKNOWN";
                    }
                }
                else
                {
                    dt.Rows[0][1] = "UNKNOWN";
                    dt.Rows[0][0] = "UNKNOWN";
                    dt.Rows[0][2] = "UNKNOWN";
                    dt.Rows[0][3] = "UNKNOWN";
                }
            }
            catch
            {
                dt.Rows[0][1] = "UNKNOWN";
                dt.Rows[0][0] = "UNKNOWN";
                dt.Rows[0][2] = "UNKNOWN";
                dt.Rows[0][3] = "UNKNOWN";
            }


            //if (strMid == "PDP02")
            //{   //1층
            //	dt.Rows[0][1] = "JF HEV";
            //	dt.Rows[0][0] = "JF HEV";
            //	dt.Rows[0][2] = "LF HEV";
            //	dt.Rows[0][3] = "LF HEV";

            //}
            //else if (strMid == "PDP05")
            //{   //2층
            //	dt.Rows[0][0] = "HG/VG";
            //	dt.Rows[0][1] = "HG/VG";
            //	dt.Rows[0][2] = "LF PHEV";
            //	dt.Rows[0][3] = "LF PHEV";
            //}
            //else if (strMid == "PDP06")
            //{   //1층 AEDE
            //	dt.Rows[0][0] = "AE/DE HEV2";
            //	dt.Rows[0][1] = "AE/DE HEV2";
            //	dt.Rows[0][2] = "AE/DE HEV1";
            //	dt.Rows[0][3] = "AE/DE HEV1";
            //}
            //else if (strMid == "PDP07")
            //{   //2층 AEEV
            //	dt.Rows[0][0] = "AE EV";
            //	dt.Rows[0][1] = "AE EV";
            //	dt.Rows[0][2] = "JF PHEV W/G";
            //	dt.Rows[0][3] = "JF PHEV W/G";
            //}

            if (p_ds != null && p_ds.Tables.Count > 0)
            {
                for (int i = 0; i < p_ds.Tables[0].Rows.Count; i++)
                {
                    if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() == shop1 &&
                      p_ds.Tables[0].Rows[i]["LINE_CD"].ToString() == line1)
                    {
                        dt.Rows[2][0] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["BUSINESS_PLAN"].ToString())).ToString();
                        dt.Rows[2][1] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString())).ToString();
                        dt.Rows[3][0] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["M_COUNT"].ToString())).ToString();
                        dt.Rows[3][1] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["COMP_QTY"].ToString())).ToString();

                        if (p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString().Trim() != "0")
                        {
                            dt.Rows[4][0] = p_ds.Tables[0].Rows[i]["M_ACHIEVE_RATE"].ToString() + "%";
                            dt.Rows[4][1] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                            dt.Rows[5][1] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";

                        }
                        else
                        {
                            dt.Rows[4][0] = "";
                            dt.Rows[4][1] = "";
                            dt.Rows[5][1] = "";
                        }

                        if (strVM == "0")
                        {
                            if (Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) > iStandardLine1 || Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) == 0)
                            {
                                dt.Rows[5][1] = "<span class=\"RateState1\">" + dt.Rows[5][1].ToString() + "</span>";
                            }
                            else
                            {
                                dt.Rows[5][1] = "<span class=\"RateState2\">" + dt.Rows[5][1].ToString() + "</span>";

                            }
                        }
                    }
                    else if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() == shop2 &&
                      p_ds.Tables[0].Rows[i]["LINE_CD"].ToString() == line2)
                    {
                        dt.Rows[2][2] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["BUSINESS_PLAN"].ToString())).ToString();
                        dt.Rows[2][3] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString())).ToString();
                        dt.Rows[3][2] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["M_COUNT"].ToString())).ToString();
                        dt.Rows[3][3] = String.Format("{0:#,###}", Convert.ToInt32(p_ds.Tables[0].Rows[i]["COMP_QTY"].ToString())).ToString();

                        if (p_ds.Tables[0].Rows[i]["PLAN_QTY"].ToString().Trim() != "0")
                        {
                            dt.Rows[4][2] = p_ds.Tables[0].Rows[i]["M_ACHIEVE_RATE"].ToString() + "%";
                            dt.Rows[4][3] = p_ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "%";
                            dt.Rows[5][3] = p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "%";

                        }
                        else
                        {
                            dt.Rows[4][2] = "";
                            dt.Rows[5][3] = "";
                            dt.Rows[5][3] = "";
                        }

                        if (strVM == "0")
                        {
                            if (Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) > iStandardLine2 || Convert.ToDouble(p_ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString()) == 0)
                            {
                                dt.Rows[5][3] = "<span class=\"RateState1\">" + dt.Rows[5][3].ToString() + "</span>";
                            }
                            else
                            {
                                dt.Rows[5][3] = "<span class=\"RateState2\">" + dt.Rows[5][3].ToString() + "</span>";

                            }
                        }
                    }
                }
            }
            //ds.Tables.Add(dt);
            return dt;
        }
        #endregion

        #region 공정 모니터링 편집
        private DataSet EditDispData_StationMonitor(DataSet p_ds)
        {
            int nRowIdx = 0;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DateTime MyDateTime = DateTime.Now;
            string[] strObj = new String[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, String.Format("{0:yyyy'/'MM'/'dd ddd}", MyDateTime) + "요일", String.Format("{0:HH:mm:ss}", MyDateTime) };
            dt.Columns.Add("c1", System.Type.GetType("System.String"));
            dt.Columns.Add("SHOP_CD", System.Type.GetType("System.String"));
            dt.Columns.Add("LINE_CD", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_CD", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_NAME", System.Type.GetType("System.String"));
            dt.Columns.Add("PD_YN", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("STD_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("ARM_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("ESTOP_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_STATUS", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_STATUS_NAME", System.Type.GetType("System.String"));
            dt.Columns.Add("Qty", System.Type.GetType("System.String"));
            dt.Columns.Add("CurrDate", System.Type.GetType("System.String"));
            dt.Columns.Add("CurrTime", System.Type.GetType("System.String"));


            // 현황판 데이터 저장할 공간을 30개만 먼저 만든다.
            for (int i = 0; i < 500; i++) dt.Rows.Add(strObj);

            if (p_ds != null && p_ds.Tables.Count > 1)
            {
                for (int i = 0; i < p_ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() != "")
                        {
                            int k = i + 1;
                            nRowIdx++;
                            dt.Rows[nRowIdx][0] = k;
                            dt.Rows[nRowIdx][1] = p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString();
                            dt.Rows[nRowIdx][2] = p_ds.Tables[0].Rows[i]["LINE_CD"].ToString();
                            dt.Rows[nRowIdx][3] = p_ds.Tables[0].Rows[i]["STN_CD"].ToString();
                            dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["STN_NAME"].ToString();
                            dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["PD_YN"].ToString();
                            dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["STN_CT"].ToString();
                            dt.Rows[nRowIdx][7] = p_ds.Tables[0].Rows[i]["STD_CT"].ToString();
                            dt.Rows[nRowIdx][8] = p_ds.Tables[0].Rows[i]["ARM_CT"].ToString();
                            dt.Rows[nRowIdx][9] = p_ds.Tables[0].Rows[i]["ESTOP_CT"].ToString();
                            dt.Rows[nRowIdx][10] = p_ds.Tables[0].Rows[i]["STN_STATUS"].ToString();
                            dt.Rows[nRowIdx][11] = p_ds.Tables[0].Rows[i]["STN_STATUS_NAME"].ToString();

                            if (p_ds.Tables[1].Rows.Count > 0 && nRowIdx == 1)
                            {
                                dt.Rows[nRowIdx][12] = p_ds.Tables[1].Rows[0]["qty"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }



                // 30개 중에서 빈것은 지운다....
                for (int i = 500; i > 0; i--)
                {
                    if (dt.Rows[i - 1][0].ToString() == null || dt.Rows[i - 1][0].ToString().Length == 0)
                    {
                        dt.Rows.RemoveAt(i - 1);
                    }
                }
            }
            ds.Tables.Add(dt);
            return ds;
        }
        #endregion
        private DataSet EditDispData_StationMonitor2(DataSet p_ds)
        {
            int nRowIdx = 0;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DateTime MyDateTime = DateTime.Now;
            string[] strObj = new String[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, String.Format("{0:yyyy'/'MM'/'dd ddd}", MyDateTime) + "요일", String.Format("{0:HH:mm:ss}", MyDateTime) };
            dt.Columns.Add("c1", System.Type.GetType("System.String"));
            dt.Columns.Add("SHOP_CD", System.Type.GetType("System.String"));
            dt.Columns.Add("LINE_CD", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_CD", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_NAME", System.Type.GetType("System.String"));
            dt.Columns.Add("PD_YN", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("STD_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("ARM_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("ESTOP_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_STATUS", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_STATUS_NAME", System.Type.GetType("System.String"));
            dt.Columns.Add("Qty", System.Type.GetType("System.String"));
            dt.Columns.Add("CurrDate", System.Type.GetType("System.String"));
            dt.Columns.Add("CurrTime", System.Type.GetType("System.String"));


            // 현황판 데이터 저장할 공간을 30개만 먼저 만든다.
            for (int i = 0; i < 500; i++) dt.Rows.Add(strObj);

            if (p_ds != null && p_ds.Tables.Count > 1)
            {
                for (int i = 0; i < p_ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() != "")
                        {
                            int k = i + 1;
                            nRowIdx++;
                            dt.Rows[nRowIdx][0] = k;
                            dt.Rows[nRowIdx][1] = p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString();
                            dt.Rows[nRowIdx][2] = p_ds.Tables[0].Rows[i]["LINE_CD"].ToString();
                            dt.Rows[nRowIdx][3] = p_ds.Tables[0].Rows[i]["STN_CD"].ToString();
                            dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["STN_NAME"].ToString();
                            dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["PD_YN"].ToString();
                            dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["STN_CT"].ToString();
                            dt.Rows[nRowIdx][7] = p_ds.Tables[0].Rows[i]["STD_CT"].ToString();
                            dt.Rows[nRowIdx][8] = p_ds.Tables[0].Rows[i]["ARM_CT"].ToString();
                            dt.Rows[nRowIdx][9] = p_ds.Tables[0].Rows[i]["ESTOP_CT"].ToString();
                            dt.Rows[nRowIdx][10] = p_ds.Tables[0].Rows[i]["STN_STATUS"].ToString();
                            dt.Rows[nRowIdx][11] = p_ds.Tables[0].Rows[i]["STN_STATUS_NAME"].ToString();

                            if (p_ds.Tables[1].Rows.Count > 0 && nRowIdx == 1)
                            {

                                dt.Rows[nRowIdx][12] = p_ds.Tables[1].Rows[0]["qty"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }



                // 30개 중에서 빈것은 지운다....
                for (int i = 500; i > 0; i--)
                {
                    if (dt.Rows[i - 1][0].ToString() == null || dt.Rows[i - 1][0].ToString().Length == 0)
                    {
                        dt.Rows.RemoveAt(i - 1);
                    }
                }
            }
            ds.Tables.Add(dt);
            return ds;
        }

        private DataSet EditDispData_StationMonitor3(DataSet p_ds)
        {
            int nRowIdx = 0;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DateTime MyDateTime = DateTime.Now;
            string[] strObj = new String[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, String.Format("{0:yyyy'/'MM'/'dd ddd}", MyDateTime) + "요일", String.Format("{0:HH:mm:ss}", MyDateTime) };
            dt.Columns.Add("c1", System.Type.GetType("System.String"));
            dt.Columns.Add("SHOP_CD", System.Type.GetType("System.String"));
            dt.Columns.Add("LINE_CD", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_CD", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_NAME", System.Type.GetType("System.String"));
            dt.Columns.Add("PD_YN", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("STD_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("ARM_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("ESTOP_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_STATUS", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_STATUS_NAME", System.Type.GetType("System.String"));
            dt.Columns.Add("Qty", System.Type.GetType("System.String"));
            dt.Columns.Add("Qty1", System.Type.GetType("System.String"));
            dt.Columns.Add("CurrDate", System.Type.GetType("System.String"));
            dt.Columns.Add("CurrTime", System.Type.GetType("System.String"));


            // 현황판 데이터 저장할 공간을 30개만 먼저 만든다.
            for (int i = 0; i < 500; i++) dt.Rows.Add(strObj);

            if (p_ds != null && p_ds.Tables.Count > 1)
            {
                for (int i = 0; i < p_ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() != "")
                        {
                            int k = i + 1;
                            nRowIdx++;
                            dt.Rows[nRowIdx][0] = k;
                            dt.Rows[nRowIdx][1] = p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString();
                            dt.Rows[nRowIdx][2] = p_ds.Tables[0].Rows[i]["LINE_CD"].ToString();
                            dt.Rows[nRowIdx][3] = p_ds.Tables[0].Rows[i]["STN_CD"].ToString();
                            dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["STN_NAME"].ToString();
                            dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["PD_YN"].ToString();
                            dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["STN_CT"].ToString();
                            dt.Rows[nRowIdx][7] = p_ds.Tables[0].Rows[i]["STD_CT"].ToString();
                            dt.Rows[nRowIdx][8] = p_ds.Tables[0].Rows[i]["ARM_CT"].ToString();
                            dt.Rows[nRowIdx][9] = p_ds.Tables[0].Rows[i]["ESTOP_CT"].ToString();
                            dt.Rows[nRowIdx][10] = p_ds.Tables[0].Rows[i]["STN_STATUS"].ToString();
                            dt.Rows[nRowIdx][11] = p_ds.Tables[0].Rows[i]["STN_STATUS_NAME"].ToString();

                            if (p_ds.Tables[1].Rows.Count > 0 && nRowIdx == 1)
                            {

                                dt.Rows[nRowIdx][12] = p_ds.Tables[1].Rows[0]["qty"].ToString();
                            }
                            if (p_ds.Tables[2].Rows.Count > 0 && nRowIdx == 1)
                            {

                                dt.Rows[nRowIdx][13] = p_ds.Tables[2].Rows[0]["qty"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }



                // 30개 중에서 빈것은 지운다....
                for (int i = 500; i > 0; i--)
                {
                    if (dt.Rows[i - 1][0].ToString() == null || dt.Rows[i - 1][0].ToString().Length == 0)
                    {
                        dt.Rows.RemoveAt(i - 1);
                    }
                }
            }
            ds.Tables.Add(dt);
            return ds;
        }

        private DataSet EditDispData_StationMonitor4(DataSet p_ds)
        {
            int nRowIdx = 0;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            DateTime MyDateTime = DateTime.Now;
            string[] strObj = new String[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, String.Format("{0:yyyy'/'MM'/'dd ddd}", MyDateTime) + "요일", String.Format("{0:HH:mm:ss}", MyDateTime) };
            dt.Columns.Add("c1", System.Type.GetType("System.String"));
            dt.Columns.Add("SHOP_CD", System.Type.GetType("System.String"));
            dt.Columns.Add("LINE_CD", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_CD", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_NAME", System.Type.GetType("System.String"));
            dt.Columns.Add("PD_YN", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("STD_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("ARM_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("ESTOP_CT", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_STATUS", System.Type.GetType("System.String"));
            dt.Columns.Add("STN_STATUS_NAME", System.Type.GetType("System.String"));
            dt.Columns.Add("Qty", System.Type.GetType("System.String"));
            //dt.Columns.Add("Qty1", System.Type.GetType("System.String"));
            dt.Columns.Add("CurrDate", System.Type.GetType("System.String"));
            dt.Columns.Add("CurrTime", System.Type.GetType("System.String"));


            // 현황판 데이터 저장할 공간을 30개만 먼저 만든다.
            for (int i = 0; i < 500; i++) dt.Rows.Add(strObj);

            if (p_ds != null && p_ds.Tables.Count > 1)
            {
                for (int i = 0; i < p_ds.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        if (p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString() != "")
                        {
                            int k = i + 1;
                            nRowIdx++;
                            dt.Rows[nRowIdx][0] = k;
                            dt.Rows[nRowIdx][1] = p_ds.Tables[0].Rows[i]["SHOP_CD"].ToString();
                            dt.Rows[nRowIdx][2] = p_ds.Tables[0].Rows[i]["LINE_CD"].ToString();
                            dt.Rows[nRowIdx][3] = p_ds.Tables[0].Rows[i]["STN_CD"].ToString();
                            dt.Rows[nRowIdx][4] = p_ds.Tables[0].Rows[i]["STN_NAME"].ToString();
                            dt.Rows[nRowIdx][5] = p_ds.Tables[0].Rows[i]["PD_YN"].ToString();
                            dt.Rows[nRowIdx][6] = p_ds.Tables[0].Rows[i]["STN_CT"].ToString();
                            dt.Rows[nRowIdx][7] = p_ds.Tables[0].Rows[i]["STD_CT"].ToString();
                            dt.Rows[nRowIdx][8] = p_ds.Tables[0].Rows[i]["ARM_CT"].ToString();
                            dt.Rows[nRowIdx][9] = p_ds.Tables[0].Rows[i]["ESTOP_CT"].ToString();
                            dt.Rows[nRowIdx][10] = p_ds.Tables[0].Rows[i]["STN_STATUS"].ToString();
                            dt.Rows[nRowIdx][11] = p_ds.Tables[0].Rows[i]["STN_STATUS_NAME"].ToString();

                            if (p_ds.Tables[1].Rows.Count > 0 && nRowIdx == 1)
                            {

                                dt.Rows[nRowIdx][12] = p_ds.Tables[1].Rows[0]["qty"].ToString();
                            }
                            //if (p_ds.Tables[2].Rows.Count > 0 && nRowIdx == 1)
                            //{

                            //    dt.Rows[nRowIdx][13] = p_ds.Tables[2].Rows[0]["qty"].ToString();
                            //}
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }



                // 30개 중에서 빈것은 지운다....
                for (int i = 500; i > 0; i--)
                {
                    if (dt.Rows[i - 1][0].ToString() == null || dt.Rows[i - 1][0].ToString().Length == 0)
                    {
                        dt.Rows.RemoveAt(i - 1);
                    }
                }
            }
            ds.Tables.Add(dt);
            return ds;
        }

        public void GetMonitorInfo(string strMonId)
        {
            DataSet ds = new DataSet();
            bool bResult = true;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string strReturn = string.Empty;
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@P_MONI_ID", strMonId);
                bResult = GetDataSet(strLinkedServer + "SP_PIB_GET_MONITOR_INFO", param, CommandType.StoredProcedure, ref ds, ref m_strErrCode, ref m_strErrText);
                strReturn = jss.Serialize(GetPibList(ds));

                UpdateHeartBit(strMonId, null, strClientAddr);
            }
            catch
            {

            }
            finally
            {
                ds.Dispose();
                ds = null;
                // 결과를 리턴한다.
                Response.ContentType = "text/plain";
                Response.Write(strReturn);
                Response.End();
            }
        }

        #region json문자열 변환 메소드
        public System.Collections.Generic.List<Hashtable> GetList(DataSet p_ds)
        {
            DataRow[] drTemp = p_ds.Tables[0].Select();
            List<Hashtable> lRows = new List<Hashtable>();
            foreach (DataRow row in drTemp)
            {
                Hashtable htCols = new Hashtable();
                foreach (DataColumn col in p_ds.Tables[0].Columns)
                {
                    htCols.Add(col.ColumnName, row[col]);
                }
                lRows.Add(htCols);
            }
            return lRows;
        }
        public System.Collections.Generic.List<Hashtable> GetList2(DataSet p_ds)
        {
            List<Hashtable> lTables = new List<Hashtable>();
            if (p_ds != null && p_ds.Tables.Count > 0)
            {
                foreach (DataTable table in p_ds.Tables)
                {
                    Hashtable htTables = new Hashtable();
                    if (htTables.Count < 1)
                    {
                        htTables.Add("CurrDate", String.Format("{0:yyyy'/'MM'/'dd ddd}", DateTime.Now) + "요일");
                        htTables.Add("CurrTime", String.Format("{0:HH:mm:ss}", DateTime.Now));
                        htTables.Add("TableName", table.TableName);
                    }
                    DataRow[] drTemp = table.Select();
                    List<Hashtable> lRows = new List<Hashtable>();
                    foreach (DataRow row in drTemp)
                    {
                        Hashtable htCols = new Hashtable();
                        foreach (DataColumn col in table.Columns)
                        {
                            htCols.Add(col.ColumnName, row[col]);
                        }
                        lRows.Add(htCols);
                    }

                    htTables.Add("Data", lRows);
                    lTables.Add(htTables);
                }
            }
            else
            {
                Hashtable htTables = new Hashtable();
                htTables.Add("CurrDate", String.Format("{0:yyyy'/'MM'/'dd ddd}", DateTime.Now) + "요일");
                htTables.Add("CurrTime", String.Format("{0:HH:mm:ss}", DateTime.Now));
                htTables.Add("TableName", "XXXXXXXXX");
                lTables.Add(htTables);
            }
            return lTables;
        }
        public System.Collections.Generic.List<Hashtable> GetList2(Hashtable p_ds)
        {
            List<Hashtable> lTables = new List<Hashtable>();
            if (p_ds != null && p_ds.Count > 0)
            {
                foreach (DictionaryEntry entry in p_ds)
                {
                    DataTable table = (DataTable)entry.Value;
                    Hashtable htTables = new Hashtable();
                    if (htTables.Count < 1)
                    {
                        htTables.Add("CurrDate", String.Format("{0:yyyy'/'MM'/'dd ddd}", DateTime.Now) + "요일");
                        htTables.Add("CurrTime", String.Format("{0:HH:mm:ss}", DateTime.Now));
                        htTables.Add("TableName", table.TableName);
                    }
                    DataRow[] drTemp = table.Select();
                    List<Hashtable> lRows = new List<Hashtable>();
                    foreach (DataRow row in drTemp)
                    {
                        Hashtable htCols = new Hashtable();
                        foreach (DataColumn col in table.Columns)
                        {
                            htCols.Add(col.ColumnName, row[col]);
                        }
                        lRows.Add(htCols);
                    }

                    htTables.Add("Data", lRows);
                    lTables.Add(htTables);
                }
            }
            else
            {
                Hashtable htTables = new Hashtable();
                htTables.Add("CurrDate", String.Format("{0:yyyy'/'MM'/'dd ddd}", DateTime.Now) + "요일");
                htTables.Add("CurrTime", String.Format("{0:HH:mm:ss}", DateTime.Now));
                htTables.Add("TableName", "XXXXXXXXX");
                lTables.Add(htTables);
            }
            return lTables;
        }
        public System.Collections.Generic.List<Hashtable> GetPibList(DataSet p_ds)
        {
            List<Hashtable> lTables = new List<Hashtable>();
            foreach (DataTable table in p_ds.Tables)
            {
                Hashtable htTables = new Hashtable();
                if (htTables.Count < 1)
                {
                    htTables.Add("CurrDate", String.Format("{0:yyyy'/'MM'/'dd ddd}", DateTime.Now) + "요일");
                    htTables.Add("CurrTime", String.Format("{0:HH:mm:ss}", DateTime.Now));
                    htTables.Add("TableName", table.TableName);
                }
                DataRow[] drTemp = table.Select();
                List<Hashtable> lRows = new List<Hashtable>();
                foreach (DataRow row in drTemp)
                {
                    Hashtable htCols = new Hashtable();
                    foreach (DataColumn col in table.Columns)
                    {
                        htCols.Add(col.ColumnName, row[col]);
                        if (col.ColumnName == "RESTART_FLAG")
                        {
                            htTables[col.ColumnName] = row[col];
                        }
                    }
                    lRows.Add(htCols);
                }

                htTables.Add("Data", lRows);
                lTables.Add(htTables);
            }
            return lTables;
        }
        #endregion
    }
}