--------------------------------------------------------------------------------
--      Copyright (c) 2015 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------

local getmetatable = getmetatable
local Vector3 = Vector3
local Vector2 = Vector2
local Vector4 = Vector4
local Quaternion = Quaternion
local Color = Color
local Ray = Ray
local Bounds = Bounds
local Touch = Touch
local LayerMask = LayerMask
local RaycastHit = RaycastHit

local ValueType = 
{
	None = 0,
	Vector3 = 1,
	Quaternion = 2,
	Vector2 = 3,
	Color = 4,
	Vector4 = 5,
	Ray = 6,
	Bounds = 7,
	Touch = 8,
	LayerMask = 9,
	RaycastHit = 10,
}

function GetLuaValueType(udata)	
	local meta = getmetatable(udata)		
	
	if meta == nil then
		return ValueType.None
	elseif meta == Vector3 then		
		return ValueType.Vector3
	elseif meta == Quaternion then
		return ValueType.Quaternion
	elseif meta == Vector4 then
		return ValueType.Vector4
	elseif meta == Vector2 then
		return ValueType.Vector2
	elseif meta == Color then
		return ValueType.Color
	elseif meta == Ray then
		return ValueType.Ray
	elseif meta == Bounds then
		return ValueType.Bounds
	elseif meta == Touch then
		return ValueType.Touch
	elseif meta == LayerMask then
		return ValueType.LayerMask
	elseif meta == RaycastHit then
		return ValueType.RaycastHit
	else
		return ValueType.None
	end
end
