using System;
using LuaInterface;

public class ToLua_LuaInterface_LuaMethod
{
    public static string CallDefined =
@"		try
		{			
			LuaInterface.LuaMethod obj = (LuaInterface.LuaMethod)ToLua.CheckObject(L, 1, typeof(LuaInterface.LuaMethod));            
			return obj.Call(L);						
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}";

    public static string DestroyDefined =
@"		try
		{
			ToLua.CheckArgsCount(L, 1);
			LuaInterface.LuaMethod obj = (LuaInterface.LuaMethod)ToLua.CheckObject(L, 1, typeof(LuaInterface.LuaMethod));
			obj.Destroy();
            ToLua.Destroy(L);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}";

    [UseDefinedAttribute]
    public int Call(IntPtr L)
    {
        return 0;
    }

    [UseDefinedAttribute]
    public void Destroy()
    {

    }
}
