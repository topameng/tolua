local GridScroll = luaclass("GridScroll", require("Logic/UI/Common/SubViewBase"))
local RectTransform = UnityEngine.RectTransform
local mathf = UnityEngine.Mathf
local UITools = UITools
local Util = Util

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
    self.target = nil
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

function GridScroll:IndexToPosition(index)
    -- 算位置 index 要从0开始
    index = index - 1
    local itemSizeX = self.spacing.x + self.sizeDelta.x
    local itemSizeY = self.spacing.y + self.sizeDelta.y
    if self.moveType == Enum.Movement.Horizontal then
        return Vector2(itemSizeX * mathf.Floor(index / self.rowNum), -itemSizeY * math.fmod(index, self.rowNum))
    else
        return Vector2(itemSizeX * math.fmod(index, self.colNum), -itemSizeY * mathf.Floor(index / self.colNum))
    end
end

function GridScroll:ChangeToIndex(index)
    if #self.hideTransList <= 0 then
        return 
    end
    local tr = self.hideTransList[1]
    table.remove(self.hideTransList, 1)
    tr.anchoredPosition = self:IndexToPosition(index)
    self.transDic[index] = tr
    Util.CallFunc(self.valueChangeCallback, self.target, tr, index)
end

function GridScroll:InitChild(go, index)
    local rt = go:GetComponent("RectTransform")
    rt.anchoreMax = Vector2(0,1)
    rt.anchoreMin = Vector2(0,1)
    rt.pivot = Vector2(0,1)
    rt.sizeDelta = self.sizeDelta
    rt.anchoredPosition = self:IndexToPosition(index)
end


function GridScroll:InitChildren()
    self.transCount = self.rowNum * self.colNum
    if self.transCount > self.itemCount then
        self.transCount = self.itemCount
    end
    for i, self.transCount do
        local item = self:GetItem()
        self:InitChild(item.gameObject, i)
        Util.CallFunc(self.valueChangeCallback, item, i)
        self.transIndexSet[i] = true
        self.transDic[i] = item
    end
end

function GridScroll:InitTransform()
    local itemSizeX = self.spacing.x + self.sizeDelta.x
    local itemSizeY = self.spacing.y + self.sizeDelta.y
    self.colNum = mathf.Round((self.rectTransform.rect.width + self.spacing.x) / itemSizeX)
    self.rowNum = mathf.Round((self.rectTransform.rect.height + self.spacing. y) / itemSizeY)
    if self.moveType == Enum.Movement.Horizontal then
        if self.gridRt.pivot.x ~= 0 then
            logError("gridscorll pivot.x ~= 0")
        end
        self.colNum = self.colNum + 2
        --self.gridRt:SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, mathf.Ceil(self.itemCount/self.rowNum)* itemSizeX)
        UITools.SetRectTransformSize(self.gridRt, 
            function (size)
                size.x = mathf.Ceil(self.itemCount/self.rowNum) * itemSizeX)
            end)
        UITools.SetRectTransformPos(self.gridRt, function(pos) pos.x = 0 end)
    else
         if self.gridRt.pivot.y ~= 0 then
            logError("gridscorll pivot.y ~= 0")
        end
        self.rowNum = self.rowNum + 2
        UITools.SetRectTransformSize(self.gridRt, 
            function (size) 
                size.x = matthf.Ceil(self.itemCount / self.colNum) * itemSizeY
            end)
        UITools.SetRectTransformPos(self.gridRt, function (pos) pos.x = 0 end)
    end
end

function GridScroll:InitScroll()
    self.scroll = self:GetComponent("", "ScrollRect")
    if nil == self.scroll then
        logError("scroll is nil")
    end
    if self.moveType == Enum.Movement.Horizontal then
        self.scroll.vertical = false
        self.scroll.horizontal = true
    else
        self.scroll.vertical = true
        self.scroll.horizontal = false
    end
end

function GridScroll:InitGrid()
    self.gridRt = self.scroll.content
    self.gridGo = self.gridRt.gameObject
end

----外部接口
function GridScroll:New(go, itemGo, target, moveType)
    self.itemGo = itemGo
    self.moveType = moveType or Enum.Movement.Vertical
    self.target = target
    self.class.super.New(go)
    return self
end


function GridScroll:Init(callback, itemCount, sizeDelta, spacing)
    self.valueChangeCallback = callback
    self.itemCount = itemCount
    self.sizeDelta = sizeDelta
    self.spacing = spacing
    self:InitScroll()
    self:InitTransform()
    self:InitChildren()
    LuaHelper.AddScrollRectHandler(self.go, System.Action_UnityEngine_GameObject_UnityEndgine_Vector2(self.OnValueChange,self))
end


function GridScroll:Clear()
    for _, tr in pairs(self.transDic) do
        tr.gameObject:SetActive(false)
        table.insert(self.cacheList, tr)
    end
    for _, tr in pairs(self.hideTransList) do
        tr.gameObject:SetActive(false)
        table.insert(self.cacheList, tr)
    end
    self.hideTransList = {}
    self.transDic = {}
    self.itemCount = 0
    self.transIndexSet = {}
    self.showIndexSet = {}
    LuaHelper.RemoveScrollRectHandler(self.go)
end    

function GridScroll:RefreshCurrent()
    for k in pairs(self.transIndexSet) do
        Util.CallFunc(self.valueChangeCallback, self.target, self.transDic[k], k)
    end
end

function GridScroll:UpdateItemCount(count, savePos, forceRefresh)
    if not forceRefresh and self.itemCount == count then
        self:RefreshCurrent()
        return
    end

    local cachePos = 0
    if savePos then
        if self.moveType == Enum.Movement.Horizontal then
            cachePos = self.gridRt.anchoredPosition.x
        else
            cachePos = self.gridRt.anchoredPosition.y
        end
        if cachePos < 0 then
            cachePos = 0
        end
    end

    self:Clear()
    self.itemCount = count
    self:InitTransform()
    self:InitChildren()
    LuaHelper.AddScrollRectHandler(self.go, System.Action_UnityEngine_GameObject_UnityEndgine_Vector2(self.OnValueChange,self))

    
    if not savePos then
        return
    end

    if self.moveType == Enum.Movement.Horizontal then
        if -cachePos > self.gridRt.rect.width - self.rectTransform.rect.width then
            cachePos = -(self.gridRt.rect.width - self.rectTransform.rect.width)
        end
        UITools.SetRectTransformPos(self.gridRt, function(pos) pos.x = cachePos end)
    else
         if -cachePos > self.gridRt.rect.height - self.rectTransform.rect.height then
            cachePos = -(self.gridRt.rect.height - self.rectTransform.rect.height)
        end
        UITools.SetRectTransformPos(self.gridRt, function(pos) pos.y = cachePos end)
    end
    
    if forceRefresh then
        self:OnValueChange()
    end
end

-------------------------------------------------


function GridScroll:SwapIndex(index)
    self.showIndexSet = {}
    for i = 1, self.transCount do
        if (i + index) <= self.itemCount and (i + index) >= 1 then
            self.showIndexSet[i + index] = true
        end
    end
    if not IsSetEqual(self.showIndexSet, self.transIndexSet) then
        local lhsIter = GetExcept(self.showIndexSet, self.transIndexSet)
        local rhsIter = GetExcept(self.transIndexSet, self.showIndexSet)
        for k in pairs(rhsIter) do
            table.insert(self.hideTransList, self.transDic)
            self.transIndexSet[k] = nil
        end

        for k in pairs(lhsIter) do
            self:ChangeToIndex(k)
        end
        self.transIndexSet, self.showIndexSet = self.showIndexSet, self.transIndexSet
    end
end

function GridScroll:OnValueChange(rectGo, normalPos)
    if self.transCount == self.itemCount then
        return
    end
    local itemSizeX = self.spacing.x + self.sizeDelta.x
    local itemSizeY = self.spacing.y + self.sizeDelta.y
    local startIndex = 0
    if self.moveType == Enum.Movement.Horizontal then
        local len = -self.gridRt.anchoredPosition.y 
        local scrollCol = mathf.Floor(len/itemSizeX)
        startIndex = scrollCol * self.rowNum
    else
         local len = -self.gridRt.anchoredPosition.x 
        local scrollCol = mathf.Floor(len/itemSizeY)
        startIndex = scrollCol * self.colNum
    end
    self:SwapIndex(startIndex)
end


return GridScroll