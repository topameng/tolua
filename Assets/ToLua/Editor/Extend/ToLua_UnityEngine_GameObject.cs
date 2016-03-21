using UnityEngine;
using System.Collections;
using LuaInterface;

public class ToLua_UnityEngine_GameObject
{
    public static string SendMessageDefined =
@"		try
		{
            ++LuaException.SendMsgCount;
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.GameObject), typeof(string)))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				string arg0 = ToLua.ToString(L, 2);
                obj.SendMessage(arg0);
                --LuaException.SendMsgCount;

                if (LuaDLL.lua_toboolean(L, LuaDLL.lua_upvalueindex(1)))
                {
                    string error = LuaDLL.lua_tostring(L, -1);
                    LuaDLL.lua_pop(L, 1);
                    throw new LuaException(error, LuaException.luaStack);
                }
                
                return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.GameObject), typeof(string), typeof(UnityEngine.SendMessageOptions)))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				UnityEngine.SendMessageOptions arg1 = (UnityEngine.SendMessageOptions)ToLua.ToObject(L, 3);
				obj.SendMessage(arg0, arg1);
                --LuaException.SendMsgCount;

                if (LuaDLL.lua_toboolean(L, LuaDLL.lua_upvalueindex(1)))
                {
                    string error = LuaDLL.lua_tostring(L, -1);
                    LuaDLL.lua_pop(L, 1);
                    throw new LuaException(error, LuaException.luaStack);
                }

                return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.GameObject), typeof(string), typeof(object)))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				object arg1 = ToLua.ToVarObject(L, 3);
				obj.SendMessage(arg0, arg1);
                --LuaException.SendMsgCount;

                if (LuaDLL.lua_toboolean(L, LuaDLL.lua_upvalueindex(1)))
                {
                    string error = LuaDLL.lua_tostring(L, -1);
                    LuaDLL.lua_pop(L, 1);
                    throw new LuaException(error, LuaException.luaStack);
                }

                return 0;
			}
			else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.GameObject), typeof(string), typeof(object), typeof(UnityEngine.SendMessageOptions)))
			{
				UnityEngine.GameObject obj = (UnityEngine.GameObject)ToLua.ToObject(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				object arg1 = ToLua.ToVarObject(L, 3);
				UnityEngine.SendMessageOptions arg2 = (UnityEngine.SendMessageOptions)ToLua.ToObject(L, 4);
				obj.SendMessage(arg0, arg1, arg2);
                --LuaException.SendMsgCount;

                if (LuaDLL.lua_toboolean(L, LuaDLL.lua_upvalueindex(1)))
                {
                    string error = LuaDLL.lua_tostring(L, -1);
                    LuaDLL.lua_pop(L, 1);
                    throw new LuaException(error, LuaException.luaStack);
                }

                return 0;
			}
			else
			{
                --LuaException.SendMsgCount;                                
                return LuaDLL.luaL_throw(L, ""invalid arguments to method: UnityEngine.GameObject.SendMessage"");     
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}";

    [UseDefinedAttribute]
    public void SendMessage(string methodName)
    {
    }
}
