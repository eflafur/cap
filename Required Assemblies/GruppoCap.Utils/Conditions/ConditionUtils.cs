using System;
using System.Collections.Generic;
using GruppoCap.Conditions;

namespace GruppoCap
{

	public static class ConditionUtils
	{

		// IS NOT EQUAL TO
		public static ICondition<T> IsNotEqualTo<T>(this ICondition<T> c, T comparisonValue)
			where T : IComparable<T>
		{
			if (c.ArgumentValue.CompareTo(comparisonValue) == 0)
				throw new ArgumentException(String.Format(@"The argument {0} cannot be equal to {1}.", c.ArgumentName, comparisonValue.ToString()), c.ArgumentName);

			return c;
		}

		// IS NOT NULL
		public static ICondition<T> IsNotNull<T>(this ICondition<T> c)
			where T : class
		{
			if (c.ArgumentValue == null)
				throw new ArgumentNullException(c.ArgumentName, String.Format(@"The argument {0} cannot be null.", c.ArgumentName));

			return c;
		}

		// IS NOT NULL OR EMPTY
		public static ICondition<String> IsNotNullOrEmpty(this ICondition<String> c)
		{
			if (c.ArgumentValue.IsNullOrEmpty())
				throw new ArgumentNullException(c.ArgumentName, String.Format(@"The argument {0} cannot be null or empty.", c.ArgumentName));

			return c;
		}

		// IS NOT NULL OR WHITESPACE
		public static ICondition<String> IsNotNullOrWhiteSpace(this ICondition<String> c)
		{
			if (c.ArgumentValue.IsNullOrWhiteSpace())
				throw new ArgumentNullException(c.ArgumentName, String.Format(@"The argument {0} cannot be null or empty or whitespace.", c.ArgumentName));

			return c;
		}

		// STARTS WITH
		public static ICondition<String> StartsWith(this ICondition<String> c, String s, Boolean relaxed = false)
		{
			if (c.ArgumentValue.StartsWith(s, relaxed ? StringComparison.InvariantCultureIgnoreCase : StringComparison.Ordinal))
				throw new ArgumentException(String.Format(@"The argument {0} must start with ""{1}"" .", c.ArgumentName, s), c.ArgumentName);

			return c;
		}

		// ENDS WITH
		public static ICondition<String> EndsWith(this ICondition<String> c, String s, Boolean relaxed = false)
		{
			if (c.ArgumentValue.EndsWith(s, relaxed ? StringComparison.InvariantCultureIgnoreCase : StringComparison.Ordinal))
				throw new ArgumentException(String.Format(@"The argument {0} must end with ""{1}"" .", c.ArgumentName, s), c.ArgumentName);

			return c;
		}

		// CONTAINs
		public static ICondition<String> Contains(this ICondition<String> c, String s)
		{
			if (c.ArgumentValue.Contains(s))
				throw new ArgumentException(String.Format(@"The argument {0} must contain ""{1}"" .", c.ArgumentName, s), c.ArgumentName);

			return c;
		}

		// IS GREATER THAN
		public static ICondition<T> IsGreaterThan<T>(this ICondition<T> c, T minValue, Boolean inclusive = false)
			where T : IComparable<T>
		{
			if (MathUtils.IsGreaterThen(c.ArgumentValue, minValue, inclusive) == false)
				throw new ArgumentOutOfRangeException(c.ArgumentName, String.Format(@"The argument {0} ({1}) must be greater than{2} {3}.", c.ArgumentName, c.ArgumentValue.ToString(), inclusive ? " or equal to" : String.Empty, minValue.ToString()));

			return c;
		}

		// IS GREATER THAN OR EQUAL TO
		public static ICondition<T> IsGreaterThanOrEqualTo<T>(this ICondition<T> c, T minValue)
			where T : IComparable<T>
		{
			c.IsGreaterThan<T>(minValue, true);

			return c;
		}

		// IS LESS THEN
		public static ICondition<T> IsLessThen<T>(this ICondition<T> c, T maxValue, Boolean inclusive = false)
			where T : IComparable<T>
		{
			if (MathUtils.IsLessThen(c.ArgumentValue, maxValue, inclusive) == false)
				throw new ArgumentOutOfRangeException(c.ArgumentName, String.Format(@"The argument {0} ({1}) must be less than{2} {3}.", c.ArgumentName, c.ArgumentValue.ToString(), inclusive ? " or equal to" : String.Empty, maxValue.ToString()));

			return c;
		}

		// IS LESS THAN OR EQUAL TO
		public static ICondition<T> IsLessThanOrEqualTo<T>(this ICondition<T> c, T maxValue)
			where T : IComparable<T>
		{
			c.IsLessThen<T>(maxValue, true);

			return c;
		}

		// IS IN RANGE
		public static ICondition<T> IsInRange<T>(this ICondition<T> c, T minValue, T maxValue, Boolean inclusive = true)
			where T : IComparable<T>
		{
			c
				.IsGreaterThan<T>(minValue, inclusive)
				.IsLessThen<T>(maxValue, inclusive)
			;

			return c;
		}

		// IS NOT NULL OR EMPTY
		public static ICondition<IEnumerable<T>> IsNotNullOrEmpty<T>(this ICondition<IEnumerable<T>> c)
		{
			if (c.ArgumentValue.IsNullOrEmpty())
				throw new ArgumentNullException(c.ArgumentName, String.Format(@"The argument {0} cannot be null or empty.", c.ArgumentName));

			return c;
		}

		// VALIDATES
		public static ICondition<T> Validates<T>(this ICondition<T> c, Func<T, Boolean> predicate, String predicateName = null)
		{
			if (predicate(c.ArgumentValue) == false)
				throw new ArgumentException(String.Format(@"The argument {0} does not validate the{1} predicate.", c.ArgumentName, predicateName.IsNullOrWhiteSpace() ? "" : (" " + predicateName)), c.ArgumentName);

			return c;
		}

	}

}