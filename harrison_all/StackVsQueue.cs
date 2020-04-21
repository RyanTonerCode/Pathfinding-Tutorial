using PathfindingTutorial.Data_Structures;
using System;

namespace PathfindingTutorial
{
    public partial class Program
    {
        static void StackVsQueue()
        {
            //demo the different between a stack and queue
            //set the default capacity to 4 (this is to check that scaling works)
            IGraphSearcher<int> test_stack = new Stack<int>(4);
            IGraphSearcher<int> test_queue = new Queue<int>(4);

            for (int i = 0; i < 100; i++)
            {
                test_stack.Add(i);
                test_queue.Add(i);
            }

            //show the difference between stacks and queues
            while (!test_stack.IsEmpty() && !test_queue.IsEmpty())
            {
                int from_stack = test_stack.Remove();
                int from_queue = test_queue.Remove();
                Console.WriteLine("Stack {0}, Queue {1}", from_stack, from_queue);
            }


        }
    }
}
