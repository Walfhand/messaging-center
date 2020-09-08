using MessaginCenterDemo.Interface;

namespace MessaginCenterDemo
{
    public class Player
    {
        public Player(IMessagingCenter messagingCenter)
        {
            messagingCenter.Send(this, "SANS_ARGS");
            messagingCenter.Send(this, "AVEC_ARGS", "Un message de la part d'un sender");
        }
    }
}
