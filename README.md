#tolua*#*

tolua# is an unity lua static binder solution. 

tolua# is an unity plugin that greatly simplifies the integration of C# code with Lua. can automatically generates the binding code to access unity from Lua. it maps c# constants,  variables, functions, properties, classes, and methods to Lua.

tolua# grows up from cstolua.  tolua#'s goal is to be a powerful development environment for unity.

support unity4.6.x and unity5.x all(in unity5.x, delete Plugins/x86 or Plugins/x86_64)

 **if you want to test in mobile, first click menu Lua/Copy lua files to Resources. then build it**

 **如果你想在手机上测试，首先点击菜单Lua/Copy lua files to Resources， 之后再build.**

 **有bug 可以到QQ群反馈: 286510803. 不闲聊，非bug相关不要加群**

#Library

**Debugger** <br>
https://github.com/topameng/Debugger

**tolua_runtime** <br>
https://github.com/topameng/tolua_runtime

**protoc-gen-lua** <br>
https://github.com/topameng/protoc-gen-lua

**luapb**<br>
https://github.com/Neopallium/lua-pb<br>
支持luapb可自行整合.未放入插件内
#FrameWork and Demo
**LuaFrameWork**<br>
https://github.com/jarjin/LuaFramework_NGUI <br>
https://github.com/jarjin/LuaFramework_UGUI <br>
**XlsxToLua**<br>
https://github.com/zhangqi-ulua/XlsxToLua<br>
**UnityHello**<br>
https://github.com/woshihuo12/UnityHello<br>

#Packages
　**Basics**　　　　　　　　**Math**　　　　　　**Data Structures**<br>
　luabitop　　　　　　　Quaternion　　　　　　　list<br>
　 struct　　　　　　　 　Vector3　　　　　　　　event<br>
　 int64　　　　 　　　  　Vector4　　　　　　　　slot<br>
　 Time　　　　 　　　  　Vector2<br>
**Networking**　　　　 　　　Ray<br>
　luasocket　　　　 　　　 Color<br>
　**Parsing**　　　　 　　　Bounds<br>
　lpeg　　 　　 　　　 　  　Mathf<br>
　**Protol**　　　　　 　 　　 Touch<br>
　pblua　　　 　　 　 　RaycastHit<br>
#关于反射
tolua# 不支持动态反射，因为il2cpp之后，很多未用到的属性并不一定会发布到运行包中(貌似已修正)，但还有参数匹配问题，重载函数排序问题，ref ,out 参数问题等等，这样动态反射很鸡肋，会有各种问题。tolua#提供的替换方法是 preloading, 把你未来可能需要的类型添加到导出列表customTypeList，同时也添加到dynamicList列表中，这样导出后该类型并不会随binder注册到lua中，你可以通过 require "namespace.classname" 动态注册到lua中，对于非枚举类型tolua#系统也可以在第一次push该类型时动态载入，当然也可在过场动画、资源下载、登录、场景加载或者某个的函数中require这个类型。
　
#Performance

#Examples
参考包内1-20例子

#About Lua
win, android using luajit2.0.4. macos using luac(for u5.x). ios using luajit2.1beta
