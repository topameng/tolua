using System;
using LuaInterface;

public class UnityEngine_ObjectWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.Object), typeof(System.Object));
		L.RegFunction("Equals", Equals);
		L.RegFunction("GetHashCode", GetHashCode);
		L.RegFunction("GetInstanceID", GetInstanceID);
		L.RegFunction("Instantiate", Instantiate);
		L.RegFunction("FindObjectsOfType", FindObjectsOfType);
		L.RegFunction("FindObjectOfType", FindObjectOfType);
		L.RegFunction("DontDestroyOnLoad", DontDestroyOnLoad);
		L.RegFunction("ToString", ToString);
		L.RegFunction("DestroyImmediate", DestroyImmediate);
		L.RegFunction("Destroy", Destroy);
		L.RegFunction("New", _CreateUnityEngine_Object);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", Lua_ToString);
		L.RegVar("name", get_name, set_name);
		L.RegVar("hideFlags", get_hideFlags, set_hideFlags);
		L.RegVar("out", get_out, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateUnityEngine_Object(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			UnityEngine.Object obj = new UnityEngine.Object();
			ToLua.Push(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: UnityEngine.Object.New");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Equals(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		UnityEngine.Object obj = (UnityEngine.Object)ToLua.CheckObject(L, 1, typeof(UnityEngine.Object));
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
		UnityEngine.Object obj = (UnityEngine.Object)ToLua.CheckObject(L, 1, typeof(UnityEngine.Object));
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
	static int GetInstanceID(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		UnityEngine.Object obj = (UnityEngine.Object)ToLua.CheckObject(L, 1, typeof(UnityEngine.Object));
		int o;

		try
		{
			o = obj.GetInstanceID();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushinteger(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Instantiate(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(UnityEngine.Object)))
		{
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object o = null;

			try
			{
				o = UnityEngine.Object.Instantiate(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(UnityEngine.Object), typeof(UnityEngine.Vector3), typeof(UnityEngine.Quaternion)))
		{
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Vector3 arg1 = ToLua.ToVector3(L, 2);
			UnityEngine.Quaternion arg2 = ToLua.ToQuaternion(L, 3);
			UnityEngine.Object o = null;

			try
			{
				o = UnityEngine.Object.Instantiate(arg0, arg1, arg2);
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
			LuaDLL.luaL_error(L, "invalid arguments to method: UnityEngine.Object.Instantiate");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindObjectsOfType(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type arg0 = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		UnityEngine.Object[] o = null;

		try
		{
			o = UnityEngine.Object.FindObjectsOfType(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindObjectOfType(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type arg0 = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		UnityEngine.Object o = null;

		try
		{
			o = UnityEngine.Object.FindObjectOfType(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DontDestroyOnLoad(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.CheckUnityObject(L, 1, typeof(UnityEngine.Object));

		try
		{
			UnityEngine.Object.DontDestroyOnLoad(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToString(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		UnityEngine.Object obj = (UnityEngine.Object)ToLua.CheckObject(L, 1, typeof(UnityEngine.Object));
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
	static int op_Equality(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
		UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
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
	static int DestroyImmediate(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.CheckObject(L, 1);
			ToLua.Destroy(L);
			UnityEngine.Object.DestroyImmediate(arg0);
			return 0;
		}
		else if (count == 2)
		{
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.CheckObject(L, 1);
			bool arg1 = LuaDLL.luaL_checkboolean(L, 2);
			ToLua.Destroy(L);
			UnityEngine.Object.DestroyImmediate(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Object.DestroyImmediate");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Destroy(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.CheckObject(L, 1);
            ToLua.Destroy(L);
            UnityEngine.Object.Destroy(arg0);				
			return 0;
		}
		else if (count == 2)
		{
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.CheckObject(L, 1);            
            float arg1 = (float)LuaDLL.luaL_checknumber(L, 2);
            int udata = LuaDLL.tolua_rawnetobj(L, 1);
            ObjectTranslator translator = LuaState.GetTranslator(L);

            Action Call = () =>
            {
                translator.Destroy(udata);                
                UnityEngine.Object.Destroy(arg0);
            };

            translator.DelayDestroy(Call, arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: Object.Destroy");
		}

		return 0;
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
	static int get_name(IntPtr L)
	{
		UnityEngine.Object obj = (UnityEngine.Object)ToLua.ToObject(L, 1);
		string ret = null;

		try
		{
			ret = obj.name;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index name on a nil value" : e.Message);
		}

		LuaDLL.lua_pushstring(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_hideFlags(IntPtr L)
	{
		UnityEngine.Object obj = (UnityEngine.Object)ToLua.ToObject(L, 1);
		UnityEngine.HideFlags ret;

		try
		{
			ret = obj.hideFlags;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index hideFlags on a nil value" : e.Message);
		}

		ToLua.Push(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_name(IntPtr L)
	{
		UnityEngine.Object obj = (UnityEngine.Object)ToLua.ToObject(L, 1);
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
	static int set_hideFlags(IntPtr L)
	{
		UnityEngine.Object obj = (UnityEngine.Object)ToLua.ToObject(L, 1);
		UnityEngine.HideFlags arg0 = (UnityEngine.HideFlags)ToLua.CheckObject(L, 2, typeof(UnityEngine.HideFlags));

		try
		{
			obj.hideFlags = arg0;
		}
		catch(Exception e)
		{
			if (obj == null)
			{
				LuaDLL.luaL_error(L, "attempt to index hideFlags on a nil value");
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
	static int get_out(IntPtr L)
	{
		ToLua.PushOut<UnityEngine.Object>(L, new LuaOut<UnityEngine.Object>());
		return 1;
	}
}

