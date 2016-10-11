using System;
using System.Collections.Generic;
using System.Linq;

namespace Energy.Solving
{
	public class MainSolver
	{
	    public static int[][] SolveSimple(double[,] matrixR, double[,] matrixS)
	    {
	        var matrixT = GetT(matrixR, matrixS);
	        var matrixW = GetW(matrixT);
	        var treshold = GetTreshold(matrixW);
	        return GetZones(matrixT, treshold);
	    }

	    public static SimpleTaskSolveResult Solve(SimpleTask task)
	    {
	        var result = new SimpleTaskSolveResult { MatrixR = task.MatrixR, MatrixS = task.MatrixS };
	        result.MatrixT = GetT(result.MatrixR, result.MatrixS);
	        result.MatrixW = GetW(result.MatrixT);
            result.Treshold= GetTreshold(result.MatrixW);
            var zones = GetZones(result.MatrixT, result.Treshold);
	        for(int z = 0; z < zones.Length; z++)
	        {
	            var list = new List<string>();
	            foreach (var c in zones[z])
	            {
	                list.Add(task.ConsumersList[c]);
	            }
	            result.Result[task.StationsList[z]] = list;
	        }
            return result;
	    }

	    public static TaskSolveResult Solve(Task task)
	    {
	        var result = new TaskSolveResult();

            result.SimpleTaskSolveResults.AddRange(task.SimpleTasks.Select(Solve));

	        return result;
        }

        public static double [,] GetT(double [,] R, double [,] S)
		{
			if(R.GetLength(1) != S.GetLength(0))
			{
				throw new InvalidOperationException("Некорректные размеры входных матриц");
			}
			var consumersCount = R.GetLength(0);
			var stationsCount = S.GetLength(1);
            var featuresCount = R.GetLength(1);

			var result = new double[consumersCount, stationsCount];

		    for (var xIndex = 0; xIndex < consumersCount; xIndex++)
			{
				for (var zIndex = 0; zIndex < stationsCount; zIndex++)
				{
					var nominator = 0.0;
					var denominator = 0.0;
				    for(var yIndex = 0; yIndex < featuresCount; yIndex++)
					{
						nominator += R[xIndex, yIndex] * S[yIndex, zIndex];
						denominator += R[xIndex, yIndex];
					}

					result[xIndex, zIndex] = nominator / denominator;
				}
			}

			return result;
		}

	    public static double[,] GetW(double[,] T)
	    {
	        var p = T.GetLength(1);
	        var n = T.GetLength(0);
	        var columns = Math.Max(p * (p - 1)/2, 1);
	        var result = new double[T.GetLength(0), columns];

	        for (var k = 0; k < n; k++)
            {
                var l = 0;
                for (var i = 0; i < p - 1; i++)
	            {
	                for (var j = i + 1; j < p; j++)
	                {
	                    result[k, l++] = Math.Min(T[k, i], T[k, j]);
	                }
	            }
	        }

	        return result;
	    }

	    public static double GetTreshold(double[,] input)
	    {
	        var maximums = new double[input.GetLength(1)];

	        for (var i = 0; i < input.GetLength(0); i++)
	        {
	            for (var j = 0; j < input.GetLength(1); j++)
	            {
	                maximums[j] = Math.Max(maximums[j], input[i, j]);
	            }
	        }
            
            var upperTreshold = maximums.Min();
	        var tresholdValue = double.MinValue;
            for (var i = 0; i < input.GetLength(0); i++)
	        {
	            for (var j = 0; j < input.GetLength(1); j++)
	            {
	                if (input[i, j] >= upperTreshold || tresholdValue >= input[i, j])
	                    continue;
                    
	                tresholdValue = input[i, j];
	            }
	        }

	        return tresholdValue != double.MinValue ? tresholdValue : upperTreshold;
	    }

	    public static int[][] GetZones(double[,] input, double treshold)
	    {
	        var result = new List<HashSet<int>>(input.GetLength(1));
	        for (var i = 0; i < input.GetLength(1); i++)
	            result.Add(new HashSet<int>());

	        for (var i = 0; i < input.GetLength(1); i++)
	        {
	            for (var j = 0; j < input.GetLength(0); j++)
	            {
	                if (input[j, i] < treshold)
                        continue;

	                result[i].Add(j);
	            }
	        }

	        return result.Select(x => x.ToArray()).ToArray();
	    }
	}
}
