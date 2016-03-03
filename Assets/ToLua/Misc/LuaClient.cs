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
        protected set;
    }

    protected LuaState luaState = null;
    protected LuaLooper loop = null;
    protected LuaFunction levelLoaded = null;

    protected virtual LuaFileUtils InitLoader()
    {
        return new LuaFileUtils();
    }

    protected virtual void LoadLuaFiles()
    {
        OnLoadFinished();
    }

    protected virtual void OpenLibs()
    {
        luaState.OpenLibs(LuaDLL.luaopen_pb);
        luaState.OpenLibs(LuaDLL.luaopen_struct);
        luaState.OpenLibs(LuaDLL.luaopen_lpeg);
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        luaState.OpenLibs(LuaDLL.luaopen_bit);
#endif
    }

    //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
    protected void OpenCJson()
    {
        luaState.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
        luaState.OpenLibs(LuaDLL.luaopen_cjson);
        luaState.LuaSetField(-2, "cjson");

        luaState.OpenLibs(LuaDLL.luaopen_cjson_safe);
        luaState.LuaSetField(-2, "cjson.safe");
    }

    protected virtual void CallMain()
    {
        LuaFunction main = luaState.GetFunction("Main");
        main.Call();
        main.Dispose();
        main = null;        
    }

    protected virtual void StartMain()
    {
        luaState.DoFile("Main.lua");
        levelLoaded = luaState.GetFunction("OnLevelWasLoaded");
        CallMain();
    }

    protected void StartLooper()
    {
        loop = gameObject.AddComponent<LuaLooper>();
        loop.luaState = luaState;
    }

    protected virtual void Bind()
    {
        LuaCoroutine.Register(luaState, this);
        LuaBinder.Bind(luaState);      
    }

    protected void Init()
    {        
        InitLoader();
        luaState = new LuaState();
        OpenLibs();
        luaState.LuaSetTop(0);
        Bind();
        LoadLuaFiles();    
    }

    protected void Awake()
    {
        Instance = this;
        Init();
    }

    protected virtual void OnLoadFinished()
    {
        luaState.Start();                
        StartLooper();
        StartMain();
    }

    protected void OnLevelWasLoaded(int level)
    {
        if (levelLoaded != null)
        {
            levelLoaded.BeginPCall();
            levelLoaded.Push(level);
            levelLoaded.PCall();
            levelLoaded.EndPCall();
        }
    }

    protected void Destroy()
    {
        if (luaState != null)
        {
            if (levelLoaded != null)
            {
                levelLoaded.Dispose();
                levelLoaded = null;
            }

            loop.Destroy();
            luaState.Dispose();
            loop = null;
            luaState = null;
            Instance = null;
        }
    }

    protected void OnDestroy()
    {
        Destroy();
    }

    protected void OnApplicationQuit()
    {
        Destroy();
    }

    public static LuaState GetMainState()
    {
        return Instance.luaState;
    }

    public LuaLooper GetLooper()
    {
        return loop;
    }
}
