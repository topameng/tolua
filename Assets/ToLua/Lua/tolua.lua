if jit then
	jit.opt.start(3)
	print("jit", jit.status())
	print(string.format("os: %s, arch: %s", jit.os, jit.arch))
end

require "math/Mathf"
require "system/List"
require "system/Event"
require "system/Time"
require "system/Timer"
require "system/Coroutine"

Vector3 	= require "math/Vector3"
Quaternion	= require "math/Quaternion"
Vector2		= require "math/Vector2"
Vector4		= require "math/Vector4"
Color		= require "math/Color"
Ray			= require "math/Ray"
Bounds		= require "math/Bounds"
RaycastHit	= require "math/RaycastHit"
Touch		= require "math/Touch"

LayerMask	= require "u3d/LayerMask"

require "u3d/Plane"
require "math/ValueType"
require "system/typeof"


