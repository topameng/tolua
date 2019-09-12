using System;
using LuaInterface;
using System.Collections.Generic;
using System.Collections;

public class System_Collections_Generic_Dictionary_KeyCollectionWrap
{
    static Type TypeOfKeyCollection = typeof(Dictionary<,>.KeyCollection);

    public static void Register(LuaState L)
    {
        L.BeginClass(TypeOfKeyCollection, typeof(System.Object), "KeyCollection");
        L.RegFunction("CopyTo", new LuaCSFunction(CopyTo));
        L.RegFunction("GetEnumerator", new LuaCSFunction(GetEnumerator));
        L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
        L.RegVar("Count", new LuaCSFunction(get_Count), null);
        L.EndClass();
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int CopyTo(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 3);
            Type kt = null;
            ICollection obj = (ICollection)ToLua.CheckGenericObject(L, 1, TypeOfKeyCollection, out kt);
            Array arg0 = (Array)ToLua.CheckObject(L, 2, kt.MakeArrayType());
            int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
            obj.CopyTo(arg0, arg1);
            //LuaMethodCache.CallSingleMethod("CopyTo", obj, arg0, arg1);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetEnumerator(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 1);
            IEnumerable obj = (IEnumerable)ToLua.CheckGenericObject(L, 1, TypeOfKeyCollection);
            IEnumerator o = obj.GetEnumerator();
            ToLua.Push(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_Count(IntPtr L)
    {
        ICollection o = null;

        try
        {
            o = (ICollection)ToLua.ToObject(L, 1);
            int ret = o.Count;
            //int ret = (int)LuaMethodCache.CallSingleMethod("get_Count", o);
            LuaDLL.lua_pushinteger(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o, "attempt to index Count on a nil value");
        }
    }
}

