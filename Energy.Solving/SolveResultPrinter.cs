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
        
        private static void PrintGraphResult(StringBuilder builder, Dictionary<string, List<string>> result)
        {
            foreach (var name in result)
            {
                builder.AppendLine($"К зоне электростанции {name.Key} рекомендуется отнести:");
                var answerRow = string.Join(", ", name.Value);
                builder.AppendLine($"\t{answerRow}");
            }
        }

        public static string PrintTaskSolveResult(IList<string> stationNames, IList<string> consumersNames,
            TaskSolveResult result, CombineType combineType, bool fullPrint = true)
        {
            var resultBuilder = new StringBuilder();
            var zoneNumber = 1;
            foreach (var solveResult in result.SimpleTaskSolveResults)
            {
                PrintSimpleTaskSolveResult(stationNames, consumersNames, solveResult, resultBuilder, zoneNumber++, fullPrint);
                resultBuilder.AppendLine("\n");
            }
            
            resultBuilder.AppendLine("По результатам расчетов перспектив развития рассматриваемой " +
                               "электроэнергетической системы выявлено, что для максимального обеспечения\n" +
                               "условий и целей данной задачи и принятия наиболее рационального решения " +
                               "рекомендуется следующее распределение потребителей по зонам:");
            PrintGraphResult(resultBuilder, SolveResultCombiner.CombineSolveResults(result, combineType));
            return resultBuilder.ToString();
        }

        private static void PrintSimpleTaskSolveResult(IList<string> stationNames, IList<string> consumersNames, SimpleTaskSolveResult result, StringBuilder builder, int zoneNumber, bool fullPrint)
        {
            builder.AppendLine("Зона " + zoneNumber);
            if (fullPrint)
            {
                PrintMatrix("R", result.MatrixR, builder);
                builder.AppendLine();
                PrintMatrix("S", result.MatrixS, builder);
                builder.AppendLine();
                PrintMatrix("T", result.MatrixT, builder);
                builder.AppendLine();
                PrintMatrix("W", result.MatrixW, builder);
                builder.AppendLine();
                builder.AppendLine($"Порог различения {result.Treshold}");
                builder.AppendLine();
            }
            builder.AppendLine("Разнесение по свойствам:");
            PrintGraphResult(builder, result.CommonResult);
            builder.AppendLine("Разнесение по расстояниям:");
            PrintGraphResult(builder, result.DistanceResult);
            builder.AppendLine("Разнесение по проводимости:");
            PrintGraphResult(builder, result.ConductionResult);
        }
    }
}
