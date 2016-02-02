--------------------------------------------------------------------------------
--      Copyright (c) 2015 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
local setmetatable = setmetatable

local _slot = {}
setmetatable(_slot, _slot)	

_slot.__call = function(self, ...)			
	if nil == self.obj then
		return self.func(...)			
	else		
		return self.func(self.obj, ...)			
	end
end

_slot.__eq = function (lhs, rhs)
	return lhs.func == rhs.func and lhs.obj == rhs.obj
end

function slot(func, obj)
	local st = {func = func, obj = obj}
	setmetatable(st, _slot)		
	return st
end