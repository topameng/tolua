UITools = {}

function UITools.AddMissingComponent(go, type)
    if nil == go then
        return nil
    end
    local com = go:GetComponent(type)
    if nil == com then
        com = go:AddComponet(type)
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
    transform.SetParent(parent.transform)
    transform.localPosition = Vector3.Zero
    transform.localScale = Vector3.Zero
    transform.localRotation = Quaternion.identity

    return go
end