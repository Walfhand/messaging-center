namespace messaging_center.Interfaces;
public interface ISubscribe
{
    void Subscribe<TSender, TArgs>(object subscriber, string message, Action<TSender, TArgs> callback)
        where TSender : class
        where TArgs : class;
    void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback)
        where TSender : class;

    void Subscribe<TSender, TArgs>(object subscriber, string message, Func<TSender, TArgs, Task> callback) where TSender : class;

    void Subscribe<TSender>(object subscriber, string message, Func<TSender, Task> callback) where TSender : class;
}
