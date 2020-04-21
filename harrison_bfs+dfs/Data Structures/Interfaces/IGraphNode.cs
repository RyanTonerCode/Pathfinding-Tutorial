
using System.Collections.Generic;

namespace pathfinding_demo.Data_Structures
{
    public interface IGraphNode<T>
    {
        public void SetValue(T Value);

        public T GetValue();

        public List<IGraphNode<T>> GetNeighbors();

        public void AddNeighbor(IGraphNode<T> neighbor);
    }
}
