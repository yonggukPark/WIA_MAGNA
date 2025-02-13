<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Popup.Master" AutoEventWireup="true" CodeBehind="Qua50_p01.aspx.cs" Inherits="HQCWeb.QualityMgt.Qua50.Qua50_p01" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupContent" runat="server">
    <style>
        .searchCombo{
            text-align: center;
            z-index : 9999;
            top : 55%;
            position : absolute; 
        }
    </style>
    <script type="text/javascript">

        var cLine;

        function fn_Validation() {

            if ($("#PopupContent_txtDate").val() == "") {
                alert("발생일을 입력하세요.");
                return false;
            } else if ($("#PopupContent_ddlPartNo").val() == "") {
                alert("품번을 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlShopCd").val() == "") {
                alert("Shop 코드를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlLineCd").val() == "") {
                alert("라인 코드를 선택하세요.");
                return false;
            } else if ($("#PopupContent_ddlCarType").val() == "") {
                alert("차종 코드를 선택하세요.");
                return false;
            } else {
                if (confirm("등록 하시겠습니까?")) {
                    fn_WatingCall();
                    return true;
                } else {
                    return false;
                }
            }

            return true;
        }

        function fn_Validation2() {
            if ($("#PopupContent_txtPartNoSearch").val() == "") {
                alert("먼저 품번을 입력하세요.");
                return false;
            } else {
                fn_WatingCall();
                return true;
            }
        }

        function fn_Save() {
            $("#PopupContent_btnSave").click();
        }

        function fn_Check() {
            $("#PopupContent_btnCheck").click();
        }

        //콤보박스 초기화
        jQuery(document).ready(function ($) {
            fn_Set_Line();
        });

        //라인코드 재설정
        function fn_Set_Line() {
            $("#PopupContent_txtLineCd").comboTree({
                source: cLine,
                comboReload: false,
                isMultiple: true,
                cascadeSelect: false,
                collapse: false,
                selectAll: true,
                onLoadAllChk: true,
                valueChange: true,
                hidCon: "PopupContent_txtLineCdHidden",
                functionCall: true,                    // 콤보값 선택시 자바스크립스 함수 호출 여부
                functionCallFunc: "fn_Line_Checked"
            });

            $(".ct-drop-down-container").css("height", "100px");
        }

        //Line코드 onchange
        function fn_Line_Checked() {
            $("#PopupContent_btnFunctionCall").click();
        }
    </script>    

	<!--// POPUP -->
    <div class="popup_wrap">
        <!--// Title -->
        <div class="title">
            <h1><asp:Label ID="lbTitle" runat="server">Qua50</asp:Label> - <asp:Label ID="lbWorkName" runat="server"></asp:Label></h1>
            <a href="javascript:parent.fn_ModalCloseDiv();" title="close"></a>
        </div>
        <!-- Title //-->

        <!--// Box -->
        <div class="box">

            <!--// Table -->
            <table cellpadding="0" cellspacing="0">
                 <colgroup>
                    <col style="width:60px;" />
                    <col style="width:250px;" />
                </colgroup>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbRegDt" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" Width="80" onkeydown="return fn_EventKeyChk(event);" onkeyup="removeChar(event);" style="background-color:white; color:black;"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbEo4mFlag" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlEo4mFlag" runat="server" ></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbPartNoSearch" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtPartNoSearch" runat="server"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCheck" EventName="Click"/>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="padding:0px;">
                        <div class = "td_wrap">
                            <a href="javascript:fn_Check();" class="btn ml10" id="aCheck" runat="server" >Check</a>
                        </div>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbPartNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlPartNo" runat="server" OnSelectedIndexChanged="ddlPartNo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCheck" EventName="Click"/>
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPartDesc" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                 <asp:Label ID="lbGetPartDesc" runat="server"></asp:Label>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnCheck" EventName="Click"/>
                                <asp:AsyncPostBackTrigger ControlID="ddlPartNo" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                        <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlPartNo" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="searchCombo" style="width:200px; font-size:12px;">
                                    <input type="text" ID="txtLineCd" runat="server" style="background-color:white; color:black;" readonly/>
                                    <asp:TextBox id="txtLineCdHidden" runat="server" style="display:none;"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlPartNo" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><b style="color:red;">*</b><asp:Label ID="lbCarType" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCarType" runat="server"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnFunctionCall" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="ddlPartNo" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbEoNo" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtEoNo" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbModRemark" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtModRemark" runat="server" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <!-- Table //-->

            <!--// Btn -->
            <div class="btn_wrap mt20">
                <a href="javascript:fn_Save();" class="btn ml10" id="aSave" runat="server" visible="false">Save</a>
                <a href="javascript:parent.fn_ModalCloseDiv();" class="btn_close ml10">Close</a>

                <asp:Button ID="btnSave"    runat="server" OnClientClick="javascript:return fn_Validation();" OnClick="btnSave_Click" Text="Save" style="display:none;" />
                <asp:Button ID="btnFunctionCall" runat="server" Text="FunctionCall" OnClick="btnFunctionCall_Click"  style="display:none;"  />
                <asp:Button ID="btnCheck"     runat="server" OnClientClick="javascript:return fn_Validation2();" OnClick="btnCheck_Click" Text="Check" style="display:none;"/>
            </div>
            <!-- Btn //-->

        </div>
        <!-- Box //-->

    </div>
    <!-- POPUP //-->
</asp:Content>
