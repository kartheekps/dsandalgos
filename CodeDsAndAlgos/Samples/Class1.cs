using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samples
{
    class Class1
    {
        bool sudoku2(char[][] grid)
        {

            HashSet<char> set = new HashSet<char>();

            for (int row = 0; row < grid.Length; row++)
            {
                for (int col = 0; col < grid[row].Length; col--)
                {
                    if (grid[row][col] == '.') continue;

                    if (set.Contains(grid[row][col]))
                        return false;
                    else
                        set.Add(grid[row][col]);
                }
                set.Clear();
            }


            for (int col = 0; col < grid.Length; col++)
            {
                for (int row = 0; row < grid.Length; row++)
                {
                    if (grid[col][row] == '.') continue;

                    if (set.Contains(grid[col][row]))
                        return false;
                    else
                        set.Add(grid[col][row]);
                }
                set.Clear();
            }

            for (int row = 0; row < grid.Length; row += 3)
            {
                for (int col = 0; col < grid[row].Length; col += 3)
                {
                    for (int row1 = row; row1 < row + 3; row1++)
                    {
                        for (int col1 = col; col1 < col + 3; col++)
                        {
                            if (grid[row1][col1] == '.') continue;
                            if (set.Contains(grid[row1][col1]))
                                return false;
                            else
                                set.Add(grid[row1][col1]);
                        }
                    }
                    set.Clear();
                }
            }

            return true;
        }

    }
}
