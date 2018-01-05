--[[MIT License

Copyright (c) 2018 Jonson

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.]]--


local pcall = pcall
local pairs = pairs
local error = error
local rawget = rawget
local string = string
local tolua_tag = tolua_tag
local getmetatable = getmetatable
local CSLuaInjectStation
local bridgeInfo = require "System.Injection.InjectionBridgeInfo"

local function Check(csModule)
	local existmt = getmetatable(csModule)
	if rawget(existmt, tolua_tag) ~= 1 then
		error("Can't Inject")
	end

	return existmt
end

local function CacheCSLuaInjectStation()
	if CSLuaInjectStation == nil then
		CSLuaInjectStation = LuaInterface.LuaInjectionStation
	end
end

local function UpdateFunctionReference(metatable, injectInfo)
	local oldIndexMetamethod = metatable.__index
	metatable.__index = function(t, k)
		--Ignore Overload Function
		local infoPipeline = rawget(injectInfo, k)
		if infoPipeline ~= nil then
			local injectFunction, injectFlag = infoPipeline()
			if injectFlag == LuaInterface.InjectType.Replace
				or injectFlag == LuaInterface.InjectType.ReplaceWithPostInvokeBase
				or injectFlag == LuaInterface.InjectType.ReplaceWithPreInvokeBase
			then
				return injectFunction
			end
		end

		local status, result = pcall(oldIndexMetamethod, t, k)
		if status then
			return result
		else
			error(result)
			return nil
		end
	end
end

function InjectByModule(csModule, injectInfo)
	local mt = Check(csModule)
	local moduleName = mt[".name"]

	InjectByName(moduleName, injectInfo)
	UpdateFunctionReference(mt, injectInfo)
end

--Won't Update Function Reference In Lua Env
function InjectByName(moduleName, injectInfo)
	CacheCSLuaInjectStation()
	local moduleBridgeInfo = rawget(bridgeInfo, moduleName)
	if moduleBridgeInfo == nil then
		error(string.format("Module %s Can't Inject", moduleName))
	end

	for funcName, infoPipeline in pairs(injectInfo) do
		local injectFunction, injectFlag = infoPipeline()
		local injectIndex = rawget(moduleBridgeInfo, funcName)
		if injectIndex == nil then
			error(string.format("Function %s Doesn't Exist In Module %s", funcName, moduleName))
		end

		CSLuaInjectStation.CacheInjectFunction(injectIndex, injectFlag:ToInt(), injectFunction)
	end
end

require "System.Injection.LuaInjectionBus"