using System;
using LuaInterface;

namespace LuaInterface
{    
    public class LuaEvent : IDisposable
    {
        protected LuaState luaState;
        protected bool beDisposed;

        LuaTable self = null;
        LuaFunction _add = null;
        LuaFunction _remove = null;
        //LuaFunction _call = null;

        public LuaEvent(LuaTable table)            
        {
            self = table;
            luaState = table.GetLuaState();
            self.AddRef();

            _add = self.RawGetLuaFunction("Add");
            _remove = self.RawGetLuaFunction("Remove");
            //_call = self.RawGetLuaFunction("__call");            
        }

        public void Dispose()
        {
            self.Dispose();            
            _add.Dispose();
            _remove.Dispose();
            //_call.Dispose();
            Clear();
        }

        void Clear()
        {
            //_call = null;
            _add = null;
            _remove = null;
            self = null;            
            luaState = null;
        }

        public void Dispose(bool disposeManagedResources)
        {
            if (!beDisposed)
            {
                beDisposed = true;

                //if (_call != null)
                //{
                //    _call.Dispose(disposeManagedResources);
                //    _call = null;
                //}

                if (_add != null)
                {
                    _add.Dispose(disposeManagedResources);
                    _add = null;
                }

                if (_remove != null)
                {
                    _remove.Dispose(disposeManagedResources);
                    _remove = null;
                }

                if (self != null)
                {
                    self.Dispose(disposeManagedResources);
                }

                Clear();
            }
        }

        public void Add(LuaFunction func, LuaTable obj)
        {
            if (func == null)
            {
                return;
            }

            _add.BeginPCall();
            _add.Push(self);
            _add.Push(func);
            _add.Push(obj);
            _add.PCall();
            _add.EndPCall();
        }

        public void Remove(LuaFunction func, LuaTable obj)
        {
            if (func == null)
            {
                return;
            }

            _remove.BeginPCall();
            _remove.Push(self);
            _remove.Push(func);
            _remove.Push(obj);
            _remove.PCall();
            _remove.EndPCall();
        }

        //public override int GetReference()
        //{
        //    return self.GetReference();
        //}
    }
}
