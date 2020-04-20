using System;
using System.Collections.Generic;

public static class LinqUtil
{
	public static bool ContainsWhere<T>(this IEnumerable<T> elements, Func<T, bool> func)
	{
		IEnumerator<T> enumerator = elements.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				T arg = enumerator.Current;
				if (func(arg))
				{
					return true;
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				enumerator.Dispose();
			}
		}
		return false;
	}
}
