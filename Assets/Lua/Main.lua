--主入口函数。从这里开始lua逻辑
function Main()					
	 		
end
	
--逻辑update
function Update(deltaTime, unscaledDeltaTime)
	Time:SetDeltaTime(deltaTime, unscaledDeltaTime)				
	UpdateBeat()			
end

function LateUpdate()	
	LateUpdateBeat()	
	CoUpdateBeat()	
	Time:SetFrameCount()		
end

--物理update
function FixedUpdate(fixedDeltaTime)
	Time:SetFixedDelta(fixedDeltaTime)
	FixedUpdateBeat()
end
