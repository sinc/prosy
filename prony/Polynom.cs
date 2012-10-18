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
            m_coeff = new double[K + 1];
        }

        public double value(double x)
        {
            double ret = m_coeff[0];
            for (int i = 1; i < m_coeff.Length; i++)
            {
               // ret += m_coeff[i] * Math.Pow(x, (double)i);
            }
            return ret;
        }
        
        public double kalitkin(int M, double[] a, ref Complex root)
        {
            double eps = double.MaxValue;
            double[] b = new double[M];
            double[] c = new double[M];
            for (int j = 0; j < M - 1; j++)
            {
                b[j] = a[j] * (M - j);
                c[j] = b[j] * (M - j - 1);
            }
            b[M - 1] = a[M - 1];
            for (int i = 0; i < 100 && eps > 5e-16; i++)
            {
                Complex zz = root;
                Complex p0 = new Complex(a[0], 0.0);
                Complex p1 = new Complex(b[1], 0.0);
                Complex p2 = new Complex(c[2], 0.0);
                Complex pp = new Complex();
                for (int k = 1; k < M - 1; k++)
                {
                    pp.Re = a[M - k] * zz.Re; pp.Im = a[M - k] * zz.Im;
                    p0 += pp;
                    pp.Re = b[M - k - 1] * zz.Re; pp.Im = b[M - k - 1] * zz.Im;
                    p1 += pp;
                    pp.Re = c[M - k - 2] * zz.Re; pp.Im = c[M - k - 2] * zz.Im;
                    p2 += pp;
                    zz *= root;
                }
                pp.Re = a[1] * zz.Re; pp.Im = a[1] * zz.Im;
                p0 += pp;
                pp.Re = b[0] * zz.Re; pp.Im = b[0] * zz.Im;
                p1 += pp;
                zz *= root;
                pp.Re = a[0] * zz.Re; pp.Im = a[0] * zz.Im;
                p0 += pp;
                Complex g = p1 / p0;
                Complex h = (g * g) - (p2 / p1);
                h *= M;
                pp = h - (g * g);
                pp *= M - 1;
                double um = Math.Sqrt(pp.Abs);
                double uf = pp.Arg / 2.0;
                pp.Re = um * Math.Cos(uf); pp.Im = um * Math.Sin(uf);
                h = g + pp;
                g -= pp;
                if (h.Abs < g.Abs)
                {
                    h = new Complex(g);
                }
                pp = new Complex(M, 0.0);
                pp /= h;
                root -= pp;
                eps = pp.Abs;
            }
            return eps;
        }

        public bool kalitkin(int K, double[] b, ref Complex zz)
        {
            double eps = 1e-16;
            bool kritKey = false;
            bool exitKey = true;
            double xerr0 = 1e5;
            if (K > 1)
            {

                Complex x0 = new Complex(-0.1, 0.0);
                Complex x1 = new Complex(0.1, 0.0);
                Complex x2 = new Complex(0.0, 0.0);
                while (exitKey)
                {
                    Complex p0 = new Complex(1.0, 0.0);
                    Complex p1 = new Complex(1.0, 0.0);
                    Complex p2 = new Complex(1.0, 0.0);
                    for (int i = 1; i < K + 1; i++)
                    {
                        p2 *= x2; p2.Re += b[i];
                        p1 *= x1; p1.Re += b[i];
                        p0 *= x0; p0.Re += b[i];
                    }
                    Complex f1 = p1 - p0;
                    Complex f2 = p2 - p1;
                    Complex y1 = x1 - x0;
                    Complex y2 = x2 - x1;
                    Complex y0 = x2 - x0;
                    Complex f0 = f2 * y1;
                    f1 *= y2;
                    f0 -= f1;
                    y0 *= y1 * y2;
                    if (y0.Abs == 0.0)
                        return true;
                    f0 /= y0;
                    f1 = (x2 + x1) * f0;
                    f2 /= y2;
                    f1 = f2 - f1;
                    y0 = f0 * x2 * x2;
                    y0 = p2 - y0;
                    f2 = y0 - (x2 * f1);
                    if (f0.Abs == 0.0)
                        return true;
                    y0 = f1 / f0;
                    y0.Re *= -0.5; y0.Im *= -0.5;
                    y1 = f1 * f1;
                    y2 = f0 * f2;
                    y2.Re *= 4.0; y2.Im *= 4.0;
                    y2 = y1 - y2;
                    f1.Re = Math.Sqrt(y2.Abs);
                    f1.Im = y2.Arg / 2.0;
                    y1.Re = f1.Re * Math.Cos(f1.Im) / 2.0;
                    y1.Im = f1.Re * Math.Sin(f1.Im) / 2.0;
                    y1 /= f0;
                    f0 = y0 - y1;
                    f1 = y0 + y1;
                    if (f0.Abs > f1.Abs)
                        f2 = f0;
                    else
                        f2 = f1;
                    f0 = f2 - x2;
                    double xerr1 = f0.Abs;
                    if (xerr1 < eps && !kritKey)
                        kritKey = true;
                    if (kritKey)
                        if (xerr1 < xerr0 && xerr1 > 1e-20)
                            exitKey = true;
                        else
                            exitKey = false; //??
                    x0 = new Complex(x1);
                    x1 = new Complex(x2);
                    x2 = new Complex(f2);
                    xerr0 = xerr1;
                }
                zz = x1;
            }
            if (K == 1)
            {
                zz = new Complex(-b[1], 0.0);
                return false;
            }
            return true;
        }

        public bool rootsLagerr(ref Complex[] roots)
        {
            int i = 0, j = 0;
            if (roots == null)
                roots = new Complex[P];
            Complex[] rr = Enumerable.Range(0, P).Select(x => new Complex(0.0, 1.0)).ToArray();
            double[] aa = new double[P + 1];
            m_coeff.CopyTo(aa, 0);
            double maxEps = 0.0;
            int k = 0;
            for (i = P; i >= 0; k++)
            {
                double eps = kalitkin(i, aa, ref rr[k]);
                rr[k].Im = Math.Abs(rr[k].Im);
                if (eps > maxEps)
                    maxEps = eps;
                if (rr[k].Im < 5e-16)
                {
                    rr[k].Im = 0.0;
                    i--;
                    for (j = 1; j < i + 1; j++)
                    {
                        aa[j] += aa[j - 1] * rr[k].Re;
                    }
                }
                else
                {
                    double alpha = -2.0 * rr[k].Re;
                    double betta = rr[k].Re * rr[k].Re - rr[k].Im * rr[k].Im;
                    i -= 2;
                    aa[1] -= alpha * aa[0];
                    for (j = 2; j < i + 1; j++)
                    {
                        aa[j] -= alpha * aa[j - 1] + betta * aa[j - 2];
                    }
                }
            }
            if (maxEps < 1e-10)
            {
                j = 0;
                for (i = 0; i < k; i++)
                {
                    if (rr[i].Im == 0)
                    {
                        roots[j] = rr[i];
                        j++;
                    }
                }
                i = (P - j) % 2;
                if ((2 * i + j) == P)
                {
                    int l = k;
                    for (i = 0; i < k; i++)
                    {
                        if (rr[i].Im > 0)
                        {
                            roots[j] = rr[i];
                            j++;
                            roots[l] = ~rr[i];
                            l++;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Получить корни полинома
        /// </summary>
        public Complex[] roots()
        {
            if (m_roots == null)
            {
                m_roots = new Complex[P];
                switch (P)
                {
                    case 1:
                    {   
                        m_roots[0].Re = -m_coeff[1] / m_coeff[0]; m_roots[0].Im = 0.0;
                        break;
                    }
                    case 2:
                    {
                        double dis = Math.Pow(m_coeff[1], 2.0) - 4.0 * m_coeff[0] * m_coeff[2];
                        if (dis < 0)
                        {
                            dis = Math.Sqrt(-dis);
                            m_roots[0].Re = -m_coeff[1] / m_coeff[0] / 2.0; m_roots[0].Im = dis / m_coeff[0] / 2.0;
                            m_roots[1].Re = m_roots[0].Re; m_roots[1].Im = -m_roots[0].Im;
                        }
                        else
                        {
                            dis = Math.Sqrt(dis);
                            m_roots[0].Re = (-m_coeff[1] + dis) / m_coeff[0] / 2.0; m_roots[0].Im = 0.0;
                            m_roots[1].Re = (-m_coeff[1] - dis) / m_coeff[0] / 2.0; m_roots[1].Im = 0.0;
                        }
                        break;
                    }
                    case 3:
                    {
                        //нормируем коэффициент при старшей степени
                        double a1 = m_coeff[1] / m_coeff[0];
                        double a2 = m_coeff[2] / m_coeff[0];
                        double a3 = m_coeff[3] / m_coeff[0];
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
                        double a1 = m_coeff[1] / m_coeff[0];
                        double a2 = m_coeff[2] / m_coeff[0];
                        double a3 = m_coeff[3] / m_coeff[0];
                        double a4 = m_coeff[4] / m_coeff[0];
                        double s1 = -a2;
                        double s2 = a1 * a3 - 4.0 * a4;
                        double s3 = (-a1 * a1 + 4.0 * a2) * a4 - a3 * a3;
                        double q1 = (s1 * s1 - 3.0 * s2) / 9.0;
                        double q2 = (s1 * (2 * s1 * s1 - 9.0 * s2) + 27.0 * s3) / 54.0;
                        double q3 = q1 * q1 * q1;
                        double s4 = q3 - q2 * q2, s5 = 0.0;
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
                        double q5 = q3 * q3 - s3*s3 - q4; s5 = 2 * q3 * s3 - s4;
                        q4 = Math.Sqrt(Math.Sqrt(q5 * q5 + s5 * s5)); s4 = Math.Atan2(s5, q5) / 2.0; //test atan
                        q5 = q4 * Math.Cos(s4); s5 = q4 * Math.Sin(s4);
                        m_roots[0].Re = (-q3 + q5) / 2; m_roots[0].Im = (-s3 + s5) / 2;
                        m_roots[1].Re = (-q3 - q5) / 2; m_roots[1].Im = (-s3 - s5) / 2;
                        q3 = a1 / 2 - q1; s3 = -s1;
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
