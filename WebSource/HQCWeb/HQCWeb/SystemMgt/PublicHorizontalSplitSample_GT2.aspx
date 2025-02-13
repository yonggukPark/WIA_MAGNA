<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="PublicHorizontalSplitSample_GT2.aspx.cs" Inherits="HQCWeb.SystemMgt.PublicHorizontalSplitSample_GT2" %>

<%@ Register Src="~/UserContorls/GridControl.ascx" TagPrefix="uc1" TagName="GridControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var column, field, data;
        var container, dataProvider, gridView;

        // 그리드 생성
        function createGrid() {
            container = document.getElementById('realgrid');
            dataProvider = new RealGrid.LocalDataProvider(false);
            gridView = new RealGrid.GridView(container);
            gridView.setDataSource(dataProvider);
            gridView.setColumns(column);
            dataProvider.setFields(field);
            dataProvider.setRows(data);

            dataProvider.softDeleting = true;
            gridView.displayOptions.fitStyle = "even";//그리드 채우기
            gridView.displayOptions.emptyMessage = "표시할 데이타가 없습니다.";
            gridView.columnByName("REGIST_DATE").editable = false;
            gridView.columnByName("REGIST_USER_ID").editable = false;
            gridView.columnByName("MODIFY_DATE").editable = false;
            gridView.columnByName("MODIFY_USER_ID").editable = false;
            let col = gridView.columnByName('DIC_ID');
            col.styleCallback = f1;
            setPaging();
        }

        //차트생성
        // Load the Visualization API and the corechart package.
        google.charts.load('current', { 'packages': ['corechart'] });

        // Set a callback to run when the Google Visualization API is loaded.
        google.charts.setOnLoadCallback(drawChart);

        // Callback that creates and populates a data table,
        // instantiates the pie chart, passes in the data and
        // draws it.
        function drawChart() {

            // Create the data table.
            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Topping');
            data.addColumn('number', 'Slices');
            data.addRows([
              ['Mushrooms', 3],
              ['Onions', 1],
              ['Olives', 1],
              ['Zucchini', 1],
              ['Pepperoni', 2]
            ]);

            // Set chart options
            var options = {
                'title': 'How Much Pizza I Ate Last Night',
                'width': 400,
                'height': 400
            };

            // Instantiate and draw our chart, passing in some options.
            var chart = new google.visualization.BarChart(document.getElementById('chart_div'));
            chart.draw(data, options);
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
            <h3><asp:Label ID="lbTitle" runat="server">WEB_12000</asp:Label></h3>
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
                    </td>
                </tr>
            </table>
        </div>

        <div class="graph_wrap mt14" style="height:370px; overflow:auto;" id="divChart">
            <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <%--<dxchartsui:WebChartControl ID="WebChartControl" runat="server" CrosshairEnabled="false">
                    </dxchartsui:WebChartControl>--%>

                    <%--<img src="../img/sample_graph.png" style="width:100%; height:100%;" title="그래프 샘플 이미지 입니다."><!--이 부분에 그래프를 넣어주세요.-->--%>

                    <div id="chart_div"></div>

                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnExcel" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <br />

        <p class="sub_tit">타이틀</p><!--통계수치 테이블의 타이틀-->

        <div style="width:100%; height:300px;">
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="realgrid" style="width: 100%; height: 440px;"></div>
                    <div id="gridPage" onselectstart="return false" ondragstart="return false"></div>
                    <input type="text" id="current-page-value" value="10"></input>
                    <input type="button" value="페이지 리셋" onclick="setPaging(); return false;" />
                    <%--<div class="toolbar">
                      <input type="button" value="<" onclick="setPrevPage(); return false;" />
                      <span id="current-page-view"></span>/<span id="total-page-view"></span>
                      <input type="button" value=">" onclick="setNextPage(); return false;" />
                      <input type="text" id="current-page-value" value="10"></input>
                      <input type="button" value="페이지 리셋" onclick="setPaging(); return false;" />
                    </div>--%>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnExcel" EventName="Click" />
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
