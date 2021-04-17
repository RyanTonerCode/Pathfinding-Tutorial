using System.Collections.Generic;

namespace PathfindingTutorial.Data_Structures
{
    public class GraphNode<T> : IGraphNode<T>
    {
        protected T value = default;
        protected readonly HashSet<IGraphNode<T>> neighbors;

        public bool Marked = false;

        bool IGraphNode<T>.Marked { get; set; }

        public GraphNode(T value)
        {
            this.value = value;
            neighbors = new(10);
        }

        public GraphNode(T value, int degree)
        {
            this.value = value;
            neighbors = new(degree);
        }

        public static void AddMutualNeighbor(IGraphNode<T> a, IGraphNode<T> b)
        {
            a.AddNeighbor(b);
            b.AddNeighbor(a);
        }

        public static void RemoveMutualNeighbor(IGraphNode<T> a, IGraphNode<T> b)
        {
            a.RemoveNeighbor(b);
            b.RemoveNeighbor(a);
        }

        public virtual HashSet<IGraphNode<T>> GetNeighbors()
        {
            return neighbors;
        }

        public virtual void AddNeighbor(IGraphNode<T> neighbor)
        {
            neighbors.Add(neighbor);
        }

        public virtual void RemoveNeighbor(IGraphNode<T> neighbor)
        {
            neighbors.Remove(neighbor);
        }

        public void SetValue(T Value)
        {
            value = Value;
        }

        public T GetValue()
        {
            return value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public IGraphNode<T> Clone()
        {
            var cln = new GraphNode<T>(value, GetDegree());
            return cln;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public int GetDegree() => neighbors.Count;
    }
}
