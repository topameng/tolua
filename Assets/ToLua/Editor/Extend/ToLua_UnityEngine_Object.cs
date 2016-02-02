using UnityEngine;
using System.Collections;
using LuaInterface;

public class ToLua_UnityEngine_Object     
{        
    public static string DestroyDefined =
@"        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 1)
            {
                UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.CheckObject(L, 1);
                ToLua.Destroy(L);
                UnityEngine.Object.Destroy(arg0);
                return 0;
            }
            else if (count == 2)
            {                
                float arg1 = (float)LuaDLL.luaL_checknumber(L, 2);
                int udata = LuaDLL.tolua_rawnetobj(L, 1);
                ObjectTranslator translator = LuaState.GetTranslator(L);
                translator.DelayDestroy(udata, arg1);
                return 0;
            }
            else
            {
                return LuaDLL.tolua_error(L, ""invalid arguments to method: Object.Destroy"");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }";
    
    public static string DestroyImmediateDefined =
@"        try
        {
            int count = LuaDLL.lua_gettop(L);

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
                UnityEngine.Object.DestroyImmediate(arg0, arg1);
                return 0;
            }
            else
            {
                return LuaDLL.tolua_error(L, ""invalid arguments to method: Object.DestroyImmediate"");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }";

    public static string InstantiateDefined =
@"        try
        {
            ++LuaException.InstantiateCount;
            int count = LuaDLL.lua_gettop(L);

            if (count == 1 && ToLua.CheckTypes(L, 1, typeof(UnityEngine.Object)))
            {
                UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
                UnityEngine.Object o = UnityEngine.Object.Instantiate(arg0);

                if (LuaDLL.lua_toboolean(L, LuaDLL.lua_upvalueindex(1)))
                {
                    string error = LuaDLL.lua_tostring(L, -1);
                    LuaDLL.lua_pop(L, 1);
                    throw new LuaException(error, LuaException.luaStack);
                }
                else
                {
                    ToLua.Push(L, o);
                }

                --LuaException.InstantiateCount;
                return 1;
            }
            else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(UnityEngine.Object), typeof(UnityEngine.Vector3), typeof(UnityEngine.Quaternion)))
            {
                UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
                UnityEngine.Vector3 arg1 = ToLua.ToVector3(L, 2);
                UnityEngine.Quaternion arg2 = ToLua.ToQuaternion(L, 3);
                UnityEngine.Object o = UnityEngine.Object.Instantiate(arg0, arg1, arg2);

                if (LuaDLL.lua_toboolean(L, LuaDLL.lua_upvalueindex(1)))
                {
                    string error = LuaDLL.lua_tostring(L, -1);
                    LuaDLL.lua_pop(L, 1);
                    throw new LuaException(error, LuaException.luaStack);
                }
                else
                {
                    ToLua.Push(L, o);
                }

                --LuaException.InstantiateCount;
                return 1;
            }
            else
            {
                --LuaException.InstantiateCount;
                return LuaDLL.tolua_error(L, ""invalid arguments to method: UnityEngine.Object.Instantiate"");
            }
        }
        catch (Exception e)
        {
            --LuaException.InstantiateCount;
            return LuaDLL.toluaL_exception(L, e);
        }  
";


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

    [UseDefinedAttribute]
    public static void Instantiate(Object original)
    {

    }
}
