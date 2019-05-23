using System;

namespace GruppoCap.Conditions
{

	public class Condition<T>
		: ICondition<T>
	{

		// CTOR
		public Condition(String argumentName, T argumentValue)
		{
			ArgumentName = argumentName;
			ArgumentValue = argumentValue;
		}

		public T ArgumentValue { get; protected set; }
		public String ArgumentName { get; protected set; }

	}

}