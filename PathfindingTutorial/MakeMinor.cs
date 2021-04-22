using PathfindingTutorial.Data_Structures;
using System;
using System.Collections.Generic;

namespace PathfindingTutorial
{
    public partial class Program
    {

        private static void IsValidMinor(int order)
        {

            var M = Graph<int>.GenerateCompleteGraph(4);
            M.RemoveEdge(0, 2);
            M.RemoveEdge(2, 3);

            var g6 = Graph<int>.GenerateCompleteGraph(6);
            g6.RemoveEdge(0, 1);
            g6.RemoveEdge(0, 3);
            g6.RemoveEdge(2, 3);
            g6.RemoveEdge(4, 5);
            g6.RemoveEdge(2, 5);
            g6.RemoveEdge(1, 3);
            g6.RemoveEdge(2, 4);

            g6.IsValidMinor(M, true);


            /*
            Console.WriteLine("Checking for this minor:");
            M.PrintAdjacencyMatrix();

            var graphs = LoadGraphsOfOrder(order);

            int totalGraphsWithMinor = 0;

            Console.WriteLine("Starting minor check against all graphs of order {0}\n", order);

            foreach (var G in graphs)
            {
                var valid_minor = G.IsValidMinor(M);
                if (valid_minor)
                {
                    G.PrintAdjacencyMatrix();
                    Console.WriteLine();
                    totalGraphsWithMinor++;
                }
            }

            graphs[^1].IsValidMinor(M, true);

            Console.WriteLine("There are {0} graphs with this minor out of {1} total graphs", totalGraphsWithMinor, graphs.Count);
            */

        }

        private static void PlanarityChecker(int order)
        {
            var graphs = LoadGraphsOfOrder(order);

            int totalPlanarGraphs = 0, totalConnectedGraphs = 0;

            Console.WriteLine("Starting planarity check against all graphs of order {0}\n", order);

            int i = 0;
            foreach (var G in graphs)
            {
                if(Graph<int>.IsPlanar(G))
                {
                    //G.PrintAdjacencyMatrix();
                    //Console.WriteLine();
                    totalPlanarGraphs++;
                    if (G.FindConnectedComponents().Count == 1)
                        totalConnectedGraphs++;
                }
                Console.WriteLine(i++);
            }

            Console.WriteLine("There are {0} connected planar graphs (of {1} planar graphs) out of {2} total graphs", totalConnectedGraphs, totalPlanarGraphs, graphs.Count);

        }
    }
}
