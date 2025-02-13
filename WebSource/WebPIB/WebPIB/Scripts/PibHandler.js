function resizePanel() {
	var _screenWidth = window.innerWidth;
	var _screenHeight = window.innerHeight;
	var _baseWidth = 1024;
	var _baseHeight = 600;

	var _panelLayOut = document.querySelector("#docBody > table");
	_panelLayOut.style.width = _baseWidth + "px";
	_panelLayOut.style.height = _baseHeight + "px";
	_panelLayOut.style["transform-origin"] = "left top";
	_panelLayOut.style["transform"] = "scale(" + (_screenWidth / _baseWidth) + "," + (_screenHeight / _baseHeight) + ")";
};