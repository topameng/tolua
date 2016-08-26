require("Common/Define")

Main = {}

local function Init()
	ModelManager.Init()
	ControlManager.Init()
	ViewManager.Init()
end
--主入口函数。从这里开始lua逻辑
function GameMain()					
	Init()
	Main.UIRoot = GameObject.Find("UI/Canvas")
	--ResourceManager.LoadAsset("UI/PanelTest", function (obj) end)
end



--场景切换通知
function OnLevelWasLoaded(level)
	Time.timeSinceLevelLoad = 0
end


function Main.Update()
	ControlManager.Update()
	ViewManager.Update()
end

function Main.LateUpate()
	EventManger.LateUpate()
end