using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 22
    /// </summary>
    public class Day22
    {
        public long Part1(string[] input) => Execute(input, Part.One);

        public long Part2(string[] input) => Execute(input, Part.Two);

        /// <summary>
        /// Execute all of the rules and return the total number of cubes left switched on
        /// </summary>
        /// <param name="input">Input instructions</param>
        /// <param name="part">Part to execute</param>
        /// <returns>Total number of cubes switched on</returns>
        private static long Execute(string[] input, Part part)
        {
            IEnumerable<Cuboid> shapes = Array.Empty<Cuboid>();

            foreach (string line in input)
            {
                var numbers = line.Numbers<int>();
                bool on = line.StartsWith("on");

                if (part == Part.One && Math.Abs(numbers[0]) > 50)
                {
                    // part 1 stops once it hits an instruction with a value > 50 from origin
                    break;
                }

                var cuboid = new Cuboid
                {
                    MinX = Math.Min(numbers[0], numbers[1]),
                    MaxX = Math.Max(numbers[0], numbers[1]),
                    MinY = Math.Min(numbers[2], numbers[3]),
                    MaxY = Math.Max(numbers[2], numbers[3]),
                    MinZ = Math.Min(numbers[4], numbers[5]),
                    MaxZ = Math.Max(numbers[4], numbers[5])
                };

                // check if this intersects any other shapes and split them into regular shapes
                shapes = shapes.SelectMany(s => s.Split(cuboid));

                if (on)
                {
                    shapes = shapes.Append(cuboid);
                }
            }

            return shapes.Sum(s => s.Volume);
        }

        public class Cuboid
        {
            public int MinX { get; init; }
            public int MaxX { get; init; }
            public int MinY { get; init; }
            public int MaxY { get; init; }
            public int MinZ { get; init; }
            public int MaxZ { get; init; }

            public long Volume => (long)(this.MaxX - this.MinX + 1) * (this.MaxY - this.MinY + 1) * (this.MaxZ - this.MinZ + 1);

            public Cuboid()
            {
            }

            public Cuboid(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
            {
                this.MinX = minX;
                this.MaxX = maxX;
                this.MinY = minY;
                this.MaxY = maxY;
                this.MinZ = minZ;
                this.MaxZ = maxZ;
            }

            /// <summary>
            /// Does this cuboid intersection with the other one?
            /// </summary>
            /// <param name="other">Other shape to check</param>
            /// <returns>If the two shapes intersect</returns>
            public bool Intersects(Cuboid other)
            {
                bool x = this.MinX <= other.MaxX && other.MinX <= this.MaxX;
                bool y = this.MinY <= other.MaxY && other.MinY <= this.MaxY;
                bool z = this.MinZ <= other.MaxZ && other.MinZ <= this.MaxZ;

                return x && y && z;
            }

            /// <summary>
            /// Check if this cuboid needs splitting into sub-shapes because it intersects with the rule
            /// </summary>
            /// <param name="rule">Rule to check against</param>
            /// <returns>Sub-shapes (which might just be this shape if it didn't overlap)</returns>
            public IEnumerable<Cuboid> Split(Cuboid rule)
            {
                if (!this.Intersects(rule))
                {
                    // they don't overlap, so just keep using this shape
                    yield return this;
                    yield break;
                }

                // intersection range
                int left = Math.Max(this.MinX, rule.MinX);
                int right = Math.Min(this.MaxX, rule.MaxX);
                int bottom = Math.Max(this.MinY, rule.MinY);
                int top = Math.Min(this.MaxY, rule.MaxY);

                if (rule.MinX > this.MinX)
                {
                    // left
                    yield return new Cuboid(this.MinX, rule.MinX - 1, this.MinY, this.MaxY, this.MinZ, this.MaxZ);
                }

                if (rule.MaxX < this.MaxX)
                {
                    // right
                    yield return new Cuboid(rule.MaxX + 1, this.MaxX, this.MinY, this.MaxY, this.MinZ, this.MaxZ);
                }

                if (rule.MinY > this.MinY)
                {
                    // bottom
                    yield return new Cuboid(left, right, this.MinY, rule.MinY - 1, this.MinZ, this.MaxZ);
                }

                if (rule.MaxY < this.MaxY)
                {
                    // top
                    yield return new Cuboid(left, right, rule.MaxY + 1, this.MaxY, this.MinZ, this.MaxZ);
                }

                if (rule.MinZ > this.MinZ)
                {
                    // front
                    yield return new Cuboid(left, right, bottom, top, this.MinZ, rule.MinZ - 1);
                }

                if (rule.MaxZ < this.MaxZ)
                {
                    // back
                    yield return new Cuboid(left, right, bottom, top, rule.MaxZ + 1, this.MaxZ);
                }
            }
        }
    }
}
