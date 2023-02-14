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
