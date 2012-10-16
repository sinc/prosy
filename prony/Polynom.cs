using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prony
{
    public class Polynom
    {
        /// <summary>
        /// коэффициенты полинома
        /// bn*x^n + ... + b1*x + b0 = 0
        /// </summary>
        private double[] m_coeff;
        private Complex[] m_roots;

        /// <summary>
        /// Порядок полинома
        /// </summary>
        public int P { get; private set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="K">Порядок полинома</param>
        public Polynom(int K)
        {
            P = K;
            m_B = new double[K+1];
        }

        public double value(double x)
        {
            double ret = m_coeff[0];
            for (int i = 1; i < m_coeff.Length; i++)
            {
                ret += m_coeff[i] * Math.Pow(x, (double)i);
            }
            return ret;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /*double x0 = -0.1;
            double x1 = 0.1;
            double x2 = 0.0;
            while (Math.Abs(x2-x1) > eps)
            {
                double P0 = value(x0);
                double P1 = value(x1);
                double P2 = value(x2);
                double a = ((P2 - P1) * (x1 - x0) - (P1 - P0) * (x2 - x1)) / ((x1 - x0) * (x2 - x1) * (x2 - x0));
                double b = (-a * (x2 + x1)) + ((P2 - P1) / (x2 - x1));
                double c = P2 - a * x2 * x2 - b * x2;

                x0 = x1;
                x1 = x2;
            }
        */ 
        public void kalitkin(ref Complex root, double eps)
        {
            double[] b = new double[P];
            double[] c = new double[P];
            for (int j = 0; j < P-1; j++)
            {
                //ПОРЯДОК КОЭФФИЦИЕНТОВ!!!!
                b[j] = m_coeff[j] * (P - j);
                c[j] = b[j] * (P - j - 1);
            }
            b[P - 1] = m_coeff[P - 1];
        }

        /// <summary>
        /// 
        /// </summary>
        public Complex[] roots
        {
            get
            {
                if (m_roots == null)
                {
                    m_roots = new Complex[P];
                    switch (P)
                    {
                        case 1:
                        {
                            m_roots[0].Re = -m_coeff[0] / m_coeff[1]; m_roots[0].Im = 0.0;
                            break;
                        }
                        case 2:
                        {
                            double dis = Math.Pow(m_coeff[1], 2.0) - 4.0 * m_coeff[0] * m_coeff[2];
                            if (dis < 0)
                            {
                                dis = Math.Sqrt(-dis);
                                m_roots[0].Re = -m_coeff[1] / m_coeff[2] / 2.0; m_roots[0].Im = dis / m_coeff[2] / 2.0;
                                m_roots[1].Re = m_roots[0].Re; m_roots[1].Im = -m_roots[0].Im;
                            }
                            else
                            {
                                dis = Math.Sqrt(dis);
                                m_roots[0].Re = (-m_coeff[1] + dis) / m_coeff[2] / 2.0; m_roots[0].Im = 0.0;
                                m_roots[1].Re = (-m_coeff[1] - dis) / m_coeff[2] / 2.0; m_roots[1].Im = 0.0;
                            }
                            break;
                        }
                        case 3:
                        {
                            //нормируем коэффициент при старшей степени
                            double a1 = m_coeff[2] / m_coeff[3];
                            double a2 = m_coeff[1] / m_coeff[3];
                            double a3 = m_coeff[0] / m_coeff[3];
                            double q = (Math.Pow(a1, 2.0) - 3.0 * a2) / 9.0;
                            double r = (a1 * (2.0 * Math.Pow(a1, 2.0) - 9.0 * a2) + 27.0 * a3) / 54.0;
                            double y1 = q * q * q;
                            double s = y1 - r * r;
                            y1 = Math.Sqrt(Math.Abs(y1));
                            double y2 = Math.Sqrt(Math.Abs(q));
                            if (s > 0.0)
                            {
                                double y3 = Math.Acos(r / y1) / 3.0;
                                double y4 = -2.0 * y2;
                                m_roots[0].Re = y4 * Math.Cos(y3) - a1 / 3.0; m_roots[0].Im = 0;
                                m_roots[1].Re = y4 * Math.Cos(y3 + 2.0 * Math.PI / 3.0) - a1 / 3.0; m_roots[0].Im = 0;
                                m_roots[2].Re = y4 * Math.Cos(y3 + 2.0 * Math.PI / 3.0) - a1 / 3.0; m_roots[0].Im = 0;
                            }
                            else if (s < 0.0)
                            {
                                double y4 = Math.Sign(r) * y2;
                                if (q > 0.0)
                                {
                                    double x = Math.Abs(r) / y1;
                                    double y3 = Math.Log(x + Math.Sqrt(x * x - 1.0)) / 3.0; //arccosh
                                    m_roots[0].Re = -2.0 * y4 * Math.Cosh(y3) - a1 / 3.0; m_roots[0].Im = 0.0;
                                    m_roots[1].Re = y4 * Math.Cosh(y3) - a1 / 3.0; m_roots[1].Im = Math.Sqrt(3.0) * y2 * Math.Sinh(y3);
                                    m_roots[2].Re = m_roots[1].Re; m_roots[2].Im = -m_roots[1].Im;
                                }
                                else
                                {
                                    double x = Math.Abs(r) / y1;
                                    double y3 = Math.Log(x + Math.Sqrt(x * x + 1.0)); //arcsinh
                                    m_roots[0].Re = -2.0 * y4 * Math.Sinh(y3) - a1 / 3.0; m_roots[0].Im = 0.0;
                                    m_roots[1].Re = y4 * Math.Sinh(y3) - a1 / 3.0; m_roots[1].Im = Math.Sqrt(3.0) * y2 * Math.Cosh(y3);
                                    m_roots[2].Re = m_roots[1].Re; m_roots[2].Im = -m_roots[1].Im;
                                }
                            }
                            else if (s == 0.0)
                            {
                                double y4 = Math.Sign(r) * y2;
                                m_roots[0].Re = -2.0 * y4 - a1 / 3.0; m_roots[0].Im = 0;
                                m_roots[1].Re = y4 - a1 / 3.0; m_roots[0].Im = 0;
                                m_roots[2].Re = m_roots[1].Re; m_roots[0].Im = 0;
                            }
                            break;
                        }
                        case 4:
                        {
                            //нормируем коэффициент при старшей степени
                            double a1 = m_coeff[3] / m_coeff[4];
                            double a2 = m_coeff[2] / m_coeff[4];
                            double a3 = m_coeff[1] / m_coeff[4];
                            double a4 = m_coeff[0] / m_coeff[4];
                            double s1 = -a2;
                            double s2 = a1 * a3 - 4.0 * a4;
                            double s3 = (-a1 * a1 + 4.0 * a2) * a4 - a3 * a3;
                            double q1 = (s1 * s1 - 3.0 * s2) / 9.0;
                            double q2 = (s1 * (2 * s1 * s1 - 9.0 * s2) + 27.0 * s3) / 54.0;
                            double q3 = q1 * q1 * q1;
                            double s4 = q3 - q2 * q2, s5;
                            q3 = Math.Sqrt(Math.Abs(q3));
                            double q4 = Math.Sqrt(Math.Abs(q1));
                            if (s4 > 0.0)
                            {
                                s5 = -2.0 * q4 * Math.Cos(Math.Acos(q2 / q3) / 3.0);
                            }
                            else if (s4 < 0.0)
                            {
                                double x = q2 / q3;
                                if (q1 > 0.0)
                                {
                                    s5 = -2.0 * Math.Sign(q2) * q4 * Math.Cosh(Math.Log(x + Math.Sqrt(x * x - 1.0)) / 3.0);
                                }
                                else
                                {
                                    s5 = -2.0 * Math.Sign(q2) * q4 * Math.Sinh(Math.Log(x + Math.Sqrt(x * x + 1.0)) / 3.0);
                                }
                            }
                            else if (s4 == 0.0)
                            {
                                s5 = -2.0 * Math.Sign(q2) * q4;
                            }
                            double qs = s5 - s1 / 3.0;
                            q3 = a1 * a1 / 4.0 - a2 + qs;
                            if (q3 >= 0.0)
                            {
                                q1 = Math.Sqrt(q3);
                                s1 = 0.0;
                            }
                            else
                            {
                                q1 = 0.0;
                                s1 = Math.Sqrt(-q3);
                            }
                            q3 = qs * qs / 4.0 - a4;
                            if (q3 >= 0.0)
                            {
                                q2 = Math.Sqrt(q3);
                                s2 = 0.0;
                            }
                            else
                            {
                                q2 = 0.0;
                                s2 = Math.Sqrt(-q3);
                            }
                            q3 = a1 * qs / 2.0 - a3 - 2.0 * (q1 * q2 - s1 * s2);
                            if (Math.Abs(q3) > 1e-10)
                            {
                                q2 = -q2;
                                s2 = -s2;
                            }
                            q3 = a1 / 2 + q1; s3 = s1;
                            q4 = 2 * qs + 4 * q2; s4 = 4 * s2;
                            q5 = q3 * q3 - Sqr(s3) - q4; s5 = 2 * q3 * s3 - s4;
                            q4 = Math.Sqrt(Math.Sqrt(q5 * q5 + s5 * s5)); s4 = Math.Atan2(s5, q5) / 2.0; //test atan
                            q5 = q4 * Math.Cos(s4); s5 = q4 * Math.Sin(s4);
                            m_roots[0].Re = (-q3 + q5) / 2; m_roots[0].Im = (-s3 + s5) / 2;
                            m_roots[1].Re = (-q3 - q5) / 2; m_roots[1].Im = (-s3 - s5) / 2;
                            q3 = a[1] / 2 - q1; s3 = -s1;
                            q4 = 2 * qs - 4 * q2; s4 = -4 * s2;
                            q5 = q3 * q3 - s3 * s3 - q4; s5 = 2 * q3 * s3 - s4;
                            q4 = Math.Sqrt(Math.Sqrt(q5 + s5)); s4 = Math.Atan2(s5, q5) / 2;
                            q5 = q4 * Math.Cos(s4); s5 = q4 * Math.Sin(s4);
                            m_roots[2].Re = (-q3 + q5) / 2; m_roots[2].Im = (-s3 + s5) / 2;
                            m_roots[3].Re = (-q3 - q5) / 2; m_roots[3].Im = (-s3 - s5) / 2;
                            break;
                        }
                    }
                }
                return m_roots;
            }
        }
    }
}
