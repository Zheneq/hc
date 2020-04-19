using System;
using System.Collections.Generic;

internal class FuncEqualityComparer<T> : IEqualityComparer<T>
{
	private readonly Func<T, T, bool> equals;

	private readonly Func<T, int> hashCode;

	public FuncEqualityComparer(Func<T, T, bool> equals, Func<T, int> hashCode)
	{
		this.equals = equals;
		this.hashCode = hashCode;
	}

	public bool Equals(T t1, T t2)
	{
		return this.equals(t1, t2);
	}

	public int GetHashCode(T t)
	{
		return this.hashCode(t);
	}
}
