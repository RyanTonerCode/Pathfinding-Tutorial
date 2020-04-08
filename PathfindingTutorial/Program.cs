using System;
using PathfindingTutorial.Data_Structures;

namespace PathfindingTutorial
{
    class Program
    {
        static void Main(string[] args)
        {

            Stack<int> test_stack = new Stack<int>(4);
            Queue<int> test_queue = new Queue<int>(4);

            for (int i = 0; i < 100; i++)
            {
                test_stack.Push(i);
                test_queue.Enqueue(i);
            }

            while (!test_stack.IsEmpty)
            {
                int from_stack = test_stack.Pop();
                int from_queue = test_queue.Dequeue();
                Console.WriteLine("Stack {0} Queue {1}", from_stack, from_queue);
            }

            Console.ReadLine();

        }
    }
}
