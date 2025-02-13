<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="OM_001_69.aspx.cs" Inherits="HQCWeb.Cell.Material.OM_001_69" %>

<%@ Register Src="~/UserContorls/GridControl.ascx" TagPrefix="uc1" TagName="GridControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:TextBox ID="hidMainGridHeight" runat="server" style="display:none;" />
    <asp:TextBox ID="hidDetailGriddHeight" runat="server" style="display:none;" />

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">WEB_71100</asp:Label></h3>
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
                        <input type="text" id="txtFromDt" runat="server" onchange="javascript:fn_DateChagne();" style="width:80px;" /> ~ <asp:TextBox ID="txtToDt" runat="server" style="width:80px;"></asp:TextBox>                        
                    </td>
                    <th><asp:Label ID="lbShift" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlShift" runat="server"></asp:DropDownList>&nbsp;&nbsp;<input type="checkbox" id="chkShift" runat="server" onclick="javascript:fn_ChangeCon();" /></td>
                    <th><asp:Label ID="lbEquipment" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlEquipment" runat="server"></asp:DropDownList></td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search" OnClientClick="fn_WatingCall();"  Visible="false" />
                    </td>
                </tr>
            </table>
        </div>

        <br />

        <div class="col2">
            <div class="tbl">
                <p class="sub_tit">SCREEN</p><!--통계수치 테이블의 타이틀-->

                <div id="divMainGrid">
                    <asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc1:GridControl runat="server" ID="MainGrid" Horizontal="Y" RowColorUsed="Y" OnHtmlRowPrepared="Main_HtmlRowPrepared" /> 
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>

                </div>
            </div>

            <div class="tbl ml20">
                <p class="sub_tit">LIFETIME</p><!--통계수치 테이블의 타이틀-->

                <div id="divDetailGrid">
                    <asp:HiddenField ID="hidParams" runat="server" />

                    <asp:UpdatePanel ID="upDetail" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <uc1:GridControl runat="server" ID="DetailGrid" Horizontal="Y"  RowColorUsed="Y" OnHtmlRowPrepared="Detail_HtmlRowPrepared"  />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    

    <script type="text/javascript">

        if (screen.width == 1280) {
            $("#MainContent_hidMainGridHeight").val("595");
            $("#MainContent_hidDetailGriddHeight").val("595");
        }
        else {
            $("#MainContent_hidMainGridHeight").val("660");
            $("#MainContent_hidDetailGriddHeight").val("660");
        }

        jQuery(document).ready(function ($) {
            $("#MainContent_ddlShift").attr("disabled", true);

            if (screen.width == 1280) {
                $("#MainContent_upMain .dxgvCSD").attr("style", "height:595px; overflow: scroll;");
                $("#MainContent_upDetail .dxgvCSD").attr("style", "height:595px; overflow: scroll;");
            } else {
                $("#MainContent_upMain .dxgvCSD").attr("style", "height:660px; overflow: scroll;");
                $("#MainContent_upDetail .dxgvCSD").attr("style", "height:660px;  overflow: scroll;");
            }
        });

        function fn_ChangeCon() {
            if ($('#MainContent_chkShift').is(':checked')) {
                $("#MainContent_ddlShift").attr("disabled", false);
                $("#MainContent_txtToDt").attr("disabled", true);
            } else {
                $("#MainContent_ddlShift").attr("disabled", true);
                $("#MainContent_txtToDt").attr("disabled", false);
            }
        }

        function fn_DateChagne() {
            var strFromDt = $("#MainContent_txtFromDt").val().replace(/-/g, '');
            var strToDt = $("#MainContent_txtToDt").val().replace(/-/g, '');

            console.log("strFromDt=", strFromDt);
            console.log("strToDt=", strToDt);

            if (strFromDt > strToDt) {
                $("#MainContent_txtToDt").val($("#MainContent_txtFromDt").val());
            }
        }

        function fn_Detail(_val) {
            $("#MainContent_hidParams").val(_val);
            $("#MainContent_btnDetailSearch").click();
        }



    </script>

</asp:Content>
