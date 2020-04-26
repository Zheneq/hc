using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	public static class TMPro_ExtensionMethods
	{
		public static string ArrayToString(this char[] chars)
		{
			string text = string.Empty;
			for (int i = 0; i < chars.Length; i++)
			{
				if (chars[i] != 0)
				{
					text += chars[i];
					continue;
				}
				break;
			}
			return text;
		}

		public static int FindInstanceID<T>(this List<T> list, T target) where T : Object
		{
			int instanceID = target.GetInstanceID();
			for (int i = 0; i < list.Count; i++)
			{
				T val = list[i];
				if (val.GetInstanceID() != instanceID)
				{
					continue;
				}
				while (true)
				{
					return i;
				}
			}
			while (true)
			{
				return -1;
			}
		}

		public static bool Compare(this Color32 a, Color32 b)
		{
			int result;
			if (a.r == b.r)
			{
				if (a.g == b.g)
				{
					if (a.b == b.b)
					{
						result = ((a.a == b.a) ? 1 : 0);
						goto IL_006a;
					}
				}
			}
			result = 0;
			goto IL_006a;
			IL_006a:
			return (byte)result != 0;
		}

		public static bool CompareRGB(this Color32 a, Color32 b)
		{
			int result;
			if (a.r == b.r)
			{
				if (a.g == b.g)
				{
					result = ((a.b == b.b) ? 1 : 0);
					goto IL_0050;
				}
			}
			result = 0;
			goto IL_0050;
			IL_0050:
			return (byte)result != 0;
		}

		public static bool Compare(this Color a, Color b)
		{
			int result;
			if (a.r == b.r)
			{
				if (a.g == b.g && a.b == b.b)
				{
					result = ((a.a == b.a) ? 1 : 0);
					goto IL_0060;
				}
			}
			result = 0;
			goto IL_0060;
			IL_0060:
			return (byte)result != 0;
		}

		public static bool CompareRGB(this Color a, Color b)
		{
			int result;
			if (a.r == b.r && a.g == b.g)
			{
				result = ((a.b == b.b) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}

		public static Color32 Multiply(this Color32 c1, Color32 c2)
		{
			byte r = (byte)((float)(int)c1.r / 255f * ((float)(int)c2.r / 255f) * 255f);
			byte g = (byte)((float)(int)c1.g / 255f * ((float)(int)c2.g / 255f) * 255f);
			byte b = (byte)((float)(int)c1.b / 255f * ((float)(int)c2.b / 255f) * 255f);
			byte a = (byte)((float)(int)c1.a / 255f * ((float)(int)c2.a / 255f) * 255f);
			return new Color32(r, g, b, a);
		}

		public static Color32 Tint(this Color32 c1, Color32 c2)
		{
			byte r = (byte)((float)(int)c1.r / 255f * ((float)(int)c2.r / 255f) * 255f);
			byte g = (byte)((float)(int)c1.g / 255f * ((float)(int)c2.g / 255f) * 255f);
			byte b = (byte)((float)(int)c1.b / 255f * ((float)(int)c2.b / 255f) * 255f);
			byte a = (byte)((float)(int)c1.a / 255f * ((float)(int)c2.a / 255f) * 255f);
			return new Color32(r, g, b, a);
		}

		public static Color32 Tint(this Color32 c1, float tint)
		{
			byte r = (byte)Mathf.Clamp((float)(int)c1.r / 255f * tint * 255f, 0f, 255f);
			byte g = (byte)Mathf.Clamp((float)(int)c1.g / 255f * tint * 255f, 0f, 255f);
			byte b = (byte)Mathf.Clamp((float)(int)c1.b / 255f * tint * 255f, 0f, 255f);
			byte a = (byte)Mathf.Clamp((float)(int)c1.a / 255f * tint * 255f, 0f, 255f);
			return new Color32(r, g, b, a);
		}

		public static bool Compare(this Vector3 v1, Vector3 v2, int accuracy)
		{
			bool flag = (int)(v1.x * (float)accuracy) == (int)(v2.x * (float)accuracy);
			bool flag2 = (int)(v1.y * (float)accuracy) == (int)(v2.y * (float)accuracy);
			bool flag3 = (int)(v1.z * (float)accuracy) == (int)(v2.z * (float)accuracy);
			int result;
			if (flag)
			{
				if (flag2)
				{
					result = (flag3 ? 1 : 0);
					goto IL_0072;
				}
			}
			result = 0;
			goto IL_0072;
			IL_0072:
			return (byte)result != 0;
		}

		public static bool Compare(this Quaternion q1, Quaternion q2, int accuracy)
		{
			bool flag = (int)(q1.x * (float)accuracy) == (int)(q2.x * (float)accuracy);
			bool flag2 = (int)(q1.y * (float)accuracy) == (int)(q2.y * (float)accuracy);
			bool flag3 = (int)(q1.z * (float)accuracy) == (int)(q2.z * (float)accuracy);
			bool flag4 = (int)(q1.w * (float)accuracy) == (int)(q2.w * (float)accuracy);
			int result;
			if (flag && flag2)
			{
				if (flag3)
				{
					result = (flag4 ? 1 : 0);
					goto IL_008e;
				}
			}
			result = 0;
			goto IL_008e;
			IL_008e:
			return (byte)result != 0;
		}
	}
}
