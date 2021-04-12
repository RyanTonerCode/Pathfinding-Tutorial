
namespace PathfindingTutorial.Data_Structures
{
    public interface IGraph<T>
    {

        public void AddNode(IGraphNode<T> Node);

        public void RemoveNode(IGraphNode<T> Node, bool RemoveNeighbors);

        public void RemoveEdge(int i, int j, bool directed=false);

        public void ContractEdge(int i, int j);

        public int GetOrder();

        public int GetSize();

    }
}
