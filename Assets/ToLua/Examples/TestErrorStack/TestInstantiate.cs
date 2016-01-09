using UnityEngine;
using System;
using LuaInterface;

public class TestInstantiate : MonoBehaviour 
{
    void Awake()
    {
        LuaState state = LuaState.Get(IntPtr.Zero);

        LuaFunction func = state.GetFunction("Show");
        func.BeginPCall(TracePCall.Trace);
        func.PCall();
        func.EndPCall();
        func.Dispose();
        func = null;
    }
}
