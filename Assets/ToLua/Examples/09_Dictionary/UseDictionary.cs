using UnityEngine;
using System.Collections.Generic;
using LuaInterface;

public class TestAccount
{
    public int id;
    public string name;
    public int sex;

    public TestAccount(int id, string name, int sex)
    {
        this.id = id;
        this.name = name;
        this.sex = sex;
    }
}

public class UseDictionary : MonoBehaviour 
{
    Dictionary<int, TestAccount> map = new Dictionary<int, TestAccount>();

    string script =
        @"              
            function TestDict(map)                        
                local iter = map:GetEnumerator() 
                
                while iter:MoveNext() do
                    local v = iter.Current.Value
                    print('id: '..v.id ..' name: '..v.name..' sex: '..v.sex)                                
                end
            end                        
        ";

	void Awake () 
    {
        map.Add(1, new TestAccount(2, "隔壁老王", 2));
        map.Add(2, new TestAccount(1, "王伟", 1));
        map.Add(3, new TestAccount(2, "王芳", 0));
        
        LuaState luaState = new LuaState();
        luaState.Start();
        BindMap(luaState);        

        luaState.DoString(script);        
        LuaFunction func = luaState.GetFunction("TestDict");
        func.BeginPCall();
        func.Push(map);
        func.PCall();
        func.EndPCall();        

        func.Dispose();
        func = null;
        luaState.CheckTop();
        luaState.Dispose();
        luaState = null;    
	}

    //示例方式，正常导出无需手写下面代码
    void BindMap(LuaState L)
    {
        L.BeginModule(null);
        TestAccountWrap.Register(L);
        L.BeginModule("System");
        L.BeginModule("Collections");
        L.BeginModule("Generic");
        System_Collections_Generic_Dictionary_int_TestAccountWrap.Register(L);
        System_Collections_Generic_KeyValuePair_int_TestAccountWrap.Register(L);
        L.EndModule();
        L.EndModule();
        L.EndModule();
        L.EndModule();
    }
}
