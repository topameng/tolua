local ToggleView = luaclass("ToggleView", require("Logic/UI/Common/SubViewBase"))

local UIEventListener = UIEventListener
local DefaultTextPath = "Text"
local DefaultHighlightPath = "highlight"
local ToggleType = Enum.ToggleType

function ToggleView:initialize()
    self.class.super.initialize(self)
    self.text = nil
    self.highlight = nil
    self.isOn = false
    self.callback = nil
    self.value = ""
    self.textPath = DefaultTextPath 
    self.highlightPath = DefaultHighlightPath
    self.selectType = ToggleType.autoSelect
    self.target = nil
    self.listener = nil
end

function ToggleView:InitUI()
    self.class.super.InitUI(self)
    self.text = self:GetComponent(self.textPath, "Text")
    self.highlight = self:GetGameObject(self.highlightPath)
    self.isOn = self.highlight.activeSelf
    self:SetValue(self.value)
    self.listener = self:AddComponent("", typeof(UIEventListener))
    self.listener.onClick = UIEventListener.VoidDelegate(self.OnClick, self)
end

function ToggleView:SetValue(value)
    self.value = value
    if nil == self.text then
        return 
    end
    self.text.text = value
end

function ToggleView:SetOn(isOn)
    if isOn == self.isOn then
        return 
    end
    self.isOn = isOn
    if nil ~= self.highlight then
        self.highlight:SetActive(self.isOn)
    end
end

function ToggleView:GetOn()
    return self.isOn
end

function ToggleView:New(go, value, callback, target, selectType ,highlightPath, textPath)
    self.go = go
    self.value = value
    self.callback = callback
    self.target = target
    self.selectType = selectType or self.selectType
    self.highlightPath = highlightPath or self.highlightPath
    self.textPath = textPath or self.textPath
    self.class.super.New(self, go)
    return self
end


function ToggleView:OnClick(go)
    if self.selectType == ToggleType.autoSelect then
        self:SetOn(true)
    elseif self.selectType == ToggleType.reverse then
        self:SetOn(not self.isOn)
    end
    if nil ~= self.callback then
        if nil == self.target then
            self.callback(self)
        else
            self.callback(self.target, self)
        end
    end
end

return ToggleView