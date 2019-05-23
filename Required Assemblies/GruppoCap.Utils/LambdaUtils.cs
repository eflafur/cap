using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace GruppoCap
{

	public static class LambdaUtils
	{

		// TRY EXECUTE
		public static Exception TryExecute(this Action action)
		{
			try
			{
				action();
				return null;
			}
			catch (Exception ex)
			{
				return ex;
			}
		}

		// TRY EXECUTE MULTI
		public static IEnumerable<Exception> TryExecuteMulti(Boolean stopAtFirstException, params Action[] actions)
		{
			IList<Exception> exceptions;
			Exception exc;

			exceptions = new List<Exception>();

			foreach (Action action in actions)
			{
				// TRY EXECUTE
				exc = TryExecute(action);

				// IF AN EXCEPTION HAS OCCURRED...
				if (exc != null)
				{
					// ... COLLECT IT...
					exceptions.Add(exc);

					// ... AND EVENTUALLY EXIT RIGHT NOW
					if (stopAtFirstException)
						return exceptions;
				}
			}

			return exceptions;
		}

		// RETURN OR DEFAULT
		public static T ReturnOrDefault<T>(this Func<T> f, T defaultValue = default(T))
		{
			try
			{
				return f();
			}
			catch
			{
				return defaultValue;
			}
		}

		// EXTRACT MEMBER NAME FROM MEMBER EXPRESSION
		public static String ExtractMemberNameFromMemberExpression<T>(Expression<Func<T, Object>> exp)
		{
			if (exp is LambdaExpression && exp.Body is MemberExpression)
			{
				return ((MemberExpression)exp.Body).Member.Name;
			}

			return null;
		}

		// EXTRACT MEMBER NAME FROM MEMBER EXPRESSION
		public static String ExtractMemberNameFromMemberExpression<T>(Expression<Func<T>> exp)
		{
			if (exp is LambdaExpression && exp.Body is MemberExpression)
			{
				return ((MemberExpression)exp.Body).Member.Name;
			}

			return null;
		}

		// EXECUTE WITH ATTEMPTs
		public static void ExecuteWithAttempts(this Action action, Int32 attempts, Func<Int32, Exception, Boolean> onError = null)
		{
			Int32 max;

			max = Math.Max(attempts, 1);

			for (int i = 1; i <= max; i++)
			{
				try
				{
					// EXECUTE THE ACTION
					action();
					return;
				}
				catch (Exception exc)
				{
					if (onError != null)
					{
						if (onError(i, exc) == false)
							return;
					}

					// IF THIS IS THE LAST ATTEMPT...
					if (i == max)
					{
						// ... RE-THROW THE EXCEPTION
						throw;
					}
				}
			}
		}

		// TRY EXECUTE WITH DELAYED RETRIES
		public static Boolean TryExecuteWithDelayedRetries(this Func<Boolean> f, TimeSpan retryDelay, TimeSpan maxDelay)
		{
			//Enforce.IsLessThen("delay", retryDelay, maxDelay);

			Boolean res = false;
			TimeSpan currentTotalDelay = TimeSpan.Zero;

			do
			{
				res = f();

				if (res == false)
				{
					Thread.Sleep(retryDelay);
					currentTotalDelay += retryDelay;
				}
			} while (res == false && (currentTotalDelay + retryDelay) < maxDelay);

			return res;
		}

	}

}