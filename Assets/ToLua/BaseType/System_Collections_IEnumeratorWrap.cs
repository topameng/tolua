using System;
using LuaInterface;

public class System_Collections_IEnumeratorWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(System.Collections.IEnumerator), null);
		L.RegFunction("MoveNext", MoveNext);
		L.RegFunction("Reset", Reset);
		L.RegFunction("New", _CreateSystem_Collections_IEnumerator);
		L.RegVar("Current", get_Current, null);
		L.RegVar("out", get_out, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSystem_Collections_IEnumerator(IntPtr L)
	{
		return LuaDLL.luaL_error(L, "System.Collections.IEnumerator class does not have a constructor function");
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MoveNext(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Collections.IEnumerator obj = (System.Collections.IEnumerator)ToLua.CheckObject(L, 1, typeof(System.Collections.IEnumerator));
		bool o;

		try
		{
			o = obj.MoveNext();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushboolean(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Reset(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Collections.IEnumerator obj = (System.Collections.IEnumerator)ToLua.CheckObject(L, 1, typeof(System.Collections.IEnumerator));

		try
		{
			obj.Reset();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Current(IntPtr L)
	{
		System.Collections.IEnumerator obj = (System.Collections.IEnumerator)ToLua.ToObject(L, 1);
		object ret = null;

		try
		{
			ret = obj.Current;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index Current on a nil value" : e.Message);
		}

		ToLua.Push(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_out(IntPtr L)
	{
		ToLua.PushOut<System.Collections.IEnumerator>(L, new LuaOut<System.Collections.IEnumerator>());
		return 1;
	}
}

