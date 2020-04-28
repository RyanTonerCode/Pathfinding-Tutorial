using System.Collections.Generic;

namespace PathfindingTutorial.Data_Structures
{
    public class GraphNode<T> : IGraphNode<T>
    {
        protected T value = default;
        protected readonly List<IGraphNode<T>> neighbors = new List<IGraphNode<T>>(10);

        public bool Marked = false;

        public GraphNode(T value)
        {
            this.value = value;
        }

        public virtual List<IGraphNode<T>> GetNeighbors()
        {
            return neighbors;
        }

        public virtual void AddNeighbor(IGraphNode<T> neighbor)
        {
            neighbors.Add(neighbor);
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
