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

require "system/slot"
require "system/Event"
require "system/Timer"
require "system/Coroutine"
require "u3d/Plane"
require "math/ValueType"
require "system/typeof"
--require "misc/strict"
