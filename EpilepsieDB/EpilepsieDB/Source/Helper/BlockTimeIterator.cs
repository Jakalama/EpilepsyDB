using EpilepsieDB.Services.Impl;
using System;

namespace EpilepsieDB.Source.Helper
{
    public class BlockTimeIterator
    {
        private DateTime startTime;
        private DateTime currentTimeStep;
        private DateTime endTime;

        private float duration;

        private bool isInterupted;
        private float gap;

        private bool isfirstStep;

        public BlockTimeIterator(DateTime startTime, DateTime endTime, float duration, bool isInterupted)
        {
            this.startTime = startTime;
            this.currentTimeStep = startTime;
            this.endTime = endTime;

            this.duration = duration;

            this.isInterupted = isInterupted;
            this.gap = 0;

            this.isfirstStep = true;
        }

        public TimeInfo Next(float offset)
        {
            if (isfirstStep)
            {
                isfirstStep = false;
                return GetTimeInfo();
            }

            if (isInterupted)
                NextInterupted(offset);
            else
                NextUninterupted();

            return GetTimeInfo();
        }

        private TimeInfo GetTimeInfo()
        {
            return new TimeInfo()
            {
                StartTime = DateTime.SpecifyKind(currentTimeStep, DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(endTime, DateTimeKind.Utc),
                Gap = gap
            };
        }

        private void NextInterupted(float offset)
        {
            currentTimeStep = startTime.AddSeconds(offset);
            gap = (float)(currentTimeStep - endTime).TotalSeconds;
            endTime = currentTimeStep.AddSeconds(duration);
        }

        private void NextUninterupted()
        {
            currentTimeStep = endTime;
            endTime = currentTimeStep.AddSeconds(duration);
            gap = 0f;
        }

        
    }

    
}
