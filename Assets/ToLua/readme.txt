tolua# 1.01
- FIX: 5.x AssetBundle.Load函数废弃问题.
- FIX: 修正模版类导出命名空间问题
- FIX: pblua protobuf协议tostring卡死问题
- FIX: Array index 不再检测null参数
- FIX: LuaInteger64 重载函数匹配检测问题
- NEW: 指定RenderSettings为静态类
- NEW: LuaFunction转委托函数支持可变参数列表
- NEW: Wrap函数出错同时附加c#异常堆栈
tolua# 1.02
- New: c# event +=和-=操作支持
- New: 添加 mac 和 ios 运行库
- Opt: 优化list双向链表