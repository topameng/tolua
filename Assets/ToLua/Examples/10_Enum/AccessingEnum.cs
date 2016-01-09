using UnityEngine;
using System;
using LuaInterface;

public class AccessingEnum : MonoBehaviour 
{
    string script =
        @"
            space = nil

            function TestEnum(e)        
                print('Enum is:'..tostring(e))        

                if space:ToInt() == 0 then
                    print('enum ToInt() is ok')                
                end

                if not space:Equals(0) then
                    print('enum compare int is ok')                
                end

                if space == e then
                    print('enum compare enum is ok')
                end

                local s = UnityEngine.Space.IntToEnum(0)

                if space == s then
                    print('IntToEnum change type is ok')
                end
            end

            function ChangeLightType(light, type)
                print('change light type to Directional')
                light.type = UnityEngine.LightType.Directional
            end
        ";

	void Start () 
    {
        LuaState state = new LuaState();
        state.Start();
        LuaBinder.Bind(state);

        state.DoString(script);
        state["space"] = Space.World;

        LuaFunction func = state.GetFunction("TestEnum");
        func.BeginPCall();
        func.Push(Space.World);
        func.PCall();
        func.EndPCall();
        func.Dispose();
        func = null;

        GameObject go = GameObject.Find("/Light");
        Light light = go.GetComponent<Light>();
        func = state.GetFunction("ChangeLightType");
        func.BeginPCall();
        func.Push(light);
        func.Push(LightType.Directional);
        func.PCall();
        func.EndPCall();
        func.Dispose();
        func = null;
 
        state.CheckTop();
        state.Dispose();
        state = null;
	}
}
