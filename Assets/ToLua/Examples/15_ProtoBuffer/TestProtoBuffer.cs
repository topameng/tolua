using UnityEngine;
using System.Collections;
using LuaInterface;
using System;

public class TestProtoBuffer : MonoBehaviour
{
    private string script = @"      
        local person_pb = require ""person_pb""

        function Decoder()  
            local msg = person_pb.Person()
            msg:ParseFromString(TestProtol.data)
            print('person_pb decoder: '..tostring(msg))
        end

        function Encoder()                           
            local msg = person_pb.Person()
            msg.id = 1024
            msg.name = 'foo'
            msg.email = 'bar'
            local pb_data = msg:SerializeToString()
            TestProtol.data = pb_data
        end
        ";

    private string tips = "";

    //实际应用如Socket.Send(LuaStringBuffer buffer)函数发送协议, 在lua中调用Socket.Send(pb_data)
    //读取协议 Socket.PeekMsgPacket() {return MsgPacket}; lua 中，取协议字节流 MsgPack.data 为 LuaStringBuffer类型
    //msg = Socket.PeekMsgPacket() 
    //pb_data = msg.data    
    void Start()
    {
        Application.RegisterLogCallback(ShowTips);  
        new LuaResLoader();        
        LuaState state = new LuaState();
        state.Start();
        Bind(state);
        state.OpenLibs(LuaDLL.luaopen_pb);                   

        state.DoString(script);
        LuaFunction func = state.GetFunction("Encoder");
        func.Call();
        func.Dispose();
        func = null;

        func = state.GetFunction("Decoder");
        func.Call();
        func.Dispose();
        func = null;

        state.CheckTop();
        state.Dispose();
        state = null;
    }

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        tips = tips + msg + "\r\n";
    }

    void Bind(LuaState state)
    {
        state.BeginModule(null);
        TestProtolWrap.Register(state);
        state.EndModule();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2, 300, 200), tips);
    }
}
