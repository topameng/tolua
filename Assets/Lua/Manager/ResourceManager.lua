local Resources = UnityEngine.Resources

ResourceManager = {}
local this = ResourceManager

function ResourceManager.LoadAsset(path, func, self)
	--coroutine.start(
	--	function ()
	--		coroutine.step(1)			--延时一帧模拟加载
			local obj = Resources.Load(path)
			if nil == obj then
				logError("can not find " + path)
			end
			if nil ~= func then
				if nil ~= self then
					func(self, obj)
				else
					func(obj)
				end
			end
	--	 end)
	
end

function ResourceManager.LoadUIAsset(name, func, self)
	this.LoadAsset("UI/"..name, func,self)
end
