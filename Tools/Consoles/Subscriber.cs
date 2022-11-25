using messaging_center.Interfaces;
using System;
using System.Threading.Tasks;

namespace messaging_center
{
    internal class Subscriber
    {
        public Subscriber(IMessagingCenter messagingCenter)
        {
            messagingCenter.Subscribe<Sender>(this, "SANS_ARGS", (sender) =>
            {
                //Je m'abonne à test sender avec un message 'SANS_ARGS'
                Console.WriteLine("Callback sans args");
            });

            //A savoir : tu peux passer ce que tu veux en argument donc tu peux recevoir dans ton callback n'importe quel objet
            messagingCenter.Subscribe<Sender, string>(this, "AVEC_ARGS", (sender, args) =>
            {
                //Je m'abonne à test sender avec un message 'AVEC_ARGS'
                Console.WriteLine(args);
            });

            messagingCenter.Subscribe<Sender, string>(this, "AVEC_ARGS", async (sender, args) =>
            {
                await Task.Delay(1000);
                //Je m'abonne à test sender avec un message 'AVEC_ARGS'
                Console.WriteLine(args);
            });
        }
    }
}
