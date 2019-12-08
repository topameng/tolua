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
using System.Collections;

namespace LuaInterface
{
    public class LuaMatchType
    {
        static readonly Il2cppType il2cpp = new Il2cppType();

        public bool CheckNumber(IntPtr L, int pos)
        {            
            return LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TNUMBER;
        }

#if LUAC_5_3
		public bool CheckInteger(IntPtr L, int pos)
        {
			return LuaDLL.lua_isinteger(L, pos) != 0;	
        }
#else
        public bool CheckInteger(IntPtr L, int pos)
        {
			return LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TNUMBER;
        }
#endif

        public bool CheckBool(IntPtr L, int pos)
        {
            return LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TBOOLEAN;
        }

        public bool CheckLong(IntPtr L, int pos)
        {
#if LUAC_5_3
            return LuaDLL.lua_isinteger(L, pos) != 0;
#else
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNUMBER:
                    return true;
                case LuaTypes.LUA_TUSERDATA:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Int64;                    
                default:
                    return false;
            }
#endif
		}

        public bool CheckULong(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNUMBER:
                    return LuaDLL.lua_tonumber(L, pos) >= 0;
                case LuaTypes.LUA_TUSERDATA:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.UInt64;                    
                default:
                    return false;
            }
        }

        public bool CheckNullNumber(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);
            return luaType == LuaTypes.LUA_TNUMBER || luaType == LuaTypes.LUA_TNIL;
        }

#if LUAC_5_3       
        //需要优化,合并为1个call		
        public bool CheckNullInteger(IntPtr L, int pos)
        {
            return LuaDLL.lua_isinteger(L, pos) != 0 || LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TNIL;
        }
#else
        public bool CheckNullInteger(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);
            return luaType == LuaTypes.LUA_TNUMBER || luaType == LuaTypes.LUA_TNIL;
        }
#endif

        public bool CheckNullBool(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);
            return luaType == LuaTypes.LUA_TBOOLEAN || luaType == LuaTypes.LUA_TNIL;
        }

        public bool CheckNullLong(IntPtr L, int pos)
        {
#if LUAC_5_3
			//需要优化,合并为1个call		
			return LuaDLL.lua_isinteger(L, pos) != 0 || LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TNIL;
#else
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TNUMBER:
                    return true;
                case LuaTypes.LUA_TUSERDATA:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Int64;                    
                default:
                    return false;
            }
#endif
		}

        public bool CheckNullULong(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TNUMBER:
                    return true;
                case LuaTypes.LUA_TUSERDATA:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.UInt64;                    
                default:
                    return false;
            }
        }

        readonly Type TypeOfString = typeof(string);

        public bool CheckString(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TSTRING:
                    return true;
                case LuaTypes.LUA_TUSERDATA:
                    return CheckClassType(TypeOfString, L, pos);
                default:
                    return false;
            }            
        }

        readonly Type TypeOfByteArray = typeof(byte[]);

        public bool CheckByteArray(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TSTRING:
                    return true;
                case LuaTypes.LUA_TUSERDATA:
                    return CheckClassType(TypeOfByteArray, L, pos);
                default:
                    return false;
            }
        }

        readonly Type TypeOfCharArray = typeof(char[]);

        public bool CheckCharArray(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TSTRING:
                    return true;
                case LuaTypes.LUA_TUSERDATA:
                    return CheckClassType(TypeOfCharArray, L, pos);
                default:
                    return false;
            }
        }

        public bool CheckArray(Type t, IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TTABLE:                                
                    return true;
                case LuaTypes.LUA_TUSERDATA:
                    return CheckClassType(t, L, pos);
                default:
                    return false;
            }
        }

        readonly Type TypeOfBoolArray = typeof(bool[]);

        public bool CheckBoolArray(IntPtr L, int pos)
        {
            return CheckArray(TypeOfBoolArray, L, pos);
        }

        readonly Type TypeOfSByteArray = typeof(sbyte[]);
        
        public bool CheckSByteArray(IntPtr L, int pos)
        {
            return CheckArray(TypeOfSByteArray, L, pos);
        }

        readonly Type TypeOfShortArray = typeof(short[]);

        public bool CheckInt16Array(IntPtr L, int pos)
        {
            return CheckArray(TypeOfShortArray, L, pos);
        }

        readonly Type TypeOfUShortArray = typeof(ushort[]);

        public bool CheckUInt16Array(IntPtr L, int pos)
        {
            return CheckArray(TypeOfUShortArray, L, pos);
        }

        readonly Type TypeOfDecimalArray = typeof(decimal[]);

        public bool CheckDecimalArray(IntPtr L, int pos)
        {
            return CheckArray(TypeOfDecimalArray, L, pos);
        }

        readonly Type TypeOfFloatArray = typeof(float[]);

        public bool CheckSingleArray(IntPtr L, int pos)
        {
            return CheckArray(TypeOfFloatArray, L, pos);
        }

        readonly Type TypeOfDoubleArray = typeof(double[]);

        public bool CheckDoubleArray(IntPtr L, int pos)
        {
            return CheckArray(TypeOfDoubleArray, L, pos);
        }

        readonly Type TypeOfIntArray = typeof(int[]);

        public bool CheckInt32Array(IntPtr L, int pos)
        {
            return CheckArray(TypeOfIntArray, L, pos);
        }

        readonly Type TypeOfUIntArray = typeof(uint[]);

        public bool CheckUInt32Array(IntPtr L, int pos)
        {
            return CheckArray(TypeOfUIntArray, L, pos);
        }

        readonly Type TypeOfLongArray = typeof(long[]);

        public bool CheckInt64Array(IntPtr L, int pos)
        {
            return CheckArray(TypeOfLongArray, L, pos);
        }

        readonly Type TypeOfULongArray = typeof(ulong[]);

        public bool CheckUInt64Array(IntPtr L, int pos)
        {
            return CheckArray(TypeOfULongArray, L, pos);
        }

        readonly Type TypeOfStringArray = typeof(string[]);

        public bool CheckStringArray(IntPtr L, int pos)
        {
            return CheckArray(TypeOfStringArray, L, pos);
        }

        readonly Type TypeOfTypeArray = typeof(Type[]);

        public bool CheckTypeArray(IntPtr L, int pos)
        {
            return CheckArray(TypeOfTypeArray, L, pos);
        }

        readonly Type TypeOfObjectArray = typeof(object[]);

        public bool CheckObjectArray(IntPtr L, int pos)
        {
            return CheckArray(TypeOfObjectArray, L, pos);
        }        

        bool CheckValueType(IntPtr L, int pos, int valueType, Type nt)
        {
            if (LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TTABLE)
            {
                int vt = LuaDLL.tolua_getvaluetype(L, pos);                
                return vt == valueType;
            }

            return false;
        }

        public bool CheckVec3(IntPtr L, int pos)
        {
            if (LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TTABLE)
            {
                return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Vector3;
            }

            return false;            
        }

        public bool CheckQuat(IntPtr L, int pos)
        {
            if (LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TTABLE)
            {
                return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Quaternion;
            }

            return false;            
        }

        public bool CheckVec2(IntPtr L, int pos)
        {
            if (LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TTABLE)
            {
                return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Vector2;
            }

            return false;            
        }

        public bool CheckColor(IntPtr L, int pos)
        {
            if (LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TTABLE)
            {
                return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Color;
            }

            return false;            
        }

        public bool CheckVec4(IntPtr L, int pos)
        {
            if (LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TTABLE)
            {
                return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Vector4;
            }

            return false;            
        }

        public bool CheckRay(IntPtr L, int pos)
        {
            if (LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TTABLE)
            {
                return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Ray;
            }

            return false;            
        }

        public bool CheckBounds(IntPtr L, int pos)
        {
            if (LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TTABLE)
            {
                return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Bounds;
            }

            return false;            
        }

        public bool CheckTouch(IntPtr L, int pos)
        {
            if (LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TTABLE)
            {
                return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Touch;
            }

            return false;            
        }

        public bool CheckLayerMask(IntPtr L, int pos)
        {
            if (LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TTABLE)
            {
                return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.LayerMask;
            }

            return false;            
        }

        public bool CheckRaycastHit(IntPtr L, int pos)
        {
            if (LuaDLL.lua_type(L, pos) == LuaTypes.LUA_TTABLE)
            {
                return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.RaycastHit;
            }

            return false;            
        }

        public bool CheckNullVec3(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TTABLE:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Vector3;
                default:
                    return false;
            }
        }

        public bool CheckNullQuat(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TTABLE:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Quaternion;
                default:
                    return false;
            }
        }

        public bool CheckNullVec2(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TTABLE:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Vector2;
                default:
                    return false;
            }
        }

        public bool CheckNullColor(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TTABLE:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Color;
                default:
                    return false;
            }
        }

        public bool CheckNullVec4(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TTABLE:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Vector4;
                default:
                    return false;
            }
        }

        public bool CheckNullRay(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TTABLE:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Ray;
                default:
                    return false;
            }
        }

        public bool CheckNullBounds(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TTABLE:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Bounds;
                default:
                    return false;
            }
        }

        public bool CheckNullTouch(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TTABLE:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.Touch;
                default:
                    return false;
            }
        }

        public bool CheckNullLayerMask(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TTABLE:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.LayerMask;
                default:
                    return false;
            }
        }

        public bool CheckNullRaycastHit(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TTABLE:
                    return LuaDLL.tolua_getvaluetype(L, pos) == LuaValueType.RaycastHit;
                default:
                    return false;
            }
        }

        readonly Type TypeOfVector3Array = typeof(Vector3[]);

        public bool CheckVec3Array(IntPtr L, int pos)
        {
            return CheckArray(TypeOfVector3Array, L, pos);
        }

        readonly Type TypeOfQuaternionArray = typeof(Quaternion[]);

        public bool CheckQuatArray(IntPtr L, int pos)
        {
            return CheckArray(TypeOfQuaternionArray, L, pos);
        }

        readonly Type TypeOfVector2Array = typeof(Vector2[]);

        public bool CheckVec2Array(IntPtr L, int pos)
        {
            return CheckArray(TypeOfVector2Array, L, pos);
        }

        readonly Type TypeOfVector4Array = typeof(Vector4[]);

        public bool CheckVec4Array(IntPtr L, int pos)
        {
            return CheckArray(TypeOfVector4Array, L, pos);
        }

        readonly Type TypeOfColorArray = typeof(Color[]);

        public bool CheckColorArray(IntPtr L, int pos)
        {
            return CheckArray(TypeOfColorArray, L, pos);
        }

        public bool CheckPtr(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);
            return luaType == LuaTypes.LUA_TLIGHTUSERDATA || luaType == LuaTypes.LUA_TNIL;
        }

        public bool CheckLuaFunc(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);
            return luaType == LuaTypes.LUA_TFUNCTION || luaType == LuaTypes.LUA_TNIL;
        }

        public bool CheckLuaTable(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);
            return luaType == LuaTypes.LUA_TTABLE || luaType == LuaTypes.LUA_TNIL;
        }

        public bool CheckLuaThread(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);
            return luaType == LuaTypes.LUA_TTHREAD || luaType == LuaTypes.LUA_TNIL;
        }

        public bool CheckLuaBaseRef(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch(luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TFUNCTION:
                    return true;
                case LuaTypes.LUA_TTABLE:
                    return true;
                case LuaTypes.LUA_TTHREAD:
                    return true;
                default:
                    return false;
            }            
        }

        public bool CheckByteBuffer(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);
            return luaType == LuaTypes.LUA_TSTRING || luaType == LuaTypes.LUA_TNIL;
        }

        readonly Type TypeOfEventObject = typeof(EventObject);

        public bool CheckEventObject(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TUSERDATA:
                    return CheckClassType(TypeOfEventObject, L, pos);
                default:
                    return false;
            }
        }

        public bool CheckEnumerator(IntPtr L, int pos)
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
                        Il2cppType il = il2cpp;
                        Type type = translator.CheckOutNodeType(udata);
                        return type == null ? udata == 1 : type == il.TypeOfIEnumerator || il.TypeOfIEnumerator.IsAssignableFrom(type);
                    }
                    return false;                    
                default:
                    return false;
            }
        }

        //不存在派生类的类型
        bool CheckFinalType(Type type, IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TUSERDATA:
                    return CheckClassType(type, L, pos);
                default:
                    return false;
            }
        }

        readonly Type TypeOfGameObject = typeof(GameObject);

        public bool CheckGameObject(IntPtr L, int pos)
        {
            return CheckFinalType(TypeOfGameObject, L, pos);
        }

        public bool CheckTransform(IntPtr L, int pos)
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
                        Il2cppType il = il2cpp;
                        ObjectTranslator translator = ObjectTranslator.Get(L);
                        Type type = translator.CheckOutNodeType(udata);
                        return type == null ? udata == 1 : type == il.TypeOfTransform || il.TypeOfTransform.IsAssignableFrom(type);
                    }

                    return false;                    
                default:
                    return false;
            }
        }

        readonly Type monoType = typeof(Type).GetType();

        public bool CheckMonoType(IntPtr L, int pos)
        {
            LuaTypes luaType = LuaDLL.lua_type(L, pos);

            switch (luaType)
            {
                case LuaTypes.LUA_TNIL:
                    return true;
                case LuaTypes.LUA_TUSERDATA:
                    return CheckClassType(monoType, L, pos);
                default:
                    return false;
            }            
        }        

        public bool CheckVariant(IntPtr L, int pos)
        {
            return true;
        }

        bool CheckClassType(Type t, IntPtr L, int pos)
        {            
            int udata = LuaDLL.tolua_rawnetobj(L, pos);

            if (udata != -1)
            {                
                ObjectTranslator translator = ObjectTranslator.Get(L);
                Type type = translator.CheckOutNodeType(udata);
                return type == null ? udata == 1 : type == t;
            }

            return false;
        }
    }
}

