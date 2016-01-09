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
                return luaState.GetObject(reference, key);
            }

            set
            {
                luaState.SetObject(reference, key, value);
            }
        }

        public object this[int key]
        {
            get
            {
                if (key < 1)
                {
                    throw new LuaException("array index out of bounds: {0}", key);                    
                }

                luaState.Push(this);
                object obj = luaState.RawGetI(-1, key);
                luaState.LuaPop(2);
                return obj;
            }

            set
            {
                if (key < 1)
                {                    
                    throw new LuaException("array index out of bounds: {0}", key);                          
                }

                luaState.Push(this);
                luaState.Push(value);
                luaState.LuaRawSetI(-2, key);
                luaState.LuaPop(1);
            }
        }

        public int Length
        {
            get
            {
                return luaState.LuaObjLen(reference);
            }
        }

        public LuaFunction RawGetLuaFunction(string key)
        {
            luaState.Push(this);
            luaState.LuaRawGet(-1, key);
            string error = null;
            LuaFunction func = luaState.CheckLuaFunction(-1, out error);
            luaState.LuaPop(2);

            if (error != null)
            {                
                throw new LuaException(error);
            }

            if (func != null)
            {
                func.name = name + "." + key;
            }

            return func;
        }

        public LuaFunction GetLuaFunction(string key)
        {
            luaState.Push(this);
            luaState.GetTableField(-1, key);
            string error = null;
            LuaFunction func = luaState.CheckLuaFunction(-1, out error);
            luaState.LuaPop(2);

            if (error != null)
            {                
                throw new LuaException(error);
            }

            if (func != null)
            {
                func.name = name + "." + key;
            }

            return func;
        }

        public void AddTable(string name)
        {
            luaState.Push(this);
            luaState.AddTable(-1, name);
            luaState.LuaPop(1);
        }

        public object[] ToArray()
        {            
            return luaState.TableToArray(this);
        }

        public override string ToString()
        {
            return luaState.TableToString(this);
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
            return luaState.GetMetaTable(this);
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

        ~LuaArrayTable()
        {
            table.Dispose(false);
        }

        public void Dispose()
        {
            table.Dispose(true);            
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

            public Enumerator(LuaArrayTable list)
            {                
                state = list.state;
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
                current = state.RawGetI(-1, index);
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
                state.LuaPop(1);
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

        ~LuaDictTable()
        {
            table.Dispose(false);
        }

        public void Dispose()
        {
            table.Dispose(true);            
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

            public Enumerator(LuaDictTable list)
            {                
                state = list.state;
                state.Push(list.table);
                state.Push((object)null);
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
                state.LuaPop(1);
            }
        }
    }
}
