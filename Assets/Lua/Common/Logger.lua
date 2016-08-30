function log(obj)
    local str = tostring(obj)
	local output = str ..'\n'.. debug.traceback()..'\n'
	Util.Log(output)
end


function logWarning(obj)
     local str = tostring(obj)
    local output = str ..'\n'.. debug.traceback()..'\n'
	Util.LogWarning(output)
end

function logError(obj)
    local str = tostring(obj)
    local output = str ..'\n'.. debug.traceback()..'\n'
	Util.LogError(output)
end