using UnityEngine;
using System.Collections;
using LuaInterface;

public class TestCJson : LuaClient
{
    string script = @"
    local json = require 'cjson'

    function Test(str)
	    local data = json.decode(str)
        print(data.glossary.title)
	    s = json.encode(data)
	    print(s)
    end
";
    protected override LuaFileUtils InitLoader()
    {
        return new LuaResLoader();
    }
    
    protected override void OpenLibs()
    {
        base.OpenLibs();
        OpenCJson();                   
    }

    protected override void OnLoadFinished()
    {        
        base.OnLoadFinished();

        TextAsset text = (TextAsset)Resources.Load("jsonexample", typeof(TextAsset));
        string str = text.ToString();
        luaState.DoString(script);
        LuaFunction func = luaState.GetFunction("Test");
        func.BeginPCall();
        func.Push(str);
        func.PCall();
        func.EndPCall();
        func.Dispose();                        
    }

    //屏蔽，例子不需要运行
    protected override void CallMain() { }
}
