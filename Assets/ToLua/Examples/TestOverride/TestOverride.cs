using UnityEngine;
using System.Collections;
using LuaInterface;

public class TestExport
{
    [LuaByteBufferAttribute]
    public delegate void TestBuffer(byte[] buffer);       

    public enum Space
    {
        World = 1
    }

    //public int Item { get; set; }

    public int this[int pos]
    {
        get { return pos; }
        set { Debugger.Log(value); }
    }

    [LuaByteBufferAttribute]
    public byte[] buffer;

    public static int get_Item(string pos) { return 0; }
    public static int get_Item(double pos) { return 0; }

    public static int set_Item(double pos) { return 0; }

    public void TestByteBuffer(TestBuffer tb)
    {

    }

    public int Test(object o, string str)
    {
        Debug.Log("call Test(object o, string str)");
        return 1;
    }

    public int Test(char c)
    {
        Debug.Log("call Test(char c)");
        return 2;
    }

    public int Test(int i)
    {
        Debug.Log("call Test(int i)");
        return 3;
    }

    //有这个函数要扔掉上面两个精度不匹配的，因为lua是double
    public int Test(double d)
    {
        Debug.Log("call Test(double d)");
        return 4;
    }

    public int Test(int i, int j)
    {
        Debug.Log("call Test(int i, int j)");
        return 5;
    }


    public int Test(string str)
    {
        Debug.Log("call Test(string str)");
        return 6;
    }

    public static int Test(string str1, string str2)
    {
        Debug.Log("call static Test(string str1, string str2)");
        return 7;
    }

    public int Test(object o)
    {
        Debug.Log("call Test(object o)");
        return 8;
    }

    public int Test(params object[] objs)
    {
        Debug.Log("call Test(params object[] objs)");
        return 9;
    }

    public int Test(Space e)
    {
        Debug.Log("call Test(TestEnum e)");
        return 10;
    }
}

public class TestOverride : MonoBehaviour
{
    private string script =
@"                  
        function Test(to)
            assert(to:Test(1) == 4)
            assert(to:Test('hello') == 6)
            assert(to:Test(System.Object.New()) == 8)
            assert(to:Test(123, 456) == 5)            
            assert(to:Test('123', '456') == 1)
            assert(to:Test(System.Object.New(), '456') == 1)
            assert(to:Test('123', 456) == 9)
            assert(to:Test('123', System.Object.New()) == 9)
            assert(to:Test(1,2,3) == 9)            
            --assert(to:Test(TestExport.Space.World) == 10)        
            print(to.this:get(123))
            to.this:set(1, 456)
        end
    ";
    
    void Awake ()
    {
        LuaState state = new LuaState();
        state.Start();

        Bind(state);
        state.DoString(script, "TestOverride.cs");

        TestExport to = new TestExport();
        LuaFunction func = state.GetFunction("Test");
        func.Call(to);
    }

    void Bind(LuaState state)
    {
        state.BeginModule(null);
        TestExportWrap.Register(state);
        state.BeginModule("TestExport");
        //TestExport_SpaceWrap.Register(state);
        state.EndModule();
        state.EndModule();
    }
}
