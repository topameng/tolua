local RoleBaseData = luaclass("RoleBaseData")

function RoleBaseData:New()
	self:DefineProperty()
	return self
end

function RoleBaseData:DefineProperty()
	self.id = 0
	self.url = ""
	self.name = ""
end

function RoleBaseData:Get(property)
	return self[property]
end

return RoleBaseData
