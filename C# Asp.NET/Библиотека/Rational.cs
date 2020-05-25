using System;
using System.Runtime.CompilerServices;

namespace GeneratorLibrary
{
    public struct Rational : IComparable<Rational>
    {
        private int numerator;
        private int denominator;

        public Rational(int int_value)
        {
            numerator = int_value;
            denominator = 1;
        }

        public Rational(int new_numerator, int new_denominator)
        {
            numerator = new_numerator;
            denominator = new_denominator;
            Regularize();
        }

        public int Numerator
        {
            get => numerator;
            set
            {
                numerator = value;
                Regularize();
            }
        }

        public int Denominator
        {
            get => denominator;
            set
            {
                if (value == 0)
                {
                    throw new PolyException("Denominator must not be 0");
                }
                denominator = value;
                Regularize();
            }
        }

        private void Regularize()
        {
            int divisor = Math.Sign(Denominator) * Gcd(Numerator, Denominator);
            if (divisor == 0)
            {
                numerator = 0;
                denominator = 1;
            }
            else
            {
                numerator /= divisor;
                denominator /= divisor;
            }
        }

        public Rational Pow(Rational p)
        {
            int numerator = (int)(Math.Pow(Numerator, (double)p)*100);
            int denominator = (int)(Math.Pow(Denominator, (double)p)*100);
            return new Rational(numerator, denominator);
        }

        public static int Gcd(int v1, int v2)
        {
            int tmp;

            if (v1 == 0 || v2 == 0) return 0;

            v1 = Math.Abs(v1);
            v2 = Math.Abs(v2);

            if (v2 > v1)
            {
                tmp = v1;
                v1 = v2;
                v2 = tmp;
            }

            while (true)
            {
                tmp = v1 % v2;
                if (tmp == 0) return v2;
                v1 = v2;
                v2 = tmp;
            }
        }

        public static int CommonDenominator(Rational r1, Rational r2)
        {
            return r1.Denominator / Gcd(r1.Denominator, r2.Denominator) * r2.Denominator;
        }

        public int CompareTo(Rational rational)
        {
            return (Numerator * (long)rational.Denominator).CompareTo(Denominator * (long)rational.Numerator);
        }
        public static bool operator ==(Rational r1, Rational r2)
        {
            return (r1.Numerator == r2.Numerator && r1.Denominator == r2.Denominator);
        }

        public static bool operator !=(Rational r1, Rational r2)
        {
            return !(r1 == r2);
        }

        public static bool operator <(Rational r1, Rational r2)
        {
            return r1.CompareTo(r2) == -1;
        }

        public static bool operator >(Rational r1, Rational r2)
        {
            return (r2 < r1);
        }
        public static bool operator <=(Rational r1, Rational r2)
        {
            return (r1 == r2 || r1 < r2);
        }
        public static bool operator >=(Rational r1, Rational r2)
        {
            return (r1 == r2 || r2 < r1);
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == GetType())
            {
                return (this == (Rational)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (Numerator | (Denominator << 16));
        }

        public static explicit operator int(Rational r)
        {
            return r.Numerator / r.Denominator;
        }

        public static explicit operator double(Rational r)
        {
            return (double)r.Numerator / (double)r.Denominator;
        }

        public static explicit operator float(Rational r)
        {
            return (float)r.Numerator / (float)r.Denominator;
        }

        public static implicit operator Rational(int r)
        {
            return new Rational(r);
        }

        public static Rational operator +(Rational r)
        {
            return new Rational(r.Numerator, r.Denominator);
        }

        public static Rational operator -(Rational r)
        {
            return new Rational(-r.Numerator, r.Denominator);
        }

        public static Rational operator +(Rational r1, Rational r2)
        {
            int denominator = CommonDenominator(r1, r2);
            int numerator = denominator / r1.Denominator * r1.Numerator + denominator / r2.Denominator * r2.Numerator;
            return new Rational(numerator, denominator);
        }

        public static Rational operator -(Rational r1, Rational r2)
        {
            return r1 + (-r2);
        }

        public static Rational operator *(Rational r1, Rational r2)
        {
            if (r1.Numerator == 0 || r2.Numerator == 0)
                return new Rational(0);
            int numerator = (r1.Numerator / Gcd(r1.Numerator, r2.Denominator)) * (r2.Numerator / Gcd(r1.Denominator, r2.Numerator));
            int denominator = (r1.Denominator / Gcd(r1.Denominator, r2.Numerator)) * (r2.Denominator / Gcd(r1.Numerator, r2.Denominator));
            return new Rational(numerator, denominator);
        }
        public static Rational operator ^(Rational r1, Rational r2)
        {
            return r1.Pow(r2);
        }

        public static Rational operator /(Rational r1, Rational r2)
        {
            if (r2.Numerator == 0)
                throw new DivideByZeroException();

            if (r1.Numerator == 0)
                return new Rational(0);

            int numerator = (r1.Numerator / Gcd(r1.Numerator, r2.Numerator)) * (r2.Denominator/ Gcd(r1.Denominator, r2.Denominator));
            int denominator = (r2.Numerator / Gcd(r1.Numerator, r2.Numerator)) * (r1.Denominator/ Gcd(r1.Denominator, r2.Denominator));
            return new Rational(numerator, denominator);
        }
        public override string ToString()
        {
            if (Denominator == 1) return Numerator.ToString();

            if(Numerator < 0)
                return @"-\frac{" + (-Numerator) + "}{" + Denominator + "}";
            
            return @"\frac{" + Numerator + "}{" + Denominator + "}";
        }
    }
}
