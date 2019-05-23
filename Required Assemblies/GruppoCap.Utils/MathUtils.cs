using System;

namespace GruppoCap
{
    public static class MathUtils
    {

        // ENSURE BETWEEN
        public static Int32 EnsureBetween(Int32 value, Int32 inclusiveMinValue, Int32 inclusiveMaxValue)
        {
            return Math.Max(inclusiveMinValue, Math.Min(inclusiveMaxValue, value));
        }

        // ENSURE BETWEEN
        public static Int64 EnsureBetween(Int64 value, Int64 inclusiveMinValue, Int64 inclusiveMaxValue)
        {
            return Math.Max(inclusiveMinValue, Math.Min(inclusiveMaxValue, value));
        }

        // ENSURE BETWEEN
        public static Byte EnsureBetween(Byte value, Byte inclusiveMinValue, Byte inclusiveMaxValue)
        {
            return Math.Max(inclusiveMinValue, Math.Min(inclusiveMaxValue, value));
        }

        // ENSURE BETWEEN
        public static Single EnsureBetween(Single value, Single inclusiveMinValue, Single inclusiveMaxValue)
        {
            return Math.Max(inclusiveMinValue, Math.Min(inclusiveMaxValue, value));
        }

        // ENSURE BETWEEN
        public static Double EnsureBetween(Double value, Double inclusiveMinValue, Double inclusiveMaxValue)
        {
            return Math.Max(inclusiveMinValue, Math.Min(inclusiveMaxValue, value));
        }

        // X STAY TO Y AS Z STAY TO WHAT
        public static Int32 XStayToYAsZStayToWhat(Int32 x, Int32 y, Int32 z)
        {
            return y * z / x;
        }

        // IS GREATER THEN
        public static Boolean IsGreaterThen<T>(T value, T comparisonValue, Boolean limitIsInclusive)
            where T : IComparable<T>
        {
            Int32 compareResult;

            compareResult = value.CompareTo(comparisonValue);

            return ((compareResult == 0 && limitIsInclusive == true) || (compareResult > 0));
        }

        // IS LESS THEN
        public static Boolean IsLessThen<T>(T value, T comparisonValue, Boolean limitIsInclusive)
            where T : IComparable<T>
        {
            Int32 compareResult;

            compareResult = value.CompareTo(comparisonValue);

            return ((compareResult == 0 && limitIsInclusive == true) || (compareResult < 0));
        }

        // IS IN RANGE
        public static Boolean IsInRange<T>(T value, T minValue, T maxValue, Boolean limitIsInclusive)
            where T : IComparable<T>
        {
            return
                IsGreaterThen(value, minValue, limitIsInclusive) &&
                IsLessThen(value, maxValue, limitIsInclusive)
            ;
        }

        //GENERATE A RANDOM NUMBER
        public static int GenerateRandom(int minValue, int maxValue)
        {
            Random rnd = new Random();
            int rndNumber = rnd.Next(minValue, maxValue);

            return rndNumber;
        }

        public static Int32 Sum(this Int32 a, Int32 b)
        {
            return a + b;
        }

    }
}
