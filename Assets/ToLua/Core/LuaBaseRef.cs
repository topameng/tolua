/*
Copyright (c) 2015-2021 topameng(topameng@qq.com)
https://github.com/topameng/tolua

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
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace LuaInterface
{
    public abstract class LuaBaseRef : IDisposable
    {
        public string name = null;
        protected int reference = -1;
        protected LuaState luaState;
        protected ObjectTranslator translator = null;

        protected bool disposed = false;
        protected int count = 0;

        protected LuaTypes type = LuaTypes.LUA_TNONE;

        public LuaBaseRef(int luaRef, LuaState state)
        {
            reference = luaRef;
            luaState = state;            
            count = 1;
        }

        ~LuaBaseRef()
        {            
            Dispose(false);
        }

        public virtual void Dispose()
        {
            --count;
            //Debugger.Log("{0} {1} SubRef to {2}", type == LuaTypes.LUA_TFUNCTION ? "Luafunction" : "LuaTable", reference, count);

            if (count > 0)
            {
                return;
            }
            
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void AddRef()
        {            
            ++count;
            //Debugger.Log("{0} {1}AddRef to {2}", type == LuaTypes.LUA_TFUNCTION ? "Luafunction" : "LuaTable", reference, count);
        }

        public virtual void Dispose(bool disposeManagedResources)
        {
            if (!disposed)
            {                
                if (disposeManagedResources)
                {
                    luaState.CollectByMain(reference, name, type == LuaTypes.LUA_TFUNCTION);
                }
                else
                {
                    luaState.AddToGCList(reference, name, type == LuaTypes.LUA_TFUNCTION);
                }

                luaState = null;
                count = 0;
                disposed = true;
            }
        }

        //慎用
        public void Dispose(int generation)
        {                         
            if (count > generation)
            {
                return;
            }

            Dispose(true);
        }

        public LuaState GetLuaState()
        {
            return luaState;
        }

        public void Push()
        {
            luaState.Push(this);
        }

        public virtual int GetReference()
        {
            return reference;
        }

        public override bool Equals(object o)
        {
            if (o == null)
            {
                return luaState == null;
            }

            if (System.Object.ReferenceEquals(this, o))
            {
                return true;
            }

            LuaBaseRef r = o as LuaBaseRef;      
            
            if (r == null || r.reference != reference || r.luaState != luaState)
            {
                return false;
            }

            return true;
        }        

        public override int GetHashCode()
        {
            return reference;
        }

        static bool Equals(LuaBaseRef a, LuaBaseRef b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            object l = a;
            object r = b;

            if (l == null && r != null)
            {
                return b.luaState == null;
            }

            if (l != null && r == null)
            {
                return a.luaState == null;
            }

            if (a.reference != b.reference || a.luaState != b.luaState)
            {
                return false;
            }

            return true;
        }

        public static bool operator == (LuaBaseRef a, LuaBaseRef b)
        {
            return Equals(a, b);
        }

        public static bool operator != (LuaBaseRef a, LuaBaseRef b)
        {
            return !Equals(a, b);
        }               
    }
}