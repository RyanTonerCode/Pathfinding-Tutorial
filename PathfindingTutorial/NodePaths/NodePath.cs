
namespace PathfindingTutorial.Data_Structures
{
    public class NodePath<T>
    {
        public IGraphNode<T> Node { get; private set; }
        public NodePath<T> Parent { get; private set; }

        public int PathLength { get; private set; }

        public NodePath(IGraphNode<T> n)
        {
            Node = n;
        }
        public NodePath(IGraphNode<T> n, NodePath<T> par)
        {
            Node = n;
            Parent = par;
        }
        public NodePath(IGraphNode<T> n, NodePath<T> par, int pathLength)
        {
            Node = n;
            Parent = par;
            PathLength = pathLength;
        }

    }
}
