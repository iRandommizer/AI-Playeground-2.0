using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap2<T> where T : IHeapItem2<T>
{
    T[] listOfItems;
    int totalIndexCount;

    // Create a new node at the last position of the list and add in the new item. Re-sort the tree again
    public void Enqueue(T item)
    {

    }

    // Return the root node value then resort the whole tree again 
    public T Dequeue() // We'll always dequeue the root node so no need to pass an arguement 
    {

    }

    public void SortUp(T item)
    {

    }

    public void SortDown(T item)
    {

    }

    public void Compare(T item1, T item2)
    {

    }
}

public interface IHeapItem2<T>: IComparer<T>
{
    int HeapIndex { get; set; }
    T parentItem { get; set; } // There is no need for this variable because you can always calculate the parent node of a binary tree using the function: Mathf.RoundDown(currentNodeIndex/2)
}
