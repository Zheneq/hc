using System;
using UnityEngine;

namespace TMPro
{
	internal static class SetPropertyUtility
	{
		public unsafe static bool SetColor(ref Color currentValue, Color newValue)
		{
			if (currentValue.r == newValue.r)
			{
				if (currentValue.g == newValue.g)
				{
					if (currentValue.b == newValue.b && currentValue.a == newValue.a)
					{
						return false;
					}
				}
			}
			currentValue = newValue;
			return true;
		}

		public static bool SetEquatableStruct<T>(ref T currentValue, T newValue) where T : IEquatable<T>
		{
			if (currentValue.Equals(newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		public unsafe static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (currentValue.Equals(newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		public unsafe static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if (currentValue == null)
			{
				if (newValue == null)
				{
					return false;
				}
			}
			if (currentValue == null || !currentValue.Equals(newValue))
			{
				currentValue = newValue;
				return true;
			}
			return false;
		}
	}
}
