using messaging_center.Interfaces;
using messaging_center.Models;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace messaging_center.Impl
{
    public class MessagingCenter : IMessagingCenter
    {
        private readonly Dictionary<SubscriptionKey, List<Subscription>> _subscriptions;
        private readonly ILogger _logger;

        public MessagingCenter(ILogger logger) 
        {
            _subscriptions = new Dictionary<SubscriptionKey, List<Subscription>>();
            _logger = logger;
        }
        public void Send<TSender, TArgs>(TSender sender, string message, TArgs args) 
            where TSender : class
            where TArgs : class 
            => InnerSend(message, sender, args).Wait();

        public void Send<TSender>(TSender sender, string message) where TSender : class
            => InnerSend(message, sender).Wait();

        private async Task InnerSend<TSender>(string? message, TSender? sender, object? args = null)
        {
            if (sender is null)
                throw new ArgumentNullException(nameof(sender));

            var subscriptionKey = new SubscriptionKey(sender.GetType().Name, message, args?.GetType()?.Name);

            if (_subscriptions.TryGetValue(subscriptionKey, out List<Subscription>? subs))
                foreach (var sub in subs)
                {
                    _logger.LogDebug("Event send by {0} for the target {1}", sender.GetType().Name, sub.Subscriber.GetType().Name);
                    await sub.InvokeCallBack(sender, args);
                }
        }

        public void Subscribe<TSender, TArgs>(object subscriber, string message, Action<TSender, TArgs> callback)
            where TSender : class
            where TArgs : class
            => InnerSubscribe<TSender>(subscriber, message, callback.Target, callback.GetMethodInfo(), typeof(TArgs));

        public void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback) where TSender : class
            => InnerSubscribe<TSender>(subscriber, message, callback.Target, callback.GetMethodInfo());

        public void Subscribe<TSender, TArgs>(object subscriber, string message, Func<TSender, TArgs, Task> callback) where TSender : class
            => InnerSubscribe<TSender>(subscriber, message, callback.Target, callback.GetMethodInfo(), typeof(TArgs));

        public void Subscribe<TSender>(object subscriber, string message, Func<TSender, Task> callback) where TSender : class
            => InnerSubscribe<TSender>(subscriber, message, callback.Target, callback.GetMethodInfo());


        private void InnerSubscribe<TSender>(object? subscriber, string message, object? target,
            MethodInfo methodInfo, Type? argType = null)
        {
            if(subscriber is null)
                throw new ArgumentNullException(nameof(subscriber));
            if(target is null)
                throw new ArgumentNullException(nameof(target));

            _logger.LogDebug("New subscriber {0} for the target {1}", subscriber.GetType().Name, typeof(TSender).Name);

            var subscriptionKey = new SubscriptionKey(typeof(TSender).Name, message, argType?.Name);

            var subscriptionValue = new Subscription(subscriber, target, methodInfo);

            if (_subscriptions.TryGetValue(subscriptionKey, out List<Subscription>? value))
                value?.Add(subscriptionValue);
            else
                _subscriptions?.Add(subscriptionKey, new List<Subscription>() { subscriptionValue });
        }


        public void Unsubscribe<TSender>(object subscriber, string message)
            => InnerUnsubscribe<TSender>(subscriber, message);

        public void Unsubscribe<TSender, TArgs>(object subscriber, string message)
            => InnerUnsubscribe<TSender>(subscriber, message, typeof(TArgs));

        private void InnerUnsubscribe<TSender>(object? subscriber, string message, Type? argType = null)
        {
            if (subscriber is null)
                throw new ArgumentNullException(nameof(subscriber));

            var subscriptionKey = new SubscriptionKey(typeof(TSender).Name, message, argType?.Name);

            if (_subscriptions.TryGetValue(subscriptionKey, out List<Subscription>? value))
            {
                var subscriptionValue = value?.FirstOrDefault(x => x.Subscriber.Equals(subscriber));
                if (subscriptionValue != null)
                    value?.Remove(subscriptionValue);
            }
        }
    }
}
