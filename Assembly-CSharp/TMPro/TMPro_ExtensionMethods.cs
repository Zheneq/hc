using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	public static class TMPro_ExtensionMethods
	{
		public static string ArrayToString(this char[] chars)
		{
			string text = string.Empty;
			int i = 0;
			while (i < chars.Length)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(char[].ArrayToString()).MethodHandle;
				}
				if (chars[i] == '\0')
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						return text;
					}
				}
				else
				{
					text += chars[i];
					i++;
				}
			}
			return text;
		}

		public static int FindInstanceID<T>(this List<T> list, T target) where T : UnityEngine.Object
		{
			int instanceID = target.GetInstanceID();
			for (int i = 0; i < list.Count; i++)
			{
				T t = list[i];
				if (t.GetInstanceID() == instanceID)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(List<T>.FindInstanceID(T)).MethodHandle;
					}
					return i;
				}
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
			return -1;
		}

		public static bool Compare(this Color32 a, Color32 b)
		{
			if (a.r == b.r)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Color32.Compare(Color32)).MethodHandle;
				}
				if (a.g == b.g)
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
					if (a.b == b.b)
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
						return a.a == b.a;
					}
				}
			}
			return false;
		}

		public static bool CompareRGB(this Color32 a, Color32 b)
		{
			if (a.r == b.r)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Color32.CompareRGB(Color32)).MethodHandle;
				}
				if (a.g == b.g)
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
					return a.b == b.b;
				}
			}
			return false;
		}

		public static bool Compare(this Color a, Color b)
		{
			if (a.r == b.r)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Color.Compare(Color)).MethodHandle;
				}
				if (a.g == b.g && a.b == b.b)
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
					return a.a == b.a;
				}
			}
			return false;
		}

		public static bool CompareRGB(this Color a, Color b)
		{
			bool result;
			if (a.r == b.r && a.g == b.g)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Color.CompareRGB(Color)).MethodHandle;
				}
				result = (a.b == b.b);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static Color32 Multiply(this Color32 c1, Color32 c2)
		{
			byte r = (byte)((float)c1.r / 255f * ((float)c2.r / 255f) * 255f);
			byte g = (byte)((float)c1.g / 255f * ((float)c2.g / 255f) * 255f);
			byte b = (byte)((float)c1.b / 255f * ((float)c2.b / 255f) * 255f);
			byte a = (byte)((float)c1.a / 255f * ((float)c2.a / 255f) * 255f);
			return new Color32(r, g, b, a);
		}

		public static Color32 Tint(this Color32 c1, Color32 c2)
		{
			byte r = (byte)((float)c1.r / 255f * ((float)c2.r / 255f) * 255f);
			byte g = (byte)((float)c1.g / 255f * ((float)c2.g / 255f) * 255f);
			byte b = (byte)((float)c1.b / 255f * ((float)c2.b / 255f) * 255f);
			byte a = (byte)((float)c1.a / 255f * ((float)c2.a / 255f) * 255f);
			return new Color32(r, g, b, a);
		}

		public static Color32 Tint(this Color32 c1, float tint)
		{
			byte r = (byte)Mathf.Clamp((float)c1.r / 255f * tint * 255f, 0f, 255f);
			byte g = (byte)Mathf.Clamp((float)c1.g / 255f * tint * 255f, 0f, 255f);
			byte b = (byte)Mathf.Clamp((float)c1.b / 255f * tint * 255f, 0f, 255f);
			byte a = (byte)Mathf.Clamp((float)c1.a / 255f * tint * 255f, 0f, 255f);
			return new Color32(r, g, b, a);
		}

		public static bool Compare(this Vector3 v1, Vector3 v2, int accuracy)
		{
			bool flag = (int)(v1.x * (float)accuracy) == (int)(v2.x * (float)accuracy);
			bool flag2 = (int)(v1.y * (float)accuracy) == (int)(v2.y * (float)accuracy);
			bool result = (int)(v1.z * (float)accuracy) == (int)(v2.z * (float)accuracy);
			if (flag)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Vector3.Compare(Vector3, int)).MethodHandle;
				}
				if (flag2)
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
					return result;
				}
			}
			return false;
		}

		public static bool Compare(this Quaternion q1, Quaternion q2, int accuracy)
		{
			bool flag = (int)(q1.x * (float)accuracy) == (int)(q2.x * (float)accuracy);
			bool flag2 = (int)(q1.y * (float)accuracy) == (int)(q2.y * (float)accuracy);
			bool flag3 = (int)(q1.z * (float)accuracy) == (int)(q2.z * (float)accuracy);
			bool result = (int)(q1.w * (float)accuracy) == (int)(q2.w * (float)accuracy);
			if (flag && flag2)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Quaternion.Compare(Quaternion, int)).MethodHandle;
				}
				if (flag3)
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
					return result;
				}
			}
			return false;
		}
	}
}
