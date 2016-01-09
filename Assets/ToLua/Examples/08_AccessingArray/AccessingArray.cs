using UnityEngine;
using LuaInterface;

public class AccessingArray : MonoBehaviour 
{
    private string script =
        @"
            function TestArray(strs)
                local len = strs.Length
                
                for i = 0, len - 1 do
                    print(strs[i])
                end
                return 1, '123', true
            end            
        ";

    void Start()
    {
        LuaState lua = new LuaState();
        lua.Start();
        lua.DoString(script);

        string[] strs = { "aaa", "bbb", "ccc" };
        LuaFunction func = lua.GetFunction("TestArray");

        func.BeginPCall();
        func.Push(strs);
        func.PCall();
        double arg1 = func.CheckNumber();
        string arg2 = func.CheckString();
        bool arg3 = func.CheckBoolean();
        Debugger.Log("return is {0} {1} {2}", arg1, arg2, arg3);
        func.EndPCall();

        //转换一下类型，避免可变参数拆成多个参数传递
        object[] objs = func.Call((object)strs);

        if (objs != null)
        {
            Debugger.Log("return is {0} {1} {2}", objs[0], objs[1], objs[2]);
        }

        lua.CheckTop();
        func.Dispose();
        lua.Dispose();
    }
}
