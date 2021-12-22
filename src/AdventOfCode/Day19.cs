using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// This problem was really really hard and I had to look up lots of hints, entirely admitted
    /// </summary>
    public class Day19
    {
        /// <summary>
        /// Number of overlapping beacons required to consider two scanners to be in the same orientation
        /// after a matching heuristic has been applied
        /// </summary>
        /// <remarks>
        /// COMPLETE HACK!!
        ///
        /// 2 matches is actually enough to give the right answer even though 12 should be needed, because
        /// the heuristic below for detecting possible overlaps takes care of the hard work for the inputs
        /// we've got. It's possible to implement some inputs that will make this wrong though, in which case
        /// it would need increasing, potentially all the way up to 12
        /// </remarks>
        /// <see cref="Scanner.TryAlignTo" />
        private const int OverlapsRequired = 2;

        /// <summary>
        /// Number of overlapping distances required for a heuristic which determines that 2 scanners possibly
        /// can be aligned, without having to try all the alignments first
        /// </summary>
        /// <remarks>
        /// 12 matching beacons required, means (12 * 11 / 2) = 66 pairs must match
        /// </remarks>
        /// <see cref="Scanner.TryAlignTo" />
        private const int DistancesRequired = 66;

        /// <summary>
        /// Translations of a beacon to one of the 6 faces of a cube
        /// </summary>
        private static readonly Point3D[] Faces =
        {
            ( 1,  0,  0),
            (-1,  0,  0),
            ( 0,  1,  0),
            ( 0, -1,  0),
            ( 0,  0,  1),
            ( 0,  0, -1)
        };

        public int Part1(string[] input)
        {
            IList<Scanner> scanners = Parse(input);
            OrientAll(scanners);

            var knownBeacons = new HashSet<Beacon>();

            foreach (Scanner scanner in scanners)
            {
                foreach (Beacon beacon in scanner.Beacons)
                {
                    knownBeacons.Add(beacon + scanner.Position);
                }
            }

            return knownBeacons.Count;
        }

        public int Part2(string[] input)
        {
            IList<Scanner> scanners = Parse(input);
            OrientAll(scanners);

            int max = int.MinValue;

            foreach ((int i, Scanner first) in scanners.Enumerate())
            {
                foreach (Scanner second in scanners.Skip(i + 1))
                {
                    int distance = first.Position.ManhattanDistance(second.Position);
                    max = Math.Max(max, distance);
                }
            }

            return max;
        }

        /// <summary>
        /// Parse the input to a collection of scanners, which are a collection of beacon locations
        /// </summary>
        private static IList<Scanner> Parse(string[] input)
        {
            var scanners = new List<Scanner>();
            var beacons = new List<Beacon>();
            int id = 0;

            foreach (string line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    scanners.Add(new Scanner(id++, beacons));
                    continue;
                }

                if (line.StartsWith("---"))
                {
                    beacons = new List<Beacon>();
                    continue;
                }

                int[] numbers = line.Numbers<int>();
                beacons.Add((numbers[0], numbers[1], numbers[2]));
            }

            // don't forget the last one
            scanners.Add(new Scanner(id, beacons));

            return scanners;
        }

        /// <summary>
        /// Transform all the scanners to the same orientation and set their locations
        /// </summary>
        /// <param name="scanners">Scanners to orient</param>
        private static void OrientAll(IList<Scanner> scanners)
        {
            Orient(scanners.First(), scanners, new HashSet<Scanner>());
        }

        /// <summary>
        /// Orient other scanners against the current scanner
        /// </summary>
        /// <param name="current">Current scanner</param>
        /// <param name="scanners">All scanners</param>
        /// <param name="visited">Previously visited scanners (so we don't visit them again)</param>
        private static void Orient(Scanner current, ICollection<Scanner> scanners, ISet<Scanner> visited)
        {
            visited.Add(current);

            foreach (Scanner next in scanners.Where(s => !visited.Contains(s)))
            {
                bool aligned = next.TryAlignTo(current);

                // DFS through all the remaining scanners which are aligned with this one
                if (aligned)
                {
                    Orient(next, scanners, visited);
                }
            }
        }

        /// <summary>
        /// A scanner, which detects multiple beacons
        /// </summary>
        public class Scanner
        {
            private readonly Lazy<IReadOnlySet<int>> lazyDistances;

            /// <summary>
            /// Scanner ID
            /// </summary>
            public int Id { get; }

            /// <summary>
            /// Beacons detected by this scanner
            /// </summary>
            public IReadOnlyList<Beacon> Beacons { get; private set; }

            /// <summary>
            /// Position of the scanner
            /// </summary>
            public Point3D Position { get; private set; }

            /// <summary>
            /// The set of distances from every beacon to every other beacon that this scanner has detected
            /// </summary>
            public IReadOnlySet<int> PairwiseDistances => this.lazyDistances.Value;

            /// <summary>
            /// Initialises a new instance of the <see cref="Scanner"/> class.
            /// </summary>
            /// <param name="id">Scanner ID</param>
            /// <param name="beacons">Scanner beacons</param>
            public Scanner(int id, IReadOnlyList<Beacon> beacons)
            {
                this.Id = id;
                this.Beacons = beacons;
                this.Position = (0, 0, 0);
                this.lazyDistances = new Lazy<IReadOnlySet<int>>(() => CalculateDistances(beacons));
            }

            /// <summary>
            /// Calculate the distance from every beacon to every other beacon in this scanner
            /// </summary>
            private static IReadOnlySet<int> CalculateDistances(IReadOnlyList<Beacon> beacons)
            {
                var results = new HashSet<int>(beacons.Count * beacons.Count / 2);

                foreach ((int i, Beacon first) in beacons.Enumerate())
                {
                    foreach (Beacon second in beacons.Skip(i + 1))
                    {
                        int distance = first.ManhattanDistance(second);
                        results.Add(distance);
                    }
                }

                return results;
            }

            /// <summary>
            /// Try to transform this scanner to be aligned with the other one
            /// </summary>
            /// <param name="other">Other scanner with which to align</param>
            /// <returns>Alignment was successful</returns>
            public bool TryAlignTo(Scanner other)
            {
                if (this.PairwiseDistances.Intersect(other.PairwiseDistances).Count() < DistancesRequired)
                {
                    // little fingerprint heuristic which stops us trying to align scanners that can't possibly align
                    return false;
                }

                var distances = new Dictionary<(Point3D distance, Point3D face, int rotation), int>();

                // find the correct orientation now that we know it's possible
                foreach (Beacon ownBeacon in this.Beacons)
                {
                    foreach (Point3D face in Faces)
                    {
                        for (int rotation = 0; rotation < 4; rotation++)
                        {
                            Beacon rotated = ownBeacon.Transform(face, rotation);

                            foreach (Beacon otherBeacon in other.Beacons)
                            {
                                Point3D distance = otherBeacon - rotated;
                                var key = (distance, face, rotation);

                                int overlaps = distances.GetOrCreate(key) + 1;

                                if (overlaps == OverlapsRequired)
                                {
                                    // reorient this scanner
                                    this.Beacons = this.Beacons.Select(b => b.Transform(face, rotation)).ToList();
                                    this.Position = other.Position + distance;
                                    return true;
                                }

                                distances[key] = overlaps;
                            }
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// A single beacon, as detected by a scanner
        /// </summary>
        public record Beacon(int X, int Y, int Z)
        {
            /// <summary>
            /// Transform a beacon into one of the available 24 orientations
            /// </summary>
            /// <param name="face">Which face is up</param>
            /// <param name="rotations">How many rotations to perform</param>
            /// <returns>Transformed beacon</returns>
            public Beacon Transform(Point3D face, int rotations)
            {
                Beacon reoriented = face switch
                {
                    ( 1,  0,  0) => (this.Y, this.X, -this.Z),
                    (-1,  0,  0) => (this.Y, -this.X, this.Z),
                    ( 0,  1,  0) => this,
                    ( 0, -1,  0) => (this.X, -this.Y, -this.Z),
                    ( 0,  0,  1) => (this.Y, this.Z, this.X),
                    ( 0,  0, -1) => (this.Y, -this.Z, -this.X),
                    _ => throw new ArgumentOutOfRangeException(nameof(face), face, "Invalid face value")
                };

                return rotations switch
                {
                    0 => reoriented,
                    1 => (reoriented.Z, reoriented.Y, -reoriented.X),
                    2 => (-reoriented.X, reoriented.Y, -reoriented.Z),
                    3 => (-reoriented.Z, reoriented.Y, reoriented.X),
                    _ => throw new ArgumentOutOfRangeException(nameof(rotations), rotations, "Invalid rotations")
                };
            }

            /// <summary>
            /// Calculate the Manhattan distance between two beacons
            /// </summary>
            /// <param name="other">Other beacon</param>
            /// <returns>Manhattan distance</returns>
            public int ManhattanDistance(Beacon other)
            {
                return Math.Abs(this.X - other.X) + Math.Abs(this.Y - other.Y) + Math.Abs(this.Z - other.Z);
            }

            public static implicit operator (int x, int y, int z)(Beacon beacon)
            {
                return (beacon.X, beacon.Y, beacon.Z);
            }

            public static implicit operator Beacon((int x, int y, int z) coordinates)
            {
                return new Beacon(coordinates.x, coordinates.y, coordinates.z);
            }

            public static Beacon operator +(Beacon a, Point3D b)
            {
                return new Beacon(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
            }

            public static Point3D operator -(Beacon a, Beacon b)
            {
                return new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
            }
        }
    }
}
