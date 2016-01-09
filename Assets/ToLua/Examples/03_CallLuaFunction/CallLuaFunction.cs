using UnityEngine;
using System.Collections;
using LuaInterface;
using System;

public class CallLuaFunction : MonoBehaviour 
{
    private string script = 
        @"  function luaFunc(num)                        
                return num + 1
            end

            test = {}
            test.luaFunc = luaFunc
        ";

    LuaFunction func = null;
    LuaState lua = null;
	
	void Start () 
    {
        lua = new LuaState();
        lua.Start();
        lua.DoString(script);

        //Get the function object
        func = lua.GetFunction("test.luaFunc");

        if (func != null)
        {
            //有gc alloc
            object[] r = func.Call(123456);
            Debugger.Log(r[0]);

            // no gc alloc
            int num = CallFunc();
            Debugger.Log(num);
        }

        lua.CheckTop();
	}

    void OnDestroy()
    {
        if (func != null)
        {
            func.Dispose();
            func = null;
        }

        lua.Dispose();
        lua = null;
    }

    int CallFunc()
    {        
        func.BeginPCall();                
        func.Push(123456);
        func.PCall();        
        int num = (int)func.CheckNumber();                    
        func.EndPCall();
        return (int)num;        
    }
		
    //取消注释, 在profiler中查看gc alloc
	//void Update () 
    //{
        //func.Call(123456);
        //CallFunc();        
	//}
}
