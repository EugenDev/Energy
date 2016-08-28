﻿using Energy.Solving;
using NUnit.Framework;

namespace Energy.Tests
{
	[TestFixture]
    public class MainTest
    {
		private const double Tolerance = 0.001;

		[Test, Ignore("Нахер")]
		public void TestGetT1()
		{
			var inputR = new[,]
			{
				{1.0, 0.4, 0.8, 0.5},
				{1.0, 0.8, 1.0, 0.5},
				{1.0, 0.2, 0.2, 0.8},
				{1.0, 1.0, 0.5, 0.5}
			};

			var inputS = new[,]
			{
				{0.8, 0.2, 0.1, 0.5},
				{0.5, 0.9, 0.6, 0.6},
				{0.4, 0.9, 0.5, 0.4},
				{0.8, 0.1, 0.5, 0.6}
			};

			var output = new[,]
			{
				{0.348, 0.641, 0.348, 0.300},
				{0.454, 0.618, 0.418, 0.369},
				{0.318, 0.495, 0.318, 0.213},
				{0.483, 0.516 ,0.483, 0.400}
			};

			Assert.That(MainSolver.GetT(inputR, inputS), Is.EqualTo(output).Within(Tolerance));
		}

        [Test]
        public void TestGetT2()
        {
            var inputR = new[,]
            {
                {1.0, 0.0, 0.0, 0.0},
                {0.0, 1.0, 0.0, 0.0},
                {0.0, 0.0, 1.0, 0.0},
                {0.0, 0.0, 0.0, 1.0},
                {1.0, 1.0, 1.0, 1.0},
                {0.8, 0.4, 0.5, 0.9},
                {0.7, 0.3, 0.4, 0.8},
                {0.5, 0.8, 0.8, 0.2},
                {0.5, 0.5, 0.5, 0.5},
                {0.6, 0.7, 0.8, 0.5},
                {0.1, 0.1, 0.1, 0.1},
                {0.0, 0.0, 1.0, 1.0}
            };

            var inputS = new[,]
            {
                {0.9, 0.1, 0.5, 0.7},
                {0.5, 0.9, 0.6, 0.6},
                {0.4, 0.9, 0.5, 0.4},
                {0.8, 0.1, 0.5, 0.6}
            };

            var output = new[,]
            {
                {0.900, 0.100, 0.500, 0.700},
                {0.500, 0.900, 0.600, 0.600},
                {0.400, 0.900, 0.500, 0.400},
                {0.800, 0.100, 0.500, 0.600},
                {0.650, 0.500, 0.525, 0.575},
                {0.708, 0.377, 0.515, 0.592},
                {0.718, 0.355, 0.514, 0.595},
                {0.578, 0.657, 0.535, 0.552},
                {0.650, 0.500, 0.525, 0.575},
                {0.619, 0.562, 0.527, 0.562},
                {0.650, 0.500, 0.525, 0.575},
                {0.600, 0.500, 0.500, 0.500}
            };

            Assert.That(MainSolver.GetT(inputR, inputS), Is.EqualTo(output).Within(Tolerance));
        }

        [Test]
	    public void TestGetW()
	    {
            var inputT = new[,]
            {
                {0.900, 0.100, 0.500, 0.700},
                {0.500, 0.900, 0.600, 0.600},
                {0.400, 0.900, 0.500, 0.400},
                {0.800, 0.100, 0.500, 0.600},
                {0.650, 0.500, 0.525, 0.575},
                {0.708, 0.377, 0.515, 0.592},
                {0.718, 0.355, 0.514, 0.595},
                {0.578, 0.657, 0.535, 0.552},
                {0.650, 0.500, 0.525, 0.575},
                {0.619, 0.562, 0.527, 0.562},
                {0.650, 0.500, 0.525, 0.575},
                {0.600, 0.500, 0.500, 0.500}
            };

            var expectedOutput = new[,]
            {
                {0.100, 0.500, 0.700, 0.100, 0.100, 0.500},
                {0.500, 0.500, 0.500, 0.600, 0.600, 0.600},
                {0.400, 0.400, 0.400, 0.500, 0.400, 0.400},
                {0.100, 0.500, 0.600, 0.100, 0.100, 0.500},
                {0.500, 0.525, 0.575, 0.500, 0.500, 0.525},
                {0.377, 0.515, 0.592, 0.377, 0.377, 0.515},
                {0.355, 0.514, 0.595, 0.355, 0.355, 0.514},
                {0.578, 0.535, 0.552, 0.535, 0.552, 0.535},
                {0.500, 0.525, 0.575, 0.500, 0.500, 0.525},
                {0.562, 0.527, 0.562, 0.527, 0.562, 0.527},
                {0.500, 0.525, 0.575, 0.500, 0.500, 0.525},
                {0.500, 0.500, 0.500, 0.500, 0.500, 0.500}
            };

            Assert.That(MainSolver.GetW(inputT), Is.EqualTo(expectedOutput).Within(Tolerance));
        }

        [Test]
	    public void TestGetUpperTreshold()
	    {
            var input = new[,]
            {
                {0.100, 0.500, 0.700, 0.100, 0.100, 0.500},
                {0.500, 0.500, 0.500, 0.600, 0.600, 0.600},
                {0.400, 0.400, 0.400, 0.500, 0.400, 0.400},
                {0.100, 0.500, 0.600, 0.100, 0.100, 0.500},
                {0.500, 0.525, 0.575, 0.500, 0.500, 0.525},
                {0.377, 0.515, 0.592, 0.377, 0.377, 0.515},
                {0.355, 0.514, 0.595, 0.355, 0.355, 0.514},
                {0.578, 0.535, 0.552, 0.535, 0.552, 0.535},
                {0.500, 0.525, 0.575, 0.500, 0.500, 0.525},
                {0.562, 0.527, 0.562, 0.527, 0.562, 0.527},
                {0.500, 0.525, 0.575, 0.500, 0.500, 0.525},
                {0.500, 0.500, 0.500, 0.500, 0.500, 0.500}
            };

            Assert.That(MainSolver.GetTreshold(input), Is.EqualTo(0.527).Within(Tolerance));
        }

	    [Test]
	    public void TestGetZones()
	    {
            var inputT = new[,]
            {
                {0.900, 0.100, 0.500, 0.700},
                {0.500, 0.900, 0.600, 0.600},
                {0.400, 0.900, 0.500, 0.400},
                {0.800, 0.100, 0.500, 0.600},
                {0.650, 0.500, 0.525, 0.575},
                {0.708, 0.377, 0.515, 0.592},
                {0.718, 0.355, 0.514, 0.595},
                {0.578, 0.657, 0.535, 0.552},
                {0.650, 0.500, 0.525, 0.575},
                {0.619, 0.562, 0.527, 0.562},
                {0.650, 0.500, 0.525, 0.575},
                {0.600, 0.500, 0.500, 0.500}
            };

	        var expectedResult = new[]
	        {
	            new[] {0, 3, 4, 5, 6, 7, 8, 9, 10, 11},
	            new[] {1, 2, 7, 9},
	            new[] {1, 7, 9},
	            new[] {0, 1, 3, 4, 5, 6, 7, 8, 9, 10}
	        };

            Assert.That(MainSolver.GetZones(inputT, 0.527), Is.EqualTo(expectedResult));
        }

        [Test]
	    public void ComplexTest()
	    {
            var inputR = new[,]
            {
                {1.0, 0.0, 0.0, 0.0},
                {0.0, 1.0, 0.0, 0.0},
                {0.0, 0.0, 1.0, 0.0},
                {0.0, 0.0, 0.0, 1.0},
                {1.0, 1.0, 1.0, 1.0},
                {0.8, 0.4, 0.5, 0.9},
                {0.7, 0.3, 0.4, 0.8},
                {0.5, 0.8, 0.8, 0.2},
                {0.5, 0.5, 0.5, 0.5},
                {0.6, 0.7, 0.8, 0.5},
                {0.1, 0.1, 0.1, 0.1},
                {0.0, 0.0, 1.0, 1.0}
            };

            var inputS = new[,]
            {
                {0.9, 0.1, 0.5, 0.7},
                {0.5, 0.9, 0.6, 0.6},
                {0.4, 0.9, 0.5, 0.4},
                {0.8, 0.1, 0.5, 0.6}
            };

            var expectedResult = new[]
            {
                new[] {0, 3, 4, 5, 6, 7, 8, 9, 10, 11},
                new[] {1, 2, 7, 9},
                new[] {1, 7, 9},
                new[] {0, 1, 3, 4, 5, 6, 7, 8, 9, 10}
            };

            Assert.That(MainSolver.Solve(inputR, inputS), Is.EqualTo(expectedResult));
        }
    }
}
