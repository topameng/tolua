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
            
            local go = GameObject('go', typeof(UnityEngine.Camera))
            go:AddComponent(typeof(ParticleSystem))
            local node = go.transform
            node.position = Vector3.one      
            print('gameObject is: '..tostring(go))                 
            --go.transform:DOPath({Vector3.zero, Vector3.one * 10}, 1, DG.Tweening.PathType.Linear, DG.Tweening.PathMode.Full3D, 10, nil)
            --go.transform:DORotate(Vector3(0,0,360), 2, DG.Tweening.RotateMode.FastBeyond360):OnComplete(OnComplete)
            GameObject.Destroy(go, 2)                  
            print('delay destroy gameobject is: '..go.name)                                           
        ";

    LuaState lua = null;

    void Start()
    {        
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

    void OnApplicationQuit()
    {        
        lua.Dispose();
        lua = null;   
    }
}
