using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuValidator
{
    /// <summary>
    /// This class takes as argument an int[][] array and validates if it is filled properly.
    /// Pass the array to the constructor and check its validity by the IsValid property.
    /// </summary>
    public class SudokuValidator
    {
        /// <summary>
        /// Validates if the Sudoku is filled correct.
        /// </summary>
        /// <param name="SudokuArray">The Sudoku array</param>
        public SudokuValidator(int[][] SudokuArray)
        {
            this.SudokuArray = SudokuArray;
            ValidateSudokuInput();
        }

        /// <summary>
        /// The Sudoku array
        /// </summary>
        private int[][] SudokuArray;

        /// <summary>
        /// The sum of the values of a proper line (row or column [and box])
        /// </summary>
        private int LineSum = 0;

        /// <summary>
        /// The N for NxN small box size
        /// </summary>
        private int SmallBoxSize = 0;

        /// <summary>
        /// If the given array is valid the value is true
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Checks how much should be the sum of each column, row, box
        /// </summary>
        /// <param name="DimensionSize">The size of the dimension</param>
        private void GetProperSum(int DimensionSize)
        {
            LineSum = 0;

            for (int i = 1; i <= DimensionSize; i++)
                LineSum += i;
        }

        /// <summary>
        /// Given that the size has to be NxN for the array to be valid has to follow the The square of a binomial (1,2(this is not valid),4,9,etc)
        /// </summary>
        /// <returns>If true dimensions are valid</returns>
        private bool IsDimensionValid(int size)
        {
            int smallBoxSize = SquareOfBinomial(size);

            if (smallBoxSize != -1)
            {
                this.SmallBoxSize = smallBoxSize;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Recursive calculation of the power value (used for small squares and to validate if the Sudoku is the right size)
        /// </summary>
        /// <param name="size">The size of the Sudoku</param>
        /// <param name="value">Leave it blank, is used for the recursion</param>
        /// <returns></returns>
        private int SquareOfBinomial(int size, int value = 1)
        {
            double SquareValue = Math.Pow(Convert.ToDouble(value), 2.0);

            if (SquareValue == size)
            {
                return value;
            }
            else if (SquareValue < size)
            {
                return SquareOfBinomial(size, ++value);
            }
            else if (SquareValue > size)
            {
                return -1;
            }

            return value - 1;
        }

        /// <summary>
        /// The main functionality of the validator
        /// </summary>
        private void ValidateSudokuInput()
        {

            int sizeX = this.SudokuArray.Length;
            int sizeY = this.SudokuArray[1].Length;

            if (sizeX != sizeY)
            {
                IsValid = false;
                return;
            }

            if (!IsDimensionValid(sizeX))
            {
                IsValid = false;
                return;
            }

            GetProperSum(sizeX);

            int[] DistinctLineNumbers = new int[sizeX];

            //Rows
            foreach (int[] obj in SudokuArray)
            {
                DistinctLineNumbers = (from int x in obj where x >= 0 && x <= sizeX select x).Distinct().ToArray();
                if (DistinctLineNumbers.Count() != sizeX || DistinctLineNumbers.Sum() > LineSum)
                {
                    IsValid = false;
                    return;
                }
            }

            //Columns
            for (int i = 0; i < sizeX; i++)
            {
                DistinctLineNumbers = (from int[] x in SudokuArray where x[i] >= 0 && x[i] <= sizeX select x[i]).Distinct().ToArray();
                if (DistinctLineNumbers.Count() != sizeX || DistinctLineNumbers.Sum() > LineSum)
                {
                    IsValid = false;
                    return;
                }
            }

            //Small boxes
            int[][] SmallBox = new int[SmallBoxSize][];
            for (int i = 0; i < SmallBoxSize; i++)
            {
                for (int j = 0; j < SmallBoxSize; j++)
                {
                    int rangeX = i * SmallBoxSize;
                    int rangeY = j * SmallBoxSize;

                    SmallBox = SudokuArray.ToList().GetRange(rangeX, SmallBoxSize).ToArray();

                    List<int[]> tmpList = new List<int[]>();
                    foreach (int[] obj in SmallBox)
                    {
                        tmpList.Add(obj.Skip(rangeY).Take(SmallBoxSize).ToArray());
                    }
                    SmallBox = tmpList.ToArray();

                    int sumOfSmallBox = (from int[] x in tmpList select x.Sum()).Sum();

                    if (sumOfSmallBox != LineSum)
                    {
                        IsValid = false;
                        return;
                    }
                }
            }
            IsValid = true;
        }
    }

    class Program
    {
        /// <summary>
        /// Does what it says...
        /// </summary>
        /// <param name="SudokuArray"></param>
        /// <returns>if true the sudoku table is valid</returns>
        public static bool CheckSudokuValidity(ref int[][] SudokuArray)
        {
            SudokuValidator Validator = new SudokuValidator(SudokuArray);

            return Validator.IsValid;
        }

        static void Main(string[] args)
        {
            int[][] goodSudoku1 = {
                new int[] {7,8,4,  1,5,9,  3,2,6},
                new int[] {5,3,9,  6,7,2,  8,4,1},
                new int[] {6,1,2,  4,3,8,  7,5,9},
                new int[] {9,2,8,  7,1,5,  4,6,3},
                new int[] {3,5,7,  8,4,6,  1,9,2},
                new int[] {4,6,1,  9,2,3,  5,8,7},
                new int[] {8,7,6,  3,9,4,  2,1,5},
                new int[] {2,4,3,  5,6,1,  9,7,8},
                new int[] {1,9,5,  2,8,7,  6,3,4}
            };

            int[][] badSudoku1 = {
                new int[] {7,8,4,  1,5,9,  3,2,6},
                new int[] {5,3,9,  6,7,2,  8,4,1},
                new int[] {6,1,2,  4,3,8,  7,5,9},
                new int[] {9,2,8,  7,1,5,  4,6,3},
                new int[] {3,5,7,  8,4,6,  1,9,2},
                new int[] {4,6,1,  9,2,3,  5,8,7},
                new int[] {8,7,6,  3,9,4,  2,1,5},
                new int[] {2,7,3,  5,6,1,  9,7,8},
                new int[] {1,9,5,  2,8,7,  6,3,4}
            };

            Console.WriteLine("goodSudoku1:");
            Console.WriteLine(CheckSudokuValidity(ref goodSudoku1) ? "Sudoku is valid!" : "Not valid :(");

            Console.WriteLine("badSudoku1:");
            Console.WriteLine(CheckSudokuValidity(ref badSudoku1) ? "Sudoku is valid!" : "Not valid");

            //wait
            Console.ReadLine();
        }
    }
}
