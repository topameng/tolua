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

    protected bool openLuaSocket = false;
    protected bool beZbStart = false;

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

        if (LuaConst.openLuaSocket)
        {            
            luaState.OpenLibs(LuaDLL.luaopen_socket_core);
            luaState.OpenLibs(LuaDLL.luaopen_luasocket_scripts);
        }
    }

    public void OpenZbsDebugger(string ip = null)
    {
        if (!LuaConst.openLuaSocket)
        {
            LuaConst.openLuaSocket = true;
            luaState.OpenLibs(LuaDLL.luaopen_socket_core);
            luaState.OpenLibs(LuaDLL.luaopen_luasocket_scripts);
        }
        
        luaState.AddSearchPath(LuaConst.zbsDir);

        if (ip != null)
        {
            luaState.DoString(string.Format("require('mobdebug').start('{0}')", ip));
        }
        else
        {
            luaState.DoString("require('mobdebug').start()");
        }
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
