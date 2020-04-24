using System;
using System.Collections.Generic;

namespace TMPro
{
	internal static class TMP_ListPool<T>
	{
		private static readonly TMP_ObjectPool<List<T>> s_ListPool = new TMP_ObjectPool<List<T>>(null, delegate(List<T> l)
		{
			l.Clear();
		});

		public static List<T> Get()
		{
			return TMP_ListPool<T>.s_ListPool.Get();
		}

		public static void Release(List<T> toRelease)
		{
			TMP_ListPool<T>.s_ListPool.Release(toRelease);
		}
	}
}
