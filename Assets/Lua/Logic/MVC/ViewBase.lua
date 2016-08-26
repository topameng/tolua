local ViewBase = luaclass("ViewBase")

function ViewBase:initialize()
	ViewManager.Regist(self.class.name, self)
end

function ViewBase:Init()
end

function ViewBase:Update()
end


return ViewBase