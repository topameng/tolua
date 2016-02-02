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
		try
		{
			ToLua.CheckArgsCount(L, 1);
			System.Collections.Generic.KeyValuePair<int,TestAccount> obj = (System.Collections.Generic.KeyValuePair<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.KeyValuePair<int,TestAccount>));
			string o = obj.ToString();
			LuaDLL.lua_pushstring(L, o);
			ToLua.SetBack(L, 1, obj);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
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
			LuaDLL.lua_pushnil(L);
		}

		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Key(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);

			System.Collections.Generic.KeyValuePair<int,TestAccount> obj = (System.Collections.Generic.KeyValuePair<int,TestAccount>)o;
			int ret = obj.Key;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index Key on a nil value" : e.Message);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Value(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);

			System.Collections.Generic.KeyValuePair<int,TestAccount> obj = (System.Collections.Generic.KeyValuePair<int,TestAccount>)o;
			TestAccount ret = obj.Value;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index Value on a nil value" : e.Message);
		}
	}
}

