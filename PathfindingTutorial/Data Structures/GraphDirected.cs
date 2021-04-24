using System.Collections.Generic;

namespace PathfindingTutorial.Data_Structures
{
    public class DirectedGraph<T> : Graph<T>, IGraph<T>
    {


        public override List<Edge<T>> GetEdgeList()
        {
            var edges = new List<Edge<T>>();

            foreach (var node in graphStructure)
                foreach (var neighbor in node.GetNeighbors())
                {
                    if (node is WeightedGraphNode<T> wgn)
                        edges.Add(new Edge<T>(node, neighbor, wgn.EdgeWeights[neighbor]));
                    else
                        edges.Add(new Edge<T>(node, neighbor));
                }

            return edges;
        }
    }
}
