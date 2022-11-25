using messaging_center.Impl;
using messaging_center.Interfaces;

namespace messaging_center
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IMessagingCenter messagingCenter = new MessagingCenter(null);

            Subscriber testSubsciber = new Subscriber(messagingCenter);

            Sender testSender = new Sender(messagingCenter);
        }
    }
}
