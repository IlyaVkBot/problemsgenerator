using Microsoft.VisualBasic.CompilerServices;
using System.Data.Common;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Numerics;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;

namespace GeneratorLibrary
{
    public class Polynomial
    {
        [JsonIgnore]
        public int Type { get; }
        private PolyPart Part;
        public List<Polynomial> Polynomials = new List<Polynomial>();

        [JsonIgnore]
        public Rational Power { get; set; }

        private Rational coeff = 1;
        [JsonIgnore]
        public Rational Coeff
        {
            get
            {
                if (Type == 1)
                    return Part.Coeff;
                return coeff;
            }
            set
            {
                if (Type == 1)
                    Part.Coeff = value;
                else
                    coeff = value;
            }
        }

        public Polynomial(PolyPart polyPart, Rational? coeff = null, Rational? power = null)
        {
            Type = 1;
            Power = 1;
            if (power != null)
                polyPart ^= (Rational)power;
            if (coeff != null)
                polyPart *= coeff;

            Part = polyPart;

        }
        public Polynomial(Polynomial p)
        {
            Type = p.Type;
            Power = p.Power;
            if (p.Type == 1)
            {
                Part = new PolyPart(p.Part.Coeff, new List<Variable>(p.Part.Variables));
            }
            else
            {
                foreach (Polynomial pol in p.Polynomials)
                {
                    Polynomials.Add(pol.Copy());
                }
            }
            Coeff = p.Coeff;
        }

        public Polynomial(List<Polynomial> p, int type, Rational? coeff = null, Rational? power = null)
        {
            if (type != 2 && type != 3)
                throw new PolyException("Type can only be 2(+) or 3(*)");
            Type = type;
            Power = 1;
            Coeff = 1;
            if (power != null)
                Power = (Rational)power;
            if (coeff != null)
                Coeff = (Rational)coeff;

            foreach (Polynomial pol in p)
            {
                Polynomials.Add(pol.Copy());
            }
        }

        public Polynomial Copy()
        {
            return new Polynomial(this);
        }

        public static implicit operator Polynomial(Rational r)
        {
            return new Polynomial(r);
        }

        public static implicit operator Polynomial(int r)
        {
            return new Polynomial(r);
        }

        public static implicit operator Polynomial(Symbol s)
        {
            try
            {
                return new Polynomial(int.Parse(s.Name));
            }
            catch
            {
                return new Polynomial(new Variable(s));
            }
        }

        public static Polynomial operator +(Polynomial p)
        {
            return p.Copy();
        }

        public static Polynomial operator -(Polynomial p)
        {
            Polynomial newPoly = p.Copy();
            if (newPoly.Type == 1)
            {
                newPoly.Part.Coeff = -newPoly.Part.Coeff;
            }
            else
            {
                newPoly.Coeff = -newPoly.Coeff;
            }
            return newPoly;
        }

        public static Polynomial operator +(Polynomial p1, Polynomial p2)
        {
            Polynomial newPoly;
            if (p1.Type == 2 && p2.Type == 2)
            {
                newPoly = p1.Copy();
                foreach (Polynomial pol in p2.Polynomials)
                {
                    newPoly.Polynomials.Add(pol.Copy());
                }
            }
            else if (p1.Type == 2)
            {
                newPoly = p1.Copy();
                newPoly.Polynomials.Add(p2.Copy());
            }
            else if (p2.Type == 2)
            {
                newPoly = p2.Copy();
                newPoly.Polynomials.Add(p1.Copy());
            }
            else
            {
                if (p1.Type == 1 && p2.Type == 1)
                {
                    try
                    {
                        return p1.Part + p2.Part;
                    }
                    catch { }
                }
                List<Polynomial> polyList = new List<Polynomial>();
                polyList.Add(p1);
                polyList.Add(p2);
                newPoly = new Polynomial(polyList, 2);
            }
            return newPoly;
        }
        public static Polynomial operator -(Polynomial p1, Polynomial p2)
        {
            return p1 + (-p2);
        }
        public static Polynomial operator *(Polynomial p1, Polynomial p2)
        {
            Polynomial newPoly;
            if (p1.Type == 1 && p2.Type != 1 && p1.Part.Variables.Count() == 0)
            {
                newPoly = p2.Copy();
                newPoly.Coeff *= p1.Part.Coeff;
                return newPoly;
            }
            else if (p2.Type == 1 && p1.Type != 1 && p2.Part.Variables.Count() == 0)
            {
                newPoly = p1.Copy();
                newPoly.Coeff *= p2.Part.Coeff;
                return newPoly;
            }
            else if (p1.Type == 3 && p2.Type == 3)
            {
                newPoly = p1.Copy();
                foreach (Polynomial pol in p2.Polynomials)
                {
                    newPoly.Polynomials.Add(pol.Copy());
                }
            }
            else if (p1.Type == 3)
            {
                newPoly = p1.Copy();
                newPoly.Polynomials.Add(p2.Copy());
            }
            else if (p2.Type == 3)
            {
                newPoly = p2.Copy();
                newPoly.Polynomials.Add(p1.Copy());
            }
            else
            {
                if (p1.Type == 1 && p2.Type == 1)
                {
                    return p1.Part * p2.Part;
                }
                List<Polynomial> polyList = new List<Polynomial>();
                polyList.Add(p1);
                polyList.Add(p2);
                newPoly = new Polynomial(polyList, 3);
            }
            return newPoly;
        }

        public static Polynomial operator /(Polynomial p1, Polynomial p2)
        {
            return p1 * (p2 ^ -1);
        }

        public static Polynomial operator ^(Polynomial p, Rational r)
        {
            Polynomial newPoly = p.Copy();
            if (newPoly.Type == 1)
            {
                newPoly.Part = newPoly.Part ^ r;
            }
            else
            {
                newPoly.Power *= r;
            }
            return newPoly;
        }

        public string Latex
        {
            get
            {
                string str = ToString();
                if (Type != 3 && str.StartsWith('(') && str.EndsWith(')'))
                    str = str.Substring(1, str.Length - 2);
                str.Replace("+", " + ");
                return "$" + str + "$";
            }
        }

        public override string ToString()
        {
            if (Type == 1)
                return Part.ToString();

            List<string> polys = new List<string>();

            if (Type == 2)
            {
                foreach (Polynomial poly in Polynomials)
                    polys.Add(poly.ToString());
                string str = "(" + string.Join("+", polys) + ")";
                if (Power != 1)
                    str = str + "^" + Power;
                if (Coeff == -1)
                    str = "-" + str;
                else if (Coeff != 1)
                    str = Coeff + "" + str;

                return str.Replace("+-", " - ");
            }
            else
            {
                List<string> polysDown = new List<string>();
                if (Coeff != 1 && Coeff != -1)
                    polys.Add(Coeff.ToString());
                foreach (Polynomial poly in Polynomials)
                {
                    if (poly.Power > 0)
                        polys.Add(poly.ToString());
                    else if (poly.Power < 0)
                        polysDown.Add((poly ^ -1).ToString());
                }
                string str;

                if (polys.Count > 0)
                {
                    if (Coeff != -1)
                        if (polys.Count == 1 && polys[0].StartsWith("(") && polys[0].EndsWith(")"))
                            str = polys[0].Substring(1, polys[0].Length - 2);
                        else
                            str = string.Join("", polys);
                    else
                        str = '-' + string.Join("", polys);
                }
                else
                {
                    str = Coeff.ToString();
                }


                if (polysDown.Count == 1 && polysDown[0].StartsWith("(") && polysDown[0].EndsWith(")"))
                    polysDown[0] = polysDown[0].Substring(1, polysDown[0].Length - 2);

                if (polysDown.Count > 0)
                    str = @"\frac{" + str + "}{" + string.Join("", polysDown) + "}";

                if (Power != 1)
                        str = "(" + str + ")^" + Power;

                return str;
            }
        }
    }
}

