using messaging_center.Impl;
using messaging_center.Interfaces;

namespace messaging_center
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Attention : Le messaging center doit être disponible tout au long de la durée de vie de l'appli / jeux
            // Idéalement utiliser l'injection de dépendance
            // Si non vous pouvez l'utiliser en static ou en faire un singleton vous même mais je déconseille. 
            IMessagingCenter messagingCenter = new MessagingCenter();

            //Je crée un subscriber et je vais me subscribe dans le constructeur pour la démo
            Subscriber testSubsciber = new Subscriber(messagingCenter);

            //Je crée un sender et je vais lancer dans le constructeur pour la démo
            Sender testSender = new Sender(messagingCenter);
        }
    }
}
