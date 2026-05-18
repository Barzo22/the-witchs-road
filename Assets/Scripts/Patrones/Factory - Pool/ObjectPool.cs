using System;
using System.Collections.Generic;

public class ObjectPool<T>
{
    private Func<T> _factoryMethod;
    private Action<T, bool> _turnOnOffCallback;
    private List<T> _availableObjects = new();
    private bool _dynamic;

    public ObjectPool(Func<T> factoryMethod, Action<T, bool> turnOnOffCallback,
                      int initialSize = 10, bool dynamic = true)
    {
        _factoryMethod = factoryMethod;
        _turnOnOffCallback = turnOnOffCallback;
        _dynamic = dynamic;

        for (int i = 0; i < initialSize; i++)
        {
            T obj = _factoryMethod();
            _turnOnOffCallback(obj, false);
            _availableObjects.Add(obj);
        }
    }

    public T GetObject()
    {
        T result;

        if (_availableObjects.Count > 0)
        {
            result = _availableObjects[0];
            _availableObjects.RemoveAt(0);
        }
        else if (_dynamic)
        {
            result = _factoryMethod();
        }
        else
        {
            return default;
        }

        _turnOnOffCallback(result, true);
        return result;
    }

    public void ReturnObject(T obj)
    {
        _turnOnOffCallback(obj, false);
        _availableObjects.Add(obj);
    }
}