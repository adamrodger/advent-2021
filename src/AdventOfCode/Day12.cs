using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 12
    /// </summary>
    public class Day12
    {
        public int Part1(string[] input)
        {
            IDictionary<string, ICollection<string>> vertices = ParseVertices(input);
            return CountPathsFromNode("start", vertices, new HashSet<string>(), string.Empty, Part.One);
        }

        public int Part2(string[] input)
        {
            IDictionary<string, ICollection<string>> vertices = ParseVertices(input);
            return CountPathsFromNode("start", vertices, new HashSet<string>(), string.Empty, Part.Two);
        }

        /// <summary>
        /// Parse the input to a collection of two-way vertices between nodes
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>Vertices</returns>
        private static IDictionary<string, ICollection<string>> ParseVertices(IEnumerable<string> input)
        {
            IDictionary<string, ICollection<string>> vertices = new Dictionary<string, ICollection<string>>();

            foreach (string line in input)
            {
                var nodes = line.Split('-');
                vertices.GetOrCreate(nodes[0], () => new List<string>()).Add(nodes[1]);
                vertices.GetOrCreate(nodes[1], () => new List<string>()).Add(nodes[0]);
            }

            return vertices;
        }

        /// <summary>
        /// Counts the number of valid paths from the given node to the end node
        /// </summary>
        /// <param name="node">Current node</param>
        /// <param name="vertices">Valid vertices</param>
        /// <param name="visited">Currently-visited nodes on this path</param>
        /// <param name="twice">Which node has been visited twice</param>
        /// <param name="part">Which part of the problem are we solving</param>
        /// <returns>Total number of valid paths</returns>
        private static int CountPathsFromNode(string node, IDictionary<string, ICollection<string>> vertices, ISet<string> visited, string twice, Part part)
        {
            if (node == "end")
            {
                return 1;
            }

            if (node == "start" && visited.Any())
            {
                return 0;
            }

            if (node.ToLower() == node && visited.Contains(node))
            {
                if (part == Part.One || !string.IsNullOrWhiteSpace(twice))
                {
                    // invalid path:
                    //     part 1 - not allowed to double-visit
                    //     part 2 - already used our double-visit
                    return 0;
                }

                // this node is our double-visit in this path
                twice = node;
            }

            int paths = 0;
            visited.Add(node);

            foreach (string neighbour in vertices[node])
            {
                paths += CountPathsFromNode(neighbour, vertices, visited.ToHashSet(), twice, part); // got to clone visited
            }

            return paths;
        }
    }
}
