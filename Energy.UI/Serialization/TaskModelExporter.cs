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

            builder.AppendLine(string.Join(",", taskModel.FeaturesNames));
            builder.AppendLine(string.Join(",", taskModel.Stations.Select(s => s.Name)));
            builder.AppendLine(string.Join(",", taskModel.Consumers.Select(c => c.Name)));
            foreach (var station in taskModel.Stations)
            {
                builder.AppendLine(station.Serialize());
            }
            foreach (var consumer in taskModel.Consumers)
            {
                builder.AppendLine(consumer.Serialize());
            }

            return builder.ToString();
        }

        //Матрица потребителей
        public static double[,] GetRMatrix(TaskModel taskModel)
        {
            var rowsCount = taskModel.Consumers.Count;
            var columnsNames = taskModel.FeaturesNames;
            var columnsCount = columnsNames.Count;
            var result = new double[rowsCount, columnsCount];

            var row = 0;
            foreach (var consumer in taskModel.Consumers)
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
            var columnsCount = taskModel.Stations.Count;
            var columnsNames = taskModel.FeaturesNames;
            var rowsCount = columnsNames.Count;
            var result = new double[rowsCount, columnsCount];

            var row = 0;
            foreach (var station in taskModel.Stations)
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
