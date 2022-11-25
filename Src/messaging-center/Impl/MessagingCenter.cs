using messaging_center.Interfaces;
using System.Reflection;

namespace messaging_center.Impl
{
    public class MessagingCenter : IMessagingCenter
    {
        private record SubscriptionKey(string SubscriptionType, string? Message, string? ArgType)
        {
        }

        private class Subscription
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

        private readonly Dictionary<SubscriptionKey, List<Subscription>> _subscriptions;

        public MessagingCenter() => _subscriptions = new Dictionary<SubscriptionKey, List<Subscription>>();
        public void Send<TSender, TArgs>(TSender sender, string message, TArgs args) 
            where TSender : class
            where TArgs : class 
            => InnerSend(message, sender, args).Wait();

        public void Send<TSender>(TSender sender, string message) where TSender : class
            => InnerSend(message, sender).Wait();

        public void Subscribe<TSender, TArgs>(object subscriber, string message, Action<TSender, TArgs> callback) 
            where TSender : class
            where TArgs : class
            => InnerSubscribe<TSender>(subscriber, message, callback.Target, callback.GetMethodInfo(), typeof(TArgs));

        public void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback) where TSender : class
            => InnerSubscribe<TSender>(subscriber, message, callback.Target, callback.GetMethodInfo());

        public void Subscribe<TSender, TArgs>(object subscriber, string message, Func<TSender, TArgs, Task> callback) where TSender : class
            => InnerSubscribe<TSender>(subscriber, message, callback.Target, callback.GetMethodInfo(), typeof(TArgs));


        private void InnerSubscribe<TSender>(object? subscriber, string message, object? target,
            MethodInfo methodInfo, Type? argType = null)
        {
            if(subscriber is null)
                throw new ArgumentNullException(nameof(subscriber));
            if(target is null)
                throw new ArgumentNullException(nameof(target));

            var subscriptionKey = new SubscriptionKey(typeof(TSender).Name, message, argType?.Name);

            var subscriptionValue = new Subscription(subscriber, target, methodInfo);

            if (_subscriptions.TryGetValue(subscriptionKey, out List<Subscription>? value))
                value?.Add(subscriptionValue);
            else
                _subscriptions?.Add(subscriptionKey, new List<Subscription>() { subscriptionValue });
        }

        private async Task InnerSend<TSender>(string? message, TSender? sender, object? args = null)
        {
            if(sender is null)
                throw new ArgumentNullException(nameof(sender));

            var subscriptionKey = new SubscriptionKey(sender.GetType().Name, message, args?.GetType()?.Name);

            if (_subscriptions.TryGetValue(subscriptionKey, out List<Subscription>? subs))
                foreach (var sub in subs)
                    await sub.InvokeCallBack(sender, args);
        }
    }
}
