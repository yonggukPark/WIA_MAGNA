<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Widget.Master" AutoEventWireup="true" CodeBehind="WidgetMgt.aspx.cs" Inherits="HQCWeb.SystemMgt.WidgetManagement.WidgetMgt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .widget_button {
            position: relative;
            display: inline-block;
            width: 80px;
            height: 28px;
            line-height: 2px;
            font-size: 12px;
            color: #ffffff;
            font-weight: 500;
            margin-left: 5px;
            background-color: #8a877c;
            margin-top:20px !important;
            margin-left:30px;
        }
    </style>

    <script type="text/javascript">
        var _id;

        $(function () {
            $.contextMenu({
                selector: '.context-menu-one',
                items: {
                    editCard: {
                        name: "수정",
                        callback: function (key, opt) {
                            fn_Modify(opt.$trigger[0].id);
                            _id = opt.$trigger[0].id;
                        }

                    }, deleteCard: {
                        name: "삭제",
                        callback: function (key, opt) {
                            fn_Delete(opt.$trigger[0].id);
                            _id = opt.$trigger[0].id;
                            //console.log("_id=", _id);
                        }
                    }
                },
                events: {
                    show: function (opt) {
                        var $this = this;
                        $.contextMenu.setInputValues(opt, $this.data());
                    },
                    hide: function (opt) {
                        var $this = this;
                        $.contextMenu.getInputValues(opt, $this.data());
                    }
                }
            });
        });
        
        //txtdivCon1Size
        function fn_Modify(_val) {
            var _width = $("#MainContent_txt" + _val + "Size").val();

            fn_PopupCall();

            $("#divLayer").show();

            var jsonData = JSON.stringify({ sParams: _width });

            $.ajax({
                type: "POST",
                url: "WidgetMgt.aspx/GetWidgetTemplateInfo",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    fn_TemplateSetting(msg.d);
                }
            });
        }

        function fn_TemplateSetting(_val) {
            $("#tbodyWidgetList").html("");
            $("#tbodyWidgetList").html(_val);
        }

        function fn_Delete(_val) {
            $("#" + _val).empty();
            $("#MainContent_txt" + _val + "Widget").val("");
        }

        function fn_Change(_val, _val2, _val3, _val4) {

            const divID = $("#" + _id);
            const txtID = $("#MainContent_txt" + _id + "Widget");

            divID.load("<%=strWebIpPort%>" + _val4);
            txtID.val(_val3);

            $("#divLayer").hide();
            fn_loadingEnd();
        }
    </script>
    <div class="frame_search mt14">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:DropDownList ID="ddlFrameNum" runat="server" OnSelectedIndexChanged="ddlFrameNum_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <asp:Button ID="btnSearch"      runat="server" Text="Search" class="widget_button mt20" OnClick="btnSearch_Click" style="display:none;" />
                    <asp:Button ID="btnSave"        runat="server" Text="저장"  OnClick="btnSave_Click"  />
                </td>
            </tr>
        </table>
    </div>

    <div class="contents" id="MainContents" style="width:calc(100% - 108px); position:absolute; top:30px;">
		<%--<div class="contents_l">
			<div class="contents_l01" >
				<div class="context-menu-one btn btn-neutral" id="divCon1"></div>
				<div class="ml30 context-menu-one btn btn-neutral" id="divCon2"></div>
				<div class="ml30 context-menu-one btn btn-neutral" id="divCon3"></div>
			</div>

			<div class="contents_l02 mt30">
				<div class="graph real_board context-menu-one btn btn-neutral" id="divCon4"></div>
				<div class="ml30 pie notice context-menu-one btn btn-neutral" id="divCon5"></div>
			</div>

			<div class="contents_l03 mt30">
				<div class="graph real_board context-menu-one btn btn-neutral" id="divCon6"></div>
				<div class="ml30 pie notice context-menu-one btn btn-neutral" id="divCon7"></div>
			</div>
		</div>

		<div class="contents_r ml30">
			<div class="calendar context-menu-one btn btn-neutral" id="divCon8"></div>
			<div class="alarm mt30 context-menu-one btn btn-neutral" id="divCon9"></div>
		</div>--%>
	</div>

    <asp:TextBox ID="txtdivCon1Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon2Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon3Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon4Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon5Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon6Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon7Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon8Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon9Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon10Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon11Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon12Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon13Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon14Widget" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon15Widget" runat="server" style="display:none;"></asp:TextBox>

    <asp:TextBox ID="txtdivCon1Size" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon2Size" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon3Size" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon4Size" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon5Size" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon6Size" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon7Size" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon8Size" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon9Size" runat="server" style="display:none;"></asp:TextBox>

    <asp:TextBox ID="txtdivCon10Size" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon11Size" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon12Size" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon13Size" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon14Size" runat="server" style="display:none;"></asp:TextBox>
    <asp:TextBox ID="txtdivCon15Size" runat="server" style="display:none;"></asp:TextBox>

	<div class="popup" style="z-index:101; position:absolute; top:35%; left:45%; width:400px; display:none;" id="divLayer">
		<div class="title">
			<h4>위젯 셋팅</h4>
			<button type="button" class="btn_close" id="close" onclick="javascript:divLayer_close();"></button>
		</div>

		<div class="popup_cont">
			<!--// POPUP Grid 스타일 -->
			<table cellpadding="0" cellspacing="0">
				<thead>
					<tr>
						<th>위젯이름</th>
						<th>선택</th>
					</tr>
				</thead>
				<tbody id="tbodyWidgetList">
						
				</tbody>
			</table>
			<!-- POPUP Grid 스타일 //-->
		</div>
	</div>
	


    <script>
        function fn_WidgetSetting(_val, _val2, _val3, _val4) {
            const divID = $("#divCon" + _val);
            const txtID = $("#MainContent_txtdivCon" + _val + "Widget");
            const sizeID = $("#MainContent_txtdivCon" + _val + "Size");

            if(_val3 != "")
                divID.load("<%=strWebIpPort%>" + _val3);

            txtID.val(_val2);
            sizeID.val(_val4);
        }

        function divLayer_close() {
            $("#divLayer").hide();
            fn_loadingEnd();
        }
    </script>

</asp:Content>