using PathfindingTutorial.Data_Structures;
using System;
using System.Collections.Generic;

namespace PathfindingTutorial
{
    public partial class Program
    {

        private static void IsValidMinor()
        {

            var graphAdjacency = new int[5, 5];
            graphAdjacency[0, 1] = 1;
            graphAdjacency[0, 2] = 1;
            graphAdjacency[1, 2] = 1;
            graphAdjacency[1, 3] = 1;
            graphAdjacency[1, 4] = 1;
            graphAdjacency[2, 3] = 1;
            graphAdjacency[3, 4] = 1;

            var G = Graph<int>.GenerateGraphForAdjacencyMatrix(graphAdjacency, true);

            var minorAdjacency = new int[4, 4];
            minorAdjacency[0, 1] = 1;
            minorAdjacency[1, 2] = 1;
            minorAdjacency[1, 3] = 1;
            minorAdjacency[2, 3] = 1;

            var M = Graph<int>.GenerateGraphForAdjacencyMatrix(minorAdjacency, true);

            G.IsValidMinor(M);
        }
    }
}
