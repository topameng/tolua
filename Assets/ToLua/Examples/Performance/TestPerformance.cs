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
        Application.RegisterLogCallback(ShowTips);           
        new LuaResLoader();          
        state = new LuaState();
        state.Start();
        LuaBinder.Bind(state);                       
        state.DoFile("Test.lua");        
        state.LuaGC(LuaGCOptions.LUA_GCCOLLECT);      
	}

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        tips += msg;             
    }

    void OnDestroy()
    {
        Application.RegisterLogCallback(null);
        state.Dispose();
        state = null;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2, 300, 200), tips);

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
            Debug.Log("c# Transform getset cost time: " + time);
            tips += "\r\n";   
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
            Debug.Log("c# Transform.Rotate cost time: " + time);
            tips += "\r\n";   

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
            Debug.Log("c# new Vector3 cost time: " + time);
            tips += "\r\n";   

            LuaFunction func = state.GetFunction("Test3");
            func.Call();
            func.Dispose();
            func = null;  
        }
        else if (GUI.Button(new Rect(50, 350, 120, 45), "Test4"))
        {
            float time = Time.realtimeSinceStartup;

            for (int i = 0; i < 200000; i++)
            {
                new GameObject();
            }

            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debug.Log("c# new GameObject cost time: " + time);
            tips += "\r\n";   

            LuaFunction func = state.GetFunction("Test4");
            func.Call();         
            func.Dispose();
            func = null;  
        }
        else if (GUI.Button(new Rect(50, 450, 120, 45), "Test5"))
        {
            float time = Time.realtimeSinceStartup;

            for (int i = 0; i < 20000; i++)
            {
                GameObject go = new GameObject();
                go.AddComponent<SkinnedMeshRenderer>();
                SkinnedMeshRenderer sm = go.GetComponent<SkinnedMeshRenderer>();
                sm.castShadows = false;
                sm.receiveShadows = false;
            }

            time = Time.realtimeSinceStartup - time;
            tips = "";
            Debug.Log("Test5 c# cost time: " + time);
            tips += "\r\n";   

            LuaFunction func = state.GetFunction("Test5");
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
            Debug.Log("Vector3 New Normalize cost: " + time);
            tips += "\r\n";   
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
            Debug.Log("Quaternion Euler Slerp cost: " + time);
            tips += "\r\n";
            
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
    }
}
