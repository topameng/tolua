using UnityEngine;
using System;
using System.Collections;
using LuaInterface;

//例子5和6展示的两套协同系统勿交叉使用，此为推荐方案
public class TestCoroutine : MonoBehaviour 
{
    public TextAsset luaFile = null;
    private LuaState lua = null;
    private LuaLooper looper = null;

	void Awake () 
    {        
        lua  = new LuaState();
        lua.Start();
        LuaBinder.Bind(lua);                
        looper = gameObject.AddComponent<LuaLooper>();
        looper.luaState = lua;

        lua.DoString(luaFile.text, "TestCoroutine.cs");
        LuaFunction f = lua.GetFunction("TestCortinue");
        f.Call();
        f.Dispose();
        f = null;        
    }

    void OnDestroy()
    {
        looper.Destroy();
        lua.Dispose();
        lua = null;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 120, 45), "Start Counter"))
        {
            LuaFunction func = lua.GetFunction("StartDelay");
            func.Call();
            func.Dispose();
        }
        else if (GUI.Button(new Rect(50, 150, 120, 45), "Stop Counter"))
        {
            LuaFunction func = lua.GetFunction("StopDelay");
            func.Call();
            func.Dispose();
        }
        else if (GUI.Button(new Rect(50, 250, 120, 45), "GC"))
        {
            lua.DoString("collectgarbage('collect')", "TestCoroutine.cs");
            Resources.UnloadUnusedAssets();
        }
    }
}
