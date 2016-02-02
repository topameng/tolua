/*
Copyright (c) 2015-2016 topameng(topameng@qq.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.IO;

namespace LuaInterface
{
    public static class LuaStatic
    {
        static public void OpenLibs(IntPtr L)
        {
			LuaDLL.tolua_atpanic(L, Panic);
			LuaDLL.tolua_pushcfunction(L, Print);
            LuaDLL.lua_setglobal(L, "print");
            LuaDLL.tolua_pushcfunction(L, DoFile);
            LuaDLL.lua_setglobal(L, "dofile");

            AddLuaLoader(L);
        }

        static void AddLuaLoader(IntPtr L)
        {
            LuaDLL.lua_getglobal(L, "package");
            LuaDLL.lua_getfield(L, -1, "loaders");
            LuaDLL.tolua_pushcfunction(L, Loader);

            for (int i = LuaDLL.lua_objlen(L, -2) + 1; i > 2; i--)
            {
                LuaDLL.lua_rawgeti(L, -2, i - 1);
                LuaDLL.lua_rawseti(L, -3, i);
            }
            
            LuaDLL.lua_rawseti(L, -2, 2);
            LuaDLL.lua_settop(L, 0);
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int Panic(IntPtr L)
        {
            string reason = String.Format("PANIC: unprotected error in call to Lua API ({0})", LuaDLL.lua_tostring(L, -1));
            throw new Exception(reason);
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int Print(IntPtr L)
        {
            try
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
            catch (Exception e)
            {
                return LuaDLL.toluaL_exception(L, e);
            }
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int Loader(IntPtr L)
        {
            try
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
            catch (Exception e)
            {
                return LuaDLL.toluaL_exception(L, e);                
            }
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        public static int DoFile(IntPtr L)
        {
            try
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
                    if (LuaDLL.lua_pcall(L, 0, LuaDLL.LUA_MULTRET, 0) != 0)
                    {
                        string error = LuaDLL.lua_tostring(L, -1);
                        throw new LuaException(error);
                    }
                }

                return LuaDLL.lua_gettop(L) - n;
            }
            catch (Exception e)
            {
                return LuaDLL.toluaL_exception(L, e);
            }
        }
    }
}