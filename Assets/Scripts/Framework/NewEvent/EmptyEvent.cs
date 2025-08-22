//所有不带参数，直接触发方法的类标记于此处（推荐事件用法）


/// <summary>
/// 无参数事件基类
/// 用于触发不需要传递数据的方法（如刷新页面、播放音效等）
/// </summary>
public class EmptyEvent : IEvent
{
    // 无需定义任何字段，仅作为事件标识
}

/// <summary>
/// 具体无参数事件示例：刷新页面事件
/// 假设我们要刷新某个页面，必然是不带参数的
/// </summary>
public class RefreshPageEvent : EmptyEvent
{
    // 继承自EmptyEvent，无需额外参数
}

public class EmptyEventOne : EmptyEvent { }
public class EmptyEventTwo : EmptyEvent { }
public class EmptyEventThree : EmptyEvent { }
public class EmptyEventFour : EmptyEvent { }