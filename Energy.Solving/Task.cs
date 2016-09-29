using System.Collections.Generic;

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

        public SimpleTask(List<string> stationsList, List<string> consumersList, double[,] matrixR, double[,] matrixS)
        {
            StationsList = stationsList;
            ConsumersList = consumersList;
            MatrixR = matrixR;
            MatrixS = matrixS;
        }
    }
}
