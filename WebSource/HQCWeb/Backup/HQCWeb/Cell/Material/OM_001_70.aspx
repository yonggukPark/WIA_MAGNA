<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="OM_001_70.aspx.cs" Inherits="HQCWeb.Cell.Material.OM_001_70" %>

<%@ Register Src="~/UserContorls/GridControl.ascx" TagPrefix="uc1" TagName="GridControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:TextBox ID="hidGridHeight" runat="server" style="display:none;" />

    <asp:TextBox ID="hidScreenType" runat="server" style="display:none;"></asp:TextBox>

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">WEB_71200</asp:Label></h3>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:210px;" />
                    <col style="width:100px;" />
                    <col style="width:210px;" />
                    <col style="width:100px;" />
                    <col style="width:140px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbPeriod" runat="server"></asp:Label></th>
                    <td>
                        <asp:TextBox ID="txtFromDt" runat="server" style="width:80px;" /> ~ <asp:TextBox ID="txtToDt" runat="server" style="width:80px;"></asp:TextBox>                        
                    </td>
                    <th><asp:Label ID="lbLine" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlLine" runat="server"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbEquipment" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlEquipment" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPrinter" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlPrinter" runat="server" onclick="javascript:return fn_PrinterChk();"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbHeader" runat="server"></asp:Label></th>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlHeader" runat="server" onclick="javascript:return fn_HeaderChk();"></asp:DropDownList>
                    </td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_SearchValChk(); fn_WatingCall();" visible="false" />
                        <asp:Button ID="btnGridReload" runat="server" Text="POCall" OnClick="btnGridReload_Click"  style="display:none;"  />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:GridControl runat="server" ID="grid" Horizontal="Y" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnGridReload" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <script>

        if (screen.width == 1280) {
            $("#MainContent_hidGridHeight").val("595");
            fn_ScreenType("T");
        }
        else {
            $("#MainContent_hidGridHeight").val("665");
            fn_ScreenType("W");
        }

        jQuery(document).ready(function ($) {
            if (screen.width == 1280) {
                $("#MainContent_up1 .dxgvCSD").attr("style", "height:595px; overflow: scroll;");
            } else {
                $("#MainContent_up1 .dxgvCSD").attr("style", "height:665px;");
                
            }
        });

        function fn_PrinterChk() {
            var _val = $('#MainContent_ddlEquipment option:selected').val();

            var _valChkLabel = $("#MainContent_lbEquipment").text();

            if (_val == "") {
                alert(_valChkLabel + "를 선택해 주세요.");
                return false;
            }

            return true;
        }

        function fn_HeaderChk() {
            var _val = $('#MainContent_ddlPrinter option:selected').val();

            var _valChkLabel = $("#MainContent_lbPrinter").text();

            if (_val == "") {
                alert(_valChkLabel + "를 선택해 주세요.");
                return false;
            }

            return true;
        }


        function fn_SearchValChk() {

            var _val = $('#MainContent_ddlHeader option:selected').val();

            var _valChkLabel = $("#MainContent_lbHeader").text();

            if (_val == "") {
                alert(_valChkLabel + "를 선택해 주세요.");
                return false;
            }
        }


        function fn_ScreenType(_val) {
            $("#MainContent_hidScreenType").val(_val);

            $("#MainContent_btnGridReload").click();
        }


    </script>


</asp:Content>