local ControlBase = luaclass("ControlBase")

function ControlBase:initialize()
	ControlManager.Regist(self.class.name, self)
end

function ControlBase:Init()
end

function ControlBase:Update()
end


return ControlBase