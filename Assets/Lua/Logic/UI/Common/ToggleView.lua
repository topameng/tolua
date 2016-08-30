local ToggleView = luaclass("ToggleView", require("UI/Common/SubViewBase"))

local UIEventListener = UIEventListener
local DefaultTextPath = "Text"
local DefaultHighlightPath = "highlight"
local ToggleType = Enum.ToggleType

function ToggleView:initialize()
    self.text = nil
    self.highlight = nil
    self.isOn = false
    self.callback = nil
    self.value = ""
    self.textPath = DefaultTextPath
    self.highlightPath = DefaultHighlightPath
    self.selectType = ToggleType.autoSelect
    self.listener = nil
end

function ToggleView:InitUI()
    self.class.super.InitUI(self)
    self.text = self:GetComponent(self.textPath, "Text")
    self.highlight = self:GetGameObject(self.highlightPath)
    self.isOn = self.highlight.activeSelf
    self:SetValue(self.value)
    self.listener = self:AddComponet("", typeof(UIEventListener))
    self.listener.OnClick = System.Action_UnityEngine_GameObject(self.OnClick, self)
end

function ToggleView:SetValue(value)
      self.value = value
    if nil == self.text then
        reutrn 
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

function ToggleView:New(go, value, callback, highlightPath, textPath)
    self.go = go
    self.value = value
    self.callback = callback
    self.highlightPath = highlightPath
    self.textPath = textPath
    self.class.super.New(self, go)
    return self
end


function ToggleView:OnClick(go)
    if self.selectType == ToggleType.autoSelect then
        self:SetOn(true)
    else if self.selectType == ToggleType.reverse then
        self:SetOn(not self.isOn)
    end
    if nil ~= self.callback then
        self.callback(self)
    end
end
return ToggleView