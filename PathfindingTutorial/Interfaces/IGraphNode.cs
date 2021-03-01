using System.Collections.Generic;

namespace PathfindingTutorial.Data_Structures
{
    public interface IGraphNode<T>
    {
        public void SetValue(T Value);

        public T GetValue();

        public List<IGraphNode<T>> GetNeighbors();

        public void AddNeighbor(IGraphNode<T> neighbor);

        public void RemoveNeighbor(IGraphNode<T> neighbor);

        public bool Marked { get; set; }

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

    }
}
