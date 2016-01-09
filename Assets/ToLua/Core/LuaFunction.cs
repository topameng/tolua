using System;
using UnityEngine;

namespace LuaInterface
{
    public enum TracePCall
    {        
        Ignore = 1,            
        Trace = 2,
    }

    public class LuaFunction : LuaBaseRef
    {
        protected int oldTop = -1;
        protected TracePCall trace = TracePCall.Trace;
        private int argCount = 0;
        private int stackPos = -1;        

        public LuaFunction(int reference, LuaState state)
        {
            this.reference = reference;
            this.luaState = state;            
        }

        public override void Dispose()
        {
#if UNITY_EDITOR
            if (oldTop != -1)
            {
                Debugger.LogError("you muse call EndPCall before Dispose it");
            }
#endif
            base.Dispose();
        }

        public virtual int BeginPCall(TracePCall trace = TracePCall.Trace)
        {
#if UNITY_EDITOR
            if (oldTop != -1)
            {
                //整个调用过程未完成被嵌套调用回自己
                Debugger.LogError("you muse call EndPCall before call BeginPCall again");
            }
#endif
            this.trace = trace;
            oldTop = luaState.BeginPCall(reference, trace);
            return oldTop;
        }

        public void PCall()
        {
#if UNITY_EDITOR
            if (oldTop == -1)
            {
                Debugger.LogError("you muse call BeginPCall before PCall");
            }
#endif

            stackPos = oldTop + 1;
            string error = luaState.PCall(argCount, trace != TracePCall.Ignore ? oldTop : 0);
            ThrowException(error);            
        }

        public void EndPCall()
        {
            if (oldTop != -1)
            {
                luaState.EndPCall(oldTop);

                if (trace == TracePCall.Trace)
                {
                    luaState.LuaRemove(oldTop);                    
                }

                oldTop = -1;
                argCount = 0;
                stackPos = -1;
            }
        }

        public void Call()
        {            
            BeginPCall();
            PCall();
            EndPCall();            
        }

        public object[] Call(params object[] args)
        {
            BeginPCall();
            int count = args == null ? 0 : args.Length;

            if (!luaState.CehckStack(count))
            {
                ThrowException("Lua stack overflow");
            }

            PushArgs(args);
            PCall();
            object[] objs = luaState.CheckObjects(oldTop);
            EndPCall();
            return objs;
        }

        public void Push(double num)
        {
            luaState.Push(num);
            ++argCount;
        }

        public void Push(int num)
        {
            luaState.Push(num);
            ++argCount;
        }

        public void PushInt64(LuaInteger64 n64)
        {
            luaState.PushInt64(n64);
            ++argCount;
        }

        public void Push(bool b)
        {
            luaState.Push(b);
            ++argCount;
        }

        public void Push(string str)
        {
            luaState.Push(str);
            ++argCount;
        }

        public void Push(IntPtr ptr)
        {
            luaState.Push(ptr);
            ++argCount;
        }

        public void Push(LuaBaseRef lbr)
        {
            luaState.Push(lbr);
            ++argCount;
        }

        public void Push(object o)
        {
            luaState.Push(o);
            ++argCount;
        }

        public void Push(UnityEngine.Object o)
        {
            luaState.Push(o);
            ++argCount;
        }

        public void Push(Type t)
        {
            luaState.Push(t);
            ++argCount;
        }

        public void Push(ValueType value)
        {
            luaState.Push(value);
            ++argCount;
        }

        public void Push(Enum e)
        {
            luaState.Push(e);
            ++argCount;
        }

        public void Push(Array array)
        {
            luaState.Push(array);
            ++argCount;
        }

        public void Push(Vector3 v3)
        {
            luaState.Push(v3);
            ++argCount;
        }

        public void Push(Vector2 v2)
        {
            luaState.Push(v2);
            ++argCount;
        }

        public void Push(Vector4 v4)
        {
            luaState.Push(v4);
            ++argCount;
        }

        public void Push(Quaternion quat)
        {
            luaState.Push(quat);
            ++argCount;
        }

        public void Push(Color clr)
        {
            luaState.Push(clr);
            ++argCount;
        }

        public void PushLayerMask(LayerMask mask)
        {
            luaState.PushLayerMask(mask);
            ++argCount;
        }

        public void Push(Ray ray)
        {
            string error = null;
            luaState.Push(ray, out error);
            ++argCount;
            ThrowException(error);
        }

        public void Push(Bounds bounds)
        {
            string error = null;
            luaState.Push(bounds, out error);
            ++argCount;
            ThrowException(error);
        }

        public void Push(Touch t)
        {
            string error = null;
            luaState.Push(t, out error);
            ++argCount;
            ThrowException(error);
        }

        public void Push(LuaByteBuffer buffer)
        {
            luaState.Push(buffer);
            ++argCount;
        }

        public void PushObject(object o)
        {
            luaState.PushObject(o);
            ++argCount;
        }

        public void PushArgs(object[] args)
        {
            argCount += args.Length;
            luaState.PushArgs(args);
        }

        public void PushByteBuffer(byte[] buffer)
        {
            luaState.PushByteBuffer(buffer);
            ++argCount;
        }

        void ThrowException(string error)
        {
            if (error != null)
            {
                EndPCall();
                throw new LuaException(error);                
            }            
        }

        public double CheckNumber()
        {
            string error = null;
            double num = luaState.CheckNumber(stackPos++, out error);
            ThrowException(error);
            return num;
        }

        public bool CheckBoolean()
        {
            string error = null;
            bool flag =  luaState.CheckBoolean(stackPos++, out error);
            ThrowException(error);
            return flag;
        }

        public string CheckString()
        {
            string error = null;
            string str = luaState.CheckString(stackPos++, out error);
            ThrowException(error);
            return str;
        }

        public Vector3 CheckVector3()
        {
            string error = null;
            Vector3 v3 = luaState.CheckVector3(stackPos++, out error);
            ThrowException(error);
            return v3;
        }

        public Vector2 CheckVector2()
        {
            string error = null;
            Vector2 v2 = luaState.CheckVector2(stackPos++, out error);
            ThrowException(error);
            return v2;
        }

        public Vector4 CheckVector4()
        {
            string error = null;
            Vector4 v4 = luaState.CheckVector4(stackPos++, out error);
            ThrowException(error);
            return v4;
        }

        public Quaternion CheckQuaternion()
        {
            string error = null;
            Quaternion quat = luaState.CheckQuaternion(stackPos++, out error);
            ThrowException(error);
            return quat;
        }

        public Color CheckColor()
        {
            string error = null;
            Color clr = luaState.CheckColor(stackPos++, out error);
            ThrowException(error);
            return clr;
        }

        public Ray CheckRay()
        {
            string error = null;
            Ray ray = luaState.CheckRay(stackPos++, out error);
            ThrowException(error);
            return ray;
        }

        public Bounds CheckBounds()
        {
            string error = null;
            Bounds bound = luaState.CheckBounds(stackPos++, out error);
            ThrowException(error);
            return bound;            
        }

        public LayerMask CheckLayerMask()
        {
            string error = null;
            LayerMask mask = luaState.CheckLayerMask(stackPos++, out error);
            ThrowException(error);
            return mask;  
        }

        public LuaInteger64 CheckInteger64()
        {
            string error = null;
            LuaInteger64 i64 = luaState.CheckInteger64(stackPos++, out error);
            ThrowException(error);
            return i64;
        }

        public Delegate CheckDelegate()
        {
            string error = null;
            Delegate ev = luaState.CheckDelegate(stackPos++, out error);
            ThrowException(error);
            return ev;
        }

        public object CheckVariant()
        {
            return luaState.ToVariant(stackPos++);
        }

        public char[] CheckCharBuffer()
        {
            string error = null;
            char[] buffer = luaState.CheckCharBuffer(stackPos++, out error);
            ThrowException(error);
            return buffer;
        }

        public byte[] CheckByteBuffer()
        {
            string error = null;
            byte[] buffer = luaState.CheckByteBuffer(stackPos++, out error);
            ThrowException(error);
            return buffer;
        }

        public object CheckObject(Type t)
        {
            string error = null;
            object obj = luaState.CheckObject(stackPos++, t, out error);
            ThrowException(error);
            return obj;
        }

        public LuaFunction CheckLuaFunction()
        {
            string error = null;
            LuaFunction func = luaState.CheckLuaFunction(stackPos++, out error);
            ThrowException(error);
            return func;
        }

        public LuaTable CheckLuaTable()
        {
            string error = null;
            LuaTable table = luaState.CheckLuaTable(stackPos++, out error);
            ThrowException(error);
            return table;
        }

        public LuaThread CheckLuaThread()
        {
            string error = null;
            LuaThread thread = luaState.CheckLuaThread(stackPos++, out error);
            ThrowException(error);
            return thread;
        }
    }
}
