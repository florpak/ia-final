using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void MethodToSubscribe(params object[] parameters);

    private static Dictionary<EventsType, MethodToSubscribe> _events;

    //Suscribirse al evento
    public static void SubscribeToEvent(EventsType eventType, MethodToSubscribe methodToSubscribe)
    {

        if (_events == null) _events = new Dictionary<EventsType, MethodToSubscribe>();

        if (!_events.ContainsKey(eventType))
        {
            _events.Add(eventType, null);
        }

        _events[eventType] += methodToSubscribe;
    }

    //Desuscribirse al evento
    public static void UnsubscribeToEvent(EventsType eventType, MethodToSubscribe methodToUnsubscribe)
    {
        if (_events == null) return;

        if (!_events.ContainsKey(eventType)) return;

        _events[eventType] -= methodToUnsubscribe;
    }

    //Dispara el evento
    public static void TriggerEvent(EventsType eventType, params object[] parameters)
    {
        if (_events == null) return;

        if (!_events.ContainsKey(eventType)) return;

        if (_events[eventType] == null) return;

        _events[eventType](parameters);
    }
}

public enum EventsType
{
    WALL_BLOCK,
}

