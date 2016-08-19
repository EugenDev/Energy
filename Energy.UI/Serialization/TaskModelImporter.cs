using System.Globalization;
using System.Linq;
using Energy.UI.Model;

namespace Energy.UI.Serialization
{
    public static class TaskModelImporter
    {
        public static TaskModel CreateFromText(string[] lines)
        {
            var result = new TaskModel();
            var lineNumber = 0;
            var commonFeaturesNames = lines[lineNumber++].Split(',');
            var stationsNames = lines[lineNumber++].Split(',');
            var consumersNames = lines[lineNumber++].Split(',');

            foreach (var stationName in stationsNames)
                result.FeaturesModel.AddStation(stationName);

            foreach (var consumerName in consumersNames)
                result.FeaturesModel.AddConsumer(consumerName);

            foreach (var featureName in commonFeaturesNames)
                result.FeaturesModel.AddFeature(featureName);

            var stationsCount = stationsNames.Length;
            for (var i = 0; i < stationsCount; i++)
            {
                var columnIndex = 0;
                var stationData = lines[lineNumber++].Split(',').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
                var station = result.FeaturesModel.Stations[i];
                foreach (var featureName in result.FeaturesModel.FeaturesNames)
                    station[featureName] = stationData[columnIndex++];
                foreach (var featureName in result.FeaturesModel.FeaturesNames)
                    station[featureName] = stationData[columnIndex++];
            }

            var consumersCount = consumersNames.Length;
            for (var i = 0; i < consumersCount; i++)
            {
                var columnIndex = 0;
                var consumerData = lines[lineNumber++].Split(',').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
                var consumer = result.FeaturesModel.Consumers[i];
                foreach (var featureName in result.FeaturesModel.FeaturesNames)
                    consumer[featureName] = consumerData[columnIndex++];
                foreach (var featureName in result.FeaturesModel.FeaturesNames)
                    consumer[featureName] = consumerData[columnIndex++];
            }

            return result;
        }
    }
}
