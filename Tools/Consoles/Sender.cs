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
