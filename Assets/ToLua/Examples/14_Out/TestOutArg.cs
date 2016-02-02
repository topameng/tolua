using UnityEngine;
using System.Collections;
using LuaInterface;

public class TestOutArg : MonoBehaviour 
{            
    string script =
        @"
            local layer = 2 ^ LayerMask.NameToLayer('Default')

            function TestPick(ray)                
                local flag, hit = UnityEngine.Physics.Raycast(ray, nil, 5000, layer)                
                --local flag, hit = UnityEngine.Physics.Raycast(ray, RaycastHit.out, 5000, layer)
                
                if flag then
                    print('pick from lua, point: '..tostring(hit.point))
                end
            end
        ";

    LuaState state = null;
    LuaFunction func = null;

	void Start () 
    {
        state = new LuaState();
        state.Start();
        LuaBinder.Bind(state);
        state.DoString(script);

        func = state.GetFunction("TestPick");
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera camera = Camera.main;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool flag = Physics.Raycast(ray, out hit, 5000, 1 << LayerMask.NameToLayer("Default"));

            if (flag)
            {
                Debugger.Log("pick from c#, point: [{0}, {1}, {2}]", hit.point.x, hit.point.y, hit.point.z);
            }

            func.BeginPCall();
            func.Push(ray);
            func.PCall();
            func.EndPCall();
        }

        state.CheckTop();
        state.Collect();
    }

    void OnDestroy()
    {
        func.Dispose();
        func = null;

        state.Dispose();
        state = null;
    }
}
