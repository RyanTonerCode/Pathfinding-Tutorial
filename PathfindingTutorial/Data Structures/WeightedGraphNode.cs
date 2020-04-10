using System;
using System.Collections.Generic;
using System.Text;

namespace PathfindingTutorial.Data_Structures
{
    public class WeightedGraphNode<T> : IGraphNode<T>
    {
        private T value = default;
        private readonly List<IGraphNode<T>> neighbors = new List<IGraphNode<T>>(10);

        public Dictionary<IGraphNode<T>, double> EdgeWeights { get; private set; } = new Dictionary<IGraphNode<T>, double>();

        public WeightedGraphNode(T value)
        {
            this.value = value;
        }

        public List<IGraphNode<T>> GetNeighbors()
        {
            return neighbors;
        }

        public void AddNeighbor(IGraphNode<T> neighbor) => AddNeighbor(neighbor, 0);

        public void AddNeighbor(IGraphNode<T> neighbor, double weight)
        {
            neighbors.Add(neighbor);
            EdgeWeights.Add(neighbor, weight);
        }

        public void SetValue(T Value)
        {
            value = Value;
        }

        public T GetValue()
        {
            return value;
        }
    }
}
