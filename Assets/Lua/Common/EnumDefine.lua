Enum = {}

local this = Enum
this.ToggleType = 
{
    autoSelect = 1,     --点击只选中
    reverse = 2,        --点击后取反
    noOperation = 3,    --不改变选中状态 由回调控制
}