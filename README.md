## Presentation
This event management package is designed to help you send and receive events in your C# applications. It allows you to easily create, publish, subscribe and consume events in your applications.

## Features
- Subscribe to an event by specifying the type to which we are attached, we can also specify a message to differentiate the triggers (several subscribers can subscribe to the same type of event without necessarily being triggered if the message is not the same), we can also provide arguments that we will receive in a callback
- Trigger events simply by giving in parameter an instance of type A that is listened to by one or more subscribers and by providing a message and arguments

## Install
```
dotnet add package messaging-center --version 1.0.2
```
## Usage
You can either configure dependency injection using this
```c#
services.AddMessagingCenter();
```
All you have to do is inject the IMessagingCenter interface into your constructors

If you don't want to use dependency injection you can use the MessagingCenter class directly

### Subscribe to an event
```c#
using messaging_center.Interfaces;
using System;
using System.Threading.Tasks;

namespace messaging_center
{
    internal class Subscriber
    {
        public Subscriber(IMessagingCenter messagingCenter)
        {
            messagingCenter.Subscribe<Sender>(this, "WITHOUT_ARGS", (sender) =>
            {
                Console.WriteLine("Callback Without args");
            });

            messagingCenter.Subscribe<Sender, string>(this, "WITH_ARGS", (sender, args) =>
            {
                Console.WriteLine(args);
            });

            messagingCenter.Subscribe<Sender, string>(this, "WITH_ARGS", async (sender, args) =>
            {
                //Async callback
                await Task.Delay(1000);
                Console.WriteLine(args);
            });
        }
    }
}
```

### Send event
```c#
using messaging_center.Interfaces;

namespace messaging_center
{
    internal class Sender
    {
        public Sender(IMessagingCenter messagingCenter)
        {
            messagingCenter.Send(this, "WITH_ARGS");
            messagingCenter.Send(this, "WITHOUT_ARGS", "An argument from a sender");
        }
    }
}

```
