using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace PathfindingTutorial.Data_Structures
{
    public class WeightedNodePath<T> : NodePath<T>
    {
        public double Weight { get => weight; set => weight = value; }

        private double weight;

        public WeightedNodePath(IGraphNode<T> n) : base(n)
        {
        }
        public WeightedNodePath(IGraphNode<T> n, NodePath<T> par) : base(n,par)
        {
        }

        public WeightedNodePath(IGraphNode<T> n, NodePath<T> par, double weight) : base(n, par)
        {
            Weight = weight;
        }
    }
}
