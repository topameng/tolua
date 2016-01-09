using UnityEngine;
using System.Collections;
using LuaInterface;
using System;
using System.IO;

public class ScriptsFromFile : MonoBehaviour 
{
	void Start () 
    {        
        LuaState lua = new LuaState();
        lua.Start();        
        
        string fullPath = Application.dataPath + "/ToLua/Examples/02_ScriptsFromFile";
        lua.AddSearchPath(fullPath);         
        lua.DoFile("ScriptsFromFile.lua");        
        //lua.DoString("require 'ScriptsFromFile'");                             
        //lua.Require("ScriptsFromFile");                

        lua.Dispose();
	}
}
