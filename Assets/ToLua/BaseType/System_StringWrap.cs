using System;
using LuaInterface;

public class System_StringWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(System.String), typeof(System.Object));
		L.RegFunction("Equals", Equals);
		L.RegFunction("Clone", Clone);
		L.RegFunction("GetTypeCode", GetTypeCode);
		L.RegFunction("CopyTo", CopyTo);
		L.RegFunction("ToCharArray", ToCharArray);
		L.RegFunction("Split", Split);
		L.RegFunction("Substring", Substring);
		L.RegFunction("Trim", Trim);
		L.RegFunction("TrimStart", TrimStart);
		L.RegFunction("TrimEnd", TrimEnd);
		L.RegFunction("Compare", Compare);
		L.RegFunction("CompareTo", CompareTo);
		L.RegFunction("CompareOrdinal", CompareOrdinal);
		L.RegFunction("EndsWith", EndsWith);
		L.RegFunction("IndexOfAny", IndexOfAny);
		L.RegFunction("IndexOf", IndexOf);
		L.RegFunction("LastIndexOf", LastIndexOf);
		L.RegFunction("LastIndexOfAny", LastIndexOfAny);
		L.RegFunction("Contains", Contains);
		L.RegFunction("IsNullOrEmpty", IsNullOrEmpty);
		L.RegFunction("Normalize", Normalize);
		L.RegFunction("IsNormalized", IsNormalized);
		L.RegFunction("Remove", Remove);
		L.RegFunction("PadLeft", PadLeft);
		L.RegFunction("PadRight", PadRight);
		L.RegFunction("StartsWith", StartsWith);
		L.RegFunction("Replace", Replace);
		L.RegFunction("ToLower", ToLower);
		L.RegFunction("ToLowerInvariant", ToLowerInvariant);
		L.RegFunction("ToUpper", ToUpper);
		L.RegFunction("ToUpperInvariant", ToUpperInvariant);
		L.RegFunction("ToString", ToString);
		L.RegFunction("Format", Format);
		L.RegFunction("Copy", Copy);
		L.RegFunction("Concat", Concat);
		L.RegFunction("Insert", Insert);
		L.RegFunction("Intern", Intern);
		L.RegFunction("IsInterned", IsInterned);
		L.RegFunction("Join", Join);
		L.RegFunction("GetEnumerator", GetEnumerator);
		L.RegFunction("GetHashCode", GetHashCode);
		L.RegFunction("New", _CreateSystem_String);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", Lua_ToString);
		L.RegVar("Empty", get_Empty, null);
		L.RegVar("Length", get_Length, null);
		L.RegVar("out", get_out, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSystem_String(IntPtr L)
	{
        LuaTypes luatype = LuaDLL.lua_type(L, 1);

        if (luatype == LuaTypes.LUA_TSTRING)
        {
            string arg0 = LuaDLL.lua_tostring(L, 1);
            ToLua.PushObject(L, arg0);
            return 1;
        }
        else
        {
            LuaDLL.luaL_error(L, "invalid arguments to method: String.New");
        }
        
		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Equals(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
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
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(object)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
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
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(System.StringComparison)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.StringComparison arg1 = (System.StringComparison)ToLua.ToObject(L, 3);
			bool o;

			try
			{
				o = obj != null ? obj.Equals(arg0, arg1) : arg0 == null;
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.Equals");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Clone(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.String obj = (System.String)ToLua.CheckObject(L, 1, typeof(System.String));
		object o = null;

		try
		{
			o = obj.Clone();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeCode(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.String obj = (System.String)ToLua.CheckObject(L, 1, typeof(System.String));
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
	static int CopyTo(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 5);
		System.String obj = (System.String)ToLua.CheckObject(L, 1, typeof(System.String));
		int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
		char[] arg1 = ToLua.CheckCharBuffer(L, 3);
		int arg2 = (int)LuaDLL.luaL_checknumber(L, 4);
		int arg3 = (int)LuaDLL.luaL_checknumber(L, 5);

		try
		{
			obj.CopyTo(arg0, arg1, arg2, arg3);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToCharArray(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char[] o = null;

			try
			{
				o = obj.ToCharArray();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			char[] o = null;

			try
			{
				o = obj.ToCharArray(arg0, arg1);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.ToCharArray");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Split(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char[]), typeof(System.StringSplitOptions)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char[] arg0 = ToLua.CheckCharBuffer(L, 2);
			System.StringSplitOptions arg1 = (System.StringSplitOptions)ToLua.ToObject(L, 3);
			string[] o = null;

			try
			{
				o = obj.Split(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char[]), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char[] arg0 = ToLua.CheckCharBuffer(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			string[] o = null;

			try
			{
				o = obj.Split(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string[]), typeof(System.StringSplitOptions)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string[] arg0 = ToLua.CheckStringArray(L, 2);
			System.StringSplitOptions arg1 = (System.StringSplitOptions)ToLua.ToObject(L, 3);
			string[] o = null;

			try
			{
				o = obj.Split(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string[]), typeof(int), typeof(System.StringSplitOptions)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string[] arg0 = ToLua.CheckStringArray(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			System.StringSplitOptions arg2 = (System.StringSplitOptions)ToLua.ToObject(L, 4);
			string[] o = null;

			try
			{
				o = obj.Split(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char[]), typeof(int), typeof(System.StringSplitOptions)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char[] arg0 = ToLua.CheckCharBuffer(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			System.StringSplitOptions arg2 = (System.StringSplitOptions)ToLua.ToObject(L, 4);
			string[] o = null;

			try
			{
				o = obj.Split(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (ToLua.CheckParamsType(L, typeof(char), 2, count - 1))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char[] arg0 = ToLua.ToParamsChar(L, 2, count - 1);
			string[] o = null;

			try
			{
				o = obj.Split(arg0);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.Split");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Substring(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
			string o = null;

			try
			{
				o = obj.Substring(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			string o = null;

			try
			{
				o = obj.Substring(arg0, arg1);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.Substring");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Trim(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string o = null;

			try
			{
				o = obj.Trim();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (ToLua.CheckParamsType(L, typeof(char), 2, count - 1))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char[] arg0 = ToLua.ToParamsChar(L, 2, count - 1);
			string o = null;

			try
			{
				o = obj.Trim(arg0);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.Trim");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TrimStart(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		System.String obj = (System.String)ToLua.CheckObject(L, 1, typeof(System.String));
		char[] arg0 = ToLua.CheckParamsChar(L, 2, count - 1);
		string o = null;

		try
		{
			o = obj.TrimStart(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushstring(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TrimEnd(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		System.String obj = (System.String)ToLua.CheckObject(L, 1, typeof(System.String));
		char[] arg0 = ToLua.CheckParamsChar(L, 2, count - 1);
		string o = null;

		try
		{
			o = obj.TrimEnd(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushstring(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Compare(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string)))
		{
			string arg0 = ToLua.ToString(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			int o;

			try
			{
				o = System.String.Compare(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(System.StringComparison)))
		{
			string arg0 = ToLua.ToString(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			System.StringComparison arg2 = (System.StringComparison)ToLua.ToObject(L, 3);
			int o;

			try
			{
				o = System.String.Compare(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(bool)))
		{
			string arg0 = ToLua.ToString(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			bool arg2 = LuaDLL.lua_toboolean(L, 3);
			int o;

			try
			{
				o = System.String.Compare(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(System.Globalization.CultureInfo), typeof(System.Globalization.CompareOptions)))
		{
			string arg0 = ToLua.ToString(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			System.Globalization.CultureInfo arg2 = (System.Globalization.CultureInfo)ToLua.ToObject(L, 3);
			System.Globalization.CompareOptions arg3 = (System.Globalization.CompareOptions)ToLua.ToObject(L, 4);
			int o;

			try
			{
				o = System.String.Compare(arg0, arg1, arg2, arg3);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(bool), typeof(System.Globalization.CultureInfo)))
		{
			string arg0 = ToLua.ToString(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			bool arg2 = LuaDLL.lua_toboolean(L, 3);
			System.Globalization.CultureInfo arg3 = (System.Globalization.CultureInfo)ToLua.ToObject(L, 4);
			int o;

			try
			{
				o = System.String.Compare(arg0, arg1, arg2, arg3);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 5 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int), typeof(string), typeof(int), typeof(int)))
		{
			string arg0 = ToLua.ToString(L, 1);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
			string arg2 = ToLua.ToString(L, 3);
			int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
			int arg4 = (int)LuaDLL.lua_tonumber(L, 5);
			int o;

			try
			{
				o = System.String.Compare(arg0, arg1, arg2, arg3, arg4);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 6 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int), typeof(string), typeof(int), typeof(int), typeof(System.StringComparison)))
		{
			string arg0 = ToLua.ToString(L, 1);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
			string arg2 = ToLua.ToString(L, 3);
			int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
			int arg4 = (int)LuaDLL.lua_tonumber(L, 5);
			System.StringComparison arg5 = (System.StringComparison)ToLua.ToObject(L, 6);
			int o;

			try
			{
				o = System.String.Compare(arg0, arg1, arg2, arg3, arg4, arg5);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 6 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int), typeof(string), typeof(int), typeof(int), typeof(bool)))
		{
			string arg0 = ToLua.ToString(L, 1);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
			string arg2 = ToLua.ToString(L, 3);
			int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
			int arg4 = (int)LuaDLL.lua_tonumber(L, 5);
			bool arg5 = LuaDLL.lua_toboolean(L, 6);
			int o;

			try
			{
				o = System.String.Compare(arg0, arg1, arg2, arg3, arg4, arg5);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 7 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int), typeof(string), typeof(int), typeof(int), typeof(System.Globalization.CultureInfo), typeof(System.Globalization.CompareOptions)))
		{
			string arg0 = ToLua.ToString(L, 1);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
			string arg2 = ToLua.ToString(L, 3);
			int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
			int arg4 = (int)LuaDLL.lua_tonumber(L, 5);
			System.Globalization.CultureInfo arg5 = (System.Globalization.CultureInfo)ToLua.ToObject(L, 6);
			System.Globalization.CompareOptions arg6 = (System.Globalization.CompareOptions)ToLua.ToObject(L, 7);
			int o;

			try
			{
				o = System.String.Compare(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 7 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int), typeof(string), typeof(int), typeof(int), typeof(bool), typeof(System.Globalization.CultureInfo)))
		{
			string arg0 = ToLua.ToString(L, 1);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
			string arg2 = ToLua.ToString(L, 3);
			int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
			int arg4 = (int)LuaDLL.lua_tonumber(L, 5);
			bool arg5 = LuaDLL.lua_toboolean(L, 6);
			System.Globalization.CultureInfo arg6 = (System.Globalization.CultureInfo)ToLua.ToObject(L, 7);
			int o;

			try
			{
				o = System.String.Compare(arg0, arg1, arg2, arg3, arg4, arg5, arg6);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.Compare");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CompareTo(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
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
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(object)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
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
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.CompareTo");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CompareOrdinal(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string)))
		{
			string arg0 = ToLua.ToString(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			int o;

			try
			{
				o = System.String.CompareOrdinal(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 5 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int), typeof(string), typeof(int), typeof(int)))
		{
			string arg0 = ToLua.ToString(L, 1);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
			string arg2 = ToLua.ToString(L, 3);
			int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
			int arg4 = (int)LuaDLL.lua_tonumber(L, 5);
			int o;

			try
			{
				o = System.String.CompareOrdinal(arg0, arg1, arg2, arg3, arg4);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.CompareOrdinal");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int EndsWith(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			bool o;

			try
			{
				o = obj.EndsWith(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(System.StringComparison)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.StringComparison arg1 = (System.StringComparison)ToLua.ToObject(L, 3);
			bool o;

			try
			{
				o = obj.EndsWith(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(bool), typeof(System.Globalization.CultureInfo)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			bool arg1 = LuaDLL.lua_toboolean(L, 3);
			System.Globalization.CultureInfo arg2 = (System.Globalization.CultureInfo)ToLua.ToObject(L, 4);
			bool o;

			try
			{
				o = obj.EndsWith(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.EndsWith");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IndexOfAny(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char[])))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char[] arg0 = ToLua.CheckCharBuffer(L, 2);
			int o;

			try
			{
				o = obj.IndexOfAny(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char[]), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char[] arg0 = ToLua.CheckCharBuffer(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int o;

			try
			{
				o = obj.IndexOfAny(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char[]), typeof(int), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char[] arg0 = ToLua.CheckCharBuffer(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
			int o;

			try
			{
				o = obj.IndexOfAny(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.IndexOfAny");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IndexOf(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char arg0 = (char)LuaDLL.lua_tonumber(L, 2);
			int o;

			try
			{
				o = obj.IndexOf(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			int o;

			try
			{
				o = obj.IndexOf(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int o;

			try
			{
				o = obj.IndexOf(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char arg0 = (char)LuaDLL.lua_tonumber(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int o;

			try
			{
				o = obj.IndexOf(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(System.StringComparison)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.StringComparison arg1 = (System.StringComparison)ToLua.ToObject(L, 3);
			int o;

			try
			{
				o = obj.IndexOf(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(int), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
			int o;

			try
			{
				o = obj.IndexOf(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(int), typeof(System.StringComparison)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			System.StringComparison arg2 = (System.StringComparison)ToLua.ToObject(L, 4);
			int o;

			try
			{
				o = obj.IndexOf(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char), typeof(int), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char arg0 = (char)LuaDLL.lua_tonumber(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
			int o;

			try
			{
				o = obj.IndexOf(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 5 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(int), typeof(int), typeof(System.StringComparison)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
			System.StringComparison arg3 = (System.StringComparison)ToLua.ToObject(L, 5);
			int o;

			try
			{
				o = obj.IndexOf(arg0, arg1, arg2, arg3);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.IndexOf");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LastIndexOf(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char arg0 = (char)LuaDLL.lua_tonumber(L, 2);
			int o;

			try
			{
				o = obj.LastIndexOf(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			int o;

			try
			{
				o = obj.LastIndexOf(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int o;

			try
			{
				o = obj.LastIndexOf(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char arg0 = (char)LuaDLL.lua_tonumber(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int o;

			try
			{
				o = obj.LastIndexOf(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(System.StringComparison)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.StringComparison arg1 = (System.StringComparison)ToLua.ToObject(L, 3);
			int o;

			try
			{
				o = obj.LastIndexOf(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(int), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
			int o;

			try
			{
				o = obj.LastIndexOf(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(int), typeof(System.StringComparison)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			System.StringComparison arg2 = (System.StringComparison)ToLua.ToObject(L, 4);
			int o;

			try
			{
				o = obj.LastIndexOf(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char), typeof(int), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char arg0 = (char)LuaDLL.lua_tonumber(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
			int o;

			try
			{
				o = obj.LastIndexOf(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 5 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(int), typeof(int), typeof(System.StringComparison)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
			System.StringComparison arg3 = (System.StringComparison)ToLua.ToObject(L, 5);
			int o;

			try
			{
				o = obj.LastIndexOf(arg0, arg1, arg2, arg3);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.LastIndexOf");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LastIndexOfAny(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char[])))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char[] arg0 = ToLua.CheckCharBuffer(L, 2);
			int o;

			try
			{
				o = obj.LastIndexOfAny(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char[]), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char[] arg0 = ToLua.CheckCharBuffer(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int o;

			try
			{
				o = obj.LastIndexOfAny(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char[]), typeof(int), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char[] arg0 = ToLua.CheckCharBuffer(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
			int o;

			try
			{
				o = obj.LastIndexOfAny(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.LastIndexOfAny");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Contains(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.String obj = (System.String)ToLua.CheckObject(L, 1, typeof(System.String));
		string arg0 = ToLua.CheckString(L, 2);
		bool o;

		try
		{
			o = obj.Contains(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushboolean(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsNullOrEmpty(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		string arg0 = ToLua.CheckString(L, 1);
		bool o;

		try
		{
			o = System.String.IsNullOrEmpty(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushboolean(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Normalize(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string o = null;

			try
			{
				o = obj.Normalize();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(System.Text.NormalizationForm)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			System.Text.NormalizationForm arg0 = (System.Text.NormalizationForm)ToLua.ToObject(L, 2);
			string o = null;

			try
			{
				o = obj.Normalize(arg0);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.Normalize");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsNormalized(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			bool o;

			try
			{
				o = obj.IsNormalized();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(System.Text.NormalizationForm)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			System.Text.NormalizationForm arg0 = (System.Text.NormalizationForm)ToLua.ToObject(L, 2);
			bool o;

			try
			{
				o = obj.IsNormalized(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.IsNormalized");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Remove(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
			string o = null;

			try
			{
				o = obj.Remove(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
			int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
			string o = null;

			try
			{
				o = obj.Remove(arg0, arg1);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.Remove");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PadLeft(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
			string o = null;

			try
			{
				o = obj.PadLeft(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int), typeof(char)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
			char arg1 = (char)LuaDLL.lua_tonumber(L, 3);
			string o = null;

			try
			{
				o = obj.PadLeft(arg0, arg1);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.PadLeft");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int PadRight(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
			string o = null;

			try
			{
				o = obj.PadRight(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(int), typeof(char)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
			char arg1 = (char)LuaDLL.lua_tonumber(L, 3);
			string o = null;

			try
			{
				o = obj.PadRight(arg0, arg1);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.PadRight");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int StartsWith(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			bool o;

			try
			{
				o = obj.StartsWith(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(System.StringComparison)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.StringComparison arg1 = (System.StringComparison)ToLua.ToObject(L, 3);
			bool o;

			try
			{
				o = obj.StartsWith(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(bool), typeof(System.Globalization.CultureInfo)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			bool arg1 = LuaDLL.lua_toboolean(L, 3);
			System.Globalization.CultureInfo arg2 = (System.Globalization.CultureInfo)ToLua.ToObject(L, 4);
			bool o;

			try
			{
				o = obj.StartsWith(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.StartsWith");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Replace(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			string arg1 = ToLua.ToString(L, 3);
			string o = null;

			try
			{
				o = obj.Replace(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(char), typeof(char)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			char arg0 = (char)LuaDLL.lua_tonumber(L, 2);
			char arg1 = (char)LuaDLL.lua_tonumber(L, 3);
			string o = null;

			try
			{
				o = obj.Replace(arg0, arg1);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.Replace");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToLower(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string o = null;

			try
			{
				o = obj.ToLower();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(System.Globalization.CultureInfo)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			System.Globalization.CultureInfo arg0 = (System.Globalization.CultureInfo)ToLua.ToObject(L, 2);
			string o = null;

			try
			{
				o = obj.ToLower(arg0);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.ToLower");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToLowerInvariant(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.String obj = (System.String)ToLua.CheckObject(L, 1, typeof(System.String));
		string o = null;

		try
		{
			o = obj.ToLowerInvariant();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushstring(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToUpper(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			string o = null;

			try
			{
				o = obj.ToUpper();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(System.Globalization.CultureInfo)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			System.Globalization.CultureInfo arg0 = (System.Globalization.CultureInfo)ToLua.ToObject(L, 2);
			string o = null;

			try
			{
				o = obj.ToUpper(arg0);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.ToUpper");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToUpperInvariant(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.String obj = (System.String)ToLua.CheckObject(L, 1, typeof(System.String));
		string o = null;

		try
		{
			o = obj.ToUpperInvariant();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushstring(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToString(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(string)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
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
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(System.IFormatProvider)))
		{
			System.String obj = (System.String)ToLua.ToObject(L, 1);
			System.IFormatProvider arg0 = (System.IFormatProvider)ToLua.ToObject(L, 2);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.ToString");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Format(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(object)))
		{
			string arg0 = ToLua.ToString(L, 1);
			object arg1 = ToLua.ToVarObject(L, 2);
			string o = null;

			try
			{
				o = System.String.Format(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(object), typeof(object)))
		{
			string arg0 = ToLua.ToString(L, 1);
			object arg1 = ToLua.ToVarObject(L, 2);
			object arg2 = ToLua.ToVarObject(L, 3);
			string o = null;

			try
			{
				o = System.String.Format(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(object), typeof(object), typeof(object)))
		{
			string arg0 = ToLua.ToString(L, 1);
			object arg1 = ToLua.ToVarObject(L, 2);
			object arg2 = ToLua.ToVarObject(L, 3);
			object arg3 = ToLua.ToVarObject(L, 4);
			string o = null;

			try
			{
				o = System.String.Format(arg0, arg1, arg2, arg3);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (ToLua.CheckTypes(L, 1, typeof(System.IFormatProvider), typeof(string)) && ToLua.CheckParamsType(L, typeof(object), 3, count - 2))
		{
			System.IFormatProvider arg0 = (System.IFormatProvider)ToLua.ToObject(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			object[] arg2 = ToLua.ToParamsObject(L, 3, count - 2);
			string o = null;

			try
			{
				o = System.String.Format(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (ToLua.CheckTypes(L, 1, typeof(string)) && ToLua.CheckParamsType(L, typeof(object), 2, count - 1))
		{
			string arg0 = ToLua.ToString(L, 1);
			object[] arg1 = ToLua.ToParamsObject(L, 2, count - 1);
			string o = null;

			try
			{
				o = System.String.Format(arg0, arg1);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.Format");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Copy(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		string arg0 = ToLua.CheckString(L, 1);
		string o = null;

		try
		{
			o = System.String.Copy(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushstring(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Concat(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(object)))
		{
			object arg0 = ToLua.ToVarObject(L, 1);
			string o = null;

			try
			{
				o = System.String.Concat(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string)))
		{
			string arg0 = ToLua.ToString(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			string o = null;

			try
			{
				o = System.String.Concat(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(object), typeof(object)))
		{
			object arg0 = ToLua.ToVarObject(L, 1);
			object arg1 = ToLua.ToVarObject(L, 2);
			string o = null;

			try
			{
				o = System.String.Concat(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(string)))
		{
			string arg0 = ToLua.ToString(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			string arg2 = ToLua.ToString(L, 3);
			string o = null;

			try
			{
				o = System.String.Concat(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(object), typeof(object), typeof(object)))
		{
			object arg0 = ToLua.ToVarObject(L, 1);
			object arg1 = ToLua.ToVarObject(L, 2);
			object arg2 = ToLua.ToVarObject(L, 3);
			string o = null;

			try
			{
				o = System.String.Concat(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(string), typeof(string)))
		{
			string arg0 = ToLua.ToString(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			string arg2 = ToLua.ToString(L, 3);
			string arg3 = ToLua.ToString(L, 4);
			string o = null;

			try
			{
				o = System.String.Concat(arg0, arg1, arg2, arg3);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(object), typeof(object), typeof(object), typeof(object)))
		{
			object arg0 = ToLua.ToVarObject(L, 1);
			object arg1 = ToLua.ToVarObject(L, 2);
			object arg2 = ToLua.ToVarObject(L, 3);
			object arg3 = ToLua.ToVarObject(L, 4);
			string o = null;

			try
			{
				o = System.String.Concat(arg0, arg1, arg2, arg3);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (ToLua.CheckParamsType(L, typeof(string), 1, count))
		{
			string[] arg0 = ToLua.ToParamsString(L, 1, count);
			string o = null;

			try
			{
				o = System.String.Concat(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (ToLua.CheckParamsType(L, typeof(object), 1, count))
		{
			object[] arg0 = ToLua.ToParamsObject(L, 1, count);
			string o = null;

			try
			{
				o = System.String.Concat(arg0);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.Concat");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Insert(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 3);
		System.String obj = (System.String)ToLua.CheckObject(L, 1, typeof(System.String));
		int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
		string arg1 = ToLua.CheckString(L, 3);
		string o = null;

		try
		{
			o = obj.Insert(arg0, arg1);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushstring(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Intern(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		string arg0 = ToLua.CheckString(L, 1);
		string o = null;

		try
		{
			o = System.String.Intern(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushstring(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsInterned(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		string arg0 = ToLua.CheckString(L, 1);
		string o = null;

		try
		{
			o = System.String.IsInterned(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushstring(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Join(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string[])))
		{
			string arg0 = ToLua.ToString(L, 1);
			string[] arg1 = ToLua.CheckStringArray(L, 2);
			string o = null;

			try
			{
				o = System.String.Join(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string[]), typeof(int), typeof(int)))
		{
			string arg0 = ToLua.ToString(L, 1);
			string[] arg1 = ToLua.CheckStringArray(L, 2);
			int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
			int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
			string o = null;

			try
			{
				o = System.String.Join(arg0, arg1, arg2, arg3);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: System.String.Join");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetEnumerator(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.String obj = (System.String)ToLua.CheckObject(L, 1, typeof(System.String));
		System.CharEnumerator o = null;

		try
		{
			o = obj.GetEnumerator();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetHashCode(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.String obj = (System.String)ToLua.CheckObject(L, 1, typeof(System.String));
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
	static int op_Equality(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		string arg0 = ToLua.ToString(L, 1);
		string arg1 = ToLua.ToString(L, 2);
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
	static int get_Empty(IntPtr L)
	{
		LuaDLL.lua_pushstring(L, System.String.Empty);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Length(IntPtr L)
	{
		System.String obj = (System.String)ToLua.ToObject(L, 1);
		int ret;

		try
		{
			ret = obj.Length;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index Length on a nil value" : e.Message);
		}

		LuaDLL.lua_pushinteger(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_out(IntPtr L)
	{
		ToLua.PushOut<System.String>(L, new LuaOut<System.String>());
		return 1;
	}
}

