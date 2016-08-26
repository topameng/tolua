ViewManager = {}

local _map = {}

function ViewManager.Regist(name, view)
	if nil ~= _map[name] then
		logError("重复注册")
	end
	_map[name] = view
end

function ViewManager.Get(name)
	return _map[name]
end

function ViewManager.Update()
	for k,v in pairs(_map) do
		v:Update()
	end
end

function ViewManager.Init()
	print("init")
	for k, v in pairs(_map) do
		v:Init()
	end
end