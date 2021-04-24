using System;

namespace PathfindingTutorial.Data_Structures.Matrix
{
    public partial class Matrix
    {
        /// <summary>
        /// Get the identity matrix of the specific dimension
        /// </summary>
        /// <param name="dim"></param>
        /// <returns></returns>
        public static Matrix GetIdentityMatrix(int dim)
        {
            var I = new Matrix(dim, dim);
            for (int i = 0; i < dim; i++)
                I[i, i] = 1;
            return I;
        }

        /// <summary>
        /// Adds two matrices and will produce a matrix of the largest dimension possible.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Matrix operator +(Matrix left, Matrix right)
        {
            int minRows = Math.Min(left.Rows, right.Rows);
            int minCols = Math.Min(left.Cols, right.Cols);

            var m = new Matrix(minRows, minCols);

            for (int i = 0; i < minRows; i++)
                for (int j = 0; j < minCols; j++)
                    m[i, j] = left[i, j] + right[i, j];

            return m;
        }

        public static Matrix operator -(Matrix left, Matrix right)
        {
            int minRows = Math.Min(left.Rows, right.Rows);
            int minCols = Math.Min(left.Cols, right.Cols);

            var m = new Matrix(minRows, minCols);

            for (int i = 0; i < minRows; i++)
                for (int j = 0; j < minCols; j++)
                    m[i, j] = left[i, j] - right[i, j];

            return m;
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            int n = left.Rows;
            int m = left.Cols;
            int p = right.Cols;

            if (m != right.Rows)
                throw new Exception(
                    string.Format("Cannot multiply a {0}x{1} matrix by a {2}x{3} matrix", n, m, right.Rows, p)
                );

            var mult = new Matrix(n, p);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < p; j++)
                    for (int k = 0; k < m; k++)
                        mult[i, j] += left[i, k] * right[k, j];

            return mult;
        }

        public static Matrix operator *(int left, Matrix right)
        {
            int n = right.Rows;
            int m = right.Cols;

            var mult = new Matrix(n, m);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    mult[i, j] = left * right[i, j];

            return mult;
        }

    }
}
