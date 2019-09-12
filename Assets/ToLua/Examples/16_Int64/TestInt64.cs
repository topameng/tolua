using UnityEngine;
using System.Collections;
using System;
using LuaInterface;
using System.Collections.Generic;


public class TestInt64 : MonoBehaviour
{
    private string tips = "";

    string script =
        @"            
            function TestInt64(x)                
                x = 789 + x
                assert(tostring(x) == '9223372036854775807')	                
                
                local low, high = int64.tonum2(x)                
                print('x value is: '..tostring(x)..' low is: '.. low .. ' high is: '..high.. ' type is: '.. tolua.typename(x))           
                local y = int64.new(1,2)                
                local z = int64.new(1,2)
                
                if y == z then
                    print('int64 equals int64 is ok, value: '..tostring(y))
                end

                x = int64.new(123)                   
            
                if int64.equals(x, 123) then
                    print('int64 equals to number ok')
                else
                    print('int64 equals to number failed')
                end

                x = int64.new('78962871035984074')
                print('int64 x is: '..tostring(x))

                local str = int64.tostring(int64.new(3605690779, 30459971))                
                local n2 = int64.new(str)
                local l, h = int64.tonum2(n2)                        
                print(str..':'..int64.tostring(n2)..' low:'..l..' high:'..h)       

                x = x / 8 * 2 + 99 - 4    --基本的四则运算            
                print('x is '.. int64.tostring(x))       

                print('----------------------------uint64-----------------------------')
                x = uint64.new('18446744073709551615')                                
                print('uint64 max is: '..tostring(x)..' type is '..tolua.typename(x))
                l, h = uint64.tonum2(x)      
                str = tostring(uint64.new(l, h))
                print(str..':'..x:tostring()..' low:'..l..' high:'..h)                                

                x = x / 8 * 2 + 99 - 4    --基本的四则运算            
                print('x is: '.. tostring(x))                          
                x = x / x
                x = 8 / x
                                
                if (x:equals(8)) then
                    print('x equals '..x:tostring())
                end

                return y
            end
        ";


    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int _uint64eq(IntPtr L)    
    {
        ulong lhs = LuaDLL.tolua_touint64(L, 1);
        ulong rhs = LuaDLL.tolua_touint64(L, 2);
        LuaDLL.lua_pushboolean(L, lhs == rhs);        
        return 1;
    }

    void Start()
    {
#if UNITY_4_6 || UNITY_4_7
        Application.RegisterLogCallback(ShowTips);
#else
        Application.logMessageReceived += ShowTips;
#endif      
        new LuaResLoader();
        LuaState lua = new LuaState();
        lua.Start();
        lua.DoString(script, "TestInt64.cs");

        //lua.BeginModule(null);
        //lua.BeginModule("uint64");
        //lua.RegFunction("__eq", _uint64eq);
        //lua.EndModule();
        //lua.EndModule();

        LuaFunction func = lua.GetFunction("TestInt64");
        long n64 = func.Invoke<long, long>(9223372036854775807 - 789);
        Debugger.Log("int64 return from lua is: {0}", n64);                
        func.Dispose();
        func = null;

        lua.CheckTop();
        lua.Dispose();
        lua = null;

        
        //GameObject go1 = new GameObject();
        //GameObject go2 = GameObject.Instantiate(go1);
        //map.Add(go1, 123456);        
        
        //UnityEngine.Object.Destroy(go1);
        //UnityEngine.Object.Destroy(go2);

        //StartCoroutine(DoCheck(go1, go2));
    }    

    IEnumerator DoCheck(GameObject go1, GameObject go2)
    {
        yield return null;

        if (go1.Equals(null))
        {
            Debugger.LogWarning("go1 == null");
        }

        if (go1.Equals(null))
        {
            Debugger.LogWarning("go1 == go2");
        }
    }

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        tips += msg;
        tips += "\r\n";
    }

    void OnDestroy()
    {
#if UNITY_4_6 || UNITY_4_7
        Application.RegisterLogCallback(null);

#else
        Application.logMessageReceived -= ShowTips;
#endif
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 300, 600, 600), tips);
    }
}
