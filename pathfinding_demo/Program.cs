﻿using System;
using pathfinding_demo.Data_Structures;

namespace pathfinding_demo
{
    class Program
    {

        static void StackVsQueue()
        {
            //demo the different between a stack and queue
            //set the default capacity to 4 (this is to check that scaling works)
            Stack<int> test_stack = new Stack<int>(4);
            Queue<int> test_queue = new Queue<int>(4);

            for (int i = 0; i < 100; i++)
            {
                test_stack.Push(i);
                test_queue.Enqueue(i);
            }

            //show the difference between stacks and queues
            while (!test_stack.IsEmpty && !test_queue.IsEmpty)
            {
                int from_stack = test_stack.Pop();
                int from_queue = test_queue.Dequeue();
                Console.WriteLine("Stack {0}, Queue {1}", from_stack, from_queue);
            }

            
        }

        static void MakeGraph()
        {
            GraphNode<char> A = new GraphNode<char>('A');
            GraphNode<char> B = new GraphNode<char>('B');
            GraphNode<char> C = new GraphNode<char>('C');
            GraphNode<char> D = new GraphNode<char>('D');
            GraphNode<char> E = new GraphNode<char>('E');
            GraphNode<char> F = new GraphNode<char>('F');
            GraphNode<char> G = new GraphNode<char>('G');

            A.AddNeighbor(D);

            B.AddNeighbor(G);

            C.AddNeighbor(E);

            D.AddNeighbor(B);
            D.AddNeighbor(C);

            F.AddNeighbor(E);

            G.AddNeighbor(A);
            G.AddNeighbor(F);

            GraphNode<char>[] nodes = { A,B,C,D,E,F,G }; //add nodes to array
            Graph<char> a_graph = new Graph<char>(nodes); //convert array to graph object

            foreach(var a in nodes)
            {
                foreach(var b in nodes)
                {
                    if (a == b)
                        continue; //this is not a very interesting case...

                    Console.WriteLine("Does a path exist from {0} to {1}?\n", a.GetValue(), b.GetValue());
                    //var path = a_graph.RunDFS(a, b);
                    var path = a_graph.RunBFS(a, b);
                    if (path == null)
                        Console.WriteLine("Nope :(");
                    else
                    {
                        Console.WriteLine("Using backtracking, the path is...");

                        //use a stack to figure out the order to take
                        //var reverse_backtracking = new Stack<NodePath<char>>();
                        var reverse_backtracking = new Queue<NodePath<char>>();

                        while (path != null)
                        {
                            //reverse_backtracking.Push(path);
                            reverse_backtracking.Enqueue(path);

                            Console.Write("{0}", path.Node.GetValue());
                            path = path.Parent;
                            if (path != null)
                                Console.Write(" <- ");
                        }

                        Console.WriteLine("\n Using a stack, the path in-order is ");

                        while(!reverse_backtracking.IsEmpty){
                            //var top = reverse_backtracking.Pop();
                            var top = reverse_backtracking.Dequeue();
                            Console.Write("{0}", top.Node.GetValue());
                            if (reverse_backtracking.Count > 0)
                                Console.Write(" -> ");
                        }


                        Console.WriteLine("\n-------------------------------\n");
                    }
                }
            }

        }

        static void anotherGraph()
        {
            //Make a 10x10 grid where even columns have a barrier vertically centered height 8
            GraphNode<string>[][] grid = new GraphNode<string>[10][];
            for (int i = 0; i < 10; i++)
            {
                grid[i] = new GraphNode<string>[10]; 
                for (int j = 0; j < 10; j++)
                {
                    grid[i][j] = new GraphNode<string>(i+","+j);
                    if (i > 0)


                    if (j > 0 && (j % 2 != 0 || i == 0 || i == 9))
                    {
                        //add the node before, mutually
                        GraphNode<string>.AddMutualNeighbor(grid[i][j], grid[i][j-1]);
                        if (i > 1)
                        {
                            GraphNode<string>.AddMutualNeighbor(grid[i][j], grid[i-1][j]);
                        }
                    }
                }
            }
            GraphNode<string>[] nodes = new GraphNode<string>[100];
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    nodes[(i*10)+j] = grid[i][j];
                }
            }

        }

        static void Main(string[] args)
        {
            //StackVsQueue();
            MakeGraph();

            Console.ReadLine();
        }
    }
}
