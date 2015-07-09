using SimpleBet.Ultilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleBet.Test.Ultilities
{
    public class TimeUltilityTest
    {
        [Fact]
        public void IsTimeout_Not()
        {
            DateTime startTime = DateTime.Now;
            int duration = 100;
            bool isTimeout = TimeUltility.isTimeout(startTime, duration);
            Assert.Equal(false, isTimeout);
        }

        [Fact]
        public void IsTimeout_Passed()
        {
            DateTime startTime = DateTime.UtcNow;
            int duration = -1;
            bool isTimeout = TimeUltility.isTimeout(startTime, duration);
            Assert.Equal(true, isTimeout);
        }

    }
}
