using System;
using LuaInterface;

public class System_NullObjectWrap
{
    public static void Register(LuaState L)
    {
        L.BeginClass(typeof(NullObject), null);
        L.RegFunction("__eq", op_Equality);
        L.EndClass();
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int op_Equality(IntPtr L)
    {
        LuaDLL.lua_pushboolean(L, true);
        return 1;
    }
}
