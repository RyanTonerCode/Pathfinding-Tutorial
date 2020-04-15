
namespace PathfindingTutorial.Data_Structures
{
    public class NodePath<T>
    {
        public IGraphNode<T> Node { get; private set; }
        public NodePath<T> Parent { get; private set; }

        /// <summary>
        /// This is simply the number of edges taken to get to this node path
        /// </summary>
        public int PathLength { get; private set; }

        public NodePath(IGraphNode<T> n)
        {
            Node = n;
            PathLength = 0;
        }
        public NodePath(IGraphNode<T> n, NodePath<T> par)
        {
            Node = n;
            Parent = par;
            PathLength = 0;
        }
        public NodePath(IGraphNode<T> n, NodePath<T> par, int pathLength)
        {
            Node = n;
            Parent = par;
            PathLength = pathLength;
        }

    }
}
