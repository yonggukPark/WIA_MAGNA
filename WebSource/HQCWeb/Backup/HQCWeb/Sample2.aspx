<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Sample2.aspx.cs" Inherits="HQCWeb.Sample2" %>

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
            <h3><asp:Label ID="lbTitle" runat="server">WEB_09998</asp:Label></h3>
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
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="fn_WatingCall();" Visible="false" />
                        <input type="button" value="Excel" id="btnExcel" runat="server" visible="false" onclick="fn_excelExport('GridExcel'); return false;" />
                        <asp:Button ID="btnReload" runat="server" Text="Reload" OnClick="btnReload_Click" style="display:none;" />
                    </td>
                </tr>
            </table>
        </div>

        <div id="divCon1" style="width: 100%; height: 350px; display:none;" runat="server"></div>

        <div class="graph_wrap mt14" style="height:370px; overflow:auto;" id="divChart">
            <asp:UpdatePanel ID="up2" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="chartdiv" style="width: 100%; height: 350px;"></div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnReload" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <br />

        <p class="sub_tit">타이틀</p><!--통계수치 테이블의 타이틀-->

        <div style="width:100%; height:330px;">
            <asp:UpdatePanel runat="server" ID="up1" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="slide">
                        <a id="prev">prev</a>
                        <ul id="uImgs">
                            
                        </ul>
                        <a id="next">next</a>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnReload" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>


    <style>
      *{
        margin: 0; padding: 0;
      }
      .slide {
        width: 1280px;
        height: 300px;
        overflow: hidden;
        position: relative;
        margin: 0 auto;
      }
      .slide ul {
        width: 1280px;
        position: absolute;
        top:0;
        left:0;
        font-size: 0;
      }
      .slide ul li{
        display: inline-block;
      }
      #prev{
        position: absolute;
        top: 150px;
        left: 0;
        cursor: pointer;
        z-index: 1;
      }
      #next{
        position: absolute;
        top: 150px;
        right: 0;
        cursor: pointer;
        z-index: 1;
      }

    </style>

    <script>
        
        var chart;

        var chartData = [
            {
                "year": "202401",
                "visits": 4025,
                "color": "#FF0F00"
            },
            {
                "year": "202402",
                "visits": 1882,
                "color": "#FF6600"
            },
            {
                "year": "202403",
                "visits": 1809,
                "color": "#FF9E01"
            },
            {
                "year": "202404",
                "visits": 1322,
                "color": "#FCD202"
            },
            {
                "year": "202405",
                "visits": 1122,
                "color": "#F8FF01"
            },
            {
                "year": "202406",
                "visits": 1114,
                "color": "#B0DE09"
            },
            {
                "year": "202407",
                "visits": 984,
                "color": "#04D215"
            },
            {
                "year": "202408",
                "visits": 711,
                "color": "#0D8ECF"
            },
            {
                "year": "202409",
                "visits": 665,
                "color": "#0D52D1"
            }
        ];

        AmCharts.ready(function () {
            // SERIAL CHART
            chart = new AmCharts.AmSerialChart();
            chart.dataProvider = chartData;
            chart.categoryField = "year";
            // the following two lines makes chart 3D
            chart.depth3D = 20;
            chart.angle = 30;

            // AXES
            // category
            var categoryAxis = chart.categoryAxis;
            categoryAxis.labelRotation = 60;
            categoryAxis.dashLength = 5;
            categoryAxis.gridPosition = "start";
            

            // value
            var valueAxis = new AmCharts.ValueAxis();
            valueAxis.title = "Visitors";
            valueAxis.dashLength = 5;
            valueAxis.labelRotation = 360;
            chart.addValueAxis(valueAxis);

            // GRAPH
            var graph = new AmCharts.AmGraph();
            graph.valueField = "visits";
            graph.colorField = "color";
            graph.balloonText = "<span style='font-size:14px'>[[category]]: <b>[[value]]</b></span>";
            graph.type = "column";
            graph.lineAlpha = 0;
            graph.fillAlphas = 1;
            chart.addGraph(graph);

            // CURSOR
            var chartCursor = new AmCharts.ChartCursor();
            chartCursor.cursorAlpha = 0;
            chartCursor.zoomable = false;
            chartCursor.categoryBalloonEnabled = false;
            chart.addChartCursor(chartCursor);

            chart.creditsPosition = "top-right";
            
            // WRITE
            chart.write("chartdiv");
        });        

        // amcharts.js 파일 handleClick 이벤트에서 호출(라인 : 4339)
        function fn_ChartClick(_val) {

            var jsonData = JSON.stringify({ sParams: _val });

            $.ajax({
                type: "POST",
                url: "Sample2.aspx/SetImgCall",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    fn_AddImg(msg.d);
                }
            });
        }

        var imgs;
        var img_count;
        var img_position;

        function fn_AddImg(_val) {
            $("#uImgs").html(_val);

            img_position = 1;

            imgs = $(".slide ul");
            img_count = $("#uImgs li").length;

            var iWidth = 1280 * parseInt(img_count);

            $(".slide ul").attr("style", "width: " + iWidth + "px; position: absolute; top:0; left:0; font-size: 0;");

            prev.addEventListener("click", fn_prev)
            next.addEventListener("click", fn_next)
        }

        function fn_prev() {
            if (1 < img_position) {
                imgs.animate({
                    left: '+=1280px'
                });
                img_position--;
            }
        }

        function fn_next() {
            if (img_count > img_position) {
                imgs.animate({
                    left: '-=1280px'
                });
                img_position++;
            }
        }

        function fn_Load(_val, _val2) { 
            $("#MainContent_divCon" + _val).load(_val2);
        }

    </script>

</asp:Content>
