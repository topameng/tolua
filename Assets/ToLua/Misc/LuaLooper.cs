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
using System;
using UnityEngine;
using LuaInterface;

public class LuaLooper : MonoBehaviour 
{    
    LuaFunction updateFunc = null;
    LuaFunction lateUpdateFunc = null;
    LuaFunction fixedUpdateFunc = null;    

    public LuaEvent UpdateEvent
    {
        get;
        private set;
    }

    public LuaEvent LateUpdateEvent
    {
        get;
        private set;
    }

    public LuaEvent FixedUpdateEvent
    {
        get;
        private set;
    }

    public LuaState luaState = null;

    void Start() 
    {
        try
        {
            updateFunc = GetLuaFunction("Update");
            lateUpdateFunc = GetLuaFunction("LateUpdate");
            fixedUpdateFunc = GetLuaFunction("FixedUpdate");            

            UpdateEvent = GetEvent("UpdateBeat");
            LateUpdateEvent = GetEvent("LateUpdateBeat");
            FixedUpdateEvent = GetEvent("FixedUpdateBeat");
        }
        catch (Exception e)
        {
            Destroy(this);
            throw e;
        }
	}

    LuaFunction GetLuaFunction(string name)
    {
        LuaFunction func = luaState.GetFunction(name, false);

        if (func == null)
        {
            throw new LuaException(string.Format("Lua function {0} not exists", name));
        }

        return func;
    }

    LuaEvent GetEvent(string name)
    {
        LuaTable table = luaState.GetTable(name);

        if (table == null)
        {
            throw new LuaException(string.Format("Lua table {0} not exists", name));
        }

        LuaEvent e = new LuaEvent(table);
        table.Dispose();
        table = null;
        return e;
    }

    void Update()
    {
        updateFunc.BeginPCall();
        updateFunc.Push(Time.deltaTime);
        updateFunc.Push(Time.unscaledDeltaTime);
        updateFunc.PCall();
        updateFunc.EndPCall();

        luaState.Collect();
#if UNITY_EDITOR
        luaState.CheckTop();
#endif
    }

    void LateUpdate()
    {
        lateUpdateFunc.BeginPCall();
        lateUpdateFunc.PCall();
        lateUpdateFunc.EndPCall();
    }

    void FixedUpdate()
    {
        fixedUpdateFunc.BeginPCall();
        fixedUpdateFunc.Push(Time.fixedDeltaTime);
        fixedUpdateFunc.PCall();
        fixedUpdateFunc.EndPCall();
    }

    void SafeRelease(ref LuaFunction luaRef)
    {
        if (luaRef != null)
        {
            luaRef.Dispose();
            luaRef = null;
        }
    }

    public void Destroy()
    {
        if (luaState != null)
        {
            SafeRelease(ref updateFunc);
            SafeRelease(ref lateUpdateFunc);
            SafeRelease(ref fixedUpdateFunc);            

            if (UpdateEvent != null)
            {
                UpdateEvent.Dispose();
                UpdateEvent = null;
            }

            if (LateUpdateEvent != null)
            {
                LateUpdateEvent.Dispose();
                LateUpdateEvent = null;
            }

            if (FixedUpdateEvent != null)
            {
                FixedUpdateEvent.Dispose();
                FixedUpdateEvent = null;
            }

            luaState = null;
        }
    }

    void OnDestroy()
    {
        if (luaState != null)
        {
            Destroy();
        }
    }
}
