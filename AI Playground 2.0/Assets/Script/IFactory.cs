using System.Collections.Generic;

public interface IFactory
{
    T[] CreateArray<T>(int length); // function for creating an array of T
    bool FreeArray<T>(ref T[] array);

    Queue<T> CreateQueue<T>();
    bool FreeQueue<T>(ref Queue<T> queue);

    List<T> CreateList<T>();
    bool FreeList<T>(ref List<T> list);
}
