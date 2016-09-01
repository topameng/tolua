function log(obj)
    local str = tostring(obj)
	local output = str ..'\n'.. debug.traceback()..'\n'
	CsUtil.Log(output)
end


function logWarning(obj)
     local str = tostring(obj)
    local output = str ..'\n'.. debug.traceback()..'\n'
	CsUtil.LogWarning(output)
end

function logError(obj)
    local str = tostring(obj)
    local output = str ..'\n'.. debug.traceback()..'\n'
	CsUtil.LogError(output)
end