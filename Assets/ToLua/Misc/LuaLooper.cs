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

    void ThrowException()
    {
        string error = luaState.LuaToString(-1);
        luaState.LuaPop(2);
        Exception last = LuaException.luaStack;
        LuaException.luaStack = null;        
        throw new LuaException(error, last);
    }

    void Update()
    {
        if (luaState.LuaUpdate(Time.deltaTime, Time.unscaledDeltaTime) != 0)
        {
            ThrowException();
        }

        luaState.LuaPop(1);
        luaState.Collect();
#if UNITY_EDITOR
        luaState.CheckTop();
#endif
    }

    void LateUpdate()
    {
        if (luaState.LuaLateUpdate() != 0)
        {
            ThrowException();
        }

        luaState.LuaPop(1);
    }

    void FixedUpdate()
    {
        if (luaState.LuaFixedUpdate(Time.fixedDeltaTime) != 0)
        {
            ThrowException();
        }

        luaState.LuaPop(1);
    }

    public void Destroy()
    {
        if (luaState != null)
        {
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
