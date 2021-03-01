﻿using System.Collections.Generic;

namespace PathfindingTutorial.Data_Structures
{
    public interface IGraphNode<T>
    {
        public void SetValue(T Value);

        public T GetValue();

        public List<IGraphNode<T>> GetNeighbors();

        public void AddNeighbor(IGraphNode<T> neighbor);

        public static void AddMutualNeighbor(IGraphNode<T> a, IGraphNode<T> b)
        {
            a.AddNeighbor(b);
            b.AddNeighbor(a);
        }

    }
}
