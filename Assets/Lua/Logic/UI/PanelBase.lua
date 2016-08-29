local PanelBase = luaclass("PanelBase", require("Logic/MVC/ViewBase"))

local LoadState = 
{
	Init = 1,
	Loading = 2,
	Loaded = 3,
	Unloaded = 4,
}

function PanelBase:initialize()
	self.go = nil
	self.transform = nil
	self.rectTransform = nil
	self.isActive = false
	self.state = LoadState.Init
end

function PanelBase:Show()
	if self.isActive then
		return
	end
	if self.state == LoadState.Loaded then
		self:OnPreShow()
		self:gameObject:SetActive(true)
		self:OnShow()
	else
end


function PanelBase:Hide()
end

function PanelBase:Init()
end

function PanelBase:InitUI()
end

function PanelBase:OnPreShow()
end

function PanelBase:OnShow()
end



function PanelBase:FindChild(path)
	if nil == self.transform then
		return nil
	end
	return self.transform:FindChild(path)
end

function PanelBase:GetComponent(path, type)
	local tr = self.FindChild(path)
	if tr == nil then
		return nil
	end
	return tr.gameObject:GetComponent(type)
end

function PanelBase:AddComponet(path, type)
	local tr = self.FindChild(path)
	if tr == nil then
		return nil
	end
	tr.gameObject:AddComponet(type)
end