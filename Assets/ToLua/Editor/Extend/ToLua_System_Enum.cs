using System;
using LuaInterface;

public struct ToLua_System_Enum
{
    public static string ToIntDefined =
    @"		object arg0 = ToLua.CheckObject(L, 1, typeof(System.Enum));
		int ret = Convert.ToInt32(arg0);        
		LuaDLL.lua_pushinteger(L, ret);
		return 1;";

    [UseDefinedAttribute]
    public static void ToInt(System.Enum obj)
    {
    }
}
