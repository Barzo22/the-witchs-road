using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void EventReceiver(params object[] parameterContainer);

    private static Dictionary<GameEvents, EventReceiver> _events;

    public static void SubscribeToEvent(GameEvents eventType, EventReceiver listener)
    {
        if (_events == null)
            _events = new Dictionary<GameEvents, EventReceiver>();
        if (!_events.ContainsKey(eventType))
            _events.Add(eventType, null);
        _events[eventType] += listener;
    }

    public static void Unsubscribe(GameEvents eventType, EventReceiver listener)
    {
        if (_events == null) return;
        if (_events.ContainsKey(eventType))
            _events[eventType] -= listener;
    }

    public static void ExecuteEvent(GameEvents eventType, params object[] parameters)
    {
        if (_events == null)
        {
            
            Debug.Log("No events subscribed");
            
            return;
        }

        if (_events.ContainsKey(eventType) && _events[eventType] != null)
            _events[eventType](parameters);
    }

    public static void ExecuteEvent(GameEvents eventType)
    {
        ExecuteEvent(eventType, null);
    }
}