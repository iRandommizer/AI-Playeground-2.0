using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> items;

    public int Count { get { return items.Count; } }

    public PriorityQueue()
    {
        items = new List<T>();
    }

    public void Enqueue(T item)
    {
        items.Add(item);
        int childIndex = items.Count - 1;
        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;
            if (items[childIndex].CompareTo(items[parentIndex]) >= 0)
                break;
            T temp = items[childIndex];
            items[childIndex] = items[parentIndex];
            items[parentIndex] = temp;
            childIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        if (items.Count == 0)
            throw new InvalidOperationException("Priority queue is empty");

        int lastIndex = items.Count - 1;
        T frontItem = items[0];
        items[0] = items[lastIndex];
        items.RemoveAt(lastIndex);

        lastIndex--;
        int parentIndex = 0;
        while (true)
        {
            int childIndex = parentIndex * 2 + 1;
            if (childIndex > lastIndex)
                break;
            int rightChild = childIndex + 1;
            if (rightChild <= lastIndex && items[rightChild].CompareTo(items[childIndex]) < 0)
                childIndex = rightChild;
            if (items[parentIndex].CompareTo(items[childIndex]) <= 0)
                break;
            T temp = items[parentIndex];
            items[parentIndex] = items[childIndex];
            items[childIndex] = temp;
            parentIndex = childIndex;
        }
        return frontItem;
    }

    public T Peek()
    {
        if (items.Count == 0)
            throw new InvalidOperationException("Priority queue is empty");
        return items[0];
    }
}

