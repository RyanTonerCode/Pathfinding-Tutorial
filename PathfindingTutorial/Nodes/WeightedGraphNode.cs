using System.Collections.Generic;

namespace PathfindingTutorial.Data_Structures
{
    public class WeightedGraphNode<T> : GraphNode<T>, IGraphNode<T>
    {
        public Dictionary<IGraphNode<T>, double> EdgeWeights { get; private set; } = new Dictionary<IGraphNode<T>, double>();

        public WeightedGraphNode(T value) : base(value)
        {
        }

        public override void AddNeighbor(IGraphNode<T> neighbor)
        {
            AddNeighbor(neighbor);
        }

        public void AddNeighbor(IGraphNode<T> neighbor, double weight = 0)
        {
            if (!neighbors.Contains(neighbor))
            {
                neighbors.Add(neighbor);
                EdgeWeights.Add(neighbor, weight);
            }
        }

        public void AddMutualNeighbor(WeightedGraphNode<T> neighbor, double weight) => AddMutualNeighbor(this, neighbor, weight);

        public static void AddMutualNeighbor(WeightedGraphNode<T> n1, WeightedGraphNode<T> n2, double weight)
        {
            n1.AddNeighbor(n2, weight);
            n2.AddNeighbor(n1, weight);
        }

    }
}
