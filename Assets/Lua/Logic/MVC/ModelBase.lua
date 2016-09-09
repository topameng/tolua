local ModelBase = luaclass("ViewBase")

function ModelBase:initialize()
	ModelManager.Regist(self.class.name, self)
end

function ModelBase:Init()
end

return ModelBase