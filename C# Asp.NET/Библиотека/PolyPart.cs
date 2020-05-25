using System;
using System.Linq;
using System.Collections.Generic;

namespace GeneratorLibrary
{
    public class PolyPart
    {
        public List<Variable> Variables = new List<Variable>();

        public Rational Coeff { get; set; } = 1;

        public PolyPart(Rational coeff, Variable? variable = null)
        {
            if (variable != null)
                Variables.Add((Variable)variable);

            Coeff = coeff;
        }
        public PolyPart(Rational coeff, List<Variable> variables)
        {
            Variables = variables;
            Coeff = coeff;
        }

        public PolyPart Pow(Rational p)
        {
            if (p == 0)
                return new PolyPart(1);

            List<Variable> newVariables = new List<Variable>();
            foreach (Variable var in Variables)
            {
                newVariables.Add(var.Pow(p));
            }
            return new PolyPart(Coeff.Pow(p), newVariables);
        }

        public static implicit operator Polynomial(PolyPart polyPart)
        {
            return new Polynomial(polyPart);
        }

        public static implicit operator PolyPart(Rational r)
        {
            return new PolyPart(r);
        }

        public static implicit operator PolyPart(int r)
        {
            return new PolyPart(r);
        }

        public static PolyPart operator +(PolyPart p1)
        {
            return new PolyPart(p1.Coeff, new List<Variable>(p1.Variables));
        }
        public static PolyPart operator -(PolyPart p1)
        {
            return new PolyPart(-p1.Coeff, new List<Variable>(p1.Variables));
        }
        public static PolyPart operator +(PolyPart p1, PolyPart p2)
        {
            if (p1.Variables != p2.Variables)
                throw new PolyException("PolyParts must have equal variables parts");

            return new PolyPart(p1.Coeff + p2.Coeff, new List<Variable>(p1.Variables));
        }
        public static PolyPart operator -(PolyPart p1, PolyPart p2)
        {
            if (p1.Variables != p2.Variables)
                throw new PolyException("PolyParts must have equal variables parts");

            return new PolyPart(p1.Coeff - p2.Coeff, new List<Variable>(p1.Variables));
        }
        public static PolyPart operator *(PolyPart p1, PolyPart p2)
        {
            foreach (Variable var in p2.Variables)
            {
                p1 *= var;
            }

            return new PolyPart(p1.Coeff * p2.Coeff, new List<Variable>(p1.Variables));
        }
        public static PolyPart operator *(PolyPart p1, Variable v1)
        {
            bool found = false;
            List<Variable> newVariables = new List<Variable>();
            Variable result;

            for (int i = 0; i < p1.Variables.Count; i++)
            {
                if (p1.Variables[i].Name == v1.Name)
                {
                    found = true;
                    result = v1 * p1.Variables[i];
                    if (result.Power == 0)
                        continue;
                }
                else
                {
                    result = p1.Variables[i];
                }
                newVariables.Add(result);
            }

            if (!found)
            {
                newVariables.Add(v1);
            }

            return new PolyPart(p1.Coeff, newVariables);
        }
        public static PolyPart operator /(PolyPart p1, PolyPart p2)
        {
            foreach (Variable var in p2.Variables)
            {
                p1 /= var;
            }

            return new PolyPart(p1.Coeff * p2.Coeff, new List<Variable>(p1.Variables));
        }
        public static PolyPart operator /(PolyPart p1, Variable v1)
        {
            bool found = false;
            List<Variable> newVariables = new List<Variable>();
            Variable result;

            for (int i = 0; i < p1.Variables.Count; i++)
            {
                if (p1.Variables[i].Name == v1.Name)
                {
                    found = true;
                    result = p1.Variables[i] / v1;
                    if (result.Power == 0)
                        continue;
                }
                else
                {
                    result = p1.Variables[i];
                }
                newVariables.Add(result);
            }

            if (!found)
            {
                newVariables.Add(v1);
            }

            return new PolyPart(p1.Coeff, newVariables);
        }
        public static PolyPart operator ^(PolyPart p1, Rational r1)
        {
            return p1.Pow(r1);
        }

        public override string ToString()
        {
            List<string> vars = new List<string>();
            List<string> varsDown = new List<string>();
            if (Coeff != 1 && Coeff != -1)
                vars.Add(Coeff.ToString());
            Variables.Sort();
            foreach (Variable var in Variables)
                if (var.Power > 0)
                    vars.Add(var.ToString());
                else if (var.Power < 0)
                    varsDown.Add((var^-1).ToString());

            string str;
            if (vars.Count > 0) {
                if (Coeff != -1)
                    str = string.Join("", vars);
                else
                    str = '-' + string.Join("", vars);
            }
            else
            {
                str = Coeff.ToString();
            }


            if (varsDown.Count > 0)
                str = @"\frac{" + str + "}{" + string.Join("", varsDown) + "}";

            return str;
        }
    }
}
