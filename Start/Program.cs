using System;
using System.IO;
using System.Linq;
using System.Text;
using Energy.Solving;

namespace Start
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Не указано имя файла");
                return;
            }

            try
            {
                var currentIndex = 0;
                var lines = File.ReadAllLines(args[0]);

                var matrixR = ReadMatrix(lines, currentIndex);
                currentIndex += matrixR.GetLength(0) + 1;

                var matrixS = ReadMatrix(lines, currentIndex);

                var answer = MainSolver.SolveSimple(matrixR, matrixS);
                WriteAnswer("output.txt", answer);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            Console.WriteLine("Finished");
        }

        private static void WriteAnswer(string fileName, int[][] answer)
        {
            var resultBuilder = new StringBuilder();
            for (var i = 0; i < answer.Length; i++)
            {
                var answerRow = string.Join(", ", answer[i].Select(e => "x" + (e + 1)));
                resultBuilder.AppendFormat("Z{0}: {1}", i + 1, answerRow);
                resultBuilder.AppendLine();
            }
            
            File.WriteAllText(fileName, resultBuilder.ToString());
        }

        private static double[,] ReadMatrix(string[] lines, int startIndex)
        {
            var line = lines[startIndex];
            var parts = line.Split(';');
            var nRows = int.Parse(parts[0]);
            var nColumns = int.Parse(parts[1]);

            var result = new double[nRows, nColumns];

            for (var rowIndex = 0; rowIndex < nRows; rowIndex++)
            {
                var lineIndex = startIndex + rowIndex + 1;
                parts = lines[lineIndex].Split(';');

                if(parts.Length != nColumns)
                    throw new InvalidDataException();

                for (var columnIndex = 0; columnIndex < nColumns; columnIndex++)
                {
                    result[rowIndex, columnIndex] = double.Parse(parts[columnIndex]);
                }
            }

            return result;
        }
    }
}
