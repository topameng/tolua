--------------------------------------------------------------------------------
--      Copyright (c) 2015 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--
--      Use, modification and distribution are subject to the "New BSD License"
--      as listed at <url: http://www.opensource.org/licenses/bsd-license.php >.
--------------------------------------------------------------------------------

local setmetatable = setmetatable
local xpcall = xpcall
local pcall = pcall
local traceback = traceback

function xfunctor(func, obj)
	local st	= {}
	st.func	= func
	st.obj	= obj
	st.class	= "functor"
	setmetatable(st, st)	
	
	st.__call	= function(self, ...)
		local flag 	= true	
		local msg = nil	

		if nil == self.obj then
			flag, msg = xpcall(self.func, traceback, ...)					
		else		
			flag, msg = xpcall(self.func, traceback, self.obj, ...)					
		end
		
		return flag, msg
	end
	
	st.__eq = function(lhs, rhs)
		return lhs.func == rhs.func and lhs.obj == rhs.obj
	end
	
	return st
end

function functor(func, obj)
	local st	= {}
	st.func	= func
	st.obj	= obj
	st.class	= "functor"
	setmetatable(st, st)	
	
	st.__call	= function(self, ...)
		local flag 	= true	
		local msg = nil	

		if nil == self.obj then
			flag, msg = pcall(self.func, ...)					
		else		
			flag, msg = pcall(self.func, self.obj, ...)					
		end
		
		return flag, msg
	end
	
	st.__eq = function(lhs, rhs)
		return lhs.func == rhs.func and lhs.obj == rhs.obj
	end
	
	return st
end


function Event(name, safe)
	local ev = {}

	ev.name 	= name	
	ev.rmList	= List:New()
	ev.lock		= false
	ev.list		= List:New()
	ev.keepSafe	= safe or false
	setmetatable(ev, ev)	
	
	ev.Add = function(self, func, obj)
		 if nil == func then
			error("Add a nil function to event ".. self.name or "")
			return
		end
		
		if self.keepSafe then			
			self.list:PushBack(xfunctor(func, obj))						
		else
			self.list:PushBack(functor(func, obj))
		end		
	end

	ev.Remove = function(self, func, obj)
		if nil == func then
			return
		end
	
		if self.lock then
			self.rmList:PushBack(functor(func, obj))
		else
			for f, iter in rilist(self.list) do
				if f.func == func and f.obj == obj then
					self.list:Erase(iter)
					return
				end 
			end
		end
	end
	
	ev.Count = function(self)
		return self.list:Size()
	end	
	
	ev.Clear = function(self)
		self.list:Clear()
		self.rmList:Clear()
		self.lock = false
	end
	
	ev.__call = function(self, ...)
		local lock = self.lock
		self.lock = true

		for i in ilist(self.rmList) do
			for f, iter in ilist(self.list) do
				if f == i or (f.func == i.func and f.obj == i.obj) then
					self.list:Erase(iter)
					break
				end 
			end
		end

		self.rmList:Clear()

		for f in ilist(self.list) do					
			local flag,msg = f(...)
			if not flag then				
				self.rmList:PushBack(f)									
				self.lock = lock		
				error(msg)				
			end
		end

		self.lock = lock			
	end
	
	ev.print = function(self)
		local count = 0
		
		for f in ilist(self.list) do
			if f.obj then
				print("update function:", f.func, "object name:", f.obj.name)
			else
				print("update function: ", f.func)
			end
			count = count + 1
		end
		
		print("all function is:", count)
	end
	
	return ev
end

LuaSlot = 
{
	func = nil,
	obj = nil,
}

setmetatable(LuaSlot, LuaSlot)	

LuaSlot.__call	= function(self, ...)			
	if nil == self.obj then
		return self.func(...)			
	else		
		return self.func(self.obj, ...)			
	end
end

LuaSlot.__eq = function(lhs, rhs)
	return lhs.func == rhs.func and lhs.obj == rhs.obj
end

function slot(func, obj)
	local st = {}
	st.func	= func
	st.obj	= obj	
	setmetatable(st, LuaSlot)		
	return st
end

function CoEvent(name)
	local ev = {}

	ev.name 	= name	
	ev.rmList	= List:New()
	ev.lock		= false
	ev.list		= List:New()	
	ev.list		= List:New()	
	setmetatable(ev, ev)	
	
	ev.Add = function(self, func, obj)
		 if nil == func then
			error("Add a nil function to event ".. self.name or "")
			return
		end
				
		local s = slot(func, obj)
		self.list:PushBack(s)						
	end

	ev.Remove = function(self, func, obj)
		if nil == func then
			return
		end
	
		if self.lock then
			self.rmList:PushBack(slot(func, obj))
		else
			for st, iter in rilist(self.list) do
				if st.func == func and st.obj == obj then
					self.list:Erase(iter)
					return
				end 
			end
		end
	end
	
	ev.Count = function(self)
		return self.list:Size()
	end	
	
	ev.__call = function(self, ...)
		local lock = self.lock
		self.lock = true

		for i in ilist(self.rmList) do
			for st, iter in ilist(self.list) do
				if st == i or (st.func == i.func and st.obj == i.obj) then
					self.list:Erase(iter)
					break
				end 
			end
		end

		self.rmList:Clear()

		for st in ilist(self.list) do					
			st(...)
		end

		self.lock = lock			
	end
	
	return ev
end

UpdateBeat 		= Event("Update", true)
LateUpdateBeat	= Event("LateUpdate", true)
FixedUpdateBeat	= Event("FixedUpdate", true)
CoUpdateBeat	= Event("CoUpdate")


function PrintEvents()
	UpdateBeat:print()
	FixedUpdateBeat:print()
end