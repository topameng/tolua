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

namespace LuaInterface
{
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
            luaState.LuaPop(1);
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
                LuaDLL.lua_settop(L, 0);
                throw new LuaException(error);
            }

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
}