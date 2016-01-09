using System;
using UnityEngine;

namespace LuaInterface
{
    public abstract class LuaBaseRef : IDisposable
    {
        public string name = null;
        protected int reference = 0;
        protected LuaState luaState;
        protected ObjectTranslator translator = null;

        protected bool beDisposed;
        protected int count = 0;

        public LuaBaseRef()
        {
            count = 1;
        }

        ~LuaBaseRef()
        {
            Dispose(false);
        }

        public virtual void Dispose()
        {
            --count;

            if (count > 0)
            {
                return;
            }

            Dispose(true);            
        }

        public void AddRef()
        {
            ++count;            
        }

        public virtual void Dispose(bool disposeManagedResources)
        {
            if (!beDisposed)
            {
                beDisposed = true;   

                if (reference > 0 && luaState != null)
                {
                    luaState.CollectRef(reference, name, !disposeManagedResources);
                }
                
                reference = 0;
                luaState = null;                             
            }            
        }

        public LuaState GetLuaState()
        {
            return luaState;
        }

        public void Push()
        {
            luaState.Push(this);
        }

        public override int GetHashCode()
        {
            return reference;
        }

        public virtual int GetReference()
        {
            return reference;
        }

        public override bool Equals(object o)
        {
            if ((object)o == null) return false;            
            LuaBaseRef lbref = o as LuaBaseRef;
            return lbref != null && lbref.reference == reference;
        }

        public static bool operator == (LuaBaseRef a, LuaBaseRef b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            object l = (object)a;
            object r = b;

            if (l == null)
            {
                return r == null || b.reference == 0;
            }

            if (r == null)
            {
                return a.reference == 0;
            }

            return r != null && a.reference == b.reference;
        }

        public static bool operator != (LuaBaseRef a, LuaBaseRef b)
        {
            return !(a == b);
        }

        public bool IsAlive()
        {
            return !beDisposed;
        }
    }
}