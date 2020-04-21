using PathfindingTutorial.Data_Structures;
using System;

namespace PathfindingTutorial
{
    public partial class Program
    {
        /// <summary>
        /// Demo the difference between the stack and queue data structures
        /// </summary>
        static void StackVsQueue()
        {
            /*Set the default capacity to 4. The data structure will automatically rescale by doubling when necessary.
             * 
             * The IGraphSearcher interface maps an abstract interface between stack and queue:
             *      push, enqueue => add
             *      pop,  dequeue => remove
             * 
             */
           
            IGraphSearcher<int> test_stack = new Stack<int>(4);
            IGraphSearcher<int> test_queue = new Queue<int>(4);

            //add numbers from 1-100
            for (int i = 1; i <= 100; i++)
            {
                test_stack.Add(i);
                test_queue.Add(i);
            }

            //Show the difference between stacks and queues
            while (!test_stack.IsEmpty() && !test_queue.IsEmpty())
            {
                int from_stack = test_stack.Remove();
                int from_queue = test_queue.Remove();
                Console.WriteLine("Stack {0}, Queue {1}", from_stack, from_queue);
            }


        }
    }
}
