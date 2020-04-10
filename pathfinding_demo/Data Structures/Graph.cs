using System.Collections.Generic;
using System;

namespace pathfinding_demo.Data_Structures
{
    public class Graph<T> : IGraph<T>
    {

        private readonly List<IGraphNode<T>> graphStructure = new List<IGraphNode<T>>(); 


        public Graph(IGraphNode<T>[] nodes)
        {
            graphStructure = new List<IGraphNode<T>>(nodes); //i assume this means make the list length = nodes length...?
        }

        public void AddNode(IGraphNode<T> Node)
        {
            graphStructure.Add(Node);
        }

        public void RemoveNode(IGraphNode<T> Node)
        {
            graphStructure.Remove(Node);
        }

        public NodePath<T> RunDFS(IGraphNode<T> Start, IGraphNode<T> End)
        {
            var stk = new Stack<NodePath<T>>(); //why are node paths separate from the nodes themselves, like in the trie?

            var begin = new NodePath<T>(Start, null);

            stk.Push(begin);

            HashSet<IGraphNode<T>> found = new HashSet<IGraphNode<T>>();

            while (stk.Count > 0) //diff from isempty? 
            {
                NodePath<T> cur = stk.Pop();
                //Console.Write(".");

                if (cur.Node == End)
                {
                    Console.WriteLine("");
                    return cur;
                }

                found.Add(cur.Node);

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                    if (!found.Contains(neighbor))
                        stk.Push(new NodePath<T>(neighbor, cur));
            }

            return null;
        }

        public NodePath<T> RunBFS(IGraphNode<T> Start, IGraphNode<T> End)
        {
            var stk = new Queue<NodePath<T>>();

            var begin = new NodePath<T>(Start, null);

            stk.Enqueue(begin);

            HashSet<IGraphNode<T>> found = new HashSet<IGraphNode<T>>();

            while (stk.Count > 0)
            {
                NodePath<T> cur = stk.Dequeue();
                //Console.Write(".");

                if (cur.Node == End)
                {
                    Console.WriteLine("");
                    return cur;
                }
                //Console.WriteLine(cur.Node.GetValue());

                found.Add(cur.Node);

                //add all new nodes to the stack
                foreach (var neighbor in cur.Node.GetNeighbors())
                    if (!found.Contains(neighbor))
                        stk.Enqueue(new NodePath<T>(neighbor, cur));
            }

            return null;
        }

    }
}
