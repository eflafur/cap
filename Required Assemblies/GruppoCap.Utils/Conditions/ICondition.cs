using System;

namespace GruppoCap.Conditions
{

	public interface ICondition<T>
	{
		String ArgumentName { get; }
		T ArgumentValue { get; }
	}

}