using System.Collections.Generic;
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

        private static void PrintZones(IList<string> stationNames, IList<string> consumersNames, Dictionary<string, List<string>> zonesDescription, StringBuilder builder)
        {
            builder.AppendLine("По результатам расчетов перспектив развития рассматриваемой " +
                               "электроэнергетической системы выявлено, что для максимального обеспечения\n" +
                               "условий и целей данной задачи и принятия наиболее рационального решения " +
                               "рекомендуется следующее распределение потребителей по зонам:").AppendLine();

            foreach (var zone in zonesDescription)
            {
                builder.AppendLine($"К зоне электростанции {zone.Key} рекомендуется отнести:");
                var answerRow = string.Join(", ", zone.Value);
                builder.AppendLine($"\t{answerRow}");
                builder.AppendLine();
            }
        }

        public static string PrintTaskSolveResult(IList<string> stationNames, IList<string> consumersNames,
            TaskSolveResult result)
        {
            var resultBuilder = new StringBuilder();
            foreach (var solveResult in result.SimpleTaskSolveResults)
            {
                PrintSimpleTaskSolveResult(stationNames, consumersNames, solveResult, resultBuilder);
            }
            return resultBuilder.ToString();
        }

        private static void PrintSimpleTaskSolveResult(IList<string> stationNames, IList<string> consumersNames, SimpleTaskSolveResult result, StringBuilder builder)
        {
            PrintMatrix("R", result.MatrixR, builder);
            builder.AppendLine();
            PrintMatrix("S", result.MatrixS, builder);
            builder.AppendLine();
            PrintMatrix("T", result.MatrixT, builder);
            builder.AppendLine();
            PrintMatrix("W", result.MatrixW, builder);
            builder.AppendLine();
            PrintZones(stationNames, consumersNames, result.Result, builder);
        }
    }
}
