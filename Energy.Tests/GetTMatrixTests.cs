using Energy.Solving;
using NUnit.Framework;

namespace Energy.Tests
{
    public class GetTMatrixTests
    {
        [Test]
        public void SamePartisipantsCountTest()
        {
            var rMatrix = new[,]
            {
                { 0.1, 0.2, 0.7 },
                { 0.8, 0.9, 0.1 },
                { 0.6,   1, 0.1 }
            };

            var sMatrix = new[,]
            {
                { 0.3, 0.7, 0.9 },
                { 0.5, 0.6,   1 },
                {   1, 0.4, 0.2 }
            };

            var expected = new[,]
            {
                { 0.830, 0.470, 0.430 },
                { 0.438, 0.633, 0.911 },
                { 0.459, 0.623, 0.917 }
            };

            var result = MainSolver.GetT(rMatrix, sMatrix);
            Assert.That(result, Is.EqualTo(expected).Within(SolverTestConstants.Tolerance));
        }

        [Test]
        public void MoreStationsTest()
        {
            var rMatrix = new[,]
            {
                { 0.1, 0.2, 0.7 },
                { 0.8, 0.9, 0.1 },
                { 0.6,   1, 0.1 }
            };

            var sMatrix = new[,]
            {
                { 0.3, 0.7, 0.9, 0.2 },
                { 0.5, 0.6,   1, 0.6 },
                {   1, 0.4, 0.2, 0.4 }
            };

            var expected = new[,]
            {
                { 0.830, 0.470, 0.430, 0.420 },
                { 0.438, 0.633, 0.911, 0.411 },
                { 0.459, 0.623, 0.917, 0.447 }
            };

            var result = MainSolver.GetT(rMatrix, sMatrix);
            Assert.That(result, Is.EqualTo(expected).Within(SolverTestConstants.Tolerance));
        }


        [Test]
        public void MoreConsumersTest()
        {
            var rMatrix = new[,]
            {
                { 0.1, 0.2, 0.7 },
                { 0.8, 0.9, 0.1 },
                { 0.6,   1, 0.1 },
                { 0.2, 0.6, 0.4 }
            };

            var sMatrix = new[,]
            {
                { 0.3, 0.7, 0.9 },
                { 0.5, 0.6,   1 },
                {   1, 0.4, 0.2 }
            };

            var expected = new[,]
            {
                { 0.830, 0.470, 0.430 },
                { 0.438, 0.633, 0.911 },
                { 0.459, 0.623, 0.917 },
                { 0.633, 0.550, 0.716 }
            };

            var result = MainSolver.GetT(rMatrix, sMatrix);
            Assert.That(result, Is.EqualTo(expected).Within(SolverTestConstants.Tolerance));
        }
    }
}
