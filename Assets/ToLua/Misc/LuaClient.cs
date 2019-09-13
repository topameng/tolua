/*
Copyright (c) 2015-2021 topameng(topameng@qq.com)
https://github.com/topameng/tolua

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
using System.Collections.Generic;
using LuaInterface;
using System.Collections;
using System.IO;
using System;
#if UNITY_5_4_OR_NEWER
using UnityEngine.SceneManagement;
#endif

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

    protected bool openLuaSocket = false;
    protected bool beZbStart = false;

    protected virtual LuaFileUtils InitLoader()
    {
        return LuaFileUtils.Instance;                       
    }

    protected virtual void LoadLuaFiles()
    {
        OnLoadFinished();
    }

    protected virtual void OpenLibs()
    {
        //保持库名字与5.1.5库中一致
        luaState.BeginPreLoad();                        
        luaState.AddPreLoadLib("pb2", new LuaCSFunction(LuaDLL.luaopen_pb));
        luaState.AddPreLoadLib("struct", new LuaCSFunction(LuaDLL.luaopen_struct));
        luaState.AddPreLoadLib("lpeg", new LuaCSFunction(LuaDLL.luaopen_lpeg));
        luaState.AddPreLoadLib("cjson", new LuaCSFunction(LuaDLL.luaopen_cjson));
        luaState.AddPreLoadLib("cjson.safe", new LuaCSFunction(LuaDLL.luaopen_cjson_safe));
#if (UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX) && !LUAC_5_3
        luaState.AddPreLoadLib("bit", new LuaCSFunction(LuaDLL.luaopen_bit));
#endif

        if (LuaConst.openLuaSocket || LuaConst.openLuaDebugger)
        {
            OpenLuaSocket();
        }

        luaState.EndPreLoad();          

        if (LuaConst.openLuaDebugger)
        {
            OpenZbsDebugger();
        }
    }

    public void OpenZbsDebugger(string ip = "localhost")
    {
        if (!Directory.Exists(LuaConst.zbsDir))
        {
            Debugger.LogWarning("ZeroBraneStudio not install or LuaConst.zbsDir not right");
            return;
        }

        if (!string.IsNullOrEmpty(LuaConst.zbsDir))
        {
            luaState.AddSearchPath(LuaConst.zbsDir);
        }

        luaState.LuaDoString(string.Format("DebugServerIp = '{0}'", ip), "@LuaClient.cs");
    }

    protected void OpenLuaSocket()
    {
        LuaConst.openLuaSocket = true;        
        luaState.AddPreLoadLib("socket.core", new LuaCSFunction(LuaDLL.luaopen_socket_core));
        luaState.AddPreLoadLib("mime.core", new LuaCSFunction(LuaDLL.luaopen_mime_core));        
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
        luaState.Require("Main");
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
        LuaBinder.Bind(luaState);
        DelegateFactory.Init();   
        LuaCoroutine.Register(luaState, this);        
    }

    protected void Init()
    {        
        InitLoader();
        luaState = new LuaState();
        OpenLibs();
        luaState.LuaSetTop(0);  //清掉可能残留的堆栈
        Bind();        
        LoadLuaFiles();        
    }

    protected void Awake()
    {
        Instance = this;
        Init();

#if UNITY_5_4_OR_NEWER
        SceneManager.sceneLoaded += OnSceneLoaded;
#endif        
    }

    protected virtual void OnLoadFinished()
    {
        luaState.Start();
        StartLooper();
        StartMain();        
    }

    void OnLevelLoaded(int level)
    {
        if (levelLoaded != null)
        {
            levelLoaded.BeginPCall();
            levelLoaded.Push(level);
            levelLoaded.PCall();
            levelLoaded.EndPCall();
        }

        if (luaState != null)
        {            
            luaState.RefreshDelegateMap();
        }
    }

#if UNITY_5_4_OR_NEWER
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnLevelLoaded(scene.buildIndex);
    }
#else
    protected void OnLevelWasLoaded(int level)
    {
        OnLevelLoaded(level);
    }
#endif

    public virtual void Destroy()
    {
        if (luaState != null)
        {
#if UNITY_5_4_OR_NEWER
            SceneManager.sceneLoaded -= OnSceneLoaded;
#endif    
            luaState.Call("OnApplicationQuit", false);
            DetachProfiler();
            LuaState state = luaState;
            luaState = null;

            if (levelLoaded != null)
            {
                levelLoaded.Dispose();
                levelLoaded = null;
            }

            if (loop != null)
            {
                loop.Destroy();
                loop = null;
            }

            state.Dispose();
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

    LuaTable profiler = null;

    public void AttachProfiler()
    {
        if (profiler == null)
        {
            profiler = luaState.Require<LuaTable>("UnityEngine.Profiler");
            profiler.Call("start", profiler);
        }
    }
    public void DetachProfiler()
    {
        if (profiler != null)
        {
            profiler.Call("stop", profiler);
            profiler.Dispose();
            LuaProfiler.Clear();
        }
    }
}
