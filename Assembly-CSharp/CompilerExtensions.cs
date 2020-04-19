using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public static class CompilerExtensions
{
	public static bool IsNullOrEmpty(this string s)
	{
		return string.IsNullOrEmpty(s);
	}

	public static bool IsNullOrEmpty<T>(this T[] t)
	{
		bool result;
		if (t != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(T[].IsNullOrEmpty()).MethodHandle;
			}
			result = (t.Length == 0);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public static bool IsNullOrEmpty<T>(this IEnumerable<T> t)
	{
		bool result;
		if (t != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(IEnumerable<T>.IsNullOrEmpty()).MethodHandle;
			}
			result = !t.Any<T>();
		}
		else
		{
			result = true;
		}
		return result;
	}

	public static string SafeReplace(this string value, string key, string replacement)
	{
		string result;
		if (value == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(string.SafeReplace(string, string)).MethodHandle;
			}
			result = null;
		}
		else
		{
			result = value.Replace(key, replacement);
		}
		return result;
	}

	public static bool SafeContains(this string value, string search, StringComparison comparison = StringComparison.Ordinal)
	{
		return value != null && value.IndexOf(search, comparison) >= 0;
	}

	public static bool SafeEquals(this string value, string rhs)
	{
		bool result;
		if (value == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(string.SafeEquals(string)).MethodHandle;
			}
			result = false;
		}
		else
		{
			result = string.Equals(value, rhs);
		}
		return result;
	}

	public static string SafeGetFullPath(this string value)
	{
		string result;
		if (value.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(string.SafeGetFullPath()).MethodHandle;
			}
			result = value;
		}
		else
		{
			result = Path.GetFullPath(value);
		}
		return result;
	}

	public static int GetLastIndexOf(this string value, char charToSearch, int occurence)
	{
		int result = -1;
		int num = 0;
		for (int i = value.Length - 1; i >= 0; i--)
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
			if (num >= occurence)
			{
				break;
			}
			if (value[i] == charToSearch)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(string.GetLastIndexOf(char, int)).MethodHandle;
				}
				result = i;
				num++;
			}
		}
		return result;
	}

	public static string ToISOString(this DateTime value)
	{
		if (value.Kind == DateTimeKind.Local)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DateTime.ToISOString()).MethodHandle;
			}
			return value.ToString("yyyy-MM-dd HH:mm:sszzz");
		}
		return value.ToString("yyyy-MM-dd HH:mm:ss+00");
	}

	public static bool EqualsIgnoreCase(this string lhs, string rhs)
	{
		return string.Compare(lhs, rhs, StringComparison.OrdinalIgnoreCase) == 0;
	}

	public static long ToUnixTimestamp(this DateTime value)
	{
		return (long)value.Subtract(new DateTime(0x7B2, 1, 1)).TotalSeconds;
	}

	public static string ToInitialCharUpper(this string value)
	{
		string result;
		if (value == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(string.ToInitialCharUpper()).MethodHandle;
			}
			result = null;
		}
		else
		{
			result = char.ToUpper(value[0]) + value.Substring(1);
		}
		return result;
	}

	public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
	{
		return "{ " + string.Join(", ", dictionary.Select(delegate(KeyValuePair<TKey, TValue> kv)
		{
			TKey key = kv.Key;
			string str = key.ToString();
			string str2 = "=";
			TValue value = kv.Value;
			return str + str2 + value.ToString();
		}).ToArray<string>()) + " }";
	}

	public static string GetAttribute(this XmlNode value, string attributeName)
	{
		if (value == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(XmlNode.GetAttribute(string)).MethodHandle;
			}
			return null;
		}
		XmlAttribute xmlAttribute = value.Attributes[attributeName];
		if (xmlAttribute == null)
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
			throw new Exception(string.Format("Could not find XML attribute '{0}'", xmlAttribute));
		}
		return xmlAttribute.Value;
	}

	public static string GetAttribute(this XmlNode value, string attributeName, string defaultValue)
	{
		if (value == null)
		{
			return null;
		}
		XmlAttribute xmlAttribute = value.Attributes[attributeName];
		if (xmlAttribute == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(XmlNode.GetAttribute(string, string)).MethodHandle;
			}
			return defaultValue;
		}
		return xmlAttribute.Value;
	}

	public static string GetChildNodeAsString(this XmlNode value, string childNodeName)
	{
		if (value == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(XmlNode.GetChildNodeAsString(string)).MethodHandle;
			}
			return null;
		}
		XmlNode xmlNode = value.SelectSingleNode(childNodeName);
		if (xmlNode == null)
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
			throw new Exception(string.Format("Could not find child XML node '{0}'", childNodeName));
		}
		return xmlNode.InnerText;
	}

	public static string GetChildNodeAsString(this XmlNode value, string childNodeName, string defaultValue)
	{
		if (value == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(XmlNode.GetChildNodeAsString(string, string)).MethodHandle;
			}
			return null;
		}
		XmlNode xmlNode = value.SelectSingleNode(childNodeName);
		if (xmlNode == null)
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
			return defaultValue;
		}
		return xmlNode.InnerText;
	}

	public static int GetChildNodeAsInt32(this XmlNode value, string childNodeName, int? defaultValue = null)
	{
		string childNodeAsString = value.GetChildNodeAsString(childNodeName, null);
		if (childNodeAsString != null)
		{
			return Convert.ToInt32(childNodeAsString);
		}
		if (defaultValue != null)
		{
			return defaultValue.Value;
		}
		throw new Exception(string.Format("Could not find child XML node '{0}'", childNodeName));
	}

	public static long GetChildNodeAsInt64(this XmlNode value, string childNodeName, long? defaultValue = null)
	{
		string childNodeAsString = value.GetChildNodeAsString(childNodeName, null);
		if (childNodeAsString != null)
		{
			return Convert.ToInt64(childNodeAsString);
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(XmlNode.GetChildNodeAsInt64(string, long?)).MethodHandle;
		}
		if (defaultValue != null)
		{
			return defaultValue.Value;
		}
		throw new Exception(string.Format("Could not find child XML node '{0}'", childNodeName));
	}

	public static ulong GetChildNodeAsUInt64(this XmlNode value, string childNodeName, ulong? defaultValue = null)
	{
		string childNodeAsString = value.GetChildNodeAsString(childNodeName, null);
		if (childNodeAsString != null)
		{
			return Convert.ToUInt64(childNodeAsString);
		}
		if (defaultValue != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(XmlNode.GetChildNodeAsUInt64(string, ulong?)).MethodHandle;
			}
			return defaultValue.Value;
		}
		throw new Exception(string.Format("Could not find child XML node '{0}'", childNodeName));
	}

	public static void Shuffle<T>(this IList<T> list, Random rnd)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list.Swap(i, rnd.Next(i, list.Count));
		}
	}

	public static void Swap<T>(this IList<T> list, int i, int j)
	{
		T value = list[i];
		list[i] = list[j];
		list[j] = value;
	}

	public static void Shuffle<T>(this T[] list, Random rnd)
	{
		for (int i = 0; i < list.Length; i++)
		{
			list.Swap(i, rnd.Next(i, list.Length));
		}
	}

	public static void Swap<T>(this T[] list, int i, int j)
	{
		T t = list[i];
		list[i] = list[j];
		list[j] = t;
	}

	public static IEnumerable<T> Shuffled<T>(this IEnumerable<T> list, Random rnd)
	{
		T[] array = list.ToArray<T>();
		array.Shuffle(rnd);
		return array;
	}

	public static IEnumerable<T> ToEnumerable<T>(this T item)
	{
		return CompilerExtensions.CreateEnumerable<T>(new T[]
		{
			item
		});
	}

	public static IEnumerable<T> CreateEnumerable<T>(params T[] items)
	{
		return items;
	}

	public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : class
	{
		TValue result = (TValue)((object)null);
		dictionary.TryGetValue(key, out result);
		return result;
	}

	public static string Reverse(this string text)
	{
		if (text == null)
		{
			return null;
		}
		char[] array = text.ToCharArray();
		Array.Reverse(array);
		return new string(array);
	}

	public static bool HasBeenModified(this FileInfo fileInfo, FileInfo onDiskFileInfo = null)
	{
		if (onDiskFileInfo == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FileInfo.HasBeenModified(FileInfo)).MethodHandle;
			}
			onDiskFileInfo = new FileInfo(fileInfo.FullName);
		}
		if (onDiskFileInfo.Exists == fileInfo.Exists)
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
			if (!(onDiskFileInfo.CreationTimeUtc > fileInfo.CreationTimeUtc))
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
				if (!(onDiskFileInfo.LastWriteTimeUtc > fileInfo.LastWriteTimeUtc))
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
		}
		return true;
	}

	public static IEnumerable<T> GetValues<T>(this T value)
	{
		return Enum.GetValues(typeof(T)).Cast<T>();
	}

	public static object GetDefaultValue(this Type t)
	{
		if (t.IsValueType)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Type.GetDefaultValue()).MethodHandle;
			}
			return Activator.CreateInstance(t);
		}
		return null;
	}

	public static string ToSignatureString(this MethodBase method)
	{
		bool flag = true;
		StringBuilder stringBuilder = new StringBuilder();
		if (method.IsPublic)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MethodBase.ToSignatureString()).MethodHandle;
			}
			stringBuilder.Append("public ");
		}
		else if (method.IsPrivate)
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
			stringBuilder.Append("private ");
		}
		else if (method.IsAssembly)
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
			stringBuilder.Append("internal ");
		}
		if (method.IsFamily)
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
			stringBuilder.Append("protected ");
		}
		if (method.IsStatic)
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
			stringBuilder.Append("static ");
		}
		stringBuilder.Append(method.ReflectedType.FullName);
		stringBuilder.Append('.');
		stringBuilder.Append(method.Name);
		if (method.IsGenericMethod)
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
			stringBuilder.Append("<");
			foreach (Type type in method.GetGenericArguments())
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(type.ToTypeString());
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
			stringBuilder.Append(">");
		}
		stringBuilder.Append("(");
		flag = true;
		bool flag2 = false;
		foreach (ParameterInfo parameterInfo in method.GetParameters())
		{
			if (flag)
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
				flag = false;
				if (method.IsDefined(typeof(ExtensionAttribute), false))
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
					stringBuilder.Append("this ");
				}
			}
			else if (flag2)
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
				flag2 = false;
			}
			else
			{
				stringBuilder.Append(", ");
			}
			if (parameterInfo.ParameterType.IsByRef)
			{
				stringBuilder.Append("ref ");
			}
			else if (parameterInfo.IsOut)
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
				stringBuilder.Append("out ");
			}
			stringBuilder.Append(parameterInfo.ParameterType.ToTypeString());
			stringBuilder.Append(' ');
			stringBuilder.Append(parameterInfo.Name);
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		stringBuilder.Append(")");
		return stringBuilder.ToString();
	}

	public static string ToTypeString(this Type type)
	{
		Type underlyingType = Nullable.GetUnderlyingType(type);
		if (underlyingType != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Type.ToTypeString()).MethodHandle;
			}
			return underlyingType.Name + "?";
		}
		if (!type.IsGenericType)
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
			string name = type.Name;
			if (name != null)
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
				if (name == "String")
				{
					return "string";
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
				if (name == "Int32")
				{
					return "int";
				}
				if (name == "Decimal")
				{
					return "decimal";
				}
				if (name == "Object")
				{
					return "object";
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
				if (name == "Void")
				{
					return "void";
				}
			}
			string result;
			if (string.IsNullOrEmpty(type.FullName))
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
				result = type.Name;
			}
			else
			{
				result = type.FullName;
			}
			return result;
		}
		StringBuilder stringBuilder = new StringBuilder(type.Name.Substring(0, type.Name.IndexOf('`')));
		stringBuilder.Append('<');
		bool flag = true;
		foreach (Type type2 in type.GetGenericArguments())
		{
			if (!flag)
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
				stringBuilder.Append(',');
			}
			stringBuilder.Append(type2.ToTypeString());
			flag = false;
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
		stringBuilder.Append('>');
		return stringBuilder.ToString();
	}

	public static IEnumerable<Type> GetClassesOfType(this Assembly assembly, Type baseClass, string namespaceName = null)
	{
		return assembly.GetTypes().Where(delegate(Type type)
		{
			if (type.IsClass)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CompilerExtensions.<GetClassesOfType>c__AnonStorey1.<>m__0(Type)).MethodHandle;
				}
				if (!namespaceName.IsNullOrEmpty())
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
					if (!type.Namespace.EqualsIgnoreCase(namespaceName))
					{
						goto IL_67;
					}
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				return baseClass.IsAssignableFrom(type);
			}
			IL_67:
			return false;
		});
	}

	public static string ToLowPrecisionString(this TimeSpan timespan)
	{
		TimeSpan duration = timespan.Duration();
		string bigPrecision = null;
		string smallPrecision = null;
		Action<double, TimeSpan, char> action = delegate(double count, TimeSpan unitLength, char unitChar)
		{
			if (smallPrecision == null)
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
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(CompilerExtensions.<ToLowPrecisionString>c__AnonStorey2.<>m__0(double, TimeSpan, char)).MethodHandle;
				}
				int num = (int)Math.Floor(count);
				if (num <= 0)
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
					if (bigPrecision == null)
					{
						return;
					}
				}
				duration -= TimeSpan.FromTicks((long)num * unitLength.Ticks);
				string text = string.Format("{0}{1}", num, unitChar);
				if (bigPrecision == null)
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
					bigPrecision = text;
				}
				else
				{
					smallPrecision = text;
				}
			}
		};
		action(duration.TotalDays / 356.25, TimeSpan.FromDays(356.25), 'y');
		action(duration.TotalDays, TimeSpan.FromDays(1.0), 'd');
		action(duration.TotalHours, TimeSpan.FromHours(1.0), 'h');
		action(duration.TotalMinutes, TimeSpan.FromMinutes(1.0), 'm');
		action(duration.TotalSeconds, TimeSpan.FromSeconds(1.0), 's');
		if (smallPrecision != null)
		{
			return string.Format("{0} {1}", bigPrecision, smallPrecision);
		}
		if (bigPrecision != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TimeSpan.ToLowPrecisionString()).MethodHandle;
			}
			return bigPrecision;
		}
		return "soon";
	}

	public static string ToReadableString(this TimeSpan timespan)
	{
		string format = "{0}{1}{2}{3}";
		object[] array = new object[4];
		int num = 0;
		object obj;
		if (timespan.Duration().Days > 0)
		{
			string format2 = "{0:0} day{1}, ";
			object arg = timespan.Days;
			object arg2;
			if (timespan.Days == 1)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TimeSpan.ToReadableString()).MethodHandle;
				}
				arg2 = string.Empty;
			}
			else
			{
				arg2 = "s";
			}
			obj = string.Format(format2, arg, arg2);
		}
		else
		{
			obj = string.Empty;
		}
		array[num] = obj;
		array[1] = ((timespan.Duration().Hours <= 0) ? string.Empty : string.Format("{0:0} hour{1}, ", timespan.Hours, (timespan.Hours != 1) ? "s" : string.Empty));
		int num2 = 2;
		object obj2;
		if (timespan.Duration().Minutes > 0)
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
			string format3 = "{0:0} minute{1}, ";
			object arg3 = timespan.Minutes;
			object arg4;
			if (timespan.Minutes == 1)
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
				arg4 = string.Empty;
			}
			else
			{
				arg4 = "s";
			}
			obj2 = string.Format(format3, arg3, arg4);
		}
		else
		{
			obj2 = string.Empty;
		}
		array[num2] = obj2;
		array[3] = ((timespan.Duration().Seconds <= 0) ? string.Empty : string.Format("{0:0} second{1}", timespan.Seconds, (timespan.Seconds != 1) ? "s" : string.Empty));
		string text = string.Format(format, array);
		if (text.EndsWith(", "))
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
			text = text.Substring(0, text.Length - 2);
		}
		if (string.IsNullOrEmpty(text))
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
			text = "0 seconds";
		}
		return text;
	}

	public static string ToReadableString(this JsonSerializationException exception)
	{
		string text = exception.ToString();
		string[] array = text.Split(new char[]
		{
			'\n'
		});
		string[] array2 = new string[array.Length];
		int num = 0;
		int i = 0;
		while (i < array.Length)
		{
			string text2 = array[i];
			if (!text2.Contains("System.Enum."))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(JsonSerializationException.ToReadableString()).MethodHandle;
				}
				if (!text2.Contains("Newtonsoft.Json.Serialization.") && !text2.Contains("Newtonsoft.Json.JsonSerializer."))
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
					if (!text2.Contains("Newtonsoft.Json.JsonConvert."))
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
						if (text2.Contains("--- End of inner exception stack trace ---"))
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
						}
						else
						{
							array2[num++] = text2;
						}
					}
				}
			}
			IL_C0:
			i++;
			continue;
			goto IL_C0;
		}
		return string.Join("\n", array2).Trim();
	}

	public static string ToReadableString(this Exception exception)
	{
		string text = exception.ToString();
		string[] array = text.Split(new char[]
		{
			'\n'
		});
		string[] array2 = new string[array.Length];
		int num = 0;
		int i = 0;
		while (i < array.Length)
		{
			string text2 = array[i];
			if (!text2.Contains("System.Runtime.CompilerServices.TaskAwaiter"))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Exception.ToReadableString()).MethodHandle;
				}
				if (!text2.Contains("System.Runtime.ExceptionServices.ExceptionDispatchInfo"))
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
					if (text2.Contains("--- End of stack trace"))
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
					}
					else
					{
						int num2 = text2.IndexOf(".<");
						int num3 = text2.IndexOf(">d__");
						int num4 = text2.IndexOf("MoveNext()");
						if (num2 > 0)
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
							if (num3 > 0 && num4 > 0)
							{
								StringBuilder stringBuilder = new StringBuilder();
								stringBuilder.Append(text2.Substring(0, num2));
								stringBuilder.Append(".");
								stringBuilder.Append(text2.Substring(num2 + 2, num3 - num2 - 2));
								stringBuilder.Append("()");
								stringBuilder.Append(text2.Substring(num4 + 0xA));
								text2 = stringBuilder.ToString();
							}
						}
						array2[num++] = text2;
					}
				}
			}
			IL_15E:
			i++;
			continue;
			goto IL_15E;
		}
		return string.Join("\n", array2).Trim();
	}

	public static string ToJson(this object obj)
	{
		if (!obj.GetType().IsSerializable)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(object.ToJson()).MethodHandle;
			}
			throw new Exception(string.Format("{0} is not serializable", obj.GetType().FullName));
		}
		return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, new JsonConverter[]
		{
			new StringEnumConverter()
		});
	}

	public static IEnumerable<T> Descendants<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> DescendBy)
	{
		bool flag = false;
		uint num;
		IEnumerator<T> enumerator;
		switch (num)
		{
		case 0U:
			enumerator = source.GetEnumerator();
			break;
		case 1U:
		case 2U:
			break;
		default:
			yield break;
		}
		try
		{
			while (enumerator.MoveNext())
			{
				T value = enumerator.Current;
				yield return value;
				flag = true;
				IEnumerator<T> enumerator2 = DescendBy(value).Descendants(DescendBy).GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						T child = enumerator2.Current;
						yield return child;
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(CompilerExtensions.<Descendants>c__Iterator0.MoveNext()).MethodHandle;
						}
						flag = true;
					}
				}
				finally
				{
					if (flag)
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
					}
					else if (enumerator2 != null)
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
						enumerator2.Dispose();
					}
				}
			}
		}
		finally
		{
			if (flag)
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
			}
			else if (enumerator != null)
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
				enumerator.Dispose();
			}
		}
		yield break;
	}

	public static T[] ToArray<T>(this LinkedList<T> item)
	{
		T[] array = new T[item.Count];
		item.CopyTo(array, 0);
		return array;
	}

	public static string ToHexString(this byte[] bytes)
	{
		StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);
		for (int i = 0; i < bytes.Length; i++)
		{
			stringBuilder.Append(bytes[i].ToString("X2"));
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(byte[].ToHexString()).MethodHandle;
		}
		return stringBuilder.ToString();
	}

	public static string ToHexString(this ulong value)
	{
		return string.Format("0x{0:x}", value);
	}

	public static byte[] FromHexString(this string s)
	{
		IEnumerable<int> source = Enumerable.Range(0, s.Length);
		if (CompilerExtensions.<>f__am$cache0 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(string.FromHexString()).MethodHandle;
			}
			CompilerExtensions.<>f__am$cache0 = ((int x) => x % 2 == 0);
		}
		return (from x in source.Where(CompilerExtensions.<>f__am$cache0)
		select Convert.ToByte(s.Substring(x, 2), 0x10)).ToArray<byte>();
	}

	public static bool IsTimeSpanFormat(this string s)
	{
		string[] array = s.Split(new char[]
		{
			':'
		});
		if (array.Length == 3)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(string.IsTimeSpanFormat()).MethodHandle;
			}
			int num;
			if (int.TryParse(array[0], out num))
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
				int num2;
				if (int.TryParse(array[1], out num2))
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
					int num3;
					return int.TryParse(array[2], out num3);
				}
			}
		}
		return false;
	}

	public static bool IsNumeric(this object o)
	{
		switch (Type.GetTypeCode(o.GetType()))
		{
		case TypeCode.SByte:
		case TypeCode.Byte:
		case TypeCode.Int16:
		case TypeCode.UInt16:
		case TypeCode.Int32:
		case TypeCode.UInt32:
		case TypeCode.Int64:
		case TypeCode.UInt64:
		case TypeCode.Single:
		case TypeCode.Double:
		case TypeCode.Decimal:
			return true;
		default:
			return false;
		}
	}

	public static int Max(this Enum e)
	{
		return Enum.GetValues(e.GetType()).Cast<int>().Max();
	}
}
