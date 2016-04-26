--------------------------------------------------------------------------------
--      Copyright (c) 2015 - 2016 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
local rawget = rawget
local setmetatable = setmetatable

local Ray = {}
local get = tolua.initget(Ray)

Ray.__index = function(t,k)
	local var = rawget(Ray, k)
		
	if var == nil then							
		var = rawget(get, k)
		
		if var ~= nil then
			return var(t)	
		end
	end
	
	return var
end

Ray.__call = function(t, direction, origin)
	return Ray.New(direction, origin)
end

function Ray.New(direction, origin)
	local ray = {}	
	ray.direction 	= direction:Normalize()
	ray.origin 		= origin
	setmetatable(ray, Ray)	
	return ray
end

function Ray:GetPoint(distance)
	local dir = self.direction * distance
	dir:Add(self.origin)
	return dir
end

function Ray:Get()		
	return self.origin, self.direction
end

Ray.__tostring = function(self)
	return string.format("Origin:(%f,%f,%f),Dir:(%f,%f, %f)", self.origin.x, self.origin.y, self.origin.z, self.direction.x, self.direction.y, self.direction.z)
end

UnityEngine.Ray = Ray
setmetatable(Ray, Ray)
return Ray