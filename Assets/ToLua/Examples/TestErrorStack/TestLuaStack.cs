using UnityEngine;
using System.Collections;
using LuaInterface;
using System;

//检测合理报错
public class TestLuaStack : MonoBehaviour
{
    public GameObject go = null;
    public static LuaFunction show = null;
    public static LuaFunction testRay = null;

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Test1(IntPtr L)
    {
        try
        {
            show.BeginPCall();
            show.PCall();
            show.EndPCall();
        }
        catch (Exception e)
        {
            LuaDLL.luaL_error(L, e.Message);
        }

        return 1;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Test2(IntPtr L)
    {
        try
        {
            testRay.BeginPCall();
            testRay.Push(new Ray());
            testRay.PCall();
            testRay.EndPCall();
        }
        catch (Exception e)
        {
            LuaDLL.luaL_error(L, e.Message);
        }

        return 1;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Test3(IntPtr L)
    {
        try
        {
            testRay.BeginPCall();
            testRay.PCall();
            testRay.CheckRay();
            testRay.EndPCall();
        }
        catch (Exception e)
        {
            LuaDLL.luaL_error(L, e.Message);
        }

        return 1;
    }

    LuaState state = null;

    string script =
        @"
            function Show()
                error('this is error')
            end

            function ShowStack()
                TestStack.Test1()                        
            end      

            function Instantiate(obj)
                UnityEngine.Object.Instantiate(obj)
            end

            function TestRay(ray)                
                return Vector3.zero
            end

            function Test2()
                TestStack.Test2()          
            end

            function Test3()
                TestStack.Test3()          
            end
        ";

    void Awake()
    {
        state = new LuaState();
        state.Start();
        LuaBinder.Bind(state);

        state.BeginModule(null);
        state.BeginStaticLibs("TestStack");
        state.RegFunction("Test1", Test1);
        state.RegFunction("Test2", Test2);
        state.RegFunction("Test3", Test3);
        state.EndStaticLibs();
        state.EndModule();

        state.DoString(script, "script");
        show = state.GetFunction("Show");
        testRay = state.GetFunction("TestRay");
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 120, 40), "Error1"))
        {
            LuaFunction func = state.GetFunction("ShowStack");
            func.Call();
            func.Dispose();
            func = null;
        }
        else if (GUI.Button(new Rect(10, 60, 120, 40), "Instantiate Error"))
        {
            LuaFunction func = state.GetFunction("Instantiate");
            func.BeginPCall();
            func.Push(go);
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }
        else if (GUI.Button(new Rect(10, 110, 120, 40), "Check Error"))
        {
            LuaFunction func = state.GetFunction("TestRay");
            func.BeginPCall();
            func.PCall();
            func.CheckRay();
            func.EndPCall();
            func.Dispose();
        }
        else if (GUI.Button(new Rect(10, 160, 120, 40), "Push Error"))
        {
            LuaFunction func = state.GetFunction("TestRay");
            func.BeginPCall();
            func.Push(new Ray());
            func.PCall();
            func.CheckRay();
            func.EndPCall();
            func.Dispose();
        }
        else if (GUI.Button(new Rect(10, 210, 120, 40), "Push Error"))
        {
            LuaFunction func = state.GetFunction("Test2");
            func.BeginPCall();
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }
        else if (GUI.Button(new Rect(10, 260, 120, 40), "Check Error"))
        {
            LuaFunction func = state.GetFunction("Test3");
            func.BeginPCall();
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }
    }
}
