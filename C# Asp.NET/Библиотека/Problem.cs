using System;
using System.Collections.Generic;

namespace WebApplication1
{
    public class Problem
    {
        private Random rng;

        public int Seed { get; set; }

        public List<Task> Tasks { get; set; } = new List<Task>();

        public int Index { get; set; }

        public Problem(int paragraph, int tasks, int seed, int index)
        {
            Index = index;
            Seed = seed + index;
            rng = new Random(Seed);
            for (int i = 0; i < Math.Min(5, tasks); i++)
            {
                Tasks.Add(new Task(rng, paragraph, tasks / 5 + ((tasks % 5 > i) ? 1 : 0), i));
            }
        }
    }
}
