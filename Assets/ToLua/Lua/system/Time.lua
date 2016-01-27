local scaledTime = 0
local unscaledTime = tolua.gettime()

local Time = 
{
	fixedDeltaTime 	= 0,
	deltaTime 		= 0,
	frameCount 		= 1,
	timeScale		= 1,
	timeSinceLevelLoad 	= 0,
	unscaledDeltaTime	= 0,		
}

local mt = {}
mt.__index = function(obj, name)
	if name == "time" then
		return scaledTime
	elseif name == "unscaledTime" then
		return tolua.gettime() - unscaledTime
	else
		return rawget(obj, name)		
	end
end

setmetatable(Time, mt)

function Time:SetDeltaTime(deltaTime, unscaledDeltaTime)
	scaledTime = scaledTime + deltaTime
	self.deltaTime = deltaTime	
	self.timeSinceLevelLoad = self.timeSinceLevelLoad + deltaTime
	self.unscaledDeltaTime = unscaledDeltaTime
end

function Time:SetFixedDelta(time)
	self.fixedDeltaTime = time
	self.deltaTime = time
end

function Time:SetFrameCount()
	self.frameCount = self.frameCount + 1
end

function Time:SetTimeScale(scale)
	local lastScale = self.timeScale
	self.timeScale = scale
	UnityEngine.Time.timeScale = scale
	return lastScale
end

return Time