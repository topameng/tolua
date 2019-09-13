using System;
using LuaInterface;

public class ToLua_System_Delegate
{    
    public static string AdditionNameSpace = "System.Collections.Generic";

    [NoToLuaAttribute]
    public static string op_AdditionDefined =
@"        try
        {
            LuaTypes type = LuaDLL.lua_type(L, 1);

            switch (type)
            {
                case LuaTypes.LUA_TFUNCTION:
                    Delegate arg0 = (Delegate)ToLua.ToObject(L, 2);
                    LuaFunction func = ToLua.ToLuaFunction(L, 1);
                    Type t = arg0.GetType();
                    Delegate arg1 = DelegateFactory.CreateDelegate(t, func);
                    Delegate arg2 = Delegate.Combine(arg0, arg1);
                    arg0.AddRef();
                    ToLua.Push(L, arg2);
                    func.Dispose();
                    return 1;
                case LuaTypes.LUA_TNIL:
                    LuaDLL.lua_pushvalue(L, 2);
                    return 1;
                case LuaTypes.LUA_TUSERDATA:
                    Delegate a0 = (Delegate)ToLua.ToObject(L, 1);
                    Delegate a1 = ToLua.CheckDelegate(a0.GetType(), L, 2);
                    Delegate ret = Delegate.Combine(a0, a1);

                    if (LuaDLL.lua_type(L, 2) == LuaTypes.LUA_TFUNCTION)
                    {
                        a0.AddRef();//如果是Lua函数已经加1
                    }
                    else
                    {
                        ret.AddRef();
                    }

                    ToLua.Push(L, ret);
                    return 1;
                default:
                    LuaDLL.luaL_typerror(L, 1, ""Delegate"");
                    return 1;
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }";

    [NoToLuaAttribute]
    public static string op_SubtractionDefined =
@"        try
        {            
            Delegate arg0 = (Delegate)ToLua.ToObject(L, 1);         //不能增加引用计数，否则自身包含的都会增加
            LuaTypes type = LuaDLL.lua_type(L, 2);

            if (type == LuaTypes.LUA_TFUNCTION)
            {                
                LuaFunction func = ToLua.ToLuaFunction(L, 2);
                Delegate[] ds = arg0.GetInvocationList();

                for (int i = 0; i < ds.Length; i++)
                {
                    LuaDelegate target = ds[i].Target as LuaDelegate;

                    if (target != null && target.func == func && target.self == null)
                    {
                        arg0 = Delegate.Remove(arg0, ds[i]);
                        if (arg0 != null) arg0.AddRef();                              //产生新的委托需要增加计数                        
                        break;
                    }
                }

                func.Dispose();
                ToLua.Push(L, arg0);
                return 1;
            }
            else
            {
                Delegate arg1 = (Delegate)ToLua.CheckObject(L, 2);
                arg0 = DelegateFactory.RemoveDelegate(arg0, arg1);                
                ToLua.Push(L, arg0);
                
                return 1;
            }
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }";

    public static bool operator ==(ToLua_System_Delegate lhs, ToLua_System_Delegate rhs)
    {
        return false;
    }

    public static bool operator !=(ToLua_System_Delegate lhs, ToLua_System_Delegate rhs)
    {
        return false;
    }

    [UseDefinedAttribute]
    public static ToLua_System_Delegate operator +(ToLua_System_Delegate a, ToLua_System_Delegate b)
    {
        return null;
    }

    [UseDefinedAttribute]
    public static ToLua_System_Delegate operator -(ToLua_System_Delegate a, ToLua_System_Delegate b)
    {
        return null;
    }


    public override bool Equals(object other)
    {
        return false;
    }

    public override int GetHashCode()
    {
        return 0;
    }

    public static string DestroyDefined =
@"        Delegate arg0 = (Delegate)ToLua.CheckObject<Delegate>(L, 1);
        Delegate[] ds = arg0.GetInvocationList();

        for (int i = 0; i < ds.Length; i++)
        {
            LuaDelegate ld = ds[i].Target as LuaDelegate;

            if (ld != null)
            {                
                ld.DelayDispose();                
            }
        }

        return 0;";

    [UseDefinedAttribute]
    public static void Destroy(object obj)
    {
    }
}
