using PathfindingTutorial.Data_Structures;
using System;

namespace PathfindingTutorial
{
    public partial class Program
    {

        private static void IsIsomorphic()
        {
            var a1 = new GraphNode<int>(1);
            var b1 = new GraphNode<int>(2);
            var c1 = new GraphNode<int>(3);
            var d1 = new GraphNode<int>(4);
            var e1 = new GraphNode<int>(5);
            var f1 = new GraphNode<int>(6);

            IGraphNode<int>.AddMutualNeighbor(a1, b1);
            IGraphNode<int>.AddMutualNeighbor(b1, f1);
            IGraphNode<int>.AddMutualNeighbor(b1, c1);
            IGraphNode<int>.AddMutualNeighbor(c1, d1);
            IGraphNode<int>.AddMutualNeighbor(d1, e1);
            var G1 = new Graph<int>(a1, b1, c1, d1, e1, f1);

            var a2 = new GraphNode<int>(1);
            var b2 = new GraphNode<int>(2);
            var c2 = new GraphNode<int>(3);
            var d2 = new GraphNode<int>(4);
            var e2 = new GraphNode<int>(5);
            var f2 = new GraphNode<int>(6);

            IGraphNode<int>.AddMutualNeighbor(a2, b2);
            IGraphNode<int>.AddMutualNeighbor(b2, c2);
            IGraphNode<int>.AddMutualNeighbor(b2, f2);
            IGraphNode<int>.AddMutualNeighbor(c2, d2);
            IGraphNode<int>.AddMutualNeighbor(d2, e2);
            var G2 = new Graph<int>(f2, e2, d2, c2, b2, a2);

            Graph<int>.CheckGraphIsomorphism(G1, G2);

            Console.WriteLine("\n----------------------------------------------\n");

            int numberVertices = 4;

            var g3_adj = new int[numberVertices, numberVertices];
            g3_adj[0, 1] = 1;
            g3_adj[1, 2] = 1;
            g3_adj[1, 3] = 1;
            g3_adj[2, 3] = 1;

            var G3 = Graph<int>.GenerateGraphForAdjacencyMatrix(g3_adj, true);

            var g4_adj = new int[numberVertices, numberVertices];
            g4_adj[0, 1] = 1;
            g4_adj[0, 2] = 1;
            g4_adj[0, 3] = 1;
            g4_adj[2, 3] = 1;

            var G4 = Graph<int>.GenerateGraphForAdjacencyMatrix(g4_adj, true);

            Graph<int>.CheckGraphIsomorphism(G3, G4);

            Console.WriteLine("\n----------------------------------------------\n");

            numberVertices = 7;

            var g5_adj = new int[numberVertices, numberVertices];
            g5_adj[0, 1] = 1;
            g5_adj[1, 2] = 1;
            g5_adj[1, 3] = 1;
            g5_adj[2, 3] = 1;
            g5_adj[4, 5] = 1;

            var G5 = Graph<int>.GenerateGraphForAdjacencyMatrix(g5_adj, true);

            var g6_adj = new int[numberVertices, numberVertices];
            g6_adj[0, 1] = 1;
            g6_adj[0, 2] = 1;
            g6_adj[0, 3] = 1;
            g6_adj[2, 3] = 1;
            g6_adj[5, 6] = 1;

            var G6 = Graph<int>.GenerateGraphForAdjacencyMatrix(g6_adj, true);

            Graph<int>.CheckGraphIsomorphism(G5, G6);

        }
    }
}
