using System;
using LuaInterface;

public class TestAccountWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(TestAccount), typeof(System.Object));
		L.RegFunction("New", _CreateTestAccount);		
		L.RegVar("id", get_id, set_id);
		L.RegVar("name", get_name, set_name);
		L.RegVar("sex", get_sex, set_sex);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateTestAccount(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3)
		{
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
			string arg1 = ToLua.CheckString(L, 2);
			int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
			TestAccount obj = new TestAccount(arg0, arg1, arg2);
			ToLua.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: TestAccount.New");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_id(IntPtr L)
	{
		TestAccount obj = (TestAccount)ToLua.ToObject(L, 1);
		int ret;

		try
		{
			ret = obj.id;
		}
		catch(Exception e)
		{
			if (obj == null)
			{
				LuaDLL.luaL_error(L, "attempt to index id on a nil value");
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
	static int get_name(IntPtr L)
	{
		TestAccount obj = (TestAccount)ToLua.ToObject(L, 1);
		string ret = null;

		try
		{
			ret = obj.name;
		}
		catch(Exception e)
		{
			if (obj == null)
			{
				LuaDLL.luaL_error(L, "attempt to index name on a nil value");
			}
			else
			{
				LuaDLL.luaL_error(L, e.Message);
			}
			return 0;
		}

		LuaDLL.lua_pushstring(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_sex(IntPtr L)
	{
		TestAccount obj = (TestAccount)ToLua.ToObject(L, 1);
		int ret;

		try
		{
			ret = obj.sex;
		}
		catch(Exception e)
		{
			if (obj == null)
			{
				LuaDLL.luaL_error(L, "attempt to index sex on a nil value");
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
	static int set_id(IntPtr L)
	{
		TestAccount obj = (TestAccount)ToLua.ToObject(L, 1);
		int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);

		try
		{
			obj.id = arg0;
		}
		catch(Exception e)
		{
			if (obj == null)
			{
				LuaDLL.luaL_error(L, "attempt to index id on a nil value");
			}
			else
			{
				LuaDLL.luaL_error(L, e.Message);
			}
			return 0;
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_name(IntPtr L)
	{
		TestAccount obj = (TestAccount)ToLua.ToObject(L, 1);
		string arg0 = ToLua.CheckString(L, 2);

		try
		{
			obj.name = arg0;
		}
		catch(Exception e)
		{
			if (obj == null)
			{
				LuaDLL.luaL_error(L, "attempt to index name on a nil value");
			}
			else
			{
				LuaDLL.luaL_error(L, e.Message);
			}
			return 0;
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_sex(IntPtr L)
	{
		TestAccount obj = (TestAccount)ToLua.ToObject(L, 1);
		int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);

		try
		{
			obj.sex = arg0;
		}
		catch(Exception e)
		{
			if (obj == null)
			{
				LuaDLL.luaL_error(L, "attempt to index sex on a nil value");
			}
			else
			{
				LuaDLL.luaL_error(L, e.Message);
			}
			return 0;
		}

		return 0;
	}
}

