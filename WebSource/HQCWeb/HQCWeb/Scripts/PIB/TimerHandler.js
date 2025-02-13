var timer = new TimerHandler();
function TimerHandler() {
	this.CurrDateText = null;
	this.CurrTimeText = null;
	this.CurrDate = null;
	this.CurrTime = null;
	this.dateObj = null;
	this.DataDispInterval = null;
	this.DisplayData = function () {
		try {
			if (pib.DataRecvStat == 0) {
				timer.CurrDate = null;
				timer.CurrTime = null;
				timer.dateObj = null;
				timer.CurrDate = document.getElementById("tdCurrDate");
				timer.CurrTime = document.getElementById("tdCurrTime");
				timer.CurrDateText = timer.CurrDate.innerText;
				timer.CurrTimeText = timer.CurrTime.innerText;
				if (timer.CurrDateText.length > 0 && timer.CurrTimeText.length > 0) {
					timer.dateObj = new Date(
						parseInt(timer.CurrDateText.substr(0, 4)),
						parseInt(timer.CurrDateText.substr(5, 2)),
						parseInt(timer.CurrDateText.substr(8, 2)),
						parseInt(timer.CurrTimeText.substr(0, 2)),
						parseInt(timer.CurrTimeText.substr(3, 2)),
						parseInt(timer.CurrTimeText.substr(6, 2)));

					timer.dateObj = new Date(timer.dateObj.valueOf() + 1000);
					timer.CurrTime.innerHTML
						= ((timer.dateObj.getHours().toString().length < 2) ? "0" : "") + timer.dateObj.getHours().toString()
						+ ":"
						+ ((timer.dateObj.getMinutes().toString().length < 2) ? "0" : "") + timer.dateObj.getMinutes().toString()
						+ ":"
						+ ((timer.dateObj.getSeconds().toString().length < 2) ? "0" : "") + timer.dateObj.getSeconds().toString();
				};
			};
			timer.DisplayMoreData();
		}
		catch (Exception) {
		};
	};

	this.DisplayMoreData = function () {
		if (timer.DataDispInterval != null) self.clearTimeout(timer.DataDispInterval);
		timer.DataDispInterval = self.setTimeout(function () { timer.DisplayData(); }, 1000);	// 10초단위
	};
};