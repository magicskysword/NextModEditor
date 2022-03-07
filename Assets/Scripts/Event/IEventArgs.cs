public interface IEventArgs
{
    public object Sender { get; set; }
}

public class EventArgs : IEventArgs
{
    public object Sender { get; set; }
}