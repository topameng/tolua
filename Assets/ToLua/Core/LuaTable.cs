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

        const char SplitKey ='.';

        public LuaTable(int reference, LuaState state)
        {
            this.reference = reference;
            this.luaState = state;            
        }
        /// <summary>
        /// Gets or sets a value indicating whether ignore point .
        /// </summary>
        /// <value><c>true</c> if ignore point; otherwise, <c>false</c>.</value>
        public bool ignorePoint {get;set;}

        public object this[string key]
        {
            get
            {
                if(string.IsNullOrEmpty(key)) return null;

                int oldTop = luaState.LuaGetTop();

                object ret  = null;

                try
                {
                    if(ignorePoint)
                    {
                        ret = GetTableVal(key);
                    }
                    else
                    {
                        string[] tableskey= key.Split(SplitKey);

                        if(tableskey != null && tableskey.Length >1)
                            ret = GetMultiTableVal(tableskey);
                        else
                            ret = GetTableVal(key);
                    }

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
                    if(ignorePoint)
                    {
                        SetTable(key,value);
                    }
                    else
                    {
                        string[] tableskey= key.Split(SplitKey);

                        if(tableskey != null && tableskey.Length >1)
                            SetMultiTableVal(tableskey,value);
                        else
                            SetTable(key,value);
                    }

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

        private void SetTable(string key,object value)
        {
            luaState.Push(this);
            luaState.Push(key);
            luaState.Push(value);
            luaState.LuaSetTable(-3);
        }

        private void SetMultiTableVal(string[] tableskey,object value)
        {

            LuaTable subTable =  null ;
            LuaState targetState =luaState;
            LuaBaseRef lbr = this;

            for(int i =0; i < tableskey.Length ;++i)
            {
                string subKey = tableskey[i];

                targetState.Push(lbr);
                targetState.Push(subKey);
                targetState.LuaGetTable(-2);

                int top = targetState.LuaGetTop();

                if(targetState.LuaType(top) == LuaTypes.LUA_TTABLE)
                {
                    object target =targetState.ToVariant(-1);

                    subTable= target as LuaTable;

                    targetState = subTable.luaState;
                    lbr = subTable;

                    if(subTable.ignorePoint)
                    {
                        targetState.Push(lbr);
                        targetState.Push(GetLeftKey(tableskey,i+1));
                        targetState.Push(value);

                        targetState.LuaSetTable(-3);

                        break;
                    }
                }
                else
                {

                    targetState.Push(lbr);
                    targetState.Push(subKey);
                    targetState.Push(value);

                    targetState.LuaSetTable(-3);

                    if(i != tableskey.Length-1)
                    {
                        Debugger.LogWarning("it's not an full table path,So return value earlierly");
                    }

                    break;
                }


            }
        }

        private string GetLeftKey(string[] tablekeys,int index)
        {
            System.Text.StringBuilder sb= StringBuilderCache.Acquire();

            for(int i =index; i < tablekeys.Length;++i)
            {
                sb.Append(tablekeys[i]);
                if(i != tablekeys.Length -1)
                    sb.Append(SplitKey);
            }

            return sb.ToString();
        }

        private object GetTableVal(string key)
        {
            luaState.Push(this);
            luaState.Push(key);
            luaState.LuaGetTable(-2);
            return luaState.ToVariant(-1);
        }

        private object GetMultiTableVal(string[] tableskey)
        {
            if(tableskey.Length < 1)
                return null;

            LuaTable subTable =  null ;
            LuaState targetState =luaState;
            LuaBaseRef lbr = this;

            string subKey = tableskey[0];

            for(int i =0; i < tableskey.Length ;++i)
            {

                targetState.Push(lbr);
                targetState.Push(subKey);
                targetState.LuaGetTable(-2);

                int top = targetState.LuaGetTop();

                object target =targetState.ToVariant(-1);

                if(targetState.LuaType(top) == LuaTypes.LUA_TTABLE)
                {
                   
                    subTable= target as LuaTable;

                    targetState = subTable.luaState;
                    lbr = subTable;

                    if(!subTable.ignorePoint)
                    {
                        if(i+1 < tableskey.Length )
                            subKey =tableskey[i+1];
                    }
                    else
                    {
                        subKey = GetLeftKey(tableskey,i+1);
                        i = tableskey.Length -2;
                    }
                }
                else
                {
 
                    if(i != tableskey.Length-1)
                    {
                        Debugger.LogWarning("it's not an full table path,So return value earlierly");
                    }
                    return target;
                }
                    
            }

            return null;
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
                state.LuaPop(1);
            }
        }
    }
}
