local SubViewBase = luaclass("SubViewBase")

function SubViewBase:New(go)
    self:SetGo(go)
    return self
end


function SubViewBase:initialize()
    self.go = nil
    self.transform = nil
    self.rectTransform = nil
    self.isActive = false
end

function SubViewBase:SetGo(go)
    if nil == go then
        logError("go is nil"..self.class.name)
    end
    self.go = go
    self.transform = go.transform
    self.rectTransform = go:GetComponent("RectTransform")
    self.isActive = go.activeSelf
    self:InitUI()
end


function SubViewBase:SetActive(value)
    if value == self.isActive then
        return
    end
    self.isActive = value
    if self.isActive then
        self:OnShow()
    else
        self:OnHide()
    end
end

function SubViewBase:GetActive()
    return self.isActive
end

function SubViewBase:InitUI()
end

function SubViewBase:OnShow()
end

function SubViewBase:OnHide()
end

function SubViewBase:FindChild(path)
    local tr = self.transform:FindChild(path)
    if nil == tr then
        logError(path.." not found")
    end
    return tr
end

function SubViewBase:GetGameObject(path)
    local tr = self:FindChild(path)
    if tr == nil then
        logError(path.." not found")
        return nil
    end
    return tr.gameObject
end

function SubViewBase:GetComponent(path)
    local go = self:GetGameObject(path)
    if nil == go then
        return nil
    end
    local com = go:GetComponent(path)
    return com
end

function SubViewBase:AddComponent(path, type)
    local go = self:GetGameObject(path)
    if nil == go then
        return nil
    end
    return UITools.AddMissingComponent(go, type)
end


return SubViewBase