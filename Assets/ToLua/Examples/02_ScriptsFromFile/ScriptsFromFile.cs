using UnityEngine;
using System.Collections;
using LuaInterface;
using System;
using System.IO;

//展示searchpath 使用，require 与 dofile 区别
public class ScriptsFromFile : MonoBehaviour 
{
    LuaState lua = null;
    private string strLog = "";

	void Start () 
    {
#if UNITY_5		
		Application.logMessageReceived += Log;
#else
        Application.RegisterLogCallback(Log);
#endif 
        lua = new LuaState();
        lua.Start();
        //移动了ToLua路径，自己手动修复吧，只是例子就不做配置了
        string fullPath = Application.dataPath + "/ToLua/Examples/02_ScriptsFromFile";
        lua.AddSearchPath(fullPath);
    }

    void Log(string msg, string stackTrace, LogType type)
    {
        strLog += msg;
        strLog += "\r\n";
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 300), strLog);

        if (GUI.Button(new Rect(50, 50, 120, 45), "DoFile"))
        {
            strLog = "";
            lua.DoFile("ScriptsFromFile.lua");            
        }
        else if (GUI.Button(new Rect(50, 150, 120, 45), "Require"))
        {
            strLog = "";
            lua.Require("ScriptsFromFile");
        }
    }

    void OnApplicationQuit()
    {
        lua.Dispose();
        lua = null;

#if UNITY_5		
		Application.logMessageReceived -= Log;
#else
        Application.RegisterLogCallback(null);
#endif 
    }
}
