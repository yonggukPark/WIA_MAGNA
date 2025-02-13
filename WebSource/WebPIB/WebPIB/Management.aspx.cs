using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WebPIB
{
	public partial class Management : PageBase
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			DataSet ds = new DataSet();
			try
			{
				if (!IsPostBack)
				{
					ds = GetMonitor(string.Empty);
					DisplayMonitorList(ds);
				}
			}
			catch
			{

			}
			finally
			{
				ds.Dispose();
				ds = null;
			}
		}

		protected void timerMonitor_Tick(object sender, EventArgs e)
		{
            DataSet ds = new DataSet();
            try
            {
                ds = GetMonitor(string.Empty);
                DisplayMonitorList(ds);
            }
            catch
            {

            }
            finally
            {
                ds.Dispose();
                ds = null;
                //GC.Collect();
            }
        }

		protected void btnRestartMonitor_Click(object sender, EventArgs e)
		{
			m_DBTransaction = true;
			DataSet ds = new DataSet();
			try
			{
				// 모니터 변경정보 저장
				RestartMonitor();
				this.hfMonId_Old.Value = this.tbMonId.Text;
				// 모니터 리스트 다시조회
				ds = GetMonitor(string.Empty);
				DisplayMonitorList(ds);
			}
			catch
			{

			}
			finally
			{
				ds.Dispose();
				ds = null;
				m_DBTransaction = false;
			}

		}
		protected void btnSearchMonitor_Click(object sender, EventArgs e)
		{
			DataSet ds = new DataSet();
			try
			{
				ds = GetMonitor(this.hfMonId_Old.Value);


				if(ds != null && ds.Tables.Count > 0)
				{
					foreach (DataRow row in ds.Tables[0].Rows)
					{
						this.hfMonId_Old.Value = row["MONI_ID"].ToString();
						this.tbMonId.Text = row["MONI_ID"].ToString();
						this.tbMonIp.Text = row["MONI_IP"].ToString();
						this.tbMonDesc.Text = row["MONI_DESC"].ToString();
						this.tbMonMsg.Text = row["MSG"].ToString();
					}

					if (ds.Tables.Count > 1)
					{
						for (int i = this.tblPibEdit.Rows.Count - 1; i >= 1; i--)
						{
							this.tblPibEdit.Rows.RemoveAt(i);
						}

						foreach (DataRow row in ds.Tables[1].Rows)
						{
							this.tblPibEdit.Rows.Add(new HtmlTableRow());
							this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells.Add(new HtmlTableCell());
							this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells.Add(new HtmlTableCell());
							this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells.Add(new HtmlTableCell());
							this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells.Add(new HtmlTableCell());

							//HtmlInputCheckBox hicb = new HtmlInputCheckBox();
							//hicb.Name = "cbDISP_CHK";
							//HtmlInputHidden hih = new HtmlInputHidden();
							//hih.Name = "hfDISP_SEQ";
							//hih.Value = row["DISP_SEQ"].ToString();
							//this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells[0].Controls.Add(hicb);
							//this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells[0].Controls.Add(hih);
							this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells[0].Style[HtmlTextWriterStyle.TextAlign] = "center";
							this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells[0].InnerHtml
								= "<input type='checkbox' name='cbDISP_CHK'>"
								+ "<input type='hidden' name='hfDISP_SEQ' value='" + row["DISP_SEQ"].ToString() + "'>";

							//HtmlInputText hit = new HtmlInputText("text");
							//hit.Style[HtmlTextWriterStyle.TextAlign] = "center";
							//hit.Name = "tbDISP_SEQ";
							//hit.Size = 2;
							//hit.MaxLength = 2;
							//hit.Value = row["DISP_SEQ"].ToString();
							//this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells[1].Controls.Add(hit);
							this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells[1].Style[HtmlTextWriterStyle.TextAlign] = "center";
							this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells[1].InnerHtml
								= "<input type='text' name='tbDISP_SEQ' size='2' maxlength='2' value='" + row["DISP_SEQ"].ToString() + "' style='text-align:center;'>";


							//hit = new HtmlInputText("text");
							//hit.Name = "tbURL";
							//hit.Size = 100;
							//hit.MaxLength = 100;
							//hit.Value = row["PIB_URL"].ToString();
							//this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells[2].Controls.Add(hit);
							this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells[2].Style[HtmlTextWriterStyle.TextAlign] = "center";
							this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells[2].InnerHtml
								= "<input type='text' name='tbURL' size='100' maxlength='100' value='" + row["PIB_URL"].ToString() + "' style='width:100%'>";

							//hit = new HtmlInputText("text");
							//hit.Style[HtmlTextWriterStyle.TextAlign] = "center";
							//hit.Name = "tbALIVE_TM";
							//hit.Size = 2;
							//hit.MaxLength = 2;
							//hit.Value = row["ALIVE_TM"].ToString();
							//this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells[3].Controls.Add(hit);
							this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells[3].Style[HtmlTextWriterStyle.TextAlign] = "center";
							this.tblPibEdit.Rows[this.tblPibEdit.Rows.Count - 1].Cells[3].InnerHtml
								= "<input type='text' name='tbALIVE_TM' size='2' maxlength='2' value='" + row["ALIVE_TM"].ToString() + "' style='text-align:center;'>";
						}
					}
				}


			}
			catch
			{

			}
			finally
			{
				ds.Dispose();
				ds = null;
				GC.Collect();
				m_DBTransaction = false;
			}
		}

		protected void btnAddMonitor_Click(object sender, EventArgs e)
		{
			m_DBTransaction = false;
			DataSet ds = new DataSet();
			try
			{
				// 모니터 변경정보 저장
				SetMonitor("I");
				// 기존모니터 정보 변경
				this.hfMonId_Old.Value = this.tbMonId.Text;
				// 모니터 리스트 다시조회
				ds = GetMonitor(string.Empty);
				DisplayMonitorList(ds);
				btnSearchMonitor_Click(null, null);
			}
			catch
			{

			}
			finally
			{
				ds.Dispose();
				ds = null;
				GC.Collect();
				m_DBTransaction = false;
			}

		}

		protected void btnSaveMonitor_Click(object sender, EventArgs e)
		{
			m_DBTransaction = false;
			DataSet ds = new DataSet();
			try
			{
				// 모니터 변경정보 저장
				SetMonitor("S");
				// 기존모니터 정보 변경
				this.hfMonId_Old.Value = this.tbMonId.Text;
				// 모니터 리스트 다시조회
				ds = GetMonitor(string.Empty);
				DisplayMonitorList(ds);
				btnSearchMonitor_Click(null, null);
			}
			catch
			{

			}
			finally
			{
				ds.Dispose();
				ds = null;
				GC.Collect();
				m_DBTransaction = false;
			}

		}

		protected void btnDelMonitor_Click(object sender, EventArgs e)
		{
			m_DBTransaction = false;
			DataSet ds = new DataSet();
			try
			{
				// 모니터 정보 삭제
				SetMonitor("D");
				// 입력데이터 초기화
				this.hfMonId_Old.Value = string.Empty;
				this.tbMonId.Text = string.Empty;
				this.tbMonIp.Text = string.Empty;
				this.tbMonDesc.Text = string.Empty;
				this.tbMonMsg.Text = string.Empty;
				// 모니터 리스트 다시조회
				ds = GetMonitor(string.Empty);
				DisplayMonitorList(ds);
			}
			catch
			{

			}
			finally
			{
				ds.Dispose();
				ds = null;
				GC.Collect();
				m_DBTransaction = false;
			}
		}
		protected void btnSavePIB_Click(object sender, EventArgs e)
		{
			m_DBTransaction = false;
			try
			{
				string[] strDispSeq = Request.Params.GetValues("tbDISP_SEQ");
				string[] strUrl = Request.Params.GetValues("tbURL");
				string[] strAliveTime = Request.Params.GetValues("tbALIVE_TM");

				for (int i = 0; i < strDispSeq.Length; i++)
				{
					SetDisplay(i+1, strUrl[i], strAliveTime[i]);
				}

				btnSearchMonitor_Click(null, null);

			}
			catch
			{

			}
			finally
			{
				m_DBTransaction = false;
			}
		}
		

		public DataSet GetMonitor(string strMonId)
		{
			DataSet dsReturn = new DataSet();
			bool bResult = true;
			SqlParameter[] param = new SqlParameter[1];
			param[0] = new SqlParameter("@P_MONI_ID", strMonId);
			bResult = GetDataSet(strLinkedServer+"SP_PIB_GET_MONITOR", param, CommandType.StoredProcedure, ref dsReturn, ref m_strErrCode, ref m_strErrText);
			return dsReturn;
		}

		public void SetMonitor(string strWorkType)
		{
			bool bResult = true;
			SqlParameter[] param = new SqlParameter[6];
			param[0] = new SqlParameter("@P_WORK_TYPE", strWorkType);
			param[1] = new SqlParameter("@P_MONI_ID_OLD", this.hfMonId_Old.Value);
			param[2] = new SqlParameter("@P_MONI_ID", this.tbMonId.Text);
			param[3] = new SqlParameter("@P_MONI_IP", this.tbMonIp.Text);
			param[4] = new SqlParameter("@P_MONI_DESC", this.tbMonDesc.Text);
			param[5] = new SqlParameter("@P_MSG", this.tbMonMsg.Text);
			bResult = ExecuteNonQuery(strLinkedServer + "SP_PIB_SET_MONITOR", param, CommandType.StoredProcedure, ref m_strErrCode, ref m_strErrText);
		}

		public void RestartMonitor()
		{
			bool bResult = true;
			SqlParameter[] param = new SqlParameter[1];
			param[0] = new SqlParameter("@P_MONI_ID", this.tbMonId.Text);
			bResult = ExecuteNonQuery(strLinkedServer + "SP_PIB_SET_RESTART", param, CommandType.StoredProcedure, ref m_strErrCode, ref m_strErrText);
		}

		public void SetDisplay(int nDispSeq, string strPibUrl, string strAliveTime)
		{
			SqlParameter[] param = new SqlParameter[4];
			param[0] = new SqlParameter("@P_MONI_ID", this.tbMonId.Text);
			param[1] = new SqlParameter("@P_DISP_SEQ", nDispSeq);
			param[2] = new SqlParameter("@P_ALIVE_TM", strAliveTime);
			param[3] = new SqlParameter("@P_PIB_URL", strPibUrl);
			ExecuteNonQuery(strLinkedServer + "SP_PIB_SET_DISPLAY", param, CommandType.StoredProcedure, ref m_strErrCode, ref m_strErrText);
		}

		private void DisplayMonitorList(DataSet p_ds)
		{
			for (int i = this.tblMonitorStatus.Rows.Count - 1; i >= 1; i--)
			{
				this.tblMonitorStatus.Rows.RemoveAt(i);
			}
            
			if (p_ds != null && p_ds.Tables.Count > 0)
			{
				for (int i = 0; i < p_ds.Tables[0].Rows.Count; i++)
				{
					this.tblMonitorStatus.Rows.Add(new HtmlTableRow());
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells.Add(new HtmlTableCell());
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells.Add(new HtmlTableCell());
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells.Add(new HtmlTableCell());
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells.Add(new HtmlTableCell());
			
					// 상태이미지
					HtmlImage hi = new HtmlImage();                    
					hi.Style[HtmlTextWriterStyle.Width] = "16px";
					hi.Style[HtmlTextWriterStyle.Height] = "16px";
					hi.Style[HtmlTextWriterStyle.Cursor] = "pointer";
					hi.Attributes.Add("onclick", "OpenNewWindow('"+p_ds.Tables[0].Rows[i]["MONI_ID"].ToString()+"')");
					if (p_ds.Tables[0].Rows[i]["STATUS"].ToString() == "1")
					{
						hi.Src = "./Images/attention.png";
						this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[1].InnerHtml = "비정상";
					}
					else if (p_ds.Tables[0].Rows[i]["STATUS"].ToString() == "2")
					{
						hi.Src = "./Images/bonus.png";
						this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[1].InnerHtml = "중지됨";
					}
					else
					{
						hi.Src = "./Images/check.png";
						this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[1].InnerHtml = "정상";
					}

					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[0].Controls.Add(hi);
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[2].InnerHtml
						= string.Format("<a style=\"cursor:pointer;\" onclick=\"ClickMonitorId('{0}');\">{0}</a>", p_ds.Tables[0].Rows[i]["MONI_ID"].ToString());
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[3].InnerHtml = p_ds.Tables[0].Rows[i]["MONI_DESC"].ToString();
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[0].Style[HtmlTextWriterStyle.TextAlign] = "center";
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[1].Style[HtmlTextWriterStyle.TextAlign] = "center";
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[2].Style[HtmlTextWriterStyle.TextAlign] = "center";
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[3].Style[HtmlTextWriterStyle.TextAlign] = "left";
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[0].Style[HtmlTextWriterStyle.WhiteSpace] = "nowrap";
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[1].Style[HtmlTextWriterStyle.WhiteSpace] = "nowrap";
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[2].Style[HtmlTextWriterStyle.WhiteSpace] = "nowrap";
					this.tblMonitorStatus.Rows[this.tblMonitorStatus.Rows.Count - 1].Cells[3].Style[HtmlTextWriterStyle.WhiteSpace] = "nowrap";
				}
			}
		}
	}
}