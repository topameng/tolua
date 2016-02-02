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