namespace Energy.Solving
{
    public class TaskSolveResult
    {
        public double[,] MatrixR { get; set; }

        public double[,] MatrixS { get; set; }

        public double[,] MatrixT { get; set; }

        public double[,] MatrixW { get; set; }

        public double Treshold { get; set; }

        public int[][] Result { get; set; }
    }
}
