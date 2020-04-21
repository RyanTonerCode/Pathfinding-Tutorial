using System;

namespace pathfinding_demo.Data_Structures
{
    class QueueUnderflowException : Exception
    {
        private const string error = "Queue underflow: cannot retrieve item from empty queue.";

        public QueueUnderflowException() : base(error)
        {
        }

        public QueueUnderflowException(string message) : base(message)
        {
        }

        public QueueUnderflowException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    class QueueOverflowException : Exception
    {
        private const string error = "Queue overflow: queue cannot grow larger.";

        public QueueOverflowException() : base(error)
        {
        }

        public QueueOverflowException(string message) : base(message)
        {
        }

        public QueueOverflowException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class Queue<T>
    {
        /* The queue is stored from back-to-front. The front indicates where elements are taken from
         * (this follows the first in, first out rule)
         * and the back indicates where elements are added (back of the line). 
         * 
         * As the queue adds elements, the rear will grow towards the front, but
         * this must happen in a cyclical manner because of the way the memory is stored.
         * For example, a queue size of 4:
         * q = [0,0,0,0], front = 0, rear = 3
         * q.enqueue(1) => [1,0,0,0], front = 0, rear = 0
         * q.enqueue(2) => [1,2,0,0], front = 0, rear = 1
         * no surprise here!
         * except now dequeue an element...
         * we know that the 1 must be removed first, since it was added first (this is a queue, after all, right?!)
         * q.dequeue() => [0,2,0,0], front = 1, rear = 1
         * This seems to be a big problem. We now have an empty slot at the beginning of our array.
         * Except, if we also increment the rear, we may still grow the front towards the beginning of the array 
         * and use that space. For example,
         * q.enqueue(3) => q = [0,2,3,0], front = 1, rear = 2 (same pattern as above)
         * q.enqueue(4) => [0,2,3,4], front = 1, rear = 3 (same pattern as above)
         * q.enqueue(5) => [5,2,3,4], front = 1, rear = 0 <= here we see why both a front and rear are necessary to fully utilize the queue's memory
         * 
         * Thus, when dequeue-ing, we must iterate cyclically front front = 1, and make sure to use % by the queue size after incrementing front.
         */

        //********************PRIVATE**********************
        private T[] queue_data;
        private int queue_capacity;
        private const int max_capacity = 100000;
        private int front;
        private int rear;
        private void resize() //resizes and copies the array
        {
            queue_capacity *= 2;
            if (queue_capacity > max_capacity)
                throw new QueueOverflowException();
            var buffer = new T[queue_capacity]; //basically an empty, temp copy of queue :::: so we can shift elements all the way around (cyclical)

            //copy the queue to the new queue going from front to back:
            int queueIterator = front; //find the front of the queue
            for(int i = 0; i < Count; i++) //for each element in array, 
            {
                buffer[i] = queue_data[queueIterator]; //copy over the data so that the front is now array[0] while starting from the og queue's front,
                queueIterator = (queueIterator + 1) % queue_data.Length; //and wrapping around the back if necessary
            }
            queue_data = buffer;

            //update front and rear:
            front = 0;
            rear = Count - 1;
        }

        //********************PUBLIC**********************
        public int Count { get; private set; } //we can only change it inside of stack
        public bool IsEmpty => Count == 0; //if nothing in the stack, empty is true else false

        public Queue(int capacity = 64) //make a new queue
        {
            Count = 0; //initialize count of elements
            queue_capacity = capacity;
            front = 0;
            rear = capacity - 1;
            queue_data = new T[capacity]; //mak sum noo plezes fo da dada
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new QueueUnderflowException();
            T top = queue_data[front];
            return top;
        }

        public T Dequeue()
        {
            if (IsEmpty)
                throw new QueueUnderflowException();
            T top = queue_data[front]; //get the first in line
            front = (front + 1) % queue_capacity; //increment the front of the queue to next element, wrapping around the back if necessary
            Count--;
            return top;
        }

        public void Enqueue(T value)
        {
            try
            {
                if (Count == queue_capacity) //double the array size if it is full
                    resize();
            }
            catch (QueueOverflowException ex)
            {
                throw ex;
            }

            //place the newest elements at the rear of the queue
            rear = (rear + 1) % queue_capacity; //the rear has to wrap around too

            queue_data[rear] = value; //add the 
            Count++;
        }
    }
}
