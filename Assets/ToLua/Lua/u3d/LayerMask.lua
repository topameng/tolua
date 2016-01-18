local LayerMask =
{
	value = 0,
}

local get = tolua.initget(LayerMask)

LayerMask.__index = function(t,k)
	local var = rawget(LayerMask, k)
	
	if var == nil then							
		var = rawget(get, k)
		
		if var ~= nil then
			return var(t)	
		end
	end
	
	return var
end

LayerMask.__call = function(t,v)
	return LayerMask.New(v)
end

function LayerMask.New(value)
	local layer = {value = value or 0}
	setmetatable(layer, LayerMask)	
	return layer
end

function LayerMask:Get()
	return self.value
end

function LayerMask.NameToLayer(name)
	return Layer[name]
end

function LayerMask.GetMask(...)
	local arg = {...}
	local value = 0	

	for i = 1, #arg do		
		local n = LayerMask.NameToLayer(arg[i])
		
		if n ~= 0 then
			value = value + 2 ^ n				
		end
	end	
		
	return value
end

setmetatable(LayerMask, LayerMask)
return LayerMask



