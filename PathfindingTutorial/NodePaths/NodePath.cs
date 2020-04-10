
namespace PathfindingTutorial.Data_Structures
{
    public class NodePath<T>
    {
        public IGraphNode<T> Node { get; private set; }
        public NodePath<T> Parent { get; private set; }

        public NodePath(IGraphNode<T> n)
        {
            Node = n;
        }
        public NodePath(IGraphNode<T> n, NodePath<T> par)
        {
            Node = n;
            Parent = par;
        }
    }
}
