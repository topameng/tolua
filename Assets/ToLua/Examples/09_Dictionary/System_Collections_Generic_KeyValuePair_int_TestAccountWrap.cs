using System;
using LuaInterface;

public class System_Collections_Generic_KeyValuePair_int_TestAccountWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(System.Collections.Generic.KeyValuePair<int,TestAccount>), null, "KeyValuePair_int_TestAccount");
		L.RegFunction("ToString", ToString);
		L.RegFunction("New", _CreateSystem_Collections_Generic_KeyValuePair_int_TestAccount);		
		L.RegFunction("__tostring", Lua_ToString);
		L.RegVar("Key", get_Key, null);
		L.RegVar("Value", get_Value, null);
		L.RegVar("out", get_out, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSystem_Collections_Generic_KeyValuePair_int_TestAccount(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2)
		{
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
			TestAccount arg1 = (TestAccount)ToLua.CheckObject(L, 2, typeof(TestAccount));
			System.Collections.Generic.KeyValuePair<int,TestAccount> obj = new System.Collections.Generic.KeyValuePair<int,TestAccount>(arg0, arg1);
			ToLua.PushValue(L, obj);
			return 1;
		}
		else if (count == 0)
		{
			System.Collections.Generic.KeyValuePair<int,TestAccount> obj = new System.Collections.Generic.KeyValuePair<int,TestAccount>();
			ToLua.PushValue(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Collections.Generic.KeyValuePair<int,TestAccount>.New");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToString(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Collections.Generic.KeyValuePair<int,TestAccount> obj = (System.Collections.Generic.KeyValuePair<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.KeyValuePair<int,TestAccount>));
		string o = null;

		try
		{
			o = obj.ToString();
		}
		catch(Exception e)
		{
			LuaDLL.luaL_error(L, e.Message);
			return 0;
		}

		LuaDLL.lua_pushstring(L, o);
		ToLua.SetBack(L, 1, obj);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Lua_ToString(IntPtr L)
	{
		object obj = ToLua.ToObject(L, 1);

		if (obj != null)
		{
			LuaDLL.lua_pushstring(L, obj.ToString());
		}
		else
		{
			LuaDLL.lua_pushstring(L, "class System.Collections.Generic.KeyValuePair<int,TestAccount>");
		}

		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Key(IntPtr L)
	{
		object o = ToLua.ToObject(L, 1);
		System.Collections.Generic.KeyValuePair<int,TestAccount> obj = (System.Collections.Generic.KeyValuePair<int,TestAccount>)o;
		int ret;

		try
		{
			ret = obj.Key;
		}
		catch(Exception e)
		{
			if (o == null)
			{
				LuaDLL.luaL_error(L, "attempt to index Key on a nil value");
			}
			else
			{
				LuaDLL.luaL_error(L, e.Message);
			}
			return 0;
		}

		LuaDLL.lua_pushinteger(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Value(IntPtr L)
	{
		object o = ToLua.ToObject(L, 1);
		System.Collections.Generic.KeyValuePair<int,TestAccount> obj = (System.Collections.Generic.KeyValuePair<int,TestAccount>)o;
		TestAccount ret = null;

		try
		{
			ret = obj.Value;
		}
		catch(Exception e)
		{
			if (o == null)
			{
				LuaDLL.luaL_error(L, "attempt to index Value on a nil value");
			}
			else
			{
				LuaDLL.luaL_error(L, e.Message);
			}
			return 0;
		}

		ToLua.PushObject(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_out(IntPtr L)
	{
		ToLua.PushOut<System.Collections.Generic.KeyValuePair<int,TestAccount>>(L, new LuaOut<System.Collections.Generic.KeyValuePair<int,TestAccount>>());
		return 1;
	}
}

