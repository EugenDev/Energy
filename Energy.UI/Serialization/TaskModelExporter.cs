using System.Text;
using System.Linq;
using Energy.UI.Model;

namespace Energy.UI.Serialization
{
    public class TaskModelExporter
    {
        public static string ToText(TaskModel taskModel)
        {
            var builder = new StringBuilder(); //TODO: Не экспортировать проводимость и расстояние
            builder.AppendLine(string.Join(";", taskModel.FeaturesNames));

            builder.AppendLine(taskModel.Stations.Count.ToString());
            foreach (var station in taskModel.Stations)
            {
                var featuresValues = string.Join(";", station.Select(f => station[f.Key]));
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

            var row = 0;
            foreach (var station in taskModel.Stations)
            {
                var column = 0;
                foreach (var columnsName in taskModel.FeaturesNames)
                {
                    result[row, column++] = station[columnsName];
                }
                row++;
            }

            return result;
        }
    }
}
