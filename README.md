#tolua*#*

tolua# is an unity lua static binder solution. 

tolua# is an unity plugin that greatly simplifies the integration of C# code with Lua. can automatically generates the binding code to access unity from Lua. it maps c# constants,  variables, functions, properties, classes, and methods to Lua.

tolua# grows up from cstolua.  tolua#'s goal is to be a powerful development environment for unity.

support unity4.6.x and unity5.x all(in unity5.x, delete Plugins/x86 directory)

 **if you want to test in mobile, first click menu Lua/Copy lua files to Resources. then build it**

 **unity5.x 删除Plugins/x86目录, 如果你想在手机上测试，首先点击菜单Lua/Copy lua files to Resources， 之后再build.**

 **有bug 可以到QQ群反馈: 286510803. 不闲聊，非bug相关不要加群**

#Library

**Debugger** <br>
https://github.com/topameng/Debugger

**tolua_runtime** <br>
https://github.com/topameng/tolua_runtime

**protoc-gen-lua** <br>
https://github.com/topameng/protoc-gen-lua

**LuaSocket** <br>
https://github.com/diegonehab/luasocket

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
tolua# 不支持动态反射，对于重载函数有参数匹配问题，函数排序问题，ref ,out 参数问题等等，这样动态反射会有各种问题。<br>
tolua#提供的替换方法是:<br>
1. preloading, 把你未来可能需要的类型添加到导出列表customTypeList，同时也添加到dynamicList列表中，这样导出后该类型并不会随binder注册到lua中，你可以通过 require "namespace.classname" 动态注册到lua中，对于非枚举类型tolua#系统也可以在第一次push该类型时动态载入，当然也可在过场动画、资源下载、登录、场景加载或者某个的函数中require这个类型。<br>
2. 1.0.5 版本开始加入静态反射，参考例子22。通过静态反射支持精确的函数参数匹配和类型检查。不会存在重载函数参数混乱匹配错误问题
　
#Performance
|   平台    	|   属性读写   | 函数调用  | Vector3构造 |GameObject构造|Vector3归一化|Slerp|
| :-- 		| :-----------:|:---------:| :---------: |:---------: |:---------: |:---------: |
| PC  		|  0.0465:0.15 | 0.076:0.19|0.02:0.001   |0.1:0.14|0.014:0.001|0.10:0.11|
| Android   |   0.26:1.03  | 0.39:1.15 |0.2:0.0049   |0.43:0.5|0.27:0.02|0.49:0.16|
| iOS       |   待测       | 待测      |   待测      |待测|待测|待测|

测试结果为C#:Lua. 环境不同会略有差异。可用数字倍率做参考, 加了peer表略降了些<br>
PC: Intel(R) Core(TM) i5-4590 CPU@3.3GHz + 8GB + 64 位win7<br>
Android: 中兴nubia z9 max(NX512J) + Adnroid5.0.2<br>
#Examples
参考包内1-20例子

#About Lua
win, android using luajit2.0.4. macos using luac(for u5.x). ios using luajit2.1-beta2