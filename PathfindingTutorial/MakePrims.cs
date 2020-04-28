using PathfindingTutorial.Data_Structures;
using System;
using System.Collections.Generic;

namespace PathfindingTutorial
{
    public partial class Program
    {
        private static void MakePrims()
        {
            List<WeightedGraphNode<int>> nodes = new List<WeightedGraphNode<int>>();
            for (int i = 0; i <= 8; i++)
                nodes.Add(new WeightedGraphNode<int>(i));

            nodes[0].AddMutualNeighbor(nodes[1], 4);
            nodes[0].AddMutualNeighbor(nodes[7], 8);
            nodes[1].AddMutualNeighbor(nodes[7], 11);
            nodes[1].AddMutualNeighbor(nodes[2], 8);
            nodes[7].AddMutualNeighbor(nodes[6], 1);
            nodes[7].AddMutualNeighbor(nodes[8], 7);
            nodes[2].AddMutualNeighbor(nodes[8], 2);
            nodes[2].AddMutualNeighbor(nodes[3], 7);
            nodes[2].AddMutualNeighbor(nodes[5], 4);
            nodes[8].AddMutualNeighbor(nodes[6], 6);
            nodes[6].AddMutualNeighbor(nodes[5], 2);
            nodes[5].AddMutualNeighbor(nodes[4], 10);
            nodes[5].AddMutualNeighbor(nodes[3], 14);
            nodes[3].AddMutualNeighbor(nodes[4], 9);

            Graph<int> G = new Graph<int>();
            foreach (var node in nodes)
                G.AddNode(node);

            Graph<int> MST = G.PrimsAlgorithmForMST();

            MST.PrintNodeInfo();

        }
    }
}
