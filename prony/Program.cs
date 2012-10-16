using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prony;

namespace GetPronyKoeff
{
    class Program
    {
        static void Main(string[] args)
        {
            /*ComplexMatrix A = new ComplexMatrix(3, 3);
            Complex[] B = new Complex[] { new Complex(1.0, 3.0), new Complex(2.0, -1.0), new Complex(0.5, 0.8) };
            Complex[] X;

            A[0, 0] = new Complex(2, 0);
            A[0, 1] = new Complex(0.5, -0.5);
            A[0, 2] = new Complex(-0.2, 0.1);
            A[1, 0] = new Complex(0.5, 0.5);
            A[1, 1] = new Complex(1, 0);
            A[1, 2] = new Complex(0.3, -0.2);
            A[2, 0] = new Complex(-0.2, -0.1);
            A[2, 1] = new Complex(0.3, 0.2);
            A[2, 2] = new Complex(0.5, 0);

            A.Holetsky(B, out X);*/

            alglib.complex[,] A = new alglib.complex[3, 3];
            A[0, 0] = new alglib.complex(2, 0);
            A[0, 1] = new alglib.complex(0.5, -0.5);
            A[0, 2] = new alglib.complex(-0.2, 0.1);
            A[1, 0] = new alglib.complex(0.5, 0.5);
            A[1, 1] = new alglib.complex(1, 0);
            A[1, 2] = new alglib.complex(0.3, -0.2);
            A[2, 0] = new alglib.complex(-0.2, -0.1);
            A[2, 1] = new alglib.complex(0.3, 0.2);
            A[2, 2] = new alglib.complex(0.5, 0);

            alglib.complex[] B = new alglib.complex[] { new alglib.complex(1.0, 3.0),
                new alglib.complex(2.0, -1.0), new alglib.complex(0.5, 0.8) };

            alglib.complex[] X;
            int info = 0;
            alglib.densesolverreport rep;
            alglib.hpdmatrixcholeskysolve(A, 3, false, B, out info, out rep, out X);
        }
    }
}
