using System;
using LuaInterface;
using System.Collections.ObjectModel;
using System.Collections;

public class System_Collections_ObjectModel_ReadOnlyCollectionWrap
{
    static Type TypeOfReadOnlyCollection = typeof(ReadOnlyCollection<>);

    public static void Register(LuaState L)
    {
        L.BeginClass(TypeOfReadOnlyCollection, typeof(System.Object), "ReadOnlyCollection");
        L.RegFunction("Contains", new LuaCSFunction(Contains));
        L.RegFunction("CopyTo", new LuaCSFunction(CopyTo));
        L.RegFunction("GetEnumerator", new LuaCSFunction(GetEnumerator));
        L.RegFunction("IndexOf", new LuaCSFunction(IndexOf));
        L.RegFunction(".geti", new LuaCSFunction(get_Item));
        L.RegFunction("get_Item", new LuaCSFunction(get_Item));
        L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
        L.RegVar("Count", new LuaCSFunction(get_Count), null);
        L.EndClass();
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Contains(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            Type argType = null;
            IList obj = (IList)ToLua.CheckGenericObject(L, 1, TypeOfReadOnlyCollection, out argType);
            object arg0 = ToLua.CheckVarObject(L, 2, argType);
            bool o = obj.Contains(arg0);
            //bool o = (bool)LuaMethodCache.CallSingleMethod("Contains", obj, arg0);
            LuaDLL.lua_pushboolean(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int CopyTo(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 3);
            Type argType = null;
            ICollection obj = (ICollection)ToLua.CheckGenericObject(L, 1, TypeOfReadOnlyCollection, out argType);
            Array arg0 = (Array)ToLua.CheckObject(L, 2, argType.MakeArrayType());
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
            IEnumerable obj = (IEnumerable)ToLua.CheckGenericObject(L, 1, TypeOfReadOnlyCollection);
            IEnumerator o = obj.GetEnumerator();
            //IEnumerator o = (IEnumerator)LuaMethodCache.CallSingleMethod("GetEnumerator", obj);
            ToLua.Push(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int IndexOf(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            Type argType = null;
            IList obj = (IList)ToLua.CheckGenericObject(L, 1, TypeOfReadOnlyCollection, out argType);
            object arg0 = ToLua.CheckVarObject(L, 2, argType);
            int o = obj.IndexOf(arg0);
            //int o = (int)LuaMethodCache.CallSingleMethod("IndexOf", obj, arg0);
            LuaDLL.lua_pushinteger(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_Item(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            IList obj = (IList)ToLua.CheckGenericObject(L, 1, TypeOfReadOnlyCollection);
            int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
            object o = obj[arg0];
            //object o = LuaMethodCache.CallSingleMethod("get_Item", obj, arg0);
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
            LuaDLL.lua_pushinteger(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o, "attempt to index Count on a nil value");
        }
    }
}

