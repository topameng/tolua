--------------------------------------------------------------------------------
--      Copyright (c) 2015 - 2016 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--      Use, modification and distribution are subject to the "MIT License"
--------------------------------------------------------------------------------
if jit then		
	if jit.opt then		
		jit.opt.start(3)				
	end		
	
	print("ver"..jit.version_num.." jit: ", jit.status())
	print(string.format("os: %s, arch: %s", jit.os, jit.arch))
end

if DebugServerIp then  
  require("mobdebug").start(DebugServerIp)
end

require "misc.functions"
Mathf		= require "UnityEngine.Mathf"
Vector3 	= require "UnityEngine.Vector3"
Quaternion	= require "UnityEngine.Quaternion"
Vector2		= require "UnityEngine.Vector2"
Vector4		= require "UnityEngine.Vector4"
Color		= require "UnityEngine.Color"
Ray			= require "UnityEngine.Ray"
Bounds		= require "UnityEngine.Bounds"
RaycastHit	= require "UnityEngine.RaycastHit"
Touch		= require "UnityEngine.Touch"
LayerMask	= require "UnityEngine.LayerMask"
Plane		= require "UnityEngine.Plane"
Time		= reimport "UnityEngine.Time"

list		= require "list"
utf8		= require "misc.utf8"

require "event"
require "typeof"
require "slot"
require "System.Timer"
require "System.coroutine"
require "System.ValueType"
require "System.Reflection.BindingFlags"

--require "misc.strict"

if jit then
	if jit.status() then		
		local t = os.clock()

		for i=1,10000 do
			local q1 = Quaternion.Euler(i, i, i)				
			Quaternion.Slerp(Quaternion.identity, q1, 0.5)		
		end	
		
		jit.flush(true)
	end	

	if not jit.status() then
		print("jit off")	
	end
end