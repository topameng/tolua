local DisplayBase = luaclass("DisplayBase")
local ResourceManager = ResourceManager

function DisplayBase:DefineProperty()
    self.roleBase =  nil
    self.insideEventMgr = nil
    self.go = nil
    self.tr = nil
    self.pos = Vector3.zero
    self.forward = Vector3.forward
    self.eulerAngles = Vector3.zero
end

function DisplayBase:New(role, eventMgr)
    self:DefineProperty()
    self.roleBase = role
    self.insideEventMgr = eventMgr
    return self
end

function DisplayBase:Init()

end

function DisplayBase:Show()
    ResourceManager.Load(roleBase.data:Get("url"), self.OnLoaded, self)
end

function DisplayBase:OnLoaded(obj)
    if nil == obj then
        return
    end
    self.go = GameObject.Instantiate(obj)
    self.tr = self.go.transform
end

function DisplayBase:SetPos(value)
    local pos = value:Clone()
    self.pos = pos
    if nil ~= self.tr then
        self.tr.position = pos
    end
end

function DisplayBase:Regist(type, callback)
    self.insideEventMgr:RegistEvent(type, callback, self)
end

function DisplayBase:Unregist(type, callback)
    self.insideEventMgr:UnRegistEvent(type, callback, self)
end


return DisplayBase

