using UnityEngine;
using System;
using LuaInterface;

public class System_ArrayWrap 
{
    public static void Register(LuaState L)
    {
        L.BeginClass(typeof(Array), null);
        L.RegFunction(".geti", get_Item);
        L.RegFunction(".seti", set_Item);
        L.RegFunction("ToTable", ToTable);    
        L.RegVar("Length", get_Length, null);
        L.EndClass();        
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_Item(IntPtr L)
    {
        try
        {
            Array obj = ToLua.ToObject(L, 1) as Array;

            if (obj == null)
            {
                throw new LuaException("trying to index an invalid object reference");                
            }

            int index = (int)LuaDLL.lua_tointeger(L, 2);

            if (index >= obj.Length)
            {
                throw new LuaException("array index out of bounds: " + index + " " + obj.Length);                
            }

            object val = obj.GetValue(index);

            if (val == null)
            {
                throw new LuaException(string.Format("array index {0} is null", index));                
            }

            ToLua.Push(L, val);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int set_Item(IntPtr L)
    {
        try
        {
            Array obj = ToLua.ToObject(L, 1) as Array;

            if (obj == null)
            {
                throw new LuaException("trying to index an invalid object reference");                
            }

            int index = (int)LuaDLL.lua_tointeger(L, 2);
            object val = ToLua.ToVarObject(L, 3);
            Type type = obj.GetType().GetElementType();

            if (!ToLua.CheckType(L, type, 3))
            {                
                throw new LuaException("trying to set object type is not correct");                
            }

            val = Convert.ChangeType(val, type);
            obj.SetValue(val, index);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_Length(IntPtr L)
    {
        try
        {
            Array obj = ToLua.ToObject(L, 1) as Array;

            if (obj == null)
            {
                throw new LuaException("trying to index an invalid object reference");                
            }

            LuaDLL.lua_pushinteger(L, obj.Length);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int ToTable(IntPtr L)
    {
        try
        {
            Array obj = ToLua.ToObject(L, 1) as Array;

            if (obj == null)
            {
                throw new LuaException("trying to index an invalid object reference");                
            }

            LuaDLL.lua_createtable(L, obj.Length, 0);

            for (int i = 0; i < obj.Length; i++)
            {
                object val = obj.GetValue(i);
                ToLua.Push(L, val);
                LuaDLL.lua_rawseti(L, -2, i);
            }

            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }
}
