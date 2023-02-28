using System;

namespace EpilepsieDB.Source.Helper
{
    public static class Calculator
    {
        public static float Gain(float physMax, float physMin, float digMax, float digMin)
        {
            if (digMax - digMin == 0)
                return 0;

            float res =  (physMax - physMin) / (digMax - digMin);
            return MathF.Round(res, 5);
        }

        public static float SamplingRate(float time, float samples)
        {
            if (time == 0)
                return 0;

            float res = samples / time;
            return MathF.Round(res, 2);
        }
    }
}
