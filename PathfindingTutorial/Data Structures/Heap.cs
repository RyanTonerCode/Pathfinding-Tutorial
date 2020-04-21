using System;
using System.Collections.Generic;
using System.Text;

namespace PathfindingTutorial.Data_Structures
{
    public class Heap<T> : IPriorityQueue<T> where T : IComparable
    {
        private readonly List<T> heapData;  //list of heap elements
        private int lastIndex;              // index of last element in priority queue
        private int queue_capacity;         // index of last position in ArrayList
        private const int max_capacity = Program.MaxHeapSize;

        public Heap(int capacity)
        {
            if (capacity > max_capacity)
                throw new QueueOverflowException();
            heapData = new List<T>(capacity);
            lastIndex = -1;
            queue_capacity = capacity;
        }

        private void resize()
        {
            int newSize = queue_capacity * 2;
            if (newSize > max_capacity)
                throw new QueueOverflowException();
            queue_capacity = newSize;
        }

        // Returns true if this priority queue is empty; otherwise, returns false.
        public bool IsEmpty() =>  (lastIndex == -1);

        // Returns true if this priority queue is full; otherwise, returns false.
        public bool IsFull() => (lastIndex == queue_capacity - 1);

        private int getParentIndex(int element) => (element - 1) / 2;

        private void reheapUp(T element)
        // Current lastIndex position is empty.
        // Inserts element into the tree and ensures shape and order properties.
        {
            int hole = lastIndex;
            while (
                // hole is not root and element > hole's parent
                hole > 0 &&
                element.CompareTo(heapData[getParentIndex(hole)]) > 0)
            {
                // move hole's parent down and then move hole up
                int parent = getParentIndex(hole);
                heapData[hole] = heapData[parent];
                hole = parent;
            }
            heapData[hole] = element;  // place element into final hole
        }

        public void Enqueue(T element) 
        {
            if (IsFull())
            {
                try
                {
                    resize();
                }
                catch(QueueOverflowException ex)
                {
                    throw ex;
                }
            }
            else
            {
                lastIndex++;
                heapData.Insert(lastIndex, element);
                reheapUp(element);
            }
        }

    private int newHole(int hole, T element)
    // If either child of hole is larger than element, return the index
    // of the larger child; otherwise, return the index of hole.
    {
        int left = (hole * 2) + 1;
        int right = (hole * 2) + 2;

        if (left > lastIndex) // hole has no children
            return hole;
        else if (left == lastIndex) // hole has left child only
            return (element.CompareTo(heapData[left]) < 0) ? left : hole;

        // hole has two children 
        if (heapData[left].CompareTo(heapData[right]) < 0)
            return (heapData[right].CompareTo(element) <= 0) ? hole : right;

        // left child >= right child
        return (heapData[left].CompareTo(element) <= 0) ? hole : left;
    }

    private void reheapDown(T element)
    // Current root position is "empty";
    // Inserts element into the tree and ensures shape and order properties.
    {
        int hole = 0;      // current index of hole
        int newhole;       // index where hole should move to

        newhole = newHole(hole, element);   // find next hole
        while (newhole != hole)
        {
            heapData[hole] = heapData[newhole];  // move element up
            hole = newhole;                            // move hole down
            newhole = newHole(hole, element);          // find next hole
        }
        heapData[hole] = element;           // fill in the final hole
    }

    public T Dequeue()
    {
        T hold;      // element to be dequeued and returned
        T toMove;    // element to move down heap

        if (lastIndex == -1)
            throw new QueueUnderflowException("Priority queue is empty");
        else
        {
          hold = heapData[0];              // remember element to be returned
          toMove = heapData[lastIndex]; // element to reheap down
                    heapData.RemoveAt(lastIndex); //remove that element
          lastIndex--;                         // decrease priority queue size
          if (lastIndex != -1)
             reheapDown(toMove);               // restore heap properties
          return hold;                         // return largest element
        }
      }

    public override string ToString()
    // Returns a string of all the heap elements.
    {
        StringBuilder theHeap = new StringBuilder("the heap is:\n");
        for (int index = 0; index <= lastIndex; index++)
            theHeap.Append(index).Append(". ").Append(heapData[index]).Append("\n");
        return theHeap.ToString();
    }


    }
}
