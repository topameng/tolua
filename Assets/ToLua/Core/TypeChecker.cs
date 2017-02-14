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
using UnityEngine;

namespace LuaInterface
{
    public static class TypeChecker
    {
        public static bool IsValueType(Type t)
        {
            return !t.IsEnum && t.IsValueType;
        }

        public static bool CheckTypes(IntPtr L, int begin, Type type0)
        {
            return CheckType(L, type0, begin);
        }

        public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1)
        {
            return CheckType(L, type0, begin) && CheckType(L, type1, begin + 1);
        }

        public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2)
        {
            return CheckType(L, type0, begin) && CheckType(L, type1, begin + 1) && CheckType(L, type2, begin + 2);
        }

        public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3)
        {
            return CheckType(L, type0, begin) && CheckType(L, type1, begin + 1) && CheckType(L, type2, begin + 2) && CheckType(L, type3, begin + 3);
        }

        public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3, Type type4)
        {
            return CheckType(L, type0, begin) && CheckType(L, type1, begin + 1) && CheckType(L, type2, begin + 2) && CheckType(L, type3, begin + 3) && CheckType(L, type4, begin + 4);
        }

        public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3, Type type4, Type type5)
        {
            return CheckType(L, type0, begin) && CheckType(L, type1, begin + 1) && CheckType(L, type2, begin + 2) && CheckType(L, type3, begin + 3) && CheckType(L, type4, begin + 4) &&
                   CheckType(L, type5, begin + 5);
        }

        public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6)
        {
            return CheckType(L, type0, begin) && CheckType(L, type1, begin + 1) && CheckType(L, type2, begin + 2) && CheckType(L, type3, begin + 3) && CheckType(L, type4, begin + 4) &&
                   CheckType(L, type5, begin + 5) && CheckType(L, type6, begin + 6);
        }

        public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7)
        {
            return CheckType(L, type0, begin) && CheckType(L, type1, begin + 1) && CheckType(L, type2, begin + 2) && CheckType(L, type3, begin + 3) && CheckType(L, type4, begin + 4) &&
                   CheckType(L, type5, begin + 5) && CheckType(L, type6, begin + 6) && CheckType(L, type7, begin + 7);
        }

        public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7, Type type8)
        {
            return CheckType(L, type0, begin) && CheckType(L, type1, begin + 1) && CheckType(L, type2, begin + 2) && CheckType(L, type3, begin + 3) && CheckType(L, type4, begin + 4) &&
                   CheckType(L, type5, begin + 5) && CheckType(L, type6, begin + 6) && CheckType(L, type7, begin + 7) && CheckType(L, type8, begin + 8);
        }

        public static bool CheckTypes(IntPtr L, int begin, Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7, Type type8, Type type9)
        {
            return CheckType(L, type0, begin) && CheckType(L, type1, begin + 1) && CheckType(L, type2, begin + 2) && CheckType(L, type3, begin + 3) && CheckType(L, type4, begin + 4) &&
                   CheckType(L, type5, begin + 5) && CheckType(L, type6, begin + 6) && CheckType(L, type7, begin + 7) && CheckType(L, type8, begin + 8) && CheckType(L, type9, begin + 9);
        }

        public static bool CheckTypes(IntPtr L, int begin, params Type[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if (!CheckType(L, types[i], i + begin))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool CheckParamsType(IntPtr L, Type t, int begin, int count)
        {
            if (t == typeof(object))
            {
                return true;
            }

            for (int i = 0; i < count; i++)
            {
                if (!CheckType(L, t, i + begin))
                {
                    return false;
                }
            }

            return true;
        }

        static bool IsNilType(Type t)
        {
            if (t == null || !IsValueType(t))
            {
                return true;
            }

            if (IsNullable(t))
            {
                return true;
            }

            return false;
        }

        public static bool IsNullable(Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return true;
            }

            return false;
        }

        public static Type GetNullableType(Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type[] ts = t.GetGenericArguments();
                t = ts[0];
            }

            return t;
        }

        public static bool CheckType(IntPtr L, Type t, int pos)
        {
            //默认都可以转 object
            if (t == typeof(object))
            {
                return true;
            }

            t = GetNullableType(t);
            LuaTypes luaType = LuaDLL.lua_type(L, pos);            

            switch (luaType)
            {
                case LuaTypes.LUA_TNUMBER:
                    return IsNumberType(t);
                case LuaTypes.LUA_TSTRING:
                    return t == typeof(string) || t == typeof(byte[]) || t == typeof(char[]);
                case LuaTypes.LUA_TUSERDATA:
                    return IsMatchUserData(L, t, pos);
                case LuaTypes.LUA_TBOOLEAN:
                    return t == typeof(bool);
                case LuaTypes.LUA_TFUNCTION:
                    return t == typeof(LuaFunction);                            
                case LuaTypes.LUA_TTABLE:
                    return IsUserTable(L, t, pos);
                case LuaTypes.LUA_TLIGHTUSERDATA:
                    return t == typeof(IntPtr) || t == typeof(UIntPtr);
                case LuaTypes.LUA_TNIL:
                    return IsNilType(t);
                default:
                    break;
            }

            throw new LuaException("undefined type to check" + LuaDLL.luaL_typename(L, pos));
        }

        static Type monoType = typeof(Type).GetType();

        public static T ChangeType<T>(object temp, Type type)
        {
            if (temp.GetType() == monoType)
            {
                return (T)temp;
            }
            else
            {
                return (T)Convert.ChangeType(temp, type);
            }
        }

        public static object ChangeType(object temp, Type type)
        {
            if (temp.GetType() == monoType)
            {
                return (Type)temp;
            }
            else
            {
                return Convert.ChangeType(temp, type);
            }
        }

        static bool IsMatchUserData(IntPtr L, Type t, int pos)
        {
            if (t == typeof(long))
            {
                return LuaDLL.tolua_isint64(L, pos);
            }
            else if (t == typeof(ulong))
            {
                return LuaDLL.tolua_isuint64(L, pos);
            }

            object obj = null;
            int udata = LuaDLL.tolua_rawnetobj(L, pos);

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                obj = translator.GetObject(udata);

                if (obj != null)
                {
                    Type objType = obj.GetType();

                    if (t == objType || t.IsAssignableFrom(objType))
                    {
                        return true;
                    }
                }
                else
                {
                    return !t.IsValueType;
                }
            }

            return false;
        }

        public static bool IsNumberType(Type t)
        {
            if (t.IsPrimitive)
            {
                if (t == typeof(bool) || t == typeof(IntPtr) || t == typeof(UIntPtr))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        static bool IsUserTable(IntPtr L, Type t, int pos)
        {
            if (t.IsArray)
            {
                if (t.GetElementType().IsArray || t.GetArrayRank() > 1)
                {
                    return false;
                }

                return true;
            }
            else if (t == typeof(LuaTable))
            {
                return true;
            }
            else if (t.IsValueType)
            {
                LuaValueType vt = LuaDLL.tolua_getvaluetype(L, pos);

                switch (vt)
                {
                    case LuaValueType.Vector3:
                        return typeof(Vector3) == t;
                    case LuaValueType.Quaternion:
                        return typeof(Quaternion) == t;
                    case LuaValueType.Color:
                        return typeof(Color) == t;
                    case LuaValueType.Ray:
                        return typeof(Ray) == t;
                    case LuaValueType.Bounds:
                        return typeof(Bounds) == t;
                    case LuaValueType.Vector2:
                        return typeof(Vector2) == t;
                    case LuaValueType.Vector4:
                        return typeof(Vector4) == t;
                    case LuaValueType.Touch:
                        return typeof(Touch) == t;
                    case LuaValueType.LayerMask:
                        return typeof(LayerMask) == t;
                    case LuaValueType.RaycastHit:
                        return typeof(RaycastHit) == t;
                    default:
                        break;
                }
            }
            else if (LuaDLL.tolua_isvptrtable(L, pos))
            {
                return IsMatchUserData(L, t, pos);
            }

            return false;
        }        
    }
}