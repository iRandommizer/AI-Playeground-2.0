using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class DefaultFactory : IFactory
{
    public T[] CreateArray<T>(int length)
    {
        return new T[length];
    }

    public bool FreeArray<T>(ref T[] array)
    {
        array = null;
        return array == null; // Check if the array was freed
    }

    public Queue<T> CreateQueue<T>()
    {
        return new Queue<T>();
    }

    public bool FreeQueue<T>(ref Queue<T> queue)
    {
        queue = null;
        return queue == null;
    }

    public List<T> CreateList<T>()
    {
        return new List<T>();
    }

    public bool FreeList<T>(ref List<T> list)
    {
        list = null;
        return list == null;
    }
}
