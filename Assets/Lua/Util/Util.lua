Util = {}

function Util.CallFunc(func, self, ...)
    if nil == func then
        return
    end
    if self == nil then
        func(...)
    else
        func(self, ...)
    end
end