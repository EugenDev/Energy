using System;
using Energy.UI.Model;

namespace Energy.UI
{
    public class TaskModelGenerator
    {
        public static TaskModel GenerateRandom()
        {
            var r = new Random(DateTime.Now.Millisecond);
            var result = new TaskModel();
            var stationsCount = r.Next(3, 7);
            var customersCount = r.Next(2, 5);
            var commonFeaturesCount = r.Next(1, 6);

            for (int i = 0; i < stationsCount; i++)
            {
                result.AddStation("Station_" + i);
            }
            for (int i = 0; i < customersCount; i++)
            {
                result.AddConsumer("Consumer" + i);
            }
            for (int i = 0; i < commonFeaturesCount; i++)
            {
                result.AddFeature("Feature_" + i);
            }
            foreach (var station in result.Stations)
            {
                foreach (var stationFeature in result.FeaturesNames)
                    station[stationFeature] = Math.Round(r.NextDouble(), 1);
                foreach (var commonFeature in result.FeaturesNames)
                    station[commonFeature] = Math.Round(r.NextDouble(), 1);
            }
            foreach (var consumer in result.Consumers)
            {
                foreach (var consumerFeature in result.FeaturesNames)
                    consumer[consumerFeature] = Math.Round(r.NextDouble(), 1);
                foreach (var commonFeature in result.FeaturesNames)
                    consumer[commonFeature] = Math.Round(r.NextDouble(), 1);
            }
            return result;
        }
    }
}
