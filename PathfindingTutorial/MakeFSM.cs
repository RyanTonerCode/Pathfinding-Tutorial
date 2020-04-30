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
            public int I, J, K, L, M;
            public VectorizeState(int I, int J, int K, int L, int M)
            {
                this.I = I;
                this.J = J;
                this.K = K;
                this.L = L;
                this.M = M;
            }

            public VectorizeState IncI() => new VectorizeState(I + 1, J, K, L, M);
            public VectorizeState IncJ() => new VectorizeState(I, J + 1, K, L, M);
            public VectorizeState IncK() => new VectorizeState(I, J, K + 1, L, M);
            public VectorizeState IncL() => new VectorizeState(I, J, K, L + 1, M);
            public VectorizeState IncM() => new VectorizeState(I, J, K, L, M + 1);
        }

        private static string[] hat = { "hat" };
        private static string[] goggles = { "goggles" };
        private static string[] jacket = { "jacket", "gloves" };
        private static string[] pants = { "pants", "boots", "skis" };
        private static string[] grab = { "grab ticket" };

        /// <summary>
        /// Recursively generate the FSM
        /// </summary>
        /// <param name="par"></param>
        private static void generateFSM(FSM_Node<VectorizeState> par)
        {

            var p = par.GetValue();
            if (p.I < hat.Length)
            {
                var tmp = new FSM_Node<VectorizeState>(p.IncI());
                par.AddNeighbor(tmp, hat[p.I]);
            }
            if (p.J < goggles.Length)
            {
                var tmp = new FSM_Node<VectorizeState>(p.IncJ());
                par.AddNeighbor(tmp, goggles[p.J]);
            }
            if (p.K < jacket.Length)
            {
                var tmp = new FSM_Node<VectorizeState>(p.IncK());
                par.AddNeighbor(tmp, jacket[p.K]);
            }
            if (p.L < pants.Length)
            {
                var tmp = new FSM_Node<VectorizeState>(p.IncL());
                par.AddNeighbor(tmp, pants[p.L]);
            }
            if (p.M  < grab.Length)
            {
                var tmp = new FSM_Node<VectorizeState>(p.IncM());
                par.AddNeighbor(tmp, grab[p.M]);
            }
            foreach (var n in par.GetNeighbors())
                generateFSM((FSM_Node<VectorizeState>)n);
        }
        public static void MakeFSM()
        {

            var starting_state = new VectorizeState(0, 0, 0, 0, 0);
            var starting_node = new FSM_Node<VectorizeState>(starting_state);

            generateFSM(starting_node);

            int traces = 0;

            var queue = new Queue<NodePath<VectorizeState>>();

            var root = new NodePath<VectorizeState>(starting_node, null);
            queue.Enqueue(root);

            while (!queue.IsEmpty())
            {
                var top = queue.Dequeue();

                var top_state = top.Node.GetValue();

                traces = checkFinishedTrace(traces, top, top_state);

                //add all new nodes to the stack
                foreach (var neighbor in top.Node.GetNeighbors())
                    queue.Enqueue(new NodePath<VectorizeState>(neighbor, top));
            }
        }

        private static int checkFinishedTrace(int traces, NodePath<VectorizeState> top, VectorizeState top_state)
        {
            if (top_state.I == 1 && top_state.J == 1 && top_state.K == 2 & top_state.L == 3 && top_state.M == 1)
            {
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

            }

            return traces;
        }
    }
}
