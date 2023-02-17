namespace messaging_center.Interfaces;
public interface ISend
{
    void Send<TSender, TArgs>(TSender sender, string message, TArgs args)
    where TSender : class
    where TArgs : class;

    void Send<TSender>(TSender sender, string message)
        where TSender : class;
}
