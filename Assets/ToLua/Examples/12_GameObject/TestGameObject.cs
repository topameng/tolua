using UnityEngine;
using System.Collections;
using LuaInterface;

public class TestGameObject: MonoBehaviour
{
    private string script =
        @"                                                
            local Color = UnityEngine.Color
            local GameObject = UnityEngine.GameObject
            local ParticleSystem = UnityEngine.ParticleSystem 

            function OnComplete()
                print('OnComplete CallBack')
            end                              
            
            local go = GameObject('go')
            go:AddComponent(typeof(ParticleSystem))
            local node = go.transform
            node.position = Vector3.one                  
            print('gameObject is: '..tostring(go))                 
            --go.transform:DOPath({Vector3.zero, Vector3.one * 10}, 1, DG.Tweening.PathType.Linear, DG.Tweening.PathMode.Full3D, 10, nil)
            --go.transform:DORotate(Vector3(0,0,360), 2, DG.Tweening.RotateMode.FastBeyond360):OnComplete(OnComplete)            
            GameObject.Destroy(go, 2)                  
            go.name = '123'
            --print('delay destroy gameobject is: '..go.name)                                           
            local go2 = GameObject('go2')
            --GameObject被Destroy的时候，并不代表其在lua环境的proxy(下例中的go2)会被gc立即回收，存在C#端GameObject为null，而lua端相关proxy尚未回收的情况
            --所以此时就得利用tolua.isnull来判断GameObject是否为“空”
            GameObject.Destroy(go2)
            print(go2 == nil)
            print(tolua.isnull(go2))
        ";

    LuaState lua = null;

    void Start()
    {
#if UNITY_4_6 || UNITY_4_7
        Application.RegisterLogCallback(ShowTips);
#else
        Application.logMessageReceived += ShowTips;
#endif
        new LuaResLoader();
        lua = new LuaState();
        lua.LogGC = true;
        lua.Start();
        LuaBinder.Bind(lua);
        lua.DoString(script, "TestGameObject.cs");
    }

    void Update()
    {
        lua.CheckTop();
        lua.Collect();        
    }

    string tips = "";
        
    void ShowTips(string msg, string stackTrace, LogType type)
    {
        tips += msg;
        tips += "\r\n";
    }

    void OnApplicationQuit()
    {        
        lua.Dispose();
        lua = null;
#if UNITY_4_6 || UNITY_4_7
        Application.RegisterLogCallback(null);

#else
        Application.logMessageReceived -= ShowTips;
#endif 
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 300, 600, 600), tips);
    }
}
