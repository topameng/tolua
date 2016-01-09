--------------------------------------------------------------------------------
--      Copyright (c) 2015 , 蒙占志(topameng) topameng@gmail.com
--      All rights reserved.
--
--      Use, modification and distribution are subject to the "New BSD License"
--      as listed at <url: http://www.opensource.org/licenses/bsd-license.php >.
--------------------------------------------------------------------------------

local create = coroutine.create
local running = coroutine.running
local resume = coroutine.resume
local yield = coroutine.yield
local error = error

function coroutine.start(f, ...)	
	local co = create(f)
	
	if running() == nil then
		local flag, msg = resume(co, ...)
	
		if not flag then		
			msg = debug.traceback(co, msg)					
			error(msg)				
		end					
	else
		local args = {...}
		local timer = nil
		
		local action = function()							
			local flag, msg = resume(co, unpack(args))
	
			if not flag then				
				timer:Stop()
				msg = debug.traceback(co, msg)				
				error(msg)						
			end		
		end
			
		timer = FrameTimer.New(action, 0, 1)
		timer:Start()		
	end
end

function coroutine.wait(t, co, ...)
	local args = {...}
	co = co or running()		
	local timer = nil
		
	local action = function()		
		local flag, msg = resume(co, unpack(args))
		
		if not flag then	
			timer:Stop()			
			msg = debug.traceback(co, msg)							
			error(msg)			
			return
		end
	end
	
	timer = CoTimer.New(action, t, 1)	
	timer:Start()
	return yield()
end

function coroutine.step(t, co, ...)
	local args = {...}
	co = co or running()		
	local timer = nil
	
	local action = function()							
		local flag, msg = resume(co, unpack(args))
	
		if not flag then							
			timer:Stop()					
			msg = debug.traceback(co, msg)					
			error(msg)
			return	
		end		
	end
				
	timer = FrameTimer.New(action, t or 1, 1)
	timer:Start()
	return yield()
end

function coroutine.www(www, co)			
	co = co or running()			
	local timer = nil			
			
	local action = function()				
		if not www.isDone then		
			return		
		end		
		
		timer:Stop()		
		local flag, msg = resume(co)		
			
		if not flag then						
			msg = debug.traceback(co, msg)						
			error(msg)			
			return			
		end				
	end		
					
	timer = FrameTimer.New(action, 1, -1)		
 	timer:Start()
 	return yield()
 end
