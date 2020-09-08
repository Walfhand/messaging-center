using MessaginCenterDemo.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MessaginCenterDemo.Implementation
{

    //ATTENTION !!! Je n'ai pas implémenter le Unsubscribe --> je te laisse faire c'est pas compliqué du tout check le code tu comprendras ;)
    //Je n'ai pas non plus checké ce que je recois en param donc pas de check si c'est null etc donc fait le !
    public class MessagingCenter : IMessagingCenter
    {
        private struct SubscriptionKey
        {
            public string SubscriptionType { get; set; }
            public string Message { get; set; }
            public string ArgType { get; set; }
        }

        private class Subscription
        {
            public object Subscriber { get; }
            public object Target { get; }
            public object Args { get; }
            public MethodInfo MethodInfo { get; }

            public Subscription(object subscriber, object target, MethodInfo methodInfo)
            {
                Subscriber = subscriber;
                Target = target;
                MethodInfo = methodInfo;
            }

            public void InvokeCallBack(object sender, object args)
            {
                MethodInfo.Invoke(Target, MethodInfo.GetParameters().Length == 1 ? new[] { sender } : new[] { sender, args });
            }
        }

        private Dictionary<SubscriptionKey, List<Subscription>> subscriptions;

        public MessagingCenter()
        {
            subscriptions = new Dictionary<SubscriptionKey, List<Subscription>>();
        }
        public void Send<TSender, TArgs>(TSender sender, string message, TArgs args) where TSender : class
        {
            InnerSend(message, typeof(TSender), sender, args, typeof(TArgs));
        }

        public void Send<TSender>(TSender sender, string message) where TSender : class
        {
            InnerSend(message, typeof(TSender), sender);
        }

        public void Subscribe<TSender, TArgs>(object subscriber, string message,
            Action<TSender, TArgs> callback, TSender source = null) where TSender : class
        {
            InnerSubscribe(subscriber, message, callback.Target, callback.GetMethodInfo(), typeof(TSender), typeof(TArgs));
        }

        public void Subscribe<TSender>(object subscriber, string message, Action<TSender> callback, TSender source = null) where TSender : class
        {
            InnerSubscribe(subscriber, message, callback.Target, callback.GetMethodInfo(), typeof(TSender));
        }


        private void InnerSubscribe(object subscriber, string message, object target,
            MethodInfo methodInfo, Type senderType, Type argType = null)
        {
            var subscriptionKey = new SubscriptionKey()
            {
                Message = message,
                SubscriptionType = senderType.Name,
                ArgType = argType?.Name
            };

            var subscriptionValue = new Subscription(subscriber, target, methodInfo);

            if (subscriptions.ContainsKey(subscriptionKey))
            {
                subscriptions[subscriptionKey].Add(subscriptionValue);
            }
            else
            {
                subscriptions.Add(subscriptionKey, new List<Subscription>() { subscriptionValue });
            }
        }

        private void InnerSend(string message, Type subscribtionType, object sender, object args = null, Type argType = null)
        {
            var subscriptionKey = new SubscriptionKey()
            {
                Message = message,
                SubscriptionType = subscribtionType.Name,
                ArgType = argType?.Name
            };

            if (subscriptions.ContainsKey(subscriptionKey))
            {
                List<Subscription> subs = subscriptions[subscriptionKey];

                foreach (Subscription sub in subs)
                {
                    sub.InvokeCallBack(sender, args);
                }
            }

        }
    }
}
