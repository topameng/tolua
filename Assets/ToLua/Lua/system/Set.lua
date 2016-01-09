local Set = 
{
  mt = {}
}

Set.mt.__index = Set

function Set.Create(table)
  local set = {}
  set._table = {}
  setmetatable(set, Set.mt)

  for _, l in ipairs(table) do
    set[l] = true
    table.insert(set._table, l)
  end

  return set
end

function Set.Insert(set, element)
  if not set[element] then
    set[element] = true
    table.insert(set._table, element)
  end
end

function Set.Remove(set, element)
  set[element] = nil

  if not set._lock then
    local index

    for i, e in ipairs(set._table) do
      if e == element then
        index = i
        break
      end
    end

    if not index then
      return
    end

    table.remove(set._table, index)
  else
    set._rm = set._rm or {}
    set._rm[element] = true
  end
end

function Set.RemoveIf(set, if_clause)
  for i = #set._table, 1, -1 do
    local element = set._table[i]
    if if_clause(element) then
      set[element] = nil
      table.remove(set._table, i)
    end
  end
end

function Set.Union(a, b)
  local res = Set.create({})
  if a then
    local length = #a._table
    for i = 1, length do
      local k = a._table[i]
      Set.insert(res, k)
    end
  end
  if b then
    local length = #b._table
    for i = 1, length do
      local k = b._table[i]
      Set.insert(res, k)
    end
  end
  return res
end

function Set.Intersection(a, b)
  local res = Set.create({})
  if a and b then
    local length = #a._table
    for i = 1, length do
      local k = a._table[i]
      if b[k] then
        Set.insert(res, k)
      end
    end
  end
  return res
end

function Set.ForEach(set, func)
  local _lock = set._lock
  set._lock = true
  local t = set._table
  local length = #t

  for i = 1, length do
    local k = t[i]
    if func(k) then
      return
    end
  end

  set._lock = _lock

  if set._rm and not _lock then
    set:removeIf(function(element) return set._rm[element] end)
    set._rm = nil
  end
end

return Set
