using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Solving
{
    public static class SolveResultPrinter
    {
        private static void PrintMatrix(string name, double[,] matrix, StringBuilder builder)
        {
            builder.AppendLine($"Матрица {name}");
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    builder.AppendFormat("{0:F2} ", matrix[i, j]);
                builder.AppendLine();
            }
        }

        private static void PrintZones(IList<string> stationNames, IList<string> consumersNames, int[][] zonesDescription, StringBuilder builder)
        {
            builder.AppendLine("По результатам расчетов перспектив развития рассматриваемой " +
                               "электроэнергетической системы выявлено, что для максимального обеспечения\n" +
                               "условий и целей данной задачи и принятия наиболее рационального решения " +
                               "рекомендуется следующее распределение потребителей по зонам:").AppendLine();

            for (var i = 0; i < zonesDescription.Length; i++)
            {
                builder.AppendLine($"К зоне электростанции {stationNames[i]} рекомендуется отнести:");
                var answerRow = string.Join(", ", zonesDescription[i].Select(e => consumersNames[e]));
                builder.AppendLine($"\t{answerRow}");
                builder.AppendLine();
            }
        }

        public static string PrintTaskSolveResult(IList<string> stationNames, IList<string> consumersNames, TaskSolveResult result)
        {
            var builder = new StringBuilder();
            PrintMatrix("R", result.MatrixR, builder);
            builder.AppendLine();
            PrintMatrix("S", result.MatrixS, builder);
            builder.AppendLine();
            PrintMatrix("T", result.MatrixT, builder);
            builder.AppendLine();
            PrintMatrix("W", result.MatrixW, builder);
            builder.AppendLine();
            PrintZones(stationNames, consumersNames, result.Result, builder);
            return builder.ToString();
        }
    }
}
