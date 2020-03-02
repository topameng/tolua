using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LuaInterface;
using UnityEngine;
using UnityEngine.Events;

public class TestHotReload : MonoBehaviour
{
    public bool start;

    private LuaState luaState;
    private LuaHotLoader loader;

    private static event UnityAction onPrintTime;


    void Start()
    {
        loader = FindObjectOfType<LuaHotLoader>();
        if (loader == null)
            return;

        luaState = new LuaState();
        luaState.AddSearchPath(loader.FullScriptPath);
        LuaBinder.Bind(luaState);
        DelegateFactory.Init();
        luaState.Start();
        loader.LuaState = luaState;
        StartTest();
    }

    private void OnDisable()
    {
        ResetBellRinger();
    }

    private void StartTest()
    {
        ResetBellRinger();
        luaState.BeginModule(null);
        luaState.BeginStaticLibs("testhotreload");
        luaState.RegVar("onPrintTime", get_onPrintTime, set_onPrintTime);
        luaState.EndStaticLibs();
        luaState.EndModule();
        luaState.DoString("require 'hotreloadmain'");
        luaState.DoString("printtime()");

        StartCoroutine(PrintTime());
        StartCoroutine(ModifyLuaScript());
    }



    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_onPrintTime(IntPtr L)
    {
        ToLua.Push(L, new EventObject(typeof(UnityEngine.Events.UnityAction)));
        return 1;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int set_onPrintTime(IntPtr L)
    {
        try
        {
            EventObject arg0 = null;

            if (LuaDLL.lua_isuserdata(L, 2) != 0)
            {
                arg0 = (EventObject)ToLua.ToObject(L, 2);
            }
            else
            {
                return LuaDLL.luaL_throw(L, "The event 'onPrintTime' can only appear on the left hand side of += or -= when used outside of the type 'UnityEngine.Application'");
            }

            if (arg0.op == EventOp.Add)
            {
                UnityEngine.Events.UnityAction ev = (UnityEngine.Events.UnityAction)arg0.func;
                onPrintTime += ev;
            }
            else if (arg0.op == EventOp.Sub)
            {
                UnityEngine.Events.UnityAction ev = (UnityEngine.Events.UnityAction)arg0.func;
                onPrintTime -= ev;
            }

            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    private IEnumerator PrintTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            luaState.DoString("printtime()");
            Debug.Log("print time by onPrintTime event.");
            onPrintTime.Invoke();
        }
    }

    private IEnumerator ModifyLuaScript()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (start == false)
                continue;
            var bellringer = Path.Combine(loader.FullScriptPath, "bellringer.lua");
            File.WriteAllText(bellringer, string.Format(@"
local bellringer = {{}}

function bellringer.gettime()
    print('Current time is {0}.')
end

function bellringer:getname()
    print('My name is ' .. self.name)
end

return bellringer
", DateTime.Now));
            var gbellringer = Path.Combine(loader.FullScriptPath, "Gbellringer.lua");
            File.WriteAllText(gbellringer, string.Format(@"
Gbellringer = {{}}

function Gbellringer:gettime()
    print('Current time is {0}. My name is ' .. self.name)
end

return 'Gbellringer'
", DateTime.Now));
            loader.CheckAndReload();
        }
    }

    private void ResetBellRinger()
    {
        var bellringer = Path.Combine(loader.FullScriptPath, "bellringer.lua");
        File.WriteAllText(bellringer, @"
local bellringer = {name='best bellringer'}

function bellringer.gettime()
    print('I dont know current time.')
end

function bellringer:getname()
    print('My name is ' .. self.name)
end

return bellringer
");


        var gbellringer = Path.Combine(loader.FullScriptPath, "Gbellringer.lua");
        File.WriteAllText(gbellringer, @"
Gbellringer= {name='best Gbellringer'}

function Gbellringer:gettime()
    print('I dont know current time. My name is ' .. self.name)
end

return 'Gbellringer'
");


    }
}
