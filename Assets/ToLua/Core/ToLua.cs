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
using UnityEngine;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;

namespace LuaInterface
{    
    public static class ToLua
    {
        static Type monoType = typeof(Type).GetType();

        public static void OpenLibs(IntPtr L)
        {            
            LuaDLL.lua_getglobal(L, "tolua");

            LuaDLL.lua_pushstring(L, "isnull");
            LuaDLL.lua_pushcfunction(L, IsNull);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "tolstring");
            LuaDLL.tolua_pushcfunction(L, BufferToString);
            LuaDLL.lua_rawset(L, -3);

            LuaDLL.lua_pushstring(L, "typeof");
            LuaDLL.lua_pushcfunction(L, GetClassType);
            LuaDLL.lua_rawset(L, -3);  

            int meta = LuaStatic.GetMetaReference(L, typeof(NullObject));
            LuaDLL.lua_pushstring(L, "null");
            LuaDLL.tolua_pushnewudata(L, meta, 1);
            LuaDLL.lua_rawset(L, -3);
            LuaDLL.lua_pop(L, 1);


            LuaDLL.tolua_pushudata(L, 1);
            LuaDLL.lua_setfield(L, LuaIndexes.LUA_GLOBALSINDEX, "null");
            LuaDLL.lua_pop(L, 1);
        }

        /*--------------------------------对于tolua扩展函数------------------------------------------*/
        #region TOLUA_EXTEND_FUNCTIONS
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int IsNull(IntPtr L)
        {
            LuaTypes t = LuaDLL.lua_type(L, 1);

            if (t == LuaTypes.LUA_TNIL)
            {
                LuaDLL.lua_pushboolean(L, true);                
            }
            else
            {
                object o = ToLua.ToObject(L, -1);

                if (o == null)
                {
                    LuaDLL.lua_pushboolean(L, true);
                }
                else if (o is UnityEngine.Object)
                {
                    UnityEngine.Object obj = (UnityEngine.Object)o;
                    LuaDLL.lua_pushboolean(L, obj == null);
                }
                else
                {
                    LuaDLL.lua_pushboolean(L, false);
                }
            }

            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int BufferToString(IntPtr L)
        {
            try
            {
                object o = CheckObject(L, 1);

                if (o is byte[])
                {
                    byte[] buff = (byte[])o;
                    LuaDLL.lua_pushlstring(L, buff, buff.Length);
                }
                else if (o is char[])
                {
                    byte[] buff = System.Text.Encoding.UTF8.GetBytes((char[])o);                     
                    LuaDLL.lua_pushlstring(L, buff, buff.Length);
                }
                else if (o is string)
                {
                    LuaDLL.lua_pushstring(L, (string)o);
                }
                else
                {
                    LuaDLL.luaL_typerror(L, 1, "byte[] or char[]");
                }
            }
            catch (Exception e)
            {
                LuaDLL.toluaL_exception(L, e);
            }

            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int GetClassType(IntPtr L)
        {
            int reference = LuaDLL.tolua_getmetatableref(L, -1);

            if (reference > 0)
            {                
                Type t = LuaStatic.GetClassType(L, reference);
                Push(L, t);
            }
            else
            {
                LuaValueType ret = LuaDLL.tolua_getvaluetype(L, -1);

                switch (ret)
                {
                    case LuaValueType.Vector3:
                        Push(L, typeof(Vector3));
                        break;
                    case LuaValueType.Quaternion:
                        Push(L, typeof(Quaternion));
                        break;
                    case LuaValueType.Vector4:
                        Push(L, typeof(Vector4));
                        break;
                    case LuaValueType.Color:
                        Push(L, typeof(Color));
                        break;
                    case LuaValueType.Ray:
                        Push(L, typeof(Ray));
                        break;
                    case LuaValueType.Bounds:
                        Push(L, typeof(Bounds));
                        break;
                    case LuaValueType.Vector2:
                        Push(L, typeof(Vector2));
                        break;
                    case LuaValueType.LayerMask:
                        Push(L, typeof(LayerMask));
                        break;
                    case LuaValueType.RaycastHit:
                        Push(L, typeof(RaycastHit));
                        break;
                    default:
                        Debugger.LogError("type not register to lua");
                        LuaDLL.lua_pushnil(L);
                        break;
                }                  
            }

            return 1;
        }
        #endregion
        /*-------------------------------------------------------------------------------------------*/

        public static string ToString(IntPtr L, int stackPos)
        {
            if (LuaDLL.lua_isstring(L, stackPos) == 1)
            {
                return LuaDLL.lua_tostring(L, stackPos);
            }
            else if (LuaDLL.lua_isuserdata(L, stackPos) == 1)
            {
                return (string)ToObject(L, stackPos);                
            }

            return null;
        }

        public static object ToObject(IntPtr L, int stackPos)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                return translator.GetObject(udata);
            }

            return null;
        }

        public static LuaFunction ToLuaFunction(IntPtr L, int stackPos)
        {
            stackPos = LuaDLL.abs_index(L, stackPos);            
            LuaDLL.lua_pushvalue(L, stackPos);
            int reference = LuaDLL.toluaL_ref(L);
            return LuaStatic.GetFunction(L, reference);
        }

        public static LuaTable ToLuaTable(IntPtr L, int stackPos)
        {
            stackPos = LuaDLL.abs_index(L, stackPos);            
            LuaDLL.lua_pushvalue(L, stackPos);
            int reference = LuaDLL.toluaL_ref(L);
            return LuaStatic.GetTable(L, reference);
        }

        public static LuaThread ToLuaThread(IntPtr L, int stackPos)
        {
            stackPos = LuaDLL.abs_index(L, stackPos);            
            LuaDLL.lua_pushvalue(L, stackPos);
            int reference = LuaDLL.toluaL_ref(L);
            return LuaStatic.GetLuaThread(L, reference);
        }
       
        public static Vector3 ToVector3(IntPtr L, int stackPos)
        {            
            float x = 0, y = 0, z = 0;
            LuaDLL.tolua_getvec3(L, stackPos, out x, out y, out z);            
            return new Vector3(x, y, z);
        }

        public static Vector4 ToVector4(IntPtr L, int stackPos)
        {            
            float x, y, z, w;
            LuaDLL.tolua_getvec4(L, stackPos, out x, out y, out z, out w);
            return new Vector4(x, y, z, w);
        }

        public static Vector2 ToVector2(IntPtr L, int stackPos)
        {            
            float x, y;
            LuaDLL.tolua_getvec2(L, stackPos, out x, out y);
            return new Vector2(x, y);
        }

        public static Quaternion ToQuaternion(IntPtr L, int stackPos)
        {            
            float x, y, z, w;
            LuaDLL.tolua_getquat(L, stackPos, out x, out y, out z, out w);
            return new Quaternion(x, y, z, w);
        }

        public static Color ToColor(IntPtr L, int stackPos)
        {            
            float r, g, b, a;
            LuaDLL.tolua_getclr(L, stackPos, out r, out g, out b, out a);
            return new Color(r, g, b, a);
        }

        public static Ray ToRay(IntPtr L, int stackPos)
        {            
            int top = LuaDLL.lua_gettop(L);                        
            LuaStatic.GetUnpackRayRef(L);
            LuaDLL.lua_pushvalue(L, stackPos);

            if (LuaDLL.lua_pcall(L, 1, 2, 0) == 0)
            {
                Vector3 origin = ToVector3(L, top + 1);
                Vector3 dir = ToVector3(L, top + 2);
                return new Ray(origin, dir);
            }
            else
            {
                string error = LuaDLL.lua_tostring(L, -1);
                throw new LuaException(error);
            }
        }

        public static Bounds ToBounds(IntPtr L, int stackPos)
        {            
            int top = LuaDLL.lua_gettop(L);                        
            LuaStatic.GetUnpackBounds(L);
            LuaDLL.lua_pushvalue(L, stackPos);

            if (LuaDLL.lua_pcall(L, 1, 2, 0) == 0)
            {
                Vector3 center = ToVector3(L, top + 1);
                Vector3 size = ToVector3(L, top + 2);
                return new Bounds(center, size);
            }
            else
            {
                string error = LuaDLL.lua_tostring(L, -1);
                throw new LuaException(error);
            }
        }

        public static LayerMask ToLayerMask(IntPtr L, int stackPos)
        {            
            return LuaDLL.tolua_getlayermask(L, stackPos);         
        }

        public static LuaInteger64 CheckLuaInteger64(IntPtr L, int stackPos)
        {
            if (!LuaDLL.tolua_isint64(L, stackPos))
            {                
                LuaDLL.luaL_typerror(L, stackPos, "int64");
                return 0;
            }

            return LuaDLL.tolua_toint64(L, stackPos);
        }

        public static object ToVarObject(IntPtr L, int stackPos)
        {
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            switch (type)
            {
                case LuaTypes.LUA_TNUMBER:
                    return LuaDLL.lua_tonumber(L, stackPos);
                case LuaTypes.LUA_TSTRING:
                    return LuaDLL.lua_tostring(L, stackPos);
                case LuaTypes.LUA_TUSERDATA:
                    return ToObject(L, stackPos);
                case LuaTypes.LUA_TBOOLEAN:
                    return LuaDLL.lua_toboolean(L, stackPos);
                case LuaTypes.LUA_TFUNCTION:                                                            
                    return ToLuaFunction(L, stackPos);
                case LuaTypes.LUA_TTABLE:
                    return ToVarTable(L, stackPos);
                case LuaTypes.LUA_TNIL:
                    return null;
                case LuaTypes.LUA_TLIGHTUSERDATA:
                    return LuaDLL.lua_touserdata(L, stackPos);
                case LuaTypes.LUA_TTHREAD:
                    return ToLuaThread(L, stackPos);
                default:                    
                    return null;
            }
        }

        public static object ToVarTable(IntPtr L, int stackPos)
        {
            stackPos = LuaDLL.abs_index(L, stackPos);
            LuaValueType ret = LuaDLL.tolua_getvaluetype(L, stackPos);

            switch(ret)
            {
                case LuaValueType.Vector3:
                    return ToVector3(L, stackPos);
                case LuaValueType.Quaternion:
                    return ToQuaternion(L, stackPos);
                case LuaValueType.Vector4:
                    return ToVector4(L, stackPos);
                case LuaValueType.Color:
                    return ToColor(L, stackPos);
                case LuaValueType.Ray:
                    return ToRay(L, stackPos);
                case LuaValueType.Bounds:
                    return ToBounds(L, stackPos);
                case LuaValueType.Vector2:
                    return ToVector2(L, stackPos);
                case LuaValueType.LayerMask:
                    return ToLayerMask(L, stackPos);
                default:                    
                    LuaDLL.lua_pushvalue(L, stackPos);
                    int reference = LuaDLL.toluaL_ref(L);
                    return LuaStatic.GetTable(L, reference);
            }            
        }

        public static LuaFunction CheckLuaFunction(IntPtr L, int stackPos)
        {
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            if (type == LuaTypes.LUA_TNIL)
            {
                return null;
            }
            else if (type != LuaTypes.LUA_TFUNCTION)
            {
                LuaDLL.luaL_typerror(L, stackPos, "function");
                return null;
            }            

            return ToLuaFunction(L, stackPos);
        }

        public static LuaTable CheckLuaTable(IntPtr L, int stackPos)
        {
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            if (type == LuaTypes.LUA_TNIL)
            {
                return null;
            }
            else if (type != LuaTypes.LUA_TTABLE)
            {
                LuaDLL.luaL_typerror(L, stackPos, "table");
                return null;
            }

            return ToLuaTable(L, stackPos);
        }

        public static LuaThread CheckLuaThread(IntPtr L, int stackPos)
        {
            LuaTypes type = LuaDLL.lua_type(L, stackPos);

            if (type == LuaTypes.LUA_TNIL)
            {
                return null;
            }
            else if (type != LuaTypes.LUA_TTHREAD)
            {
                LuaDLL.luaL_typerror(L, stackPos, "thread");
                return null;
            }

            return ToLuaThread(L, stackPos);
        }

        public static string CheckString(IntPtr L, int stackPos)
        {
            if (LuaDLL.lua_isstring(L, stackPos) == 1)
            {
                return LuaDLL.lua_tostring(L, stackPos);
            }
            else if (LuaDLL.lua_isuserdata(L, stackPos) == 1)
            {
                return (string)CheckObject(L, stackPos, typeof(string));
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "string");
            return null;
        }

        public static object CheckObject(IntPtr L, int stackPos)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                return translator.GetObject(udata);
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "object");
            return null;
        }

        public static object CheckObject(IntPtr L, int stackPos, Type type)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                object obj = translator.GetObject(udata);

                if (obj != null)
                {
                    Type objType = obj.GetType();

                    if (type == objType || type.IsAssignableFrom(objType))
                    {
                        return obj;
                    }
                    
                    LuaDLL.luaL_argerror(L, stackPos, string.Format("{0} expected, got {1}", type.FullName, objType.FullName));
                }

                return null;
            }
            else if(LuaDLL.lua_isnil(L, stackPos) && !TypeChecker.IsValueType(type))
            {                                        
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, type.FullName);
            return null;
        }

        public static UnityEngine.Object CheckUnityObject(IntPtr L, int stackPos, Type type)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);
            object obj = null;

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                obj = translator.GetObject(udata);

                if (obj != null)
                {
                    UnityEngine.Object uObj = (UnityEngine.Object)obj;

                    if (uObj == null)
                    {
                        LuaDLL.luaL_argerror(L, stackPos, string.Format("{0} expected, got nil", type.FullName));
                        return null;
                    }

                    Type objType = uObj.GetType();

                    if (type == objType || objType.IsSubclassOf(type))
                    {
                        return uObj;
                    }

                    LuaDLL.luaL_argerror(L, stackPos, string.Format("{0} expected, got {1}", type.FullName, objType.FullName));
                }

                return null;
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, type.FullName);
            return null;
        }

        public static UnityEngine.TrackedReference CheckTrackedReference(IntPtr L, int stackPos, Type type)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);
            object obj = null;

            if (udata != -1)
            {
                ObjectTranslator translator = ObjectTranslator.Get(L);
                obj = translator.GetObject(udata);

                if (obj != null)
                {
                    UnityEngine.TrackedReference uObj = (UnityEngine.TrackedReference)obj;

                    if (uObj == null)
                    {
                        LuaDLL.luaL_argerror(L, stackPos, string.Format("{0} expected, got nil", type.FullName));
                        return null;
                    }

                    Type objType = uObj.GetType();

                    if (type == objType || objType.IsSubclassOf(type))
                    {
                        return uObj;
                    }

                    LuaDLL.luaL_argerror(L, stackPos, string.Format("{0} expected, got {1}", type.FullName, objType.FullName));
                }

                return null;
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, type.FullName);
            return null;
        }

        //必须检测类型
        public static object[] CheckObjectArray(IntPtr L, int stackPos)
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            if (luatype == LuaTypes.LUA_TTABLE)
            {
                int index = 1;
                object val = null;                
                List<object> list = new List<object>();
                LuaDLL.lua_pushvalue(L, stackPos);

                while (true)
                {
                    LuaDLL.lua_rawgeti(L, -1, index);
                    luatype = LuaDLL.lua_type(L, -1);

                    if (luatype == LuaTypes.LUA_TNIL)
                    {
                        LuaDLL.lua_pop(L, 1);
                        return list.ToArray(); ;
                    }

                    val = ToVarObject(L, -1);
                    list.Add(val);
                    LuaDLL.lua_pop(L, 1);
                    ++index;
                }
            }
            else if (luatype == LuaTypes.LUA_TUSERDATA)
            {
                return (object[])CheckObject(L, stackPos, typeof(object[]));
            }
            else if (luatype == LuaTypes.LUA_TNIL)
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "object[] or table");
            return null;
        }

        public static T[] CheckObjectArray<T>(IntPtr L, int stackPos)
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            if (luatype == LuaTypes.LUA_TTABLE)
            {
                int index = 1;
                T val = default(T);
                Type t = typeof(T);
                List<T> list = new List<T>();
                LuaDLL.lua_pushvalue(L, stackPos);                

                while(true)
                {
                    LuaDLL.lua_rawgeti(L, -1, index);
                    luatype = LuaDLL.lua_type(L, -1);

                    if (luatype == LuaTypes.LUA_TNIL)
                    {
                        LuaDLL.lua_pop(L, 1);
                        return list.ToArray(); ;
                    }
                    else if (!TypeChecker.CheckType(L, t, -1))
                    {
                        LuaDLL.lua_pop(L, 1);
                        break;
                    }

                    val = (T)ToVarObject(L, -1);
                    list.Add(val);
                    LuaDLL.lua_pop(L, 1);
                    ++index;
                }
            }
            else if (luatype == LuaTypes.LUA_TUSERDATA)
            {
                return (T[])CheckObject(L, stackPos, typeof(T[]));
            }
            else if (luatype == LuaTypes.LUA_TNIL)
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, typeof(T[]).FullName);            
            return null;
        }

        public static char[] CheckCharBuffer(IntPtr L, int stackPos)
        {
            if (LuaDLL.lua_isstring(L, stackPos) != 0)
            {                
                string str = LuaDLL.lua_tostring(L, stackPos);
                return str.ToCharArray(); ;
            }
            else if (LuaDLL.lua_isuserdata(L, stackPos) != 0)
            {
                return (char[])CheckObject(L, stackPos, typeof(char[]));
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "string or char[]");
            return null;
        }

        public static byte[] CheckByteBuffer(IntPtr L, int stackPos)
        {            
            if (LuaDLL.lua_isstring(L, stackPos) != 0)
            {
                int len;
                IntPtr source = LuaDLL.lua_tolstring(L, stackPos, out len);
                byte[] buffer = new byte[len];
                Marshal.Copy(source, buffer, 0, len);                
                return buffer;
            }
            else if (LuaDLL.lua_isuserdata(L, stackPos) != 0)
            {
                return (byte[])CheckObject(L, stackPos, typeof(byte[]));
            }
            else if (LuaDLL.lua_isnil(L, stackPos))
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "string or byte[]");
            return null;
        }

        public static T[] CheckNumberArray<T>(IntPtr L, int stackPos)
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            if (luatype == LuaTypes.LUA_TTABLE)
            {
                int index = 1;
                T ret = default(T);
                List<T> list = new List<T>();
                LuaDLL.lua_pushvalue(L, stackPos);

                while (true)
                {
                    LuaDLL.lua_rawgeti(L, -1, index);
                    luatype = LuaDLL.lua_type(L, -1);

                    if (luatype == LuaTypes.LUA_TNIL)
                    {
                        LuaDLL.lua_pop(L, 1);
                        return list.ToArray();
                    }
                    else if (luatype != LuaTypes.LUA_TNUMBER)
                    {
                        break;
                    }

                    ret = (T)Convert.ChangeType(LuaDLL.lua_tonumber(L, -1), typeof(T));
                    list.Add(ret);
                    LuaDLL.lua_pop(L, 1);
                    ++index;
                }
            }
            else if (luatype == LuaTypes.LUA_TUSERDATA)
            {
                return (T[])CheckObject(L, stackPos, typeof(T[]));
            }
            else if (luatype == LuaTypes.LUA_TNIL)
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, LuaMisc.GetTypeName(typeof(T[])));
            return null;
        }
        

        public static bool[] CheckBoolArray(IntPtr L, int stackPos)
        {
            throw new LuaException("not defined CheckBoolArray");            
        }

        public static string[] CheckStringArray(IntPtr L, int stackPos)
        {
            LuaTypes luatype = LuaDLL.lua_type(L, stackPos);

            if (luatype == LuaTypes.LUA_TTABLE)
            {
                int index = 1;
                string retVal = null;
                Type t = typeof(string);
                List<string> list = new List<string>();
                LuaDLL.lua_pushvalue(L, stackPos);

                while (true)
                {
                    LuaDLL.lua_rawgeti(L, -1, index);
                    luatype = LuaDLL.lua_type(L, -1);

                    if (luatype == LuaTypes.LUA_TNIL)
                    {
                        LuaDLL.lua_pop(L, 1);
                        return list.ToArray();
                    }
                    else if (!TypeChecker.CheckType(L, t, -1))
                    {
                        LuaDLL.lua_pop(L, 1);
                        break;
                    }

                    retVal = ToString(L, -1);
                    list.Add(retVal);
                    LuaDLL.lua_pop(L, 1);
                    ++index;
                }
            }
            else if (luatype == LuaTypes.LUA_TUSERDATA)
            {
                return (string[])CheckObject(L, stackPos, typeof(string[]));                                
            }
            else if (luatype == LuaTypes.LUA_TNIL)
            {
                return null;
            }

            LuaDLL.luaL_typerror(L, stackPos, "string[]");            
            return null;
        }

        public static object[] ToParamsObject(IntPtr L, int stackPos, int count)
        {
            List<object> list = new List<object>(count);
            object obj = null;

            while (count > 0)
            {
                obj = ToVarObject(L, stackPos);
                list.Add(obj);
                --count;
                ++stackPos;
            }

            return list.ToArray();
        }

        public static T[] ToParamsObject<T>(IntPtr L, int stackPos, int count) where T : class
        {
            List<T> list = new List<T>(count);
            T obj = default(T);

            while (count > 0)
            {
                object tmp = ToObject(L, stackPos);
                obj = (T)tmp;
                list.Add(obj);
                --count;
                ++stackPos;
            }

            return list.ToArray();
        }

        public static string[] ToParamsString(IntPtr L, int stackPos, int count)
        {
            List<string> list = new List<string>(count);
            string obj = null;

            while (count > 0)
            {
                obj = ToString(L, stackPos);
                ++stackPos;
                --count;
                list.Add(obj);
            }

            return list.ToArray();
        }

        public static T[] ToParamsNumber<T>(IntPtr L, int stackPos, int count)
        {
            List<T> list = new List<T>(count);            

            while (count > 0)
            {
                double d = LuaDLL.lua_tonumber(L, stackPos);
                ++stackPos;
                --count;
                T obj = (T)Convert.ChangeType(d, typeof(T));
                list.Add(obj);
            }

            return list.ToArray();
        }

        public static char[] ToParamsChar(IntPtr L, int stackPos, int count)
        {
            char[] buffer = new char[count];
            int pos = 0;

            while (pos < count)
            {
                char c = (char)LuaDLL.lua_tointeger(L, stackPos);
                buffer[pos++] = c;
                ++stackPos;                                            
            }

            return buffer;
        }

        public static bool[] CheckParamsBool(IntPtr L, int stackPos, int count)
        {
            throw new LuaException("not defined CheckParamsBool");            
        }

        public static T[] CheckParamsNumber<T>(IntPtr L, int stackPos, int count)
        {
            double[] buffer = new double[count];
            int pos = 0;

            while (pos < count)
            {
                if (LuaDLL.lua_isnumber(L, stackPos) != 0)
                {
                    buffer[pos] = LuaDLL.lua_tonumber(L, stackPos);                     
                }
                else
                {
                    LuaDLL.luaL_typerror(L, stackPos, LuaMisc.GetTypeName(typeof(T)));
                    return null;
                }

                ++pos;
                ++stackPos;
            }

            return (T[])Convert.ChangeType(buffer, typeof(T[]));            
        }

        public static char[] CheckParamsChar(IntPtr L, int stackPos, int count)
        {
            char[] buffer = new char[count];
            int pos = 0;

            while (pos < count)
            {
                if (LuaDLL.lua_isnumber(L, stackPos) != 0)
                {
                    buffer[pos] = (char)LuaDLL.lua_tointeger(L, stackPos);                     
                }
                else
                {
                    LuaDLL.luaL_typerror(L, stackPos, "char");
                    return null;
                }

                ++pos;
                ++stackPos;
            }

            return buffer;
        }

        public static T[] CheckParamsObject<T>(IntPtr L, int stackPos, int count) where T : class
        {
            List<T> list = new List<T>(count);
            T obj = default(T);
            Type type = typeof(T);

            while (count > 0)
            {
                object tmp = ToObject(L, stackPos);

                if (TypeChecker.CheckType(L, type, stackPos))
                {
                    obj = (T)tmp;
                    list.Add(obj);
                }
                else
                {
                    LuaDLL.luaL_typerror(L, stackPos, LuaMisc.GetTypeName(type));
                    break;
                }

                --count;
                ++stackPos;
            }

            return list.ToArray();
        }

        public static void Push(IntPtr L, Vector3 v3)
        {
            LuaDLL.tolua_pushvec3(L, v3.x, v3.y, v3.z);
        }

        public static void Push(IntPtr L, Vector2 v2)
        {            
            LuaDLL.tolua_pushvec2(L, v2.x, v2.y);
        }

        public static void Push(IntPtr L, Vector4 v4)
        {            
            LuaDLL.tolua_pushvec4(L, v4.x, v4.y, v4.z, v4.w);
        }

        public static void Push(IntPtr L, Quaternion q)
        {            
            LuaDLL.tolua_pushquat(L, q.x, q.y, q.z, q.w);
        }

        public static void Push(IntPtr L, Color clr)
        {            
            LuaDLL.tolua_pushclr(L, clr.r, clr.g, clr.b, clr.a);
        }

        public static void Push(IntPtr L, Ray ray)
        {
            LuaStatic.GetPackRay(L);
            Push(L, ray.direction);
            Push(L, ray.origin);

            if (LuaDLL.lua_pcall(L, 2, 1, 0) != 0)
            {
                string error = LuaDLL.lua_tostring(L, -1);
                throw new LuaException(error);
            }
        }

        public static void Push(IntPtr L, Bounds bound)
        {                        
            LuaStatic.GetPackBounds(L);
            Push(L, bound.center);
            Push(L, bound.size);

            if (LuaDLL.lua_pcall(L, 2, 1, 0) != 0)
            {
                string error = LuaDLL.lua_tostring(L, -1);
                throw new LuaException(error);
            }
        }

        public static void Push(IntPtr L, RaycastHit hit)
        {
            Push(L, hit, RaycastBits.ALL);
        }

        public static void Push(IntPtr L, RaycastHit hit, int flag)
        {                        
            LuaStatic.GetPackRaycastHit(L);

            if ((flag & RaycastBits.Collider) != 0)
            {
                Push(L, hit.collider);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            LuaDLL.lua_pushnumber(L, hit.distance);

            if ((flag & RaycastBits.Normal) != 0)
            {
                Push(L, hit.normal);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            if ((flag & RaycastBits.Point) != 0)
            {
                Push(L, hit.point);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            if ((flag & RaycastBits.Rigidbody) != 0)
            {            
                Push(L, hit.rigidbody);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            if ((flag & RaycastBits.Transform) != 0)
            {
                Push(L, hit.transform);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            if (LuaDLL.lua_pcall(L, 6, 1, 0) != 0)
            {
                string error = LuaDLL.lua_tostring(L, -1);
                throw new LuaException(error);
            }
        }

        public static void Push(IntPtr L, Touch t)
        {
            Push(L, t, TouchBits.ALL);
        }

        public static void Push(IntPtr L, Touch t, int flag)
        {                                    
            LuaStatic.GetPackTouch(L);
            LuaDLL.lua_pushinteger(L, t.fingerId);

            if ((flag & TouchBits.Position) != 0)
            {
                Push(L, t.position);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            if ((flag & TouchBits.RawPosition) != 0)
            {
                Push(L, t.rawPosition);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            if ((flag & TouchBits.DeltaPosition) != 0)
            {
                Push(L, t.deltaPosition);
            }
            else
            {
                LuaDLL.lua_pushnil(L);
            }

            LuaDLL.lua_pushnumber(L, t.deltaTime);
            LuaDLL.lua_pushinteger(L, t.tapCount);
            LuaDLL.lua_pushinteger(L, (int)t.phase);

            if (LuaDLL.lua_pcall(L, 7, -1, 0) != 0)
            {
                string error = LuaDLL.lua_tostring(L, -1);
                throw new LuaException(error);
            }
        }

        public static void PushLayerMask(IntPtr L, LayerMask l)
        {
            LuaDLL.tolua_pushlayermask(L, l.value);
        }

        public static void Push(IntPtr L, LuaByteBuffer bb)
        {            
            LuaDLL.lua_pushlstring(L, bb.buffer, bb.buffer.Length);
        }

        public static void Push(IntPtr L, Array array)
        {
            if (array == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {                
                int arrayMetaTable = LuaStatic.GetArrayMetatable(L);
                PushUserData(L, array, arrayMetaTable);
            }
        }

        public static void Push(IntPtr L, LuaBaseRef lbr)
        {
            if (lbr == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {                
                LuaDLL.lua_getref(L, lbr.GetReference());
            }
        }

        public static void Push(IntPtr L, Type t)
        {
            if (t == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                int typeMetatable = LuaStatic.GetTypeMetatable(L);
                PushUserData(L, t, typeMetatable);
            }
        }

        public static void Push(IntPtr L, Delegate ev)
        {            
            if (ev == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {                
                int delegateMetatable = LuaStatic.GetDelegateMetatable(L);
                PushUserData(L, ev, delegateMetatable);
            }
        }

        public static void Push(IntPtr L, EventObject ev)
        {
            if (ev == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                int eventMetatable = LuaStatic.GetEventMetatable(L);                
                PushUserData(L, ev, eventMetatable);
            }
        }

        public static void Push(IntPtr L, IEnumerator iter)
        {
            if (iter == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {                
                int iterMetatable = LuaStatic.GetIterMetatable(L);
                PushUserData(L, iter, iterMetatable);                
            }
        }

        public static void Push(IntPtr L, System.Enum e)
        {
            if (e == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                object obj = null;
                int enumMetatable = LuaStatic.GetEnumObject(L, e, out obj);
                PushUserData(L, obj, enumMetatable);
            }
        }

        //基础类型获取需要一个函数
        public static void PushOut<T>(IntPtr L, LuaOut<T> lo)
        {
            ObjectTranslator translator = ObjectTranslator.Get(L);
            int index = translator.AddObject(lo);
            int outMetatable = LuaStatic.GetOutMetatable(L);
            LuaDLL.tolua_pushnewudata(L, outMetatable, index);
        }

        public static void PushValue(IntPtr L, ValueType v)
        {
            if (v == null)
            {
                LuaDLL.lua_pushnil(L);
                return;
            }

            Type type = v.GetType();
            int reference = LuaStatic.GetMetaReference(L, type);
            ObjectTranslator translator = ObjectTranslator.Get(L);

            if (reference > 0)
            {                
                int index = translator.AddObject(v);
                LuaDLL.tolua_pushnewudata(L, reference, index);
            }
            else
            {
                LuaCSFunction LuaOpenLib = LuaStatic.GetPreModule(L, type);

                if (LuaOpenLib != null)
                {
#if UNITY_EDITOR
                    Debugger.LogWarning("register PreLoad type {0} to lua", LuaMisc.GetTypeName(type));
#endif
                    LuaOpenLib(L);
                    reference = LuaStatic.GetMetaReference(L, type);

                    if (reference > 0)
                    {
                        int index = translator.AddObject(v);
                        LuaDLL.tolua_pushnewudata(L, reference, index);
                        return;
                    }
                }

                //类型未Wrap
                LuaDLL.lua_pushnil(L);
                Debugger.LogError("Type {0} not wrap to lua", LuaMisc.GetTypeName(type));
            }
        }


        static void PushUserData(IntPtr L, object o, int reference)
        {
            int index;
            ObjectTranslator translator = ObjectTranslator.Get(L);

            if (translator.Getudata(o, out index))
            {
                if (LuaDLL.tolua_pushudata(L, index))
                {
                    return;
                }
            }

            index = translator.AddObject(o);
            LuaDLL.tolua_pushnewudata(L, reference, index);
        }

        static void LuaPCall(IntPtr L, LuaCSFunction func)
        {            
            LuaDLL.tolua_pushcfunction(L, func);

            if (LuaDLL.lua_pcall(L, 0, -1, 0) != 0)
            {
                string error = LuaDLL.lua_tostring(L, -1);

                Exception last = LuaException.luaStack;
                LuaException.luaStack = null;
                throw new LuaException(error, last);
            }
        }

        static void PushPreLoadType(IntPtr L, object o, Type type)
        {
            LuaCSFunction LuaOpenLib = LuaStatic.GetPreModule(L, type);

            if (LuaOpenLib != null)
            {
#if UNITY_EDITOR
                Debugger.LogWarning("register PreLoad type {0} to lua", LuaMisc.GetTypeName(type));
#endif
                LuaPCall(L, LuaOpenLib);
                int reference = LuaStatic.GetMetaReference(L, type);

                if (reference > 0)
                {
                    PushUserData(L, o, reference);
                    return;
                }
            }

            //类型未Wrap
            LuaDLL.lua_pushnil(L);
            Debugger.LogError("Type {0} not wrap to lua", LuaMisc.GetTypeName(type));
        }
        
        //o 不为 null
        static void PushUserObject(IntPtr L, object o)
        {
            Type type = o.GetType();
            int reference = LuaStatic.GetMetaReference(L, type);

            if (reference > 0)
            {
                PushUserData(L, o, reference);
            }
            else
            {
                PushPreLoadType(L, o, type);
            }
        }

        public static void Push(IntPtr L, UnityEngine.Object obj)
        {
            if (obj == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                PushUserObject(L, obj);
            }
        }

        public static void Push(IntPtr L, UnityEngine.TrackedReference obj)
        {
            if (obj == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                PushUserObject(L, obj);
            }
        }

        public static void PushObject(IntPtr L, object o)
        {
            if (o == null)
            {
                LuaDLL.lua_pushnil(L);
            }
            else
            {
                PushUserObject(L, o);
            }
        }

        /*static void PushNull(IntPtr L)
        {
            LuaDLL.tolua_pushudata(L, 1);
        }*/

        //PushVarObject
        public static void Push(IntPtr L, object obj)
        {
            if (obj == null)
            {
                LuaDLL.lua_pushnil(L);
                return;
            }

            Type t = obj.GetType();

            if (t.IsValueType)
            {
                if (t == typeof(bool))
                {
                    bool b = (bool)obj;
                    LuaDLL.lua_pushboolean(L, b);
                }
                else if (t.IsEnum)
                {
                    Push(L, (System.Enum)obj);
                }
                else if (t.IsPrimitive)
                {
                    double d = LuaMisc.ToDouble(obj);
                    LuaDLL.lua_pushnumber(L, d);
                }
                else if (t == typeof(Vector3))
                {
                    Push(L, (Vector3)obj);
                }
                else if (t == typeof(Quaternion))
                {
                    Push(L, (Quaternion)obj);
                }
                else if (t == typeof(Vector2))
                {
                    Push(L, (Vector2)obj);
                }
                else if (t == typeof(Vector4))
                {
                    Push(L, (Vector4)obj);
                }
                else if (t == typeof(Color))
                {
                    Push(L, (Color)obj);
                }
                else if (t == typeof(RaycastHit))
                {
                    Push(L, (RaycastHit)obj);
                }
                else if (t == typeof(Touch))
                {
                    Push(L, (Touch)obj);
                }
                else if (t == typeof(Ray))
                {
                    Push(L, (Ray)obj);
                }
                else if (t == typeof(Bounds))
                {
                    Push(L, (Bounds)obj);
                }
                else if (t == typeof(LayerMask))
                {
                    Push(L, (LayerMask)obj);
                }
                else
                {
                    PushValue(L, (ValueType)obj);                    
                }
            }
            else
            {
                if (t.IsArray)
                {
                    Push(L, (Array)obj);
                }
                else if(t == typeof(string))
                {
                    LuaDLL.lua_pushstring(L, (string)obj);
                }
                else if (t.IsSubclassOf(typeof(LuaBaseRef)))
                {
                    Push(L, (LuaBaseRef)obj);
                }
                else if (t.IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    Push(L, (UnityEngine.Object)obj);
                }
                else if (t.IsSubclassOf(typeof(UnityEngine.TrackedReference)))
                {
                    Push(L, (UnityEngine.TrackedReference)obj);
                }
                else if (t == typeof(LuaByteBuffer))
                {
                    LuaByteBuffer lbb = (LuaByteBuffer)obj;
                    LuaDLL.lua_pushlstring(L, lbb.buffer, lbb.buffer.Length);
                }
                else if (t.IsSubclassOf(typeof(Delegate)))
                {
                    Push(L, (Delegate)obj);
                }
                else if (obj is System.Collections.IEnumerator)
                {
                    Push(L, (IEnumerator)obj);
                }
                else if (t == typeof(EventObject))
                {
                    Push(L, (EventObject)obj);
                }
                else if (t == monoType)
                {
                    Push(L, (Type)obj);
                }
                else
                {
                    PushObject(L, obj);                    
                }
            }
        }

        public static void SetBack(IntPtr L, int stackPos, object o)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, stackPos);
            ObjectTranslator translator = ObjectTranslator.Get(L);

            if (udata != -1)
            {
                translator.SetBack(udata, o);
            }
        }

        public static int Destroy(IntPtr L)
        {
            int udata = LuaDLL.tolua_rawnetobj(L, 1);
            ObjectTranslator translator = ObjectTranslator.Get(L);
            translator.Destroy(udata);
            return 0;
        }

        public static void CheckArgsCount(IntPtr L, string method, int count)
        {
            int c = LuaDLL.lua_gettop(L);

            if (c != count)
            {
                throw new LuaException(string.Format("no overload for method '{0}' takes '{1}' arguments", method, c));
            }
        }

        public static void CheckArgsCount(IntPtr L, int count)
        {
            int c = LuaDLL.lua_gettop(L);

            if (c != count)
            {
                throw new LuaException(string.Format("no overload for method takes '{0}' arguments", c));
            }
        }  
    }
}