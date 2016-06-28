--------------------------------------------------------------------------------
--      Copyright (c) 2015 - 2016 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

local setmetatable = setmetatable
local xpcall = xpcall
local pcall = pcall
local assert = assert
local rawget = rawget
local error = error
local print = print
local traceback = traceback
local ilist = ilist

local _xpcall = {}
setmetatable(_xpcall, _xpcall)

_xpcall.__call = function(self, ...)	
	local flag 	= true	
	local msg = nil	

	if jit then
		if nil == self.obj then
			flag, msg = xpcall(self.func, traceback, ...)					
		else		
			flag, msg = xpcall(self.func, traceback, self.obj, ...)					
		end
	else
		local args = {...}
			
		if nil == self.obj then
			local func = function() self.func(unpack(args)) end
			flag, msg = xpcall(func, traceback)					
		else		
			local func = function() self.func(self.obj, unpack(args)) end
			flag, msg = xpcall(func, traceback)
		end
	end
		
	return flag, msg
end

_xpcall.__eq = function(lhs, rhs)
	return lhs.func == rhs.func and lhs.obj == rhs.obj
end

local function xfunctor(func, obj)
	local st = {func = func, obj = obj}	
	setmetatable(st, _xpcall)		
	return st
end

local _pcall = {}

_pcall.__call = function(self, ...)
	local flag 	= true	
	local msg = nil	

	if nil == self.obj then
		flag, msg = pcall(self.func, ...)					
	else		
		flag, msg = pcall(self.func, self.obj, ...)					
	end
		
	return flag, msg
end

_pcall.__eq = function(lhs, rhs)
	return lhs.func == rhs.func and lhs.obj == rhs.obj
end

local function functor(func, obj)
	local st = {func = func, obj = obj}		
	setmetatable(st, _pcall)		
	return st
end

local _event = 
{	
	name	 = "",
	lock	 = false,
	keepSafe = false,
}

_event.__index = function(t, k)	
	return rawget(_event, k)
end

function _event:Add(func, obj)
	assert(func)
				
	if self.keepSafe then			
		self.list:push(xfunctor(func, obj))						
	else
		self.list:push(functor(func, obj))
	end		
end

function _event:Remove(func, obj)
	assert(func)

	for i, v in ilist(self.list) do							
		if v.func == func and v.obj == obj then
			if self.lock then
				self.rmList:push({func = func, obj = obj})		
			else
				self.list:remove(i)
			end
		end
	end		
end

function _event:Count()
	return self.list.length
end	

function _event:Clear()
	self.list:clear()
	self.rmList:clear()
	self.lock = false
	self.keepSafe = false
end

function _event:Dump()
	local count = 0
	
	for _, v in ilist(self.list) do
		if v.obj then
			print("update function:", v.func, "object name:", v.obj.name)
		else
			print("update function: ", v.func)
		end
		
		count = count + 1
	end
	
	print("all function is:", count)
end

_event.__call = function(self, ...)		
	local safe = self.keepSafe
	local _list = self.list
	local _rmList = self.rmList
	self.lock = true	

	for i, f in ilist(_list) do								
		local flag, msg = f(...)
		
		if not flag then
			if safe then								
				_list:remove(i)
			end
			self.lock = false		
			error(msg)				
		end
	end

	for _, v in ilist(_rmList) do					
		for i, item in ilist(_list) do							
			if v.func == item.func and v.obj == item.obj then
				_list:remove(i)
				break
			end 
		end
	end

	_rmList:clear()

	self.lock = false			
end

setmetatable(_event, _event)

function event(name, safe)
	local ev 	= {name = name}	
	ev.keepSafe	= safe or false
	ev.rmList	= list:new()	
	ev.list		= list:new()
	setmetatable(ev, _event)			
	return ev
end

UpdateBeat 		= event("Update", true)
LateUpdateBeat	= event("LateUpdate", true)
FixedUpdateBeat	= event("FixedUpdate", true)
CoUpdateBeat	= event("CoUpdate")				--只在协同使用

local Time = Time
local UpdateBeat = UpdateBeat
local LateUpdateBeat = LateUpdateBeat
local FixedUpdateBeat = FixedUpdateBeat
local CoUpdateBeat = CoUpdateBeat

--逻辑update
function Update(deltaTime, unscaledDeltaTime)
	Time:SetDeltaTime(deltaTime, unscaledDeltaTime)				
	UpdateBeat()	
end

function LateUpdate()	
	LateUpdateBeat()	
	CoUpdateBeat()	
	Time:SetFrameCount()		
end

--物理update
function FixedUpdate(fixedDeltaTime)
	Time:SetFixedDelta(fixedDeltaTime)
	FixedUpdateBeat()
end

function PrintEvents()
	UpdateBeat:Dump()
	FixedUpdateBeat:Dump()
end