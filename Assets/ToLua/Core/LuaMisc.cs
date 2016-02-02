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
                return t.FullName;
            }
        }

        static string[] GetGenericName(Type[] types)
        {
            string[] results = new string[types.Length];

            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].IsGenericType)
                {
                    results[i] = GetGenericName(types[i]);
                }
                else
                {
                    results[i] = GetTypeName(types[i]);
                }

            }

            return results;
        }

        static string GetGenericName(Type t)
        {
            Type[] gArgs = t.GetGenericArguments();
            string typeName = t.FullName;
            string pureTypeName = typeName.Substring(0, typeName.IndexOf('`'));            

            if (typeName.Contains("+"))
            {
                int pos1 = typeName.IndexOf("+");
                int pos2 = typeName.IndexOf("[");

                if (pos2 > pos1)
                {
                    string add = typeName.Substring(pos1 + 1, pos2 - pos1 - 1);
                    return pureTypeName + "<" + string.Join(",", GetGenericName(gArgs)) + ">." + add;
                }
                else
                {
                    return pureTypeName + "<" + string.Join(",", GetGenericName(gArgs)) + ">";
                }
            }
            else
            {
                return pureTypeName + "<" + string.Join(",", GetGenericName(gArgs)) + ">";
            }
        }

        public static Delegate GetEventHandler(object obj, Type t, string eventName)
        {
            FieldInfo eventField = t.GetField(eventName, BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            return (Delegate)eventField.GetValue(obj);
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

    public enum DestroyFlag
    {
        Destroy = 1,
        DestroyImmediate = 2,
        DestroyObject = 3,
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

