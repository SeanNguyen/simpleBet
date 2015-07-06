using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Ultilities
{
    public class TimeUltility
    {
        public static bool isTimeout(DateTime startTime, int durationMinute)
        {
            TimeSpan passedTime = DateTime.UtcNow.Subtract(startTime);
            double passTimeMinute = passedTime.TotalMinutes;
            bool isPassed = durationMinute < passTimeMinute;
            return isPassed;
        }
    }
}
