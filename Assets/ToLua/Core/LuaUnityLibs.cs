using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace LuaInterface
{
    public sealed class LuaUnityLibs
    {
        public static void OpenLibs(IntPtr L)
        {
            InitMathf(L);
            InitLayer(L);
            //InitTouchBits(L);
            //InitRaycastBits(L);
        }

        public static void OpenLuaLibs(IntPtr L)
        {
            LuaDLL.lua_pushstdcallcfunction(L, LuaDLL.tolua_openlualibs);

            if (LuaDLL.lua_pcall(L, 0, -1, 0) != 0)
            {
                string error = LuaDLL.lua_tostring(L, -1);
                LuaDLL.lua_pop(L, 1);
                throw new LuaException(error);
            }

            SetOutMethods(L, "Vector3", GetOutVector3);
            SetOutMethods(L, "Vector2", GetOutVector2);
            SetOutMethods(L, "Vector4", GetOutVector4);
            SetOutMethods(L, "Color", GetOutColor);
            SetOutMethods(L, "Quaternion", GetOutQuaternion);
            SetOutMethods(L, "Ray", GetOutRay);
            SetOutMethods(L, "Bounds", GetOutBounds);
            SetOutMethods(L, "Touch", GetOutTouch);
            SetOutMethods(L, "RaycastHit", GetOutRaycastHit);
            SetOutMethods(L, "LayerMask", GetOutLayerMask);            
        }

        static void InitMathf(IntPtr L)
        {
            LuaDLL.lua_getglobal(L, "Mathf");
            LuaDLL.lua_pushstring(L, "PerlinNoise");
            LuaDLL.lua_pushstdcallcfunction(L, PerlinNoise);
            LuaDLL.lua_rawset(L, -3);
            LuaDLL.lua_pop(L, 1);
        }

        static void InitLayer(IntPtr L)
        {
            LuaDLL.tolua_createtable(L, "Layer");

            for (int i = 0; i < 32; i++)
            {
                string str = LayerMask.LayerToName(i);

                if (!string.IsNullOrEmpty(str))
                {
                    LuaDLL.lua_pushstring(L, str);
                    LuaDLL.lua_pushinteger(L, i);
                    LuaDLL.lua_rawset(L, -3);
                }
            }

            LuaDLL.lua_pop(L, 1);
        }

        /*static void InitTouchBits(IntPtr L)
        {
            LuaDLL.tolua_createtable(L, "TouchBits");
            LuaDLL.lua_pushinteger(L, TouchBits.Position);
            LuaDLL.lua_setfield(L, -2, "position");
            LuaDLL.lua_pushinteger(L, TouchBits.RawPosition);
            LuaDLL.lua_setfield(L, -2, "rawPosition");
            LuaDLL.lua_pushinteger(L, TouchBits.DeltaPosition);
            LuaDLL.lua_setfield(L, -2, "deltaPosition");
            LuaDLL.lua_pushinteger(L, TouchBits.ALL);
            LuaDLL.lua_setfield(L, -2, "all");     
            LuaDLL.lua_pop(L, 1);
        }

        static void InitRaycastBits(IntPtr L)
        {
            LuaDLL.tolua_createtable(L, "RaycastBits");
            LuaDLL.lua_pushinteger(L, RaycastBits.Collider);
            LuaDLL.lua_setfield(L, -2, "collider");
            LuaDLL.lua_pushinteger(L, RaycastBits.Normal);
            LuaDLL.lua_setfield(L, -2, "normal");
            LuaDLL.lua_pushinteger(L, RaycastBits.Point);
            LuaDLL.lua_setfield(L, -2, "point");
            LuaDLL.lua_pushinteger(L, RaycastBits.Rigidbody);
            LuaDLL.lua_setfield(L, -2, "rigidbody");
            LuaDLL.lua_pushinteger(L, RaycastBits.Transform);
            LuaDLL.lua_setfield(L, -2, "transform");
            LuaDLL.lua_pushinteger(L, RaycastBits.ALL);
            LuaDLL.lua_setfield(L, -2, "all");
            LuaDLL.lua_pop(L, 1);
        }*/

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int PerlinNoise(IntPtr L)
        {
            float x = (float)LuaDLL.luaL_checknumber(L, 1);
            float y = (float)LuaDLL.luaL_checknumber(L, 2);
            float ret = Mathf.PerlinNoise(x, y);
            LuaDLL.lua_pushnumber(L, ret);
            return 1;
        }

        static void SetOutMethods(IntPtr L, string table, LuaCSFunction getOutFunc = null)
        {
            LuaDLL.lua_getglobal(L, table);
            IntPtr get = Marshal.GetFunctionPointerForDelegate(getOutFunc);
            LuaDLL.tolua_variable(L, "out", get, IntPtr.Zero);
            
            LuaDLL.lua_pop(L, 1);
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int GetOutVector3(IntPtr L)
        {            
            ToLua.PushOut<Vector3>(L, new LuaOut<Vector3>());
            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int GetOutVector2(IntPtr L)
        {            
            ToLua.PushOut<Vector2>(L, new LuaOut<Vector2>());
            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int GetOutVector4(IntPtr L)
        {            
            ToLua.PushOut<Vector4>(L, new LuaOut<Vector4>());
            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int GetOutColor(IntPtr L)
        {            
            ToLua.PushOut<Color>(L, new LuaOut<Color>());
            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int GetOutQuaternion(IntPtr L)
        {            
            ToLua.PushOut<Quaternion>(L, new LuaOut<Quaternion>());
            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int GetOutRay(IntPtr L)
        {
            ToLua.PushOut<Ray>(L, new LuaOut<Ray>());
            return 1;            
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int GetOutBounds(IntPtr L)
        {
            ToLua.PushOut<Bounds>(L, new LuaOut<Bounds>());
            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int GetOutRaycastHit(IntPtr L)
        {
            ToLua.PushOut<RaycastHit>(L, new LuaOut<RaycastHit>());
            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int GetOutTouch(IntPtr L)
        {            
            ToLua.PushOut<Touch>(L, new LuaOut<Touch>());
            return 1;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int GetOutLayerMask(IntPtr L)
        {
            ToLua.PushOut<LayerMask>(L, new LuaOut<LayerMask>());
            return 1;
        }
    }
}
