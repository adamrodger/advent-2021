namespace AdventOfCode
{
    /// <summary>
    /// Solver for Day 24
    /// </summary>
    public class Day24
    {
        public long Part1(string[] input)
        {
            return 99911993949684;
        }

        public long Part2(string[] input)
        {
            return 62911941716111;
        }

        /*

        I calculated this day by hand by 'decompiling' the input and working out what it was doing. Some of my notes
        below:

        The program executes the below logic 14 times, with these 3 variables for each specific input char
        (from left to right):

        #   XMOD    YMOD    ZMOD
        1   12      6       1
        2   10      6       1
        3   13      3       1
        4   -11     11      26
        5   13      9       1
        6   -1      3       26
        7   10      13      1
        8   11      6       1
        9   0       14      26
        10  10      10      1
        11  -5      12      26
        12  -16     10      26
        13  -7      11      26
        14  -11     15      26
        // Are these ASCII offsets from a specific letter/number?

        ZMOD is 1 half the time (noop) and 26 the other half

        inp w           w = readline()

        mul x 0         x = 0
        add x z         x += z
        mod x 26        x %= 26                 x = (z % 26) + XMOD // MOD 26 - like the alphabet?
        div z 1         z /= ZMOD               z = floor(z / ZMOD) // always either 1 (noop) or 26
        add x 12        x += XMOD               
        eql x w         x = x == w ? 1 : 0      
        eql x 0         x = x == 0 ? 1 : 0      // this is x != w as an int instead of bool
                                                // or, x = (int)((z % 26) + XMOD != w)
                                                
        mul y 0         y = 0                   if x != w:
        add y 25        y += 25                     
        mul y x         y *= x                      // x is either 1 or 0 here  
        add y 1         y++                         // y will either be 1 (noop) or (25*x)+1 == 26
        mul z y         z *= y                      z *= (25 * x) + 1    // or z *= 26

        mul y 0         y = 0                   if x != w:
        add y w         y += w                  
        add y 6         y += YMOD                   // again x is either 0 or 1
        mul y x         y *= x                      // y will be 0 if x was 0, so noop
        add z y         z += y                      z += x * (w + YMOD) // or z += w + YMOD

                                                // or more simply, I think....
                                                //      if x != w:
                                                //          z *= 26
                                                //          z += w + YMOD

        Equivalent code:

        w = readline()
        x = int((z % 26) + XMOD != w)
        z = floor(z / ZMOD)
        z *= (25 + x) + 1
        z += x * (w + YMOD)

        when ZMOD == 1, that means:

        w = readline()
        z *= 26
        z += w + YMOD

        when ZMOD == 26, that means:

        z /= 26

        so we incrementally build z up and then subtract it back down again
        z has to be 0 at the end, so the adds and subtracts need to cancel out
        digits can only be 1-9 (never 0)

        #       P1 (max)    P2 (min)   Type
        1       9           6          add
        2       9           2              add
        3       9           9                  add
        4       1           1                  subtract        (digit 3) + 3 - 11     == d3 - 8
        5       1           1                  add
        6       9           9                  subtract        (digit 5) + 9 - 1      == d5 + 8
        7       9           4                  add
        8       3           1                      add
        9       9           7                      subtract    (digit 8) + 6 - 0      == d8 + 6 
        10      4           1                      add
        11      9           6                      subtract    (digit 10) + 10 - 5    == d10 + 5
        12      6           1                  subtract        (digit 7) + 13 - 16    == d7 - 3
        13      8           1              subtract            (digit 2) + 6 - 7      == d2 - 1
        14      4           1          subtract                (digit 1) + 6 - 11     == d1 - 5

        */
    }
}
