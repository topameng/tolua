local zero = Vector2.zero

TouchPhase =
{
	Began = 0,
	Moved = 1,
	Stationary = 2,
	Ended = 3,
	Canceled = 4,
}

TouchBits = 
{
	DeltaPosition = 1,
    Position = 2,
    RawPosition = 4,
    ALL = 7,
}

UnityEngine.TouchPhase = TouchPhase

local Touch = 
{
	fingerId 		= 0,
	position	 	= zero,
	rawPosition	 	= zero,
	deltaPostion 	= zero,
	deltaTime 		= 0,
	tapCount 		= 0,
	phase 			= TouchPhase.Began,		
}

local get = tolua.initget(Touch)

Touch.__index = function(t,k)
	local var = rawget(Touch, k)
	
	if var == nil then							
		var = rawget(get, k)
		
		if var ~= nil then
			return var(t)	
		end
	end
	
	return var
end

--c# 创建
function Touch.New(fingerId, position, rawPosition, deltaPostion, deltaTime, tapCount, phase)
	local touch = {fingerId = fingerId or 0, position = position or zero, rawPosition = rawPosition or zero, deltaPostion = deltaPostion or zero, deltaTime = deltaTime or 0, tapCount = tapCount or 0, phase = phase or 0}		
	setmetatable(touch, Touch)
	return touch
end

function Touch:Init(fingerId, position, rawPosition, deltaPostion, deltaTime, tapCount, phase)
	self.fingerId = fingerId
	self.position = position
	self.rawPosition = rawPosition
	self.deltaPosition = deltaPostion
	self.deltaTime = deltaTime
	self.tapCount = tapCount
	self.phase = phase	
end

function Touch:Destroy()
	self.position 		= nil
	self.rawPosition	= nil
	self.deltaPostion 	= nil
	self.phase			= nil
end

function Touch.GetMask(...)
	local arg = {...}
	local value = 0	

	for i = 1, #arg do		
		local n = TouchBits[arg[i]] or 0
		
		if n ~= 0 then
			value = value + n				
		end
	end	
		
	if value == 0 then value = TouchBits["all"] end
		
	return value
end

setmetatable(Touch, Touch)
return Touch


