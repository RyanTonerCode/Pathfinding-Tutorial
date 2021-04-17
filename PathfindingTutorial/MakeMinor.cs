using PathfindingTutorial.Data_Structures;
using System;
using System.Collections.Generic;

namespace PathfindingTutorial
{
    public partial class Program
    {

        private static void IsValidMinor()
        {
            var adj1 = new int[5, 5];
            adj1[0, 1] = 1;
            adj1[0, 2] = 1;
            adj1[3, 4] = 1;

            var adj2 = new int[5, 5];
            adj2[0, 1] = 1;
            adj2[2, 3] = 1;
            adj2[2, 4] = 1;

            var g1 = Graph<int>.GenerateGraphForAdjacencyMatrix(adj1);
            var g2 = Graph<int>.GenerateGraphForAdjacencyMatrix(adj2);

            bool result = Graph<int>.CheckGraphIsomorphism(g1, g2);

            /*
            var graphsUpTo5 = Graph<int>.GenerateNonIsomorphicGraphsOfOrder(5);

            Console.WriteLine(graphsUpTo5.Count);

            int i = 0;
            foreach (var graph in graphsUpTo5)
            {
                Console.WriteLine(i);
                graph.PrintAdjacencyMatrix();
                var deg = graph.GetDegreeSequence(false, true);
                Console.WriteLine(string.Join(",", deg));
                Console.WriteLine();
                i++;
            }
            */

            int break1 = 5;

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
