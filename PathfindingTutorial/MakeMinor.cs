using PathfindingTutorial.Data_Structures;
using System;
using System.Collections.Generic;

namespace PathfindingTutorial
{
    public partial class Program
    {

        private static void IsValidMinor()
        {
            //var g = Graph<int>.GenerateGraphForDegreeSequence(new int[] { 5, 3, 3, 2, 2, 2, 1 });

            //g.PrintAdjacencyMatrix();

            /*
            //EXAMPLE 1
            var graphAdjacency = new int[6, 6];
            graphAdjacency[0, 1] = 1;
            graphAdjacency[0, 2] = 1;
            graphAdjacency[0, 3] = 1;
            graphAdjacency[1, 4] = 1;
            graphAdjacency[1, 5] = 1;

            var minorAdjacency = new int[5, 5];
            minorAdjacency[0, 1] = 1;
            minorAdjacency[0, 2] = 1;
            minorAdjacency[0, 3] = 1;
            minorAdjacency[0, 4] = 1;
            */

            /*
            //EXAMPLE 2
            var graphAdjacency = new int[5, 5];
            var minorAdjacency = new int[4, 4];
            graphAdjacency[0, 1] = 1;
            graphAdjacency[0, 2] = 1;
            graphAdjacency[1, 2] = 1;
            graphAdjacency[1, 3] = 1;
            graphAdjacency[1, 4] = 1;
            graphAdjacency[2, 3] = 1;
            graphAdjacency[3, 4] = 1;

            minorAdjacency[0, 1] = 1;
            minorAdjacency[1, 2] = 1;
            minorAdjacency[1, 3] = 1;
            //minorAdjacency[2, 3] = 1;

            var G = Graph<int>.GenerateGraphForAdjacencyMatrix(graphAdjacency, undirected);
            var M = Graph<int>.GenerateGraphForAdjacencyMatrix(minorAdjacency, undirected);
            */

            //k1,3

            /**
            var minorAdjacency = new int[5, 5];
            minorAdjacency[0, 1] = 1;
            minorAdjacency[0, 2] = 1;
            minorAdjacency[1, 3] = 1;
            minorAdjacency[2, 4] = 1;
            minorAdjacency[3, 4] = 1;

            var graphAdjacency = new int[5, 5];
            graphAdjacency[0, 2] = 1;
            graphAdjacency[1, 2] = 1;
            graphAdjacency[1, 3] = 1;
            graphAdjacency[1, 4] = 1;
            graphAdjacency[2, 3] = 1;
            graphAdjacency[2, 4] = 1;
            graphAdjacency[3, 4] = 1;

            var G = Graph<int>.GenerateGraphForAdjacencyMatrix(graphAdjacency, undirected);
            var M = Graph<int>.GenerateGraphForAdjacencyMatrix(minorAdjacency, undirected);
            */

            var minorAdjacency = new int[4, 4];
            minorAdjacency[0, 1] = 1;
            minorAdjacency[0, 2] = 1;
            minorAdjacency[0, 3] = 1;
            minorAdjacency[1, 2] = 1;
            minorAdjacency[1, 3] = 1;
            //minorAdjacency[2, 3] = 1;


            var G = Graph<int>.GenerateCompleteGraph(6);
            G.RemoveEdge(0, 1);
            G.RemoveEdge(0, 2);
            G.RemoveEdge(0, 3);
            G.RemoveEdge(0, 4);
            G.RemoveEdge(1, 3);
            G.RemoveEdge(2, 4);
            G.RemoveEdge(3, 5);
            G.RemoveEdge(4, 5);

            var M = Graph<int>.GenerateGraphForAdjacencyMatrix(minorAdjacency);

            G.IsValidMinor(M);
        }
    }
}
