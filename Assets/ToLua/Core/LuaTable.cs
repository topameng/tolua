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
using System.Collections;
using System.Collections.Generic;

namespace LuaInterface
{
    public class LuaTable : LuaBaseRef
    {        
        public LuaTable(int reference, LuaState state)
        {
            this.reference = reference;
            this.luaState = state;            
        }

        public object this[string key]
        {
            get
            {
                int oldTop = luaState.LuaGetTop();

                try
                {
                    luaState.Push(this);
                    luaState.Push(key);
                    luaState.LuaGetTable(-2);
                    object ret = luaState.ToVariant(-1);
                    luaState.LuaSetTop(oldTop);
                    return ret;
                }
                catch (Exception e)
                {
                    luaState.LuaSetTop(oldTop);
                    throw e;                    
                }                
            }

            set
            {
                int oldTop = luaState.LuaGetTop();

                try
                {
                    luaState.Push(this);
                    luaState.Push(key);
                    luaState.Push(value);
                    luaState.LuaSetTable(-3);
                    luaState.LuaSetTop(oldTop);
                }
                catch (Exception e)
                {
                    luaState.LuaSetTop(oldTop);
                    throw e;
                }
            }
        }

        public object this[int key]
        {
            get
            {
                int oldTop = luaState.LuaGetTop();

                try
                {
                    luaState.Push(this);
                    luaState.LuaRawGetI(-1, key);
                    object obj = luaState.ToVariant(-1);
                    luaState.LuaSetTop(oldTop);
                    return obj;
                }
                catch (Exception e)
                {
                    luaState.LuaSetTop(oldTop);
                    throw e;
                }
            }

            set
            {
                int oldTop = luaState.LuaGetTop();

                try
                {
                    luaState.Push(this);
                    luaState.Push(value);
                    luaState.LuaRawSetI(-2, key);
                    luaState.LuaSetTop(oldTop);
                }
                catch (Exception e)
                {
                    luaState.LuaSetTop(oldTop);
                    throw e;
                }
            }
        }

        public int Length
        {
            get
            {
                luaState.Push(this);
                int n = luaState.LuaObjLen(-1);
                luaState.LuaPop(1);
                return n;
            }
        }

        public LuaFunction RawGetLuaFunction(string key)
        {            
            int top = luaState.LuaGetTop();

            try
            {
                luaState.Push(this);
                luaState.Push(key);
                luaState.LuaRawGet(-2);
                LuaFunction func = luaState.CheckLuaFunction(-1);
                luaState.LuaSetTop(top);

                if (func != null)
                {
                    func.name = name + "." + key;
                }

                return func;
            }
            catch(Exception e)            
            {
                luaState.LuaSetTop(top);
                throw e;
            }
        }

        public LuaFunction GetLuaFunction(string key)
        {
            int top = luaState.LuaGetTop();

            try
            {
                luaState.Push(this);
                luaState.Push(key);
                luaState.LuaGetTable(-2);
                LuaFunction func = luaState.CheckLuaFunction(-1);
                luaState.LuaSetTop(top);

                if (func != null)
                {
                    func.name = name + "." + key;
                }

                return func;
            }
            catch(Exception e)
            {
                luaState.LuaSetTop(top);
                throw e;
            }
        }

        public string GetStringField(string name)
        {
            int oldTop = luaState.LuaGetTop();
     
            try
            {
                luaState.Push(this);
                luaState.LuaGetField(-1, name);
                string str = luaState.CheckString(-1);
                luaState.LuaSetTop(oldTop);
                return str;
            }
            catch(LuaException e)
            {
                luaState.LuaSetTop(oldTop);
                throw e;
            }
        }

        public void AddTable(string name)
        {
            int oldTop = luaState.LuaGetTop();

            try
            {
                luaState.Push(this);
                luaState.Push(name);
                luaState.LuaCreateTable();                
                luaState.LuaRawSet(-3);
                luaState.LuaSetTop(oldTop);
            }
            catch (Exception e)
            {
                luaState.LuaSetTop(oldTop);
                throw e;
            }
        }

        public object[] ToArray()
        {
            int oldTop = luaState.LuaGetTop();

            try
            {
                luaState.Push(this);
                int len = luaState.LuaObjLen(-1);
                List<object> list = new List<object>(len + 1);
                int index = 1;
                object obj = null;

                while(index <= len)
                {
                    luaState.LuaRawGetI(-1, index++);
                    obj = luaState.ToVariant(-1);
                    luaState.LuaPop(1);
                    list.Add(obj);
                }                

                luaState.LuaSetTop(oldTop);
                return list.ToArray();
            }
            catch (Exception e)
            {
                luaState.LuaSetTop(oldTop);
                throw e;
            }
        }

        public override string ToString()
        {
            luaState.Push(this);
            IntPtr p = luaState.LuaToPointer(-1);
            luaState.LuaPop(1);
            return string.Format("table:0x{0}", p.ToString("X"));            
        }

        public LuaArrayTable ToArrayTable()
        {            
            return new LuaArrayTable(this);
        }

        public LuaDictTable ToDictTable()
        {
            return new LuaDictTable(this);
        }        

        public LuaTable GetMetaTable()
        {            
            int oldTop = luaState.LuaGetTop();

            try
            {
                LuaTable t = null;
                luaState.Push(this);

                if (luaState.LuaGetMetaTable(-1) != 0)
                {
                    t = luaState.CheckLuaTable(-1);
                }

                luaState.LuaSetTop(oldTop);
                return t;
            }
            catch (Exception e)
            {
                luaState.LuaSetTop(oldTop);
                throw e;
            }
        }
    }

    public class LuaArrayTable : IDisposable, IEnumerable<object>
    {       
        private LuaTable table = null;
        private LuaState state = null;

        public LuaArrayTable(LuaTable table)           
        {
            table.AddRef();
            this.table = table;            
            this.state = table.GetLuaState();
        }

        public void Dispose()
        {
            if (table != null)
            {
                table.Dispose();
                table = null;
            }
        }

        public int Length
        {
            get
            {
                return table.Length;
            }
        }

        public object this[int key]
        {
            get
            {
                return table[key];
            }
            set 
            {
                table[key] = value;
            }
        }

        public void ForEach(Action<object> action)
        {
            using (var iter = this.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    action(iter.Current);
                }                
            }
        }

        public IEnumerator<object> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        class Enumerator : IEnumerator<object>
        {            
            LuaState state;
            int index = 1;
            object current = null;
            int top = -1;

            public Enumerator(LuaArrayTable list)
            {                
                state = list.state;
                top = state.LuaGetTop();
                state.Push(list.table);                
            }

            public object Current
            {
                get
                {
                    return current;
                }
            }

            public bool MoveNext()
            {
                state.LuaRawGetI(-1, index);
                current = state.ToVariant(-1);
                state.LuaPop(1);
                ++index;
                return current == null ? false : true;
            }

            public void Reset()
            {
                index = 1;
                current = null;
            }

            public void Dispose()
            {
                if (state != null)
                {
                    state.LuaSetTop(top);
                    state = null;
                }
            }
        }
    }

    public class LuaDictTable : IDisposable, IEnumerable<DictionaryEntry>
    {
        LuaTable table;
        LuaState state;

        public LuaDictTable(LuaTable table)            
        {
            table.AddRef();
            this.table = table;
            this.state = table.GetLuaState() ;
        }

        public void Dispose()
        {
            if (table != null)
            {
                table.Dispose();
                table = null;
            }
        }

        public object this[string key]
        {
            get
            {
                return table[key];
            }

            set
            {
                table[key] = value;
            }
        }

        public Hashtable ToHashtable()
        {
            Hashtable hash = new Hashtable();
            var iter = GetEnumerator();

            while (iter.MoveNext())
            {
                hash.Add(iter.Current.Key, iter.Current.Value);                
            }

            iter.Dispose();
            return hash;
        }

        public IEnumerator<DictionaryEntry> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        class Enumerator : IEnumerator<DictionaryEntry>
        {            
            LuaState state;                        
            DictionaryEntry current = new DictionaryEntry();
            int top = -1;

            public Enumerator(LuaDictTable list)
            {                
                state = list.state;
                top = state.LuaGetTop();
                state.Push(list.table);
                state.LuaPushNil();                
            }

            public DictionaryEntry Current
            {
                get 
                {
                    return current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public bool MoveNext()
            {
                if (state.LuaNext(-2))
                {
                    current = new DictionaryEntry();
                    current.Key = state.ToVariant(-2);
                    current.Value = state.ToVariant(-1);
                    state.LuaPop(1);
                    return true;
                }
                else
                {
                    current = new DictionaryEntry();
                    return false;
                }                
            }

            public void Reset()
            {
                current = new DictionaryEntry();
            }

            public void Dispose()
            {
                if (state != null)
                {
                    state.LuaSetTop(top);
                    state = null;
                }
            }
        }
    }
}
