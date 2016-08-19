using System.Text;
using System.Linq;
using Energy.UI.Model;

namespace Energy.UI.Serialization
{
    public class TaskModelExporter
    {
        public static string ToText(TaskModel taskModel)
        {
            var builder = new StringBuilder();

            builder.AppendLine(string.Join(",", taskModel.FeaturesModel.FeaturesNames));
            builder.AppendLine(string.Join(",", taskModel.FeaturesModel.Stations.Select(s => s.Name)));
            builder.AppendLine(string.Join(",", taskModel.FeaturesModel.Consumers.Select(c => c.Name)));
            foreach (var station in taskModel.FeaturesModel.Stations)
            {
                builder.AppendLine(station.Serialize());
            }
            foreach (var consumer in taskModel.FeaturesModel.Consumers)
            {
                builder.AppendLine(consumer.Serialize());
            }

            return builder.ToString();
        }

        //Матрица потребителей
        public static double[,] GetRMatrix(TaskModel taskModel)
        {
            var rowsCount = taskModel.FeaturesModel.Consumers.Count;
            var columnsNames = taskModel.FeaturesModel.FeaturesNames;
            var columnsCount = columnsNames.Count;
            var result = new double[rowsCount, columnsCount];

            var row = 0;
            foreach (var consumer in taskModel.FeaturesModel.Consumers)
            {
                var column = 0;
                foreach (var columnsName in columnsNames)
                {
                    result[row, column++] = consumer[columnsName];
                }
                row++;
            }

            return result;
        }

        //Матрица станций
        public static double[,] GetSMatrix(TaskModel taskModel)
        {
            var columnsCount = taskModel.FeaturesModel.Stations.Count;
            var columnsNames = taskModel.FeaturesModel.FeaturesNames;
            var rowsCount = columnsNames.Count;
            var result = new double[rowsCount, columnsCount];

            var row = 0;
            foreach (var station in taskModel.FeaturesModel.Stations)
            {
                var column = 0;
                foreach (var columnsName in columnsNames)
                {
                    result[row, column++] = station[columnsName];
                }
                row++;
            }

            return result;
        }
    }
}
