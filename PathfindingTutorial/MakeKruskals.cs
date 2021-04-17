using PathfindingTutorial.Data_Structures;
using System;

namespace PathfindingTutorial
{
    public partial class Program
    {
        private static void MakeKruskals()
        {
            var A = new WeightedGraphNode<char>('A');
            var B = new WeightedGraphNode<char>('B');
            var C = new WeightedGraphNode<char>('C');
            var D = new WeightedGraphNode<char>('D');
            var E = new WeightedGraphNode<char>('E');
            var F = new WeightedGraphNode<char>('F');

            A.AddMutualNeighbor(B, 1);
            A.AddMutualNeighbor(D, 2);
            B.AddMutualNeighbor(D, 4);
            B.AddMutualNeighbor(C, 2);
            C.AddMutualNeighbor(D, 3);
            C.AddMutualNeighbor(E, 4);
            C.AddMutualNeighbor(F, 5);
            E.AddMutualNeighbor(F, 1);

            var G = new Graph<char>(A, B, C, D, E, F);

            var MSF_G = G.KruskalsAlgorithmForMinimumSpanningForest();

            var edges = MSF_G.GetEdgeList();

            foreach (var edge in edges)
            {
                if (edge.Node1.GetValue().CompareTo(edge.Node2.GetValue()) > 0)
                    Console.WriteLine(edge.FlipDirection().ToString());
                else
                    Console.WriteLine(edge.ToString());
            }
        }
    }
}
