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
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Runtime.CompilerServices;

namespace LuaInterface
{
    public struct GCRef
    {
        public int reference;
        public string name;
        public bool beFunction;

        public GCRef(int reference, string name, bool befunc)
        {
            this.reference = reference;
            this.name = name;
            this.beFunction = befunc;
        }
    }

    //让byte[] 压入成为lua string 而不是数组 userdata
    //也可以使用LuaByteBufferAttribute来标记byte[]
    public struct LuaByteBuffer
    {        
        public LuaByteBuffer(IntPtr source, int len)
            : this()            
        {
            buffer = new byte[len];
            Length = len;
            Marshal.Copy(source, buffer, 0, len);
        }
        
        public LuaByteBuffer(byte[] buf)
            : this()
        {
            buffer = buf;
            Length = buf.Length;            
        }

        public LuaByteBuffer(byte[] buf, int len)
            : this()
        {            
            buffer = buf;
            Length = len;
        }

        public LuaByteBuffer(System.IO.MemoryStream stream)   
            : this()         
        {
            buffer = stream.GetBuffer();
            Length = (int)stream.Length;            
        }

        public static implicit operator LuaByteBuffer(System.IO.MemoryStream stream)
        {
            return new LuaByteBuffer(stream);
        }

        public byte[] buffer;    

        public int Length
        {
            get;
            private set;
        }    
    }   

    public class LuaOut<T> { }
    //public class LuaOutMetatable {}
    public class NullObject { }

    //泛型函数参数null代替
    public struct nil { }

    public class LuaDelegate
    {
        public LuaFunction func = null;
        public LuaTable self = null;
        public MethodInfo method = null;

        private int count = 0;
        private int reference = 0;

        public LuaDelegate(LuaFunction func)
        {
            this.func = func;
            this.func.AddRef();
            reference = func.GetReference();
            self = null;
            method = null;
            count = 1;
        }

        public LuaDelegate(LuaFunction func, LuaTable self)
        {
            this.func = func;
            this.func.AddRef();
            this.self = self;
            this.self.AddRef();
            reference = func.GetReference();
            method = null;
            count = 1;
        }

        public void AddRef()
        {
            ++count;
            //Debugger.Log("LuaDelegate {0} AddRef {1}", reference, count);
            func.AddRef();            

            if (self != null)
            {
                self.AddRef();
            }
        }
        
        public void Dispose()
        {
            --count;
            //Debugger.Log("LuaDelegate {0} SubRef {1}", reference, count);
            
            func.Dispose();

            if (self != null)
            {                
                self.Dispose();
            }

            if (count == 0)
            {
                method = null;
                func = null;
                self = null;
            }
        }

        public void DelayDispose()
        {
            if (count <= 0)
            {
                throw new LuaException(string.Format("LuaDelegate {0} Dispose more than reference count", reference));
            }

            LuaState state = func.GetLuaState();
            state.DelayDispose(this);
        }

        public override bool Equals(object o)
        {
            if (o == null)
            {
                return func == null;
            }

            LuaDelegate ld = o as LuaDelegate;

            if (ld == null || ld.func != func || ld.self != self)
            {
                return false;
            }

            return reference == ld.reference;
        }

        static bool Equals(LuaDelegate a, LuaDelegate b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            object l = a;
            object r = b;

            if (l == null && r != null)
            {
                return b.func == null;
            }

            if (l != null && r == null)
            {
                return a.func == null;
            }

            if (a.func != b.func || a.self != b.self)
            {
                return false;
            }

            return a.reference == b.reference;
        }

        public static bool operator == (LuaDelegate a, LuaDelegate b)
        {
            return Equals(a, b);
        }

        public static bool operator != (LuaDelegate a, LuaDelegate b)
        {
            return !Equals(a, b);
        }        

        public override int GetHashCode()
        {
            return reference;
        }
    }

    [NoToLuaAttribute]
    public static class LuaMisc
    {
        static readonly Il2cppType il2cpp = new Il2cppType();

        public static string GetArrayRank(Type t)
        {
            int count = t.GetArrayRank();

            if (count == 1)
            {                
                return "[]";
            }

            using (CString.Block())
            {
                CString sb = CString.Alloc(64);
                sb.Append('[');

                for (int i = 1; i < count; i++)
                {
                    sb.Append(',');
                }

                sb.Append(']');
                return sb.ToString();
            }
        }

        public static string GetTypeName(Type t)
        {
            if (t.IsArray)
            {
                string str = GetTypeName(t.GetElementType());
                str += GetArrayRank(t);
                return str;                
            }
            else if (t.IsByRef)
            {
                t = t.GetElementType();
                return GetTypeName(t);
            }
            else if (t.IsGenericType)
            {
                return GetGenericName(t);
            }
            else if (t == il2cpp.TypeOfVoid)
            {
                return "void";
            }
            else
            {
                string name = GetPrimitiveStr(t);
                return name.Replace('+', '.');
            }
        }

        public static string[] GetGenericName(Type[] types, int offset, int count)
        {
            string[] results = new string[count];

            for (int i = 0; i < count; i++)
            {
                int pos = i + offset;

                if (types[pos].IsGenericType)
                {
                    results[i] = GetGenericName(types[pos]);
                }
                else
                {
                    results[i] = GetTypeName(types[pos]);
                }

            }

            return results;
        }

        static string CombineTypeStr(string space, string name)
        {
            if (string.IsNullOrEmpty(space))
            {
                return name;
            }
            else
            {
                return space + "." + name;
            }
        }

        static string GetGenericName(Type t)
        {
            Type[] gArgs = t.GetGenericArguments();
            string typeName = t.FullName ?? t.Name;
            int count = gArgs.Length;
            int pos = typeName.IndexOf("[");

            if (pos > 0)
            {
                typeName = typeName.Substring(0, pos);
            }

            string str = null;
            string name = null;
            int offset = 0;
            pos = typeName.IndexOf("+");

            while (pos > 0)
            {
                str = typeName.Substring(0, pos);
                typeName = typeName.Substring(pos + 1);
                pos = str.IndexOf('`');

                if (pos > 0)
                {
                    count = (int)(str[pos + 1] - '0');
                    str = str.Substring(0, pos);
                    str += "<" + string.Join(",", GetGenericName(gArgs, offset, count)) + ">";
                    offset += count;
                }

                name = CombineTypeStr(name, str);
                pos = typeName.IndexOf("+");
            }

            str = typeName;

            if (offset < gArgs.Length)
            {
                pos = str.IndexOf('`');
                count = (int)(str[pos + 1] - '0');
                str = str.Substring(0, pos);
                str += "<" + string.Join(",", GetGenericName(gArgs, offset, count)) + ">";
            }

            return CombineTypeStr(name, str);
        }

        public static Delegate GetEventHandler(object obj, Type t, string eventName)
        {
            FieldInfo eventField = t.GetField(eventName, BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            return (Delegate)eventField.GetValue(obj);
        }

        public static string GetPrimitiveStr(Type t)
        {
            Il2cppType il = il2cpp;

            if (t == il.TypeOfFloat)
            {
                return "float";
            }
            else if (t == il.TypeOfString)
            {
                return "string";
            }
            else if (t == il.TypeOfInt)
            {
                return "int";
            }
            else if (t == il.TypeOfDouble)
            {
                return "double";
            }
            else if (t == il.TypeOfBool)
            {
                return "bool";
            }
            else if (t == il.TypeOfUInt)
            {
                return "uint";
            }
            else if (t == il.TypeOfSByte)
            {
                return "sbyte";
            }
            else if (t == il.TypeOfByte)
            {
                return "byte";
            }
            else if (t == il.TypeOfShort)
            {
                return "short";
            }
            else if (t == il.TypeOfUShort)
            {
                return "ushort";
            }
            else if (t == il.TypeOfChar)
            {
                return "char";
            }
            else if (t == il.TypeOfLong)
            {
                return "long";
            }
            else if (t == il.TypeOfULong)
            {
                return "ulong";
            }
            else if (t == il.TypeOfDecimal)
            {
                return "decimal";
            }
            else if (t == il.TypeOfObject)
            {
                return "object";
            }
            else
            {
                return t.ToString();
            }
        }        

        //可产生导出文件的基类
        public static Type GetExportBaseType(Type t)
        {
            Type baseType = t.BaseType;

            if (baseType == il2cpp.TypeOfStruct)
            {
                return null;
            }

            if (t.IsAbstract && t.IsSealed)
            {
                return baseType == il2cpp.TypeOfObject ? null : baseType;
            }

            return baseType;
        }
    }       

    /*[NoToLuaAttribute]
    public struct LuaInteger64
    {
        public long i64;

        public LuaInteger64(long i64)
        {
            this.i64 = i64;
        }

        public static implicit operator LuaInteger64(long i64)
        {
            return new LuaInteger64(i64);
        }

        public static implicit operator long(LuaInteger64 self)
        {
            return self.i64;
        }

        public ulong ToUInt64()
        {
            return (ulong)i64;
        }

        public override string ToString()
        {
            return Convert.ToString(i64);
        }
    }*/

    public class TouchBits
    {
        public const int DeltaPosition = 1;
        public const int Position = 2;
        public const int RawPosition = 4;
        public const int ALL = 7;
    }

    public class RaycastBits
    {
        public const int Collider = 1;
        public const int Normal = 2;
        public const int Point = 4;
        public const int Rigidbody = 8;
        public const int Transform = 16;
        public const int ALL = 31;
    }

    public enum EventOp
    {
        None = 0,
        Add = 1,
        Sub = 2,
    }

    /*因为事件只能加或者减不能set，故使用此类记录加减操作*/
    public class EventObject
    {
        [NoToLuaAttribute]
        public EventOp op = EventOp.None;
        [NoToLuaAttribute]
        public Delegate func = null;
        [NoToLuaAttribute]
        public Type type;

        [NoToLuaAttribute]
        public EventObject(Type t)
        {
            type = t;
        }

        public static EventObject operator +(EventObject a, Delegate b)
        {
            a.op = EventOp.Add;
            a.func = b;
            return a;
        }

        public static EventObject operator -(EventObject a, Delegate b)
        {
            a.op = EventOp.Sub;
            a.func = b;
            return a;
        }
    }
}

