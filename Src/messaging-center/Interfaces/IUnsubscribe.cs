namespace messaging_center.Interfaces;
public interface IUnsubscribe
{
    void Unsubscribe<TSender>(object subscriber, string message);

    void Unsubscribe<TSender, TArgs>(object subscriber, string message);
}
