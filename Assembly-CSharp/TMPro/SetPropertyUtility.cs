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
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (currentValue.g == newValue.g)
				{
					while (true)
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (newValue == null)
				{
					goto IL_005d;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (currentValue != null && currentValue.Equals(newValue))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				goto IL_005d;
			}
			currentValue = newValue;
			return true;
			IL_005d:
			return false;
		}
	}
}
