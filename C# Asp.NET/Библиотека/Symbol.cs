using System;

namespace GeneratorLibrary
{
    public class Symbol : IComparable<Symbol>
    {
        public string Name { get; set; }

        public Symbol(string name)
        {
            Name = name;
        }

        public int CompareTo(Symbol other)
        {
            return Name.CompareTo(other.Name);
        }

        public static implicit operator Variable(Symbol symbol)
        {
            return new Variable(symbol, 1);
        }

        public static implicit operator Symbol(string name)
        {
            return new Symbol(name);
        }

        public static implicit operator Symbol(char name)
        {
            return new Symbol(name + "");
        }

        public static implicit operator Symbol(int name)
        {
            return new Symbol(name.ToString());
        }

        public static Polynomial operator +(Symbol s1, Symbol s2)
        {
            return (Polynomial)s1 + s2;
        }
        public static Polynomial operator -(Symbol s1, Symbol s2)
        {
            return (Polynomial)s1 - s2;
        }
        public static Polynomial operator *(Symbol s1, Symbol s2)
        {
            return (Polynomial)s1 * s2;
        }
        public static Polynomial operator /(Symbol s1, Symbol s2)
        {
            return (Polynomial)s1 / s2;
        }
        public static Polynomial operator ^(Symbol s1, Rational r1)
        {
            return (Polynomial)s1^r1;
        }

        public static bool operator ==(Symbol s1, Symbol s2)
        {
            return s1.Name == s2.Name;
        }

        public static bool operator !=(Symbol s1, Symbol s2)
        {
            return !(s1 == s2);
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() == GetType())
            {
                return (this == (Symbol)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
