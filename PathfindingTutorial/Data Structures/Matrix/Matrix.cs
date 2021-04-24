using System;
using System.Text;

namespace PathfindingTutorial.Data_Structures.Matrix
{
    public partial class Matrix
    {
        public int[,] MatrixArray { get; private set; }

        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public bool IsSquare => Rows == Cols;

        /// <summary>
        /// This is a 0-indexed indexer for the underlying matrix.
        /// </summary>
        /// <param name="i">row index</param>
        /// <param name="j">col index</param>
        /// <returns></returns>
        public int this[int i, int j]
        {
            get => MatrixArray[i, j];
            set => MatrixArray[i, j] = value;
        }

        public bool UpperTriangleEquals(object obj)
        {
            if (obj is Matrix mat)
            {
                if (mat.Rows != Rows || mat.Cols != Cols)
                    return false;

                for (int i = 0; i < Rows; i++)
                    for (int j = i; j < Cols; j++)
                        if (this[i, j] != mat[i, j])
                            return false;

                return true;
            }
            return base.Equals(obj);
        }

        public override bool Equals(object obj)
        {
            if (obj is Matrix mat)
            {
                if (mat.Rows != Rows || mat.Cols != Cols)
                    return false;

                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Cols; j++)
                        if (this[i, j] != mat[i, j])
                            return false;

                return true;
            }
            return base.Equals(obj);
        }

        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            MatrixArray = new int[rows, cols];
        }

        public Matrix(int[,] Input)
        {
            Rows = Input.GetLength(0);
            Cols = Input.GetLength(1);
            MatrixArray = Input;
        }

        /// <summary>
        /// Get the transpose A^T of the current matrix.
        /// </summary>
        /// <returns></returns>
        public Matrix GetTranspose()
        {
            var transpose = new Matrix(Cols, Rows);

            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    transpose[j, i] = this[i, j];
            return transpose;
        }

        /// <summary>
        /// Basic function to resize the array and copy all possible elements.
        /// </summary>
        /// <param name="new_rows"></param>
        /// <param name="new_cols"></param>
        /// <returns></returns>
        public Matrix Resize(int new_rows, int new_cols)
        {
            var resizeMatrix = new int[new_rows, new_cols];

            for (int i = 0; i < new_rows && i < Rows; i++)
                for (int j = 0; j < new_cols && j < Cols; j++)
                    resizeMatrix[i, j] = MatrixArray[i, j];

            return new Matrix(resizeMatrix);
        }

        public void Print()
        {
            //go column by column to determine number of spaces.

            string[,] str = new string[Rows, Cols];

            int[] col_len = new int[Cols];

            for (int i = 0; i < Cols; i++)
                for (int j = 0; j < Rows; j++)
                {
                    str[j, i] = MatrixArray[j, i].ToString();

                    if (str[j, i].Length > col_len[i])
                        col_len[i] = str[j, i].Length;
                }

            var sb = new StringBuilder();

            for (int i = 0; i < Rows; i++)
            {
                sb.Append('[');
                for (int j = 0; j < Cols; j++)
                {
                    int spaces = col_len[j] - str[i, j].Length;

                    for (int k = 0; k < spaces; k++)
                        sb.Append(' ');

                    sb.Append(str[i, j]);

                    if (j + 1 != Cols)
                        sb.Append(',');
                }
                sb.Append("]\n");
            }

            Console.WriteLine(sb);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
