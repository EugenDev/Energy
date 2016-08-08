﻿using System.Globalization;
using System.Linq;

namespace Energy.UI.Serialization
{
    public static class TaskModelImporter
    {
        public static TaskModel CreateFromText(string[] lines)
        {
            var result = new TaskModel();
            var lineNumber = 0;
            var commonFeaturesNames = lines[lineNumber++].Split(',');
            //var stationFeaturesNames = lines[lineNumber++].Split(',');
            //var consumersFeaturesNames = lines[lineNumber++].Split(',');
            var stationsNames = lines[lineNumber++].Split(',');
            var consumersNames = lines[lineNumber++].Split(',');

            foreach (var stationName in stationsNames)
                result.AddStation(stationName);

            foreach (var consumerName in consumersNames)
                result.AddConsumer(consumerName);

            foreach (var featureName in commonFeaturesNames)
                result.AddCommonFeature(featureName);

            var stationsCount = stationsNames.Length;
            for (var i = 0; i < stationsCount; i++)
            {
                var columnIndex = 0;
                var stationData = lines[lineNumber++].Split(',').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
                var station = result.Stations[i];
                foreach (var featureName in result.StationsFeaturesNames)
                    station[featureName] = stationData[columnIndex++];
                foreach (var featureName in result.CommonFeatureNames)
                    station[featureName] = stationData[columnIndex++];
            }

            var consumersCount = consumersNames.Length;
            for (var i = 0; i < consumersCount; i++)
            {
                var columnIndex = 0;
                var consumerData = lines[lineNumber++].Split(',').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
                var consumer = result.Consumers[i];
                foreach (var featureName in result.ConsumersFeaturesNames)
                    consumer[featureName] = consumerData[columnIndex++];
                foreach (var featureName in result.CommonFeatureNames)
                    consumer[featureName] = consumerData[columnIndex++];
            }

            return result;
        }
    }
}
