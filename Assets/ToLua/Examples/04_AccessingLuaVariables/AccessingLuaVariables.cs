using UnityEngine;
using System.Collections;
using LuaInterface;

public class AccessingLuaVariables : MonoBehaviour 
{
    private string script =
        @"
            print('Objs2Spawn is: '..Objs2Spawn)
            var2read = 42
            Layers = {}
            Layers.default = 1

            function TestFunc(strs)
                print('get func by variable')
            end
        ";

	void Start () 
    {        
        LuaState lua = new LuaState();
        lua["Objs2Spawn"] = 5;
        lua.DoString(script);

        Debugger.Log("Read var from lua: {0}", lua["var2read"]);
        Debugger.Log("Read table var from lua: {0}", lua["Layers.default"]);

        LuaFunction func = lua["TestFunc"] as LuaFunction;
        func.Call();

        func.Dispose();
        lua.CheckTop();
        lua.Dispose();
	}
}
