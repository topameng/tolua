using UnityEngine;
using System;
using System.Collections;
using LuaInterface;

public class TestCoroutine : MonoBehaviour 
{
    public TextAsset luaFile = null;
    private LuaState lua = null;
    private LuaFunction update = null;
    private LuaFunction lateupdate = null;
    private LuaFunction fixedupdate = null;

	void Awake () 
    {        
        lua  = new LuaState();
        lua.Start();
        LuaBinder.Bind(lua);

        lua.DoString(luaFile.text, "LuaCoroutine.lua");
        update = lua.GetFunction("Update");
        lateupdate = lua.GetFunction("LateUpdate");
        fixedupdate = lua.GetFunction("FixedUpdate");                
        LuaFunction f = lua.GetFunction("TestCortinue");
        f.Call();
        f.Dispose();
	}

    void OnDestroy()
    {
        update.Dispose();
        lateupdate.Dispose();
        fixedupdate.Dispose();
        lua.Dispose();
    }
	
	// Update is called once per frame
	void Update () 
    {
        update.BeginPCall(TracePCall.Ignore);
        update.Push(Time.deltaTime);
        update.Push(Time.unscaledDeltaTime);
        update.PCall();
        update.EndPCall();

        lua.CheckTop();
	}

    void LateUpdate()
    {
        lateupdate.BeginPCall(TracePCall.Ignore);
        lateupdate.PCall();
        lateupdate.EndPCall();
    }

    void FixedUpdate()
    {
        fixedupdate.BeginPCall(TracePCall.Ignore);
        fixedupdate.Push(Time.fixedDeltaTime);
        fixedupdate.PCall();
        fixedupdate.EndPCall();        
    }
}
