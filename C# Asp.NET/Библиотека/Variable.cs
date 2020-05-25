using System;

namespace GeneratorLibrary
{
    public struct Variable : IComparable<Variable>
    {
        public Symbol Name { get; set; }
        public Rational Power { get; set; }

        public Variable(Symbol name, Rational? power = null)
        {
            if (power == null)
                Power = 1;
            else
                Power = (Rational)power;

            Name = name;
        }

        public Variable Pow(Rational p)
        {
            return new Variable(Name, Power * p);
        }
        public int CompareTo(Variable other)
        {
            int compare = Power.CompareTo(other.Power);
            if (compare != 0)
                return compare;
            return Name.CompareTo(other.Name);
        }

        public static implicit operator PolyPart(Variable variable)
        {
            return new PolyPart(1, variable);
        }

        public static Variable operator *(Variable v1, Variable v2)
        {
            if (v1.Name != v2.Name)
                throw new PolyException("Variables must have the same name");
            return new Variable(v1.Name, v1.Power + v2.Power);
        }
        public static Variable operator /(Variable v1, Variable v2)
        {
            if (v1.Name != v2.Name)
                throw new PolyException("Variables must have the same name");
            return new Variable(v1.Name, v1.Power - v2.Power);
        }
        public static Variable operator ^(Variable v1, Rational r1)
        {
            return v1.Pow(r1);
        }

        public static bool operator ==(Variable v1, Variable v2)
        {
            return v1.Power == v2.Power && v1.Name == v2.Name;
        }

        public static bool operator !=(Variable v1, Variable v2)
        {
            return !(v1 == v2);
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() == GetType())
            {
                return (this == (Variable)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() | Power.GetHashCode();
        }

        public override string ToString()
        {
            if (Power == 1)
                return Name.ToString();
            return Name.ToString() + "^" + Power;
        }
    }
}
