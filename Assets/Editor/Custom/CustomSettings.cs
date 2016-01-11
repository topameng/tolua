using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;

using BindType = ToLuaMenu.BindType;

public static class CustomSettings
{
    public static string saveDir = Application.dataPath + "/Source/Generate/";
    public static string luaDir = Application.dataPath + "/Lua/";
    public static string toluaBaseType = Application.dataPath + "/ToLua/BaseType/";


    //导出时强制做为静态类的类型(注意customTypeList 还要添加这个类型才能导出)
    //unity 有些类作为sealed class, 其实完全等价于静态类
    public static List<Type> staticClassTypes = new List<Type>
    {        
        typeof(UnityEngine.Application),
        typeof(UnityEngine.Time),
        typeof(UnityEngine.Screen),
        typeof(UnityEngine.SleepTimeout),
        typeof(UnityEngine.Input),
        typeof(UnityEngine.Resources),
        typeof(UnityEngine.Physics),
        typeof(UnityEngine.RenderSettings),
    };

    //附加导出委托类型(在导出委托时, customTypeList 中牵扯的委托类型都会导出， 无需写在这里)
    public static DelegateType[] customDelegateList = 
    {        
        _DT(typeof(Action)),
        _DT(typeof(Action<GameObject>)),
        _DT(typeof(UnityEngine.Events.UnityAction)),       
        
        _DT(typeof(TestEventListener.OnClick)),
        _DT(typeof(TestEventListener.VoidDelegate)),
    };

    //在这里添加你要导出注册到lua的类型列表
    public static BindType[] customTypeList = 
    {                
        //------------------------为例子导出--------------------------------
        //_GT(typeof(TestEventListener)),                
        //_GT(typeof(TestAccount)),
        //_GT(typeof(Dictionary<int, TestAccount>)).SetLibName("AccountMap"),                
        //_GT(typeof(KeyValuePair<int, TestAccount>)),    
        //-------------------------------------------------------------------
        
        _GT(typeof(Debugger)),                       
                                       
        _GT(typeof(Component)),
        _GT(typeof(Behaviour)),
        _GT(typeof(MonoBehaviour)),        
        _GT(typeof(GameObject)),
        _GT(typeof(Transform)),
        _GT(typeof(Space)),

        _GT(typeof(Camera)),   
        _GT(typeof(CameraClearFlags)),           
        _GT(typeof(Material)),
        _GT(typeof(Renderer)),        
        _GT(typeof(MeshRenderer)),
        _GT(typeof(SkinnedMeshRenderer)),
        _GT(typeof(Light)),
        _GT(typeof(LightType)),     
        _GT(typeof(ParticleEmitter)),
        _GT(typeof(ParticleRenderer)),
        _GT(typeof(ParticleAnimator)),   
        _GT(typeof(ParticleSystem)),                
        _GT(typeof(Physics)),
        _GT(typeof(Collider)),
        _GT(typeof(BoxCollider)),
        _GT(typeof(MeshCollider)),
        _GT(typeof(SphereCollider)),        
        _GT(typeof(CharacterController)),
        _GT(typeof(Animation)),             
        _GT(typeof(AnimationClip)),
        _GT(typeof(TrackedReference)),
        _GT(typeof(AnimationState)),  
        _GT(typeof(QueueMode)),  
        _GT(typeof(PlayMode)),                          
        _GT(typeof(AudioClip)),
        _GT(typeof(AudioSource)),                        
        _GT(typeof(Application)),
        _GT(typeof(Input)),              
        _GT(typeof(KeyCode)),             
        _GT(typeof(Screen)),
        _GT(typeof(Time)),
        _GT(typeof(RenderSettings)),
        _GT(typeof(SleepTimeout)),                        
        _GT(typeof(AsyncOperation)),
        _GT(typeof(AssetBundle)),   
        _GT(typeof(BlendWeights)),   
        _GT(typeof(QualitySettings)),          
        _GT(typeof(AnimationBlendMode)),  
        _GT(typeof(RenderTexture)),
        _GT(typeof(Rigidbody)), 
        _GT(typeof(CapsuleCollider)),
        _GT(typeof(WrapMode)),
        _GT(typeof(Texture)),
        _GT(typeof(Shader)),
        _GT(typeof(Texture2D)),
        _GT(typeof(WWW)),
    };

    static BindType _GT(Type t)
    {
        return new BindType(t);
    }

    static DelegateType _DT(Type t)
    {
        return new DelegateType(t);
    }    
}
