using UnityEngine;
using System.Collections;
using System;
using LuaInterface;

/*public class TestInt64Exp
{
    public Func<LuaInteger64, LuaInteger64> click;
    public LuaInteger64 value;

    public void Test(long n)
    {

    }

    public void Test(LuaInteger64 n) { }

    public LuaInteger64 Test1() { return 0; }

    public LuaInteger64 Test2(LuaInteger64 n) { return 0; }
}*/


public class TestInt64 : MonoBehaviour 
{
    string script =
        @"
            function TestInt64(x)
                x = x + 789
                local low, high = x:toint32()
                print('x value is: '..tostring(x)..' low is: '.. low .. ' high is: '..high)           
                local y = tolua.int64(1,2)                
                local z = tolua.int64(1,2)
                
                if y == z then
                    print('int64 equals is ok, value: '..tostring(y))
                end

                x = tolua.int64(123)                   
            
                if x:equals(123) then
                    print('int64 equals to number ok')
                end

                x = tolua.int64('78962871035984074')
                print('int64 is', tostring(x))

                local t = {}
                t[y] = 123
                
                --lua table userdata做key就这样
                print('for lua userdata in table, t[z] not t[y], table index return not 123 but '..(t[z] or 'nil'))
                return y
            end
        ";

	void Start () 
    {
        LuaState lua = new LuaState();
        lua.Start();
        lua.DoString(script);

        long x  = 123456789123456789;
        double low = (double)(x & 0xFFFFFFFF);
        double high = (double)(x >> 32);
        Debugger.Log("number x low is {0}, high is {1}", low, high);

        LuaFunction func = lua.GetFunction("TestInt64");
        func.BeginPCall();
        func.PushInt64(123456789123456000);
        func.PCall();
        LuaInteger64 n64 = func.CheckInteger64();
        Debugger.Log("int64 return from lua is: {0}", n64);
        func.EndPCall();
        func.Dispose();
        func = null;

        lua.CheckTop();
        lua.Dispose();        
	}	
}
