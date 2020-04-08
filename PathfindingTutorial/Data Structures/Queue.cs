using System;

namespace PathfindingTutorial.Data_Structures
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

        private T[] queue_data;

        private int queue_capacity;

        private const int max_capacity = 100000;

        /* The queue is stored from front-to-back, indicating the front where elements are taken from, and the
         * back where elements are removed. As the queue adds elements, the front will grow towards the rear, but
         * this must happen in a cyclical manner because of the way the memory is stored.
         * For example, a queue size of 4:
         * q = [0,0,0,0]
         * q.enqueue(1) => [1,0,0,0]
         * q.enqueue(2) => [1,2,0,0]
         * no surprise here!
         * except now dequeue an element...
         * we know that the 1 must be removed first, since it was added first (this is a queue, after all, right?!)
         * q.dequeue() => [0,2,0,0]
         * This seems to be a big problem. We now have an empty slot at the beginning of our array.
         * Except, if we also increment the rear, we may still grow the front towards the beginning of the array 
         * and use that space. For example,
         * q.enqueue(3) => q = [0,2,3,0] (same as above)
         * q.enqueue(4) => [0,2,3,4] (same as above)
         * q.enqueue(5) => [5,2,3,4] <= here we see why both a front and rear are necessary to fully utilize the queue's memory
         */
        private int front;
        private int rear;

        /// <summary>
        /// This method resizes and copies the array
        /// </summary>
        private void resize()
        {
            queue_capacity *= 2;
            if (queue_capacity > max_capacity)
                throw new QueueOverflowException();
            var buffer = new T[queue_capacity];

            int queueIterator = front;
            for(int i = 0; i < Count; i++)
            {
                buffer[i] = queue_data[queueIterator];
                queueIterator = (queueIterator + 1) % queue_data.Length;
            }
            queue_data = buffer;
        }


        public Queue(int capacity = 64)
        {
            Count = 0;
            queue_capacity = capacity;
            front = 0;
            rear = capacity - 1;
            queue_data = new T[capacity];
        }

        public int Count { get; private set; }

        public bool IsEmpty => Count == 0;

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
            T top = queue_data[front];
            front = (front + 1) % queue_capacity; //increment the front of the queue
            Count++;
            return top;
        }

        public void Enqueue(T value)
        {
            try
            {
                if (Count == queue_capacity)
                    resize();
            }
            catch (QueueOverflowException ex)
            {
                throw ex;
            }

            //place the newest elements at the rear of the queue
            rear = (rear + 1) % queue_capacity;

            queue_data[rear] = value;
            Count++;
        }



    }
}
