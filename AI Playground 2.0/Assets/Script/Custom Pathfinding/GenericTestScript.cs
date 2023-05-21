using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTestScript : IHeapItem2<GenericTestScript>
{
    public int lifeTotal;

    int heapIndex;

    public int HeapIndex { get { return heapIndex; } set { heapIndex = value; } }
    public GenericTestScript parentItem { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public GenericTestScript(int lifeTotal)
    {
        this.lifeTotal = lifeTotal;
    }

    public int CompareTo(GenericTestScript other)
    {
        // If greater than
        if (this.lifeTotal > other.lifeTotal) return 1;

        // If equal to
        else if (this.lifeTotal == other.lifeTotal) return 0;

        // If lesser than
        return -1;
    }

}
