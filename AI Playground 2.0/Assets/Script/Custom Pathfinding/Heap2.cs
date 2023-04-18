using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap2<T> where T : IHeapItem2<T>
{
    T[] listOfItems;
    public int totalIndexCount;

    public Heap2(int maxHeapSize)
    {
        listOfItems = new T[maxHeapSize];
    }

    // Create a new node at the last position of the list and add in the new item. Re-sort the tree again
    public void Enqueue(T item)
    {
        item.HeapIndex = totalIndexCount;

        listOfItems[totalIndexCount] = item;

        SortUp(item);
 
        totalIndexCount++;

        Debug.Log("New Item:" + item.ToString());
    }

    // Return the root node value then resort the whole tree again 
    public T Dequeue() // We'll always dequeue the root node so no need to pass an arguement 
    {
        T firstItem = listOfItems[0];
        totalIndexCount--;
        listOfItems[0] = listOfItems[totalIndexCount];
        listOfItems[0].HeapIndex = 0;
        SortDown(listOfItems[0]);
        return firstItem;        
    }

    public void SortUp(T item)
    {
        T parentItem = listOfItems[Mathf.FloorToInt((item.HeapIndex -1)/2)]; //Find its parent node
        if(item.CompareTo(parentItem) > 0)
        {
            Swap(item, parentItem);
            SortUp(item);
        }
    }

    public void SortDown(T item)
    {
        if (item.HeapIndex * 2 + 1 >= listOfItems.Length) return;
        T childItemA = listOfItems[(item.HeapIndex * 2) + 1];
        T childItemB = default;
        if (item.HeapIndex * 2 + 2 <= listOfItems.Length)
        {
            childItemB = listOfItems[(item.HeapIndex * 2) + 2];
        }

        //Get Highest Child
        T HighestItem = childItemA.CompareTo(childItemB) >= 0 ? childItemA : childItemB;
        //Set up the recursive portion of the code and the swap
        if(item.CompareTo(HighestItem) < 0)
        {
            Swap(item, HighestItem);
            {
                SortDown(item);
            }
        }
    }

    public void Swap(T itemA, T itemB)
    {
        listOfItems[itemA.HeapIndex] = itemB;
        listOfItems[itemB.HeapIndex] = itemA;
        int itemAHeapIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAHeapIndex;
    }
}

public interface IHeapItem2<T>: IComparable<T>
{
    int HeapIndex { get; set; }
    T parentItem { get; set; } // There is no need for this variable because you can always calculate the parent node of a binary tree using the function: Mathf.RoundDown(currentNodeIndex/2)
}
