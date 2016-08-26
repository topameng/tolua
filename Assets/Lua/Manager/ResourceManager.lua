local Resources = UnityEngine.Resources

ResourceManager = {}

function ResourceManager.LoadAsset(path, func)
	local obj = Resources.Load(path)
	local newObj = GameObject.Instantiate(obj)
	if nil ~= func then
		func(newObj)
	end
end
