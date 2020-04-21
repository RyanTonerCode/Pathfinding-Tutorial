using PathfindingTutorial.Data_Structures;
using System;

namespace PathfindingTutorial
{
    public partial class Program
    {
        static void MakeGraph()
        {
            //Create the vertex set
            GraphNode<char> A = new GraphNode<char>('A');
            GraphNode<char> B = new GraphNode<char>('B');
            GraphNode<char> C = new GraphNode<char>('C');
            GraphNode<char> D = new GraphNode<char>('D');
            GraphNode<char> E = new GraphNode<char>('E');
            GraphNode<char> F = new GraphNode<char>('F');
            GraphNode<char> G = new GraphNode<char>('G');

            //Create the edge set
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

            foreach (var node1 in nodes)
            {
                foreach (var node2 in nodes)
                {
                    if (node1 == node2)
                        continue; //this is not a very interesting case...

                    Console.WriteLine("Does a path exist from {0} to {1}?\n", node1.GetValue(), node2.GetValue());
                    var path = a_graph.RunDFS(node1, node2);

                    //another way to call this function and run DFS
                    //var path = a_graph.RunSearch(new Stack<NodePath<char>>(), node1, node2);

                    if (path == null)
                        Console.WriteLine("\tNope :(");
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

                        Console.WriteLine("\n\tUsing a stack, the path in-order is ");

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
            }

        }
    }
}
