using UnityEngine;
using LuaInterface;
using System;
using System.Collections;

public static class LuaCoroutine
{
    static MonoBehaviour mb = null;

    public static void Register(LuaState state, MonoBehaviour behaviour)
    {        
        state.BeginModule(null);
        state.RegFunction("WaitForSeconds", WaitForSeconds);
        state.RegFunction("WaitForFixedUpdate", WaitForFixedUpdate);
        state.RegFunction("WaitForEndOfFrame", WaitForEndOfFrame);        
        state.RegFunction("Yield", Yield);                
        state.EndModule();

        mb = behaviour;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int WaitForSeconds(IntPtr L)
    {
        if (LuaDLL.lua_pushthread(L) == 1)
        {
            LuaDLL.luaL_error(L, "attempt to yield from outside a coroutine");
            return 0;
        }

        float sec = (float)LuaDLL.luaL_checknumber(L, 1);
        mb.StartCoroutine(CoWaitForSeconds(L, sec));
        return LuaDLL.lua_yield(L, 0);
    }

    static IEnumerator CoWaitForSeconds(IntPtr L, float sec)
    {
        yield return new WaitForSeconds(sec);
        LuaDLL.lua_pushthread(L);
        int ret = LuaDLL.lua_resume(L, 0);

        if (ret > (int)LuaThreadStatus.LUA_YIELD)
        {
            string error = ToLua.TracebackError(L);
            LuaDLL.lua_pop(L, 1);
            throw new LuaException(error);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int WaitForFixedUpdate(IntPtr L)
    {
        if (LuaDLL.lua_pushthread(L) == 1)
        {
            LuaDLL.luaL_error(L, "attempt to yield from outside a coroutine");
            return 0;
        }
        
        mb.StartCoroutine(CoWaitForFixedUpdate(L));
        return LuaDLL.lua_yield(L, 0);
    }

    static IEnumerator CoWaitForFixedUpdate(IntPtr L)
    {        
        yield return new WaitForFixedUpdate();

        LuaDLL.lua_pushthread(L);
        int ret = LuaDLL.lua_resume(L, 0);

        if (ret > (int)LuaThreadStatus.LUA_YIELD)
        {
            string error = ToLua.TracebackError(L);
            LuaDLL.lua_pop(L, 1);
            throw new LuaException(error);
        }        
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int WaitForEndOfFrame(IntPtr L)
    {
        if (LuaDLL.lua_pushthread(L) == 1)
        {
            LuaDLL.luaL_error(L, "attempt to yield from outside a coroutine");
            return 0;
        }

        mb.StartCoroutine(CoWaitForEndOfFrame(L));
        return LuaDLL.lua_yield(L, 0);
    }

    static IEnumerator CoWaitForEndOfFrame(IntPtr L)
    {
        yield return new WaitForEndOfFrame();

        LuaDLL.lua_pushthread(L);
        int ret = LuaDLL.lua_resume(L, 0);

        if (ret > (int)LuaThreadStatus.LUA_YIELD)
        {
            string error = ToLua.TracebackError(L);
            LuaDLL.lua_pop(L, 1);
            throw new LuaException(error);
        }        
    }   

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Yield(IntPtr L)
    {
        if (LuaDLL.lua_pushthread(L) == 1)
        {
            LuaDLL.luaL_error(L, "attempt to yield from outside a coroutine");
            return 0;
        }

        object o = ToLua.ToVarObject(L, 1);
        mb.StartCoroutine(CoYield(L, o));
        return LuaDLL.lua_yield(L, 0);
    }

    static IEnumerator CoYield(IntPtr L, object o)
    {
        if (o is IEnumerator)
        {
            yield return mb.StartCoroutine((IEnumerator)o);
        }
        else
        {
            yield return o;
        }

        LuaDLL.lua_pushthread(L);
        int ret = LuaDLL.lua_resume(L, 0);

        if (ret > (int)LuaThreadStatus.LUA_YIELD)
        {
            string error = ToLua.TracebackError(L);
            LuaDLL.lua_pop(L, 1);
            throw new LuaException(error);
        }   
    }
    
    
    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Resume(IntPtr L)
    {
        IntPtr L0 = LuaDLL.lua_tothread(L, 1);
        int ret = LuaDLL.lua_resume(L0, 0);

        if (ret > (int)LuaThreadStatus.LUA_YIELD)
        {
            string error = ToLua.TracebackError(L0);
            LuaDLL.lua_pop(L0, 1);
            LuaDLL.luaL_error(L0, error);            
        }

        return 2;
    }
}

