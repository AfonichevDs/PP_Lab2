using System;
using System.Collections.Generic;
using System.Linq;

namespace Ant_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph("C:\\Univ\\PP\\test.txt");
            bool allCitiesReached = false;

            double bestWayValue = Const.initialMaxValue;
            List<Node> bestWay = new List<Node>();

            for (int i = 0; i < 15; i++)
            {
                graph.SpawnAnts(10);
                for (int t = 0; t < 10; t++)
                {
                    graph.Ants.ForEach(z =>
                    {
                        //Console.ForegroundColor = z.AntColor;
                        //Console.WriteLine("Ant {0} moved to city {1}", z.AntId, z.CurrentNode.Name);
                        z.Move();
                    });
                    graph.Ants.Where(ant => ant.CompletedMoving).ToList().ForEach(ant =>
                    {
                        if (ant.VisitedNodes.Count == graph.Nodes.Count && ant.WayPassed < bestWayValue)
                        {
                            bestWayValue = ant.WayPassed;
                            bestWay = ant.VisitedNodes.ToList();
                            allCitiesReached = true;
                        }
                    });
                    graph.Ants.RemoveAll(z => z.CompletedMoving);
                    graph.Evaporation();

                    if (graph.Ants.Count == 0) break;
                }
            }

            var trToStart = graph.Nodes.FirstOrDefault(z => z.Name == bestWay[bestWay.Count - 1].Name).Neighbours.FirstOrDefault(z => z.Key == bestWay[0]);

            bestWay.Add(trToStart.Key);
            bestWayValue += trToStart.Value.Value;

            Console.ForegroundColor = ConsoleColor.Gray;
            if (allCitiesReached)
                Console.WriteLine("Best way value: {0}", bestWayValue);
            else Console.WriteLine("Ants reached dead end");

            bestWay.ForEach(z => Console.Write(" -> " + z.Name));
            Console.ReadLine();
        }
    }
}
