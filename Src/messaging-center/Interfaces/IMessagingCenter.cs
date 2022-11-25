namespace messaging_center.Interfaces
{
    public interface IMessagingCenter
    {
        void Subscribe<TSender, TArgs>(object subscriber, string message, Action<TSender, TArgs> callback)
            where TSender : class
            where TArgs : class;
        void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback)
            where TSender : class;

        void Subscribe<TSender, TArgs>(object subscriber, string message, Func<TSender, TArgs, Task> callback) where TSender : class;

        void Send<TSender, TArgs>(TSender sender, string message, TArgs args)
            where TSender : class
            where TArgs: class;

        void Send<TSender>(TSender sender, string message)
            where TSender : class;
    }
}
