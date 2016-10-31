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

        public Dictionary<string, List<string>> GetCombinedResult(CombineType combineType)
        {
            return SolveResultCombiner.CombineSolveResults(this, combineType);
        }
    }
    
    public class SimpleTaskSolveResult
    {
        public double[,] MatrixR { get; set; }

        public double[,] MatrixS { get; set; }

        public double[,] MatrixT { get; set; }

        public double[,] MatrixW { get; set; }

        public double Treshold { get; set; }

        public Dictionary<string, List<string>> CommonResult { get; set; }
        public Dictionary<string, List<string>> DistanceResult { get; set; }
        public Dictionary<string, List<string>> ConductionResult { get; set; }

        public SimpleTaskSolveResult()
        {
            CommonResult = new Dictionary<string, List<string>>();
            DistanceResult = new Dictionary<string, List<string>>();
            ConductionResult = new Dictionary<string, List<string>>();
        }
    }
}
