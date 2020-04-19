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
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(IEnumerable<T>.ContainsWhere(Func<T, bool>)).MethodHandle;
					}
					return true;
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		finally
		{
			if (enumerator != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				enumerator.Dispose();
			}
		}
		return false;
	}
}
