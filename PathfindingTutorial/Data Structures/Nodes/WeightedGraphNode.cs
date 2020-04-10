using System;
using System.Collections.Generic;
using System.Text;

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
            AddNeighbor(neighbor, 0);
        }

        public void AddNeighbor(IGraphNode<T> neighbor, double weight)
        {
            neighbors.Add(neighbor);
            EdgeWeights.Add(neighbor, weight);
        }

    }
}
