using System;
using System.Collections.Generic;
using System.Linq;

namespace prosy
{
    public class ComplexMatrix : Matrix<Complex>
    {
        private ComplexMatrix m_TriangleMatrix;

        public ComplexMatrix(int rows, int columns) : base(rows, columns)
        {
            /*for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    m_Matrix[i, j] = new Complex();
                }
            }*/
        }

        /// <summary>
        /// Эрмитово сопряжение
        /// </summary>
        /// <param name="M"></param>
        /// <returns></returns>
        public static ComplexMatrix operator ~(ComplexMatrix M)
        {
            ComplexMatrix ret = new ComplexMatrix(M.Columns, M.Rows);
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = ~M[j, i];
                }
            }
            return ret;
        }

        /// <summary>
        /// Матрица приведенная к нижнетреугольному виду
        /// </summary>
        public ComplexMatrix DownTriangleMatrix
        {
            get
            {
                if (Rows != Columns)
                    throw new Exception("Matrix must be square");
                if (m_TriangleMatrix == null)
                {
                    m_TriangleMatrix = new ComplexMatrix(Rows, Rows);
                    for (int i = 0; i < Rows; i++)
                    {
                        Complex sum;
                        for (int j = 0; j < i; j++)
                        {
                            sum = new Complex();
                            for (int k = 0; k < j; k++)
                                sum += m_TriangleMatrix[i, k] * ~m_TriangleMatrix[j, k];
                            m_TriangleMatrix[i, j] = (m_Matrix[i, j] - sum) / m_TriangleMatrix[j, j];
                        }
                        sum = new Complex();
                        for (int k = 0; k < i; k++)
                            sum += m_TriangleMatrix[i, k] * ~m_TriangleMatrix[i, k];
                        m_TriangleMatrix[i, i] = Complex.Sqrt(m_Matrix[i, i] - sum);
                    }
                }
                return m_TriangleMatrix;
            }
        }

        /// <summary>
        /// Решение эрмитовой системы линейных уравнений по методу Холецкого
        /// </summary>
        /// <param name="B">известный вектор</param>
        /// <param name="X">решение</param>
        public void Holetsky(Complex[] B, out Complex[] X)
        {
            if (this.Rows != this.Columns)
                throw new Exception("Matrix must be square");
            if (this.Rows != B.Length)
                throw new Exception("Size of matrix A must be equals to size vector B");
            int len = B.Length;
            ComplexMatrix Triangle = this.DownTriangleMatrix;
            Complex[] Y = new Complex[len];
            X = new Complex[len];
            //Solve(Triangle, B, out Y, true);
            for (int i = 0; i < len; i++)
            {
                Complex sum = new Complex();
                for (int j = 0; j < i; j++)
                    sum += Triangle[i, j] * Y[j];
                Y[i] = (B[i] - sum) / Triangle[i, i];
            }
            //Solve(~Triangle, Y, out X, false);
            Triangle = ~Triangle;
            for (int i = Triangle.Rows - 1; i >= 0; i--)
            {
                Complex sum = new Complex();
                for (int j = Triangle.Rows - 1; j > i; j--)
                    sum += Triangle[i, j] * X[j];
                X[i] = (Y[i] - sum) / Triangle[i, i];
            }
        }

        /// <summary>
        /// Решение системы комплексных линейных уравнений T*X=Z
        /// </summary>
        /// <param name="Z">Известный вектор</param>
        /// <param name="X">Решение</param>
        public void Levinson(Complex[] Z, out Complex[] X)
        {
            if (this.Rows != this.Columns)
                throw new Exception("Matrix must be square");
            if (this.Rows != Z.Length)
                throw new Exception("Size of matrix A must be equals to size vector B");
            int len = Z.Length;
            X = new Complex[len];
        }
    }
}
