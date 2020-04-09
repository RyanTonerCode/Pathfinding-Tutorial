using System;
using System.Collections.Generic;
using System.Text;

namespace pathfinding_demo.Data_Structures
{
    public interface IGraph<T>
    {

        public void AddNode(IGraphNode<T> Node);

        public void RemoveNode(IGraphNode<T> Node);

    }
}
