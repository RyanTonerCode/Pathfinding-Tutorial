
namespace PathfindingTutorial.Data_Structures
{
    public interface IGraph<T>
    {

        public void AddNode(IGraphNode<T> Node);

        public void RemoveNode(IGraphNode<T> Node);

    }
}
