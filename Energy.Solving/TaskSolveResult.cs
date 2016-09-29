using System.Collections.Generic;
using System.Linq;

namespace Energy.Solving
{
    public class TaskSolveResult
    {
        public List<SimpleTaskSolveResult> SimpleTaskSolveResults { get; }

        public TaskSolveResult()
        {
            SimpleTaskSolveResults = new List<SimpleTaskSolveResult>();
        }

        public Dictionary<string, List<string>> GetCombinedResult()
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var solveResult in SimpleTaskSolveResults)
            {
                foreach (var pair in solveResult.Result)
                {
                    result.Add(pair.Key, pair.Value);
                }
            }

            return result;
        }
    }

    public class SimpleTaskSolveResult
    {
        public double[,] MatrixR { get; set; }

        public double[,] MatrixS { get; set; }

        public double[,] MatrixT { get; set; }

        public double[,] MatrixW { get; set; }

        public double Treshold { get; set; }

        public Dictionary<string, List<string>> Result { get; set; }

        public SimpleTaskSolveResult()
        {
            Result = new Dictionary<string, List<string>>();
        }
    }
}
