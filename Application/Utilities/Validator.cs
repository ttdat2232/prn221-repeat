namespace Application.Utilities
{
    public static class Validator
    {
        public static bool IsTimeBetween(DateTime time, DateTime startTime, DateTime endTime)
        {
            if (time.TimeOfDay == startTime.TimeOfDay || time.TimeOfDay == endTime.TimeOfDay) return true;
            if (startTime.TimeOfDay <= endTime.TimeOfDay)
                return (time.TimeOfDay >= startTime.TimeOfDay && time.TimeOfDay <= endTime.TimeOfDay);
            else
                return !(time.TimeOfDay >= endTime.TimeOfDay && time.TimeOfDay <= startTime.TimeOfDay);
        }

        public static bool IsTimeBetween(DateTime targetStartTime, DateTime targetEndTime, DateTime startTime, DateTime endTime)
        {
            if (targetStartTime >= startTime && targetStartTime <= endTime && targetEndTime >= startTime && targetEndTime <= endTime)
                return true;
            return false;
        }

        public static bool IsTimeConflict(DateTime targetStartTime, DateTime targetEndTime, DateTime startTime, DateTime endTime)
        {
            if (IsTimeBetween(targetStartTime, targetEndTime, startTime, endTime))
                return true;
            if (targetStartTime < startTime)
                return targetEndTime >= startTime && targetEndTime <= endTime;
            if (targetEndTime > endTime)
                return targetStartTime >= startTime && targetStartTime <= endTime;
            return false;
        }
    }
}
