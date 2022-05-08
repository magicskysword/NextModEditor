using System;

public interface IEventSubject
{
    public void Register<TArgs>(Action<TArgs> callback) where TArgs : class, IEventArgs;
    public void Unregister<TArgs>(Action<TArgs> callback) where TArgs : class, IEventArgs;
    public void Send<TArgs>(TArgs args) where TArgs : class, IEventArgs;
}

public class EventSubject<T> : IEventSubject where T : class, IEventArgs
{
    private Action<T> action;
    
    public void Register<TArgs>(Action<TArgs> callback) where TArgs : class, IEventArgs
    {
        action += (Action<T>)callback;
    }

    public void Unregister<TArgs>(Action<TArgs> callback) where TArgs : class, IEventArgs
    {
        action -= (Action<T>)callback;
    }

    public void Send<TArgs>(TArgs args) where TArgs : class, IEventArgs
    {
        action?.Invoke(args as T);
    }
}