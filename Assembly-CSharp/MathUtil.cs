using UnityEngine;

public static class MathUtil
{
	public static int Clamp(int value, int minValue, int maxValue)
	{
		if (value > maxValue)
		{
			value = maxValue;
		}
		if (value < minValue)
		{
			while (true)
			{
				switch (6)
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
			value = minValue;
		}
		return value;
	}

	public static double Clamp(double value, double minValue, double maxValue)
	{
		if (value > maxValue)
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
			value = maxValue;
		}
		if (value < minValue)
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
			value = minValue;
		}
		return value;
	}

	public static Vector3 Vector3Abs(Vector3 value)
	{
		return new Vector3(Mathf.Abs(value.x), Mathf.Abs(value.y), Mathf.Abs(value.z));
	}

	public static Ray WorldToRay(this Camera camera, Vector3 world)
	{
		return camera.ScreenPointToRay(camera.WorldToScreenPoint(world));
	}

	public static int RoundToIntPadded(float input)
	{
		return Mathf.RoundToInt(input + 0.05f);
	}
}
