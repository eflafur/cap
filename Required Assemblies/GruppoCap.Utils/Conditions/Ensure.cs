using System;
using System.Linq.Expressions;
using GruppoCap.Conditions;

namespace GruppoCap
{

	public static class Ensure
	{

		// ARG
		public static ICondition<T> Arg<T>(String argumentName, T argumentValue)
		{
			return new Condition<T>(argumentName, argumentValue);
		}

		// ARG
		public static ICondition<T> Arg<T>(Expression<Func<T>> exp)
		{
			String argumentName;

			argumentName = LambdaUtils.ExtractMemberNameFromMemberExpression(exp);

			if (String.IsNullOrWhiteSpace(argumentName))
				throw new ArgumentException("It was not possible to extract the Argument Name from the provided expression", "exp");

			return new Condition<T>(argumentName, exp.Compile()());
		}

	}

}