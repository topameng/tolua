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
using UnityEngine;
using System;
using System.Collections.Generic;

namespace LuaInterface
{
    public static class TypeChecker
    {
        static readonly Il2cppType il2cpp = new Il2cppType();
        public static Type[] LuaValueTypeMap = new Type[LuaValueType.Max];                

        static TypeChecker()
        {
            Il2cppType il = il2cpp;

            LuaValueTypeMap[LuaValueType.None] = null;
            LuaValueTypeMap[LuaValueType.Vector3] = il.TypeOfVector3;
            LuaValueTypeMap[LuaValueType.Quaternion] = il.TypeOfQuaternion;
            LuaValueTypeMap[LuaValueType.Vector2] = il.TypeOfVector2;
            LuaValueTypeMap[LuaValueType.Color] = il.TypeOfColor;
            LuaValueTypeMap[LuaValueType.Vector4] = il.TypeOfVector4;
            LuaValueTypeMap[LuaValueType.Ray] = il.TypeOfRay;
            LuaValueTypeMap[LuaValueType.Bounds] = il.TypeOfBounds;
            LuaValueTypeMap[LuaValueType.Touch] = il.TypeOfTouch;
            LuaValueTypeMap[LuaValueType.LayerMask] = il.TypeOfLayerMask;
            LuaValueTypeMap[LuaValueType.RaycastHit] = il.TypeOfRaycastHit;
            LuaValueTypeMap[LuaValueType.Int64] = il.TypeOfLong;
            LuaValueTypeMap[LuaValueType.UInt64] = il.TypeOfULong;
        }        

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
            if (t == il2cpp.TypeOfObject)
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
            if (!t.IsValueType)
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
            if (t.IsGenericType && t.GetGenericTypeDefinition() == il2cpp.TypeofGenericNullObject)
            {
                return true;
            }

            return false;
        }

        public static Type GetNullableType(Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == il2cpp.TypeofGenericNullObject)
            {
                Type[] ts = t.GetGenericArguments();
                t = ts[0];
            }

            return t;
        }

        public static bool CheckType(IntPtr L, Type type, int pos)
        {
            Il2cppType il = il2cpp;
            //默认都可以转 object
            if (type == il.TypeOfObject)
            {
                return true;
            }

            Type t = GetNullableType(type);
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNUMBER:
                    return IsNumberType(t);
                case LuaTypes.LUA_TSTRING:
                    return t == il.TypeOfString || t == il.TypeOfByteArray || t == il.TypeOfCharArray || t == il.TypeOfLuaByteBuffer;
                case LuaTypes.LUA_TUSERDATA:
                    return IsMatchUserData(L, t, pos);
                case LuaTypes.LUA_TBOOLEAN:
                    return t == il.TypeOfBool;
                case LuaTypes.LUA_TFUNCTION:
                    return t == il.TypeOfLuaFunction;
                case LuaTypes.LUA_TTABLE:
                    return IsUserTable(t, L, pos);
                case LuaTypes.LUA_TLIGHTUSERDATA:
                    return t == il.TypeOfIntPtr || t == il.TypeOfUIntPtr;
                case LuaTypes.LUA_TNIL:
                    return IsNilType(type);
                default:
                    break;
            }

            throw new LuaException("undefined type to check" + LuaDLL.luaL_typename(L, pos));
        }

        static Type monoType = typeof(Type).GetType();

        public static T ChangeType<T>(object temp, Type type)
        {
            Type tempType = temp.GetType();

            if (tempType == monoType || type.IsAssignableFrom(tempType))
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
            Type tempType = temp.GetType();

            if (tempType == monoType)
            {
                return (Type)temp;
            }
            else if (type.IsAssignableFrom(tempType))
            {
                return temp;
            }
            else
            {
                return Convert.ChangeType(temp, type);
            }
        }

        static bool IsMatchUserData(IntPtr L, Type t, int pos)
        {
            Il2cppType il = il2cpp;

            if (t == il.TypeOfLong)
            {
                return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Int64;                
            }
            else if (t == il.TypeOfULong)
            {
                return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.UInt64;                
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
                Il2cppType il = il2cpp;

                if (t == il.TypeOfBool || t == il.TypeOfIntPtr || t == il.TypeOfUIntPtr)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public static bool IsUserTable(Type t, IntPtr L, int pos)
        {
            int type = LuaDLL.tolua_getvaluetype(L, pos);

            if (type != LuaValueType.None)
            {
                return t == LuaValueTypeMap[type];
            }

            if (t.IsArray)
            {
                if (t.GetElementType().IsArray || t.GetArrayRank() > 1)
                {
                    return false;
                }

                return true;
            }
            else if (t == il2cpp.TypeOfLuaTable)
            {
                return true;
            }
            else if (LuaDLL.tolua_isvptrtable(L, pos))
            {
                return IsMatchUserData(L, t, pos);
            }

            return false;
        }

        public static bool CheckTypes<T1>(IntPtr L, int pos)
        {            
            return TypeTraits<T1>.Check(L, pos);
        }

        public static bool CheckTypes<T1, T2>(IntPtr L, int pos)
        {
            return TypeTraits<T1>.Check(L, pos) && TypeTraits<T2>.Check(L, pos + 1);
        }

        public static bool CheckTypes<T1, T2, T3>(IntPtr L, int pos)
        {
            return TypeTraits<T1>.Check(L, pos) && TypeTraits<T2>.Check(L, pos + 1) && TypeTraits<T3>.Check(L, pos + 2);
        }

        public static bool CheckTypes<T1, T2, T3, T4>(IntPtr L, int pos)
        {
            return TypeTraits<T1>.Check(L, pos) && TypeTraits<T2>.Check(L, pos + 1) && TypeTraits<T3>.Check(L, pos + 2) && TypeTraits<T4>.Check(L, pos + 3);
        }

        public static bool CheckTypes<T1, T2, T3, T4, T5>(IntPtr L, int pos)
        {
            return TypeTraits<T1>.Check(L, pos) && TypeTraits<T2>.Check(L, pos + 1) && TypeTraits<T3>.Check(L, pos + 2) && TypeTraits<T4>.Check(L, pos + 3) && TypeTraits<T5>.Check(L, pos + 4);
        }

        public static bool CheckTypes<T1, T2, T3, T4, T5, T6>(IntPtr L, int pos)
        {
            return TypeTraits<T1>.Check(L, pos) && TypeTraits<T2>.Check(L, pos + 1) && TypeTraits<T3>.Check(L, pos + 2) && TypeTraits<T4>.Check(L, pos + 3) && TypeTraits<T5>.Check(L, pos + 4) &&
                TypeTraits<T6>.Check(L, pos + 5);
        }

        public static bool CheckTypes<T1, T2, T3, T4, T5, T6, T7>(IntPtr L, int pos)
        {
            return TypeTraits<T1>.Check(L, pos) && TypeTraits<T2>.Check(L, pos + 1) && TypeTraits<T3>.Check(L, pos + 2) && TypeTraits<T4>.Check(L, pos + 3) && TypeTraits<T5>.Check(L, pos + 4) &&
                TypeTraits<T6>.Check(L, pos + 5) && TypeTraits<T7>.Check(L, pos + 6);
        }

        public static bool CheckTypes<T1, T2, T3, T4, T5, T6, T7, T8>(IntPtr L, int pos)
        {
            return TypeTraits<T1>.Check(L, pos) && TypeTraits<T2>.Check(L, pos + 1) && TypeTraits<T3>.Check(L, pos + 2) && TypeTraits<T4>.Check(L, pos + 3) && TypeTraits<T5>.Check(L, pos + 4) &&
                TypeTraits<T6>.Check(L, pos + 5) && TypeTraits<T7>.Check(L, pos + 6) && TypeTraits<T8>.Check(L, pos + 7);
        }

        public static bool CheckTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9>(IntPtr L, int pos)
        {
            return TypeTraits<T1>.Check(L, pos) && TypeTraits<T2>.Check(L, pos + 1) && TypeTraits<T3>.Check(L, pos + 2) && TypeTraits<T4>.Check(L, pos + 3) && TypeTraits<T5>.Check(L, pos + 4) &&
                TypeTraits<T6>.Check(L, pos + 5) && TypeTraits<T7>.Check(L, pos + 6) && TypeTraits<T8>.Check(L, pos + 7) && TypeTraits<T9>.Check(L, pos + 8);
        }

        public static bool CheckTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(IntPtr L, int pos)
        {
            return TypeTraits<T1>.Check(L, pos) && TypeTraits<T2>.Check(L, pos + 1) && TypeTraits<T3>.Check(L, pos + 2) && TypeTraits<T4>.Check(L, pos + 3) && TypeTraits<T5>.Check(L, pos + 4) &&
                TypeTraits<T6>.Check(L, pos + 5) && TypeTraits<T7>.Check(L, pos + 6) && TypeTraits<T8>.Check(L, pos + 7) && TypeTraits<T9>.Check(L, pos + 8) && TypeTraits<T10>.Check(L, pos + 9);
        }

        public static bool CheckTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(IntPtr L, int pos)
        {
            return TypeTraits<T1>.Check(L, pos) && TypeTraits<T2>.Check(L, pos + 1) && TypeTraits<T3>.Check(L, pos + 2) && TypeTraits<T4>.Check(L, pos + 3) && TypeTraits<T5>.Check(L, pos + 4) &&
                TypeTraits<T6>.Check(L, pos + 5) && TypeTraits<T7>.Check(L, pos + 6) && TypeTraits<T8>.Check(L, pos + 7) && TypeTraits<T9>.Check(L, pos + 8) && TypeTraits<T10>.Check(L, pos + 9) &&
                TypeTraits<T11>.Check(L, pos + 10);
        }

        public static bool CheckTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(IntPtr L, int pos)
        {
            return TypeTraits<T1>.Check(L, pos) && TypeTraits<T2>.Check(L, pos + 1) && TypeTraits<T3>.Check(L, pos + 2) && TypeTraits<T4>.Check(L, pos + 3) && TypeTraits<T5>.Check(L, pos + 4) &&
                TypeTraits<T6>.Check(L, pos + 5) && TypeTraits<T7>.Check(L, pos + 6) && TypeTraits<T8>.Check(L, pos + 7) && TypeTraits<T9>.Check(L, pos + 8) && TypeTraits<T10>.Check(L, pos + 9) &&
                TypeTraits<T11>.Check(L, pos + 10) && TypeTraits<T12>.Check(L, pos + 11);
        }

        public static bool CheckParamsType<T>(IntPtr L, int begin, int count)
        {
            if (TypeTraits<T>.type == il2cpp.TypeOfObject)
            {
                return true;
            }

            Func<IntPtr, int, bool> _Check = TypeTraits<T>.Check;

            for (int i = 0; i < count; i++)
            {
                if (!_Check(L, i + begin))
                {
                    return false;
                }
            }

            return true;
        }

        ////换模板
        static public bool CheckDelegateType(Type type, IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TUSERDATA:                    
                    int udata = LuaDLL.tolua_rawnetobj(L, pos);

                    if (udata != -1)
                    {
                        ObjectTranslator translator = ObjectTranslator.Get(L);
                        object obj = translator.GetObject(udata);
                        return obj == null ? true : type == obj.GetType();
                    }
                    return false;
                default:
                    return false;
            }
        }

        static public bool CheckDelegateType<T>(IntPtr L, int pos) //where T : Delegate
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TUSERDATA:
                    int udata = LuaDLL.tolua_rawnetobj(L, pos);

                    if (udata != -1)
                    {
                        ObjectTranslator translator = ObjectTranslator.Get(L);
                        object obj = translator.GetObject(udata);
                        return obj == null ? true : obj is T;
                    }
                    return false;
                default:
                    return false;
            }
        }

        static public bool CheckEnumType(Type type, IntPtr L, int pos)
        {            
            if (LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TUSERDATA)
            {                
                int udata = LuaDLL.tolua_rawnetobj(L, pos);

                if (udata != -1)
                {
                    ObjectTranslator translator = ObjectTranslator.Get(L);
                    object obj = translator.GetObject(udata);
                    return obj == null ? false : type == obj.GetType();
                }
            }
            
            return false;
        }
    }
}