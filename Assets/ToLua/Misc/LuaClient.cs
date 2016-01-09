using UnityEngine;
using System.Collections.Generic;
using LuaInterface;
using System.Collections;
using System.IO;

public class LuaClient : MonoBehaviour 
{
    public static LuaClient Instance
    {
        get;
        private set;
    }

    LuaState luaState = null;
    LuaFunction updateFunc = null;
    LuaFunction lateUpdateFunc = null;
    LuaFunction fixedUpdateFunc = null;
    LuaFunction levelLoaded = null;       

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

    void Awake()
    {
        Instance = this;             
        luaState = new LuaState();
        luaState.OpenLibs(LuaDLL.luaopen_pb);
        LuaBinder.Bind(luaState);
        LuaCoroutine.Register(luaState, this);
    }

	void Start () 
    {
        luaState.Start();
        luaState.DoFile("Main.lua");

        updateFunc = luaState.GetFunction("Update");
        lateUpdateFunc = luaState.GetFunction("LateUpdate");
        fixedUpdateFunc = luaState.GetFunction("FixedUpdate");
        levelLoaded = luaState.GetFunction("OnLevelWasLoaded");

        LuaFunction main = luaState.GetFunction("Main");
        main.Call();
        main.Dispose();
        main = null;

        UpdateEvent = GetEvent("UpdateBeat");
        LateUpdateEvent = GetEvent("LateUpdateBeat");
        FixedUpdateEvent = GetEvent("FixedUpdateBeat");                
	}

    LuaEvent GetEvent(string name)
    {
        LuaTable table = luaState.GetTable(name);
        LuaEvent e = new LuaEvent(table);
        table.Dispose();
        table = null;
        return e;
    }
		
	void Update () 
    {
        if (updateFunc != null)
        {
            updateFunc.BeginPCall(TracePCall.Ignore);
            updateFunc.Push(Time.deltaTime);
            updateFunc.Push(Time.unscaledDeltaTime);
            updateFunc.PCall();
            updateFunc.EndPCall();
        }

        luaState.Collect();

#if UNITY_EDITOR
        luaState.CheckTop();
#endif
	}

    void LateUpdate()
    {
        if (lateUpdateFunc != null)
        {
            lateUpdateFunc.BeginPCall(TracePCall.Ignore);
            lateUpdateFunc.PCall();
            lateUpdateFunc.EndPCall();
        }
    }

    void FixedUpdate()
    {
        if (fixedUpdateFunc != null)
        {
            fixedUpdateFunc.BeginPCall(TracePCall.Ignore);
            fixedUpdateFunc.Push(Time.fixedDeltaTime);
            fixedUpdateFunc.PCall();
            fixedUpdateFunc.EndPCall();
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            return;
        }

        levelLoaded.BeginPCall();
        levelLoaded.Push(level);
        levelLoaded.PCall();
        levelLoaded.EndPCall();
    }

    void SafeRelease(ref LuaFunction luaRef)
    {
        if (luaRef != null)
        {
            luaRef.Dispose();
            luaRef = null;
        }
    }

    void SafeRelease(ref LuaEvent luaEvent)
    {
        if (luaEvent != null)
        {
            luaEvent.Dispose();
            luaEvent = null;
        }
    }

    void OnApplicationQuit()
    {
        if (luaState != null)
        {
            SafeRelease(ref updateFunc);
            SafeRelease(ref lateUpdateFunc);
            SafeRelease(ref fixedUpdateFunc);
            SafeRelease(ref levelLoaded);

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

            luaState.Dispose();
            luaState = null;
            Instance = null;
        }
    }

    public static LuaState GetMainState()
    {
        return Instance.luaState;
    }    
}
