namespace pathfinding_demo.Data_Structures
{
    public class NodePath<T>
    {
        private IGraphNode<T> node;
        private NodePath<T> parent;

        public IGraphNode<T> Node { get => node; set => node = value; }
        public NodePath<T> Parent { get => parent; set => parent = value; }

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
