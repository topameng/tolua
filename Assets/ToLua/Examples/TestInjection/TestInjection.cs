using UnityEngine;
using LuaInterface;
using System.Collections;

public class TestInjection : MonoBehaviour
{
    string tips = "";
    LuaState luaState = null;

    // Use this for initialization
    void Start()
    {
#if UNITY_5 || UNITY_2017
        Application.logMessageReceived += ShowTips;
#else
        Application.RegisterLogCallback(ShowTips);
#endif
        new LuaResLoader();
        luaState = new LuaState();
        luaState.Start();
        LuaBinder.Bind(luaState);
        luaState.Require("ToLuaInjectionTestInjector");

        int counter = 0;
        bool state = true;
        ToLuaInjectionTest test = new ToLuaInjectionTest(true);
        test = new ToLuaInjectionTest();
        StartCoroutine(test.TestCoroutine(0.3f));
        test.TestOverload(1, state);
        test.TestOverload(1, ref state);
        Debug.Log("ref Test Result :" + state);
        test.TestOverload(state, 1);
        Debug.Log("ref Test Return Value :" + test.TestRef(ref counter));
        Debug.Log("ref Test Result :" + counter);
        Debug.Log("Property Get Test:" + test.PropertyTest);
        test.PropertyTest = 2;
        Debug.Log("Property Set Test:" + test.PropertyTest);
    }

    void OnApplicationQuit()
    {
#if UNITY_5 || UNITY_2017
        Application.logMessageReceived -= ShowTips;
#else
        Application.RegisterLogCallback(null);
#endif
        luaState.Dispose();
        luaState = null;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 150, 800, 400), tips);
    }

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        tips += msg;
        tips += "\r\n";

        if (type == LogType.Error || type == LogType.Exception)
        {
            tips += stackTrace;
        }
    }
}
