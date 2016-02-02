using UnityEngine;
using System.Collections.Generic;
using LuaInterface;

public class AccessingLuaVariables : MonoBehaviour 
{
    private string script =
        @"
            print('Objs2Spawn is: '..Objs2Spawn)
            var2read = 42
            varTable = {1,2,3,4,5}
            varTable.default = 1
            varTable.map = {}
            varTable.map.name = 'map'
            
            meta = {name = 'meta'}
            setmetatable(varTable, meta)
            
            function TestFunc(strs)
                print('get func by variable')
            end
        ";

	void Start () 
    {        
        LuaState lua = new LuaState();
        lua.Start();
        lua["Objs2Spawn"] = 5;
        lua.DoString(script);

        //通过LuaState访问
        Debugger.Log("Read var from lua: {0}", lua["var2read"]);
        Debugger.Log("Read table var from lua: {0}", lua["varTable.default"]);

        LuaFunction func = lua["TestFunc"] as LuaFunction;
        func.Call();
        func.Dispose();

        //cache成LuaTable进行访问
        LuaTable table = lua.GetTable("varTable");
        Debugger.Log("Read varTable from lua, default: {0} name: {1}", table["default"], table["map.name"]);
        table["map.name"] = "new";
        Debugger.Log("Modify varTable name: {0}", table["map.name"]);

        table.AddTable("newmap");
        LuaTable table1 = (LuaTable)table["newmap"];
        table1["name"] = "table1";
        Debugger.Log("varTable.newmap name: {0}", table1["name"]);
        table1.Dispose();

        table1 = table.GetMetaTable();

        if (table1 != null)
        {
            Debugger.Log("varTable metatable name: {0}", table1["name"]);
        }

        object[] list = table.ToArray();

        for (int i = 0; i < list.Length; i++)
        {
            Debugger.Log("varTable[{0}], is {1}", i, list[i]);
        }

        table.Dispose();
        lua.CheckTop();
        lua.Dispose();
	}
}
