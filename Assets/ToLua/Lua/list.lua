--------------------------------------------------------------------------------
--      Copyright (c) 2015 - 2016 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
local setmetatable = setmetatable
local assert = assert

local list = {}
list.__index = list

function list:new() 
  return setmetatable({length = 0}, self)
end

setmetatable(list, {__call = list.new})

function list:clear()
	self.length = 0
	self.first = nil
	self.last = nil
end

function list:push(v)
	local t = {value = v}
	
	if self.last then
		self.last._next = t
		t._prev = self.last
		self.last = t		
	else			
		self.first = t
		self.last = t		
	end
	
	self.length = self.length + 1
end

function list:pop()
	if not self.last then return end	
	local t = self.last	
	
	if t._prev then
		t._prev._next = nil
		self.last = t._prev
		t._prev = nil
	else
		self.first = nil
		self.last = nil
	end
	
	self.length = self.length - 1
	return t.value
end

function list:unshift(v)
	local t = {value = v}

	if self.first then
		self.first._prev = t
		t._next = self.first
		self.first = t
	else
		self.first = t
		self.last = t
	end
	
	self.length = self.length + 1
end

function list:shift()
	if not self.first then return end
	local t = self.first

	if t._next then
		t._next._prev = nil
		self.first = t._next
		t._next = nil
	else
		self.first = nil
		self.last = nil
	end

	self.length = self.length - 1
	return t.value
end

function list:remove(iter)
	if iter._next then
		if iter._prev then
			iter._next._prev = iter._prev
			iter._prev._next = iter._next
		else
			assert(iter == self.first)
			iter._next._prev = nil
			self.first = iter._next
		end
	elseif iter._prev then
		assert(iter == self.last)
		iter._prev._next = nil
		self.last = iter._prev
	else
		assert(iter == self.first and iter == self.last)
		self.first = nil
		self.last = nil
	end
		
	self.length = self.length - 1
	return iter
end

function list:find(v, iter)
	if iter == nil then
		iter = self.first
	end
	
	while iter do
		if v == iter.value then
			return iter
		end
		
		iter = iter._next
	end
	
	return nil
end

function list:findlast(v, iter)
	if iter == nil then
		iter = self.last
	end
	
	while iter do
		if v == iter.value then
			return iter
		end
		
		iter = iter._prev
	end
	
	return nil
end

function list:next(iter)
	if iter then		
		if iter._next ~= nil then
			return iter._next, iter._next.value
		end
	elseif self.first then
		return self.first, self.first.value
	end
	
	return nil
end

function list:items()		
	return self.next, self
end

function list:prev(iter)
	if iter then		
		if iter._prev ~= nil then
			return iter._prev, iter._prev.value
		end
	elseif self.last then
		return self.last, self.last.value
	end
	
	return nil
end

function list:reverse_items()
	return self.prev, self
end

function list:erase(value)
	local iter = self:find(value)

	if iter then
		self:remove(iter)
	end
end

function list:insert(v, iter)
	assert(v)
	if not iter then
		return self:push(value)
	end
	
	local t = {value = v}
	
	if iter._next then
		iter._next._prev = t
		t._next = iter._next
	else
		self.last = t
	end
	
	t._prev = iter
	iter._next = t
	self.length = self.length + 1
end

function list:head()
  if self.first ~= nil then
    return self.first.value
  end
  return nil
end

function list:tail()
  if self.last ~= nil then
    return self.last.value
  end
  return nil
end

function list:clone()
	local t = list:new()
	
	for item in self:items() do
		t:push(item.value)
	end
	
	return t
end

ilist	= list.items
rilist	= list.reverse_items
return list