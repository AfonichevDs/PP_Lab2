using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ant_Algorithm
{
    public class Ant
    {
        public int AntId { get; set; }
        public ConsoleColor AntColor { get; set; } = ConsoleColor.Gray;
        public Node CurrentNode { get; set; }
        public List<Tuple<double, Node>> Choices { get; set; } = new List<Tuple<double, Node>>();
        public HashSet<Node> VisitedNodes { get; set; } = new HashSet<Node>();
        public double WayPassed { get; private set; }
        public bool CompletedMoving { get; private set; } = false;

        public Ant(Node initial, int id)
        {
            AntId = id;
            AntColor = (ConsoleColor)(Const.Count++ % 15);
            CurrentNode = initial;
            RecalculateChoices();
        }

        public void RecalculateChoices()
        {
            Choices.Clear();
            CurrentNode.Neighbours.ToList().ForEach(z =>
            {
                var P = CalculateChoice(z.Value);
                Choices.Add(new Tuple<double, Node>(P, z.Key));
            });
        }

        public void Move()
        {
            if (CompletedMoving) return;

            double prev = 0;
            double random = new Random().NextDouble();
            foreach (var item in Choices.Where(z => !VisitedNodes.Contains(z.Item2)).OrderByDescending(z => z.Item1))
            {
                if (random > prev && random < item.Item1 + prev)
                {
                    var transition = item.Item2.Neighbours.GetValueOrDefault(CurrentNode);
                    LeavePheromone(transition);
                    VisitedNodes.Add(CurrentNode);

                    WayPassed += transition.Value;
                    CurrentNode = item.Item2;   //.Neighbours.Where(z => z.Value == transition && z.Key != CurrentNode).First().Key;
                    RecalculateChoices();
                    return;
                }
                prev += item.Item1;
            }
            VisitedNodes.Add(CurrentNode);
            CompletedMoving = true;
        }

        public void LeavePheromone(Transition t)
        {
            t.Pheromone += PheromoneToLeave(t);
        }

        private double PheromoneToLeave(Transition t)
        {
            return Const.Q / t.Value;
        }

        private double CalculateChoice(Transition t)
        {
            return (Math.Pow(Math.Floor(t.Pheromone), Const.A) * Math.Pow(t.Visibility, Const.B)) /
                CurrentNode.Neighbours.Where(z => !VisitedNodes.Contains(z.Key)).Sum(s => (Math.Pow(Math.Floor(s.Value.Pheromone), Const.A) * Math.Pow(s.Value.Visibility, Const.B)));
        }
    }
}
