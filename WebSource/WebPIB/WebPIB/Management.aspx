<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Management.aspx.cs" Inherits="WebPIB.Management" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script>
		function OpenNewWindow(monId) {
			window.open("Board.html?mid=" + monId);
		};
		function ClickMonitorId(monId) {
			document.getElementById("<%=hfMonId_Old.ClientID%>").value = monId;
			document.getElementById("<%=btnSearchMonitor.ClientID%>").click();
		};

		function AddMonitor() {
			if (confirm("모니터를 추가하시겠습니까?")) {
				return true;
			};
			return false;
		};
		function SaveMonitor() {
			if (confirm("모니터 정보를 저장하시겠습니까?")) {
				return true;
			};
			return false;
		};
		function DelMonitor() {
			if (document.getElementById("<%=tbMonId.ClientID%>").value.length < 1) {
				alert("삭제할 모니터 ID를 왼쪽 목록에서 클릭하세요.");
			}
			else if (confirm("모니터를 삭제하시겠습니까?")) {
				return true;
			};
			return false;
		};

		function RestartMonitor() {
			if (confirm("모니터를 재시작 하시겠습니까?")) {
				return true;
			};
			return false;
		};

		function SavePIB() {
			if (document.getElementById("<%=hfMonId_Old.ClientID%>").value.length == 0) {
				alert("모니터 정보가 없습니다.");
			}
			else if (document.getElementById("<%=tbMonId.ClientID%>").value == document.getElementById("<%=hfMonId_Old.ClientID%>").value) {
				if (confirm("변경된 상세정보를 저장하시겠습니까?")) {
					return true;
				};
			}
			else {
				alert("아이디가 " + document.getElementById("<%=hfMonId_Old.ClientID%>").value + "에서 " + document.getElementById("<%=tbMonId.ClientID%>").value + "로 변경 후 저장되지 않았습니다.");
			};
			return false;
		};

		function DelPIB() {
			var _tblPibEdit = document.getElementById("<%=tblPibEdit.ClientID%>");
			if (_tblPibEdit.rows.length && _tblPibEdit.rows.length > 1) {
				for (var i = _tblPibEdit.rows.length - 1; i >= 1; i--) {
					if (_tblPibEdit.rows[i].cells[0].childNodes[0].checked) {
						_tblPibEdit.rows[i].removeNode(true);
					};
				};
			};
			return false;
		};

		function AddPIB() {
			var _tblPibEdit = document.getElementById("<%=tblPibEdit.ClientID%>");
			var _tr, _td, _input;
			_tr = _tblPibEdit.insertRow(-1);
			_td = _tr.insertCell(-1);
			_input = document.createElement("INPUT");
			_input.setAttribute("name", "cbDISP_CHK");
			_input.setAttribute("type", "checkbox");
			_td.appendChild(_input);

			_input = document.createElement("INPUT");
			_input.setAttribute("name", "hfDISP_SEQ");
			_input.setAttribute("type", "hidden");
			_td.appendChild(_input);
			_tblPibEdit.rows[_tblPibEdit.rows.length - 1].cells[0].style.textAlign = "center";
			
			_td = _tr.insertCell(-1);
			_input = document.createElement("INPUT");
			_input.setAttribute("name", "tbDISP_SEQ");
			_input.setAttribute("type", "text");
			_input.setAttribute("size", "2");
			_input.setAttribute("maxlength", "2");
			_input.setAttribute("value", _tblPibEdit.rows.length-1);
			_td.appendChild(_input);
			_tblPibEdit.rows[_tblPibEdit.rows.length - 1].cells[1].style.textAlign = "center";
			_tblPibEdit.rows[_tblPibEdit.rows.length - 1].cells[1].childNodes[0].style.textAlign = "center";

			_td = _tr.insertCell(-1);
			_input = document.createElement("INPUT");
			_input.setAttribute("name", "tbURL");
			_input.setAttribute("type", "text");
			_input.setAttribute("size", "100");
			_input.setAttribute("maxlength", "100");
			_td.appendChild(_input);
			_tblPibEdit.rows[_tblPibEdit.rows.length - 1].cells[2].style.textAlign = "center";
			_tblPibEdit.rows[_tblPibEdit.rows.length - 1].cells[2].childNodes[0].style.width = "100%";

			_td = _tr.insertCell(-1);
			_input = document.createElement("INPUT");
			_input.setAttribute("name", "tbALIVE_TM");
			_input.setAttribute("type", "text");
			_input.setAttribute("size", "2");
			_input.setAttribute("maxlength", "2");
			_td.appendChild(_input);
			_tblPibEdit.rows[_tblPibEdit.rows.length - 1].cells[3].style.textAlign = "center";
			_tblPibEdit.rows[_tblPibEdit.rows.length - 1].cells[3].childNodes[0].style.textAlign = "center";

			return false;
		};
	</script>
    <style type="text/css">
        .auto-style1 {
            height: 40px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<table style="width:100%; height:100%; background-color:#ffffff;">
		<tr>
			<td style="vertical-align:top; width:45%; white-space:nowrap; height:100%; overflow-y:auto; ">
				<div style="width:100%; height:100%; overflow-y:auto;">
					<table style="width:100%; border:1px solid silver; overflow-y:auto;">					
						<tr>
							<td>현황판</td>
							<td style="text-align:right;">
							</td>
						</tr>
						<tr>
							<td colspan="2" class="auto-style1">
								<table style="width:100%; border:1px solid silver;">
									<tr>
										<td >
											<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
												<ContentTemplate>
													<table id="tblMonitorStatus" class="tblMonitorStatus" runat="server" style="width:100%;">
														<tr>
															<th style="height:24px; text-align:center; white-space:nowrap;">링크</th>
															<th style="height:24px; text-align:center; white-space:nowrap;">상태</th>
															<th style="height:24px; text-align:center; white-space:nowrap;">모니터 ID</th>
															<th style="height:24px; text-align:center; white-space:nowrap;">설명</th>
														</tr>
													</table>
												</ContentTemplate>
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="btnAddMonitor" EventName="Click" />
													<asp:AsyncPostBackTrigger ControlID="btnSaveMonitor" EventName="Click" />
													<asp:AsyncPostBackTrigger ControlID="btnDelMonitor" EventName="Click" />
													<asp:AsyncPostBackTrigger ControlID="btnRestartMonitor" EventName="Click" />
													<asp:AsyncPostBackTrigger ControlID="timerMonitor" EventName="Tick" />
												</Triggers>
											</asp:UpdatePanel>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</div>
			</td>
			<td style="vertical-align:top;">
				<fieldset>
					<legend>모니터관리</legend>
					<table>
						<tr>
							<td>	
								<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table class="tblMonitorManage" style="width:100%; border:1px solid silver;">
											<tr>
												<th>아이디</th>
												<td>
													<asp:TextBox ID="tbMonId" runat="server"></asp:TextBox>
													<asp:HiddenField ID="hfMonId_Old" runat="server" />
												</td>
											</tr>
											<tr>
												<th>아이피</th>
												<td><asp:TextBox ID="tbMonIp" runat="server"></asp:TextBox></td>
											</tr>
											<tr>
												<th>설명</th>
												<td><asp:TextBox ID="tbMonDesc" size="100" runat="server" Width="400"></asp:TextBox></td>
											</tr>
											<tr>
												<th>전달 메시지</th>
												<td><asp:TextBox ID="tbMonMsg" runat="server" Rows="5" TextMode="MultiLine" Width="400"></asp:TextBox></td>
											</tr>
										</table>
									</ContentTemplate>
									<Triggers>
										<asp:AsyncPostBackTrigger ControlID="btnAddMonitor" EventName="Click" />
										<asp:AsyncPostBackTrigger ControlID="btnSaveMonitor" EventName="Click" />
										<asp:AsyncPostBackTrigger ControlID="btnDelMonitor" EventName="Click" />
										<asp:AsyncPostBackTrigger ControlID="btnSearchMonitor" EventName="Click" />
									</Triggers>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td>
								<table style="width:100%;">
									<tr>
										<td style="text-align:right;">
											<asp:Button ID="btnAddMonitor" runat="server" Text="추가" OnClientClick="return AddMonitor();" OnClick="btnAddMonitor_Click" />
											<asp:Button ID="btnSaveMonitor" runat="server" Text="저장" OnClientClick="return SaveMonitor();" OnClick="btnSaveMonitor_Click" />
											<asp:Button ID="btnDelMonitor" runat="server" Text="삭제" OnClientClick="return DelMonitor();" OnClick="btnDelMonitor_Click" />
											<asp:Button ID="btnRestartMonitor" runat="server" Text="재시작" OnClientClick="return RestartMonitor();" OnClick="btnRestartMonitor_Click" />
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</fieldset>
				<br />
				<br />
				<fieldset>
					<legend>상세관리</legend>
					<table>
						<tr>
							<td>	
								<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
									<ContentTemplate>
										<table id="tblPibEdit" class="tblPibEdit" runat="server" style="width:100%; border:1px solid silver;">
											<tr>
												<th style="white-space:nowrap;">선택</th>
												<th style="white-space:nowrap;">순서</th>
												<th style="white-space:nowrap;">URL</th>
												<th style="white-space:nowrap;">표시시간(초)</th>
											</tr>
										</table>
									</ContentTemplate>
									<Triggers>
										<asp:AsyncPostBackTrigger ControlID="btnAddMonitor" EventName="Click" />
										<asp:AsyncPostBackTrigger ControlID="btnSaveMonitor" EventName="Click" />
										<asp:AsyncPostBackTrigger ControlID="btnDelMonitor" EventName="Click" />
										<asp:AsyncPostBackTrigger ControlID="btnSearchMonitor" EventName="Click" />
										<asp:AsyncPostBackTrigger ControlID="btnAddPIB" EventName="Click" />
										<asp:AsyncPostBackTrigger ControlID="btnDelPIB" EventName="Click" />
										<asp:AsyncPostBackTrigger ControlID="btnSavePIB" EventName="Click" />
									</Triggers>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td>
								<table style="width:100%;">
									<tr>
										<td style="text-align:right;">
											<asp:Button ID="btnAddPIB" runat="server" Text="추가" OnClientClick="return AddPIB();" />
											<asp:Button ID="btnDelPIB" runat="server" Text="삭제" OnClientClick="return DelPIB();" />
											<asp:Button ID="btnSavePIB" runat="server" Text="저장" OnClientClick="return SavePIB();" OnClick="btnSavePIB_Click" />
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</fieldset>
			</td>
		</tr>
	</table>
	<div style="display:none;">
		<asp:Timer ID="timerMonitor" runat="server" Interval="3000" OnTick="timerMonitor_Tick"></asp:Timer>
		<asp:Button ID="btnSearchMonitor" runat="server" OnClick="btnSearchMonitor_Click" />
	</div>
</asp:Content>
