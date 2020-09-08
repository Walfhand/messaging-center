using MessaginCenterDemo.Implementation;
using MessaginCenterDemo.Interface;

namespace MessaginCenterDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Attention : Le messaging center doit être disponible tout au long de la durée de vie de l'appli / jeux
            // Idéalement utiliser l'injection de dépendance
            // Si non vous pouvez l'utiliser en static ou en faire un singleton vous même mais je déconseille. 
            IMessagingCenter messagingCenter = new MessagingCenter();

            //Je crée un subscriber et je vais me subscribe dans le constructeur pour la démo
            GameManager testSubsciber = new GameManager(messagingCenter);

            //Je crée un sender et je vais lancer dans le constructeur pour la démo
            Player testSender = new Player(messagingCenter);
        }
    }
}
