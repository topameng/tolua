local PanelTest = luaclass("PanelTest", require("Logic/UI/Common/PanelBase"))
function PanelTest:initialize()
	self.class.super.initialize(self)
	self.toggle = nil
end

function PanelTest:InitUI()
	local scrollGo = self:GetGameObject("scroll")
	local itemGo = self:GetGameObject("scroll/grid/item")
	self.scoll =require("Logic/UI/Common/GridScroll")():New(scrollGo, itemGo, self, Enum.Movement.Horizontal)
	self.scoll:Init(function () end, 200, Vector2(100,100))

	--local toggleGo = self:GetGameObject("toggle")
	--self.toggle = require("Logic/UI/Common/ToggleView")():New(toggleGo, "toggle",self.OnValueChange, self, Enum.ToggleType.reverse)
end

function PanelTest:OnValueChange(toggle)
	print(toggle:GetOn())
end

return PanelTest