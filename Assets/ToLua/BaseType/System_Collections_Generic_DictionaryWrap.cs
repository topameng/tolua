using System;
using LuaInterface;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections;

public class System_Collections_Generic_DictionaryWrap
{
    static Type TypeOfDictionary = typeof(Dictionary<,>);

    public static void Register(LuaState L)
    {
        L.BeginClass(TypeOfDictionary, typeof(System.Object), "Dictionary");
        L.RegFunction("get_Item", new LuaCSFunction(get_Item));
        L.RegFunction("set_Item", new LuaCSFunction(set_Item));
        L.RegFunction(".geti", new LuaCSFunction(_geti));
        L.RegFunction(".seti", new LuaCSFunction(_seti));
        L.RegFunction("Add", new LuaCSFunction(Add));
        L.RegFunction("Clear", new LuaCSFunction(Clear));
        L.RegFunction("ContainsKey", new LuaCSFunction(ContainsKey));
        L.RegFunction("ContainsValue", new LuaCSFunction(ContainsValue));
        L.RegFunction("GetObjectData", new LuaCSFunction(GetObjectData));
        L.RegFunction("OnDeserialization", new LuaCSFunction(OnDeserialization));
        L.RegFunction("Remove", new LuaCSFunction(Remove));
        L.RegFunction("TryGetValue", new LuaCSFunction(TryGetValue));
        L.RegFunction("GetEnumerator", new LuaCSFunction(GetEnumerator));
        L.RegVar("this", new LuaCSFunction(_this), null);
        L.RegFunction("__tostring", new LuaCSFunction(ToLua.op_ToString));
        L.RegVar("Count", new LuaCSFunction(get_Count), null);
        L.RegVar("Comparer", new LuaCSFunction(get_Comparer), null);
        L.RegVar("Keys", new LuaCSFunction(get_Keys), null);
        L.RegVar("Values", new LuaCSFunction(get_Values), null);
        L.EndClass();
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int _get_this(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            Type kt = null;
            object obj = ToLua.CheckGenericObject(L, 1, TypeOfDictionary, out kt);
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
            object obj = ToLua.CheckGenericObject(L, 1, TypeOfDictionary, out kt, out vt);
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
            IDictionary obj = (IDictionary)ToLua.CheckGenericObject(L, 1, TypeOfDictionary, out kt);
            object arg0 = ToLua.CheckVarObject(L, 2, kt);
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
    static int set_Item(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 3);
            Type kt, vt;
            IDictionary obj = (IDictionary)ToLua.CheckGenericObject(L, 1, TypeOfDictionary, out kt, out vt);
            object arg0 = ToLua.CheckVarObject(L, 2, kt);
            object arg1 = ToLua.CheckVarObject(L, 3, vt);
            obj[arg0] = arg1;
            //LuaMethodCache.CallSingleMethod("set_Item", obj, arg0, arg1);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    static Type TypeOfInt = typeof(int);

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int _geti(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            Type kt = null;
            IDictionary obj = (IDictionary)ToLua.CheckGenericObject(L, 1, TypeOfDictionary, out kt);

            if (kt != TypeOfInt)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                object arg0 = ToLua.CheckVarObject(L, 2, kt);
                object o = obj[arg0];
                //object o = LuaMethodCache.CallSingleMethod("get_Item", obj, arg0);
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
            IDictionary obj = (IDictionary)ToLua.CheckGenericObject(L, 1, TypeOfDictionary, out kt, out vt);

            if (kt == typeof(int))
            {
                object arg0 = ToLua.CheckVarObject(L, 2, kt);
                object arg1 = ToLua.CheckVarObject(L, 3, vt);
                obj[arg0] = arg1;
                //LuaMethodCache.CallSingleMethod("set_Item", obj, arg0, arg1);
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
            IDictionary obj = (IDictionary)ToLua.CheckGenericObject(L, 1, TypeOfDictionary, out kt, out vt);
            object arg0 = ToLua.CheckVarObject(L, 2, kt);
            object arg1 = ToLua.CheckVarObject(L, 3, vt);
            obj.Add(arg0, arg1);
            //LuaMethodCache.CallSingleMethod("Add", obj, arg0, arg1);
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
            IDictionary obj = (IDictionary)ToLua.CheckGenericObject(L, 1, TypeOfDictionary);
            obj.Clear();
            //LuaMethodCache.CallSingleMethod("Clear", obj);
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
            object obj = ToLua.CheckGenericObject(L, 1, TypeOfDictionary, out kt);
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
            object obj = ToLua.CheckGenericObject(L, 1, TypeOfDictionary, out kt, out vt);
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
            object obj = ToLua.CheckGenericObject(L, 1, TypeOfDictionary);
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
            object obj = ToLua.CheckGenericObject(L, 1, TypeOfDictionary);
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
            object obj = ToLua.CheckGenericObject(L, 1, TypeOfDictionary, out kt);
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
            object obj = ToLua.CheckGenericObject(L, 1, TypeOfDictionary, out kt);
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
            object obj = ToLua.CheckGenericObject(L, 1, TypeOfDictionary);
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
}

