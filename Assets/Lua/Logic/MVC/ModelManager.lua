ModelManager = {}

local _map = {}

function ModelManager.Regist(name, model)
	if nil ~= _map[name] then
		logError("重复注册")
	end
	_map[name] = model
end

function ModelManager.Get(name)
	return _map[name]
end

function ModelManager.Init()
	for k, v in pairs(_map) do
		v:Init()
	end
end