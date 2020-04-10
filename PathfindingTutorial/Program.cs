using System;
using System.Text;
using PathfindingTutorial.Data_Structures;

namespace PathfindingTutorial
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
            while (!test_stack.IsEmpty() && !test_queue.IsEmpty())
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

            IGraphNode<char>.AddMutualNeighbor(A, B);

            GraphNode<char>[] nodes = { A, B, C, D, E, F, G };



            Graph<char> a_graph = new Graph<char>(nodes);


            foreach(var a in nodes)
            {
                foreach(var b in nodes)
                {
                    if (a == b)
                        continue; //this is not a very interesting case...

                    Console.WriteLine("Does a path exist from {0} to {1}?\n", a.GetValue(), b.GetValue());
                    var path = a_graph.RunDFS(a, b);

                    var path2 = a_graph.RunSearch(new Stack<NodePath<char>>(), a, b);

                    if (path == null)
                        Console.WriteLine("Nope :(");
                    else
                    {
                        Console.WriteLine("Using backtracking, the path is...");

                        //use a stack to figure out the order to take
                        var reverse_backtracking = new Stack<NodePath<char>>();

                        while (path != null)
                        {
                            reverse_backtracking.Push(path);

                            Console.Write("{0}", path.Node.GetValue());
                            path = path.Parent;
                            if (path != null)
                                Console.Write(" <- ");
                        }

                        Console.WriteLine("\n Using a stack, the path in-order is ");

                        while(!reverse_backtracking.IsEmpty()){
                            var top = reverse_backtracking.Pop();
                            Console.Write("{0}", top.Node.GetValue());
                            if (reverse_backtracking.Count > 0)
                                Console.Write(" -> ");
                        }


                        Console.WriteLine("\n-------------------------------\n");
                    }
                }
            }

        }

        static (WeightedGraphNode<string>[][], char[][]) makeGrid30x30()
        {
            //Make a 100x100 grid where even columns have two barriers of equal height 35 with the space between them vertically centered
            //And connect the whitespace
            WeightedGraphNode<string>[][] grid = new WeightedGraphNode<string>[30][];
            char[][] gridPath = new char[grid.Length][];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new WeightedGraphNode<string>[30];
                gridPath[i] = new char[grid[i].Length];
                for (int j = 0; j < grid[i].Length; j++)
                {
                    grid[i][j] = new WeightedGraphNode<string>(i + "," + j);
                    if (i % 2 != 0) //if we are on an odd column and there is a row above us, connect the node above us
                    {
                        if (j > 0 && j < grid[i].Length - 1 && i < grid.Length - 2)
                        {
                            grid[i][j].AddNeighbor(grid[i][j - 1], 2);
                            grid[i][j-1].AddNeighbor(grid[i][j],100);
                            gridPath[i][j] = 'Y';
                        }
                    }
                    if (j == 1 || j == 15 || j == 28) //if we are on the top or bottom row and there is a column before us, connect the node before us
                    {
                        if (i > 0 && i < grid.Length - 2)
                        {
                            grid[i][j].AddNeighbor(grid[i - 1][j],1);
                            grid[i-1][j].AddNeighbor(grid[i][j],1);
                            gridPath[i][j] = 'Y';
                        }
                    }
                }
            }
            return (grid, gridPath);
        }

        static void anotherGraph()
        {

            var gridData = makeGrid30x30();
            WeightedGraphNode<string>[][] grid = gridData.Item1;
            char[][] gridPath = gridData.Item2;

            WeightedGraphNode<string>[] nodes = new WeightedGraphNode<string>[grid.Length * grid[0].Length]; //convert the grid to an array for pathfinding
            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length; j++)
                {
                    nodes[(i * grid.Length) + j] = grid[i][j];
                }
            }

            Graph<string> a_graph = new Graph<string>(nodes); //convert array to graph object
            var path = a_graph.RunDjikstra(grid[1][5], grid[25][20]);
            if (path == null)
                Console.WriteLine("Nope :( \n");
            else
            {
                Console.WriteLine("Success! :) \n");
                printGrid(grid, gridPath, path);
            }
        }
        static void printGrid(GraphNode<string>[][] grid, char[][] gridPath, NodePath<string> path)
        {
            StringBuilder output = new StringBuilder();

            string[] route = new string[100000];
            for (int a = 0; path != null; a++)
            {
                route[a] = (path.Node.GetValue());
                path = path.Parent;
            }

            output.Append("    ");
            for (int i = 0; i < grid[0].Length; i++)
            {
                output.Append(" " + i + (i < 10 ? " " : ""));
            }
            output.Append("\n");

            for (int j = 0; j < grid.Length; j++)
            {
                output.Append(" " + j + (j < 10 ? "  " : " "));
                for (int i = 0; i < grid[j].Length; i++)
                {
                    string cordos = i + "," + j;
                    if (Array.Exists(route, element => element == cordos))
                    {
                        output.Append(" ¤ ");
                    }
                    else
                    {
                        output.Append(gridPath[i][j] == 'Y' ? "   " : "███");
                    }
                }
                output.Append("\n");
            }
            Console.Write(output);
        }


        static void Main(string[] args)
        {
            //StackVsQueue();
            //MakeGraph();
            anotherGraph();

            Console.ReadLine();
        }
    }
}
