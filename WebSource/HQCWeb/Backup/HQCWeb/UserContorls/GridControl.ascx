<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridControl.ascx.cs" Inherits="HQCWeb.UserContorls.GridControl" %>

<%--<dx:ASPxGridView ID="grid" ClientInstanceName="grid" 
    runat="server" Width="100%" 
    AutoGenerateColumns="false" 
    EnableRowsCache="false">
    <Settings VerticalScrollBarMode="Visible" VerticalScrollBarStyle="VirtualSmooth"  />
    <SettingsPager Mode="EndlessPaging"  PageSize="100000"/>
    <SettingsLoadingPanel Mode="Disabled" />
    <Styles>
        <Header HorizontalAlign="Center"  BackColor="#edf5fa" ForeColor="#2e6e99" />  
    </Styles>
    <ClientSideEvents EndCallback="function(s, e) { fn_SortReSearch(); }" />
</dx:ASPxGridView>--%>



<div id="realgrid" style="width: 100%; height: 440px;"></div>
<table>
    <tr>
        <td>
            <div class="toolbar">
                <div id="gridPage" style="float:left" onselectstart="return false" ondragstart="return false"></div>
                &nbsp;
                <select id="current_page_value" onchange="setPaging(); return false;" style="font-size:12px; display:none; margin-top:2px;" runat="server">
                </select>
                <input type="button" value="Set" id="btnSet" onclick="getCol('<%=strMenuID%>'); return false;" style="display:none; margin-top:2px;" />
            </div>
        </td>
    </tr>
</table>