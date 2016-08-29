UIManager = {}

function UIManager.Init()
end


function UIManager.Open(name)
	local panel = ViewManager.Get(name)
	if nil == panel then
		return 
	end

	panel:Show()
end