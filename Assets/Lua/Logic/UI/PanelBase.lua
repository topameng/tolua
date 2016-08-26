local PanelBase = luaclass("PanelBase", require("Logic/MVC/ViewBase"))

local LoadState = 
{
	Init = 1,
	Loading = 2,
	Loaded = 3,
}

function PanelBase:initialize()
	self.go = nil
	self.transform = nil
	self.rectTransform = nil
	self.isActive = false
end

function PanelBase:Init()
end

function PanelBase:InitUI()
end




function PanelBase:FindChild(path)
	if nil == self.transform then
		return
	end
	return self.transform:FindChild(path)
end

