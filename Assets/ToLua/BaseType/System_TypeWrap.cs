using System;
using LuaInterface;

public class System_TypeWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(System.Type), typeof(System.Object));
		L.RegFunction("Equals", Equals);
		L.RegFunction("GetType", GetType);
		L.RegFunction("GetTypeArray", GetTypeArray);
		L.RegFunction("GetTypeCode", GetTypeCode);
		L.RegFunction("GetTypeFromCLSID", GetTypeFromCLSID);
		L.RegFunction("GetTypeFromHandle", GetTypeFromHandle);
		L.RegFunction("GetTypeFromProgID", GetTypeFromProgID);
		L.RegFunction("GetTypeHandle", GetTypeHandle);
		L.RegFunction("IsSubclassOf", IsSubclassOf);
		L.RegFunction("FindInterfaces", FindInterfaces);
		L.RegFunction("GetInterface", GetInterface);
		L.RegFunction("GetInterfaceMap", GetInterfaceMap);
		L.RegFunction("GetInterfaces", GetInterfaces);
		L.RegFunction("IsAssignableFrom", IsAssignableFrom);
		L.RegFunction("IsInstanceOfType", IsInstanceOfType);
		L.RegFunction("GetArrayRank", GetArrayRank);
		L.RegFunction("GetElementType", GetElementType);
		L.RegFunction("GetEvent", GetEvent);
		L.RegFunction("GetEvents", GetEvents);
		L.RegFunction("GetField", GetField);
		L.RegFunction("GetFields", GetFields);
		L.RegFunction("GetHashCode", GetHashCode);
		L.RegFunction("GetMember", GetMember);
		L.RegFunction("GetMembers", GetMembers);
		L.RegFunction("GetMethod", GetMethod);
		L.RegFunction("GetMethods", GetMethods);
		L.RegFunction("GetNestedType", GetNestedType);
		L.RegFunction("GetNestedTypes", GetNestedTypes);
		L.RegFunction("GetProperties", GetProperties);
		L.RegFunction("GetProperty", GetProperty);
		L.RegFunction("GetConstructor", GetConstructor);
		L.RegFunction("GetConstructors", GetConstructors);
		L.RegFunction("GetDefaultMembers", GetDefaultMembers);
		L.RegFunction("FindMembers", FindMembers);
		L.RegFunction("InvokeMember", InvokeMember);
		L.RegFunction("ToString", ToString);
		L.RegFunction("GetGenericArguments", GetGenericArguments);
		L.RegFunction("GetGenericTypeDefinition", GetGenericTypeDefinition);
		L.RegFunction("MakeGenericType", MakeGenericType);
		L.RegFunction("GetGenericParameterConstraints", GetGenericParameterConstraints);
		L.RegFunction("MakeArrayType", MakeArrayType);
		L.RegFunction("MakeByRefType", MakeByRefType);
		L.RegFunction("MakePointerType", MakePointerType);
		L.RegFunction("ReflectionOnlyGetType", ReflectionOnlyGetType);
		L.RegFunction("New", _CreateSystem_Type);
		L.RegFunction("__tostring", Lua_ToString);
		L.RegVar("Delimiter", get_Delimiter, null);
		L.RegVar("EmptyTypes", get_EmptyTypes, null);
		L.RegVar("FilterAttribute", get_FilterAttribute, null);
		L.RegVar("FilterName", get_FilterName, null);
		L.RegVar("FilterNameIgnoreCase", get_FilterNameIgnoreCase, null);
		L.RegVar("Missing", get_Missing, null);
		L.RegVar("Assembly", get_Assembly, null);
		L.RegVar("AssemblyQualifiedName", get_AssemblyQualifiedName, null);
		L.RegVar("Attributes", get_Attributes, null);
		L.RegVar("BaseType", get_BaseType, null);
		L.RegVar("DeclaringType", get_DeclaringType, null);
		L.RegVar("DefaultBinder", get_DefaultBinder, null);
		L.RegVar("FullName", get_FullName, null);
		L.RegVar("GUID", get_GUID, null);
		L.RegVar("HasElementType", get_HasElementType, null);
		L.RegVar("IsAbstract", get_IsAbstract, null);
		L.RegVar("IsAnsiClass", get_IsAnsiClass, null);
		L.RegVar("IsArray", get_IsArray, null);
		L.RegVar("IsAutoClass", get_IsAutoClass, null);
		L.RegVar("IsAutoLayout", get_IsAutoLayout, null);
		L.RegVar("IsByRef", get_IsByRef, null);
		L.RegVar("IsClass", get_IsClass, null);
		L.RegVar("IsCOMObject", get_IsCOMObject, null);
		L.RegVar("IsContextful", get_IsContextful, null);
		L.RegVar("IsEnum", get_IsEnum, null);
		L.RegVar("IsExplicitLayout", get_IsExplicitLayout, null);
		L.RegVar("IsImport", get_IsImport, null);
		L.RegVar("IsInterface", get_IsInterface, null);
		L.RegVar("IsLayoutSequential", get_IsLayoutSequential, null);
		L.RegVar("IsMarshalByRef", get_IsMarshalByRef, null);
		L.RegVar("IsNestedAssembly", get_IsNestedAssembly, null);
		L.RegVar("IsNestedFamANDAssem", get_IsNestedFamANDAssem, null);
		L.RegVar("IsNestedFamily", get_IsNestedFamily, null);
		L.RegVar("IsNestedFamORAssem", get_IsNestedFamORAssem, null);
		L.RegVar("IsNestedPrivate", get_IsNestedPrivate, null);
		L.RegVar("IsNestedPublic", get_IsNestedPublic, null);
		L.RegVar("IsNotPublic", get_IsNotPublic, null);
		L.RegVar("IsPointer", get_IsPointer, null);
		L.RegVar("IsPrimitive", get_IsPrimitive, null);
		L.RegVar("IsPublic", get_IsPublic, null);
		L.RegVar("IsSealed", get_IsSealed, null);
		L.RegVar("IsSerializable", get_IsSerializable, null);
		L.RegVar("IsSpecialName", get_IsSpecialName, null);
		L.RegVar("IsUnicodeClass", get_IsUnicodeClass, null);
		L.RegVar("IsValueType", get_IsValueType, null);
		L.RegVar("MemberType", get_MemberType, null);
		L.RegVar("Module", get_Module, null);
		L.RegVar("Namespace", get_Namespace, null);
		L.RegVar("ReflectedType", get_ReflectedType, null);
		L.RegVar("TypeHandle", get_TypeHandle, null);
		L.RegVar("TypeInitializer", get_TypeInitializer, null);
		L.RegVar("UnderlyingSystemType", get_UnderlyingSystemType, null);
		L.RegVar("ContainsGenericParameters", get_ContainsGenericParameters, null);
		L.RegVar("IsGenericTypeDefinition", get_IsGenericTypeDefinition, null);
		L.RegVar("IsGenericType", get_IsGenericType, null);
		L.RegVar("IsGenericParameter", get_IsGenericParameter, null);
		L.RegVar("IsNested", get_IsNested, null);
		L.RegVar("IsVisible", get_IsVisible, null);
		L.RegVar("GenericParameterPosition", get_GenericParameterPosition, null);
		L.RegVar("GenericParameterAttributes", get_GenericParameterAttributes, null);
		L.RegVar("DeclaringMethod", get_DeclaringMethod, null);
		L.RegVar("StructLayoutAttribute", get_StructLayoutAttribute, null);
		L.RegVar("out", get_out, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateSystem_Type(IntPtr L)
	{
		return LuaDLL.luaL_error(L, "System.Type class does not have a constructor function");
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Equals(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(System.Type)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Type arg0 = (System.Type)ToLua.ToObject(L, 2);
			bool o;

			try
			{
				o = obj != null ? obj.Equals(arg0) : arg0 == null;
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(object)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			object arg0 = ToLua.ToVarObject(L, 2);
			bool o;

			try
			{
				o = obj != null ? obj.Equals(arg0) : arg0 == null;
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.Equals");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetType(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(System.Type)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Type o = null;

			try
			{
				o = obj.GetType();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 1 && ToLua.CheckTypes(L, 1, typeof(string)))
		{
			string arg0 = ToLua.ToString(L, 1);
			System.Type o = null;

			try
			{
				o = System.Type.GetType(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(bool)))
		{
			string arg0 = ToLua.ToString(L, 1);
			bool arg1 = LuaDLL.lua_toboolean(L, 2);
			System.Type o = null;

			try
			{
				o = System.Type.GetType(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(bool), typeof(bool)))
		{
			string arg0 = ToLua.ToString(L, 1);
			bool arg1 = LuaDLL.lua_toboolean(L, 2);
			bool arg2 = LuaDLL.lua_toboolean(L, 3);
			System.Type o = null;

			try
			{
				o = System.Type.GetType(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetType");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeArray(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		object[] arg0 = ToLua.CheckObjectArray(L, 1);
		System.Type[] o = null;

		try
		{
			o = System.Type.GetTypeArray(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeCode(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type arg0 = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.TypeCode o;

		try
		{
			o = System.Type.GetTypeCode(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeFromCLSID(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(System.Guid)))
		{
			System.Guid arg0 = (System.Guid)ToLua.ToObject(L, 1);
			System.Type o = null;

			try
			{
				o = System.Type.GetTypeFromCLSID(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Guid), typeof(string)))
		{
			System.Guid arg0 = (System.Guid)ToLua.ToObject(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			System.Type o = null;

			try
			{
				o = System.Type.GetTypeFromCLSID(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Guid), typeof(bool)))
		{
			System.Guid arg0 = (System.Guid)ToLua.ToObject(L, 1);
			bool arg1 = LuaDLL.lua_toboolean(L, 2);
			System.Type o = null;

			try
			{
				o = System.Type.GetTypeFromCLSID(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(System.Guid), typeof(string), typeof(bool)))
		{
			System.Guid arg0 = (System.Guid)ToLua.ToObject(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			bool arg2 = LuaDLL.lua_toboolean(L, 3);
			System.Type o = null;

			try
			{
				o = System.Type.GetTypeFromCLSID(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetTypeFromCLSID");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeFromHandle(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.RuntimeTypeHandle arg0 = (System.RuntimeTypeHandle)ToLua.CheckObject(L, 1, typeof(System.RuntimeTypeHandle));
		System.Type o = null;

		try
		{
			o = System.Type.GetTypeFromHandle(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeFromProgID(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(string)))
		{
			string arg0 = ToLua.ToString(L, 1);
			System.Type o = null;

			try
			{
				o = System.Type.GetTypeFromProgID(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string)))
		{
			string arg0 = ToLua.ToString(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			System.Type o = null;

			try
			{
				o = System.Type.GetTypeFromProgID(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(string), typeof(bool)))
		{
			string arg0 = ToLua.ToString(L, 1);
			bool arg1 = LuaDLL.lua_toboolean(L, 2);
			System.Type o = null;

			try
			{
				o = System.Type.GetTypeFromProgID(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(string), typeof(string), typeof(bool)))
		{
			string arg0 = ToLua.ToString(L, 1);
			string arg1 = ToLua.ToString(L, 2);
			bool arg2 = LuaDLL.lua_toboolean(L, 3);
			System.Type o = null;

			try
			{
				o = System.Type.GetTypeFromProgID(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetTypeFromProgID");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTypeHandle(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		object arg0 = ToLua.ToVarObject(L, 1);
		System.RuntimeTypeHandle o;

		try
		{
			o = System.Type.GetTypeHandle(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.PushValue(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsSubclassOf(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Type arg0 = (System.Type)ToLua.CheckObject(L, 2, typeof(System.Type));
		bool o;

		try
		{
			o = obj.IsSubclassOf(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushboolean(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindInterfaces(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 3);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Reflection.TypeFilter arg0 = null;
		LuaTypes funcType2 = LuaDLL.lua_type(L, 2);

		if (funcType2 != LuaTypes.LUA_TFUNCTION)
		{
			 arg0 = (System.Reflection.TypeFilter)ToLua.CheckObject(L, 2, typeof(System.Reflection.TypeFilter));
		}
		else
		{
			LuaFunction func = ToLua.ToLuaFunction(L, 2);
			arg0 = DelegateFactory.CreateDelegate(L, typeof(System.Reflection.TypeFilter), func) as System.Reflection.TypeFilter;
		}
		object arg1 = ToLua.ToVarObject(L, 3);
		System.Type[] o = null;

		try
		{
			o = obj.FindInterfaces(arg0, arg1);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInterface(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Type o = null;

			try
			{
				o = obj.GetInterface(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(bool)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			bool arg1 = LuaDLL.lua_toboolean(L, 3);
			System.Type o = null;

			try
			{
				o = obj.GetInterface(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetInterface");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInterfaceMap(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Type arg0 = (System.Type)ToLua.CheckObject(L, 2, typeof(System.Type));
		System.Reflection.InterfaceMapping o;

		try
		{
			o = obj.GetInterfaceMap(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.PushValue(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInterfaces(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Type[] o = null;

		try
		{
			o = obj.GetInterfaces();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsAssignableFrom(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Type arg0 = (System.Type)ToLua.CheckObject(L, 2, typeof(System.Type));
		bool o;

		try
		{
			o = obj.IsAssignableFrom(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushboolean(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IsInstanceOfType(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 2);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		object arg0 = ToLua.ToVarObject(L, 2);
		bool o;

		try
		{
			o = obj.IsInstanceOfType(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushboolean(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetArrayRank(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		int o;

		try
		{
			o = obj.GetArrayRank();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushinteger(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetElementType(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Type o = null;

		try
		{
			o = obj.GetElementType();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetEvent(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.EventInfo o = null;

			try
			{
				o = obj.GetEvent(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 3);
			System.Reflection.EventInfo o = null;

			try
			{
				o = obj.GetEvent(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetEvent");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetEvents(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(System.Type)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.EventInfo[] o = null;

			try
			{
				o = obj.GetEvents();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 2);
			System.Reflection.EventInfo[] o = null;

			try
			{
				o = obj.GetEvents(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetEvents");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetField(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.FieldInfo o = null;

			try
			{
				o = obj.GetField(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 3);
			System.Reflection.FieldInfo o = null;

			try
			{
				o = obj.GetField(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetField");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetFields(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(System.Type)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.FieldInfo[] o = null;

			try
			{
				o = obj.GetFields();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 2);
			System.Reflection.FieldInfo[] o = null;

			try
			{
				o = obj.GetFields(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetFields");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetHashCode(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		int o;

		try
		{
			o = obj.GetHashCode();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushinteger(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMember(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.MemberInfo[] o = null;

			try
			{
				o = obj.GetMember(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 3);
			System.Reflection.MemberInfo[] o = null;

			try
			{
				o = obj.GetMember(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Reflection.MemberTypes), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.MemberTypes arg1 = (System.Reflection.MemberTypes)ToLua.ToObject(L, 3);
			System.Reflection.BindingFlags arg2 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 4);
			System.Reflection.MemberInfo[] o = null;

			try
			{
				o = obj.GetMember(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetMember");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMembers(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(System.Type)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.MemberInfo[] o = null;

			try
			{
				o = obj.GetMembers();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 2);
			System.Reflection.MemberInfo[] o = null;

			try
			{
				o = obj.GetMembers(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetMembers");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMethod(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.MethodInfo o = null;

			try
			{
				o = obj.GetMethod(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Type[])))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Type[] arg1 = ToLua.CheckObjectArray<System.Type>(L, 3);
			System.Reflection.MethodInfo o = null;

			try
			{
				o = obj.GetMethod(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 3);
			System.Reflection.MethodInfo o = null;

			try
			{
				o = obj.GetMethod(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Type[]), typeof(System.Reflection.ParameterModifier[])))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Type[] arg1 = ToLua.CheckObjectArray<System.Type>(L, 3);
			System.Reflection.ParameterModifier[] arg2 = ToLua.CheckObjectArray<System.Reflection.ParameterModifier>(L, 4);
			System.Reflection.MethodInfo o = null;

			try
			{
				o = obj.GetMethod(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 6 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Reflection.BindingFlags), typeof(System.Reflection.Binder), typeof(System.Type[]), typeof(System.Reflection.ParameterModifier[])))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 3);
			System.Reflection.Binder arg2 = (System.Reflection.Binder)ToLua.ToObject(L, 4);
			System.Type[] arg3 = ToLua.CheckObjectArray<System.Type>(L, 5);
			System.Reflection.ParameterModifier[] arg4 = ToLua.CheckObjectArray<System.Reflection.ParameterModifier>(L, 6);
			System.Reflection.MethodInfo o = null;

			try
			{
				o = obj.GetMethod(arg0, arg1, arg2, arg3, arg4);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 7 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Reflection.BindingFlags), typeof(System.Reflection.Binder), typeof(System.Reflection.CallingConventions), typeof(System.Type[]), typeof(System.Reflection.ParameterModifier[])))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 3);
			System.Reflection.Binder arg2 = (System.Reflection.Binder)ToLua.ToObject(L, 4);
			System.Reflection.CallingConventions arg3 = (System.Reflection.CallingConventions)ToLua.ToObject(L, 5);
			System.Type[] arg4 = ToLua.CheckObjectArray<System.Type>(L, 6);
			System.Reflection.ParameterModifier[] arg5 = ToLua.CheckObjectArray<System.Reflection.ParameterModifier>(L, 7);
			System.Reflection.MethodInfo o = null;

			try
			{
				o = obj.GetMethod(arg0, arg1, arg2, arg3, arg4, arg5);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetMethod");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetMethods(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(System.Type)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.MethodInfo[] o = null;

			try
			{
				o = obj.GetMethods();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 2);
			System.Reflection.MethodInfo[] o = null;

			try
			{
				o = obj.GetMethods(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetMethods");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNestedType(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Type o = null;

			try
			{
				o = obj.GetNestedType(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 3);
			System.Type o = null;

			try
			{
				o = obj.GetNestedType(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetNestedType");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetNestedTypes(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(System.Type)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Type[] o = null;

			try
			{
				o = obj.GetNestedTypes();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 2);
			System.Type[] o = null;

			try
			{
				o = obj.GetNestedTypes(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetNestedTypes");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetProperties(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(System.Type)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.PropertyInfo[] o = null;

			try
			{
				o = obj.GetProperties();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 2);
			System.Reflection.PropertyInfo[] o = null;

			try
			{
				o = obj.GetProperties(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetProperties");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetProperty(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.PropertyInfo o = null;

			try
			{
				o = obj.GetProperty(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Type[])))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Type[] arg1 = ToLua.CheckObjectArray<System.Type>(L, 3);
			System.Reflection.PropertyInfo o = null;

			try
			{
				o = obj.GetProperty(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Type)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Type arg1 = (System.Type)ToLua.ToObject(L, 3);
			System.Reflection.PropertyInfo o = null;

			try
			{
				o = obj.GetProperty(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 3 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 3);
			System.Reflection.PropertyInfo o = null;

			try
			{
				o = obj.GetProperty(arg0, arg1);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 4 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Type), typeof(System.Type[])))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Type arg1 = (System.Type)ToLua.ToObject(L, 3);
			System.Type[] arg2 = ToLua.CheckObjectArray<System.Type>(L, 4);
			System.Reflection.PropertyInfo o = null;

			try
			{
				o = obj.GetProperty(arg0, arg1, arg2);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 5 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Type), typeof(System.Type[]), typeof(System.Reflection.ParameterModifier[])))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Type arg1 = (System.Type)ToLua.ToObject(L, 3);
			System.Type[] arg2 = ToLua.CheckObjectArray<System.Type>(L, 4);
			System.Reflection.ParameterModifier[] arg3 = ToLua.CheckObjectArray<System.Reflection.ParameterModifier>(L, 5);
			System.Reflection.PropertyInfo o = null;

			try
			{
				o = obj.GetProperty(arg0, arg1, arg2, arg3);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 7 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Reflection.BindingFlags), typeof(System.Reflection.Binder), typeof(System.Type), typeof(System.Type[]), typeof(System.Reflection.ParameterModifier[])))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 3);
			System.Reflection.Binder arg2 = (System.Reflection.Binder)ToLua.ToObject(L, 4);
			System.Type arg3 = (System.Type)ToLua.ToObject(L, 5);
			System.Type[] arg4 = ToLua.CheckObjectArray<System.Type>(L, 6);
			System.Reflection.ParameterModifier[] arg5 = ToLua.CheckObjectArray<System.Reflection.ParameterModifier>(L, 7);
			System.Reflection.PropertyInfo o = null;

			try
			{
				o = obj.GetProperty(arg0, arg1, arg2, arg3, arg4, arg5);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetProperty");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetConstructor(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(System.Type[])))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Type[] arg0 = ToLua.CheckObjectArray<System.Type>(L, 2);
			System.Reflection.ConstructorInfo o = null;

			try
			{
				o = obj.GetConstructor(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 5 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(System.Reflection.BindingFlags), typeof(System.Reflection.Binder), typeof(System.Type[]), typeof(System.Reflection.ParameterModifier[])))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 2);
			System.Reflection.Binder arg1 = (System.Reflection.Binder)ToLua.ToObject(L, 3);
			System.Type[] arg2 = ToLua.CheckObjectArray<System.Type>(L, 4);
			System.Reflection.ParameterModifier[] arg3 = ToLua.CheckObjectArray<System.Reflection.ParameterModifier>(L, 5);
			System.Reflection.ConstructorInfo o = null;

			try
			{
				o = obj.GetConstructor(arg0, arg1, arg2, arg3);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else if (count == 6 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(System.Reflection.BindingFlags), typeof(System.Reflection.Binder), typeof(System.Reflection.CallingConventions), typeof(System.Type[]), typeof(System.Reflection.ParameterModifier[])))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 2);
			System.Reflection.Binder arg1 = (System.Reflection.Binder)ToLua.ToObject(L, 3);
			System.Reflection.CallingConventions arg2 = (System.Reflection.CallingConventions)ToLua.ToObject(L, 4);
			System.Type[] arg3 = ToLua.CheckObjectArray<System.Type>(L, 5);
			System.Reflection.ParameterModifier[] arg4 = ToLua.CheckObjectArray<System.Reflection.ParameterModifier>(L, 6);
			System.Reflection.ConstructorInfo o = null;

			try
			{
				o = obj.GetConstructor(arg0, arg1, arg2, arg3, arg4);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.PushObject(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetConstructor");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetConstructors(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(System.Type)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.ConstructorInfo[] o = null;

			try
			{
				o = obj.GetConstructors();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(System.Reflection.BindingFlags)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Reflection.BindingFlags arg0 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 2);
			System.Reflection.ConstructorInfo[] o = null;

			try
			{
				o = obj.GetConstructors(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.GetConstructors");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetDefaultMembers(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Reflection.MemberInfo[] o = null;

		try
		{
			o = obj.GetDefaultMembers();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int FindMembers(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 5);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Reflection.MemberTypes arg0 = (System.Reflection.MemberTypes)ToLua.CheckObject(L, 2, typeof(System.Reflection.MemberTypes));
		System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)ToLua.CheckObject(L, 3, typeof(System.Reflection.BindingFlags));
		System.Reflection.MemberFilter arg2 = null;
		LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

		if (funcType4 != LuaTypes.LUA_TFUNCTION)
		{
			 arg2 = (System.Reflection.MemberFilter)ToLua.CheckObject(L, 4, typeof(System.Reflection.MemberFilter));
		}
		else
		{
			LuaFunction func = ToLua.ToLuaFunction(L, 4);
			arg2 = DelegateFactory.CreateDelegate(L, typeof(System.Reflection.MemberFilter), func) as System.Reflection.MemberFilter;
		}
		object arg3 = ToLua.ToVarObject(L, 5);
		System.Reflection.MemberInfo[] o = null;

		try
		{
			o = obj.FindMembers(arg0, arg1, arg2, arg3);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InvokeMember(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 6 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Reflection.BindingFlags), typeof(System.Reflection.Binder), typeof(object), typeof(object[])))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 3);
			System.Reflection.Binder arg2 = (System.Reflection.Binder)ToLua.ToObject(L, 4);
			object arg3 = ToLua.ToVarObject(L, 5);
			object[] arg4 = ToLua.CheckObjectArray(L, 6);
			object o = null;

			try
			{
				o = obj.InvokeMember(arg0, arg1, arg2, arg3, arg4);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 7 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Reflection.BindingFlags), typeof(System.Reflection.Binder), typeof(object), typeof(object[]), typeof(System.Globalization.CultureInfo)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 3);
			System.Reflection.Binder arg2 = (System.Reflection.Binder)ToLua.ToObject(L, 4);
			object arg3 = ToLua.ToVarObject(L, 5);
			object[] arg4 = ToLua.CheckObjectArray(L, 6);
			System.Globalization.CultureInfo arg5 = (System.Globalization.CultureInfo)ToLua.ToObject(L, 7);
			object o = null;

			try
			{
				o = obj.InvokeMember(arg0, arg1, arg2, arg3, arg4, arg5);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 9 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(string), typeof(System.Reflection.BindingFlags), typeof(System.Reflection.Binder), typeof(object), typeof(object[]), typeof(System.Reflection.ParameterModifier[]), typeof(System.Globalization.CultureInfo), typeof(string[])))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			string arg0 = ToLua.ToString(L, 2);
			System.Reflection.BindingFlags arg1 = (System.Reflection.BindingFlags)ToLua.ToObject(L, 3);
			System.Reflection.Binder arg2 = (System.Reflection.Binder)ToLua.ToObject(L, 4);
			object arg3 = ToLua.ToVarObject(L, 5);
			object[] arg4 = ToLua.CheckObjectArray(L, 6);
			System.Reflection.ParameterModifier[] arg5 = ToLua.CheckObjectArray<System.Reflection.ParameterModifier>(L, 7);
			System.Globalization.CultureInfo arg6 = (System.Globalization.CultureInfo)ToLua.ToObject(L, 8);
			string[] arg7 = ToLua.CheckStringArray(L, 9);
			object o = null;

			try
			{
				o = obj.InvokeMember(arg0, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.InvokeMember");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ToString(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		string o = null;

		try
		{
			o = obj.ToString();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		LuaDLL.lua_pushstring(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetGenericArguments(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Type[] o = null;

		try
		{
			o = obj.GetGenericArguments();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetGenericTypeDefinition(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Type o = null;

		try
		{
			o = obj.GetGenericTypeDefinition();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MakeGenericType(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Type[] arg0 = ToLua.CheckParamsObject<System.Type>(L, 2, count - 1);
		System.Type o = null;

		try
		{
			o = obj.MakeGenericType(arg0);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetGenericParameterConstraints(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Type[] o = null;

		try
		{
			o = obj.GetGenericParameterConstraints();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MakeArrayType(IntPtr L)
	{
		int count = LuaDLL.lua_gettop(L);

		if (count == 1 && ToLua.CheckTypes(L, 1, typeof(System.Type)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			System.Type o = null;

			try
			{
				o = obj.MakeArrayType();
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else if (count == 2 && ToLua.CheckTypes(L, 1, typeof(System.Type), typeof(int)))
		{
			System.Type obj = (System.Type)ToLua.ToObject(L, 1);
			int arg0 = (int)LuaDLL.lua_tonumber(L, 2);
			System.Type o = null;

			try
			{
				o = obj.MakeArrayType(arg0);
			}
			catch(Exception e)
			{
				return LuaDLL.luaL_error(L, e.Message);
			}

			ToLua.Push(L, o);
			return 1;
		}
		else
		{
			LuaDLL.luaL_error(L, "invalid arguments to method: System.Type.MakeArrayType");
		}

		return 0;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MakeByRefType(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Type o = null;

		try
		{
			o = obj.MakeByRefType();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int MakePointerType(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 1);
		System.Type obj = (System.Type)ToLua.CheckObject(L, 1, typeof(System.Type));
		System.Type o = null;

		try
		{
			o = obj.MakePointerType();
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ReflectionOnlyGetType(IntPtr L)
	{
		ToLua.CheckArgsCount(L, 3);
		string arg0 = ToLua.CheckString(L, 1);
		bool arg1 = LuaDLL.luaL_checkboolean(L, 2);
		bool arg2 = LuaDLL.luaL_checkboolean(L, 3);
		System.Type o = null;

		try
		{
			o = System.Type.ReflectionOnlyGetType(arg0, arg1, arg2);
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, e.Message);
		}

		ToLua.Push(L, o);
		return 1;
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
	static int get_Delimiter(IntPtr L)
	{
		LuaDLL.lua_pushnumber(L, System.Type.Delimiter);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_EmptyTypes(IntPtr L)
	{
		ToLua.Push(L, System.Type.EmptyTypes);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FilterAttribute(IntPtr L)
	{
		ToLua.Push(L, System.Type.FilterAttribute);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FilterName(IntPtr L)
	{
		ToLua.Push(L, System.Type.FilterName);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FilterNameIgnoreCase(IntPtr L)
	{
		ToLua.Push(L, System.Type.FilterNameIgnoreCase);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Missing(IntPtr L)
	{
		ToLua.Push(L, System.Type.Missing);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Assembly(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.Reflection.Assembly ret = null;

		try
		{
			ret = obj.Assembly;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index Assembly on a nil value" : e.Message);
		}

		ToLua.PushObject(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AssemblyQualifiedName(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		string ret = null;

		try
		{
			ret = obj.AssemblyQualifiedName;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index AssemblyQualifiedName on a nil value" : e.Message);
		}

		LuaDLL.lua_pushstring(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Attributes(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.Reflection.TypeAttributes ret;

		try
		{
			ret = obj.Attributes;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index Attributes on a nil value" : e.Message);
		}

		ToLua.Push(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_BaseType(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.Type ret = null;

		try
		{
			ret = obj.BaseType;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index BaseType on a nil value" : e.Message);
		}

		ToLua.Push(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DeclaringType(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.Type ret = null;

		try
		{
			ret = obj.DeclaringType;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index DeclaringType on a nil value" : e.Message);
		}

		ToLua.Push(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DefaultBinder(IntPtr L)
	{
		ToLua.PushObject(L, System.Type.DefaultBinder);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_FullName(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		string ret = null;

		try
		{
			ret = obj.FullName;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index FullName on a nil value" : e.Message);
		}

		LuaDLL.lua_pushstring(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GUID(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.Guid ret;

		try
		{
			ret = obj.GUID;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index GUID on a nil value" : e.Message);
		}

		ToLua.PushValue(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_HasElementType(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.HasElementType;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index HasElementType on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsAbstract(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsAbstract;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsAbstract on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsAnsiClass(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsAnsiClass;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsAnsiClass on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsArray(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsArray;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsArray on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsAutoClass(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsAutoClass;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsAutoClass on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsAutoLayout(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsAutoLayout;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsAutoLayout on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsByRef(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsByRef;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsByRef on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsClass(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsClass;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsClass on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsCOMObject(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsCOMObject;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsCOMObject on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsContextful(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsContextful;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsContextful on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsEnum(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsEnum;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsEnum on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsExplicitLayout(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsExplicitLayout;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsExplicitLayout on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsImport(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsImport;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsImport on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsInterface(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsInterface;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsInterface on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsLayoutSequential(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsLayoutSequential;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsLayoutSequential on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsMarshalByRef(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsMarshalByRef;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsMarshalByRef on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNestedAssembly(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsNestedAssembly;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsNestedAssembly on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNestedFamANDAssem(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsNestedFamANDAssem;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsNestedFamANDAssem on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNestedFamily(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsNestedFamily;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsNestedFamily on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNestedFamORAssem(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsNestedFamORAssem;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsNestedFamORAssem on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNestedPrivate(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsNestedPrivate;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsNestedPrivate on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNestedPublic(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsNestedPublic;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsNestedPublic on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNotPublic(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsNotPublic;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsNotPublic on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsPointer(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsPointer;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsPointer on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsPrimitive(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsPrimitive;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsPrimitive on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsPublic(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsPublic;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsPublic on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsSealed(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsSealed;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsSealed on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsSerializable(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsSerializable;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsSerializable on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsSpecialName(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsSpecialName;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsSpecialName on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsUnicodeClass(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsUnicodeClass;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsUnicodeClass on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsValueType(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsValueType;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsValueType on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_MemberType(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.Reflection.MemberTypes ret;

		try
		{
			ret = obj.MemberType;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index MemberType on a nil value" : e.Message);
		}

		ToLua.Push(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Module(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.Reflection.Module ret = null;

		try
		{
			ret = obj.Module;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index Module on a nil value" : e.Message);
		}

		ToLua.PushObject(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Namespace(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		string ret = null;

		try
		{
			ret = obj.Namespace;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index Namespace on a nil value" : e.Message);
		}

		LuaDLL.lua_pushstring(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ReflectedType(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.Type ret = null;

		try
		{
			ret = obj.ReflectedType;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index ReflectedType on a nil value" : e.Message);
		}

		ToLua.Push(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TypeHandle(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.RuntimeTypeHandle ret;

		try
		{
			ret = obj.TypeHandle;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index TypeHandle on a nil value" : e.Message);
		}

		ToLua.PushValue(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_TypeInitializer(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.Reflection.ConstructorInfo ret = null;

		try
		{
			ret = obj.TypeInitializer;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index TypeInitializer on a nil value" : e.Message);
		}

		ToLua.PushObject(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_UnderlyingSystemType(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.Type ret = null;

		try
		{
			ret = obj.UnderlyingSystemType;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index UnderlyingSystemType on a nil value" : e.Message);
		}

		ToLua.Push(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_ContainsGenericParameters(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.ContainsGenericParameters;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index ContainsGenericParameters on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsGenericTypeDefinition(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsGenericTypeDefinition;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsGenericTypeDefinition on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsGenericType(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsGenericType;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsGenericType on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsGenericParameter(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsGenericParameter;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsGenericParameter on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsNested(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsNested;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsNested on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_IsVisible(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		bool ret;

		try
		{
			ret = obj.IsVisible;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index IsVisible on a nil value" : e.Message);
		}

		LuaDLL.lua_pushboolean(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GenericParameterPosition(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		int ret;

		try
		{
			ret = obj.GenericParameterPosition;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index GenericParameterPosition on a nil value" : e.Message);
		}

		LuaDLL.lua_pushinteger(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_GenericParameterAttributes(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.Reflection.GenericParameterAttributes ret;

		try
		{
			ret = obj.GenericParameterAttributes;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index GenericParameterAttributes on a nil value" : e.Message);
		}

		ToLua.Push(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_DeclaringMethod(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.Reflection.MethodBase ret = null;

		try
		{
			ret = obj.DeclaringMethod;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index DeclaringMethod on a nil value" : e.Message);
		}

		ToLua.PushObject(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_StructLayoutAttribute(IntPtr L)
	{
		System.Type obj = (System.Type)ToLua.ToObject(L, 1);
		System.Runtime.InteropServices.StructLayoutAttribute ret = null;

		try
		{
			ret = obj.StructLayoutAttribute;
		}
		catch(Exception e)
		{
			return LuaDLL.luaL_error(L, obj == null ? "attempt to index StructLayoutAttribute on a nil value" : e.Message);
		}

		ToLua.PushObject(L, ret);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_out(IntPtr L)
	{
		ToLua.PushOut<System.Type>(L, new LuaOut<System.Type>());
		return 1;
	}
}

