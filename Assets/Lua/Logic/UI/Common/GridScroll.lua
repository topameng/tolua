local GridScroll = luaclass("GridScroll", require("Logic/UI/Common/SubViewBase"))

local RectTransform = UnityEngine.RectTransform

local mathf = UnityEngine.Mathf

local UITools = UITools

function GridScroll:initialize()
    self.class.super.initialize(self)
    self.itemGo = nil
    self.gridGo = nil
    self.gridRt = nil
    self.scroll = nil
    self.moveType = Enum.Movement.Vertical
    self.sizeDelta = Vector2.New(0,0)
    self.spacing = Vector2.New(0,0)
    self.valueChangeCallback = nil
    self.itemCount = 0
    self.colNum = 0
    self.rowNum = 0
    self.transCount = 0
    self.cacheList = {}
    self.itemList = {}
    self.transIndexSet = {}
    self.transDic = {}
    self.hideTransList = {}
    self.showIndexSet = {}
end

local function IsSetEqual(set1, set2)
    for k in pairs(set1) do
        if set2[k] == nil or not set2[k] then
            return false
        end
    end
    for k in pairs(set2) do
        if set1[k] == nil or not set1[k] then
            return false
        end
    end
    return true
end

local function GetExcept(target, except)
    local tmp = {}
    for k in pairs(target) do
        if not except[k] then
            tmp[k] = true
        end
    end
    return tmp
end

function GridScroll:GetItem()
    local tr = nil
    if #self.cacheList > 0 then
        tr = self.cacheList[1]
        table.remove(self.cacheList, 1)
    else
        local go = UITools.AddChild(self.gridGo, self.itemGo)
        tr = go.transform
    end
    return tr
end




return GridScroll