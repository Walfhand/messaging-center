using System.Reflection;

namespace messaging_center.Models;
internal class Subscription
{
    public object Subscriber { get; }
    public object Target { get; }
    public MethodInfo MethodInfo { get; }

    public Subscription(object subscriber, object target, MethodInfo methodInfo)
    {
        Subscriber = subscriber;
        Target = target;
        MethodInfo = methodInfo;
    }

    public async Task InvokeCallBack(object sender, object? args)
    {
        var result = MethodInfo.Invoke(Target, MethodInfo.GetParameters().Length == 1 ? new[] { sender } : new[] { sender, args });
        if (result is Task task)
            await task;
    }
}
