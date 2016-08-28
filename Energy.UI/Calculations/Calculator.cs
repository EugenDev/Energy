using System.Collections.ObjectModel;
using System.Linq;
using Energy.Solving.PathFinding;
using Energy.UI.Model;

namespace Energy.UI.Calculations
{
    public class Calculator
    {
        public void ReCalculateDistances(TaskModel taskModel)
        {
            var graph = new Graph();

            foreach (var station in taskModel.Stations)
                graph.AddNode(station.Name);

            foreach (var consumer in taskModel.Consumers)
                graph.AddNode(consumer.Name);

            foreach (var link in taskModel.Links)
                graph.LinkNode(link.From.Name, link.To.Name, link.Distance, link.Conduction);

            RecalculateCollection(graph, taskModel.Stations, taskModel.Consumers);
            RecalculateCollection(graph, taskModel.Consumers, taskModel.Stations);
        }

        private void RecalculateCollection<TFrom, TTo>(Graph graph, ObservableCollection<TFrom> fromCollection,
            ObservableCollection<TTo> toCollection) where TFrom:ModelBase where TTo:ModelBase
        {
            foreach (var item in fromCollection)
            {
                var distances = graph.GetShortestDistances(item.Name);
                var conductions = graph.GetConductions(item.Name);

                var maxDistance = distances
                    .Where(n => toCollection.Any(c => c.Name.Equals(n.Key)))
                    .Max(n => n.Value);

                var maxConduction = conductions
                    .Where(n => toCollection.Any(c => c.Name.Equals(n.Key)))
                    .Max(n => n.Value);

                var minConduction = conductions
                    .Where(n => toCollection.Any(c => c.Name.Equals(n.Key)))
                    .Min(n => n.Value);

                var minDistance = distances
                    .Where(n => toCollection.Any(c => c.Name.Equals(n.Key)))
                    .Min(n => n.Value);

                item["Расстояние"] = minDistance / maxDistance;
                item["Проводимость"] = (double)minConduction / maxConduction;
            }
        }
    }
}
