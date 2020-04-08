using System;
using System.Collections.Generic;
using System.Text;

namespace PathfindingTutorial.Data_Structures
{
    public class GraphNode<T> : IGraphNode<T>
    {
        private T value = default;
        private readonly List<IGraphNode<T>> neighbors = new List<IGraphNode<T>>(10);

        public GraphNode(T value)
        {
            this.value = value;
        }

        public List<IGraphNode<T>> GetNeighbors()
        {
            return neighbors;
        }

        public void AddNeighbor(IGraphNode<T> neighbor)
        {
            neighbors.Add(neighbor);
        }

        public static void AddMutualNeighbor(IGraphNode<T> a, IGraphNode<T> b)
        {
            a.AddNeighbor(b);
            b.AddNeighbor(a);
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
