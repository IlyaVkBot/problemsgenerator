using System;

namespace GeneratorLibrary
{
    public static class RandomExtention
    {
        public static Rational NextR(this Random rng, int min, int max, int maxdenom)
        {
            int denom = rng.Next(1, maxdenom + 1);
            int nom = rng.Next(min * denom, max * denom + 1);
            return new Rational(nom, denom);
        }

        public static Symbol NextS(this Random rng, bool numbers = false, int maxnum = 5)
        {
            if (numbers)
                return rng.Next(0, 6) == 1 ? "" + rng.Next(1, maxnum + 1) : "" + (char)rng.Next('p', 'z' + 1);
            return (char)rng.Next('p', 'z' + 1);
        }

        public static int NextSign(this Random rng)
        {
            return rng.Next(0, 2) == 1 ? -1 : 1;
        }
    }
}

