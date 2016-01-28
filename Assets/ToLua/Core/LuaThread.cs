using System;
using LuaInterface;

public class LuaThread : LuaBaseRef
{
    object[] objs = null;

    public LuaThread(int reference, LuaState state)
    {
        this.luaState = state;
        this.reference = reference;
    }

    public int Resume(params object[] args)
    {
        objs = null;
        int top = 0;
        luaState.Push(this);                
        IntPtr L = luaState.LuaToThread(-1);
        int nArgs = 0;
        
        if (args != null)
        {
            nArgs = args.Length;

            for (int i = 0; i < args.Length; i++)
            {
                ToLua.Push(L, args[i]);
            }
        }
        
        int ret = LuaDLL.lua_resume(L, nArgs);

        if (ret > (int)LuaThreadStatus.LUA_YIELD)
        {
            string error = null;
            top = LuaDLL.lua_gettop(L);
            LuaDLL.tolua_pushtraceback(L);
            LuaDLL.lua_pushthread(L);
            LuaDLL.lua_pushvalue(L, -3);

            if (LuaDLL.lua_pcall(L, 2, -1, 0) != 0)
            {
                LuaDLL.lua_settop(L, top);
            }

            error = LuaDLL.lua_tostring(L, -1);
            luaState.LuaPop(1);
            throw new LuaException(error);       
        }

        luaState.LuaPop(1);
        top = LuaDLL.lua_gettop(L);

        if (top > 0)
        {
            objs = new object[top];

            for (int i = 0; i < top; i++)
            {
                objs[i] = ToLua.ToVarObject(L, i + 1);
            }
        }

        if (ret == 0)
        {            
            Dispose();
        }
        
        return ret;
    }

    public object[] GetResult()
    {
        return objs;
    }
}
