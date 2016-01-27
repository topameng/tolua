using System;
using LuaInterface;

public class TestEventListenerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(TestEventListener), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("SetOnFinished", SetOnFinished);
		L.RegFunction("New", _CreateTestEventListener);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", Lua_ToString);
		L.RegVar("onClick", get_onClick, set_onClick);
		L.RegVar("onClickEvent", get_onClickEvent, set_onClickEvent);
		L.RegFunction("VoidDelegate", VoidDelegate);
		L.RegFunction("OnClick", OnClick);
		L.RegVar("out", get_out, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateTestEventListener(IntPtr L)
	{
		return LuaDLL.luaL_error(L, "TestEventListener class does not have a constructor function");
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetOnFinished(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(TestEventListener), typeof(TestEventListener.VoidDelegate)))
		{
			TestEventListener obj = (TestEventListener)ToLua.ToObject(L, 1);
			TestEventListener.VoidDelegate arg0 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg0 = (TestEventListener.VoidDelegate)ToLua.ToObject(L, 2);
			}
			else
			{
				LuaFunction func = ToLua.ToLuaFunction(L, 2);
				arg0 = DelegateFactory.CreateDelegate(L, typeof(TestEventListener.VoidDelegate), func) as TestEventListener.VoidDelegate;
			}

			try
			{
				obj.SetOnFinished(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.toluaL_exception(L, e);
			}

			return 0;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(TestEventListener), typeof(TestEventListener.OnClick)))
		{
			TestEventListener obj = (TestEventListener)ToLua.ToObject(L, 1);
			TestEventListener.OnClick arg0 = null;
			LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

			if (funcType2 != LuaTypes.LUA_TFUNCTION)
			{
				 arg0 = (TestEventListener.OnClick)ToLua.ToObject(L, 2);
			}
			else
			{
				LuaFunction func = ToLua.ToLuaFunction(L, 2);
				arg0 = DelegateFactory.CreateDelegate(L, typeof(TestEventListener.OnClick), func) as TestEventListener.OnClick;
			}

			try
			{
				obj.SetOnFinished(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.toluaL_exception(L, e);
			}

			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: TestEventListener.SetOnFinished");
		}

		return 0;
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
			return LuaDLL.toluaL_exception(L, e);
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
	static int get_onClick(IntPtr L)
	{
		TestEventListener obj = (TestEventListener)ToLua.ToObject(L, 1);
		TestEventListener.OnClick ret = null;

		try
		{
			ret = obj.onClick;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index onClick on a nil value" : e.Message);
		}

		ToLua.Push(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_onClickEvent(IntPtr L)
	{
		ToLua.Push(L, new EventObject("TestEventListener.onClickEvent"));
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onClick(IntPtr L)
	{
		TestEventListener obj = (TestEventListener)ToLua.ToObject(L, 1);
		TestEventListener.OnClick arg0 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg0 = (TestEventListener.OnClick)ToLua.CheckObject(L, 2, typeof(TestEventListener.OnClick));
		}
		else
		{
			LuaFunction func = ToLua.ToLuaFunction(L, 2);
			arg0 = DelegateFactory.CreateDelegate(L, typeof(TestEventListener.OnClick), func) as TestEventListener.OnClick;
		}

		try
		{
			obj.onClick = arg0;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index onClick on a nil value" : e.Message);
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_onClickEvent(IntPtr L)
	{
		TestEventListener obj = (TestEventListener)ToLua.CheckObject(L, 1, typeof(TestEventListener));
		EventObject arg0 = null;

		if (LuaDLL.lua_isuserdata(L, 2) != 0)
		{
			arg0 = (EventObject)ToLua.ToObject(L, 2);
		}
		else
		{
			return LuaDLL.luaL_error(L, "The event 'TestEventListener.onClickEvent' can only appear on the left hand side of += or -= when used outside of the type 'TestEventListener'");
		}

		if (arg0.op == EventOp.Add)
		{
			TestEventListener.OnClick ev = (TestEventListener.OnClick)DelegateFactory.CreateDelegate(L, typeof(TestEventListener.OnClick), arg0.func);
			obj.onClickEvent += ev;
		}
		else if (arg0.op == EventOp.Sub)
		{
			TestEventListener.OnClick ev = (TestEventListener.OnClick)LuaMisc.GetEventHandler(obj, typeof(TestEventListener), "onClickEvent");
			Delegate[] ds = ev.GetInvocationList();

			for (int i = 0; i < ds.Length; i++)
			{
				ev = (TestEventListener.OnClick)ds[i];
				LuaDelegate ld = ev.Target as LuaDelegate;

				if (ld != null && ld.func == arg0.func)
				{
					obj.onClickEvent -= ev;
					ld.func.Dispose();
					break;
				}
			}

			arg0.func.Dispose();
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_out(IntPtr L)
	{
		ToLua.PushOut<TestEventListener>(L, new LuaOut<TestEventListener>());
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int VoidDelegate(IntPtr L)
	{
		LuaFunction func = ToLua.CheckLuaFunction(L, 1);
		Delegate arg1 = DelegateFactory.CreateDelegate(L, typeof(TestEventListener.VoidDelegate), func);
		ToLua.Push(L, arg1);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OnClick(IntPtr L)
	{
		LuaFunction func = ToLua.CheckLuaFunction(L, 1);
		Delegate arg1 = DelegateFactory.CreateDelegate(L, typeof(TestEventListener.OnClick), func);
		ToLua.Push(L, arg1);
		return 1;
	}
}

