using UnityEngine;
using System.Collections;
using LuaInterface;

public class ToLua_System_String
{
    [NoToLuaAttribute]
    public static string ToLua_System_StringDefined =
@"        LuaTypes luatype = LuaDLL.lua_type(L, 1);

        if (luatype == LuaTypes.LUA_TSTRING)
        {
            string arg0 = LuaDLL.lua_tostring(L, 1);
            ToLua.PushObject(L, arg0);
            return 1;
        }
        else
        {
            LuaDLL.luaL_error(L, ""invalid arguments to method: String.New"");
        }
        
		return 0;";

    [UseDefinedAttribute]
    public ToLua_System_String()
    {

    }
}
