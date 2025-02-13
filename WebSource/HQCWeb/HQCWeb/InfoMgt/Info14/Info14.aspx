<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Site.Master" AutoEventWireup="true" CodeBehind="Info14.aspx.cs" Inherits="HQCWeb.InfoMgt.Info14.Info14" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function fn_Validation() {
            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlPtnCd").val() == "") {
                alert("작업패턴을 선택하세요.");
                return false;
            } else {
                fn_WatingCall();
                return true;
            }
            return true;
        }

        function fn_Validation2() {
            if ($("#MainContent_ddlShopCd").val() == "") {
                alert("Shop Code를 선택하세요.");
                return false;
            } else if ($("#MainContent_ddlLineCd").val() == "") {
                alert("라인을 선택하세요.");
                return false;
            } else if ($("#MainContent_txtPtnCd").val() == "") {
                alert("패턴코드를 입력하세요.");
                return false;
            } else if ($("#MainContent_txtPtnNm").val() == "") {
                alert("패턴코드명을 입력하세요.");
                return false;
            } else {
                fn_WatingCall();
                return true;
            }
            return true;
        }

        function fn_Validation3() {
            if (confirm("삭제 하시겠습니까?")) {
                fn_WatingCall();
                return true;
            } else {
                return false;
            }
        }

        function fn_parentReload() {
            <%= Page.GetPostBackEventReference(btnSearch) %>
        }

        var data;

        //패턴 관련
        $(document).ready(function () {
            settingGrid();
        });

        //패턴그리드 출력
        function createGrid() {
            var hour, min;

            //selectedhour reset
            lbSelHour.innerHTML = '';
            cb1.checked = false;

            data.forEach((row, i) => {
                row.forEach((value, j) => {
                    hour = (i > 0) ? i.toString().padStart(2, '0') : '24';
                    min = ((j + 1) * 5).toString().padStart(2, '0');

                    if (value === "0") {
                        document.getElementById('h' + hour + 'm' + min).className = '';
                    } else {
                        document.getElementById('h' + hour + 'm' + min).className = 'use';
                    }
                });
            });

            settingGrid();
        }

        //hidValue
        //분단위 체크
        function fn_Minute_Checked(str) {

            $.ajax({
                type: "POST",
                url: "Info14.aspx/GetPattern",
                data: JSON.stringify({ clicked : str }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                },
                error: function (response) {
                    alert("에러가 발생했습니다. 관리자에게 문의바립니다.");
                }
            });
        }


        //패턴로직 설정
        function settingGrid() {
            const items = document.querySelectorAll('.no-wrap li');
            const headers = document.querySelectorAll('.hr h4');
            const checkbox = document.getElementById('cb1');


            //분
            items.forEach(item => {
                // 기존 이벤트 제거
                item.removeEventListener('click', fn_item_Click);

                // 새로운 이벤트 추가
                item.addEventListener('click', fn_item_Click);
            });

            //시간
            headers.forEach(header => {
                header.addEventListener('click', function () {
                    headers.forEach(header => {
                        header.className = '';
                    });

                    this.className = 'chk';
                    var i = 0;
                    lbSelHour.innerHTML = this.innerHTML;
                    checkbox.checked = false;
                    items.forEach(item => {
                        if (document.getElementById('h' + this.innerHTML + item.id).className == 'use') {
                            document.getElementById(item.id).className = 'use2';
                            fn_Minute_Checked(this.innerHTML + item.id + '/1');
                            i++;
                        }
                        else {
                            document.getElementById(item.id).className = '';
                            fn_Minute_Checked(this.innerHTML + item.id + '/0');
                        }
                    });

                    if(i == items.length)
                        checkbox.checked = true;

                });
            });



            // 기존 이벤트 제거
            checkbox.removeEventListener('click', fn_check_Click);

            // 새로운 이벤트 추가
            checkbox.addEventListener('click', fn_check_Click);
        }

        //아이템 클릭이벤트 함수
        function fn_item_Click() {
            const currentItem = document.getElementById(this.id);

            if (lbSelHour.innerHTML != '') {
                headers.forEach(header => {
                    if (header.className == 'chk') {
                        if (currentItem.className == '') {
                            currentItem.className = 'use2';
                            document.getElementById('h' + header.innerHTML + this.id).className = 'use';
                            fn_Minute_Checked(header.innerHTML + this.id + '/1');
                        }
                        else {
                            currentItem.className = '';
                            document.getElementById('h' + header.innerHTML + this.id).className = '';
                            fn_Minute_Checked(header.innerHTML + this.id + '/0');
                        }
                    }
                });
            }
            else {
                alert("시간을 선택하세요.");
            }
        }

        //체크박스 클릭이벤트 함수
        function fn_check_Click() {
            const items = document.querySelectorAll('.no-wrap li');
            if (lbSelHour.innerHTML == '') {
                this.checked = !this.checked;
                alert("시간을 선택하세요.");
            }
            else if (this.checked) {
                items.forEach(item => {
                    if (document.getElementById(item.id).className === '') {
                        document.getElementById(item.id).className = 'use2';
                        document.getElementById('h' + lbSelHour.innerHTML + item.id).className = 'use';
                        //fn_Minute_Checked(lbSelHour.innerHTML + item.id + '/1');
                    }
                });
                fn_Minute_Checked(lbSelHour.innerHTML + 'm65/1');
            }
            else {
                items.forEach(item => {
                    if (document.getElementById(item.id).className === 'use2') {
                        document.getElementById(item.id).className = '';
                        document.getElementById('h' + lbSelHour.innerHTML + item.id).className = '';
                        //fn_Minute_Checked(lbSelHour.innerHTML + item.id + '/0');
                    }
                });
                fn_Minute_Checked(lbSelHour.innerHTML + 'm65/0');
            }
        }

    </script>

    <asp:HiddenField ID="hidWidth" runat="server" Value="500" />
    <asp:HiddenField ID="hidHeight" runat="server" Value="500" />

    <div class="contents" tabindex="0">
        <!--// Title + Win_Btn -->
        <div class="title">
            <h3><asp:Label ID="lbTitle" runat="server">Info14</asp:Label></h3>
            <div class="al-r">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                <asp:Button ID="btnSave"   runat="server" Text="Save"   OnClick="btnSave_Click"   OnClientClick="javascript:return fn_Validation2();" Visible="false" />
                <asp:Button ID="btnModify" runat="server" Text="Modify" OnClick="btnModify_Click" OnClientClick="javascript:return fn_Validation2();" Visible="false" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" OnClientClick="javascript:return fn_Validation3();" Visible="false" />
            </div>
        </div>
        <!-- Title + Win_Btn //-->
        <!--// Grid Search -->
        <div class="search_grid mt13">
            <table cellpadding="0" cellspacing="0">
                <colgroup>
                    <col style="width:60px;">
                    <col style="width:120px;">
                    <col style="width:60px;">
                    <col style="width:220px;">
                    <col style="width:60px;">
                    <col style="width:250px;">
                </colgroup>
                <tr>
                    <th><asp:Label ID="lbShopCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:DropDownList ID="ddlShopCd" runat="server" OnSelectedIndexChanged="ddlShopCd_SelectedIndexChanged" AutoPostBack="true" Width="100"></asp:DropDownList>
                    </td>
                    <th><asp:Label ID="lbLineCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="up2" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlLineCd" runat="server" OnSelectedIndexChanged="ddlLineCd_SelectedIndexChanged" AutoPostBack="true" Width="250"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbPtnCd" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlPtnCd" runat="server" Width="250"></asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlShopCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlLineCd" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnModify" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td class="al-r">
                        <%--<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" OnClientClick="javascript:return fn_Validation();" Visible="false" />
                        <asp:Button ID="btnSave"   runat="server" Text="Save"   OnClick="btnSave_Click"   OnClientClick="javascript:return fn_Validation2();" Visible="false" />
                        <asp:Button ID="btnModify" runat="server" Text="Modify" OnClick="btnModify_Click" OnClientClick="javascript:return fn_Validation2();" Visible="false" />
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" OnClientClick="javascript:return fn_Validation3();" Visible="false" />--%>
                    </td>
                </tr>
                <tr>
                    <th><asp:Label ID="lbPtnCdNm" runat="server"></asp:Label></th>
                    <td>
                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:TextBox ID="txtPtnCd" runat="server"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <th><asp:Label ID="lbPtnNm" runat="server"></asp:Label></th>
                    <td colspan="4">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel4" UpdateMode="Conditional">
                            <ContentTemplate>
                        <asp:TextBox ID="txtPtnNm" runat="server"></asp:TextBox>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="pattern_wrap">
                    <div class="top mt30">
                        <p class="left">Selected Hour : <label id="lbSelHour"></label> </p>
                        <p><label for="cb1">전체선택</label><input type="checkbox" id="cb1" class="ml10"></p>
                    </div>
                        
                    <div class="mt20 time-box">
                        <div class="hour">
                            <div class="no">
                                <h4>00</h4>
                                <div class="no-wrap">
                                    <ul>
                                        <li id="m05" ></li>
                                        <li id="m10" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="no">
                                <h4>10</h4>
                                <div class="no-wrap">
                                    <ul>
                                        <li id="m15" ></li>
                                        <li id="m20" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="no">
                                <h4>20</h4>
                                <div class="no-wrap">
                                    <ul>
                                        <li id="m25" ></li>
                                        <li id="m30" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="no">
                                <h4>30</h4>
                                <div class="no-wrap">
                                    <ul>
                                        <li id="m35" ></li>
                                        <li id="m40" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="no">
                                <h4>40</h4>
                                <div class="no-wrap">
                                    <ul>
                                        <li id="m45" ></li>
                                        <li id="m50" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="no">
                                <h4>50</h4>
                                <div class="no-wrap">
                                    <ul>
                                        <li id="m55" ></li>
                                        <li id="m60" ></li>
                                    </ul>
                                </div>
                            </div>

                            <div class="hr">
                                <h4>08</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h08m05" ></li>
                                        <li id="h08m10" ></li>
                                        <li id="h08m15" ></li>
                                        <li id="h08m20" ></li>
                                        <li id="h08m25" ></li>
                                        <li id="h08m30" ></li>
                                        <li id="h08m35" ></li>
                                        <li id="h08m40" ></li>
                                        <li id="h08m45" ></li>
                                        <li id="h08m50" ></li>
                                        <li id="h08m55" ></li>
                                        <li id="h08m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>09</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h09m05" ></li>
                                        <li id="h09m10" ></li>
                                        <li id="h09m15" ></li>
                                        <li id="h09m20" ></li>
                                        <li id="h09m25" ></li>
                                        <li id="h09m30" ></li>
                                        <li id="h09m35" ></li>
                                        <li id="h09m40" ></li>
                                        <li id="h09m45" ></li>
                                        <li id="h09m50" ></li>
                                        <li id="h09m55" ></li>
                                        <li id="h09m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>10</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h10m05" ></li>
                                        <li id="h10m10" ></li>
                                        <li id="h10m15" ></li>
                                        <li id="h10m20" ></li>
                                        <li id="h10m25" ></li>
                                        <li id="h10m30" ></li>
                                        <li id="h10m35" ></li>
                                        <li id="h10m40" ></li>
                                        <li id="h10m45" ></li>
                                        <li id="h10m50" ></li>
                                        <li id="h10m55" ></li>
                                        <li id="h10m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>11</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h11m05" ></li>
                                        <li id="h11m10" ></li>
                                        <li id="h11m15" ></li>
                                        <li id="h11m20" ></li>
                                        <li id="h11m25" ></li>
                                        <li id="h11m30" ></li>
                                        <li id="h11m35" ></li>
                                        <li id="h11m40" ></li>
                                        <li id="h11m45" ></li>
                                        <li id="h11m50" ></li>
                                        <li id="h11m55" ></li>
                                        <li id="h11m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>12</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h12m05" ></li>
                                        <li id="h12m10" ></li>
                                        <li id="h12m15" ></li>
                                        <li id="h12m20" ></li>
                                        <li id="h12m25" ></li>
                                        <li id="h12m30" ></li>
                                        <li id="h12m35" ></li>
                                        <li id="h12m40" ></li>
                                        <li id="h12m45" ></li>
                                        <li id="h12m50" ></li>
                                        <li id="h12m55" ></li>
                                        <li id="h12m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>13</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h13m05" ></li>
                                        <li id="h13m10" ></li>
                                        <li id="h13m15" ></li>
                                        <li id="h13m20" ></li>
                                        <li id="h13m25" ></li>
                                        <li id="h13m30" ></li>
                                        <li id="h13m35" ></li>
                                        <li id="h13m40" ></li>
                                        <li id="h13m45" ></li>
                                        <li id="h13m50" ></li>
                                        <li id="h13m55" ></li>
                                        <li id="h13m60" ></li>
                                    </ul>
                                </div>
                            </div>

                            <div class="hr">
                                <h4>14</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h14m05" ></li>
                                        <li id="h14m10" ></li>
                                        <li id="h14m15" ></li>
                                        <li id="h14m20" ></li>
                                        <li id="h14m25" ></li>
                                        <li id="h14m30" ></li>
                                        <li id="h14m35" ></li>
                                        <li id="h14m40" ></li>
                                        <li id="h14m45" ></li>
                                        <li id="h14m50" ></li>
                                        <li id="h14m55" ></li>
                                        <li id="h14m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>15</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h15m05" ></li>
                                        <li id="h15m10" ></li>
                                        <li id="h15m15" ></li>
                                        <li id="h15m20" ></li>
                                        <li id="h15m25" ></li>
                                        <li id="h15m30" ></li>
                                        <li id="h15m35" ></li>
                                        <li id="h15m40" ></li>
                                        <li id="h15m45" ></li>
                                        <li id="h15m50" ></li>
                                        <li id="h15m55" ></li>
                                        <li id="h15m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>16</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h16m05" ></li>
                                        <li id="h16m10" ></li>
                                        <li id="h16m15" ></li>
                                        <li id="h16m20" ></li>
                                        <li id="h16m25" ></li>
                                        <li id="h16m30" ></li>
                                        <li id="h16m35" ></li>
                                        <li id="h16m40" ></li>
                                        <li id="h16m45" ></li>
                                        <li id="h16m50" ></li>
                                        <li id="h16m55" ></li>
                                        <li id="h16m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>17</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h17m05" ></li>
                                        <li id="h17m10" ></li>
                                        <li id="h17m15" ></li>
                                        <li id="h17m20" ></li>
                                        <li id="h17m25" ></li>
                                        <li id="h17m30" ></li>
                                        <li id="h17m35" ></li>
                                        <li id="h17m40" ></li>
                                        <li id="h17m45" ></li>
                                        <li id="h17m50" ></li>
                                        <li id="h17m55" ></li>
                                        <li id="h17m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>18</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h18m05" ></li>
                                        <li id="h18m10" ></li>
                                        <li id="h18m15" ></li>
                                        <li id="h18m20" ></li>
                                        <li id="h18m25" ></li>
                                        <li id="h18m30" ></li>
                                        <li id="h18m35" ></li>
                                        <li id="h18m40" ></li>
                                        <li id="h18m45" ></li>
                                        <li id="h18m50" ></li>
                                        <li id="h18m55" ></li>
                                        <li id="h18m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>19</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h19m05" ></li>
                                        <li id="h19m10" ></li>
                                        <li id="h19m15" ></li>
                                        <li id="h19m20" ></li>
                                        <li id="h19m25" ></li>
                                        <li id="h19m30" ></li>
                                        <li id="h19m35" ></li>
                                        <li id="h19m40" ></li>
                                        <li id="h19m45" ></li>
                                        <li id="h19m50" ></li>
                                        <li id="h19m55" ></li>
                                        <li id="h19m60" ></li>
                                    </ul>
                                </div>
                            </div>

                            <div class="hr">
                                <h4>20</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h20m05" ></li>
                                        <li id="h20m10" ></li>
                                        <li id="h20m15" ></li>
                                        <li id="h20m20" ></li>
                                        <li id="h20m25" ></li>
                                        <li id="h20m30" ></li>
                                        <li id="h20m35" ></li>
                                        <li id="h20m40" ></li>
                                        <li id="h20m45" ></li>
                                        <li id="h20m50" ></li>
                                        <li id="h20m55" ></li>
                                        <li id="h20m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>21</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h21m05" ></li>
                                        <li id="h21m10" ></li>
                                        <li id="h21m15" ></li>
                                        <li id="h21m20" ></li>
                                        <li id="h21m25" ></li>
                                        <li id="h21m30" ></li>
                                        <li id="h21m35" ></li>
                                        <li id="h21m40" ></li>
                                        <li id="h21m45" ></li>
                                        <li id="h21m50" ></li>
                                        <li id="h21m55" ></li>
                                        <li id="h21m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>22</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h22m05" ></li>
                                        <li id="h22m10" ></li>
                                        <li id="h22m15" ></li>
                                        <li id="h22m20" ></li>
                                        <li id="h22m25" ></li>
                                        <li id="h22m30" ></li>
                                        <li id="h22m35" ></li>
                                        <li id="h22m40" ></li>
                                        <li id="h22m45" ></li>
                                        <li id="h22m50" ></li>
                                        <li id="h22m55" ></li>
                                        <li id="h22m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>23</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h23m05" ></li>
                                        <li id="h23m10" ></li>
                                        <li id="h23m15" ></li>
                                        <li id="h23m20" ></li>
                                        <li id="h23m25" ></li>
                                        <li id="h23m30" ></li>
                                        <li id="h23m35" ></li>
                                        <li id="h23m40" ></li>
                                        <li id="h23m45" ></li>
                                        <li id="h23m50" ></li>
                                        <li id="h23m55" ></li>
                                        <li id="h23m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>24</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h24m05" ></li>
                                        <li id="h24m10" ></li>
                                        <li id="h24m15" ></li>
                                        <li id="h24m20" ></li>
                                        <li id="h24m25" ></li>
                                        <li id="h24m30" ></li>
                                        <li id="h24m35" ></li>
                                        <li id="h24m40" ></li>
                                        <li id="h24m45" ></li>
                                        <li id="h24m50" ></li>
                                        <li id="h24m55" ></li>
                                        <li id="h24m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>01</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h01m05" ></li>
                                        <li id="h01m10" ></li>
                                        <li id="h01m15" ></li>
                                        <li id="h01m20" ></li>
                                        <li id="h01m25" ></li>
                                        <li id="h01m30" ></li>
                                        <li id="h01m35" ></li>
                                        <li id="h01m40" ></li>
                                        <li id="h01m45" ></li>
                                        <li id="h01m50" ></li>
                                        <li id="h01m55" ></li>
                                        <li id="h01m60" ></li>
                                    </ul>
                                </div>
                            </div>

                            <div class="hr">
                                <h4>02</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h02m05" ></li>
                                        <li id="h02m10" ></li>
                                        <li id="h02m15" ></li>
                                        <li id="h02m20" ></li>
                                        <li id="h02m25" ></li>
                                        <li id="h02m30" ></li>
                                        <li id="h02m35" ></li>
                                        <li id="h02m40" ></li>
                                        <li id="h02m45" ></li>
                                        <li id="h02m50" ></li>
                                        <li id="h02m55" ></li>
                                        <li id="h02m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>03</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h03m05" ></li>
                                        <li id="h03m10" ></li>
                                        <li id="h03m15" ></li>
                                        <li id="h03m20" ></li>
                                        <li id="h03m25" ></li>
                                        <li id="h03m30" ></li>
                                        <li id="h03m35" ></li>
                                        <li id="h03m40" ></li>
                                        <li id="h03m45" ></li>
                                        <li id="h03m50" ></li>
                                        <li id="h03m55" ></li>
                                        <li id="h03m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>04</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h04m05" ></li>
                                        <li id="h04m10" ></li>
                                        <li id="h04m15" ></li>
                                        <li id="h04m20" ></li>
                                        <li id="h04m25" ></li>
                                        <li id="h04m30" ></li>
                                        <li id="h04m35" ></li>
                                        <li id="h04m40" ></li>
                                        <li id="h04m45" ></li>
                                        <li id="h04m50" ></li>
                                        <li id="h04m55" ></li>
                                        <li id="h04m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>05</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h05m05" ></li>
                                        <li id="h05m10" ></li>
                                        <li id="h05m15" ></li>
                                        <li id="h05m20" ></li>
                                        <li id="h05m25" ></li>
                                        <li id="h05m30" ></li>
                                        <li id="h05m35" ></li>
                                        <li id="h05m40" ></li>
                                        <li id="h05m45" ></li>
                                        <li id="h05m50" ></li>
                                        <li id="h05m55" ></li>
                                        <li id="h05m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>06</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h06m05" ></li>
                                        <li id="h06m10" ></li>
                                        <li id="h06m15" ></li>
                                        <li id="h06m20" ></li>
                                        <li id="h06m25" ></li>
                                        <li id="h06m30" ></li>
                                        <li id="h06m35" ></li>
                                        <li id="h06m40" ></li>
                                        <li id="h06m45" ></li>
                                        <li id="h06m50" ></li>
                                        <li id="h06m55" ></li>
                                        <li id="h06m60" ></li>
                                    </ul>
                                </div>
                            </div>
                            <div class="hr">
                                <h4>07</h4>
                                <div class="hr-wrap">
                                    <ul>
                                        <li id="h07m05" ></li>
                                        <li id="h07m10" ></li>
                                        <li id="h07m15" ></li>
                                        <li id="h07m20" ></li>
                                        <li id="h07m25" ></li>
                                        <li id="h07m30" ></li>
                                        <li id="h07m35" ></li>
                                        <li id="h07m40" ></li>
                                        <li id="h07m45" ></li>
                                        <li id="h07m50" ></li>
                                        <li id="h07m55" ></li>
                                        <li id="h07m60" ></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnModify" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>