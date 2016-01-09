using System;
using LuaInterface;

public class TestProtolWrap
{
	public static void Register(LuaState L)
	{
		L.BeginStaticLibs("TestProtol");
		L.RegVar("data", get_data, set_data);
		L.EndStaticLibs();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_data(IntPtr L)
	{
		ToLua.Push(L, TestProtol.data);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_data(IntPtr L)
	{
		LuaByteBuffer arg0 = new LuaByteBuffer(ToLua.CheckByteBuffer(L, 2));
		TestProtol.data = arg0;
		return 0;
	}
}

