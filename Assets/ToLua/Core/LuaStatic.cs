using System;
using System.IO;

namespace LuaInterface
{
    public static class LuaStatic
    {       
        static public void OpenLibs(IntPtr L)
        {
            LuaDLL.lua_atpanic(L, Panic);
            LuaDLL.lua_pushstdcallcfunction(L, Print);
            LuaDLL.lua_setfield(L, LuaIndexes.LUA_GLOBALSINDEX, "print");
            LuaDLL.lua_pushstdcallcfunction(L, DoFile);
            LuaDLL.lua_setfield(L, LuaIndexes.LUA_GLOBALSINDEX, "dofile");

//#if UNITY_EDITOR
            //AddLuaLoader(L);
//#else   
            AddLuaLoader2(L);
//#endif
        }

        static void AddLuaLoader(IntPtr L)
        {
            LuaDLL.lua_getglobal(L, "package");
            LuaDLL.lua_getfield(L, -1, "loaders");
            int pos = LuaDLL.lua_objlen(L, -1) + 1;            
            LuaDLL.lua_pushstdcallcfunction(L, Loader);
            LuaDLL.lua_rawseti(L, -2, pos);
            LuaDLL.lua_setfield(L, -2, "loaders");
            LuaDLL.lua_pop(L, 1);
        }

        static void AddLuaLoader2(IntPtr L)
        {
            LuaDLL.lua_getglobal(L, "package");
            LuaDLL.lua_getfield(L, -1, "loaders");
            int loaderTable = LuaDLL.lua_gettop(L);

            for (int i = LuaDLL.lua_objlen(L, loaderTable) + 1; i > 1; i--)
            {
                LuaDLL.lua_rawgeti(L, loaderTable, i - 1);
                LuaDLL.lua_rawseti(L, loaderTable, i);
            }

            LuaDLL.lua_pushstdcallcfunction(L, Loader);
            LuaDLL.lua_rawseti(L, loaderTable, 1);

            LuaDLL.lua_settop(L, 0);
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int Panic(IntPtr L)
        {
            string reason = String.Format("PANIC: unprotected error in call to Lua API ({0})", LuaDLL.lua_tostring(L, -1));
            throw new LuaException(reason);
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int Print(IntPtr L)
        {
            int n = LuaDLL.lua_gettop(L);
            string s = String.Empty;

            for (int i = 1; i <= n; i++)
            {
                if (i > 1) s += "    ";

                if (LuaDLL.lua_isstring(L, i) == 1)
                {
                    s += LuaDLL.lua_tostring(L, i);
                }
                else if (LuaDLL.lua_isnil(L, i))
                {
                    s += "nil";
                }
                else if (LuaDLL.lua_isboolean(L, i))
                {
                    s += LuaDLL.lua_toboolean(L, i) ? "true" : "false";
                }
                else
                {
                    IntPtr p = LuaDLL.lua_topointer(L, i);

                    if (p == IntPtr.Zero)
                    {
                        s += "nil";
                    }
                    else
                    {
                        s += string.Format("{0}:0x{1}", LuaDLL.luaL_typename(L, i), p.ToString("X"));
                    }
                }
            }

            Debugger.Log(s);
            return 0;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int Loader(IntPtr L)
        {
            string fileName = LuaDLL.lua_tostring(L, 1);

            if (!Path.HasExtension(fileName))
            {
                fileName += ".lua";
            }

            byte[] buffer = LuaFileUtils.Instance.ReadFile(fileName);

            if (buffer == null)
            {
                return 0;
            }

            LuaDLL.luaL_loadbuffer(L, buffer, buffer.Length, fileName);
            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public static int DoFile(IntPtr L)
        {                        
            string fileName = LuaDLL.lua_tostring(L, 1);

            if (!Path.HasExtension(fileName))
            {
                fileName += ".lua";
            }

            int n = LuaDLL.lua_gettop(L);
            byte[] text = LuaFileUtils.Instance.ReadFile(fileName);

            if (text == null)
            {
                return 0;
            }

            if (LuaDLL.luaL_loadbuffer(L, text, text.Length, fileName) == 0)
            {
                LuaDLL.lua_call(L, 0, LuaDLL.LUA_MULTRET);
            }

            return LuaDLL.lua_gettop(L) - n;
        }
    }
}