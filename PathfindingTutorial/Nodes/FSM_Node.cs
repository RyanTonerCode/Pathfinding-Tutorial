
using System.Collections.Generic;

namespace PathfindingTutorial.Data_Structures
{
    /// <summary>
    /// FSM Nodes have an associated transition message for their neighbors
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FSM_Node<T> : GraphNode<T>
    {
        private readonly Dictionary<FSM_Node<T>, string> neighborTransitions = new Dictionary<FSM_Node<T>, string>(10);

        public FSM_Node(T value) : base(value)
        {
        }

        public virtual void AddNeighbor(FSM_Node<T> neighbor, string TransitionMessage)
        {
            base.AddNeighbor(neighbor);
            neighborTransitions.Add(neighbor, TransitionMessage);
        }

        public string GetMessage(FSM_Node<T> neighbor)
        {
            return neighborTransitions[neighbor];
        }
    }
}
