using UnityEngine;
using System.Collections;
using LuaInterface;

public class TestCoroutine2 : MonoBehaviour 
{
    LuaState luaState = null;    

    string script =
    @"
        function CoExample()            
            WaitForSeconds(2)
            print('WaitForSeconds end time: '.. UnityEngine.Time.time)
            WaitForFixedUpdate()
            print('WaitForFixedUpdate end frameCount: '..UnityEngine.Time.frameCount)
            WaitForEndOfFrame()
            print('WaitForEndOfFrame end frameCount: '..UnityEngine.Time.frameCount)
            Yield(null)
            print('yield null end frameCount: '..UnityEngine.Time.frameCount)
            Yield(0)
            print('yield(0) end frameCime: '..UnityEngine.Time.frameCount)
            local www = UnityEngine.WWW('http://www.baidu.com')
            Yield(www)
            print('yield(www) end time: '.. UnityEngine.Time.time)
            local s = tolua.tolstring(www.bytes)
            print(s:sub(1, 128))
            print('coroutine over')
        end

        function TestCo()
            print('TestCo')
            local co = coroutine.create(CoExample)
                        
            local flag, msg = coroutine.resume(co)
            
            if not flag then
                error(msg)
            end
        end
    ";

	void Awake () 
    {
        luaState = new LuaState();
        luaState.Start();
        LuaBinder.Bind(luaState);
        LuaCoroutine.Register(luaState, this);

        luaState.DoString(script);
        LuaFunction func = luaState.GetFunction("TestCo");
        func.Call();
        func.Dispose();
	}

    void OnDestroy()
    {
        luaState.Dispose();
        luaState = null;
    }
}
