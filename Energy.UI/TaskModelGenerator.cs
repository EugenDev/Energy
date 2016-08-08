using System;

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
                result.AddCommonFeature("Feature_" + i);
            }
            foreach (var station in result.Stations)
            {
                foreach (var stationFeature in result.StationsFeaturesNames)
                    station[stationFeature] = Math.Round(r.NextDouble(), 1);
                foreach (var commonFeature in result.CommonFeatureNames)
                    station[commonFeature] = Math.Round(r.NextDouble(), 1);
            }
            foreach (var consumer in result.Consumers)
            {
                foreach (var consumerFeature in result.ConsumersFeaturesNames)
                    consumer[consumerFeature] = Math.Round(r.NextDouble(), 1);
                foreach (var commonFeature in result.CommonFeatureNames)
                    consumer[commonFeature] = Math.Round(r.NextDouble(), 1);
            }
            return result;
        }
    }
}
