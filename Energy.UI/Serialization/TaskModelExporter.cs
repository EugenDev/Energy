using System.Collections.Generic;
using System.Text;
using System.Linq;
using Energy.Solving;
using Energy.Solving.PathFinding;
using Energy.UI.Model;

namespace Energy.UI.Serialization
{
    public static class TaskModelExporter
    {
        public static string ToText(TaskModel taskModel)
        {
            var builder = new StringBuilder(); 
            builder.AppendLine(string.Join(";", taskModel.FeaturesNames.Where(f => !Constants.ConstantFeatures.Contains(f))));

            builder.AppendLine(taskModel.Stations.Count.ToString());
            foreach (var station in taskModel.Stations)
            {
                var featuresValues = string.Join(";", station.Where(f => !Constants.ConstantFeatures.Contains(f.Key)).Select(f => station[f.Key]));
                builder.AppendLine($"{station.Name};{station.X};{station.Y};{featuresValues}");
            }
            builder.AppendLine(taskModel.Consumers.Count.ToString());
            foreach (var consumer in taskModel.Consumers)
            {
                var featuresValues = string.Join(";", consumer.Select(f => consumer[f.Key]));
                builder.AppendLine($"{consumer.Name};{consumer.X};{consumer.Y};{featuresValues}");
            }
            builder.AppendLine(taskModel.Links.Count.ToString());
            foreach (var link in taskModel.Links)
            {
                builder.AppendLine($"{link.From.Name};{link.To.Name};{link.Distance};{link.Conduction}");
            }

            return builder.ToString();
        }
        
        public static double[,] GetRMatrix(TaskModel taskModel)
        {
            var rowsCount = taskModel.Consumers.Count;
            var columnsCount = taskModel.FeaturesNames.Count;
            var result = new double[rowsCount, columnsCount];

            var row = 0;
            foreach (var consumer in taskModel.Consumers)
            {
                var column = 0;
                foreach (var columnsName in taskModel.FeaturesNames)
                {
                    result[row, column++] = consumer[columnsName];
                }
                row++;
            }

            return result;
        }
        
        public static double[,] GetSMatrix(TaskModel taskModel)
        {
            var columnsCount = taskModel.Stations.Count;
            var rowsCount = taskModel.FeaturesNames.Count;
            var result = new double[rowsCount, columnsCount];

            var column = 0;
            foreach (var station in taskModel.Stations)
            {
                var row = 0;
                foreach (var columnsName in taskModel.FeaturesNames)
                {
                    result[row++, column] = station[columnsName];
                }
                column++;
            }

            return result;
        }


        public static double[,] GetRMatrix(List<ConsumerModel> consumers, List<string> features)
        {
            var rowsCount = consumers.Count;
            var columnsCount = features.Count;
            var result = new double[rowsCount, columnsCount];

            var row = 0;
            foreach (var consumer in consumers)
            {
                var column = 0;
                foreach (var columnsName in features)
                {
                    result[row, column++] = consumer[columnsName];
                }
                row++;
            }

            return result;
        }

        public static double[,] GetSMatrix(List<StationModel> stations, List<string> features)
        {
            var columnsCount = stations.Count;
            var rowsCount = features.Count;
            var result = new double[rowsCount, columnsCount];

            var column = 0;
            foreach (var station in stations)
            {
                var row = 0;
                foreach (var columnsName in features)
                {
                    result[row++, column] = station[columnsName];
                }
                column++;
            }

            return result;
        }

        public static Graph GetGraph(TaskModel taskModel)
        {
            var result = new Graph();

            foreach (var station in taskModel.Stations)
                result.AddNode(station.Name);

            foreach (var consumer in taskModel.Consumers)
                result.AddNode(consumer.Name);

            foreach (var link in taskModel.Links)
                result.LinkNode(link.From.Name, link.To.Name, link.Distance, link.Conduction);

            return result;
        }

        public static Graph GetGraph(TaskModel taskModel, List<string> componentList)
        {
            var component = new HashSet<string>();
            foreach (var item in componentList)
                component.Add(item);

            var result = new Graph();

            foreach (var station in taskModel.Stations.Where(s => component.Contains(s.Name)))
                result.AddNode(station.Name);

            foreach (var consumer in taskModel.Consumers.Where(s => component.Contains(s.Name)))
                result.AddNode(consumer.Name);

            foreach (var link in taskModel.Links)
                if(component.Contains(link.From.Name))
                    result.LinkNode(link.From.Name, link.To.Name, link.Distance, link.Conduction);

            return result;
        }

        public static Task GetTask(TaskModel taskModel)
        {
            var graph = GetGraph(taskModel);
            var components = graph.GetComponents();


            var result = new Task();
            foreach (var component in components)
            {
                var stations = taskModel.Stations.Where(i => component.Contains(i.Name)).ToList();
                var consumers = taskModel.Consumers.Where(i => component.Contains(i.Name)).ToList();

                var s = GetSMatrix(stations, taskModel.FeaturesNames.Except(Constants.ConstantFeatures).ToList());
                var r = GetRMatrix(consumers, taskModel.FeaturesNames.Except(Constants.ConstantFeatures).ToList());

                var simpleTask = new SimpleTask(stations.Select(x => x.Name).ToList(),
                    consumers.Select(x => x.Name).ToList(), r, s);

                simpleTask.ConsumersDistanceDemand = consumers.ToDictionary(i => i.Name, i => i[Constants.DistanceFeatureName]);
                simpleTask.ConsumersConductionDemand = consumers.ToDictionary(i => i.Name, i => i[Constants.ConductionFeatureName]);

                simpleTask.Graph = GetGraph(taskModel, component);

                result.SimpleTasks.Add(simpleTask);
            }
            return result;
        }
    }
}
