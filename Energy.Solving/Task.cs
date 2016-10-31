using System.Collections.Generic;
using Energy.Solving.PathFinding;

namespace Energy.Solving
{
    public class Task
    {
        public List<SimpleTask> SimpleTasks { get; }

        public Task()
        {
            SimpleTasks = new List<SimpleTask>();
        }
    }

    public class SimpleTask
    {
        public List<string> StationsList { get; set; }
        public List<string> ConsumersList { get; set; }
        public double[,] MatrixR { get; }
        public double[,] MatrixS { get; }

        public Dictionary<string, double> ConsumersDistanceDemand { get; set; }
        public Dictionary<string, double> ConsumersConductionDemand { get; set; }

        public Graph Graph { get; set; }

        public SimpleTask(List<string> stationsList, List<string> consumersList, double[,] matrixR, double[,] matrixS)
        {
            StationsList = stationsList;
            ConsumersList = consumersList;
            MatrixR = matrixR;
            MatrixS = matrixS;
        }

        public bool HasCommonFeatures => MatrixR.GetLength(0) != 0 && MatrixR.GetLength(1) != 0 && MatrixS.GetLength(0) != 0 && MatrixS.GetLength(1) != 0;
    }
}
