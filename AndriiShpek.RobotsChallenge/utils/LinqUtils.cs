using System;
using System.Collections.Generic;
using System.Linq;

namespace Robot.Common
{
    public static class LinqUtils
    {
        public static T MinBy<T>(this IList<T> list, Func<T, int> selector)
        {
            int minValue = int.MaxValue;
            T minT = default(T);

            //note if you implement this for List<T>, don't use foreach, it is slower. Use a for loop.
            foreach (T t in list)
            {
                var value = selector.Invoke(t);
                if (value < minValue)
                {
                    minT = t;
                    minValue = value;
                }
            }
            return minT;
        }

        public static T MinBy<T>(this IList<T> list, Func<T, float> selector)
        {
            float minValue = float.MaxValue;
            T minT = default(T);

            //note if you implement this for List<T>, don't use foreach, it is slower. Use a for loop.
            foreach (T t in list)
            {
                var value = selector.Invoke(t);
                if (value < minValue)
                {
                    minT = t;
                    minValue = value;
                }
            }
            return minT;
        }

        public static T MAxBy<T>(this IList<T> list, Func<T, int> selector)
        {
            int maxValue = int.MinValue;
            T minT = default(T);

            //note if you implement this for List<T>, don't use foreach, it is slower. Use a for loop.
            foreach (T t in list)
            {
                var value = selector.Invoke(t);
                if (value > maxValue)
                {
                    minT = t;
                    maxValue = value;
                }
            }
            return minT;
        }

        public static T MaxBy<T>(this IList<T> list, Func<T, float> selector)
        {
            float maxValue = float.MinValue;
            T minT = default(T);

            //note if you implement this for List<T>, don't use foreach, it is slower. Use a for loop.
            foreach (T t in list)
            {
                var value = selector.Invoke(t);
                if (value > maxValue)
                {
                    minT = t;
                    maxValue = value;
                }
            }
            return minT;
        }

    }
}
