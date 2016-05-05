/*
Copyright (c) 2015-2016 topameng(topameng@qq.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using UnityEngine;
using LuaInterface;
using System;
using System.Collections;

public static class LuaCoroutine
{
    static MonoBehaviour mb = null;    

    static string strCo =
        @"
        local _WaitForSeconds, _WaitForFixedUpdate, _WaitForEndOfFrame, _Yield = WaitForSeconds, WaitForFixedUpdate, WaitForEndOfFrame, Yield        
        local comap = {}
        setmetatable(comap, {__mode = 'kv'})

        function WaitForSeconds(t)
            local co = coroutine.running()
            local resume = function()    
                if comap[co] then
                    return coroutine.resume(co)
                end                            
            end
            
            comap[co] = true
            _WaitForSeconds(t, resume)
            return coroutine.yield()
        end

        function WaitForFixedUpdate()
            local co = coroutine.running()
            local resume = function()          
                if comap[co] then      
                    return coroutine.resume(co)
                end
            end
        
            comap[co] = true
            _WaitForFixedUpdate(resume)
            return coroutine.yield()
        end

        function WaitForEndOfFrame()
            local co = coroutine.running()
            local resume = function()        
                if comap[co] then        
                    return coroutine.resume(co)
                end
            end
        
            comap[co] = true
            _WaitForEndOfFrame(resume)
            return coroutine.yield()
        end

        function Yield(o)
            local co = coroutine.running()
            local resume = function()        
                if comap[co] then        
                    return coroutine.resume(co)
                end
            end
        
            comap[co] = true
            _Yield(o, resume)
            return coroutine.yield()
        end

        function StartCoroutine(func)
            local co = coroutine.create(func)                       
            coroutine.resume(co)
            return co
        end

        function StopCoroutine(co)
            comap[co] = false
        end
        ";

    public static void Register(LuaState state, MonoBehaviour behaviour)
    {        
        state.BeginModule(null);
        state.RegFunction("WaitForSeconds", WaitForSeconds);
        state.RegFunction("WaitForFixedUpdate", WaitForFixedUpdate);
        state.RegFunction("WaitForEndOfFrame", WaitForEndOfFrame);        
        state.RegFunction("Yield", Yield);                
        state.EndModule();        

        state.LuaDoString(strCo);
        mb = behaviour;                       
    }

    //另一种方式，非脚本回调方式(用脚本方式更好，可避免lua_yield异常出现在c#函数中)
    /*[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int WaitForSeconds(IntPtr L)
    {
        try
        {
            LuaState state = LuaState.Get(L);
            LuaDLL.lua_pushthread(L);
            LuaThread thread = ToLua.ToLuaThread(L, -1);
            float sec = (float)LuaDLL.luaL_checknumber(L, 1);
            mb.StartCoroutine(CoWaitForSeconds(sec, thread));
            return LuaDLL.lua_yield(L, 0);
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    static IEnumerator CoWaitForSeconds(float sec, LuaThread thread)
    {
        yield return new WaitForSeconds(sec);
        thread.Resume();
    }*/

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int WaitForSeconds(IntPtr L)
    {
        try
        {
            float sec = (float)LuaDLL.luaL_checknumber(L, 1);
            LuaFunction func = ToLua.ToLuaFunction(L, 2);
            mb.StartCoroutine(CoWaitForSeconds(sec, func));
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    static IEnumerator CoWaitForSeconds(float sec, LuaFunction func)
    {
        yield return new WaitForSeconds(sec);
        func.Call();
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int WaitForFixedUpdate(IntPtr L)
    {
        try
        {
            LuaFunction func = ToLua.ToLuaFunction(L, 1);
            mb.StartCoroutine(CoWaitForFixedUpdate(func));
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    static IEnumerator CoWaitForFixedUpdate(LuaFunction func)
    {        
        yield return new WaitForFixedUpdate();
        func.Call();
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int WaitForEndOfFrame(IntPtr L)
    {
        try
        {
            LuaFunction func = ToLua.ToLuaFunction(L, 1);
            mb.StartCoroutine(CoWaitForEndOfFrame(func));
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    static IEnumerator CoWaitForEndOfFrame(LuaFunction func)
    {
        yield return new WaitForEndOfFrame();
        func.Call();
    }   

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Yield(IntPtr L)
    {
        try
        {
            object o = ToLua.ToVarObject(L, 1);
            LuaFunction func = ToLua.ToLuaFunction(L, 2);
            mb.StartCoroutine(CoYield(o, func));
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    static IEnumerator CoYield(object o, LuaFunction func)
    {
        if (o is IEnumerator)
        {
            yield return mb.StartCoroutine((IEnumerator)o);
        }
        else
        {
            yield return o;
        }

        func.Call();
    }   
}

