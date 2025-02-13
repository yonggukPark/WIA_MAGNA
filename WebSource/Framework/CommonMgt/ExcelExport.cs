using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Security;
using System.Data;
using DevExpress.Web;
using ClosedXML.Excel;

namespace MES.FW.Common.CommonMgt
{
    public class ExcelExport : System.Web.UI.Page 
    {
		public ExcelExport()
        {

        }

		#region ExcelDownLoad
		public  void ExcelDownLoad(string strPageName, string strConTitle, string strConValue, DataSet ds, string[] arrColumnTitle,  bool bMultiRow, DataSet dsMultiTitle, string strFinalScript = null)
		{

            //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), "fn_gridCall();", true);

            // Make a WorkBook
            XLWorkbook XLWorkbook = new XLWorkbook();

			// Make a WorkSheet
			IXLWorksheet Worksheets = XLWorkbook.Worksheets.Add("Sheet1");

			int iColumn = ds.Tables[0].Columns.Count;
			int iRows = ds.Tables[0].Rows.Count;

			// Title Data Setting
			string strPageID = strPageName;

			IXLCell CellTitleText = Worksheets.Cell(1, 1);

			CellTitleText.Value = strPageID;
			CellTitleText.Style.Font.FontSize = 15;

			// Title Merge and Styling
			IXLRange RangeTitle = Worksheets.Range(1, 1, 1, iColumn);
			RangeTitle.Merge();
			RangeTitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
			RangeTitle.Style.Font.Bold = true;
			RangeTitle.Style.Font.FontColor = XLColor.Black;
			RangeTitle.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;

			string[] arrConTitle;
			string[] arrConValue;

			bool bCompare = false;

			arrConTitle = strConTitle.Split(',');
			arrConValue = strConValue.Split(',');

			// 검색 조건 타이틀과 검색 조건 값이 동일한지 체크
			if (arrConTitle.Length == arrConValue.Length)
			{
				bCompare = true;
			}

			int iCompare = 0;
			int iConditionStart = 2;
			string strCondition = string.Empty;

			IXLRange Range;

			for (int i = 0; i < arrConTitle.Length; i++)
			{
				iCompare++;

				if (bCompare)
				{
					// Condition Data Setting
					strCondition = arrConTitle[i].ToString() + " : " + arrConValue[i].ToString();

					// Condition End Marking
					strCondition += "#END#";

					// Remove ", " and EndMarking
					strCondition = strCondition.Replace(", #END#", "");
					strCondition = strCondition.Replace("#END#", "");

					// Make a Condition
					Worksheets.Cell(iConditionStart + i, 1).Value = strCondition;
					Range = Worksheets.Range(iConditionStart + i, 1, iConditionStart + i, iColumn);
					Range.Merge();
				}
				else
				{
					if (iCompare > arrConValue.Length)
					{
						// Condition Data Setting
						strCondition = arrConTitle[i].ToString() + " : " + "";

						// Condition End Marking
						strCondition += "#END#";

						// Remove ", " and EndMarking
						strCondition = strCondition.Replace(", #END#", "");
						strCondition = strCondition.Replace("#END#", "");

						// Make a Condition
						Worksheets.Cell(iConditionStart + i, 1).Value = strCondition;
						Range = Worksheets.Range(iConditionStart + i, 1, iConditionStart + i, iColumn);
						Range.Merge();

					}
					else
					{
						// Condition Data Setting
						strCondition = arrConTitle[i].ToString() + " : " + arrConValue[i].ToString();

						// Condition End Marking
						strCondition += "#END#";

						// Remove ", " and EndMarking
						strCondition = strCondition.Replace(", #END#", "");
						strCondition = strCondition.Replace("#END#", "");

						// Make a Condition
						Worksheets.Cell(iConditionStart + i, 1).Value = strCondition;
						Range = Worksheets.Range(iConditionStart + i, 1, iConditionStart + i, iColumn);
						Range.Merge();
					}
				}
			}

			int iHeaderStart = iConditionStart + iCompare;

			if (bMultiRow)
			{
				int iColStart = 0;
				int iColEnd = 0;

				for (int i = 0; i < dsMultiTitle.Tables[0].Rows.Count; i++) {

					if (i == 0)
					{
						iColStart = 1;
						iColEnd = Convert.ToInt32(dsMultiTitle.Tables[0].Rows[i]["CNT"].ToString());

						Worksheets.Cell(iHeaderStart, iColStart).Value = dsMultiTitle.Tables[0].Rows[i]["TITLE"].ToString();

						var RangeHeader = Worksheets.Range(iHeaderStart, iColStart, iHeaderStart, iColEnd);
						RangeHeader.Merge();
						RangeHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
						RangeHeader.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
						RangeHeader.Style.Font.Bold = true;
						RangeHeader.Style.Font.FontColor = XLColor.Black;
						RangeHeader.Style.Fill.BackgroundColor = XLColor.LightCornflowerBlue;
					}
					else {

						Worksheets.Cell(iHeaderStart, iColEnd + 1).Value = dsMultiTitle.Tables[0].Rows[i]["TITLE"].ToString();

						iColStart = iColEnd + 1;
						iColEnd += Convert.ToInt32(dsMultiTitle.Tables[0].Rows[i]["CNT"].ToString());

						var RangeHeader = Worksheets.Range(iHeaderStart, iColStart, iHeaderStart, iColEnd);
						RangeHeader.Merge();
						RangeHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
						RangeHeader.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
						RangeHeader.Style.Font.Bold = true;
						RangeHeader.Style.Font.FontColor = XLColor.Black;
						RangeHeader.Style.Fill.BackgroundColor = XLColor.LightCornflowerBlue;
					}
				}

                for (int i = 0; i < arrColumnTitle.Length; i++)
                {
                    Worksheets.Cell(iHeaderStart + 1, i + 1).Value = arrColumnTitle.ToString();

                    Worksheets.Column(i + 1).Width = 20;
                }


                var RangeHeader_Sub= Worksheets.Range(iHeaderStart + 1, 1, iHeaderStart + 1, iColumn);
				RangeHeader_Sub.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
				RangeHeader_Sub.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
				RangeHeader_Sub.Style.Font.Bold = true;
				RangeHeader_Sub.Style.Font.FontColor = XLColor.Black;
				RangeHeader_Sub.Style.Fill.BackgroundColor = XLColor.LightCornflowerBlue;


				int iDataRowStart = iHeaderStart + 1;

				for (int i = 0; i < iRows; i++)
				{
					for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
					{
						Worksheets.Cell(i + (iDataRowStart + 1), j + 1).Value = ds.Tables[0].Rows[i][j].ToString();
					}
				}

				// Out Line Set Border
				IXLRange RangeOutLine = Worksheets.Range(iHeaderStart, 1, iHeaderStart + iRows + 1, iColumn);
				RangeOutLine.Style.Border.TopBorder = XLBorderStyleValues.Thin;
				RangeOutLine.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
				RangeOutLine.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
				RangeOutLine.Style.Border.RightBorder = XLBorderStyleValues.Thick;
				RangeOutLine.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
			}
			else
			{
                for (int i = 0; i < arrColumnTitle.Length; i++) 
				{
					Worksheets.Cell(iHeaderStart, i + 1).Value = arrColumnTitle[i].ToString();

					Worksheets.Column(i + 1).Width = 20;
				}

				var RangeHeader = Worksheets.Range(iHeaderStart, 1, iHeaderStart, iColumn);
				RangeHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
				RangeHeader.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
				RangeHeader.Style.Font.Bold = true;
				RangeHeader.Style.Font.FontColor = XLColor.Black;
				RangeHeader.Style.Fill.BackgroundColor = XLColor.LightCornflowerBlue;


				int iDataRowStart = iHeaderStart;

				for (int i = 0; i < iRows; i++)
				{
					for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
					{
						Worksheets.Cell(i + (iDataRowStart + 1), j + 1).Value = ds.Tables[0].Rows[i][j].ToString();
					}
				}

				// Out Line Set Border
				IXLRange RangeOutLine = Worksheets.Range(iHeaderStart, 1, iHeaderStart + iRows, iColumn);
				RangeOutLine.Style.Border.TopBorder = XLBorderStyleValues.Thin;
				RangeOutLine.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
				RangeOutLine.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
				RangeOutLine.Style.Border.RightBorder = XLBorderStyleValues.Thick;
				RangeOutLine.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
			}

			/*

			// */

			// Make a Excel File : Header Response
			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.Buffer = true;
			HttpContext.Current.Response.Charset = "";
			HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

			// Set File Name
			HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + Server.UrlEncode(strPageID).Replace("+", "%20") + ".xlsx");

			// /*
			using (MemoryStream MyMemoryStream = new MemoryStream())
			{
				XLWorkbook.SaveAs(MyMemoryStream);
				MyMemoryStream.WriteTo(HttpContext.Current.Response.OutputStream);
				HttpContext.Current.Response.Flush();
				HttpContext.Current.Response.End();
			}
			// */
		}
		#endregion
	}
}
