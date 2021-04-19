using PathfindingTutorial.Data_Structures;
using System;

namespace PathfindingTutorial
{
    public partial class Program
    {

        private static void IsValidMinor()
        {
            var K33 = Graph<int>.GenerateCompleteBipartiteGraph(3, 3);
            K33.PrintAdjacencyMatrix();

            var K5 = Graph<int>.GenerateCompleteGraph(5);
            K5.PrintAdjacencyMatrix();

            var minorAdjacency = new int[4, 4];
            minorAdjacency[0, 1] = 1;
            minorAdjacency[0, 2] = 1;
            minorAdjacency[0, 3] = 1;
            var M = Graph<int>.GenerateGraphForAdjacencyMatrix(minorAdjacency);

            Console.WriteLine("Checking for this minor:");
            M.PrintAdjacencyMatrix();

            int graphOrder = 6;

            var graphs = LoadGraphsOfOrder(graphOrder);

            int totalGraphsWithMinor = 0;

            Console.WriteLine("Starting minor check against all graphs of order {0}\n", graphOrder);

            foreach (var G in graphs)
            {
                if (G.FindConnectedComponents().Count != 1)
                    continue;

                var k33minor = G.IsValidMinor(K33);
                if (!k33minor)
                {
                    var k5minor = G.IsValidMinor(K5);
                    if (!k5minor)
                    {
                        G.PrintAdjacencyMatrix();
                        Console.WriteLine();
                        totalGraphsWithMinor++;
                    }
                }
            }

            Console.WriteLine("There are {0} graphs out of {1} with this minor", totalGraphsWithMinor, graphs.Count);

        }
    }
}
