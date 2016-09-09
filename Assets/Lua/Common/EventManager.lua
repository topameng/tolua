EventManager = {}

local _map = {}
local _postMap = {}

function EventManager.Regist(eventType, func, self)
	self = self or ""
	if nil == _map[eventType] then
		_map[eventType] = {}
	end
	if nil == _map[eventType][func] then
		_map[eventType][func] = {}
		setmetatable(_map[eventType][func], {__mode = {'k'}})
	end
	_map[eventType][func][self] = true
end

function EventManager.UnRegist(eventType, func, self)
	self = self or ""
	if nil ~= _map[eventType] and nil ~= _map[eventType][func] then
		_map[eventType][func][self] = nil
	end
end

function EventManager.Send(eventType, obj)
	if nil == _map[eventType] then
		return
	end
	for func, selfTab in pairs(_map[eventType]) do
		for self in pairs(selfTab) do
			if self == "" then
				func(obj)
			else
				func(obj, self)
			end
		end
	end
end

function EventManager.Post(eventType, obj)
	if nil == _postMap[eventType] then
		_postMap[eventType] = {}
		_postMap[eventType].count = 0
	end
	_postMap[eventType].count = _postMap[eventType].count + 1
	_postMap[eventType][_postMap[eventType].count] = obj
end


function EventManager.LateUpate()
	for eventType, objs in pairs(_postMap) do
		if nil ~= _map[eventType] then
			for i = 1, objs.count do
				local obj = objs[i]
				objs[i] = nil
				for func, selfTab in pairs(_map[eventType]) do
					for self in pairs(selfTab) do
						if self == "" then
							func(obj)
						else
							func(obj, self)
						end
					end
				end 
			end
		end
		objs.count = 0
	end
end
