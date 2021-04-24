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

        public static Matrix GetMinor(Matrix mat, int row, int col)
        {
            var minor = new int[mat.Rows - 1, mat.Cols - 1];
            int auxI = 0;
            for (int i = 0; i < mat.Rows; i++)
            {
                if (i == row)
                    continue;
                int auxj = 0;
                for (int j = 0; j < mat.Cols; j++)
                {
                    if (j == col)
                        continue;
                    minor[auxI, auxj] = mat[i, j];
                    auxj++;
                }
                auxI++;
            }
            return new Matrix(minor);
        }

        public static double Determinant(Matrix mat, int expansion_row = 0)
        {
            if (mat.Rows == 2 && mat.Cols == 2)
                return mat[0, 0] * mat[1, 1] - (mat[0, 1] * mat[1, 0]);
            else if (mat.IsSquare && mat.Rows > 2)
            {
                double result = 0;
                for (int i = 0; i < mat.Cols; i++)
                {
                    double cofactor = Math.Pow(-1, expansion_row + i) * mat[expansion_row, i];
                    double expansion = cofactor * Determinant(GetMinor(mat, expansion_row, i));
                    result += expansion;
                }

                return result;

            }
            throw new Exception("Invalid matrix dimensions!");
        }

    }
}
