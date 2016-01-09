local rawget = rawget
local setmetatable = setmetatable
local type = type

local Color = 
{		
}

local get = tolua.initget(Color)

Color.__index = function(t,k)
	local var = rawget(Color, k)
		
	if var == nil then							
		var = rawget(get, k)
		
		if var ~= nil then
			return var(t)	
		end
	end
	
	return var
end

function Color.New(r, g, b, a)
	local v = {r = r or 0, g = g or 0, b = b or 0, a = a or 1}
	setmetatable(v, Color)
	return v
end

function Color:Set(r, g, b, a)
	self.r = r
	self.g = g
	self.b = b
	self.a = a or 1 
end

function Color:Get()
	return self.r, self.g, self.b, self.a
end

function Color:Equals(other)
	return self.r == other.r and self.g == other.g and self.b == other.b and self.a == other.a
end

function Color:Lerp(a, b, t)
	t = Mathf.Clamp01(t)
	return Color.New(a.r + t * (b.r - a.r), a.g + t * (b.g - a.g), a.b + t * (b.b - a.b), a.a + t * (b.a - a.a))
end

function Color.GrayScale(a)
	return 0.299 * a.r + 0.587 * a.g + 0.114 * a.b
end


Color.__tostring = function(self)
	return string.format("RGBA(%f,%f,%f,%f)", self.r, self.g, self.b, self.a)
end

Color.__add = function(a, b)
	return Color.New(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a)
end

Color.__sub = function(a, b)	
	return Color.New(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a)
end

Color.__mul = function(a, b)
	if type(b) == "number" then
		return Color.New(a.r * b, a.g * b, a.b * b, a.a * b)
	elseif getmetatable(b) == Color then
		return Color.New(a.r * b.r, a.g * b.g, a.b * b.b, a.a * b.a)
	end
end

Color.__div = function(a, d)
	return Color.New(a.r / d, a.g / d, a.b / d, a.a / d)
end

Color.__eq = function(a,b)
	return a.r == b.r and a.g == b.g and a.b == b.b and a.a == b.a
end

get.red 	= function() return Color.New(1,0,0,1) end
get.green	= function() return Color.New(0,1,0,1) end
get.blue	= function() return Color.New(0,0,1,1) end
get.white	= function() return Color.New(1,1,1,1) end
get.black	= function() return Color.New(0,0,0,1) end
get.yellow	= function() return Color.New(1, 0.9215686, 0.01568628, 1) end
get.cyan	= function() return Color.New(0,1,1,1) end
get.magenta	= function() return Color.New(1,0,1,1) end
get.gray	= function() return Color.New(0.5,0.5,0.5,1) end
get.clear	= function() return Color.New(0,0,0,0) end

get.grayscale = Color.GrayScale

setmetatable(Color, Color)
return Color



