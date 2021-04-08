using PathfindingTutorial.Data_Structures;
using System;
using System.Collections.Generic;

namespace PathfindingTutorial
{
    public partial class Program
    {

        private static void IsIsomorphic()
        {

            var g1_adj = new int[4, 4];
            g1_adj[0, 1] = 1;
            g1_adj[1, 2] = 1;
            g1_adj[1, 3] = 1;
            g1_adj[2, 3] = 1;

            var G1 = Graph<int>.GenerateGraphForAdjacencyMatrix(g1_adj, true);

            var g2_adj = new int[4, 4];
            g2_adj[0, 1] = 1;
            g2_adj[0, 2] = 1;
            g2_adj[0, 3] = 1;
            g2_adj[2, 3] = 1;

            var G2 = Graph<int>.GenerateGraphForAdjacencyMatrix(g2_adj, true);

            Console.WriteLine(Graph<int>.CheckGraphIsomorphism(G1, G2));

            var g3_adj = new int[7, 7];
            g3_adj[0, 1] = 1;
            g3_adj[1, 2] = 1;
            g3_adj[1, 3] = 1;
            g3_adj[2, 3] = 1;
            g3_adj[4, 5] = 1;

            var G3 = Graph<int>.GenerateGraphForAdjacencyMatrix(g3_adj, true);

            var g4_adj = new int[7, 7];
            g4_adj[0, 1] = 1;
            g4_adj[0, 2] = 1;
            g4_adj[0, 3] = 1;
            g4_adj[2, 3] = 1;
            g4_adj[4, 5] = 1;


            var G4 = Graph<int>.GenerateGraphForAdjacencyMatrix(g4_adj, true);

            Console.WriteLine(Graph<int>.CheckGraphIsomorphism(G3, G4));


        }
    }
}
