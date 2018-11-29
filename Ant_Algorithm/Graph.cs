using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ant_Algorithm
{
    public class Graph
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Ant> Ants { get; set; }

        public Graph(string inputFile)
        {
            Initialize(inputFile);
        }

        public void SpawnAnts(int num)
        {
            Ants = new List<Ant>();
            for (int i = 0; i < num; i++)
            {
                Ants.Add(new Ant(Nodes[new Random().Next(Nodes.Count)], i));
            }
        }

        private void Initialize(string inputFile)
        {
            File.ReadAllLines(inputFile).ToList().ForEach(z =>
            {
                var data = z.Split(' ');
                if (this[data[0]] == null) Nodes.Add(new Node { Name = data[0] });
                if (this[data[2]] == null) Nodes.Add(new Node { Name = data[2] });

                var transition = new Transition(Convert.ToInt32(data[1]));

                this[data[0]].Neighbours.Add(this[data[2]], transition);
                this[data[2]].Neighbours.Add(this[data[0]], transition);
            });
        }

        public void Evaporation()
        {
            Nodes.SelectMany(z =>z.Neighbours).ToList().ForEach(tr =>
            {
                tr.Value.Pheromone = CalculateEvaporation(tr.Value);
            });
        }

        private double CalculateEvaporation(Transition t)
        {
            if (t.Pheromone == 0) return 0;
            return (1 - Const.P) * t.Pheromone;
        }

        Node this[string name]
        {
            get => Nodes.SingleOrDefault(z => z.Name == name);
        }
    }

    public class Node
    {
        public string Name { get; set; }
        public Dictionary<Node, Transition> Neighbours { get; set; } = new Dictionary<Node, Transition>();
    }

    public class Transition
    {
        public Transition(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
        public double Pheromone { get; set; } = Const.InitialPheromone;
        public double Visibility { get => 1 / (double)Value; }
    }
}
