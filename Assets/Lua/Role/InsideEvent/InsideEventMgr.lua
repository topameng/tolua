local InsideEventMgr = luaclass("InsideEventMgr")
local this = InsideEventMgr

function this:New()
    self.eventMap = {}
    return self
end

function this:RegistEvent(type, callback, target)
    local key = target or ""
    if self.eventMap[type] == nil then
        self.eventMap[type] = {}
        setmetatable(self.eventMap[type], {__mode = "k"})
    end
    if self.eventMap[type][key] == nil then
        self.eventMap[type][key] = {}
    end

    table.insert(self.eventMap[type][key], callback)
end

function this:UnRegistEvent(type, callback, target)
    local key = target or ""
    if self.eventMap[type] == nil or
       self.eventMap[type][key] == nil then
       return
    end
    for i = #self.eventMap[type][key], 1, -1 do
        if self.eventMap[type][key][i] == callback then
            table.remove(self.eventMap[type][key], i)
        end
    end
    if #self.eventMap[type][key] == 0 then
        self.eventMap[type][key] = nil
    end
end

function this:Send(type, ...)
    local funcMap = self.eventMap[type]
    local midEventMap = {}
    for target, funcList in pairs(funcMap) do
        for i, callback in pairs(funcList) do
            midEventMap[callback] = target
        end 
    end

    for callback, target in pairs(midEventMap) do
        if target ~= nil or "" then
            callback(target, ...)
        end
    end
end

return InsideEventMgr