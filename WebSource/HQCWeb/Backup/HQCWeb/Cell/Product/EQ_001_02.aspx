<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="EQ_001_02.aspx.cs" Inherits="HQCWeb.Cell.Product.EQ_001_02" %>

<%@ Register Src="~/UserContorls/GridControl.ascx" TagPrefix="uc1" TagName="GridControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .searchCombo_2{
            text-align: center;
            z-index : 1;
            top : 15%;
            position : absolute; 
        }
    </style>


    <script type="text/javascript">
        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        function fn_hide() {
            if ($("#div_Search").css("display") == "none") {
                $("#div_Search").show();

                $(".dxgvCSD").attr("style", "height:255px; overflow: scroll;");

            } else {
                $("#div_Search").hide();

                $(".dxgvCSD").attr("style", "height:391px; overflow: scroll;");
            }
        }
    </script>
    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <asp:HiddenField ID="hidGraphWidth" runat="server" />
    <asp:HiddenField ID="hidGraphHeight" runat="server" />

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">WEB_11300</asp:Label></h3>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:80px;" />
                    <col style="width:100px;" />
                    <col style="width:50px;" />
                    <col style="width:190px;" />
                    <col style="width:100px;" />
                    <col style="width:80px;" />
                    <col style="width:80px;" />
                    <col style="width:220px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbWorkDate" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtDate" runat="server" style="width:80px; z-index:200;"></asp:TextBox></td>
                    <th><asp:Label ID="lbShift" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlShift" runat="server"></asp:DropDownList></td>
                    <th><asp:Label ID="lbWorkTeam" runat="server"></asp:Label></th>
                    <td><asp:DropDownList ID="ddlWorkTeam" runat="server"></asp:DropDownList></td>
                    <th><asp:Label ID="lbPO" runat="server"></asp:Label></th>
                    <td>
                        <div class="searchCombo_2" style="width:200px;">
                            <asp:TextBox id="txtPO" runat="server" placeholder="Select" autocomplete="off"></asp:TextBox>
                            <asp:TextBox id="txtPOHidden" runat="server" style="display:none;"></asp:TextBox>
                        </div>
                    </td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                        <asp:Button ID="btnReload" runat="server" Text="Reload" OnClick="btnReload_Click" style="display:none;" />
                    </td>
                </tr>
            </table>
        </div>

        <div class="graph_wrap mt14" style="height:370px; overflow:auto;" id="divChart">
            <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <dxchartsui:WebChartControl ID="chart_04" runat="server" CrosshairEnabled="false">
                    </dxchartsui:WebChartControl>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnReload" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <br />

        <p class="sub_tit">생산실적</p><!--통계수치 테이블의 타이틀-->

        <div style="width:100%; height:300px;">
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <uc1:GridControl runat="server" ID="grid" Height="340" Horizontal="Y" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>

    <script>

        jQuery(document).ready(function ($) {
            if (screen.width == 1280) {
                $("#MainContent_hidGraphWidth").val(5000);
                $("#MainContent_hidGraphHeight").val("260");

                $("#divChart").attr("style", "height:265px; overflow:auto;");
            }
            else {
                $("#MainContent_hidGraphWidth").val(5000);
                $("#MainContent_hidGraphHeight").val("338");

                $("#divChart").attr("style", "height:340px; overflow:auto;");
            }

            $("#MainContent_btnReload").click();

            $("#MainContent_txtPO").comboTree({
                source: <%=strPOJson%>,
                isMultiple: true,
                cascadeSelect: true,
                collapse: false,
                selectAll: true,
                valueChange: false
            });
        });

    </script>

</asp:Content>
