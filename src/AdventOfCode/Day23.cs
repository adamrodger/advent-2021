using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using AdventOfCode.Utilities;

namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 23
    /// </summary>
    public class Day23
    {
        public int Part1(string[] _)
        {
            /*

            #############
            #...........#
            ###D#C#A#B###
              #C#D#A#B#
              #########

             */

            State start = new(".......", "DC", "CD", "AA", "BB");
            State target = new(".......", "AA", "BB", "CC", "DD");

            return ShortestPath(start, target);

            //return 17400; // did it by hand
        }

        public int Part2(string[] _)
        {
            /*

            #############
            #...........#
            ###D#C#A#B###
              #D#C#B#A#
              #D#B#A#C#
              #C#D#A#B#
              #########

            */

            State start  = new(".......", "DDDC", "CCBD", "ABAA", "BACB");
            State target = new(".......", "AAAA", "BBBB", "CCCC", "DDDD");

            return ShortestPath(start, target);

            //return 46120; // did it by hand
        }

        /// <summary>
        /// Find the shortest path from the start to the target state
        /// </summary>
        private static int ShortestPath(State start, State target)
        {
            var parents = new Dictionary<State, State>();
            var distances = new Dictionary<State, int> { [start] = 0 };
            var open = new PriorityQueue<State, int>();
            open.Enqueue(start, 0);
            var closed = new HashSet<State>();

            while (open.Count > 0)
            {
                State current = open.Dequeue();
                closed.Add(current);

                if (current == target)
                {
                    if (Debugger.IsAttached)
                    {
                        var path = new List<State>();

                        while (parents.ContainsKey(current))
                        {
                            path.Add(current);
                            current = parents[current];
                        }

                        path.Reverse();

                        foreach (State state in path)
                        {
                            Debug.Write(state.Print());
                            Debug.WriteLine($"Cost: {distances[state]}\n");
                        }
                    }

                    return distances[target];
                }

                foreach ((State next, int cost) in current.ValidMoves())
                {
                    if (closed.Contains(next))
                    {
                        continue;
                    }

                    var newDistance = distances[current] + cost;

                    // found the first or a closer route to the adjacent node (from the current node)
                    if (!distances.ContainsKey(next) || newDistance < distances[next])
                    {
                        parents[next] = current;
                        distances[next] = newDistance;
                        open.Enqueue(next, newDistance);
                    }
                }
            }

            throw new InvalidOperationException("No path found");
        }

        /// <summary>
        /// Game state
        /// </summary>
        /// <param name="Waiting">All 7 positions of the waiting area, as a string</param>
        /// <param name="Room1">Room 1 string from top to bottom</param>
        /// <param name="Room2">Room 2 string from top to bottom</param>
        /// <param name="Room3">Room 3 string from top to bottom</param>
        /// <param name="Room4">Room 4 string from top to bottom</param>
        private record State(string Waiting, string Room1, string Room2, string Room3, string Room4)
        {
            /// <summary>
            /// Cost to move each piece a single place
            /// </summary>
            private static readonly Dictionary<char, int> CostMultipliers = new()
            {
                ['A'] = 1,
                ['B'] = 10,
                ['C'] = 100,
                ['D'] = 1000
            };

            /// <summary>
            /// Get all the valid moves from the current game state
            /// </summary>
            /// <returns>All valid next moves and the additional cost to get to them</returns>
            public IEnumerable<(State next, int cost)> ValidMoves()
            {
                // try and place anything that's waiting
                foreach ((State next, int cost) pair in this.MoveWaiting())
                {
                    yield return pair;
                }

                // try and move out anything that's ready to move
                foreach ((State next, int cost) pair in this.MoveTopOfRooms())
                {
                    yield return pair;
                }
            }

            /// <summary>
            /// Move the value from the top of each room to each possible destination
            /// </summary>
            /// <returns>Every possible next move</returns>
            private IEnumerable<(State next, int cost)> MoveTopOfRooms()
            {
                // try to move the top of each room
                foreach ((int roomId, string room) in new[] { this.Room1, this.Room2, this.Room3, this.Room4 }.Enumerate())
                {
                    (int i, char top) = room.Enumerate().FirstOrDefault(pair => pair.item != '.');

                    if (top == default)
                    {
                        // room was empty
                        continue;
                    }

                    if ((roomId == 0 && top == 'A')
                     || (roomId == 1 && top == 'B')
                     || (roomId == 2 && top == 'C')
                     || (roomId == 3 && top == 'D'))
                    {
                        // if it's already in the correct place, leave it
                        continue;
                    }

                    string modifiedRoom = ReplaceChar(room, '.', i);

                    (string one, string two, string three, string four) = roomId switch
                    {
                        0 => (modifiedRoom, this.Room2, this.Room3, this.Room4),
                        1 => (this.Room1, modifiedRoom, this.Room3, this.Room4),
                        2 => (this.Room1, this.Room2, modifiedRoom, this.Room4),
                        3 => (this.Room1, this.Room2, this.Room3, modifiedRoom),
                        _ => throw new ArgumentOutOfRangeException(nameof(roomId), roomId, "Invalid room ID")
                    };

                    // cost to get into the corridor
                    int roomCost = i + 1;

                    // try and move to every possible free waiting spot
                    for (int j = 0; j < this.Waiting.Length; j++)
                    {
                        if (this.Waiting[j] != '.')
                        {
                            // not available
                            continue;
                        }

                        if (this.IsBlocked(roomId, j))
                        {
                            // something is in the way, so we can't make that move
                            continue;
                        }

                        string modifiedWaiting = ReplaceChar(this.Waiting, top, j);

                        int corridorCost = CorridorMoves(roomId, j);
                        int cost = CostMultipliers[top] * (roomCost + corridorCost);

                        yield return (new State(modifiedWaiting, one, two, three, four), cost);
                    }
                }
            }

            /// <summary>
            /// Move each waiting element to its target room, if possible
            /// </summary>
            /// <returns>Every possible next move</returns>
            private IEnumerable<(State next, int cost)> MoveWaiting()
            {
                foreach ((int i, char w) in this.Waiting.Enumerate())
                {
                    if (w == '.')
                    {
                        // nothing waiting here
                        continue;
                    }

                    (int roomId, string target) = w switch
                    {
                        'A' => (0, this.Room1),
                        'B' => (1, this.Room2),
                        'C' => (2, this.Room3),
                        'D' => (3, this.Room4),
                        _ => throw new ArgumentOutOfRangeException(nameof(w), w, "Invalid ID")
                    };

                    if (!target.All(t => t == '.' || t == w))
                    {
                        // there's either no space or a different type already in there
                        continue;
                    }

                    if (this.IsBlocked(roomId, i, true))
                    {
                        continue;
                    }

                    int openSpace = target.LastIndexOf('.');
                    string modifiedRoom = ReplaceChar(target, w, openSpace);
                    string modifiedWaiting = ReplaceChar(this.Waiting, '.', i);

                    (string one, string two, string three, string four) = roomId switch
                    {
                        0 => (modifiedRoom, this.Room2, this.Room3, this.Room4),
                        1 => (this.Room1, modifiedRoom, this.Room3, this.Room4),
                        2 => (this.Room1, this.Room2, modifiedRoom, this.Room4),
                        3 => (this.Room1, this.Room2, this.Room3, modifiedRoom),
                        _ => throw new ArgumentOutOfRangeException(nameof(roomId), roomId, "Invalid room ID")
                    };

                    int roomCost = openSpace + 1; // cost to get into the corridor
                    int corridorCost = CorridorMoves(roomId, i);
                    int cost = CostMultipliers[w] * (roomCost + corridorCost);

                    yield return (new State(modifiedWaiting, one, two, three, four), cost);
                }
            }

            /// <summary>
            /// Check if anything is blocking the path from the given root to the given waiting position
            /// </summary>
            private bool IsBlocked(int roomId, int waitingPosition, bool excludeWaitingPosition = false)
            {
                string corridor = (roomId, waitingPosition) switch
                {
                    (0, 0) => this.Waiting[0..2],
                    (0, 1) => this.Waiting[1..2],
                    (0, 2) => this.Waiting[2..3],
                    (0, 3) => this.Waiting[2..4],
                    (0, 4) => this.Waiting[2..5],
                    (0, 5) => this.Waiting[2..6],
                    (0, 6) => this.Waiting[2..7],

                    (1, 0) => this.Waiting[0..3],
                    (1, 1) => this.Waiting[1..3],
                    (1, 2) => this.Waiting[2..3],
                    (1, 3) => this.Waiting[3..4],
                    (1, 4) => this.Waiting[3..5],
                    (1, 5) => this.Waiting[3..6],
                    (1, 6) => this.Waiting[3..7],

                    (2, 0) => this.Waiting[0..4],
                    (2, 1) => this.Waiting[1..4],
                    (2, 2) => this.Waiting[2..4],
                    (2, 3) => this.Waiting[3..4],
                    (2, 4) => this.Waiting[4..5],
                    (2, 5) => this.Waiting[4..6],
                    (2, 6) => this.Waiting[4..7],

                    (3, 0) => this.Waiting[0..5],
                    (3, 1) => this.Waiting[1..5],
                    (3, 2) => this.Waiting[2..5],
                    (3, 3) => this.Waiting[3..5],
                    (3, 4) => this.Waiting[4..5],
                    (3, 5) => this.Waiting[5..6],
                    (3, 6) => this.Waiting[5..7],

                    _ => throw new ArgumentOutOfRangeException()
                };

                if (excludeWaitingPosition)
                {
                    if (waitingPosition < roomId + 2)
                    {
                        // moving from the left
                        corridor = corridor[1..];
                    }
                    else
                    {
                        corridor = corridor[..^1];
                    }
                }

                return corridor.Any(c => c != '.');
            }

            /// <summary>
            /// Number of moves to travel along the corridor from above the given room to the given waiting position
            /// </summary>
            private static int CorridorMoves(int roomId, int waitingPosition)
            {
                // fuck it, my brain hurts doing the maths for this
                return (roomId, waitingPosition) switch
                {
                    (0, 0) => 2,
                    (0, 1) => 1,
                    (0, 2) => 1,
                    (0, 3) => 3,
                    (0, 4) => 5,
                    (0, 5) => 7,
                    (0, 6) => 8,

                    (1, 0) => 4,
                    (1, 1) => 3,
                    (1, 2) => 1,
                    (1, 3) => 1,
                    (1, 4) => 3,
                    (1, 5) => 5,
                    (1, 6) => 6,

                    (2, 0) => 6,
                    (2, 1) => 5,
                    (2, 2) => 3,
                    (2, 3) => 1,
                    (2, 4) => 1,
                    (2, 5) => 3,
                    (2, 6) => 4,

                    (3, 0) => 8,
                    (3, 1) => 7,
                    (3, 2) => 5,
                    (3, 3) => 3,
                    (3, 4) => 1,
                    (3, 5) => 1,
                    (3, 6) => 2,

                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            /// <summary>
            /// Produce debug output
            /// </summary>
            public string Print()
            {
                var sb = new StringBuilder();

                sb.AppendLine("#############");
                sb.AppendLine($"#{this.Waiting[..2]}.{this.Waiting[2]}.{this.Waiting[3]}.{this.Waiting[4]}.{this.Waiting[5..]}#");
                sb.AppendLine($"###{this.Room1[0]}#{this.Room2[0]}#{this.Room3[0]}#{this.Room4[0]}###");
                sb.AppendLine($"  #{this.Room1[1]}#{this.Room2[1]}#{this.Room3[1]}#{this.Room4[1]}#  ");

                if (this.Room1.Length > 2)
                {
                    sb.AppendLine($"  #{this.Room1[2]}#{this.Room2[2]}#{this.Room3[2]}#{this.Room4[2]}#  ");
                    sb.AppendLine($"  #{this.Room1[3]}#{this.Room2[3]}#{this.Room3[3]}#{this.Room4[3]}#  ");
                }

                sb.AppendLine("  #########  ");

                return sb.ToString();
            }
        }

        /// <summary>
        /// Create a new string from the input but with the given char at the given index
        /// </summary>
        private static string ReplaceChar(string input, char replace, int index)
        {
            return new StringBuilder(input) { [index] = replace }.ToString();
        }
    }
}
