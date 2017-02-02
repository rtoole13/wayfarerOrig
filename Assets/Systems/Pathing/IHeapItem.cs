using System;
public interface IHeapItem<T> : IComparable
{
    int HeapIndex { get; set; }
}