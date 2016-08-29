function log(str)
	local output = str .. tostring(debug.traceback())
	Util.Log(output)
end


function logWarning(str)
	local output = str .. tostring(debug.traceback())
	Util.LogWarning(output)
end

function logError(str)
	local output = str .. tostring(debug.traceback())
	Util.LogError(output)
end