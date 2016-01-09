using System;
using System.Collections.Generic;
using LuaInterface;

public static class DelegateFactory
{
	delegate Delegate DelegateValue(LuaFunction func);
	static Dictionary<Type, DelegateValue> dict = new Dictionary<Type, DelegateValue>();

	static DelegateFactory()
	{
		Register();
	}

	[NoToLuaAttribute]
	public static void Register()
	{
		dict.Clear();
	}

    [NoToLuaAttribute]
    public static Delegate CreateDelegate(IntPtr L, Type t, LuaFunction func = null)
    {
        DelegateValue create = null;

        if (!dict.TryGetValue(t, out create))
        {
            LuaDLL.luaL_error(L, string.Format("Delegate {0} not register", LuaMisc.GetTypeName(t)));
            return null;
        }
        
        return create(func);        
    }
    
}

