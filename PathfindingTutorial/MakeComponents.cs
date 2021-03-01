using PathfindingTutorial.Data_Structures;
using System;

namespace PathfindingTutorial
{
    public partial class Program
    {
        private static void MakeComponents()
        {
            var A = new WeightedGraphNode<char>('A');
            var B = new WeightedGraphNode<char>('B');
            var C = new WeightedGraphNode<char>('C');
            var D = new WeightedGraphNode<char>('D');
            var E = new WeightedGraphNode<char>('E');
            var F = new WeightedGraphNode<char>('F');

            A.AddMutualNeighbor(B);
            A.AddMutualNeighbor(C);
            B.AddMutualNeighbor(C);

            D.AddMutualNeighbor(E);

            var G = new Graph<char>(A, B, C, D, E, F);

            var components = G.FindConnectedComponents();

            foreach (var component in components)
            {
                component.PrintNodes();
                Console.WriteLine();
            }
        }
    }
}
