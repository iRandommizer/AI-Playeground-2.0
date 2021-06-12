using System;
using UnityEngine;

// First time learning generic
public class Heap<T> where T : IHeapItem<T> //!!I need to figure out how those this workXD
{
    T[] items;
    int totalHeapCount;

    public int Count { get { return totalHeapCount; } }

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    // Adds items inside the heap then sorts the heap
    public void Add(T item)
    {
        // Given item's index is equals to the total number of heap items, which means the last item [Set Item's Heap index value]
        item.HeapIndex = totalHeapCount;

        // Setting the last item's value to the given item [Set the Item's value]
        items[totalHeapCount] = item;

        // Sort Item up the heap to its appropriate position
        SortUp(item);

        // Increase the total heap count after successfully adding a new item in the heap
        totalHeapCount++;
    }

    // Takes away the first time from the heap a
    public T RemoveFirst()
    {
        // First Item of the heap
        T firstItem = items[0];

        // Total heap count goes down
        totalHeapCount--;

        // The first item of the heap then becomes equals to the last item of the heap [Set the Item's value]
        items[0] = items[totalHeapCount];

        // Set the new heap index of that item sice it's moved to the first one index [Set Item's Heap index]
        items[0].HeapIndex = 0;

        // Sort the time down to fix the order of the heap
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item) 
    {
        SortUp(item);
    }

    public bool Contains(T item)
    {
        // If items[given item index] is equals to the item given
        return Equals(items[item.HeapIndex], item);
    }

    // Basically its a loop where it checks its children nodes if they have a lower value than itself, if it does then it swaps, it repeats this until it is at the proper position of the heap
    void SortDown (T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0; // Holds the value of the index of the item to know which to swap

            if(childIndexLeft < totalHeapCount) // Comparing index values not the values of the items 
            {
                swapIndex = childIndexLeft;

                if(childIndexRight < totalHeapCount)
                {
                    // If (L compared to R) < 0, choose right. Otherwise, stick with the left
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                // If item is a lower priority than one of it's child
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    public void SortUp(T item)
    {
        // Formula for getting the parent index
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];

            // If item's value is a higher priority than the parent item
            if(item.CompareTo(parentItem)> 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            // Set the new parent index
            parentIndex = (item.HeapIndex - 1)/2;
        }
    }

    // Basically swaps the values in the heap indices
    void Swap (T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

// To allow the class that uses this interface to have a heap index set to it
public interface IHeapItem<T> : IComparable<T> // IComparable just means it's an item which can be comparable hence can use the compare to function
{
    int HeapIndex{get;set;}
}
