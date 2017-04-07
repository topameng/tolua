using System;
using LuaInterface;

public class ToLua_System_Enum
{
    public static string ToIntDefined =
@"		try
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

    public static string ParseDefined =
@"		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Type), typeof(string)))
			{
				System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				object o = System.Enum.Parse(arg0, arg1);
				ToLua.Push(L, (Enum)o);
				return 1;
			}
			else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(bool)))
			{
				System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
				string arg1 = ToLua.ToString(L, 2);
				bool arg2 = LuaDLL.lua_toboolean(L, 3);
				object o = System.Enum.Parse(arg0, arg1, arg2);
				ToLua.Push(L, (Enum)o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, ""invalid arguments to method: System.Enum.Parse"");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}";

    public static string ToObjectDefined =
@"		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Type), typeof(int)))
			{
				System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
				int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
				object o = System.Enum.ToObject(arg0, arg1);
				ToLua.Push(L, (Enum)o);
				return 1;
			}
			else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Type), typeof(object)))
			{
				System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
				object arg1 = ToLua.ToVarObject(L, 2);
				object o = System.Enum.ToObject(arg0, arg1);
				ToLua.Push(L, (Enum)o);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, ""invalid arguments to method: System.Enum.ToObject"");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}";

    [UseDefinedAttribute]
    public static void ToInt(System.Enum obj)
    {
    }

    [UseDefinedAttribute]
    public static object ToObject(Type enumType, int value)
    {
        return null;
    }

    [UseDefinedAttribute]
    public static object Parse(Type enumType, string value)
    {
        return null;
    }
}
