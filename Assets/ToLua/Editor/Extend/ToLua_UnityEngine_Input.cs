using UnityEngine;
using LuaInterface;

public class ToLua_UnityEngine_Input
{
    public static string GetTouchDefined =
@"		int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
        int arg1 = LuaDLL.luaL_optinteger(L, 2, TouchBits.ALL);        
		UnityEngine.Touch o;

		try
		{
			o = UnityEngine.Input.GetTouch(arg0);
		}
		catch(Exception e)
		{
			LuaDLL.luaL_error(L, e.Message);
			return 0;
		}

		ToLua.Push(L, o, arg1);
		return 1;";

    [UseDefinedAttribute]
    public static Touch GetTouch(int index, int flag)
    {        
        return new Touch();
    }
}
