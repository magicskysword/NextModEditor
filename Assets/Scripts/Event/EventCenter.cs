using System;
using System.Collections.Generic;

public static class EventCenter
{
    private static Dictionary<Type, IEventSubject> _events = new Dictionary<Type, IEventSubject>();

    public static IObservable<T> AsObservable<T>() where T : IEventArgs
    {
        if (!_events.TryGetValue(typeof(T), out var targetEvent))
        {
            targetEvent = new EventSubject<T>();
            _events.Add(typeof(T), targetEvent);
        }
        
        return ((EventSubject<T>)targetEvent).OnReceiver;
    }

    public static void Send<T>(T args) where T : IEventArgs
    {
        if (!_events.TryGetValue(typeof(T), out var targetEvent))
        {
            return;
        }

        var genericEvent = (EventSubject<T>)targetEvent;
        genericEvent.OnTrigger.OnNext(args);
    }
}