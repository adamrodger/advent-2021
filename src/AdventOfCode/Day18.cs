using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 18
    /// </summary>
    public class Day18
    {
        public long Part1(string[] input)
        {
            var node = input.Skip(1).Aggregate(Parse(input[0]), (current, line) => Add(current, Parse(line)));

            return Magnitude(node);
        }

        public long Part2(string[] input)
        {
            long max = long.MinValue;

            // just try every combination...
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var left = Parse(input[i]);
                    var right = Parse(input[j]);
                    var final = Add(left, right);

                    long magnitude = Magnitude(final);
                    max = Math.Max(max, magnitude);
                }
            }

            return max;
        }

        /// <summary>
        /// Parse the input to a binary tree representing the number
        /// </summary>
        /// <param name="input">Input line</param>
        /// <returns>Root node of the tree</returns>
        public static Node Parse(string input)
        {
            static Node ParseInternal(Node parent, string input, ref int cursor)
            {
                if (input[cursor] != '[')
                {
                    return new Node { Parent = parent, Value = int.Parse(input[cursor++].ToString()) };
                }

                Node node = new Node { Parent = parent };
                cursor++; // skip open bracket

                node.Left = ParseInternal(node, input, ref cursor);
                cursor++; // skip comma

                node.Right = ParseInternal(node, input, ref cursor);
                cursor++; // skip close bracket

                return node;
            }

            int cursor = 0;
            return ParseInternal(null, input, ref cursor);
        }

        /// <summary>
        /// Add 2 number nodes together
        /// </summary>
        /// <param name="left">Left node</param>
        /// <param name="right">Right node</param>
        /// <returns>New root node</returns>
        public static Node Add(Node left, Node right)
        {
            var root = new Node { Left = left, Right = right };
            left.Parent = root;
            right.Parent = root;

            /*StringBuilder sb = new StringBuilder();
            root.Debug(sb);
            Debug.WriteLine($"after addition: {sb}");*/

            Reduce(root);

            return root;
        }

        /// <summary>
        /// Keep applying explodes and splits until no more are required
        /// </summary>
        /// <param name="root">Big root number</param>
        private static void Reduce(Node root)
        {
            while (true)
            {
                if (Explode(root))
                {
                    /*StringBuilder sb = new StringBuilder();
                    root.Debug(sb);
                    Debug.WriteLine($"after explode:  {sb}");*/

                    continue;
                }

                if (Split(root))
                {
                    /*StringBuilder sb = new StringBuilder();
                    root.Debug(sb);
                    Debug.WriteLine($"after split:    {sb}");*/

                    continue;
                }

                break;
            }
        }

        /// <summary>
        /// Explode the first pair at depth >= 4
        /// </summary>
        private static bool Explode(Node root)
        {
            // flatten down an array so we can easily look left and right
            var leaves = Flatten(root);

            for (int i = 0; i < leaves.Count - 1; i++)
            {
                (Node left, int leftDepth) = leaves[i];
                (Node right, int rightDepth) = leaves[i + 1];

                if (leftDepth == rightDepth && leftDepth >= 4)
                {
                    if (i > 0)
                    {
                        leaves[i - 1].node.Value += left.Value;
                    }

                    if (i < leaves.Count - 2)
                    {
                        leaves[i + 2].node.Value += right.Value;
                    }

                    // reset exploded to literal 0
                    left.Parent.Value = 0;
                    left.Parent.Left = null;
                    left.Parent.Right = null;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Flatten a tree down to just the leaf nodes along with their depth
        /// </summary>
        private static IList<(Node node, int depth)> Flatten(Node root)
        {
            static void FlattenInner(Node node, IList<(Node node, int depth)> list, int depth)
            {
                if (node.IsLeaf)
                {
                    list.Add((node, depth));
                    return;
                }

                FlattenInner(node.Left, list, depth + 1);
                FlattenInner(node.Right, list, depth + 1);
            }

            var list = new List<(Node node, int depth)>();
            FlattenInner(root, list, -1);
            return list;
        }

        /// <summary>
        /// Find any single values greater than 10 and replace them with pairs
        /// </summary>
        /// <param name="current">Current fish</param>
        /// <returns>Whether any splits occurred</returns>
        public static bool Split(Node current)
        {
            if (!current.IsLeaf)
            {
                // branch node - recurse and stop on first successful split
                return Split(current.Left) || Split(current.Right);
            }

            // leaf node
            if (current.Value < 10)
            {
                return false;
            }

            int leftValue = (int)Math.Floor(current.Value.Value / 2.0);
            int rightValue = (int)Math.Ceiling(current.Value.Value / 2.0);

            current.Value = null;
            current.Left = new Node { Parent = current, Value = leftValue };
            current.Right = new Node { Parent = current, Value = rightValue };

            return true;
        }

        /// <summary>
        /// Work out the recursive magnitude of the given node
        /// </summary>
        private static long Magnitude(Node node)
        {
            if (node.Value.HasValue)
            {
                return (long)node.Value;
            }

            return 3 * Magnitude(node.Left) + 2 * Magnitude(node.Right);
        }

        public class Node
        {
            public Node Parent { get; set; }

            public Node Left { get; set; }

            public Node Right { get; set; }

            public int? Value { get; set; }

            public bool IsLeaf => this.Value.HasValue;

            /// <summary>Returns a string that represents the current object.</summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                if (this.IsLeaf)
                {
                    return this.Value.Value.ToString();
                }
                else
                {
                    return "Branch";
                }
            }

            public void Debug(StringBuilder sb)
            {
                if (this.IsLeaf)
                {
                    sb.Append(this.Value.ToString());
                    return;
                }

                sb.Append('[');
                this.Left.Debug(sb);
                sb.Append(',');
                this.Right.Debug(sb);
                sb.Append(']');
            }
        }
    }
}
