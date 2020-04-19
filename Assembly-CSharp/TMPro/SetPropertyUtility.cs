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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SetPropertyUtility.SetColor(Color*, Color)).MethodHandle;
				}
				if (currentValue.g == newValue.g)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (currentValue.b == newValue.b && currentValue.a == newValue.a)
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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SetPropertyUtility.SetStruct(T*, T)).MethodHandle;
				}
				return false;
			}
			currentValue = newValue;
			return true;
		}

		public unsafe static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if (currentValue == null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SetPropertyUtility.SetClass(T*, T)).MethodHandle;
				}
				if (newValue == null)
				{
					return false;
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
			if (currentValue == null || !currentValue.Equals(newValue))
			{
				currentValue = newValue;
				return true;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			return false;
		}
	}
}
