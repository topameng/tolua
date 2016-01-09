using UnityEngine;
using System.Collections;
using LuaInterface;

public class ToLua_UnityEngine_Object     
{        
    public static string DestroyDefined =
@"		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.CheckObject(L, 1);
            ToLua.Destroy(L);
            UnityEngine.Object.Destroy(arg0);				
			return 0;
		}
		else if (count == 2)
		{
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.CheckObject(L, 1);            
            float arg1 = (float)LuaDLL.luaL_checknumber(L, 2);
            int udata = LuaDLL.tolua_rawnetobj(L, 1);
            ObjectTranslator translator = LuaState.GetTranslator(L);

            Action Call = () =>
            {
                translator.Destroy(udata);                
                UnityEngine.Object.Destroy(arg0);
            };

            translator.DelayDestroy(Call, arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, ""invalid arguments to method: Object.Destroy"");
		}

		return 0;";
    
    public static string DestroyImmediateDefined =
@"		int count = LuaDLL.lua_gettop(L);

		if (count == 1)
		{
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.CheckObject(L, 1);
			ToLua.Destroy(L);
			UnityEngine.Object.DestroyImmediate(arg0);
			return 0;
		}
		else if (count == 2)
		{
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.CheckObject(L, 1);
			bool arg1 = LuaDLL.luaL_checkboolean(L, 2);
			ToLua.Destroy(L);
			UnityEngine.Object.DestroyImmediate(arg0,arg1);
			return 0;
		}
		else
		{
			LuaDLL.luaL_error(L, ""invalid arguments to method: Object.DestroyImmediate"");
		}

		return 0;";


    [UseDefinedAttribute]
    public static void Destroy(Object obj)
    {
        
    }

    [UseDefinedAttribute]
    public static void DestroyImmediate(Object obj)
    {

    }

    [NoToLuaAttribute]
    public static void DestroyObject(Object obj)
    {

    }   
}
