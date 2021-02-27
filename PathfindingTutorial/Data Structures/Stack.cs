using System;

namespace PathfindingTutorial.Data_Structures
{
    /// <summary>
    /// Denotes that stack is empty.
    /// </summary>
    class StackUnderflowException : Exception
    {
        private const string error = "Stack underflow: cannot retrieve item from empty stack.";

        public StackUnderflowException() : base(error)
        {
        }

        public StackUnderflowException(string message) : base(message)
        {
        }

        public StackUnderflowException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Denotes that stack cannot grow.
    /// </summary>
    class StackOverflowException : Exception
    {
        private const string error = "Stack overflow: stack cannot grow larger.";

        public StackOverflowException() : base(error)
        {
        }

        public StackOverflowException(string message) : base(message)
        {
        }

        public StackOverflowException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class Stack<T> : IGraphSearcher<T>
    {

        /// <summary>
        /// The data on the stack
        /// </summary>
        private T[] stack_data;

        /// <summary>
        /// The current capacity of our stack
        /// </summary>
        private int stack_capacity;

        /// <summary>
        /// The maximum allowed stack size. Change as needed.
        /// </summary>
        private const int max_capacity = Program.MaxStackSize;

        /// <summary>
        /// This method resizes and copies the array
        /// </summary>
        private void resize()
        {
            //double the stack capacity
            stack_capacity *= 2;

            //do not allow capacity to exceed the maximum
            if (stack_capacity > max_capacity)
                stack_capacity = max_capacity; 

            //use built in library to resize the array
            Array.Resize(ref stack_data, stack_capacity);
        }

        /// <summary>
        /// Creates a stack
        /// </summary>
        /// <param name="capacity">The default stack capacity</param>
        public Stack(int capacity = 64)
        {
            Count = 0;

            if (capacity > max_capacity)
                throw new StackOverflowException();

            stack_capacity = capacity;
            stack_data = new T[capacity];
        }

        /// <summary>
        /// Pass an array into the constructor to create a stack
        /// </summary>
        /// <param name="array"></param>
        public Stack(T[] array)
        {
            if (array.Length > max_capacity)
                throw new QueueOverflowException();
            Count = array.Length;
            stack_capacity = 64;
            while (stack_capacity < array.Length)
                stack_capacity *= 2;
            if (stack_capacity > max_capacity)
                stack_capacity = max_capacity;
            //copy the array into the stack
            array.CopyTo(stack_data, 0);
        }

        /// <summary>
        /// Returns the number of elements in the stack
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Returns whether or not this stack is empty
        /// </summary>
        public bool IsEmpty() => Count == 0;

        private int top_index => Count - 1;

        public T Peek()
        {
            if (IsEmpty())
                throw new StackUnderflowException();
            T top = stack_data[top_index];
            return top;
        }

        public T Pop()
        {
            if (IsEmpty())
                throw new StackUnderflowException();
            T top = stack_data[top_index];
            Count --;
            return top;
        }

        public void Push(T value)
        {
            try
            {
                if (Count == stack_capacity)
                    resize();
            }
            catch (StackOverflowException ex)
            {
                //pass the exception to the caller
                throw ex;
            }

            stack_data[Count] = value;
            Count++;
        }

        public void Add(T item) => Push(item);

        public T Remove() => Pop();

        public IGraphSearcher<T> Factory() => new Stack<T>();
    }
}
