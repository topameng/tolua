using System;
using LuaInterface;

public class LuaInterface_LuaOutWrap
{
    public static void Register(LuaState L)
    {
        //L.BeginClass(typeof(LuaInterface.LuaOutMetatable), null);        
        //L.EndClass();
        L.BeginPreLoad();
        L.RegFunction("tolua.out", LuaOpen_ToLua_Out);                   
        L.EndPreLoad();
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int LuaOpen_ToLua_Out(IntPtr L)
    {                
        LuaDLL.lua_newtable(L);

        RawSetOutType<int>(L);
        RawSetOutType<uint>(L);
        RawSetOutType<float>(L);
        RawSetOutType<double>(L);
        RawSetOutType<long>(L);
        RawSetOutType<ulong>(L);
        RawSetOutType<byte>(L);
        RawSetOutType<sbyte>(L);
        RawSetOutType<char>(L);
        RawSetOutType<short>(L);
        RawSetOutType<ushort>(L);
        RawSetOutType<bool>(L);
        
        return 1;
    }

    static void RawSetOutType<T>(IntPtr L)
    {
        string str = LuaMisc.GetPrimitiveStr(typeof(T));
        LuaDLL.lua_pushstring(L, str);
        ToLua.PushOut<T>(L, new LuaOut<T>());
        LuaDLL.lua_rawset(L, -3);
    }
}
