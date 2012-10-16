using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prony
{
    public class Complex
    {
        public double Re { get; set; }
        public double Im { get; set; }

        public double Abs { get { return Math.Sqrt(Re * Re + Im * Im); } }
        public double Arg
        {
            get
            {
                if (Re == 0.0)
                {
                    if (Im == 0.0)
                        return 0.0;
                    if (Im > 0)
                        return Math.PI / 2;
                    else
                        return -Math.PI / 2;
                }
                if (Re > 0)
                {
                    return Math.Atan(Im / Re);
                }
                else
                {
                    if (Im > 0)
                        return Math.Atan(Im / Re) + Math.PI;
                    else
                        return Math.Atan(Im / Re) - Math.PI;
                }
            }
        }

        public static bool operator ==(Complex a, Complex b)
        {
            return a.Re == b.Re && a.Im == b.Im;
        }

        public static bool operator !=(Complex a, Complex b)
        {
            return a.Re != b.Re || a.Im != b.Im;
        }

        public static Complex operator +(Complex a, Complex b)
        {
            return new Complex(a.Re + b.Re, a.Im + b.Im);
        }

        public static Complex operator -(Complex a, Complex b)
        {
            return new Complex(a.Re - b.Re, a.Im - b.Im);
        }

        public static Complex operator -(Complex a)
        {
            return new Complex(-a.Re, -a.Im);
        }

        public static Complex operator *(Complex a, Complex b)
        {
            return new Complex(a.Re * b.Re - a.Im * b.Im, a.Re * b.Im + b.Re * a.Im);
        }

        public static Complex operator *(Complex a, double b)
        {
            return new Complex(a.Re * b, a.Im * b);
        }
                
        public static Complex operator /(Complex a, Complex b)
        {
            double abs_b = b.Abs;
            if (abs_b == 0)
                throw new Exception("bad second operand");
            abs_b = 1 / (abs_b * abs_b);
            return (a * ~b) * abs_b;
        }
        
        /*public static Complex operator /(Complex a, Complex b)
        {
            double cd = b.Re * b.Re + b.Im * b.Im;

            return new Complex((a.Re * b.Re + a.Im * b.Im) / cd, (a.Im * b.Re - a.Re * b.Im) / cd);
        }*/
        
        /// <summary>
        /// Комплексное сопряжение
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex operator ~(Complex a)
        {
            return new Complex(a.Re, -a.Im);
        }

        public static Complex Ln(Complex a)
        {
            return new Complex(Math.Log(a.Abs), a.Arg);
        }

        public static Complex Sqrt(Complex a)
        {
            double sqrt_abs = Math.Sqrt(a.Abs);
            return new Complex(sqrt_abs * Math.Cos(a.Arg / 2), sqrt_abs * Math.Sin(a.Arg / 2));
        }

        public override string ToString()
        {
            return string.Format("({0}; {1})", Re, Im);
        }
    }
}
