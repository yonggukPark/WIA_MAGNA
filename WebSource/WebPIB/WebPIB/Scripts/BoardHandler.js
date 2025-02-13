var Board = new fn_Board();

function fn_Board() {
	this.PanelObj = null;   // 판넬정보를 저장
	this.PanelSrcObj = null;   // 판넬정보를 저장
	this.PanelInfo = null;   // 판넬정보를 저장
	this.PanelCount = 0;   // 판넬갯수
	this.CurrentPanel = null;    // 현재 표시하는 판넬
	this.PreviousPanel = null;    // 현재 표시하는 판넬
	this.CurrentShowIdx = 0;    // 현재 표시하는 판넬 순번
	this.PreviousShowIdx = 0;    // 현재 표시하는 판넬 순번
	this.PanelTimeId = null;  // 타이머 id
	this.RecvTimeId = null;  // 타이머 id
	this.mid = null;
	this.RecvStatus = 0;
	this.RecvRestart = "0";
	this._nowDateMsec = (new Date()).getTime().toString(16);
	
	this.Panel =
    {
    	Load: function () {
    		Board.RecvStatus = 1;
    		Board.mid = Request.getParameter("mid");

    		if (Board.RecvTimeId != null) self.clearTimeout(Board.RecvTimeId);

    		XmlHandler.Http.LoadUrl("GetPibData.aspx", "board", Board.mid, "", "", "", "", "", "");
    		//if (Board.PanelInfo && Board.PanelInfo != null && Board.PanelInfo.length && Board.PanelInfo.length > 0) {
    		//	document.title = Board.PanelInfo[0].Data[0].MONI_DESC;
			//	// 재시작이면....
    		//	if (Board.PanelInfo[0].RESTART_FLAG && Board.PanelInfo[0].RESTART_FLAG != Board.RecvRestart) {
			//		// 재시작 했음
    		//		Board.RecvRestart = Board.PanelInfo[0].RESTART_FLAG;
			//		// 재시작
    		//		Board.Panel.Restart();
    		//	};
    		//	// 판넬레이아웃 생성
    		//	if (Board.PanelCount < 1) {
    		//		Board.Panel.Create();
    		//	};
    		//}
    		//else {
    		//	Board.RecvTimeId = self.setTimeout(function () { Board.Panel.Load(); }, 1000);
    		//};
    		//Board.RecvStatus = 0;
    		//Board.Panel.ShowHide();

    	},
    	LoadEnd: function (_jsonText) {
    		Board.mid = Request.getParameter("mid");
    		Board.PanelInfo = _jsonText;
    		if (Board.PanelInfo && Board.PanelInfo != null && Board.PanelInfo.length && Board.PanelInfo.length > 0) {
    			document.title = Board.PanelInfo[0].Data[0].MONI_DESC;
    			// 재시작이면....
    			if (Board.PanelInfo[0].RESTART_FLAG && Board.PanelInfo[0].RESTART_FLAG != Board.RecvRestart) {
    				// 재시작 했음
    				Board.RecvRestart = Board.PanelInfo[0].RESTART_FLAG;
    				// 재시작
    				Board.Panel.Restart();
    			};
    			// 판넬레이아웃 생성
    			if (Board.PanelCount < 1 || Board.PanelCount != Board.PanelInfo[0].Data.length) {
    				Board.Panel.Create();
    			};
    		}
    		else {
    			Board.RecvTimeId = self.setTimeout(function () { Board.Panel.Load(); }, 3000);
    		};
    		Board.RecvStatus = 0;
    		Board.Panel.ShowHide();
    	},
    	Restart : function()
    	{
    		for (var i = document.body.childNodes.length - 1; i >= 0; i--) {
    		    //document.body.childNodes[i].removeNode(true);
    		    document.body.removeChild(document.body.childNodes[i]);
    		};
    		Board.PanelCount = 0;
    	},
    	Create: function () {
    		if (Board.PanelInfo[0].Data && Board.PanelInfo[0].Data != null && Board.PanelInfo[0].Data.length > 0) {
    			Board.PanelCount = Board.PanelInfo[0].Data.length;
    			for (Board.CurrentShowIdx = Board.PanelInfo[0].Data.length - 1; Board.CurrentShowIdx >= 0; Board.CurrentShowIdx--) {
    				Board.PanelObj = document.createElement("DIV");
    				Board.PanelObj.id = "divBoard_" + (Board.CurrentShowIdx + 1);
    				Board.PanelObj.style.width = "100%";
    				Board.PanelObj.style.height = "100%";
    				Board.PanelObj.style.position = "absolute";
    				Board.PanelObj.style.display = "none";
    				Board.PanelObj.style.zIndex = Board.CurrentShowIdx;

    				Board.PanelSrcObj = document.createElement("IFRAME");
    				Board.PanelSrcObj.id = "ifrBoard_" + Board.CurrentShowIdx;
    				if (Board.PanelInfo[0].Data[Board.CurrentShowIdx].PIB_URL.indexOf("?") >= 0) {
    					Board.PanelSrcObj.src = Board.PanelInfo[0].Data[Board.CurrentShowIdx].PIB_URL + "&DocVer=" + ((new Date()).getTime().toString(16)) + "&mid=" + Board.PanelInfo[0].Data[Board.CurrentShowIdx].MONI_ID + "&pid=" + Board.PanelInfo[0].Data[Board.CurrentShowIdx].DISP_SEQ;
    				}
    				else {
    					Board.PanelSrcObj.src = Board.PanelInfo[0].Data[Board.CurrentShowIdx].PIB_URL + "?DocVer=" + ((new Date()).getTime().toString(16)) + "&mid=" + Board.PanelInfo[0].Data[Board.CurrentShowIdx].MONI_ID + "&pid=" + Board.PanelInfo[0].Data[Board.CurrentShowIdx].DISP_SEQ;
    				};
    				Board.PanelSrcObj.style.border = "0px";
    				Board.PanelSrcObj.style.width = "100%";
    				Board.PanelSrcObj.style.height = "100%";
    				Board.PanelSrcObj.style.position = "absolute";

    				Board.PanelObj.appendChild(Board.PanelSrcObj);
    				document.body.appendChild(Board.PanelObj);    				
    			};

    			Board.CurrentShowIdx = 1;
    	
    		}
    		else {
    			Board.RecvTimeId = self.setTimeout(function () { Board.Panel.Load(); }, 1000);
    		};
    	      
    	},
    	ShowHide: function () {
    		try {
    			if (Board.PanelTimeId) Board.Panel.stopInterval();

				// 데이터를 서버에서 받는 중이면 중지...
    			if (Board.RecvStatus == 1) return;

    			if (Board.PanelCount > 1 && Board.PreviousPanel) {
    				//Board.PreviousPanel.style.display = "none";
    				Board.CurrentPanel.style.opacity = "0";
    				//Board.PreviousPanel.style.width = "0px";
    			};

    			if (Board.CurrentShowIdx) {	

    				Board.CurrentPanel = document.getElementById("divBoard_" + Board.CurrentShowIdx);

    				if (Board.CurrentPanel) {

    				    // 2020.11.18. Added by KCLEE - Panel이 바뀔때 다시 로딩 한다.
    				    if (Board.PanelCount > 1)
    				    {
    				        if (Board.CurrentPanel.childNodes.length > 0) Board.CurrentPanel.childNodes[0].contentWindow.location.reload(true);
    				    }
    				        Board.CurrentPanel.style.width = "100%";
    				        Board.CurrentPanel.style.display = "";
    				        Board.CurrentPanel.style.opacity = "1";
    				        Board.PreviousPanel = Board.CurrentPanel;
    				    
    				};

    				if (Board.CurrentShowIdx < Board.PanelCount) Board.CurrentShowIdx++;
    				else {

						// 표시가 모두 끝났으면 서버에 정보를 받으러 다시 간다.
    					Board.CurrentShowIdx = 1;
    					Board.RecvTimeId = self.setTimeout(function () { Board.Panel.Load(); }, parseInt(Board.PanelInfo[0].Data[Board.CurrentShowIdx - 1].ALIVE_TM) * 1000);
     				};
    			};
    			Board.Panel.InitShowInterval();

    		}
    		catch (exception) {
    		    Board.Panel.InitShowInterval();
    		};
    	},
    	stopInterval: function () {
    		try {
    			if(Board.PanelTimeId != null) self.clearTimeout(Board.PanelTimeId);
    		}
    		catch (exception) {
    		};
    	},
    	InitShowInterval: function () {
    	    try {    	     
    	        Board.PanelTimeId = self.setTimeout(function () { Board.Panel.ShowHide(); }, parseInt(Board.PanelInfo[0].Data[Board.CurrentShowIdx - 1].ALIVE_TM) * 1000);    	      

    		}
    		catch (exception) {
    		};
    	},
    	InitShowInterval2: function () {
    	    try {
    	        Board.CurrentShowIdx = 0;
    	        Board.PanelTimeId = self.setTimeout(function () { Board.Panel.ShowHide(); }, parseInt(Board.PanelInfo[0].Data[Board.CurrentShowIdx+1].ALIVE_TM) * 1000);

    	    }
    	    catch (exception) {
    	    };
    	}

    };
};
