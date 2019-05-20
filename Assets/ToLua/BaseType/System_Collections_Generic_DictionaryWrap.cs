using System;
using LuaInterface;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Collections;

public class System_Collections_Generic_DictionaryWrap
{
    public static void Register(LuaState L)
    {
        IntPtr lazyWrapFunc = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)LazyWrap);
        IntPtr lazyVarWrapFunc = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)LazyVarWrap);
        L.BeginClass(typeof(Dictionary<,>), typeof(System.Object), "Dictionary");
        L.RegLazyFunction("get_Item", lazyWrapFunc);
        L.RegLazyFunction("set_Item", lazyWrapFunc);
        L.RegFunction(".geti", _geti);
        L.RegFunction(".seti", _seti);
        L.RegLazyFunction("Add", lazyWrapFunc);
        L.RegLazyFunction("Clear", lazyWrapFunc);
        L.RegLazyFunction("ContainsKey", lazyWrapFunc);
        L.RegLazyFunction("ContainsValue", lazyWrapFunc);
        L.RegLazyFunction("GetObjectData", lazyWrapFunc);
        L.RegLazyFunction("OnDeserialization", lazyWrapFunc);
        L.RegLazyFunction("Remove", lazyWrapFunc);
        L.RegLazyFunction("TryGetValue", lazyWrapFunc);
        L.RegLazyFunction("GetEnumerator", lazyWrapFunc);
        L.RegVar("this", _this, null);
        L.RegFunction("__tostring", ToLua.op_ToString);
        L.RegLazyVar("Count", true, false, lazyVarWrapFunc);
        L.RegLazyVar("Comparer", true, false, lazyVarWrapFunc);
        L.RegLazyVar("Keys", true, false, lazyVarWrapFunc);
        L.RegLazyVar("Values", true, false, lazyVarWrapFunc);
        L.EndClass();
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int _get_this(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            Type kt = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>), out kt);
            object arg0 = ToLua.CheckVarObject(L, 2, kt);
            object o = LuaMethodCache.CallSingleMethod("get_Item", obj, arg0);
            ToLua.Push(L, o);
            return 1;

        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int _set_this(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 3);
            Type kt, vt;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>), out kt, out vt);
            object arg0 = ToLua.CheckVarObject(L, 2, kt);
            object arg1 = ToLua.CheckVarObject(L, 3, vt);
            LuaMethodCache.CallSingleMethod("set_Item", obj, arg0, arg1);
            return 0;

        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int _this(IntPtr L)
    {
        try
        {
            LuaDLL.lua_pushvalue(L, 1);
            LuaDLL.tolua_bindthis(L, _get_this, _set_this);
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
            Type kt = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>), out kt);
            object arg0 = ToLua.CheckVarObject(L, 2, kt);
            object o = LuaMethodCache.CallSingleMethod("get_Item", obj, arg0);
            ToLua.Push(L, o);
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
            ToLua.CheckArgsCount(L, 3);
            Type kt, vt;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>), out kt, out vt);
            object arg0 = ToLua.CheckVarObject(L, 2, kt);
            object arg1 = ToLua.CheckVarObject(L, 3, vt);
            LuaMethodCache.CallSingleMethod("set_Item", obj, arg0, arg1);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int _geti(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            Type kt = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>), out kt);

            if (kt != typeof(int))
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                object arg0 = ToLua.CheckVarObject(L, 2, kt);
                object o = LuaMethodCache.CallSingleMethod("get_Item", obj, arg0);
                ToLua.Push(L, o);
            }

            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int _seti(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 3);
            Type kt, vt;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>), out kt, out vt);

            if (kt == typeof(int))
            {
                object arg0 = ToLua.CheckVarObject(L, 2, kt);
                object arg1 = ToLua.CheckVarObject(L, 3, vt);
                LuaMethodCache.CallSingleMethod("set_Item", obj, arg0, arg1);
            }

            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Add(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 3);
            Type kt, vt;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>), out kt, out vt);
            object arg0 = ToLua.CheckVarObject(L, 2, kt);
            object arg1 = ToLua.CheckVarObject(L, 3, vt);
            LuaMethodCache.CallSingleMethod("Add", obj, arg0, arg1);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Clear(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 1);
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>));
            LuaMethodCache.CallSingleMethod("Clear", obj);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int ContainsKey(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            Type kt;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>), out kt);
            object arg0 = ToLua.CheckVarObject(L, 2, kt);
            bool o = (bool)LuaMethodCache.CallSingleMethod("ContainsKey", obj, arg0);
            LuaDLL.lua_pushboolean(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int ContainsValue(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            Type kt, vt;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>), out kt, out vt);
            object arg0 = ToLua.CheckVarObject(L, 2, vt);
            bool o = (bool)LuaMethodCache.CallSingleMethod("ContainsValue", obj, arg0);
            LuaDLL.lua_pushboolean(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetObjectData(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 3);
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>));
            SerializationInfo arg0 = (SerializationInfo)ToLua.CheckObject(L, 2, typeof(SerializationInfo));
            StreamingContext arg1 = (StreamingContext)ToLua.CheckObject(L, 3, typeof(StreamingContext));
            LuaMethodCache.CallSingleMethod("GetObjectData", obj, arg0, arg1);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int OnDeserialization(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>));
            object arg0 = ToLua.ToVarObject(L, 2);
            LuaMethodCache.CallSingleMethod("OnDeserialization", obj, arg0);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Remove(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            Type kt;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>), out kt);
            object arg0 = ToLua.CheckVarObject(L, 2, kt);
            bool o = (bool)LuaMethodCache.CallSingleMethod("Remove", obj, arg0);
            LuaDLL.lua_pushboolean(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int TryGetValue(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 3);
            Type kt;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>), out kt);
            object arg0 = ToLua.CheckVarObject(L, 2, kt);
            object arg1 = null;
            object[] args = new object[] { arg0, arg1 };
            bool o = (bool)LuaMethodCache.CallSingleMethod("TryGetValue", obj, args);
            LuaDLL.lua_pushboolean(L, o);
            ToLua.Push(L, args[1]);
            return 2;
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
            object obj = ToLua.CheckGenericObject(L, 1, typeof(Dictionary<,>));
            IEnumerator o = (IEnumerator)LuaMethodCache.CallSingleMethod("GetEnumerator", obj);
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
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            int ret = (int)LuaMethodCache.CallSingleMethod("get_Count", o);
            LuaDLL.lua_pushinteger(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o, "attempt to index Count on a nil value");
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_Comparer(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            object ret = LuaMethodCache.CallSingleMethod("get_Comparer", o);
            ToLua.PushObject(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o, "attempt to index Comparer on a nil value");
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_Keys(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            object ret = LuaMethodCache.CallSingleMethod("get_Keys", o);
            ToLua.PushObject(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o, "attempt to index Keys on a nil value");
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_Values(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            object ret = LuaMethodCache.CallSingleMethod("get_Values", o);
            ToLua.PushObject(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o, "attempt to index Values on a nil value");
        }
    }


    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int LazyWrap(IntPtr L)
    {
        try
        {
            int stackTop = LuaDLL.lua_gettop(L);
            bool lazy = LuaDLL.luaL_checkboolean(L, stackTop);
            string key = LuaDLL.lua_tostring(L, stackTop - 1);
            LuaDLL.lua_pop(L, 2);

            switch (key)
            {
                case "get_Item":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)get_Item);
                        LuaDLL.tolua_function(L, "get_Item", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return get_Item(L);
                case "set_Item":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)set_Item);
                        LuaDLL.tolua_function(L, "set_Item", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return set_Item(L);
                case "Add":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)Add);
                        LuaDLL.tolua_function(L, "Add", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return Add(L);
                case "Clear":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)Clear);
                        LuaDLL.tolua_function(L, "Clear", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return Clear(L);
                case "ContainsKey":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)ContainsKey);
                        LuaDLL.tolua_function(L, "ContainsKey", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return ContainsKey(L);
                case "ContainsValue":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)ContainsValue);
                        LuaDLL.tolua_function(L, "ContainsValue", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return ContainsValue(L);
                case "GetObjectData":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)GetObjectData);
                        LuaDLL.tolua_function(L, "GetObjectData", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return GetObjectData(L);
                case "OnDeserialization":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)OnDeserialization);
                        LuaDLL.tolua_function(L, "OnDeserialization", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return OnDeserialization(L);
                case "Remove":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)Remove);
                        LuaDLL.tolua_function(L, "Remove", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return Remove(L);
                case "TryGetValue":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)TryGetValue);
                        LuaDLL.tolua_function(L, "TryGetValue", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return TryGetValue(L);
                case "GetEnumerator":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)GetEnumerator);
                        LuaDLL.tolua_function(L, "GetEnumerator", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return GetEnumerator(L);
            }
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int LazyVarWrap(IntPtr L)
    {
        try
        {
            int stackTop = LuaDLL.lua_gettop(L);
            bool getStatus = LuaDLL.luaL_checkboolean(L, stackTop);
            bool lazy = LuaDLL.luaL_checkboolean(L, stackTop - 1);
            string key = LuaDLL.lua_tostring(L, stackTop - 2);
            LuaDLL.lua_pop(L, 3);

            switch (key)
            {
                case "Count":
                    if (lazy)
                    {
                        if (getStatus)
                        {
                            IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)get_Count);
                            LuaDLL.tolua_variable(L, "Count", fn, IntPtr.Zero);
                        }

                        LuaDLL.lua_pop(L, 1);
                    }

                    if (getStatus)
                    {
                        return get_Count(L);
                    }

                    break;
                case "Comparer":
                    if (lazy)
                    {
                        if (getStatus)
                        {
                            IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)get_Comparer);
                            LuaDLL.tolua_variable(L, "Comparer", fn, IntPtr.Zero);
                        }

                        LuaDLL.lua_pop(L, 1);
                    }

                    if (getStatus)
                    {
                        return get_Comparer(L);
                    }

                    break;
                case "Keys":
                    if (lazy)
                    {
                        if (getStatus)
                        {
                            IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)get_Keys);
                            LuaDLL.tolua_variable(L, "Keys", fn, IntPtr.Zero);
                        }

                        LuaDLL.lua_pop(L, 1);
                    }

                    if (getStatus)
                    {
                        return get_Keys(L);
                    }

                    break;
                case "Values":
                    if (lazy)
                    {
                        if (getStatus)
                        {
                            IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)get_Values);
                            LuaDLL.tolua_variable(L, "Values", fn, IntPtr.Zero);
                        }

                        LuaDLL.lua_pop(L, 1);
                    }

                    if (getStatus)
                    {
                        return get_Values(L);
                    }

                    break;
            }
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }
}

