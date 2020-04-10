using System;

namespace PathfindingTutorial.Data_Structures
{
    public class WeightedNodePath<T> : NodePath<T>, IComparable
    {
        /// <summary>
        /// The weight of the path to get to this specific Node
        /// </summary>
        public double PathWeightToHere { get; private set; }

        public WeightedNodePath(IGraphNode<T> n) : base(n)
        {
            PathWeightToHere = 0;
        }
        public WeightedNodePath(IGraphNode<T> n, NodePath<T> par) : base(n,par)
        {
            PathWeightToHere = 0;
        }

        public WeightedNodePath(IGraphNode<T> n, NodePath<T> par, double weight) : base(n, par)
        {
            PathWeightToHere = weight;
        }

        public int CompareTo(object obj)
        {
            if (obj is WeightedNodePath<T> cast) //in this order, lower values will return positive
                return cast.PathWeightToHere.CompareTo(PathWeightToHere);
            throw new NotImplementedException();
        }
    }
}
