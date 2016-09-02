using System;
using System.Collections.Generic;
using Energy.UI.Model;

namespace Energy.UI.Serialization
{
    public static class TaskModelImporter
    {
        public static TaskModel CreateFromText(string[] lines)
        {
            var result = new TaskModel();
            var lineNumber = 0;

            var featuresNames = lines[lineNumber++].Split(new [] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var featureName in featuresNames)
                result.FeaturesNames.Add(featureName);

            var map = new Dictionary<string, ModelBase>();

            var stationsCount = int.Parse(lines[lineNumber++]);
            for (var i = 0; i < stationsCount; i++)
            {
                var stationParts = lines[lineNumber++].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var station = new StationModel(stationParts[0], result.FeaturesNames)
                {
                    X = double.Parse(stationParts[1]),
                    Y = double.Parse(stationParts[2])
                };
                for (var fn = 0; fn < stationParts.Length - 3; fn++)
                {
                    station[featuresNames[fn]] = double.Parse(stationParts[fn + 3]);
                }
                result.Stations.Add(station);
                map.Add(station.Name, station);
            }

            var consumersCount = int.Parse(lines[lineNumber++]);
            for (var i = 0; i < consumersCount; i++)
            {
                var consumerParts = lines[lineNumber++].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                var consumer = new ConsumerModel(consumerParts[0], result.FeaturesNames)
                {
                    X = double.Parse(consumerParts[1]),
                    Y = double.Parse(consumerParts[2])
                };
                for (var fn = 0; fn < consumerParts.Length - 3; fn++)
                {
                    consumer[featuresNames[fn]] = double.Parse(consumerParts[fn + 3]);
                }
                result.Consumers.Add(consumer);
                map.Add(consumer.Name, consumer);
            }

            var linksConut = int.Parse(lines[lineNumber++]);
            for (int linkNumber = 0; linkNumber < linksConut; linkNumber++)
            {
                var linkParts = lines[lineNumber++].Split(';');
                var from = map[linkParts[0]];
                var to = map[linkParts[1]];
                var dist = double.Parse(linkParts[2]);
                var cond = int.Parse(linkParts[3]);
                var link = new LinkModel(from, to)
                {
                    Conduction = cond,
                    Distance = dist
                };
                result.Links.Add(link);
            }
            
            return result;
        }
    }
}
