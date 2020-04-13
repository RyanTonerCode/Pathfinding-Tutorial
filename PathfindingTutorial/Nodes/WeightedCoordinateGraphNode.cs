
namespace PathfindingTutorial.Data_Structures
{
    public class WeightedCoordinateGraphNode<T> : WeightedGraphNode<T>, IGraphNode<T>
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public WeightedCoordinateGraphNode(T value, int X, int Y) : base(value)
        {
            this.X = X;
            this.Y = Y;
        }

    }
}
