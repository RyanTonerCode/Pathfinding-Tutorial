using System;

namespace PathfindingTutorial.Data_Structures
{
    public interface IPriorityQueue<T> where T : IComparable
    {
        public T Dequeue();
        public void Enqueue(T item);
        public bool IsEmpty();
        public bool IsFull();
    }
}
