using System;
using UniRx;

public interface IEventSubject
{
    
}

public class EventSubject<T> : IEventSubject where T : IEventArgs
{
    private Subject<T> _subject = new Subject<T>();

    public IObservable<T> OnReceiver => _subject;

    public IObserver<T> OnTrigger => _subject;
}