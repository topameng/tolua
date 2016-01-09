using System;
using LuaInterface;

public class System_Collections_Generic_Dictionary_int_TestAccountWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(System.Collections.Generic.Dictionary<int,TestAccount>), typeof(System.Object), "Dictionary_int_TestAccount");
		L.RegFunction("get_Item", get_Item);
		L.RegFunction("set_Item", set_Item);
		L.RegFunction("Add", Add);
		L.RegFunction("Clear", Clear);
		L.RegFunction("ContainsKey", ContainsKey);
		L.RegFunction("ContainsValue", ContainsValue);
		L.RegFunction("GetObjectData", GetObjectData);
		L.RegFunction("OnDeserialization", OnDeserialization);
		L.RegFunction("Remove", Remove);
		L.RegFunction("TryGetValue", TryGetValue);
		L.RegFunction("GetEnumerator", GetEnumerator);
		L.RegFunction("New", _CreateSystem_Collections_Generic_Dictionary_int_TestAccount);		
		L.RegVar("Count", get_Count, null);
		L.RegVar("Comparer", get_Comparer, null);
		L.RegVar("Keys", get_Keys, null);
		L.RegVar("Values", get_Values, null);
		L.RegVar("out", get_out, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSystem_Collections_Generic_Dictionary_int_TestAccount(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 0)
		{
			System.Collections.Generic.Dictionary<int,TestAccount> obj = new System.Collections.Generic.Dictionary<int,TestAccount>();
			ToLua.PushObject(L, obj);
			return 1;
		}
		else if (count == 1 && ToLua.CheckTypes(L, 1, typeof(int)))
		{
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
			System.Collections.Generic.Dictionary<int,TestAccount> obj = new System.Collections.Generic.Dictionary<int,TestAccount>(arg0);
			ToLua.PushObject(L, obj);
			return 1;
		}
		else if (count == 1 && ToLua.CheckTypes(L, 1, typeof(System.Collections.Generic.IDictionary<int,TestAccount>)))
		{
			System.Collections.Generic.IDictionary<int,TestAccount> arg0 = (System.Collections.Generic.IDictionary<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.IDictionary<int,TestAccount>));
			System.Collections.Generic.Dictionary<int,TestAccount> obj = new System.Collections.Generic.Dictionary<int,TestAccount>(arg0);
			ToLua.PushObject(L, obj);
			return 1;
		}
		else if (count == 1 && ToLua.CheckTypes(L, 1, typeof(System.Collections.Generic.IEqualityComparer<int>)))
		{
			System.Collections.Generic.IEqualityComparer<int> arg0 = (System.Collections.Generic.IEqualityComparer<int>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.IEqualityComparer<int>));
			System.Collections.Generic.Dictionary<int,TestAccount> obj = new System.Collections.Generic.Dictionary<int,TestAccount>(arg0);
			ToLua.PushObject(L, obj);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(int), typeof(System.Collections.Generic.IEqualityComparer<int>)))
		{
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
			System.Collections.Generic.IEqualityComparer<int> arg1 = (System.Collections.Generic.IEqualityComparer<int>)ToLua.CheckObject(L, 2, typeof(System.Collections.Generic.IEqualityComparer<int>));
			System.Collections.Generic.Dictionary<int,TestAccount> obj = new System.Collections.Generic.Dictionary<int,TestAccount>(arg0, arg1);
			ToLua.PushObject(L, obj);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Collections.Generic.IDictionary<int,TestAccount>), typeof(System.Collections.Generic.IEqualityComparer<int>)))
		{
			System.Collections.Generic.IDictionary<int,TestAccount> arg0 = (System.Collections.Generic.IDictionary<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.IDictionary<int,TestAccount>));
			System.Collections.Generic.IEqualityComparer<int> arg1 = (System.Collections.Generic.IEqualityComparer<int>)ToLua.CheckObject(L, 2, typeof(System.Collections.Generic.IEqualityComparer<int>));
			System.Collections.Generic.Dictionary<int,TestAccount> obj = new System.Collections.Generic.Dictionary<int,TestAccount>(arg0, arg1);
			ToLua.PushObject(L, obj);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Collections.Generic.Dictionary<int,TestAccount>.New");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Item(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<int,TestAccount>));
		int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
		TestAccount o = null;

		try
		{
			o = obj[arg0];
		}
		catch(Exception e)
		{
			LuaDLL.luaL_error(L, e.Message);
			return 0;
		}

		ToLua.PushObject(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Item(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 3);
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<int,TestAccount>));
		int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
		TestAccount arg1 = (TestAccount)ToLua.CheckObject(L, 3, typeof(TestAccount));

		try
		{
			obj[arg0] = arg1;
		}
		catch(Exception e)
		{
			LuaDLL.luaL_error(L, e.Message);
			return 0;
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Add(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 3);
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<int,TestAccount>));
		int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
		TestAccount arg1 = (TestAccount)ToLua.CheckObject(L, 3, typeof(TestAccount));

		try
		{
			obj.Add(arg0, arg1);
		}
		catch(Exception e)
		{
			LuaDLL.luaL_error(L, e.Message);
			return 0;
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Clear(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<int,TestAccount>));

		try
		{
			obj.Clear();
		}
		catch(Exception e)
		{
			LuaDLL.luaL_error(L, e.Message);
			return 0;
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ContainsKey(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<int,TestAccount>));
		int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
		bool o;

		try
		{
			o = obj.ContainsKey(arg0);
		}
		catch(Exception e)
		{
			LuaDLL.luaL_error(L, e.Message);
			return 0;
		}

		LuaDLL.lua_pushboolean(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ContainsValue(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<int,TestAccount>));
		TestAccount arg0 = (TestAccount)ToLua.CheckObject(L, 2, typeof(TestAccount));
		bool o;

		try
		{
			o = obj.ContainsValue(arg0);
		}
		catch(Exception e)
		{
			LuaDLL.luaL_error(L, e.Message);
			return 0;
		}

		LuaDLL.lua_pushboolean(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetObjectData(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 3);
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<int,TestAccount>));
		System.Runtime.Serialization.SerializationInfo arg0 = (System.Runtime.Serialization.SerializationInfo)ToLua.CheckObject(L, 2, typeof(System.Runtime.Serialization.SerializationInfo));
		System.Runtime.Serialization.StreamingContext arg1 = (System.Runtime.Serialization.StreamingContext)ToLua.CheckObject(L, 3, typeof(System.Runtime.Serialization.StreamingContext));

		try
		{
			obj.GetObjectData(arg0, arg1);
		}
		catch(Exception e)
		{
			LuaDLL.luaL_error(L, e.Message);
			return 0;
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnDeserialization(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<int,TestAccount>));
		object arg0 = ToLua.ToVarObject(L, 2);

		try
		{
			obj.OnDeserialization(arg0);
		}
		catch(Exception e)
		{
			LuaDLL.luaL_error(L, e.Message);
			return 0;
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Remove(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<int,TestAccount>));
		int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
		bool o;

		try
		{
			o = obj.Remove(arg0);
		}
		catch(Exception e)
		{
			LuaDLL.luaL_error(L, e.Message);
			return 0;
		}

		LuaDLL.lua_pushboolean(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TryGetValue(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 3);
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<int,TestAccount>));
		int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
		TestAccount arg1 = null;
		bool o;

		try
		{
			o = obj.TryGetValue(arg0, out arg1);
		}
		catch(Exception e)
		{
			LuaDLL.luaL_error(L, e.Message);
			return 0;
		}

		LuaDLL.lua_pushboolean(L, o);
		ToLua.PushObject(L, arg1);
		return 2;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetEnumerator(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.CheckObject(L, 1, typeof(System.Collections.Generic.Dictionary<int,TestAccount>));
		System.Collections.Generic.Dictionary<int,TestAccount>.Enumerator o;

		try
		{
			o = obj.GetEnumerator();
		}
		catch(Exception e)
		{
			LuaDLL.luaL_error(L, e.Message);
			return 0;
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Count(IntPtr L)
	{
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.ToObject(L, 1);
		int ret;

		try
		{
			ret = obj.Count;
		}
		catch(Exception e)
		{
			if (obj == null)
			{
				LuaDLL.luaL_error(L, "attempt to index Count on a nil value");
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
	static int get_Comparer(IntPtr L)
	{
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.ToObject(L, 1);
		System.Collections.Generic.IEqualityComparer<int> ret = null;

		try
		{
			ret = obj.Comparer;
		}
		catch(Exception e)
		{
			if (obj == null)
			{
				LuaDLL.luaL_error(L, "attempt to index Comparer on a nil value");
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
	static int get_Keys(IntPtr L)
	{
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.ToObject(L, 1);
		System.Collections.Generic.Dictionary<int,TestAccount>.KeyCollection ret = null;

		try
		{
			ret = obj.Keys;
		}
		catch(Exception e)
		{
			if (obj == null)
			{
				LuaDLL.luaL_error(L, "attempt to index Keys on a nil value");
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
	static int get_Values(IntPtr L)
	{
		System.Collections.Generic.Dictionary<int,TestAccount> obj = (System.Collections.Generic.Dictionary<int,TestAccount>)ToLua.ToObject(L, 1);
		System.Collections.Generic.Dictionary<int,TestAccount>.ValueCollection ret = null;

		try
		{
			ret = obj.Values;
		}
		catch(Exception e)
		{
			if (obj == null)
			{
				LuaDLL.luaL_error(L, "attempt to index Values on a nil value");
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
		ToLua.PushOut<System.Collections.Generic.Dictionary<int,TestAccount>>(L, new LuaOut<System.Collections.Generic.Dictionary<int,TestAccount>>());
		return 1;
	}
}

