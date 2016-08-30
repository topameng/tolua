local ViewBase = require("Logic/MVC/ViewBase")
local PanelBase = luaclass("PanelBase", ViewBase)

local LoadState = 
{
	Init = 1,
	Loading = 2,
	Loaded = 3,
	Unloaded = 4,
}

function PanelBase:initialize()
	--self.class.super.initialize(self)
	ViewBase.initialize(self)
	self.go = nil
	self.transform = nil
	self.rectTransform = nil
	self.isActive = false
	self.state = LoadState.Init
end

function PanelBase:GetActive()
	return self.isActive
end

function PanelBase:Show()
	if self.isActive then
		print("return")
		return
	end
	self.isActive = true
	if self.state == LoadState.Loaded then
		self:OnPreShow()
		self.go:SetActive(true)
		self:OnShow()
		print("loaded")
	else
		print(self.class.name)
		ResourceManager.LoadUIAsset(self.class.name, PanelBase.OnPanelLoaded, self)
		self.state = LoadState.Loading
	end
end


function PanelBase:OnPanelLoaded(obj)
	print("loaded")
	if nil == obj then
		return
	end
	if self.state == LoadState.Unloaded then
		GameObject:Destory(obj)
		return
	end
	self.go = GameObject.Instantiate(obj)
	self.transform = self.go.transform
	self.rectTransform = self.go:GetComponent("RectTransform")
	self:InitUI()
	self.state = LoadState.Loaded
	
	if self.isActive then
		self:OnPreShow()
		self.go:SetActive(self.isActive)
		self:OnShow()
	else
		self.go:SetActive(self.isActive)
		self:OnHide()
	end
end


function PanelBase:Hide()
	if not self.isActive then
		return
	end
	self.isActive = false
	self.go:SetActive(self.isActive)
	self:OnHide()

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

return PanelBase