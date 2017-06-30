## 例子1
展示了最小的tolua#环境，以及执行一段lua代码操作代码如下：
``` csharp
        LuaState lua = new LuaState();
        lua.Start();
        string hello =
            @"                
                print('hello tolua#')                                  
            ";
        
        lua.DoString(hello, "HelloWorld.cs");
        lua.CheckTop();
        lua.Dispose();
        lua = null;
``` 
LuaState封装了对lua 主要数据结构 lua_State 指针的各种操作。<br>
一般对于客户端，推荐只创建一个LuaState对象。如果要使用多State需要在Unity中设置全局宏 MULTI_STATE<br>
* LuaState.Start 需要在tolua代码加载到内存后调用。如果使用assetbunblde加载lua文件，Start()之前assetbundle必须准备好<br>
* LuaState.DoString 执行一段lua代码,除了例子比较少用这种方式加载代码,无法避免代码重复加载覆盖等,需调用者自己保证。第二个参数用于调试信息,或者error消息(用于提示出错代码所在文件名称)<br>
* LuaState.CheckTop 检查是否堆栈是否平衡，一般放于update中，c#中任何使用lua堆栈操作，都需要调用者自己平衡堆栈（参考LuaFunction以及LuaTable代码）, 当CheckTop时其实早已经离开了堆栈操作范围，如果出现警告需自行review代码。<br>
* LuaState.Dispose 释放LuaState 以及其资源。<br>

> **注意:** 此例子无法发布到手机

## 例子2
展示了dofile跟require的区别, 代码如下:
``` csharp
    LuaState lua = null;

    void Start () 
    {      
        lua = new LuaState();                
        lua.Start();        
        //如果移动了ToLua目录，自己手动修复吧，只是例子就不做配置了
        string fullPath = Application.dataPath + "/ToLua/Examples/02_ScriptsFromFile";
        lua.AddSearchPath(fullPath);        
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 120, 45), "DoFile"))
        {
            lua.DoFile("ScriptsFromFile.lua");                        
        }
        else if (GUI.Button(new Rect(50, 150, 120, 45), "Require"))
        {            
            lua.Require("ScriptsFromFile");            
        }

        lua.Collect();
        lua.CheckTop();
    }

    void OnApplicationQuit()
    {
        lua.Dispose();
        lua = null;
    }
``` 
tolua#修正了DoFile函数, 跟lua保持一致行为,能多次执行一个文件。tolua#加入了新的Require函数,无论c#和lua谁先require一个lua文件, 都能保证加载唯一性<br>
* LuaState.AddSearchPath 增加搜索目录, 这样DoFile跟Require函数可以只用文件名,无需写全路径<br>
* LuaState.DoFile 加载一个lua文件, 注意dofile需要扩展名, 可反复执行, 后面的变量会覆盖之前的DoFile加载的变量<br>
* LuaState.Require 同lua require(modname)操作, 加载指定模块并且把结果写入到package.loaded中,如果modname存在, 则直接返回package.loaded[modname]<br>
* LuaState.Collect 垃圾回收, 对于被自动gc的LuaFunction, LuaTable, 以及委托减掉的LuaFunction, 延迟删除的Object之类。等等需要延迟处理的回收, 都在这里自动执行<br>

> **注意:** 虽然有文件加载,但此例子无法发布到手机, 如果ToLua不在/Assets目录下, 需要修改代码中的目录位置<br>

## 例子3
展示了如何调用lua的函数, 主要代码如下:
``` csharp
    private string script =
        @"  function luaFunc(num)                        
                return num + 1
            end

            test = {}
            test.luaFunc = luaFunc
        ";

    LuaFunction func = null;
    LuaState lua = null;
	
    void Start () 
    {
        lua = new LuaState();
        lua.Start();
        lua.DoString(script);

        //Get the function object
        func = lua.GetFunction("test.luaFunc");

        if (func != null)
        {
            //有gc alloc
            object[] r = func.Call(123456);
            Debugger.Log("generic call return: {0}", r[0]);

            // no gc alloc
            int num = CallFunc();
            Debugger.Log("expansion call return: {0}", num);
        }
                
        lua.CheckTop();
	}

    void OnDestroy()
    {
        if (func != null)
        {
            func.Dispose();
            func = null;
        }

        lua.Dispose();
        lua = null;
    }

    int CallFunc()
    {        
        func.BeginPCall();                
        func.Push(123456);
        func.PCall();        
        int num = (int)func.CheckNumber();                    
        func.EndPCall();
        return num;                
    }
``` 
tolua# 简化了lua函数的操作,通过LuaFunction封装(并缓存)一个lua函数各种操作步骤, 建议频繁调用函数使用无GC方式。
* LuaState.GetLuaFunction 获取并缓存一个lua函数, 此函数支持串式操作, 如"test.luaFunc"代表test表中的luaFunc函数。
* LuaFunction.Call(...) 一个通用的简易函数调用操作, 使用了可变参数列表, 返回值也用了object数组, 因此存在gc
* LuaFunction.BeginPCall() 开始函数调用
* LuaFunction.Push() 压入函数调用需要的参数，通过众多的重载函数来解决参数转换gc问题
* LuaFunction.PCall() 调用lua函数
* LuaFunction.CheckNumber() 提取返回值, 并检查返回值为lua number类型
* LuaFunction.EndPCall() 结束lua函数调用, 清楚函数调用造成的堆栈变化
* LuaFunction.Dispose() 释放LuaFunction, 递减引用计数，如果引用计数为0, 则从_R表删除该函数

> **注意:** 此例子无法发布到手机, 无论Call还是PCall只相当于lua中的函数'.'调用。<br>
请注意':'这种语法糖 self:call(...) == self.call(self, ...） <br>
c# 中需要按后面方式调用, 即必须主动传入第一个参数self<br>
如果GetLuaFuntion不使用，直接Dispose会有出错警告，可以无视。权衡利弊，相对这种用法，防止出错更重要一些
