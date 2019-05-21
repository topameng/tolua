using System;
using LuaInterface;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;
using UnityEngine;
using System.Collections;

public class System_Collections_Generic_ListWrap
{    
    public static void Register(LuaState L)
	{
        IntPtr lazyWrapFunc = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)LazyWrap);
        IntPtr lazyVarWrapFunc = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)LazyVarWrap);
        L.BeginClass(typeof(List<>), typeof(System.Object), "List");
        L.RegLazyFunction("Add", lazyWrapFunc);
        L.RegLazyFunction("AddRange", lazyWrapFunc);
        L.RegLazyFunction("AsReadOnly", lazyWrapFunc);
        L.RegLazyFunction("BinarySearch", lazyWrapFunc);
        L.RegLazyFunction("Clear", lazyWrapFunc);
        L.RegLazyFunction("Contains", lazyWrapFunc);
        L.RegLazyFunction("CopyTo", lazyWrapFunc);
        L.RegLazyFunction("Exists", lazyWrapFunc);
        L.RegLazyFunction("Find", lazyWrapFunc);
        L.RegLazyFunction("FindAll", lazyWrapFunc);
        L.RegLazyFunction("FindIndex", lazyWrapFunc);
        L.RegLazyFunction("FindLast", lazyWrapFunc);
        L.RegLazyFunction("FindLastIndex", lazyWrapFunc);
        L.RegLazyFunction("ForEach", lazyWrapFunc);
        L.RegLazyFunction("GetEnumerator", lazyWrapFunc);
        L.RegLazyFunction("GetRange", lazyWrapFunc);
        L.RegLazyFunction("IndexOf", lazyWrapFunc);
        L.RegLazyFunction("Insert", lazyWrapFunc);
        L.RegLazyFunction("InsertRange", lazyWrapFunc);
        L.RegLazyFunction("LastIndexOf", lazyWrapFunc);
        L.RegLazyFunction("Remove", lazyWrapFunc);
        L.RegLazyFunction("RemoveAll", lazyWrapFunc);
        L.RegLazyFunction("RemoveAt", lazyWrapFunc);
        L.RegLazyFunction("RemoveRange", lazyWrapFunc);
        L.RegLazyFunction("Reverse", lazyWrapFunc);
        L.RegLazyFunction("Sort", lazyWrapFunc);
        L.RegLazyFunction("ToArray", lazyWrapFunc);
        L.RegLazyFunction("TrimExcess", lazyWrapFunc);
        L.RegLazyFunction("TrueForAll", lazyWrapFunc);
        L.RegLazyFunction("get_Item", lazyWrapFunc);
        L.RegLazyFunction("set_Item", lazyWrapFunc);
        L.RegFunction(".geti", get_Item);
        L.RegFunction(".seti", set_Item);
        L.RegFunction("__tostring", ToLua.op_ToString);
        L.RegLazyVar("Capacity", true, true, lazyVarWrapFunc);
        L.RegLazyVar("Count", true, false, lazyVarWrapFunc);
        L.EndClass();        
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Add(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);
            object arg0 = ToLua.CheckVarObject(L, 2, argType);
            LuaMethodCache.CallSingleMethod("Add", obj, arg0);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int AddRange(IntPtr L)
    {
        try
        {
            ToLua.CheckArgsCount(L, 2);
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);
            object arg0 = ToLua.CheckObject(L, 2, typeof(IEnumerable<>).MakeGenericType(argType));
            LuaMethodCache.CallSingleMethod("AddRange", obj, arg0);
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AsReadOnly(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);
            object o = LuaMethodCache.CallSingleMethod("AsReadOnly", obj);            
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
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
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);

            if (count == 2)
			{                
                object arg0 = ToLua.CheckVarObject(L, 2, argType);
                int o = (int)LuaMethodCache.CallMethod("BinarySearch", obj, arg0);                
                LuaDLL.lua_pushinteger(L, o);
                return 1;
			}
			else if (count == 3)
			{                
                object arg0 = ToLua.CheckVarObject(L, 2, argType);
                object arg1 = ToLua.CheckObject(L, 3, typeof(IComparer<>).MakeGenericType(argType));
                int o = (int)LuaMethodCache.CallMethod("BinarySearch", obj, arg0, arg1);                
                LuaDLL.lua_pushinteger(L, o);
                return 1;
			}
			else if (count == 5)
			{
                int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
                int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
                object arg2 = ToLua.CheckVarObject(L, 4, argType);
                object arg3 = ToLua.CheckObject(L, 5, typeof(IComparer<>).MakeGenericType(argType));				
                int o = (int)LuaMethodCache.CallMethod("BinarySearch", obj, arg0, arg1, arg2, arg3);
                LuaDLL.lua_pushinteger(L, o);
                return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, string.Format("invalid arguments to method: List<{0}>.BinarySearch", LuaMisc.GetTypeName(argType)));
			}
		}
		catch(Exception e)
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
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>));
            LuaMethodCache.CallSingleMethod("Clear", obj);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Contains(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);
            object arg0 = ToLua.CheckVarObject(L, 2, argType);
            object o = LuaMethodCache.CallSingleMethod("Contains", obj, arg0);            
			LuaDLL.lua_pushboolean(L, (bool)o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CopyTo(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);

            if (count == 2)
			{				                
                object arg0 = ToLua.CheckObject(L, 2, argType.MakeArrayType());
                LuaMethodCache.CallMethod("CopyTo", obj, arg0);
				return 0;
			}
			else if (count == 3)
			{                
                object arg0 = ToLua.CheckObject(L, 2, argType.MakeArrayType());                
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
                LuaMethodCache.CallMethod("CopyTo", obj, arg0, arg1);
                return 0;
			}
			else if (count == 5)
			{				                
                int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);				
                object arg1 = ToLua.CheckObject(L, 3, argType.MakeArrayType());
                int arg2 = (int)LuaDLL.luaL_checknumber(L, 4);
				int arg3 = (int)LuaDLL.luaL_checknumber(L, 5);
                LuaMethodCache.CallMethod("CopyTo", obj, arg0, arg1, arg2, arg3);
                return 0;
			}
			else
			{
				
                return LuaDLL.luaL_throw(L, string.Format("invalid arguments to method: List<{0}>.CopyTo", LuaMisc.GetTypeName(argType)));
            }
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Exists(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
            Type argType = null;            
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);
            Delegate arg0 = ToLua.CheckDelegate(typeof(System.Predicate<>).MakeGenericType(argType), L, 2);       
            bool o = (bool)LuaMethodCache.CallMethod("Exists", obj, arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Find(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
            Type argType = null;            
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);
            Delegate arg0 = ToLua.CheckDelegate(typeof(System.Predicate<>).MakeGenericType(argType), L, 2);            
            object o = LuaMethodCache.CallMethod("Find", obj, arg0);
            ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindAll(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
            Type argType = null;            
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);
            Delegate arg0 = ToLua.CheckDelegate(typeof(System.Predicate<>).MakeGenericType(argType), L, 2);
            object o = LuaMethodCache.CallMethod("FindAll", obj, arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindIndex(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);

            if (count == 2)
			{
                Delegate arg0 = ToLua.CheckDelegate(typeof(System.Predicate<>).MakeGenericType(argType), L, 2);                
                int o = (int)LuaMethodCache.CallMethod("FindIndex", obj, arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 3)
			{				
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
                Delegate arg1 = ToLua.CheckDelegate(typeof(System.Predicate<>).MakeGenericType(argType), L, 3);                
                int o = (int)LuaMethodCache.CallMethod("FindIndex", obj, arg0, arg1);                
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 4)
			{				
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);				
                Delegate arg2 = ToLua.CheckDelegate(typeof(System.Predicate<>).MakeGenericType(argType), L, 4);                
                int o = (int)LuaMethodCache.CallMethod("FindIndex", obj, arg0, arg1, arg2);
                LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{
                return LuaDLL.luaL_throw(L, string.Format("invalid arguments to method: List<{0}>.FindIndex", LuaMisc.GetTypeName(argType)));                
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindLast(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
            Type argType = null;            
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);
            Delegate arg0 = ToLua.CheckDelegate(typeof(System.Predicate<>).MakeGenericType(argType), L, 2);            
            object o = LuaMethodCache.CallSingleMethod("FindLast", obj, arg0);
            ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindLastIndex(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);
            Type argType = null;            
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);

            if (count == 2)
			{				
				Delegate arg0 = (Delegate)ToLua.CheckObject(L, 2, typeof(System.Predicate<>).MakeGenericType(argType));				
                int o = (int)LuaMethodCache.CallMethod("FindLastIndex", obj, arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 3)
			{				
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				Delegate arg1 = (Delegate)ToLua.CheckObject(L, 3, typeof(System.Predicate<>).MakeGenericType(argType));				
                int o = (int)LuaMethodCache.CallMethod("FindLastIndex", obj, arg0, arg1);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 4)
			{				
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);				
                Delegate arg2 = (Delegate)ToLua.CheckObject(L, 4, typeof(System.Predicate<>).MakeGenericType(argType));
                int o = (int)LuaMethodCache.CallMethod("FindLastIndex", obj, arg0, arg1, arg2);                
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{				
                return LuaDLL.luaL_throw(L, string.Format("invalid arguments to method: List<{0}>.FindLastIndex", LuaMisc.GetTypeName(argType)));
            }
        }
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ForEach(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
            Type argType = null;            
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);            
			Delegate arg0 = ToLua.CheckDelegate(typeof(System.Action<>).MakeGenericType(argType), L, 2);	
            LuaMethodCache.CallSingleMethod("ForEach", obj, arg0);
			return 0;
		}
		catch(Exception e)
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
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>));
            IEnumerator o = LuaMethodCache.CallSingleMethod("GetEnumerator", obj) as IEnumerator;            
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetRange(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);			
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>));
            int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);			
            object o = LuaMethodCache.CallSingleMethod("GetRange", obj, arg0, arg1);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch(Exception e)
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
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);

            if (count == 2)
			{								
                object arg0 = ToLua.CheckVarObject(L, 2, argType);
				int o = (int)LuaMethodCache.CallMethod("IndexOf", obj, arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 3)
			{				
                object arg0 = ToLua.CheckVarObject(L, 2, argType);                
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);				
                int o = (int)LuaMethodCache.CallMethod("IndexOf", obj, arg0, arg1);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 4)
			{
                object arg0 = ToLua.CheckVarObject(L, 2, argType);                
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
				int arg2 = (int)LuaDLL.luaL_checknumber(L, 4);				
                int o = (int)LuaMethodCache.CallMethod("IndexOf", obj, arg0, arg1, arg2);
                LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{				
                return LuaDLL.luaL_throw(L, string.Format("invalid arguments to method: List<{0}>.IndexOf", LuaMisc.GetTypeName(argType)));
            }
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Insert(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);            
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);			
            object arg1 = ToLua.CheckVarObject(L, 3, argType);
            LuaMethodCache.CallSingleMethod("Insert", obj, arg0, arg1);			
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InsertRange(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);
            int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			IEnumerable arg1 = (IEnumerable)ToLua.CheckObject(L, 3, typeof(IEnumerable<>).MakeGenericType(argType));
            LuaMethodCache.CallSingleMethod("InsertRange", obj, arg0, arg1);
			return 0;
		}
		catch(Exception e)
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
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);

            if (count == 2)
			{
                object arg0 = ToLua.CheckVarObject(L, 2, argType);                
				int o = (int)LuaMethodCache.CallMethod("LastIndexOf", obj, arg0);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 3)
			{
                object arg0 = ToLua.CheckVarObject(L, 2, argType);
                int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
				int o = (int)LuaMethodCache.CallMethod("LastIndexOf", obj, arg0, arg1);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else if (count == 4)
			{
                object arg0 = ToLua.CheckVarObject(L, 2, argType);
                int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
				int arg2 = (int)LuaDLL.luaL_checknumber(L, 4);
				int o = (int)LuaMethodCache.CallMethod("LastIndexOf", obj, arg0, arg1, arg2);
				LuaDLL.lua_pushinteger(L, o);
				return 1;
			}
			else
			{				
                return LuaDLL.luaL_throw(L, string.Format("invalid arguments to method: List<{0}>.LastIndexOf", LuaMisc.GetTypeName(argType)));
            }
		}
		catch(Exception e)
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
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);
            object arg0 = ToLua.CheckVarObject(L, 2, argType);
            bool o = (bool)LuaMethodCache.CallSingleMethod("Remove", obj, arg0);			
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveAll(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);            
			Delegate arg0 = ToLua.CheckDelegate(typeof(System.Predicate<>).MakeGenericType(argType), L, 2);
			int o = (int)LuaMethodCache.CallSingleMethod("RemoveAll", obj, arg0);
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveAt(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>));            
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
            LuaMethodCache.CallSingleMethod("RemoveAt", obj, arg0);			
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveRange(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>));
            int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
            LuaMethodCache.CallSingleMethod("RemoveRange", obj, arg0, arg1);
			return 0;
		}
		catch(Exception e)
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
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);

            if (count == 1)
			{
                LuaMethodCache.CallMethod("Reverse", obj);				
				return 0;
			}
			else if (count == 3)
			{				
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
                LuaMethodCache.CallMethod("Reverse", obj, arg0, arg1);                
				return 0;
			}
			else
			{				
                return LuaDLL.luaL_throw(L, string.Format("invalid arguments to method: List<{0}>.LastIndexOf", LuaMisc.GetTypeName(argType)));
            }
		}
		catch(Exception e)
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
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);

            if (count == 1)
			{
                LuaMethodCache.CallMethod("Sort", obj);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes(L, 2, typeof(System.Comparison<>).MakeGenericType(argType)))
			{				
				Delegate arg0 = (Delegate)ToLua.ToObject(L, 2);
                LuaMethodCache.CallMethod("Sort", obj, arg0);                
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes(L, 2, typeof(IComparer<>).MakeGenericType(argType)))
			{
                object arg0 = ToLua.ToObject(L, 2);
                LuaMethodCache.CallMethod("Sort", obj, arg0);
                return 0;
			}
			else if (count == 4)
			{				
				int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
				int arg1 = (int)LuaDLL.luaL_checknumber(L, 3);
				object arg2 = ToLua.CheckObject(L, 4, typeof(IComparer<>).MakeGenericType(argType));
                LuaMethodCache.CallMethod("Sort", obj, arg0, arg1, arg2);                
				return 0;
			}
			else
			{				
                return LuaDLL.luaL_throw(L, string.Format("invalid arguments to method: List<{0}>.LastIndexOf", LuaMisc.GetTypeName(argType)));
            }
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToArray(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>));
            Array o = (Array)LuaMethodCache.CallSingleMethod("ToArray", obj);			
			ToLua.Push(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TrimExcess(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>));
            LuaMethodCache.CallSingleMethod("TrimExcess", obj);
            return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int TrueForAll(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
            Type argType = null;			
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);
            Delegate arg0 = ToLua.CheckDelegate(typeof(System.Predicate<>).MakeGenericType(argType), L, 2);
            bool o = (bool)LuaMethodCache.CallSingleMethod("TrueForAll", obj, arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
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
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>));
            int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
            object o = LuaMethodCache.CallSingleMethod("get_Item", obj, arg0);
            ToLua.Push(L, o);			
			return 1;
		}
		catch(Exception e)
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
            Type argType = null;
            object obj = ToLua.CheckGenericObject(L, 1, typeof(List<>), out argType);            
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
            object arg1 = ToLua.CheckObject(L, 3, argType);
            LuaMethodCache.CallSingleMethod("set_Item", obj, arg0, arg1);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Capacity(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);			
            int ret = (int)LuaMethodCache.CallSingleMethod("get_Capacity", o);			
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Capacity on a nil value");
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
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Count on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_Capacity(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);			
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
            LuaMethodCache.CallSingleMethod("set_Capacity", o, arg0);            
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index Capacity on a nil value");
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
                case "Add":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)Add);
                        LuaDLL.tolua_function(L, "Add", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return Add(L);
                case "AddRange":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)AddRange);
                        LuaDLL.tolua_function(L, "AddRange", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return AddRange(L);
                case "AsReadOnly":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)AsReadOnly);
                        LuaDLL.tolua_function(L, "AsReadOnly", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return AsReadOnly(L);
                case "BinarySearch":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)BinarySearch);
                        LuaDLL.tolua_function(L, "BinarySearch", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return BinarySearch(L);
                case "Clear":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)Clear);
                        LuaDLL.tolua_function(L, "Clear", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return Clear(L);
                case "Contains":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)Contains);
                        LuaDLL.tolua_function(L, "Contains", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return Contains(L);
                case "CopyTo":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)CopyTo);
                        LuaDLL.tolua_function(L, "CopyTo", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return CopyTo(L);
                case "Exists":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)Exists);
                        LuaDLL.tolua_function(L, "Exists", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return Exists(L);
                case "Find":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)Find);
                        LuaDLL.tolua_function(L, "Find", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return Find(L);
                case "FindAll":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)FindAll);
                        LuaDLL.tolua_function(L, "FindAll", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return FindAll(L);
                case "FindIndex":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)FindIndex);
                        LuaDLL.tolua_function(L, "FindIndex", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return FindIndex(L);
                case "FindLast":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)FindLast);
                        LuaDLL.tolua_function(L, "FindLast", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return FindLast(L);
                case "FindLastIndex":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)FindLastIndex);
                        LuaDLL.tolua_function(L, "FindLastIndex", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return FindLastIndex(L);
                case "ForEach":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)ForEach);
                        LuaDLL.tolua_function(L, "ForEach", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return ForEach(L);
                case "GetEnumerator":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)GetEnumerator);
                        LuaDLL.tolua_function(L, "GetEnumerator", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return GetEnumerator(L);
                case "GetRange":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)GetRange);
                        LuaDLL.tolua_function(L, "GetRange", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return GetRange(L);
                case "IndexOf":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)IndexOf);
                        LuaDLL.tolua_function(L, "IndexOf", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return IndexOf(L);
                case "Insert":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)Insert);
                        LuaDLL.tolua_function(L, "Insert", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return Insert(L);
                case "InsertRange":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)InsertRange);
                        LuaDLL.tolua_function(L, "InsertRange", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return InsertRange(L);
                case "LastIndexOf":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)LastIndexOf);
                        LuaDLL.tolua_function(L, "LastIndexOf", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return LastIndexOf(L);
                case "Remove":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)Remove);
                        LuaDLL.tolua_function(L, "Remove", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return Remove(L);
                case "RemoveAll":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)RemoveAll);
                        LuaDLL.tolua_function(L, "RemoveAll", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return RemoveAll(L);
                case "RemoveAt":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)RemoveAt);
                        LuaDLL.tolua_function(L, "RemoveAt", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return RemoveAt(L);
                case "RemoveRange":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)RemoveRange);
                        LuaDLL.tolua_function(L, "RemoveRange", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return RemoveRange(L);
                case "Reverse":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)Reverse);
                        LuaDLL.tolua_function(L, "Reverse", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return Reverse(L);
                case "Sort":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)Sort);
                        LuaDLL.tolua_function(L, "Sort", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return Sort(L);
                case "ToArray":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)ToArray);
                        LuaDLL.tolua_function(L, "ToArray", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return ToArray(L);
                case "TrimExcess":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)TrimExcess);
                        LuaDLL.tolua_function(L, "TrimExcess", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return TrimExcess(L);
                case "TrueForAll":
                    if (lazy)
                    {
                        IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)TrueForAll);
                        LuaDLL.tolua_function(L, "TrueForAll", fn);
                        LuaDLL.lua_pop(L, 1);
                    }

                    return TrueForAll(L);
                case "get_Item":
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
                case "Capacity":
                    if (lazy)
                    {
                        if (getStatus)
                        {
                            IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)get_Capacity);
                            LuaDLL.tolua_variable(L, "Capacity", fn, IntPtr.Zero);
                        }
                        else
                        {
                            IntPtr fn = Marshal.GetFunctionPointerForDelegate((LuaCSFunction)set_Capacity);
                            LuaDLL.tolua_variable(L, "Capacity", IntPtr.Zero, fn);
                        }

                        LuaDLL.lua_pop(L, 1);
                    }

                    if (getStatus)
                    {
                        return get_Capacity(L);
                    }
                    else
                    {
                        return set_Capacity(L);
                    }
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
            }
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }
}

