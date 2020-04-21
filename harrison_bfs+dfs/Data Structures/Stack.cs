using System;

namespace pathfinding_demo.Data_Structures
{
    /// Denotes that stack is empty.
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

    /// Denotes that stack cannot grow.
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

    public class Stack<T>
    {
        //********************PRIVATE**********************
        private T[] stack_data; // The data on the stack
        private int stack_capacity; //The current capacity of our stack
        private const int max_capacity = 100000; // The maximum allowed stack size. Change as needed.
        private void resize() //resizes and copies the array
        {
            stack_capacity *= 2; //double the stack capacity (WHY DOUBLE?: efficiency)
            if (stack_capacity > max_capacity) //do not allow capacity to exceed the maximum
                stack_capacity = max_capacity; 
            
            //create buffer array to extent array size
            var buffer = new T[stack_capacity]; //create an array of things (thats what T stands for of course) size of stack (REFERENCES???)
            Array.Resize(ref stack_data, stack_capacity); //use built in library to resize the array -- basically: resize stack to twice its size, up to maximum capacity
        }

        //********************PUBLIC**********************
        public int Count { get; private set; } // Returns the number of elements in the stack :::: this says, you can get count anywhere, but only change it inside the stack
        public bool IsEmpty => Count == 0; //Returns whether or not this stack is empty
        private int top_index => Count - 1; //returns the last spot in the stack (so the => means return a var with content: [content])

        /// Creates a stack
        /// <param name="capacity">The default stack capacity</param>
        public Stack(int capacity = 64) //make a new stack
        {
            Count = 0; //initialize count of elements in stack to 0 when making new stack
            stack_capacity = capacity; //we can specify how big we want the stack to be, or leave blank and get std 64 (cant it not be any bigger than max?)
            stack_data = new T[capacity]; //make a new array of things, sized to the stack's predefined capacity
        }

        /// Pass an array into the constructor to create a stack
        /// <param name="array"></param>
        public Stack(T[] array) 
        {
            if (array.Length > max_capacity)
                throw new QueueOverflowException();

            Count = array.Length;
            while (stack_capacity < array.Length) //WHERE IS STACK_CAPACITY INSTANTIATED AS A NUMBER? defaults to 0, but a bug
                stack_capacity *= 2;

            if (stack_capacity > max_capacity)
                stack_capacity = max_capacity;
        
            array.CopyTo(stack_data, 0); //copy the array into the stack
        }
        
        public T Peek()
        {
            if (IsEmpty)
                throw new StackUnderflowException();
            T top = stack_data[top_index];
            return top;
        }

        public T Pop() //We theoretically remove the data because it will be overwritten since Push() is predicated on Count
        {
            if (IsEmpty)
                throw new StackUnderflowException();
            T top = stack_data[top_index]; //Top index is also predicated on Count, not the arraylen
            Count --;
            return top;
        }

        public void Push(T value)
        {
            try
            {
                if (Count == stack_capacity) //When there are as many elements as there are spots in the array, double its size
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



    }
}
