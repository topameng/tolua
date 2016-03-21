using System;
using LuaInterface;

public class ToLua_System_Enum
{
    public static string ToIntDefined =
@"        try
        {
            object arg0 = ToLua.CheckObject(L, 1, typeof(System.Enum));
            int ret = Convert.ToInt32(arg0);
            LuaDLL.lua_pushinteger(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }";

    [UseDefinedAttribute]
    public static void ToInt(System.Enum obj)
    {
    }
}
