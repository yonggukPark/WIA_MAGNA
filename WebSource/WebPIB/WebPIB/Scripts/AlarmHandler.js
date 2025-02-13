var alm = new Alarm();
function Alarm() {
	this.DataRecvIntervalValue = 2000;
	this.DataRecvInterval = null;
	this.DataDispInterval = null;
	this.receivedData = null;
	this.workType = null;
	this.workData = null;
	this.AlarmArea = null;
	this.BodyArea = null;
	this.AlarmMsg = null;
	this.AlarmExist = false;
	this.GetData = function (p_workType) {
		var workType = Request.getParameter("mode");
		var mid = Request.getParameter("mid");
		var pid = Request.getParameter("pid");
		alm.BodyArea = document.getElementById("pibBody");
		alm.AlarmArea = document.getElementById("trAlarmMsg");
		alm.AlarmMsg = document.getElementById("pAlarmMsg");
		alm.workType = p_workType;
		alm.AlarmExist = false;
		//alm.receivedData =
		XmlHandler.Http.LoadUrl("GetPibData.aspx", alm.workType, mid, pid, "", "", "", "", "", workType);
		//if (alm.receivedData && alm.receivedData != null && alm.receivedData.length && alm.receivedData.length > 0) {
		//	for (var i = 0; i < alm.receivedData.length; i++) {
		//		if (alm.receivedData[i].TableName == mid) {
		//			alm.workData = alm.receivedData[i].Data;
		//			alm.AlarmExist = true;
		//			break;
		//		};
		//	};

		//	if (alm.AlarmExist) {
		//		if (p_workType == "deliveryAlm" || p_workType == "stockAlm") {
		//			if (alm.AlarmArea.style.display == "none") {
		//				alm.AlarmArea.style.display = "";
		//				alm.BodyArea.style.height = "78%";
		//				alm.AlarmArea.style.height = "7%";
		//			};
		//		}
		//		else {
		//			if (alm.AlarmArea.style.display == "none") {
		//				alm.AlarmArea.style.display = "";
		//				alm.BodyArea.style.height = "70%";
		//				alm.AlarmArea.style.height = "15%";
		//			};
		//		};
		//	}
		//	else {
		//		if (alm.AlarmArea.style.display == "") {
		//			alm.AlarmArea.style.display = "none";
		//			alm.AlarmMsg.innerHTML = "";
		//			alm.BodyArea.style.height = "100%";
		//			alm.AlarmArea.style.height = "0%";
		//		};
		//	};
		//}
		//else {
		//	alm.AlarmArea.style.display = "none";
		//	alm.AlarmMsg.innerHTML = "";
		//	alm.BodyArea.style.height = "100%";
		//	alm.AlarmArea.style.height = "0%";
		//};
		//alm.DisplayData(true);
	};
	this.LoadEnd = function (p_workType, _jsonText) {
		var workType = Request.getParameter("mode");
		var mid = Request.getParameter("mid");
		var pid = Request.getParameter("pid");
		alm.BodyArea = document.getElementById("pibBody");
		alm.AlarmArea = document.getElementById("trAlarmMsg");
		alm.AlarmMsg = document.getElementById("pAlarmMsg");
		alm.workType = p_workType;
		alm.AlarmExist = false;
		alm.receivedData = _jsonText;
		if (alm.receivedData && alm.receivedData != null && alm.receivedData.length && alm.receivedData.length > 0) {
			for (var i = 0; i < alm.receivedData.length; i++) {
				if (alm.receivedData[i].TableName == mid) {
					alm.workData = alm.receivedData[i].Data;
					alm.AlarmExist = true;
					break;
				};
			};

			if (alm.AlarmExist) {
				if (p_workType == "deliveryAlm" || p_workType == "stockAlm") {
					if (alm.AlarmArea.style.display == "none") {
						alm.AlarmArea.style.display = "";
						alm.BodyArea.style.height = "78%";
						alm.AlarmArea.style.height = "7%";
					};
				}
				else {
					if (alm.AlarmArea.style.display == "none") {
						alm.AlarmArea.style.display = "";
						alm.BodyArea.style.height = "70%";
						alm.AlarmArea.style.height = "15%";
					};
				};
			}
			else {
				if (alm.AlarmArea.style.display == "") {
					alm.AlarmArea.style.display = "none";
					alm.AlarmMsg.innerHTML = "";
					alm.BodyArea.style.height = "100%";
					alm.AlarmArea.style.height = "0%";
				};
			};
		}
		else {
			alm.AlarmArea.style.display = "none";
			alm.AlarmMsg.innerHTML = "";
			alm.BodyArea.style.height = "100%";
			alm.AlarmArea.style.height = "0%";
		};
		alm.DisplayData(true);
	};

	this.DisplayData = function (isReceived) {
		var nRowIdx = 0;
		if (alm.workData && alm.workData != null && alm.workData.length && alm.workData.length > 0) {

			if (alm.workData[0].BackColor) {
				if (alm.AlarmMsg.innerHTML != alm.workData[0].c1) {
					alm.AlarmArea.style.backgroundColor = alm.workData[0].BackColor;
					alm.AlarmMsg.style.backgroundColor = alm.workData[0].BackColor;
					alm.AlarmMsg.innerHTML = alm.workData[0].c1;
				};
			}
			else {
				if (alm.AlarmMsg.innerHTML != alm.workData[0].c1) {
					alm.AlarmArea.style.backgroundColor = "white";
					alm.AlarmMsg.style.backgroundColor = "white";
					alm.AlarmMsg.innerHTML = alm.workData[0].c1;
				};
			};

			alm.workData.shift(); // 첫번째 배열데이터를 삭제한다.
		}
		else {
			if (isReceived) {
				alm.AlarmArea.style.display = "none";
				alm.AlarmMsg.innerHTML = "";
				alm.BodyArea.style.height = "100%";
				alm.AlarmArea.style.height = "0%";
			};
		};

		// 가져온 데이터가 화면에 표시할 수량보다 많이 남아있으면...
		if (alm.workData && alm.workData != null && alm.workData.length && alm.workData.length > 0) {
			// 첫줄부터 다시 표시한다.
			alm.DisplayMoreData();
		}
		else {
			alm.DataRecvIntervalValue = 2000;
			alm.ReceiveNewData();
		};
	};

	this.DisplayMoreData = function () {
		if (alm.DataDispInterval != null) self.clearTimeout(alm.DataDispInterval);
		alm.DataDispInterval = self.setTimeout(function () { alm.DisplayData(false); }, 2000);	// 10초단위
	};
	this.ReceiveNewData = function () {
		if (alm.DataRecvInterval != null) self.clearTimeout(alm.DataRecvInterval);
		alm.DataRecvInterval = self.setTimeout(function () { alm.GetData(alm.workType); }, alm.DataRecvIntervalValue);	// 2초단위
		alm.DataRecvIntervalValue = 2000;
	};
};