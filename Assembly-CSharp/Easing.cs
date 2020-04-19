using System;
using UnityEngine;

internal class Easing
{
	internal static float QuadEaseOut(float t, float b, float c, float d)
	{
		t = Mathf.Clamp(t, 0f, d);
		return -c * (t /= d) * (t - 2f) + b;
	}

	internal static float QuadEaseIn(float t, float b, float c, float d)
	{
		t = Mathf.Clamp(t, 0f, d);
		return c * (t /= d) * t + b;
	}

	internal static float QuadEaseInOut(float t, float b, float c, float d)
	{
		t = Mathf.Clamp(t, 0f, d);
		if ((t /= d / 2f) < 1f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Easing.QuadEaseInOut(float, float, float, float)).MethodHandle;
			}
			return c / 2f * t * t + b;
		}
		return -c / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
	}

	public static float CubicEaseIn(float t, float b, float c, float d)
	{
		t = Mathf.Clamp(t, 0f, d);
		return c * (t /= d) * t * t + b;
	}

	internal static float CubicEaseInOut(float t, float b, float c, float d)
	{
		t = Mathf.Clamp(t, 0f, d);
		if ((t /= d / 2f) < 1f)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Easing.CubicEaseInOut(float, float, float, float)).MethodHandle;
			}
			return c / 2f * t * t * t + b;
		}
		return c / 2f * ((t -= 2f) * t * t + 2f) + b;
	}

	internal static float QuartEaseOut(float t, float b, float c, float d)
	{
		t = Mathf.Clamp(t, 0f, d);
		return -c * ((t = t / d - 1f) * t * t * t - 1f) + b;
	}

	internal static float QuartEaseIn(float t, float b, float c, float d)
	{
		t = Mathf.Clamp(t, 0f, d);
		return c * (t /= d) * t * t * t + b;
	}

	internal static float QuartEaseInOut(float t, float b, float c, float d)
	{
		t = Mathf.Clamp(t, 0f, d);
		if ((t /= d / 2f) < 1f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Easing.QuartEaseInOut(float, float, float, float)).MethodHandle;
			}
			return c / 2f * t * t * t * t + b;
		}
		return -c / 2f * ((t -= 2f) * t * t * t - 2f) + b;
	}

	internal static float QuintEaseInOut(float t, float b, float c, float d)
	{
		t = Mathf.Clamp(t, 0f, d);
		if ((t /= d / 2f) < 1f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Easing.QuintEaseInOut(float, float, float, float)).MethodHandle;
			}
			return c / 2f * t * t * t * t * t + b;
		}
		return c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
	}

	internal static float ExpoEaseInOut(float t, float b, float c, float d)
	{
		t = Mathf.Clamp(t, 0f, d);
		if (t == 0f)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Easing.ExpoEaseInOut(float, float, float, float)).MethodHandle;
			}
			return b;
		}
		if (t == d)
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
			return b + c;
		}
		if ((t /= d / 2f) < 1f)
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
			return c / 2f * Mathf.Pow(2f, 10f * (t - 1f)) + b;
		}
		return c / 2f * (-Mathf.Pow(2f, -10f * (t -= 1f)) + 2f) + b;
	}

	internal static float ExpoEaseIn(float t, float b, float c, float d)
	{
		t = Mathf.Clamp(t, 0f, d);
		return (t != 0f) ? (c * Mathf.Pow(2f, 10f * (t / d - 1f)) + b) : b;
	}

	internal static float ExpoEaseOut(float t, float b, float c, float d)
	{
		t = Mathf.Clamp(t, 0f, d);
		return (t != d) ? (c * (-Mathf.Pow(2f, -10f * t / d) + 1f) + b) : (b + c);
	}

	internal static float SmoothStepEaseInOut(float t, float b, float c, float d)
	{
		t = Mathf.Clamp(t, 0f, d);
		return Mathf.SmoothStep(b, b + c, t / d);
	}
}
