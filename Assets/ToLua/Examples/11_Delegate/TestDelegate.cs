using UnityEngine;
using System;
using System.Collections.Generic;
using LuaInterface;


public class TestDelegate: MonoBehaviour
{
    private string script =
    @"                              
            function DoClick1(go)                
                print('click1 gameObject is '..go.name)                    
            end

            function DoClick2(go)                
                print('click2 gameObject is '..go.name)                              
            end                                   

            function AddClick1(listener)       
                if listener.onClick then
                    listener.onClick = listener.onClick + DoClick1                           
                else
                    listener.onClick = DoClick1                                          
                end                                                       
            end

            function AddClick2(listener)
                if listener.onClick then
                    listener.onClick = listener.onClick + DoClick2                      
                else
                    listener.onClick = DoClick2
                end                
            end

            function SetClick1(listener)
                listener.onClick = DoClick1         
            end

            function RemoveClick1(listener)
                if listener.onClick then
                    listener.onClick = listener.onClick - DoClick1      
                else
                    print('empty delegate')
                end
            end

            function RemoveClick2(listener)
                if listener.onClick then
                    listener.onClick = listener.onClick - DoClick2    
                else
                    print('empty delegate')                                
                end
            end

            --测试重载问题
            function TestOverride(listener)
                listener:SetOnFinished(TestEventListener.OnClick(DoClick1))
                listener:SetOnFinished(TestEventListener.VoidDelegate(DoClick2))
            end

            function TestEvent()
                print('this is a event')
            end
            
            function AddEvent(listener)
                listener.onClickEvent = listener.onClickEvent + TestEvent
            end

            function RemoveEvent(listener)
                --事件减法需要c# gc来回收luafunction, 因为事件无法获取委托列表，不能强制回收
                listener.onClickEvent = listener.onClickEvent - TestEvent                       
            end

            local t = {name = 'byself'}

            function t:TestSelffunc(go)
                print('callback with self: '..self.name..' '..go.name)
            end       

            function AddSelfClick(listener)               
                if listener.onClick then
                    local onClick = TestEventListener.OnClick(t.TestSelffunc, t)  
                    listener.onClick = listener.onClick + onClick         
                    --删除临时变量
                    onClick:Destroy()                    
                else                    
                    listener.onClick = TestEventListener.OnClick(t.TestSelffunc, t)                    
                end                                   
            end     

            function RemoveSelfClick(listener)
                if listener.onClick then                    
                    local onClick = TestEventListener.OnClick(t.TestSelffunc, t)  
                    listener.onClick = listener.onClick - onClick                    
                    onClick:Destroy()                    
                    --也可以如下全部删除
                    --listener.onClick = nil  
                    --listener.onClick:Destroy()                    
                else
                    print('empty delegate')
                end   
            end
    ";

    LuaState state = null;
    TestEventListener listener = null;

    LuaFunction SetClick1 = null;
    LuaFunction AddClick1 = null;
    LuaFunction AddClick2 = null;
    LuaFunction RemoveClick1 = null;
    LuaFunction RemoveClick2 = null;
    LuaFunction TestOverride = null;
    LuaFunction RemoveEvent = null;
    LuaFunction AddEvent = null;
    LuaFunction AddSelfClick = null;
    LuaFunction RemoveSelfClick = null;
   
    //需要删除的转LuaFunction为委托，不需要删除的直接加或者等于即可
    void Awake()
    {
#if UNITY_4_6 || UNITY_4_7
        Application.RegisterLogCallback(ShowTips);
#else
        Application.logMessageReceived += ShowTips;
#endif
        new LuaResLoader();
        state = new LuaState();
        state.Start();
        LuaBinder.Bind(state);
        Bind(state);

        state.LogGC = true;
        state.DoString(script, "TestDelegate.cs");
        GameObject go = new GameObject("TestGo");
        listener = (TestEventListener)go.AddComponent(typeof(TestEventListener));

        SetClick1 = state.GetFunction("SetClick1");
        AddClick1 = state.GetFunction("AddClick1");
        AddClick2 = state.GetFunction("AddClick2");
        RemoveClick1 = state.GetFunction("RemoveClick1");
        RemoveClick2 = state.GetFunction("RemoveClick2");
        TestOverride = state.GetFunction("TestOverride");
        AddEvent = state.GetFunction("AddEvent");
        RemoveEvent = state.GetFunction("RemoveEvent");

        AddSelfClick = state.GetFunction("AddSelfClick");
        RemoveSelfClick = state.GetFunction("RemoveSelfClick");        
    }

    void Bind(LuaState L)
    {
        L.BeginModule(null);
        TestEventListenerWrap.Register(state);
        L.EndModule();

        DelegateFactory.dict.Add(typeof(TestEventListener.OnClick), TestEventListener_OnClick);
        DelegateFactory.dict.Add(typeof(TestEventListener.VoidDelegate), TestEventListener_VoidDelegate);

        DelegateTraits<TestEventListener.OnClick>.Init(TestEventListener_OnClick);
        DelegateTraits<TestEventListener.VoidDelegate>.Init(TestEventListener_VoidDelegate);

        TypeTraits<TestEventListener.OnClick>.Init(Check_TestEventListener_OnClick);
        TypeTraits<TestEventListener.VoidDelegate>.Init(Check_TestEventListener_VoidDelegate);

        StackTraits<TestEventListener.OnClick>.Push = Push_TestEventListener_OnClick;
        StackTraits<TestEventListener.VoidDelegate>.Push = Push_TestEventListener_VoidDelegate;
    }

    void CallLuaFunction(LuaFunction func)
    {
        tips = "";
        func.BeginPCall();
        func.Push(listener);
        func.PCall();
        func.EndPCall();                
    }

    //自动生成代码后拷贝过来
    class TestEventListener_OnClick_Event : LuaDelegate
    {
        public TestEventListener_OnClick_Event(LuaFunction func) : base(func) { }
        public TestEventListener_OnClick_Event(LuaFunction func, LuaTable self) : base(func, self) { }

        public void Call(UnityEngine.GameObject param0)
        {
            func.BeginPCall();
            func.PushSealed(param0);
            func.PCall();
            func.EndPCall();
        }

        public void CallWithSelf(UnityEngine.GameObject param0)
        {
            func.BeginPCall();
            func.Push(self);
            func.PushSealed(param0);
            func.PCall();
            func.EndPCall();
        }
    }

    public TestEventListener.OnClick TestEventListener_OnClick(LuaFunction func, LuaTable self, bool flag)
    {
        if (func == null)
        {
            TestEventListener.OnClick fn = delegate (UnityEngine.GameObject param0) { };
            return fn;
        }

        if (!flag)
        {
            TestEventListener_OnClick_Event target = new TestEventListener_OnClick_Event(func);
            TestEventListener.OnClick d = target.Call;
            target.method = d.Method;
            return d;
        }
        else
        {
            TestEventListener_OnClick_Event target = new TestEventListener_OnClick_Event(func, self);
            TestEventListener.OnClick d = target.CallWithSelf;
            target.method = d.Method;
            return d;
        }
    }

    bool Check_TestEventListener_OnClick(IntPtr L, int pos)
    {
        return TypeChecker.CheckDelegateType<TestEventListener.OnClick>(L, pos);
    }

    void Push_TestEventListener_OnClick(IntPtr L, TestEventListener.OnClick o)
    {
        ToLua.Push(L, o);
    }

    class TestEventListener_VoidDelegate_Event : LuaDelegate
    {
        public TestEventListener_VoidDelegate_Event(LuaFunction func) : base(func) { }
        public TestEventListener_VoidDelegate_Event(LuaFunction func, LuaTable self) : base(func, self) { }

        public void Call(UnityEngine.GameObject param0)
        {
            func.BeginPCall();
            func.PushSealed(param0);
            func.PCall();
            func.EndPCall();
        }

        public void CallWithSelf(UnityEngine.GameObject param0)
        {
            func.BeginPCall();
            func.Push(self);
            func.PushSealed(param0);
            func.PCall();
            func.EndPCall();
        }
    }

    public TestEventListener.VoidDelegate TestEventListener_VoidDelegate(LuaFunction func, LuaTable self, bool flag)
    {
        if (func == null)
        {
            TestEventListener.VoidDelegate fn = delegate (UnityEngine.GameObject param0) { };
            return fn;
        }

        if (!flag)
        {
            TestEventListener_VoidDelegate_Event target = new TestEventListener_VoidDelegate_Event(func);
            TestEventListener.VoidDelegate d = target.Call;
            target.method = d.Method;
            return d;
        }
        else
        {
            TestEventListener_VoidDelegate_Event target = new TestEventListener_VoidDelegate_Event(func, self);
            TestEventListener.VoidDelegate d = target.CallWithSelf;
            target.method = d.Method;
            return d;
        }
    }

    bool Check_TestEventListener_VoidDelegate(IntPtr L, int pos)
    {
        return TypeChecker.CheckDelegateType<TestEventListener.VoidDelegate>(L, pos);
    }

    void Push_TestEventListener_VoidDelegate(IntPtr L, TestEventListener.VoidDelegate o)
    {
        ToLua.Push(L, o);
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 300, Screen.height / 2 - 200, 600, 400), tips);

        if (GUI.Button(new Rect(10, 10, 120, 40), " = OnClick1"))
        {
            CallLuaFunction(SetClick1);
        }
        else if (GUI.Button(new Rect(10, 60, 120, 40), " + Click1"))
        {
            CallLuaFunction(AddClick1);
        }
        else if (GUI.Button(new Rect(10, 110, 120, 40), " + Click2"))
        {
            CallLuaFunction(AddClick2);
        }
        else if (GUI.Button(new Rect(10, 160, 120, 40), " - Click1"))
        {
            CallLuaFunction(RemoveClick1);
        }
        else if (GUI.Button(new Rect(10, 210, 120, 40), " - Click2"))
        {
            CallLuaFunction(RemoveClick2);
        }
        else if (GUI.Button(new Rect(10, 260, 120, 40), "+ Click1 in C#"))
        {
            tips = "";
            LuaFunction func = state.GetFunction("DoClick1"); //DoClick1 ref+1
            TestEventListener.OnClick onClick = (TestEventListener.OnClick)DelegateTraits<TestEventListener.OnClick>.Create(func);
            listener.onClick += onClick;
            func.Dispose();
            func = null;
        }        
        else if (GUI.Button(new Rect(10, 310, 120, 40), " - Click1 in C#"))
        {
            tips = "";
            if (listener.onClick == null) return;            
            LuaFunction func = state.GetFunction("DoClick1");//DoClick1 ref+2
            TestEventListener.OnClick onClick = (TestEventListener.OnClick)DelegateFactory.RemoveDelegate(listener.onClick, func);
                         
            if (!object.ReferenceEquals(listener.onClick, onClick))
            {
                if (listener.onClick != null) listener.onClick.SubRef();
                listener.onClick = onClick;
            }

            func.Dispose();
            func = null;
        }
        else if (GUI.Button(new Rect(10, 360, 120, 40), "+self call"))
        {
            CallLuaFunction(AddSelfClick);
        }
        else if (GUI.Button(new Rect(10, 410, 120, 40), "-self call"))
        {
            CallLuaFunction(RemoveSelfClick);
        }
        else if (GUI.Button(new Rect(10, 460, 120, 40), "OnClick"))
        {
            if (listener.onClick != null)
            {
                listener.onClick(gameObject);
            }
            else
            {
                Debug.Log("empty delegate!!");
            }
        }
        else if (GUI.Button(new Rect(10, 510, 120, 40), "Override"))
        {
            CallLuaFunction(TestOverride);
        }
        else if (GUI.Button(new Rect(200, 10, 120, 40), "event +"))
        {
            CallLuaFunction(AddEvent);
        }
        else if (GUI.Button(new Rect(200, 60, 120, 40), "event -"))
        {
            CallLuaFunction(RemoveEvent);
        }
        else if (GUI.Button(new Rect(200, 110, 120, 40), "event call"))
        {
            listener.OnClickEvent(gameObject);
        }
        else if (GUI.Button(new Rect(200, 160, 120, 40), "Force GC"))
        {
            //自动gc log: collect lua reference name , id xxx in thread 
            state.LuaGC(LuaGCOptions.LUA_GCCOLLECT, 0);
            GC.Collect();
        }
    }

    void Update()
    {
        state.Collect();
        state.CheckTop();        
    }

    void SafeRelease(ref LuaFunction luaRef)
    {
        if (luaRef != null)
        {
            luaRef.Dispose();
            luaRef = null;
        }
    }

    string tips = "";    

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        tips += msg;
        tips += "\r\n";
    }

    void OnApplicationQuit()
    {
        SafeRelease(ref AddClick1);
        SafeRelease(ref AddClick2);
        SafeRelease(ref RemoveClick1);
        SafeRelease(ref RemoveClick2);
        SafeRelease(ref SetClick1);
        SafeRelease(ref TestOverride);
        state.Dispose();
        state = null;
#if UNITY_4_6 || UNITY_4_7
        Application.RegisterLogCallback(null);

#else
        Application.logMessageReceived -= ShowTips;
#endif  
    }
}
