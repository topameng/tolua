using UnityEngine;
using System.Collections;
using LuaInterface;
using System.Collections.Generic;
using System;
using System.Reflection;


public class MyTest : MonoBehaviour 
{
    private string script =
        @"  function LuaFunc(num)                        
                return num + 1
            end

            test = {}
            test.LuaFunc = LuaFunc    

            local s1 = slot(test.LuaFunc)
            local s2 = slot(test.LuaFunc)
            
            if s1 == s2 then
                print('wooooooooooooooooooo')
                s1(1234566666)
            end            

            local GameObject = UnityEngine.GameObject
            gameObject = nil

            map = {1, '123', true, nil, ['kk'] = {}}   
            map.__call = function(self, ...)
                local args = {...}
                for i,k in ipairs(args) do
                    print('map call arg'..i..' is '..tostring(k))
                end
            end     

            function TestEvent(go)                
                print('hello world')
            end

            function Test2(ev)                     
                local e = ev + TestEvent                
                return e
            end

            function Test3(ev)
                local e = ev - TestEvent                
                return e
            end            

            function Test4()
                error('this is a test')                
            end

            function TestNull()                
                if null == gameObject then
                    print('goooooooooooooooooo')     
                elseif tolua.isnull(gameObject) then
                    print('foooooooooooooooooo')                               
                end
            end
                        
            --local go = GameObject.Find('/Testnull')
            
            --if go ~= null then
            --    go.transform.parent = null               
            --else
            --    print('kooooooooooooo')
            --end

            function TestRay(ray)
                print('ray', tostring(ray))  
                ray.origin.x = 2                              
                return ray
            end

            function TestColur()
                TestIndex.Test3()
                --return function() print('123') end
            end
            
            function TestBug()
                local co = coroutine.create(function()		                
                print('what a fuck1')      
                local go = GameObject.New(123)                                          
                end)
                
	            local r, msg = coroutine.resume(co)	                             
            end
            
    
            --print('test enum', tostring(UnityEngine.Space.World))        
            --local str = System.String.New('abcdefg')  
            --local _C = string.byte              
            --str = str:TrimStart(_C('a'), _C('b'), _C('e'))
            --print(tostring(str))
        ";

    static LuaState state = null;    

    void TestGC(LuaState state)
    {
        LuaFunction func;
        Action<GameObject> ev = delegate { };
        func = state.GetFunction("Test2");
        func.BeginPCall();
        func.Push(ev);
        func.PCall();
        ev = func.CheckDelegate() as Action<GameObject>;
        func.EndPCall();
        ev(gameObject);
        func.Dispose();
        func = state.GetFunction("Test3");
        func.BeginPCall();
        func.Push(ev);
        func.PCall();
        ev = func.CheckDelegate() as Action<GameObject>;
        ev(gameObject);
        func.Dispose();

        //ev = null;        
        //LuaDLL.lua_gc(state.L, LuaGCOptions.LUA_GCCOLLECT, 0);
        //GC.Collect();

        
    }

    void Awake()
    {                
       
    }

    static int TestCrash(IntPtr L)
    {
        Debug.Log("见证奇迹的时刻");
        return 0;
    }

    [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Test3(IntPtr L)
    {
        //try
        //{
            //func = state.GetFunction("TestRay");
            //func.BeginPCall();
            //func.Push(new Ray(Vector3.one, Vector3.one));
            //func.PCall();            
            //func.CheckRay();            
            //func.EndPCall();
        //}
        //catch(Exception e)
        //{
        //    LuaDLL.luaL_error(L, e.Message);            
        //}

        func = state.GetFunction("TestBug");
        func.BeginPCall();
        func.Call();        
        func.EndPCall();
        func.Dispose();

        return 0;
    }

	void Start () 
    {                            
        state = new LuaState();
        state.Start();

        state.BeginModule(null);
        state.BeginClass(typeof(TestIndex), null);
        state.RegFunction("Test3", Test3);
        state.EndClass();
        state.EndModule();

        //GameObject go = new GameObject("123");
        state.DoString(script, "MyTest");
        //state["gameObject"] = go;
        //func = state.GetFunction("TestNull");        
        //func.Call();
        //UnityEngine.Object.Destroy(go);
        //func.Call();
        //func.Dispose();
        //func = null;

        LuaFunction func1 = state.GetFunction("TestBug");
        func1.Call();

        //func = state.GetFunction("TestRay");
        //func.BeginPCall();
        //func.Push(new Ray(Vector3.one, Vector3.one));
        //func.PCall();        
        //Ray ray = func.CheckRay();
        //Debug.Log(ray);
        //func.EndPCall();


        //func = state.GetFunction("TestRay");        
        //func.BeginPCall();        
        //func.Push(new Ray(Vector3.one, Vector3.one));

        //if (func.PCall())
        //{            
        //    Ray ray = func.CheckRay();            
        //}

        //func.EndPCall();

        

        /*LuaTable table = state.GetTable("UnityEngine.Space");
        LuaTable t1 = table.GetMetaTable();

        LuaDictTable dict = t1.ToDictTable();
        t1.Dispose();
        var iter2 = dict.GetEnumerator();

        while (iter2.MoveNext())
        {
            Debugger.Log("map item, k,v is {0}:{1}", iter2.Current.Key, iter2.Current.Value);
        }

        iter2.Dispose();
        dict.Dispose();


        table.Dispose();
        
        state.PrintTable("test");

        Type tt = Space.World.GetType();
        Debugger.Log(tt + ":" + tt.BaseType);
        int xx = (int)Space.World;        

        if (xx == 0)
        {
            Debugger.Log("xxxxxxxxxxxxxxxxx");
        }    */    

        //func = state.GetFunction("LuaFunc");
        //LuaFunction func2 = state.GetFunction("test.LuaFunc");

        //if (func != func2)
        //{
        //    Debugger.LogWarning("Compare luafunction failure");
        //}

        //func.Dispose();
        
        //object[] objs = func2.Call(123456);
        //Debugger.Log("Lua function return: {0}", objs[0]);
        //func2.Dispose();
        
        //LuaTable table = state.GetTable("map");
        //objs = table.ToArray();
        
        //Debugger.Log("map contains {0} {1} {2} {3}", objs[0], objs[1], objs[2], objs[3]);        
        //table[1] = 123;        
        //Debugger.Log("map 1 is {0}", table[1]);        
        //table.Call(1, 2, 3);        
        //table.Call(Vector3.one);

        //LuaArrayTable array = table.ToArrayTable();
        //IEnumerator<object> iter = array.GetEnumerator();

        //while (iter.MoveNext())
        //{
        //    Debugger.Log("map item: {0}", iter.Current);
        //}

        //iter.Dispose();        
        //LuaDictTable dict = table.ToDictTable();
        //var iter2 = dict.GetEnumerator();

        //while (iter2.MoveNext())
        //{
        //    Debugger.Log("map item, k,v is {0}:{1}", iter2.Current.Key, iter2.Current.Value);
        //}

        //iter2.Dispose();        
                 
        //state.CheckTop();
        //state.Close();
        //state.Dispose();

        ////Dictionary<int, int> dict1 = new Dictionary<int, int>();

        ////dict1.Add(123, 456);
        //////dict1.Remove(123);
        ////dict1.Add(123, 789);

        ////Debugger.Log("dict1 123 {0}", dict1[123]);

        //ValueType vt = new Vector3(1,2,3);
        //transform.position = (Vector3)vt;
       

        state.CheckTop();
    }

    public class TestIndex { }

    void TestException()
    {
        state.DoString(
            @"
                testindex = nil
                local t = os.clock()
                local Physics = UnityEngine.Physics
    
                for i=1, 200000 do
                    --TestIndex.GetPosition()
                    local xx = Physics.IgnoreRaycastLayer
                end

                print('lua cost time: ', os.clock() - t)     
                print('Physics.IgnoreRaycastLayer is', Physics.IgnoreRaycastLayer)                

                function TestMemCall()
                    local t = os.clock()

                    for i=1, 200000 do
                        testindex:GetPos()
                    end

                    print('lua2 cost time: ', os.clock() - t)   
                end
            ");

        state["testindex"] = new TestIndex();

        func = state.GetFunction("TestMemCall");
        func.Call();
    }

    bool init = false;
    static LuaFunction func = null;

    void Test2()
    {
        if (!init)
        {
            state.DoString(
@"
                    Transform = UnityEngine.Transform                    
                    transform = nil
                    count = 0

                    function TestPosition() 
                        count = count + 1
                        local node = transform                                                        
                        local t = os.clock()

	                    for i = 1,200000 do
		                    local v = node.position   
                            v.x = count                                                      
		                    node.position = v
	                    end
	                    
	                    t = os.clock() - t	
	                    print('lua cost time: ', t)                                
                    end");

            state["transform"] = transform;
            init = true;

            func = state.GetFunction("TestPosition");
        }

        Profiler.BeginSample("TestLua");

        if (func != null)
        {
            func.Call();
        }

        Profiler.EndSample();
    }

    void Update()
    {
        //if (func != null)
        //{
        //    func.BeginPCall();
        //    func.PCall();
        //    LuaFunction f = func.CheckLuaFunction();
        //    func.EndPCall();
        //}

        if (state != null)
        {
            state.Collect();
            state.CheckTop();
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 120, 50), "Test"))
        {
            //TestException();            
            //TestGC(state);
            Test2();

            //func.Call();

            /*float t1 = Time.realtimeSinceStartup;

            for (int i = 0; i < 200000; i++)
            {
                GetUnityObject(IntPtr.Zero, 1, "Transform");
            }

            Debugger.Log("Createtable in c# Cost time {0}", Time.realtimeSinceStartup - t1);

            t1 = Time.realtimeSinceStartup;
            Type t = typeof(Transform);

            for (int i = 0; i < 200000; i++)
            {
                GetUnityObject(IntPtr.Zero, 1, t);
            }

            Debugger.Log("Createtable in c# Cost time {0}", Time.realtimeSinceStartup - t1);*/
        }
    }

    void OnApplicationQuit()
    {        
        state.Dispose();
        Debugger.Log("Application quit");
    }
}
