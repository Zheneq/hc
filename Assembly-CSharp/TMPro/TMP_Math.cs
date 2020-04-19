using System;
using UnityEngine;

namespace TMPro
{
	public static class TMP_Math
	{
		public const float FLOAT_MAX = 32767f;

		public const float FLOAT_MIN = -32767f;

		public const int INT_MAX = 0x7FFFFFFF;

		public const int INT_MIN = -0x7FFFFFFF;

		public const float FLOAT_UNSET = -32767f;

		public const int INT_UNSET = -0x7FFF;

		public static Vector2 MAX_16BIT = new Vector2(32767f, 32767f);

		public static Vector2 MIN_16BIT = new Vector2(-32767f, -32767f);

		public static bool Approximately(float a, float b)
		{
			return b - 0.0001f < a && a < b + 0.0001f;
		}
	}
}
