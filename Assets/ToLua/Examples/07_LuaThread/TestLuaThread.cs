using UnityEngine;
using System.Collections;
using LuaInterface;

public class TestLuaThread : MonoBehaviour 
{
    string script =
        @"
            function fib(n)
                local a, b = 0, 1
                while n > 0 do
                    a, b = b, a + b
                    n = n - 1
                end

                return a
            end

            function CoFunc(len)
                print('Coroutine started')                
                local i = 0
                for i = 0, len, 1 do                    
                    local flag = coroutine.yield(fib(i))					
                    if not flag then
                        break
                    end                                      
                end
                print('Coroutine ended')
            end

            function Test()                
                local co = coroutine.create(CoFunc)                                
                return co
            end            
        ";

    LuaState state = null;
    LuaThread thread = null;

	void Start () 
    {
        state = new LuaState();
        state.Start();
        state.DoString(script);

        LuaFunction func = state.GetFunction("Test");
        func.BeginPCall();
        func.PCall();
        thread = func.CheckLuaThread();
        func.EndPCall();
        func.Dispose();
        func = null;

        thread.Resume(10);
	}

    void OnDestroy()
    {
        if (thread != null)
        {
            thread.Dispose();
            thread = null;
        }

        state.Dispose();
        state = null;
    }

    void Update()
    {
        state.CheckTop();
        state.Collect();
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 120, 40), "Resume Thead"))
        {
            if (thread != null && thread.Resume(true) == (int)LuaThreadStatus.LUA_YIELD)
            {
                object[] objs = thread.GetResult();
                Debugger.Log("lua yield: " + objs[0]);
            }
        }
        else if (GUI.Button(new Rect(10, 60, 120, 40), "Close Thread"))
        {
            if (thread != null)
            {                
                thread.Dispose();
                thread = null;
            }
        }
    }
}
