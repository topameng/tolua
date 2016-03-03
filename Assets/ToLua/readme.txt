tolua#
git地址: https://github.com/topameng/tolua

如果你想在手机上测试，首先点击菜单Lua/Copy lua files to Resources， 之后再build
如果在mac上发布，可以删除x86 和 x86_64 目录，或者自己配置plugins系统

1.01
- FIX: 5.x AssetBundle.Load函数废弃问题.
- FIX: 修正模版类导出命名空间问题
- FIX: pblua protobuf协议tostring卡死问题
- FIX: Array index 不再检测null参数
- FIX: LuaInteger64 重载函数匹配检测问题
- NEW: 指定RenderSettings为静态类
- NEW: LuaFunction转委托函数支持可变参数列表
- NEW: Wrap函数出错同时附加c#异常堆栈

1.02
- New: c# event +=和-=操作支持
- New: 添加 mac 和 ios 运行库
- Opt: 优化list双向链表

1.0.3(需要重新导出Wrap文件)
- FIX: 在mac unity5 luac协同异常后，unity追踪堆栈(一般是log类函数)不崩溃。(luac与unity配合问题)
- FIX: ios发布mono版本编译问题
- FIX: 模拟unity协同在使用过程中发生协同被gc的bug. 加入StartCoroutine 和 StopCoroutine 来启动或者停止这种协同
- FIX: LuaFunction递归调用自身问题
- New: 出错后能反映两端正确的堆栈（并且格式与unity相同）。无论是c#异常还是lua异常！
- New: 从LuaClient拆分出LuaLooper（负责update驱动分发）
- New: Lua API 接口按照lua头文件方式排序，加入所有的Lua API函数
- New: 重写大量可发生异常的Lua API转换为C#异常。
- New: lua 全双工协同加入 coroutine.stop 函数，请跟 coroutine.start 配合使用
- New: Event 改为小写 event, 增加 c# 端委托 +- LuaFunction
- New: Add utf-8 libs and examples
- New: Add cjson libs and examples
- New: CustomSettings.cs 加入新的静态类，以及out类链表(默认不在为每个类加.out属性, 除非out列表有这个类型）
- New: 加入LuaConst， 可以自定义Lua文件目录，设置后让例子环境正常运行

1.0.4 (需要重新导出Wrap文件)
- FIX: 修复遗漏的TrackedReference问题