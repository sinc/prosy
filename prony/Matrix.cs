using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prony
{
    public class Matrix<T>
    {
        protected T[,] m_Matrix;
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public T this[int Row, int Column]
        { 
            get { return m_Matrix[Row, Column]; }
            set { m_Matrix[Row, Column] = value; }
        }

        private Matrix(T[,] matrix)
        {
            m_Matrix = matrix;
        }

        public Matrix(int rows, int columns)
        {
            Rows = rows; Columns = columns;
            m_Matrix = new T[Rows, Columns];
        }
    }
}
