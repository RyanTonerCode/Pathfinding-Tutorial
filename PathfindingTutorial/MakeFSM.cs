using PathfindingTutorial.Data_Structures;
using System;

namespace PathfindingTutorial
{
    public partial class Program
    {

        /// <summary>
        /// Encapsulate state information of the FSM as a tuple
        /// </summary>
        private class VectorizeState
        {
            public int[] States;
            public VectorizeState(params int[] states)
            {
                States = states;
            }

            public VectorizeState IncrementState(int index)
            {
                int[] copy = new int[States.Length];
                Array.Copy(States, copy, States.Length);
                copy[index]++;
                return new VectorizeState(copy);
            }
        }

        private static readonly string[] hat = { "hat" };
        private static readonly string[] goggles = { "goggles" };
        private static readonly string[] jacket = { "jacket", "gloves" };
        private static readonly string[] pants = { "pants", "boots", "skis" };
        private static readonly string[] grab = { "grab ticket" };

        /// <summary>
        /// Recursively generate the FSM
        /// </summary>
        /// <param name="par"></param>
        private static void generateFSM(FSM_Node<VectorizeState> par, string[][] indices)
        {

            var p = par.GetValue();
            for(int ar = 0; ar < indices.Length; ar++)
            {
                string[] select = indices[ar];

                if (p.States[ar] == select.Length)
                    continue;
                var tmp = new FSM_Node<VectorizeState>(p.IncrementState(ar));
                par.AddNeighbor(tmp, select[p.States[ar]]);
            }

            foreach (var n in par.GetNeighbors())
                generateFSM((FSM_Node<VectorizeState>)n, indices);
        }
        public static void MakeFSM()
        {
            var starting_state = new VectorizeState(0, 0, 0, 0, 0);
            var starting_node = new FSM_Node<VectorizeState>(starting_state);

            string[][] transitions = new string[][] { hat, goggles, jacket, pants, grab };

            generateFSM(starting_node, transitions);

            int traces = 0;

            var queue = new Queue<NodePath<VectorizeState>>();

            var root = new NodePath<VectorizeState>(starting_node, null);
            queue.Enqueue(root);

            while (!queue.IsEmpty())
            {
                var top = queue.Dequeue();

                var top_state = top.Node.GetValue();

                traces = checkFinishedTrace(traces, top, top_state, transitions);

                //add all new nodes to the stack
                foreach (var neighbor in top.Node.GetNeighbors())
                    queue.Enqueue(new NodePath<VectorizeState>(neighbor, top));
            }
        }

        private static int checkFinishedTrace(int traces, NodePath<VectorizeState> top, VectorizeState top_state, string[][] transitions)
        {
            for (int ar = 0; ar < transitions.Length; ar++)
                if (top_state.States[ar] < transitions[ar].Length)
                    return traces;

            var stk = new Stack<NodePath<VectorizeState>>();

            NodePath<VectorizeState> ptr = top;

            while (ptr != null)
            {
                stk.Add(ptr);

                ptr = ptr.Parent;
            }

            Console.Write("Trace {0}: ", ++traces);

            while (!stk.IsEmpty())
            {
                var here = stk.Pop();
                if (here.Parent != null)
                {
                    var msg = ((FSM_Node<VectorizeState>)here.Parent.Node).GetMessage((FSM_Node<VectorizeState>)here.Node);
                    Console.Write(msg);
                    if (!stk.IsEmpty())
                        Console.Write(" -> ");
                }
            }
            Console.WriteLine();

            return traces;
        }
    }
}
