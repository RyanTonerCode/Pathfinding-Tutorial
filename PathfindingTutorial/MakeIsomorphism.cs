using PathfindingTutorial.Data_Structures;
using System;
using System.Collections.Generic;

namespace PathfindingTutorial
{
    public partial class Program
    {

        private static void IsIsomorphic()
        {

            var graphAdjacency = new int[4, 4];
            graphAdjacency[0, 1] = 1;
            graphAdjacency[1, 2] = 1;
            graphAdjacency[1, 3] = 1;
            graphAdjacency[2, 3] = 1;

            var G1 = Graph<int>.GenerateGraphForAdjacencyMatrix(graphAdjacency, true);

            var minorAdjacency = new int[4, 4];
            minorAdjacency[0, 1] = 1;
            minorAdjacency[0, 2] = 1;
            minorAdjacency[0, 3] = 1;
            minorAdjacency[2, 3] = 1;

            var G2 = Graph<int>.GenerateGraphForAdjacencyMatrix(minorAdjacency, true);

            Console.WriteLine(Graph<int>.CheckGraphIsomorphism(G1, G2));
        }
    }
}
