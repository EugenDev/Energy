using System;
using System.Collections.Generic;
using System.Linq;

namespace Energy.Solving
{
    public enum CombineType
    {
        Union,
        Intersection
    }

    public class SolveResultCombiner
    {
        public static Dictionary<string, List<string>> CombineSolveResults(TaskSolveResult taskSolveResult, CombineType combineType)
        {
            switch (combineType)
            {
                case CombineType.Union:
                    return UnionCombination(taskSolveResult);

                case CombineType.Intersection:
                    return IntersectionCombination(taskSolveResult);

                default:
                    throw new ArgumentOutOfRangeException(nameof(combineType), combineType, "Неизвестный тип комбинирования");
            }
        }

        private static Dictionary<string, List<string>> UnionCombination(TaskSolveResult taskSolveResult)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var simpleTaskSolveResult in taskSolveResult.SimpleTaskSolveResults)
            {
                AddResult(result, simpleTaskSolveResult.DistanceResult);
                AddResult(result, simpleTaskSolveResult.ConductionResult);
                AddResult(result, simpleTaskSolveResult.CommonResult);
            }

            return result;
        }


        private static Dictionary<string, List<string>> IntersectionCombination(TaskSolveResult taskSolveResult)
        {
            var result = new Dictionary<string, List<string>>();

            foreach (var simpleTaskSolveResult in taskSolveResult.SimpleTaskSolveResults)
            {
                var intersectedSimpleResult = new Dictionary<string, List<string>>();

                foreach (var station in simpleTaskSolveResult.DistanceResult.Keys)
                {
                    var distanceHs = GetHasSet(simpleTaskSolveResult.DistanceResult, station);
                    var consuctionHs = GetHasSet(simpleTaskSolveResult.ConductionResult, station);
                    var commonHs = GetHasSet(simpleTaskSolveResult.CommonResult, station);

                    distanceHs.IntersectWith(consuctionHs);
                    distanceHs.IntersectWith(commonHs);

                    intersectedSimpleResult[station] = distanceHs.ToList();
                }

                AddResult(result, intersectedSimpleResult);
            }

            return result;
        }


        private static HashSet<string> GetHasSet(Dictionary<string, List<string>> source, string station)
        {
            var result = new HashSet<string>();

            if (!source.ContainsKey(station))
                return result;

            foreach (var item in source[station])
                result.Add(item);

            return result;
        }


        private static void AddResult(Dictionary<string, List<string>> result, Dictionary<string, List<string>> other)
        {
            foreach (var pair in other)
            {
                if (!result.ContainsKey(pair.Key))
                    result[pair.Key] = new List<string>();
                result[pair.Key].AddRange(pair.Value);
            }
        }
    }
}
