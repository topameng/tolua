local require = require
local string = string
local table = table

int64.zero = int64.new(0,0)
uint64.zero = uint64.new(0,0)

function string.split(input, delimiter)
    input = tostring(input)
    delimiter = tostring(delimiter)
    if (delimiter=='') then return false end
    local pos,arr = 0, {}
    -- for each divider found
    for st,sp in function() return string.find(input, delimiter, pos, true) end do
        table.insert(arr, string.sub(input, pos, st - 1))
        pos = sp + 1
    end
    table.insert(arr, string.sub(input, pos))
    return arr
end

function import(moduleName, currentModuleName)
    local currentModuleNameParts
    local moduleFullName = moduleName
    local offset = 1

    while true do
        if string.byte(moduleName, offset) ~= 46 then -- .
            moduleFullName = string.sub(moduleName, offset)
            if currentModuleNameParts and #currentModuleNameParts > 0 then
                moduleFullName = table.concat(currentModuleNameParts, ".") .. "." .. moduleFullName
            end
            break
        end
        offset = offset + 1

        if not currentModuleNameParts then
            if not currentModuleName then
                local n,v = debug.getlocal(3, 1)
                currentModuleName = v
            end

            currentModuleNameParts = string.split(currentModuleName, ".")
        end
        table.remove(currentModuleNameParts, #currentModuleNameParts)
    end

    return require(moduleFullName)
end

--重新require一个lua文件，替代系统文件,用于Time。
function reimport(name)
    local package = package
    package.loaded[name] = nil
    package.preload[name] = nil
    return require(name)    
end

if _VERSION == "Lua 5.3" then
    table.getn = function(t)        
        return #t
    end

    --5.3 用math.atan2功能替换掉了函数math.atan
    math.atan2 = math.atan
end

if math.fmod == nil then
    math.fmod = function(x, y)
        return x % y
    end
end

local function _vardump(value, set, depth, key)
  local linePrefix = ""
  local spaces = ""
  
  if key ~= nil then
    linePrefix = "["..tostring(key).."] = "
  end
  
  if depth == nil then
    depth = 0
  else
    depth = depth + 1
    for i=1, depth do spaces = spaces .. "  " end
  end
  
  if type(value) == 'table' then
    if set[value] ~= nil then
        return
    end

    set[value] = 1
    mTable = getmetatable(value)
    if mTable == nil then
      print(spaces ..linePrefix.."(table) ")
    else
      print(spaces .."(metatable) ")
        value = mTable
    end     
    for k, v in pairs(value) do            
      _vardump(v, set, depth, k)
    end
  elseif type(value) == 'function' or type(value) == 'thread' or type(value) == 'userdata' or value == nil then
    print(spaces..linePrefix..tostring(value))
  else
    print(spaces..linePrefix.."("..type(value)..") "..tostring(value))
  end
end

function vardump(value, depth, key)
    local _set = {}
    _vardump(value, _set, depth, key)
    _set = nil
end

function fullgc()
  local c = collectgarbage("count")
  print("begin gc count = "..c.." kb")
  collectgarbage("collect")
  c = collectgarbage("count")
  print("end gc count = "..c.." kb")
end

function stepgc()
    collectgarbage("step")    
    if jit then jit.flush() end
end