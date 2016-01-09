using System;
using LuaInterface;

public class System_ObjectWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(System.Object), null);
		L.RegFunction("Equals", Equals);
		L.RegFunction("GetHashCode", GetHashCode);
		L.RegFunction("GetType", GetType);
		L.RegFunction("ToString", ToString);
		L.RegFunction("ReferenceEquals", ReferenceEquals);
		L.RegFunction("Destroy", Destroy);
		L.RegFunction("New", _CreateSystem_Object);
		L.RegFunction("__eq", op_Equality);
		L.RegVar("out", get_out, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSystem_Object(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			System.Object obj = new System.Object();
			ToLua.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Object.New");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Equals(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		object obj = ToLua.CheckObject(L, 1);
		object arg0 = ToLua.ToVarObject(L, 2);
		bool o;

		try
		{
			o = obj != null ? obj.Equals(arg0) : arg0 == null;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushboolean(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetHashCode(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		object obj = ToLua.CheckObject(L, 1);
		int o;

		try
		{
			o = obj.GetHashCode();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushinteger(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetType(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		object obj = ToLua.CheckObject(L, 1);
		System.Type o = null;

		try
		{
			o = obj.GetType();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToString(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		object obj = ToLua.CheckObject(L, 1);
		string o = null;

		try
		{
			o = obj.ToString();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushstring(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReferenceEquals(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		object arg0 = ToLua.ToVarObject(L, 1);
		object arg1 = ToLua.ToVarObject(L, 2);
		bool o;

		try
		{
			o = System.Object.ReferenceEquals(arg0, arg1);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushboolean(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		object arg0 = ToLua.ToVarObject(L, 1);
		object arg1 = ToLua.ToVarObject(L, 2);
		bool o;

		try
		{
			o = arg0 == arg1;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushboolean(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Destroy(IntPtr L)
	{
		return ToLua.Destroy(L);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_out(IntPtr L)
	{
		ToLua.PushOut<System.Object>(L, new LuaOut<System.Object>());
		return 1;
	}
}

