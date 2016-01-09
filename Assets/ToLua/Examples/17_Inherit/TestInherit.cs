using UnityEngine;
using System.Collections;
using LuaInterface;

public class TestInherit : MonoBehaviour 
{
    private string script =
    @"  LuaTransform = 
        {             
             posCount = -1,
            _position = Vector3.zero,
        }        
        
        local get = tolua.initget(LuaTransform)
        local set = tolua.initset(LuaTransform)               
                
        --重写同名函数
        function LuaTransform:Translate(...)            
	        print('child Translate')
	        self.base:Translate(...)                   
        end         

        function LuaTransform:Init(u)
            self._position = u.position                    
        end

        LuaTransform.__index = LuaTransform            

        function LuaTransform.Extend(u)         
            local t = {}            
            setmetatable(t, LuaTransform)   
            t:Init(u)                     
            tolua.setpeer(u, t)                                
            return u
        end
        
        --重写同名属性获取        
        get.position = function(self)                              
            return self._position
        end

        --重写同名属性设置
        set.position = function(self, v)                 
	        if self._position ~= v then                                  
		        self._position = v                
                self.base.position = v                                             
	        end
        end
        
        --既保证支持继承函数，又支持go.transform == transform 这样的比较
        function Test(node)        
            local v = Vector3.one           
            local transform = LuaTransform.Extend(node)                                                    

            local t = os.clock()            
            for i=1, 200000 do
                transform.position = transform.position
                --local v = transform.position:Clone()                
                --v.x = v.x - 1                
                --transform.position = v
            end
            print('LuaTransform get set cost', os.clock() - t)

            transform:Translate(1,1,1)                                                                     
                        
            local child = transform:FindChild('child')
            print('child is: ', tostring(child))
            
            if child.parent == transform then            
                print('LuaTransform compare to userdata transform is ok')
            end

            transform.xyz = 123
            transform.xyz = 456
            print('extern field xyz is: '.. transform.xyz)
        end
        ";

    LuaState lua = null;

	void Start () 
    {
        lua = new LuaState();        
        lua.Start();
        LuaBinder.Bind(lua);
        lua.DoString(script);

        float time = Time.realtimeSinceStartup;

        for (int i = 0; i < 200000; i++)
        {
            Vector3 v = transform.position;            
            transform.position = v;
        }

        time = Time.realtimeSinceStartup - time;
        Debugger.Log("c# Transform get set cost time: " + time);

        LuaFunction func = lua.GetFunction("Test");
        func.BeginPCall();
        func.Push(transform);
        func.PCall();
        func.EndPCall();

        lua.CheckTop();
        lua.Dispose();
        lua = null;
	}
}
