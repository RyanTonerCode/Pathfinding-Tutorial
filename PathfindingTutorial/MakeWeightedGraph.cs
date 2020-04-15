using PathfindingTutorial.Data_Structures;
using System;

namespace PathfindingTutorial
{
    public partial class Program
    {
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
            A.AddMutualNeighbor(D, 1);

            B.AddMutualNeighbor(D, 6);
            B.AddMutualNeighbor(G, 9);

            C.AddMutualNeighbor(D, 6);
            C.AddMutualNeighbor(E, 4);

            E.AddMutualNeighbor(F, 3);

            F.AddMutualNeighbor(G, 5);

            GraphNode<char>[] nodes = { A, B, C, D, E, F, G };
            Graph<char> a_graph = new Graph<char>(nodes);

            Console.WriteLine("Finding the shorting path between B and E");
            var path = a_graph.RunDijkstra(B, E);

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
    }
}
