using System;
using UnityEngine;

namespace TMPro
{
	internal static class SetPropertyUtility
	{
		public static bool SetColor(ref Color currentValue, Color newValue)
		{
			if (currentValue.r == newValue.r)
			{
				if (currentValue.g == newValue.g)
				{
					if (currentValue.b == newValue.b && currentValue.a == newValue.a)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								return false;
							}
						}
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

		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (currentValue.Equals(newValue))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
			currentValue = newValue;
			return true;
		}

		public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if (currentValue == null)
			{
				if (newValue == null)
				{
					goto IL_005d;
				}
			}
			if (currentValue != null && currentValue.Equals(newValue))
			{
				goto IL_005d;
			}
			currentValue = newValue;
			return true;
			IL_005d:
			return false;
		}
	}
}
