using System;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            float number1 = 4;
            float number2 = 10;
            float sum = 14;
            float testSum = Sum(number1, number2);
            Assert.Equal(sum, testSum);

        }

        public float Sum(float a, float b) {
            return a + b;
        }

        
    }
}
