using System;
using UnityEngine;
using System.Collections;
using LuaInterface;

public class TestPerformance : MonoBehaviour 
{
    LuaState state = null;        
    private string tips = "";

	void Start () 
    {        
#if UNITY_5		
		Application.logMessageReceived += ShowTips;
#else
        Application.RegisterLogCallback(ShowTips);
#endif         
        new LuaResLoader();          
        state = new LuaState();
        state.Start();
        LuaBinder.Bind(state);                       
        state.DoFile("TestPerf.lua");        
        state.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        state.LogGC = false;        
	}

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        tips += msg;
        tips += "\r\n";
    }

    void OnDestroy()
    {
#if UNITY_5		
		Application.logMessageReceived -= ShowTips;
#else
        Application.RegisterLogCallback(null);
#endif
        state.Dispose();
        state = null;
    }

    //int lastFrameCount = 0;

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 100, 400, 300), tips);

        if (GUI.Button(new Rect(50, 50, 120, 45), "Test1"))
        {
            float time = Time.realtimeSinceStartup;            

            for (int i = 0; i < 200000; i++)
            {
                Vector3 v = transform.position;
                transform.position = v + Vector3.one;
            }

            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debugger.Log("c# Transform getset cost time: " + time);            
            transform.position = Vector3.zero;

            LuaFunction func = state.GetFunction("Test1");
            func.BeginPCall();
            func.Push(transform);
            func.PCall();
            func.EndPCall();
            func.Dispose();
            func = null;            
        }
        else if (GUI.Button(new Rect(50, 150, 120, 45), "Test2"))
        {
            float time = Time.realtimeSinceStartup;

            for (int i = 0; i < 200000; i++)
            {
                transform.Rotate(Vector3.up, 1);
            }

            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debugger.Log("c# Transform.Rotate cost time: " + time);

            LuaFunction func = state.GetFunction("Test2");
            func.BeginPCall();
            func.Push(transform);
            func.PCall();
            func.EndPCall();
            func.Dispose();
            func = null;    
        }
        else if (GUI.Button(new Rect(50, 250, 120, 45), "Test3"))
        {
            float time = Time.realtimeSinceStartup;            

            for (int i = 0; i < 2000000; i++)
            {
                new Vector3(i, i, i);
            }

            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debugger.Log("c# new Vector3 cost time: " + time);            

            LuaFunction func = state.GetFunction("Test3");
            func.Call();
            func.Dispose();
            func = null;  
        }
        else if (GUI.Button(new Rect(50, 350, 120, 45), "Test4"))
        {
            float time = Time.realtimeSinceStartup;
            
            for (int i = 0; i < 20000; i++)
            {
                new GameObject();
            }

            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debugger.Log("c# new GameObject cost time: " + time);

            //光gc了
            LuaFunction func = state.GetFunction("Test4");
            func.Call();         
            func.Dispose();
            func = null;  
        }
        else if (GUI.Button(new Rect(50, 550, 120, 40), "Test7"))
        {            
            float time = Time.realtimeSinceStartup;
            Vector3 v1 = Vector3.zero;

            for (int i = 0; i < 200000; i++)
            {
                Vector3 v = new Vector3(i,i,i);
                v = Vector3.Normalize(v);
                v1 = v + v1;
            }

            time = Time.realtimeSinceStartup - time;            
            tips = "";
            Debugger.Log("Vector3 New Normalize cost: " + time);
            LuaFunction func = state.GetFunction("Test7");
            func.Call();
            func.Dispose();
            func = null;  
        }
        else if (GUI.Button(new Rect(250, 50, 120, 40), "Test8"))
        {
            float time = Time.realtimeSinceStartup;            

            for (int i = 0; i < 200000; i++)
            {
		        Quaternion q1 = Quaternion.Euler(i, i, i);
                Quaternion q2 = Quaternion.Euler(i * 2, i * 2, i * 2);
                Quaternion.Slerp(q1, q2, 0.5f);
            }

            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debugger.Log("Quaternion Euler Slerp cost: " + time);            
            
            LuaFunction func = state.GetFunction("Test8");
            func.Call();
            func.Dispose();
            func = null;
        }
        else if (GUI.Button(new Rect(250, 150, 120, 40), "Test9"))
        {
            tips = "";
            LuaFunction func = state.GetFunction("Test9");
            func.Call();
            func.Dispose();
            func = null;
        }

        state.CheckTop();
        state.Collect();
    }
}
