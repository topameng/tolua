## tolua#

tolua# is a Unity lua static binder solution. the first solution that analyzes code by reflection and generates wrapper classes.

It is a Unity plugin that greatly simplifies the integration of C# code with Lua. which can automatically generate the binding code to access Unity from Lua and map c# constants, variables, functions, properties, classes, and enums to Lua.

tolua# grows up from cstolua. it's goal is to be a powerful development environment for Unity.

Support unity4.6.x and Unity5.x all(copy /Unity5.x/Assets to /Assets)

If you want to test in mobile, first click menu Lua/Copy lua files to Resources. then build it

如果你想在手机上测试，首先点击菜单Lua/Copy lua files to Resources， 之后再build.

有bug 可以到QQ群反馈: 286510803. 不闲聊，非bug相关不要加群, 请加讨论群: <br>
ulua&tolua技术交流群① 341746602(已满) <br>
ulua_tolua技术讨论群② 469941220 <br>
ulua&tolua技术交流群3 434341400(已满) <br>
tolua#技术讨论群④ 543826216<br>

# Library
**tolua_runtime** <br>
https://github.com/topameng/tolua_runtime <br>
**Debugger** <br>
https://github.com/topameng/Debugger <br>
**CString** <br>
https://github.com/topameng/CString <br>
**protoc-gen-lua** <br>
https://github.com/topameng/protoc-gen-lua <br>

# FrameWork and Demo
**LuaFrameWork**<br>
https://github.com/jarjin/LuaFramework_NGUI <br>
https://github.com/jarjin/LuaFramework_UGUI <br>
**XlsxToLua**<br>
https://github.com/zhangqi-ulua/XlsxToLua<br>
**UnityHello**<br>
https://github.com/woshihuo12/UnityHello<br>
**UWA-ToLua**<br>
http://uwa-download.oss-cn-beijing.aliyuncs.com/plugins%2FiOS%2FUWA-iOS-ToLua.zip<br>

# Debugger
**EmmyLua**<br>
https://github.com/tangzx/IntelliJ-EmmyLua<br>
**unity_tolua-_zerobrane_api**<br>
https://github.com/LabOfHoward/unity_tolua-_zerobrane_api<br>

# Packages
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
# 特性
* 自动生成绑定代码文件，非反射调用 <br>
* 大量内建基础类型支持，如枚举，委托，事件，Type, 数组，迭代器等 <br>
* 支持多种协同形式 <br>
* 支持所有unity内部类导出，支持委托类型导出 <br>
* 支持导出自定义，跳过某个空的基类，修改导出名称等 <br>
* 支持扩展函数导出 <br>
* 支持Lua中function转委托，可以区分需要不同委托的参数的重载函数 <br>
* 支持c# LuaFunction对象转委托， 简化调用方式
* 支持重载函数自动折叠, 如： Convert.ToUInt32只导出double参数的函数 <br>
* 支持重载函数自动排序, object参数执行级最低, 不会出现错误匹配情况 <br>
* 支持导出函数重命名, 可以分离导出某个重载函数(可以导出被折叠掉的函数) <br>
* 支持使用编辑器类改写导出规则 <br>
* 支持this数组访问，索引为int可以通过[]访问，其他可使用obj.get_Item或者obj.this:get()访问数组成员 <br>
* 支持委托(事件)+-lua function。独有的-操作支持, 支持通过函数接口的Add和Remove委托操作 <br>
* 支持静态反射操作, 形式同c# <br>
* 支持peer表，可在lua端扩展导出的userdata <br>
* 支持自定义struct压入和读取，做到无GC，并且成员无类型限制
* 支持preloading, 可以通过requie后绑定wrap文件 <br>
* 支持int64, uint64  <br>
* 大量的lua数学类型，如Quaternion, Vector3, Mathf等
* 包含第三方lua扩展，包括luasocket, struct, lpeg, utf8, pb等库 <br>
* 当lua出现异常，能够同时捕获c#端和lua端堆栈，便于调试 <br>
* print信息，在编辑器点击日志, 能自动打开对应lua文件 <br>
* 支持unity所有版本 <br>

# 快速入门
在CustomSetting.cs中添加需要导出的类或者委托，类加入到customTypeList列表，委托加入到customDelegateList列表 <br>
通过设置saveDir变量更改导出目录,默认生成在Assets/Source/Generate/下,点击菜单Lua->Generate All,生成绑定文件 <br>
在LuaConst.cs中配置开发lua文件目录luaDir以及tolua lua文件目录toluaDir <br>
```csharp
LuaState lua = new LuaState();
lua.Start();
lua.DoString("print('hello world')");
lua.Dispose();
```
[这里](Assets/ToLua/Examples/README.md)是更多的示例。
# 关于反射
tolua# 不支持动态反射。动态反射对于重载函数有参数匹配问题，函数排序问题，ref,out 参数问题等等。<br>
tolua#提供的替换方法是:<br>
1. preloading, 把你未来可能需要的类型添加到导出列表customTypeList，同时也添加到dynamicList列表中，这样导出后该类型并不会随binder注册到lua中，你可以通过 require "namespace.classname" 动态注册到lua中，对于非枚举类型tolua#系统也可以在第一次push该类型时动态载入，当然也可在过场动画、资源下载、登录、场景加载或者某个的函数中require这个类型。<br>
2. 静态反射，参考例子22。通过静态反射支持精确的函数参数匹配和类型检查。不会存在重载函数参数混乱匹配错误问题
　
# Performance
|   平台    |   属性读写   | 重载函数  | Vector3构造 |GameObject构造|Vector3归一化|Slerp|
| :-- 		| :-----------:|:---------:| :---------: |:-----------: |:----------: |:--: |
| PC  		|  0.0465:0.15 | 0.076:0.12|0.02:0.001   |0.1:0.14		|0.014:0.001  |0.10:0.11|
| Android   |   0.26:1.03  | 0.39:1.15 |0.2:0.0049   |0.43:0.5		|0.27:0.02	  |0.49:0.16|
| iOS       |  0.04:0.145  | 0.055:0.146 |0.017:0.05   |0.074:0.08	|0.035:0.11	  |0.078:0.58|

测试结果为C#:Lua. 环境不同会略有差异。可用数字倍率做参考<br>
PC: Intel(R) Core(TM) i5-4590 CPU@3.3GHz + 8GB + 64 位win7<br>
Android: 中兴nubia z9 max(NX512J) + Adnroid5.0.2<br>
iOS(il2cpp): IPhone6 Plus<br>
# Examples
参考包内1-24例子

# About Lua
win, android ios using luajit2.1-beta3. macos using luac5.1.5(for u5.x). 
