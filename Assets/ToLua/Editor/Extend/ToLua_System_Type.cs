using System;
using System.Reflection;
using LuaInterface;

public class ToLua_System_Type 
{
    [NoToLuaAttribute]
    public EventInfo GetEvent(string name)
    {
        return null;
    }

    [NoToLuaAttribute]
    public EventInfo[] GetEvents()
    {
        return null;
    }

    [NoToLuaAttribute]
    public MethodInfo GetMethod(string name)
    {
        return null;
    }

    [NoToLuaAttribute]
    public MethodInfo[] GetMethods()
    {
        return null;
    }

    [NoToLuaAttribute]
    public PropertyInfo GetProperty(string name)
    {
        return null;
    }

    [NoToLuaAttribute]
    public PropertyInfo[] GetProperties()
    {
        return null;
    }

    [NoToLuaAttribute]
    public FieldInfo GetField(string name)
    {
        return null;
    }

    [NoToLuaAttribute]
    public FieldInfo[] GetFields()
    {
        return null;
    }

    [NoToLuaAttribute]
    public ConstructorInfo GetConstructor(Type[] types)
    {
        return null;
    }

    [NoToLuaAttribute]
    public ConstructorInfo[] GetConstructors()
    {
        return null;
    }

    [NoToLuaAttribute]
    public MemberInfo[] GetMember(string name)
    {
        return null;
    }

    [NoToLuaAttribute]
    public MemberInfo[] GetMembers()
    {
        return null;
    }
}
