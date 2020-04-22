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
				T current = enumerator.Current;
				if (func(current))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_004c;
					}
				}
			}
			end_IL_004c:;
		}
		return false;
	}
}
