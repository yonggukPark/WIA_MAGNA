<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="EQ_013_01.aspx.cs" Inherits="HQCWeb.Cell.Alarm.EQ_013_01" %>

<%@ Register Src="~/UserContorls/GridControl.ascx" TagPrefix="uc1" TagName="GridControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        function fn_hide() {
            if ($("#div_Search").css("display") == "none") {
                $("#div_Search").show();

                if (screen.width == 1280) {
                    $(".dxgvCSD").attr("style", "height:220px; overflow: scroll;");
                } else {
                    $(".dxgvCSD").attr("style", "height:285px; overflow: scroll;");
                }
                
            } else {
                $("#div_Search").hide();

                //$(".dxgvCSD").attr("style", "height:391px; overflow: scroll;");
                //$(".dxgvCSD").attr("style", "height:461px; overflow: scroll;");

                if (screen.width == 1280) {
                    $(".dxgvCSD").attr("style", "height:396px; overflow: scroll;");
                } else {
                    $(".dxgvCSD").attr("style", "height:461px; overflow: scroll;");
                }
            }
        }
    </script>
    
    <%--<asp:HiddenField ID="hidGraphWidth" runat="server" />
    <asp:HiddenField ID="hidGraphHeight" runat="server" />--%>

    <asp:TextBox ID="hidGraphWidth" runat="server" style="display:none;" />
    <asp:TextBox ID="hidGraphHeight" runat="server" style="display:none;" />

    <asp:TextBox ID="hidGridHeight" runat="server" style="display:none;" />



    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">WEB_61100</asp:Label></h3>
            <ul class="win_btn">
                <li><a href="javascript:fn_hide();" class="reduce">축소</a></li>
            </ul>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13" id="div_Search">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbWorkDate" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtDate" runat="server" style="width:80px;"></asp:TextBox></td>
                    <th><asp:Label ID="lbShift" runat="server"></asp:Label></th>
                    <td colspan="3"><asp:DropDownList ID="ddlShift" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbOperation" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlOperation" runat="server"></asp:DropDownList></td>
                    <th><asp:Label ID="lbAlarmClass" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlAlarmClass" runat="server"></asp:DropDownList></td>
                    <th><asp:Label ID="lbEquipmentType" runat="server"></asp:Label></th>
                    <td class="2"><asp:DropDownList ID="ddlEquipmentType" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbAlarmType" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlAlarmType" runat="server"></asp:DropDownList></td>
                    <th><asp:Label ID="lbAlarmLevel" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlAlarmLevel" runat="server"></asp:DropDownList></td>
                    <th><asp:Label ID="lbAlarmGrade" runat="server"></asp:Label></th>
                    <td class="2"><asp:DropDownList ID="ddlAlarmGrade" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbAlarmCommon" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlAlarmCommon" runat="server"></asp:DropDownList></td>
                    <th><asp:Label ID="lbAlarmHead" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlAlarmHead" runat="server"></asp:DropDownList></td>
                    <th><asp:Label ID="lbAlarmCamera" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlAlarmCamera" runat="server"></asp:DropDownList></td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                        <asp:Button ID="btnReload" runat="server" Text="Reload" OnClick="btnReload_Click" style="display:none;" />
                    </td>
                </tr>
            </table>
        </div>

        <div class="graph_wrap mt14" style="height:270px;" id="divChart">
            <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dxchartsui:WebChartControl ID="chart_01" runat="server" CrosshairEnabled="false">
                    </dxchartsui:WebChartControl>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnReload" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <br />

        <p class="sub_tit">시간별 Alarm 수량 정보</p><!--통계수치 테이블의 타이틀-->

        <div style="width:100%; height:300px;">
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--<uc1:GridControl runat="server" ID="grid" SessionYN="Y" Height="280" Horizontal="Y" />--%>

                    <dx:ASPxGridView ID="grid" ClientInstanceName="grid" 
                        runat="server" Width="100%" 
                        AutoGenerateColumns="True" 
                        EnableRowsCache="false"
                        OnHtmlRowPrepared="grid_HtmlRowPrepared">
                        <SettingsLoadingPanel ImagePosition="Top" Delay="1000" />    
                        <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="VirtualSmooth"  HorizontalScrollBarMode="Visible" /> 
                        <SettingsPager Mode="EndlessPaging"  PageSize="100000"/>

                        <Styles>
                            <Header HorizontalAlign="Center"  BackColor="#edf5fa" ForeColor="#2e6e99" />  
                        </Styles>
                    </dx:ASPxGridView>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnReload" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <script>

        jQuery(document).ready(function ($) {

            if (screen.width == 1280) {
                $("#MainContent_hidGraphWidth").val(screen.width - 150);
                $("#MainContent_hidGraphHeight").val("260");

                $("#MainContent_hidGridHeight").val("220");
            }
            else {
                $("#MainContent_hidGraphWidth").val(screen.width - 319);
                $("#MainContent_hidGraphHeight").val("268");

                $("#MainContent_hidGridHeight").val("285");
            }

            $("#MainContent_btnReload").click();

        });
    </script>

</asp:Content>
