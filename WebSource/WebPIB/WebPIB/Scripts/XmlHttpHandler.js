var xmlHttp;
var xmlHttpReadyState;
var XmlHandler = new fn_XmlHandler();
function fn_XmlHandler() {
    this.Http =
    {
    	LoadUrl: function (reqUrl, workType, mid, pid, shop1, line1, shop2, line2, vm) {
            try {
                xmlHttp = new window.XMLHttpRequest();
                xmlHttp.open("POST", reqUrl + "?mode=" + workType + "&mid=" + mid + "&pid=" + pid + "&shop1=" + shop1 + "&line1=" + line1 + "&shop2=" + shop2 + "&line2=" + line2 + "&vm=" + vm, false);
                xmlHttp.setRequestHeader("content-type", "application/x-www-form-urlencoded");
                xmlHttp.onloadend = function () {
                	if (xmlHttp.readyState == 4) {
                		if (xmlHttp.status == 200) {
                			if (xmlHttp.responseText != null && xmlHttp.responseText.length > 0) {

                				if (workType == "board") {
                					Board.Panel.LoadEnd(JSON.parse(xmlHttp.responseText));
                				}
                				else if (workType == "lineProd" || workType == "prod" || workType == "tprod" || workType == "tprodEv" || workType == "tprodPhev" || workType == "stock" || workType == "delivery" 
                                    || workType == "shipment" || workType == "stationMonitor" || workType == "stationMonitor2" || workType == "stationMonitor4" || workType == "stationMonitor5" || workType == "stationMonitor6" || workType == "dailyClosing" || workType == "dailyTot" || workType == "subContPIB")      // @@ KDK 추가
                				{
                					pib.LoadEnd(JSON.parse(xmlHttp.responseText));
                				}
                				else {
                					alm.LoadEnd(workType, JSON.parse(xmlHttp.responseText));
                				};
                			}
                			else {
                				if (workType == "board") {
                					Board.RecvTimeId = self.setTimeout(function () { Board.Panel.Load(); }, 3000);
                					//Board.Panel.Load();
                				}
                				else if (workType == "lineProd" || workType == "prod" || workType == "tprod" || workType == "tprodEv" || workType == "tprodPhev" || workType == "stock" || workType == "delivery"
                                    || workType == "shipment" || workType == "stationMonitor" || workType == "stationMonitor2" || workType == "stationMonitor4" || workType == "dailyClosing" || workType == "dailyTot" || workType == "subContPIB")      // @@ KDK 추가
                				{
                				    pib.ReceiveNewData();
                				}
                				else {
                					alm.ReceiveNewData();
                				};
                			};
                		}
                		else {
                			if (workType == "board") {
                				Board.RecvTimeId = self.setTimeout(function () { Board.Panel.Load(); }, 3000);
                				//Board.Panel.Load();
                			}
                			else if (workType == "lineProd" || workType == "prod" || workType == "tprod" || workType == "tprodEv" || workType == "tprodPhev" || workType == "stock" || workType == "delivery"
                            || workType == "shipment" || workType == "stationMonitor" || workType == "stationMonitor2" || workType == "stationMonitor4" || workType == "stationMonitor5" || workType == "stationMonitor6" || workType == "dailyClosing" || workType == "dailyTot" || workType == "subContPIB")      // @@ KDK 추가
                            {
                				pib.ReceiveNewData();
                			}
                			else {
                				alm.ReceiveNewData();
                			};
                		};
                	}
                	else {
                		if (workType == "board") {
                			Board.RecvTimeId = self.setTimeout(function () { Board.Panel.Load(); }, 3000);
                			//Board.Panel.Load();
                		}
                		else if (workType == "lineProd" || workType == "prod" || workType == "tprod" || workType == "tprodEv" || workType == "tprodPhev" || workType == "stock" || workType == "delivery"
                            || workType == "shipment" || workType == "stationMonitor" || workType == "stationMonitor2" || workType == "stationMonitor4" || workType == "stationMonitor5" || workType == "stationMonitor6" || workType == "dailyClosing" || workType == "dailyTot" || workType == "subContPIB")      // @@ KDK 추가 "dailyClosing"
                		{
                			pib.ReceiveNewData();
                		}
                		else {
                			alm.ReceiveNewData();
                		};
                	};
                };
                xmlHttp.onreadystatechange = function () {
                };
                xmlHttp.send(null);
            }
            catch (e) {
                try {
                    xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
                    xmlHttp.open("POST", reqUrl + "?mode=" + workType + "&mid=" + mid + "&pid=" + pid + "&shop1=" + shop1 + "&line1=" + line1 + "&shop2=" + shop2 + "&line2=" + line2 + "&vm=" + vm, false);
                    xmlHttp.setRequestHeader("content-type", "application/x-www-form-urlencoded");
                    xmlHttp.onreadystatechange = function () {
                    	if (xmlHttp.readyState == 4) {
                    		if (xmlHttp.status == 200) {
                    			if (xmlHttp.responseText != null && xmlHttp.responseText.length > 0) {
                    				return JSON.parse(xmlHttp.responseText);
                    			};
                    		};
                    	};
                    };
                    xmlHttp.send(null);
                } catch (e) { };
            };
        },
    };
};

function fn_CollectionSchema(key, value) {
    if (key) this.key = key;
    else this.key = "";

    if (value) this.value = value;
    else this.value = "";
};



var Request = new fn_Request();
function fn_Request() {
	this.getParameter = function (name) {
		var rtnval = '';
		var nowAddress = unescape(location.href);
		var parameters = (nowAddress.slice(nowAddress.indexOf('?') + 1, nowAddress.length)).split('&');

		for (var i = 0; i < parameters.length; i++) {
			var varName = parameters[i].split('=')[0];
			if (varName.toUpperCase() == name.toUpperCase()) {
				rtnval = parameters[i].split('=')[1];
				break;
			};
		};
		return rtnval;
	};
};
