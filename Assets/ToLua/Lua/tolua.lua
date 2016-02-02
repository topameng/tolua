--------------------------------------------------------------------------------
--      Copyright (c) 2015 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
if jit then		
	if jit.opt then
		jit.opt.start(3)	
		print("jit opt to 3")
	end
	print("jit", jit.status())
	print(string.format("os: %s, arch: %s", jit.os, jit.arch))
end

Mathf		= require "math/Mathf"
Vector3 	= require "math/Vector3"
Quaternion	= require "math/Quaternion"
Vector2		= require "math/Vector2"
Vector4		= require "math/Vector4"
Color		= require "math/Color"
Ray			= require "math/Ray"
Bounds		= require "math/Bounds"
RaycastHit	= require "math/RaycastHit"
Touch		= require "math/Touch"
list		= require "system/list"
Time		= require "system/Time"
LayerMask	= require "u3d/LayerMask"
utf8		= require "misc/utf8"

require "system/slot"
require "system/typeof"
require "system/event"
require "system/Timer"
require "system/coroutine"
require "u3d/Plane"
require "math/ValueType"

--require "misc/strict"