<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" EnableEventValidation="true" AutoEventWireup="true" CodeBehind="PublicHorizontalSplitSample_GT1.aspx.cs" Inherits="HQCWeb.SystemMgt.PublicHorizontalSplitSample_GT1" %>

<%@ Register Src="~/UserContorls/GridControl.ascx" TagPrefix="uc1" TagName="GridControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }
    </script>
    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <asp:HiddenField ID="hidGraphWidth" runat="server" />
    <asp:HiddenField ID="hidGraphHeight" runat="server" />

    <asp:TextBox ID="hidGridHeight" runat="server" style="display:none;" />

    <div class="contents">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">MENU_ID</asp:Label></h3>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:100px;" />
                    <col style="width:190px;" />
                    <col />
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbCond_01" runat="server"></asp:Label></th>
                    <td><asp:TextBox ID="txtCondi1" runat="server" style="width:150px;"></asp:TextBox></td>
                    <td class="al-r">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" />
                        <asp:Button ID="btnExcel" runat="server" OnClick="Excel_Click" Text="Excel" Visible="false" />
                        <asp:Button ID="btnReload" runat="server" Text="Reload" OnClick="btnReload_Click" style="display:none;" />
                    </td>
                </tr>
            </table>
        </div>

        <div class="graph_wrap mt14" style="height:370px; overflow:auto;" id="divChart">
            <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--<dxchartsui:WebChartControl ID="WebChartControl" runat="server" CrosshairEnabled="false">
                    </dxchartsui:WebChartControl>--%>

                    <img src="img/sample_graph.png" style="width:100%; height:100%;" title="그래프 샘플 이미지 입니다."><!--이 부분에 그래프를 넣어주세요.-->
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnReload" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

            
        </div>

        <br />

        <p class="sub_tit">타이틀</p><!--통계수치 테이블의 타이틀-->

        <div style="width:100%; height:300px;">
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                   <%-- <uc1:GridControl runat="server" ID="grid" />--%>


 

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

           
                    

        </div>
    </div>

    <script>
        if (screen.width == 1280) {
            $("#MainContent_hidGraphWidth").val(screen.width - 150);
            $("#MainContent_hidGraphHeight").val("368");

            $("#MainContent_hidGridHeight").val("272");
        }
        else {
            $("#MainContent_hidGraphWidth").val(screen.width - 316);
            $("#MainContent_hidGraphHeight").val("338");

            $("#MainContent_hidGridHeight").val("260");
        }

        $("#MainContent_btnReload").click();
    </script>

</asp:Content>
