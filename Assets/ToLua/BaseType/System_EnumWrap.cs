using System;
using LuaInterface;

public class System_EnumWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(System.Enum), null);
		L.RegFunction("GetTypeCode", GetTypeCode);
		L.RegFunction("GetValues", GetValues);
		L.RegFunction("GetNames", GetNames);
		L.RegFunction("GetName", GetName);
		L.RegFunction("IsDefined", IsDefined);
		L.RegFunction("GetUnderlyingType", GetUnderlyingType);
		L.RegFunction("Parse", Parse);
		L.RegFunction("CompareTo", CompareTo);
		L.RegFunction("ToString", ToString);
		L.RegFunction("ToObject", ToObject);
		L.RegFunction("Equals", Equals);
		L.RegFunction("GetHashCode", GetHashCode);
		L.RegFunction("Format", Format);
		L.RegFunction("ToInt", ToInt);
		L.RegFunction("New", _CreateSystem_Enum);
		L.RegFunction("__tostring", Lua_ToString);
		L.RegVar("out", get_out, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSystem_Enum(IntPtr L)
	{
		return LuaDLL.luaL_error(L, "System.Enum class does not have a constructor function");
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeCode(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Enum obj = (System.Enum)ToLua.CheckObject(L, 1, typeof(System.Enum));
		System.TypeCode o;

		try
		{
			o = obj.GetTypeCode();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetValues(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type arg0 = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Array o = null;

		try
		{
			o = System.Enum.GetValues(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNames(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type arg0 = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		string[] o = null;

		try
		{
			o = System.Enum.GetNames(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetName(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.Type arg0 = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		object arg1 = ToLua.ToVarObject(L, 2);
		string o = null;

		try
		{
			o = System.Enum.GetName(arg0, arg1);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushstring(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsDefined(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.Type arg0 = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		object arg1 = ToLua.ToVarObject(L, 2);
		bool o;

		try
		{
			o = System.Enum.IsDefined(arg0, arg1);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushboolean(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetUnderlyingType(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type arg0 = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Type o = null;

		try
		{
			o = System.Enum.GetUnderlyingType(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Parse(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string)))
		{
			System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			object o = null;

			try
			{
				o = System.Enum.Parse(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(bool)))
		{
			System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			bool arg2 = LuaDLL.lua_toboolean(L, 3);
			object o = null;

			try
			{
				o = System.Enum.Parse(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Enum.Parse");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CompareTo(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.Enum obj = (System.Enum)ToLua.CheckObject(L, 1, typeof(System.Enum));
		object arg0 = ToLua.ToVarObject(L, 2);
		int o;

		try
		{
			o = obj.CompareTo(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushinteger(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToString(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(System.Enum)))
		{
			System.Enum obj = (System.Enum)ToLua.ToObject(L, 1);
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
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Enum), typeof(string)))
		{
			System.Enum obj = (System.Enum)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			string o = null;

			try
			{
				o = obj.ToString(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Enum.ToString");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToObject(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(long)))
		{
			System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
			long arg1 = (long)LuaDLL.lua_tonumber(L, 2);
			object o = null;

			try
			{
				o = System.Enum.ToObject(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(object)))
		{
			System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
			object arg1 = ToLua.ToVarObject(L, 2);
			object o = null;

			try
			{
				o = System.Enum.ToObject(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Enum.ToObject");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Equals(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.Enum obj = (System.Enum)ToLua.CheckObject(L, 1, typeof(System.Enum));
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
		System.Enum obj = (System.Enum)ToLua.CheckObject(L, 1, typeof(System.Enum));
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
	static int Format(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 3);
		System.Type arg0 = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		object arg1 = ToLua.ToVarObject(L, 2);
		string arg2 = ToLua.CheckString(L, 3);
		string o = null;

		try
		{
			o = System.Enum.Format(arg0, arg1, arg2);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushstring(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToInt(IntPtr L)
	{
		object arg0 = ToLua.CheckObject(L, 1, typeof(System.Enum));
		int ret = Convert.ToInt32(arg0);        
		LuaDLL.lua_pushinteger(L, ret);
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
			LuaDLL.lua_pushnil(L);
		}

		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_out(IntPtr L)
	{
		ToLua.PushOut<System.Enum>(L, new LuaOut<System.Enum>());
		return 1;
	}
}

