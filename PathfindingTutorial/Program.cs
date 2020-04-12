using System;
using System.Text;
using PathfindingTutorial.Data_Structures;
using PathfindingTutorial.Puzzle;

namespace PathfindingTutorial
{
    class Program
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

        static void MakeWeightedGraph()
        {
            WeightedGraphNode<char> A = new WeightedGraphNode<char>('A');
            WeightedGraphNode<char> B = new WeightedGraphNode<char>('B');
            WeightedGraphNode<char> C = new WeightedGraphNode<char>('C');
            WeightedGraphNode<char> D = new WeightedGraphNode<char>('D');
            WeightedGraphNode<char> E = new WeightedGraphNode<char>('E');
            WeightedGraphNode<char> F = new WeightedGraphNode<char>('F');
            WeightedGraphNode<char> G = new WeightedGraphNode<char>('G');

            A.AddMutualNeighbor(B, 4);
            A.AddMutualNeighbor(G, 2);
            A.AddMutualNeighbor(D, 1 );

            B.AddMutualNeighbor(D, 6);
            B.AddMutualNeighbor(G, 9);

            C.AddMutualNeighbor(D, 6);
            C.AddMutualNeighbor(E, 4);

            E.AddMutualNeighbor(F, 3);

            F.AddMutualNeighbor(G, 5);

            GraphNode<char>[] nodes = { A, B, C, D, E, F, G };
            Graph<char> a_graph = new Graph<char>(nodes);

            Console.WriteLine("Finding the shorting path between B and E");
            var path = a_graph.RunDijkstra(B,E);

            if (path == null)
                Console.WriteLine("Nope :(");
            else
            {
                Console.WriteLine("\n The shortest path has weight {0} ", path.PathWeightToHere);

                //use a stack to figure out the order to take
                var reverse_backtracking = new Stack<NodePath<char>>();

                while (path != null)
                {
                    reverse_backtracking.Add(path);
                    path = (WeightedNodePath<char>)path.Parent;
                }

                while (!reverse_backtracking.IsEmpty())
                {
                    var top = reverse_backtracking.Pop();
                    Console.Write("{0}", top.Node.GetValue());
                    if (reverse_backtracking.Count > 0)
                        Console.Write(" -> ");
                }

                Console.WriteLine("\n-------------------------------\n");
            }
        }

        static void SolvePuzzle(bool IsGreedy, bool print = false)
        {
            GameBoard gb = new GameBoard(3, 3);

            NodePath<GameBoard> solution = PuzzleSolver.A_Star_Search(gb, IsGreedy);
            //NodePath<GameBoard> solution2 = PuzzleSolver.A_Star_Search(gb, !IsGreedy);

            if (solution == null && print)
            {
                Console.WriteLine("Could not find a solution!");
                return;
            }

            Stack<NodePath<GameBoard>> stk = new Stack<NodePath<GameBoard>>();

            while (solution != null)
            {
                stk.Add(solution);
                solution = solution.Parent;
            }

            StringBuilder sb = new StringBuilder();

            while (!stk.IsEmpty())
            {
                var top = stk.Pop().Node;
                if(print)
                    sb.Append(top.GetValue()).AppendLine();
            }

            if (print)
                Console.WriteLine(sb);
            
        }

        static void Main(string[] args)
        {
            //StackVsQueue();
            //MakeGraph();
            //MakeWeightedGraph();

            int numTrials = 250;
            for (int i = 0; i < numTrials; i++)
                SolvePuzzle(false, numTrials == 1);
            

            Console.ReadLine();
        }
    }
}
