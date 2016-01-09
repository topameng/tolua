local types = {}
local _typeof = tolua.typeof


function typeof(obj)
	local t = type(obj)
	local ret = nil
	
	if t == "table" then
		ret = types[obj]
		
		if ret == nil then
			ret = _typeof(obj)
			types[obj] = ret
		end		
	end
	
	return ret
end

function tolua.class(base, child)
	child = child or {}
	local meta = getmetatable(base)
	local get = tolua.initget(child)
	local set = tolua.initset(child)
	
	child.__index = function(t, k)
		local var = rawget(child, k)
	
		if var ~= nil then							
			return var
		end
	
		var = rawget(get, k)
		
		if var ~= nil then
			return var(t)	
		end	
				
		return t.vptr[k]
	end
	
	child.__newindex = function(t, k, v)
		local var = rawget(child, k)
	
		if var ~= nil then						
			return var
		end
	
		var = rawget(set, k)
		
		if var ~= nil then
			return var(t, v)	
		end
			
		if not pcall(function() t.vptr[k] = v end) then
			rawset(child, k, v)
		end
	end
	
	return child, get, set
end

function tolua.cast(obj, class)
	local t = {}	
	t.vptr = obj		
	setmetatable(t, class)	
	return t
end