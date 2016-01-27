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
    public static LuaFunction showStack = null;
    public static LuaFunction test4 = null;

    //static GameObject _go = null;

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
            return LuaDLL.toluaL_exception(L, e);
        }

        return 0;
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
            return LuaDLL.toluaL_exception(L, e);
        }

        return 0;
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
            return LuaDLL.toluaL_exception(L, e);
        }

        return 0;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Test4(IntPtr L)
    {
        try
        {
            show.BeginPCall();
            show.PCall();
            show.EndPCall();
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }

        return 0;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Test5(IntPtr L)
    {
        try
        {
            test4.BeginPCall();
            test4.PCall();
            test4.EndPCall();
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }

        return 0;
    }

    LuaState state = null;

    string script =
        @"
            function Show()
                error('this is error')                
            end

            function ShowStack(go)
                TestStack.Test1(go)                        
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

            function Test4()
                TestStack.Test4()          
            end

            function Test5()
                TestStack.Test5()          
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
        state.RegFunction("Test4", Test4);
        state.RegFunction("Test5", Test5);
        state.EndStaticLibs();
        state.EndModule();
        
        state.DoString(script, "Test");
        show = state.GetFunction("Show");
        testRay = state.GetFunction("TestRay");

        showStack = state.GetFunction("ShowStack");
        test4 = state.GetFunction("Test4");
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 120, 40), "Error1"))
        {
            showStack.BeginPCall();
            showStack.Push(go);
            showStack.PCall();
            showStack.EndPCall();
            showStack.Dispose();
            showStack = null;
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
            LuaFunction func = state.GetFunction("Test5");
            func.BeginPCall();
            func.PCall();
            func.EndPCall();
            func.Dispose();
        }
    }
}
