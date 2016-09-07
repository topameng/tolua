local RoleControlBase = luaclass("RoleControlBase")


function RoleControlBase:DeinfeProperty()
	self.display = nil
	self.data = nil
	self.actionControl = nil
	self.moveControl = nil
end

function RoleControlBase:New()
	self:DeinfeProperty()
	return self
end


return RoleControlBase