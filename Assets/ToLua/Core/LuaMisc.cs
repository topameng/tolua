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
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Reflection;

namespace LuaInterface
{
    public class GCRef
    {
        public int reference;
        public string name = null;

        public GCRef(int reference, string name)
        {
            this.reference = reference;
            this.name = name;
        }
    }

    public enum LuaValueType
    {
        None = 0,
        Vector3 = 1,
        Quaternion = 2,
        Vector2 = 3,
        Color = 4,
        Vector4 = 5,
        Ray = 6,
        Bounds = 7,
        Touch = 8,
        LayerMask = 9,
        RaycastHit = 10,
    }

    //让byte[] 压入成为lua string 而不是数组 userdata
    public class LuaByteBuffer
    {        
        public LuaByteBuffer(IntPtr source, int len)
        {
            buffer = new byte[len];
            Marshal.Copy(source, buffer, 0, len);
        }
        
        public LuaByteBuffer(byte[] buf)
        {
            this.buffer = buf;
        }

        public override bool Equals(object o)
        {
            if ((object)o == null) return false;
            LuaByteBuffer bb = o as LuaByteBuffer;
            return bb != null && bb.buffer == buffer;
        }

        public static bool operator ==(LuaByteBuffer a, LuaByteBuffer b)
        {
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            object l = (object)a;
            object r = b;

            if (l == null && r != null)
            {
                return b.buffer == null;
            }

            if (l != null && r == null)
            {
                return a.buffer == null;
            }

            return a.buffer == b.buffer;
        }

        public static bool operator !=(LuaByteBuffer a, LuaByteBuffer b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return buffer.GetHashCode();
        }

        public byte[] buffer = null;
    }   

    public class LuaOut<T> { }
    public class LuaOutMetatable { }
    public class NullObject { }

    public class LuaDelegate
    {
        public LuaFunction func = null;

        public LuaDelegate(LuaFunction func)
        {
            this.func = func;
        }
    }

    [NoToLuaAttribute]
    public static class LuaMisc
    {
        public static string GetTypeName(Type t)
        {
            if (t.IsArray)
            {
                t = t.GetElementType();
                string str = GetTypeName(t);
                str += "[]";
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
            else if (t == typeof(void))
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
            string typeName = t.FullName;
            int count = gArgs.Length;
            int pos = typeName.IndexOf("[");
            typeName = typeName.Substring(0, pos);

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
            if (t == typeof(System.Single))
            {
                return "float";
            }
            else if (t == typeof(System.String))
            {
                return "string";
            }
            else if (t == typeof(System.Int32))
            {
                return "int";
            }
            else if (t == typeof(System.Int64))
            {
                return "long";
            }
            else if (t == typeof(System.SByte))
            {
                return "sbyte";
            }
            else if (t == typeof(System.Byte))
            {
                return "byte";
            }
            else if (t == typeof(System.Int16))
            {
                return "short";
            }
            else if (t == typeof(System.UInt16))
            {
                return "ushort";
            }
            else if (t == typeof(System.Char))
            {
                return "char";
            }
            else if (t == typeof(System.UInt32))
            {
                return "uint";
            }
            else if (t == typeof(System.UInt64))
            {
                return "ulong";
            }
            else if (t == typeof(System.Decimal))
            {
                return "decimal";
            }
            else if (t == typeof(System.Double))
            {
                return "double";
            }
            else if (t == typeof(System.Boolean))
            {
                return "bool";
            }
            else if (t == typeof(System.Object))
            {
                return "object";
            }
            else
            {
                return t.ToString();
            }
        }

        public static double ToDouble(object obj)
        {
            Type t = obj.GetType();

            if (t == typeof(double) || t == typeof(float))
            {
                double d = Convert.ToDouble(obj);
                return d;
            }
            else if (t == typeof(int))
            {
                int n = Convert.ToInt32(obj);
                return (double)n;
            }
            else if (t == typeof(uint))
            {
                uint n = Convert.ToUInt32(obj);
                return (double)n;
            }
            else if (t == typeof(long))
            {
                long n = Convert.ToInt64(obj);
                return (double)n;
            }
            else if (t == typeof(ulong))
            {
                ulong n = Convert.ToUInt64(obj);
                return (double)n;
            }
            else if (t == typeof(byte))
            {
                byte b = Convert.ToByte(obj);
                return (double)b;
            }
            else if (t == typeof(sbyte))
            {
                sbyte b = Convert.ToSByte(obj);
                return (double)b;
            }
            else if (t == typeof(char))
            {
                char c = Convert.ToChar(obj);
                return (double)c;
            }            
            else if (t == typeof(short))
            {
                Int16 n = Convert.ToInt16(obj);
                return (double)n;
            }
            else if (t == typeof(ushort))
            {
                UInt16 n = Convert.ToUInt16(obj);
                return (double)n;
            }

            return 0;
        }
    }       

    [NoToLuaAttribute]
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
    }

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

    public class EventObject
    {
        [NoToLuaAttribute]
        public EventOp op = EventOp.None;
        [NoToLuaAttribute]
        public LuaFunction func = null;
        [NoToLuaAttribute]
        public string name = string.Empty;

        [NoToLuaAttribute]
        public EventObject(string name)
        {
            this.name = name;
        }

        public static EventObject operator +(EventObject a, LuaFunction b)
        {
            a.op = EventOp.Add;
            a.func = b;
            return a;
        }

        public static EventObject operator -(EventObject a, LuaFunction b)
        {
            a.op = EventOp.Sub;
            a.func = b;
            return a;
        }

        [NoToLuaAttribute]
        public override string ToString()
        {
            return name;
        }
    }
}

