using System.Collections.Generic;

namespace PathfindingTutorial.Data_Structures
{
    public class Tree<T> : Graph<T>
    {
        IGraphNode<T> root { get; set; }

        public new Tree<T> Clone()
        {
            var T = new Tree<T>();

            var clones = new Dictionary<IGraphNode<T>, IGraphNode<T>>();

            foreach (var node in graphStructure)
            {
                var clone = node.Clone();
                clones.Add(node, clone);
                T.AddNode(clone);
            }
            foreach (var node in graphStructure)
                foreach (var neighbor in node.GetNeighbors())
                    clones[node].AddNeighbor(clones[neighbor]);

            return T;
        }

        /// <summary>
        /// V = E + 1
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool CheckTreeOrderProperty(Tree<T> t) => t.GetOrder() - t.GetSize() == 1;

        #region Prufer Encoding
        /// <summary>
        /// Create a prufer encoding for a tree
        /// The tree should have nodes with values labelled 1...n
        /// </summary>
        /// <param name="tree"></param>
        /// <returns>Prufer encoding as a list</returns>
        public static List<int> GeneratePruferEncodingForTree(Tree<int> tree)
        {
            var prufer = new List<int>(tree.graphStructure.Count - 2);

            //clone the tree for needed modifications
            var tree_copy = tree.Clone();

            while(tree_copy.graphStructure.Count > 2)
            {
                //find node with smallest label that has degree 1
                for (int i = 0; i < tree_copy.graphStructure.Count; i++)
                {
                    var node = tree_copy.graphStructure[i];
                    var neighborList = node.GetNeighbors();
                    if (neighborList.Count == 1)
                    {
                        var onlyNeighbor = neighborList.GetEnumerator().Current;
                        //remove the leaf node from the neighbor's list
                        onlyNeighbor.RemoveNeighbor(node);
                        //add the value of the leaf's only neighbor to the prufer sequence
                        prufer.Add(onlyNeighbor.GetValue());
                        //remove the leaf node from the graph structure
                        tree_copy.graphStructure.RemoveAt(i);
                        break;
                    }
                }
            }

            return prufer;
        }

        /// <summary>
        /// Creates a tree that has the given prufer encoding
        /// </summary>
        /// <param name="prufer"></param>
        /// <returns></returns>
        public static Tree<int> PruferEncodingToTree(int[] prufer)
        {
            var T = new Tree<int>();

            var labeling = new Dictionary<int, GraphNode<int>>();

            int n = 2 + prufer.Length;

            for (int i = 1; i <= n; i++)
            {
                var node = new GraphNode<int>(i);
                labeling.Add(i, node);
                T.AddNode(node);
            }

            //count the number of times a label appears in the encoding
            var pruferCount = new int[n];

            foreach (var num in prufer)
            {
                //increase the number of times this label appears
                pruferCount[num-1]++;
            }

            GraphNode<int> prufer_node = null;

            for(int prufer_index = 0; prufer_index < prufer.Length; prufer_index++)
                for(int i = 1; i <= n; i++)
                {
                    if (pruferCount[i-1] == 0)
                    {
                        var leaf_node = labeling[i];
                        var neighbor_index = prufer[prufer_index];
                        prufer_node = labeling[neighbor_index];
                        IGraphNode<int>.AddMutualNeighbor(leaf_node, prufer_node);

                        //assigned prufer for the current label
                        pruferCount[i-1] = -1;
                        //decrement prufer count for neighbor label
                        pruferCount[neighbor_index-1]--;

                        break;
                    }
                }

            //assign the last vertex to the last prufer node
            if(prufer_node != null)
                IGraphNode<int>.AddMutualNeighbor(prufer_node, labeling[n]);

            return T;
        }
        #endregion

        public Tree(params IGraphNode<T>[] nodes) : base(nodes)
        {
        }

        public Tree(IGraphNode<T>[,] nodes) : base(nodes)
        {
        }

        public Tree(List<IGraphNode<T>> graph) : base(graph)
        {
        }

    }
}
