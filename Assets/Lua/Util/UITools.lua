UITools = {}

function UITools.AddMissingComponent(go, type)
    if nil == go then
        return nil
    end
    local com = go:GetComponent(type)
    if nil == com then
        com = go:AddComponent(type)
    end
    return com
end


function UITools.AddChild(parent, child)
    local go = GameObject.Instantiate(child)
    if nil == go or nil == parent then
        return
    end
    go:SetActive(true)
    local transform = go.transform
    transform:SetParent(parent.transform, false)
    transform.localPosition = Vector3.zero
    transform.localScale = Vector3.one
    transform.localRotation = Quaternion.identity

    return go
end

--[[
    设置RectTransform的anchoredPosition
    rt  被修改的RectTransform
    changePosFunc  对pos的处理函数  参数类型为Vertor2
--]]
function UITools.SetRectTransformPos(rt, changePosFunc)
    if nil == rt then
        return
    end
    if nil == changePosFunc then
        return
    end
    local pos = rt.anchoredPosition:Clone()
    changePosFunc(pos)
    rt.anchoredPosition = pos
end

--[[
    设置RectTransform的sizeDelta
    rt  被修改的RectTransform
    changeSizeFunc  对sizeDelta的处理函数  参数类型为Vertor2
--]]
function UITools.SetRectTransformSize(rt, changeSizeFunc)
    if nil == rt then
        return
    end
    if nil == changeSizeFunc then
        return
    end
    local rect = rt.sizeDelta:Clone()
    changeSizeFunc(rect)
    rt.sizeDelta = rect
end