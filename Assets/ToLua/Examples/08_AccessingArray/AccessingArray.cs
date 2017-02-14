using UnityEngine;
using LuaInterface;

public class AccessingArray : MonoBehaviour 
{
    private string script =
        @"
            function TestArray(array)
                local len = array.Length
                
                for i = 0, len - 1 do
                    print('Array: '..tostring(array[i]))
                end

                local t = array:ToTable()                
                
                for i = 1, #t do
                    print('table: '.. tostring(t[i]))
                end

                local iter = array:GetEnumerator()

                while iter:MoveNext() do
                    print('iter: '..iter.Current)
                end

                local pos = array:BinarySearch(3)
                print('array BinarySearch: pos: '..pos..' value: '..array[pos])

                pos = array:IndexOf(4)
                print('array indexof bbb pos is: '..pos)
                
                return 1, '123', true
            end            
        ";

    LuaState lua = null;
    LuaFunction func = null;

    void Start()
    {
        lua = new LuaState();
        lua.Start();
        lua.DoString(script, "AccessingArray.cs");

        int[] array = { 1, 2, 3, 4, 5};        
        func = lua.GetFunction("TestArray");

        func.BeginPCall();
        func.Push(array);
        func.PCall();
        double arg1 = func.CheckNumber();
        string arg2 = func.CheckString();
        bool arg3 = func.CheckBoolean();
        Debugger.Log("return is {0} {1} {2}", arg1, arg2, arg3);
        func.EndPCall();

        //转换一下类型，避免可变参数拆成多个参数传递
        object[] objs = func.Call((object)array);

        if (objs != null)
        {
            Debugger.Log("return is {0} {1} {2}", objs[0], objs[1], objs[2]);
        }

        lua.CheckTop();                
    }

    void OnApplicationQuit()
    {
        func.Dispose();
        lua.Dispose();
    }
}
