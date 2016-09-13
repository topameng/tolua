local ViewBase = luaclass("ViewBase")

function ViewBase:initialize()
	print("init" .. self.class.name)
	ViewManager.Regist(self.class.name, self)
end

function ViewBase:Init()
end

function ViewBase:Update()
end


return ViewBase