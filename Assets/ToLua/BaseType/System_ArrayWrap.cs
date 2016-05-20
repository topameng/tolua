using UnityEngine;
using System;
using LuaInterface;

public class System_ArrayWrap 
{
    public static void Register(LuaState L)
    {
        L.BeginClass(typeof(Array), typeof(System.Object));
        L.RegFunction(".geti", get_Item);
        L.RegFunction(".seti", set_Item);
        L.RegFunction("ToTable", ToTable);        
		L.RegFunction("GetLength", GetLength);
		L.RegFunction("GetLongLength", GetLongLength);
		L.RegFunction("GetLowerBound", GetLowerBound);
		L.RegFunction("GetValue", GetValue);
		L.RegFunction("SetValue", SetValue);
		L.RegFunction("GetEnumerator", GetEnumerator);
		L.RegFunction("GetUpperBound", GetUpperBound);
		L.RegFunction("CreateInstance", CreateInstance);
		L.RegFunction("BinarySearch", BinarySearch);
		L.RegFunction("Clear", Clear);
		L.RegFunction("Clone", Clone);
		L.RegFunction("Copy", Copy);
		L.RegFunction("IndexOf", IndexOf);
		L.RegFunction("Initialize", Initialize);
		L.RegFunction("LastIndexOf", LastIndexOf);
		L.RegFunction("Reverse", Reverse);
		L.RegFunction("Sort", Sort);
		L.RegFunction("CopyTo", CopyTo);
		L.RegFunction("ConstrainedCopy", ConstrainedCopy);
		L.RegFunction("__tostring", Lua_ToString);
		L.RegVar("Length", get_Length, null);
		L.RegVar("LongLength", get_LongLength, null);
		L.RegVar("Rank", get_Rank, null);
		L.RegVar("IsSynchronized", get_IsSynchronized, null);
		L.RegVar("SyncRoot", get_SyncRoot, null);
		L.RegVar("IsFixedSize", get_IsFixedSize, null);
		L.RegVar("IsReadOnly", get_IsReadOnly, null);
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

            if (!TypeChecker.CheckType(L, type, 3))
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
                LuaDLL.lua_rawseti(L, -2, i + 1);
            }

            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetLength(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
            int o = obj.GetLength(arg0);
            LuaDLL.lua_pushinteger(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetLongLength(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
            long o = obj.GetLongLength(arg0);
            LuaDLL.lua_pushnumber(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetLowerBound(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
            int o = obj.GetLowerBound(arg0);
            LuaDLL.lua_pushinteger(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetValue(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(long)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                long arg0 = (long)LuaDLL.lua_tonumber(L, 2);
                object o = obj.GetValue(arg0);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(long), typeof(long)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                long arg0 = (long)LuaDLL.lua_tonumber(L, 2);
                long arg1 = (long)LuaDLL.lua_tonumber(L, 3);
                object o = obj.GetValue(arg0, arg1);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
                object o = obj.GetValue(arg0, arg1);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(long), typeof(long), typeof(long)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                long arg0 = (long)LuaDLL.lua_tonumber(L, 2);
                long arg1 = (long)LuaDLL.lua_tonumber(L, 3);
                long arg2 = (long)LuaDLL.lua_tonumber(L, 4);
                object o = obj.GetValue(arg0, arg1, arg2);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int), typeof(int)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
                object o = obj.GetValue(arg0, arg1, arg2);
                ToLua.Push(L, o);
                return 1;
            }
            else if (TypeChecker.CheckParamsType(L, typeof(long), 2, count - 1))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                long[] arg0 = ToLua.ToParamsNumber<long>(L, 2, count - 1);
                object o = obj.GetValue(arg0);
                ToLua.Push(L, o);
                return 1;
            }
            else if (TypeChecker.CheckParamsType(L, typeof(int), 2, count - 1))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                int[] arg0 = ToLua.ToParamsNumber<int>(L, 2, count - 1);
                object o = obj.GetValue(arg0);
                ToLua.Push(L, o);
                return 1;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.GetValue");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int SetValue(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(long)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2);
                long arg1 = (long)LuaDLL.lua_tonumber(L, 3);
                obj.SetValue(arg0, arg1);
                return 0;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(int), typeof(int)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
                obj.SetValue(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(long), typeof(long)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2);
                long arg1 = (long)LuaDLL.lua_tonumber(L, 3);
                long arg2 = (long)LuaDLL.lua_tonumber(L, 4);
                obj.SetValue(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(int), typeof(int), typeof(int)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 4);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 5);
                obj.SetValue(arg0, arg1, arg2, arg3);
                return 0;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(long), typeof(long), typeof(long)))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2);
                long arg1 = (long)LuaDLL.lua_tonumber(L, 3);
                long arg2 = (long)LuaDLL.lua_tonumber(L, 4);
                long arg3 = (long)LuaDLL.lua_tonumber(L, 5);
                obj.SetValue(arg0, arg1, arg2, arg3);
                return 0;
            }
            else if (TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object)) && TypeChecker.CheckParamsType(L, typeof(long), 3, count - 2))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2);
                long[] arg1 = ToLua.ToParamsNumber<long>(L, 3, count - 2);
                obj.SetValue(arg0, arg1);
                return 0;
            }
            else if (TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object)) && TypeChecker.CheckParamsType(L, typeof(int), 3, count - 2))
            {
                System.Array obj = (System.Array)ToLua.ToObject(L, 1);
                object arg0 = ToLua.ToVarObject(L, 2);
                int[] arg1 = ToLua.ToParamsNumber<int>(L, 3, count - 2);
                obj.SetValue(arg0, arg1);
                return 0;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.SetValue");
            }
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
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            System.Collections.IEnumerator o = obj.GetEnumerator();
            ToLua.Push(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int GetUpperBound(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
            int o = obj.GetUpperBound(arg0);
            LuaDLL.lua_pushinteger(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int CreateInstance(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Type), typeof(int)))
            {
                System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                System.Array o = System.Array.CreateInstance(arg0, arg1);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Type), typeof(int[]), typeof(int[])))
            {
                System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
                int[] arg1 = ToLua.CheckNumberArray<int>(L, 2);
                int[] arg2 = ToLua.CheckNumberArray<int>(L, 3);
                System.Array o = System.Array.CreateInstance(arg0, arg1, arg2);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Type), typeof(int), typeof(int)))
            {
                System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                System.Array o = System.Array.CreateInstance(arg0, arg1, arg2);
                ToLua.Push(L, o);
                return 1;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Type), typeof(int), typeof(int), typeof(int)))
            {
                System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
                System.Array o = System.Array.CreateInstance(arg0, arg1, arg2, arg3);
                ToLua.Push(L, o);
                return 1;
            }
            else if (TypeChecker.CheckTypes(L, 1, typeof(System.Type)) && TypeChecker.CheckParamsType(L, typeof(long), 2, count - 1))
            {
                System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
                long[] arg1 = ToLua.ToParamsNumber<long>(L, 2, count - 1);
                System.Array o = System.Array.CreateInstance(arg0, arg1);
                ToLua.Push(L, o);
                return 1;
            }
            else if (TypeChecker.CheckTypes(L, 1, typeof(System.Type)) && TypeChecker.CheckParamsType(L, typeof(int), 2, count - 1))
            {
                System.Type arg0 = (System.Type)ToLua.ToObject(L, 1);
                int[] arg1 = ToLua.ToParamsNumber<int>(L, 2, count - 1);
                System.Array o = System.Array.CreateInstance(arg0, arg1);
                ToLua.Push(L, o);
                return 1;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.CreateInstance");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int BinarySearch(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2);
                int o = System.Array.BinarySearch(arg0, arg1);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(System.Collections.IComparer)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2);
                System.Collections.IComparer arg2 = (System.Collections.IComparer)ToLua.ToObject(L, 3);
                int o = System.Array.BinarySearch(arg0, arg1, arg2);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int), typeof(object)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                object arg3 = ToLua.ToVarObject(L, 4);
                int o = System.Array.BinarySearch(arg0, arg1, arg2, arg3);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int), typeof(object), typeof(System.Collections.IComparer)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                object arg3 = ToLua.ToVarObject(L, 4);
                System.Collections.IComparer arg4 = (System.Collections.IComparer)ToLua.ToObject(L, 5);
                int o = System.Array.BinarySearch(arg0, arg1, arg2, arg3, arg4);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.BinarySearch");
            }
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
            ToLua.CheckArgsCount(L, 3);
            System.Array arg0 = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            int arg1 = (int)LuaDLL.luaL_checknumber(L, 2);
            int arg2 = (int)LuaDLL.luaL_checknumber(L, 3);
            System.Array.Clear(arg0, arg1, arg2);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Clone(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 1);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            object o = obj.Clone();
            ToLua.Push(L, o);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Copy(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(System.Array), typeof(long)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array arg1 = (System.Array)ToLua.ToObject(L, 2);
                long arg2 = (long)LuaDLL.lua_tonumber(L, 3);
                System.Array.Copy(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(long), typeof(System.Array), typeof(long), typeof(long)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                long arg1 = (long)LuaDLL.lua_tonumber(L, 2);
                System.Array arg2 = (System.Array)ToLua.ToObject(L, 3);
                long arg3 = (long)LuaDLL.lua_tonumber(L, 4);
                long arg4 = (long)LuaDLL.lua_tonumber(L, 5);
                System.Array.Copy(arg0, arg1, arg2, arg3, arg4);
                return 0;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(System.Array), typeof(int), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                System.Array arg2 = (System.Array)ToLua.ToObject(L, 3);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
                int arg4 = (int)LuaDLL.lua_tonumber(L, 5);
                System.Array.Copy(arg0, arg1, arg2, arg3, arg4);
                return 0;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.Copy");
            }
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
            int count = LuaDLL.lua_gettop(L);

            if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2);
                int o = System.Array.IndexOf(arg0, arg1);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int o = System.Array.IndexOf(arg0, arg1, arg2);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(int), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
                int o = System.Array.IndexOf(arg0, arg1, arg2, arg3);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.IndexOf");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Initialize(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 1);
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            obj.Initialize();
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int LastIndexOf(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2);
                int o = System.Array.LastIndexOf(arg0, arg1);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int o = System.Array.LastIndexOf(arg0, arg1, arg2);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(object), typeof(int), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                object arg1 = ToLua.ToVarObject(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
                int o = System.Array.LastIndexOf(arg0, arg1, arg2, arg3);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.LastIndexOf");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Reverse(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 1 && TypeChecker.CheckTypes(L, 1, typeof(System.Array)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array.Reverse(arg0);
                return 0;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                System.Array.Reverse(arg0, arg1, arg2);
                return 0;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.Reverse");
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Sort(IntPtr L)
    {
        try
        {
            int count = LuaDLL.lua_gettop(L);

            if (count == 1 && TypeChecker.CheckTypes(L, 1, typeof(System.Array)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array.Sort(arg0);
                return 0;
            }
            else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(System.Collections.IComparer)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Collections.IComparer arg1 = (System.Collections.IComparer)ToLua.ToObject(L, 2);
                System.Array.Sort(arg0, arg1);
                return 0;
            }
            else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(System.Array)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array arg1 = (System.Array)ToLua.ToObject(L, 2);
                System.Array.Sort(arg0, arg1);
                return 0;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(System.Array), typeof(System.Collections.IComparer)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array arg1 = (System.Array)ToLua.ToObject(L, 2);
                System.Collections.IComparer arg2 = (System.Collections.IComparer)ToLua.ToObject(L, 3);
                System.Array.Sort(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 3 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                System.Array.Sort(arg0, arg1, arg2);
                return 0;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(int), typeof(int), typeof(System.Collections.IComparer)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                int arg1 = (int)LuaDLL.lua_tonumber(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                System.Collections.IComparer arg3 = (System.Collections.IComparer)ToLua.ToObject(L, 4);
                System.Array.Sort(arg0, arg1, arg2, arg3);
                return 0;
            }
            else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(System.Array), typeof(int), typeof(int)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array arg1 = (System.Array)ToLua.ToObject(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
                System.Array.Sort(arg0, arg1, arg2, arg3);
                return 0;
            }
            else if (count == 5 && TypeChecker.CheckTypes(L, 1, typeof(System.Array), typeof(System.Array), typeof(int), typeof(int), typeof(System.Collections.IComparer)))
            {
                System.Array arg0 = (System.Array)ToLua.ToObject(L, 1);
                System.Array arg1 = (System.Array)ToLua.ToObject(L, 2);
                int arg2 = (int)LuaDLL.lua_tonumber(L, 3);
                int arg3 = (int)LuaDLL.lua_tonumber(L, 4);
                System.Collections.IComparer arg4 = (System.Collections.IComparer)ToLua.ToObject(L, 5);
                System.Array.Sort(arg0, arg1, arg2, arg3, arg4);
                return 0;
            }
            else
            {
                return LuaDLL.luaL_throw(L, "invalid arguments to method: System.Array.Sort");
            }
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
            System.Array obj = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            System.Array arg0 = (System.Array)ToLua.CheckObject(L, 2, typeof(System.Array));
            long arg1 = (long)LuaDLL.luaL_checknumber(L, 3);
            obj.CopyTo(arg0, arg1);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int ConstrainedCopy(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 5);
            System.Array arg0 = (System.Array)ToLua.CheckObject(L, 1, typeof(System.Array));
            int arg1 = (int)LuaDLL.luaL_checknumber(L, 2);
            System.Array arg2 = (System.Array)ToLua.CheckObject(L, 3, typeof(System.Array));
            int arg3 = (int)LuaDLL.luaL_checknumber(L, 4);
            int arg4 = (int)LuaDLL.luaL_checknumber(L, 5);
            System.Array.ConstrainedCopy(arg0, arg1, arg2, arg3, arg4);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Lua_ToString(IntPtr L)
    {
        object obj = ToLua.ToObject(L, 1);

        if (obj != null)
        {
            LuaDLL.lua_pushstring(L, obj.ToString());
        }
        else
        {
            LuaDLL.lua_pushnil(L);
        }

        return 1;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_LongLength(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            System.Array obj = (System.Array)o;
            long ret = obj.LongLength;
            LuaDLL.lua_pushnumber(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index LongLength on a nil value" : e.Message);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_Rank(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            System.Array obj = (System.Array)o;
            int ret = obj.Rank;
            LuaDLL.lua_pushinteger(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index Rank on a nil value" : e.Message);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_IsSynchronized(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            System.Array obj = (System.Array)o;
            bool ret = obj.IsSynchronized;
            LuaDLL.lua_pushboolean(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index IsSynchronized on a nil value" : e.Message);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_SyncRoot(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            System.Array obj = (System.Array)o;
            object ret = obj.SyncRoot;
            ToLua.Push(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index SyncRoot on a nil value" : e.Message);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_IsFixedSize(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            System.Array obj = (System.Array)o;
            bool ret = obj.IsFixedSize;
            LuaDLL.lua_pushboolean(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index IsFixedSize on a nil value" : e.Message);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int get_IsReadOnly(IntPtr L)
    {
        object o = null;

        try
        {
            o = ToLua.ToObject(L, 1);
            System.Array obj = (System.Array)o;
            bool ret = obj.IsReadOnly;
            LuaDLL.lua_pushboolean(L, ret);
            return 1;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index IsReadOnly on a nil value" : e.Message);
        }
    }
}
