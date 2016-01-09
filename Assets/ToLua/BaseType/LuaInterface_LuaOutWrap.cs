using System;
using LuaInterface;

public class LuaInterface_LuaOutWrap
{
    public static void Register(LuaState L)
    {
        L.BeginClass(typeof(LuaOutMetatable), null);        
        L.EndClass();
    }
}
