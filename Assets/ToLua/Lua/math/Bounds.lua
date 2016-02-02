--------------------------------------------------------------------------------
--      Copyright (c) 2015 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

local rawget = rawget
local setmetatable = setmetatable
local type = type
local zero = Vector3.zero

local Bounds = 
{
	center = zero,
	extents = zero,
}

local get = tolua.initget(Bounds)

Bounds.__index = function(t,k)
	local var = rawget(Bounds, k)
	
	if var == nil then							
		var = rawget(get, k)
		
		if var ~= nil then
			return var(t)	
		end
	end
	
	return var
end


function Bounds.New(center, size)
	local bd = {center = zero, extents = zero}
	bd.center = center
	bd.extents = size * 0.5
	setmetatable(bd, Bounds)	
	return bd
end

function Bounds:Get()
	local size = self:GetSize()	
	return self.center, size
end

function Bounds:GetSize()
	return self.extents * 2
end

function Bounds:SetSize(value)
	self.extents = value * 0.5
end

function Bounds:GetMin()
	return self.center - self.extents
end

function Bounds:SetMin(value)
	self:SetMinMax(value, self:GetMax())
end

function Bounds:GetMax()
	return self.center + self.extents
end

function Bounds:SetMax(value)
	self:SetMinMax(self:GetMin(), value)
end

function Bounds:SetMinMax(min, max)
	self.extents = (max - min) * 0.5
	self.center = min + self.extents
end

function Bounds:Encapsulate(point)
	self:SetMinMax(Vector3.Min(self:GetMin(), point), Vector3.Max(self:GetMax(), point))
end

function Bounds:Expand(amount)
	local t = type(amount)
	
	if t == "number" then
		amount = amount * 0.5
		self.extents:Add(Vector3.New(amount, amount, amount))
	else
		self.extents:Add(amount * 0.5)
	end
end

function Bounds:Intersects(bounds)
	local min = self:GetMin()
	local max = self:GetMax()
	
	local min2 = bounds:GetMin()
	local max2 = bounds:GetMax()
	
	return min.x <= max2.x and max.x >= min2.x and min.y <= max2.y and max.y >= min2.y and min.z <= max2.z and max.z >= min2.z
end    

function Bounds:Contains(p)
	local min = self:GetMin()
	local max = self:GetMax()
	
	if p.x < min.x or p.y < min.y or p.z < min.z or p.x > max.x or p.y > max.y or p.z > max.z then
		return false
	end
	
	return true
end

--todo
function Bounds:IntersectRay(ray)
	error("this function not defined")
	--return distance
end


function Bounds:Destroy()
	self.center 		= nil
	self.size	= nil
end

Bounds.__tostring = function(self)
	return string.format("Center: %s, Extents %s", tostring(self.center), tostring(self.extents))
end

Bounds.__eq = function(a, b)
	return a.center == b.center and a.extents == b.extents
end

get.size = Bounds.GetSize
get.min = Bounds.GetMin
get.max = Bounds.GetMax

setmetatable(Bounds, Bounds)
return Bounds
