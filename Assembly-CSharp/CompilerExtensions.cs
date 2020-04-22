using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

public static class CompilerExtensions
{
	public static bool IsNullOrEmpty(this string s)
	{
		return string.IsNullOrEmpty(s);
	}

	public static bool IsNullOrEmpty<T>(this T[] t)
	{
		int result;
		if (t != null)
		{
			result = ((t.Length == 0) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public static bool IsNullOrEmpty<T>(this IEnumerable<T> t)
	{
		int result;
		if (t != null)
		{
			result = ((!t.Any()) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public static string SafeReplace(this string value, string key, string replacement)
	{
		object result;
		if (value == null)
		{
			result = null;
		}
		else
		{
			result = value.Replace(key, replacement);
		}
		return (string)result;
	}

	public static bool SafeContains(this string value, string search, StringComparison comparison = StringComparison.Ordinal)
	{
		return value != null && value.IndexOf(search, comparison) >= 0;
	}

	public static bool SafeEquals(this string value, string rhs)
	{
		int result;
		if (value == null)
		{
			result = 0;
		}
		else
		{
			result = (string.Equals(value, rhs) ? 1 : 0);
		}
		return (byte)result != 0;
	}

	public static string SafeGetFullPath(this string value)
	{
		string result;
		if (value.IsNullOrEmpty())
		{
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
		for (int num2 = value.Length - 1; num2 >= 0; num2--)
		{
			if (num >= occurence)
			{
				break;
			}
			if (value[num2] == charToSearch)
			{
				result = num2;
				num++;
			}
		}
		return result;
	}

	public static string ToISOString(this DateTime value)
	{
		if (value.Kind == DateTimeKind.Local)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return value.ToString("yyyy-MM-dd HH:mm:sszzz");
				}
			}
		}
		return value.ToString("yyyy-MM-dd HH:mm:ss+00");
	}

	public static bool EqualsIgnoreCase(this string lhs, string rhs)
	{
		return string.Compare(lhs, rhs, StringComparison.OrdinalIgnoreCase) == 0;
	}

	public static long ToUnixTimestamp(this DateTime value)
	{
		return (long)value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
	}

	public static string ToInitialCharUpper(this string value)
	{
		object result;
		if (value == null)
		{
			result = null;
		}
		else
		{
			result = char.ToUpper(value[0]) + value.Substring(1);
		}
		return (string)result;
	}

	public static string ToDebugString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
	{
		return "{ " + string.Join(", ", dictionary.Select((KeyValuePair<TKey, TValue> kv) => kv.Key.ToString() + "=" + kv.Value.ToString()).ToArray()) + " }";
	}

	public static string GetAttribute(this XmlNode value, string attributeName)
	{
		if (value == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		XmlAttribute xmlAttribute = value.Attributes[attributeName];
		if (xmlAttribute == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					throw new Exception($"Could not find XML attribute '{xmlAttribute}'");
				}
			}
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return defaultValue;
				}
			}
		}
		return xmlAttribute.Value;
	}

	public static string GetChildNodeAsString(this XmlNode value, string childNodeName)
	{
		if (value == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		XmlNode xmlNode = value.SelectSingleNode(childNodeName);
		if (xmlNode == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					throw new Exception($"Could not find child XML node '{childNodeName}'");
				}
			}
		}
		return xmlNode.InnerText;
	}

	public static string GetChildNodeAsString(this XmlNode value, string childNodeName, string defaultValue)
	{
		if (value == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		XmlNode xmlNode = value.SelectSingleNode(childNodeName);
		if (xmlNode == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return defaultValue;
				}
			}
		}
		return xmlNode.InnerText;
	}

	public static int GetChildNodeAsInt32(this XmlNode value, string childNodeName, int? defaultValue = null)
	{
		string childNodeAsString = value.GetChildNodeAsString(childNodeName, null);
		if (childNodeAsString == null)
		{
			if (defaultValue.HasValue)
			{
				return defaultValue.Value;
			}
			throw new Exception($"Could not find child XML node '{childNodeName}'");
		}
		return Convert.ToInt32(childNodeAsString);
	}

	public static long GetChildNodeAsInt64(this XmlNode value, string childNodeName, long? defaultValue = null)
	{
		string childNodeAsString = value.GetChildNodeAsString(childNodeName, null);
		if (childNodeAsString == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (defaultValue.HasValue)
					{
						return defaultValue.Value;
					}
					throw new Exception($"Could not find child XML node '{childNodeName}'");
				}
			}
		}
		return Convert.ToInt64(childNodeAsString);
	}

	public static ulong GetChildNodeAsUInt64(this XmlNode value, string childNodeName, ulong? defaultValue = null)
	{
		string childNodeAsString = value.GetChildNodeAsString(childNodeName, null);
		if (childNodeAsString == null)
		{
			if (defaultValue.HasValue)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return defaultValue.Value;
					}
				}
			}
			throw new Exception($"Could not find child XML node '{childNodeName}'");
		}
		return Convert.ToUInt64(childNodeAsString);
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
		T val = list[i];
		list[i] = list[j];
		list[j] = val;
	}

	public static IEnumerable<T> Shuffled<T>(this IEnumerable<T> list, Random rnd)
	{
		T[] array = list.ToArray();
		array.Shuffle(rnd);
		return array;
	}

	public static IEnumerable<T> ToEnumerable<T>(this T item)
	{
		return CreateEnumerable<T>(item);
	}

	public static IEnumerable<T> CreateEnumerable<T>(params T[] items)
	{
		return items;
	}

	public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : class
	{
		TValue value = (TValue)null;
		dictionary.TryGetValue(key, out value);
		return value;
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
			onDiskFileInfo = new FileInfo(fileInfo.FullName);
		}
		if (onDiskFileInfo.Exists == fileInfo.Exists)
		{
			if (!(onDiskFileInfo.CreationTimeUtc > fileInfo.CreationTimeUtc))
			{
				if (!(onDiskFileInfo.LastWriteTimeUtc > fileInfo.LastWriteTimeUtc))
				{
					return false;
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return Activator.CreateInstance(t);
				}
			}
		}
		return null;
	}

	public static string ToSignatureString(this MethodBase method)
	{
		bool flag = true;
		StringBuilder stringBuilder = new StringBuilder();
		if (method.IsPublic)
		{
			stringBuilder.Append("public ");
		}
		else if (method.IsPrivate)
		{
			stringBuilder.Append("private ");
		}
		else if (method.IsAssembly)
		{
			stringBuilder.Append("internal ");
		}
		if (method.IsFamily)
		{
			stringBuilder.Append("protected ");
		}
		if (method.IsStatic)
		{
			stringBuilder.Append("static ");
		}
		stringBuilder.Append(method.ReflectedType.FullName);
		stringBuilder.Append('.');
		stringBuilder.Append(method.Name);
		if (method.IsGenericMethod)
		{
			stringBuilder.Append("<");
			Type[] genericArguments = method.GetGenericArguments();
			foreach (Type type in genericArguments)
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
			stringBuilder.Append(">");
		}
		stringBuilder.Append("(");
		flag = true;
		bool flag2 = false;
		ParameterInfo[] parameters = method.GetParameters();
		foreach (ParameterInfo parameterInfo in parameters)
		{
			if (flag)
			{
				flag = false;
				if (method.IsDefined(typeof(ExtensionAttribute), false))
				{
					stringBuilder.Append("this ");
				}
			}
			else if (flag2)
			{
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
				stringBuilder.Append("out ");
			}
			stringBuilder.Append(parameterInfo.ParameterType.ToTypeString());
			stringBuilder.Append(' ');
			stringBuilder.Append(parameterInfo.Name);
		}
		while (true)
		{
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}
	}

	public static string ToTypeString(this Type type)
	{
		Type underlyingType = Nullable.GetUnderlyingType(type);
		if (underlyingType != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return underlyingType.Name + "?";
				}
			}
		}
		if (!type.IsGenericType)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					string name = type.Name;
					if (name != null)
					{
						if (name == "String")
						{
							return "string";
						}
						switch (name)
						{
						case "Int32":
							return "int";
						case "Decimal":
							return "decimal";
						case "Object":
							return "object";
						}
						if (name == "Void")
						{
							return "void";
						}
					}
					string result;
					if (string.IsNullOrEmpty(type.FullName))
					{
						result = type.Name;
					}
					else
					{
						result = type.FullName;
					}
					return result;
				}
				}
			}
		}
		StringBuilder stringBuilder = new StringBuilder(type.Name.Substring(0, type.Name.IndexOf('`')));
		stringBuilder.Append('<');
		bool flag = true;
		Type[] genericArguments = type.GetGenericArguments();
		foreach (Type type2 in genericArguments)
		{
			if (!flag)
			{
				stringBuilder.Append(',');
			}
			stringBuilder.Append(type2.ToTypeString());
			flag = false;
		}
		while (true)
		{
			stringBuilder.Append('>');
			return stringBuilder.ToString();
		}
	}

	public static IEnumerable<Type> GetClassesOfType(this Assembly assembly, Type baseClass, string namespaceName = null)
	{
		return assembly.GetTypes().Where(delegate(Type type)
		{
			if (!type.IsClass)
			{
				goto IL_0067;
			}
			if (!namespaceName.IsNullOrEmpty())
			{
				if (!type.Namespace.EqualsIgnoreCase(namespaceName))
				{
					goto IL_0067;
				}
			}
			int result = baseClass.IsAssignableFrom(type) ? 1 : 0;
			goto IL_0068;
			IL_0067:
			result = 0;
			goto IL_0068;
			IL_0068:
			return (byte)result != 0;
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
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						int num = (int)Math.Floor(count);
						if (num <= 0)
						{
							if (bigPrecision == null)
							{
								return;
							}
						}
						duration -= TimeSpan.FromTicks(num * unitLength.Ticks);
						string text = $"{num}{unitChar}";
						if (bigPrecision == null)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									bigPrecision = text;
									return;
								}
							}
						}
						smallPrecision = text;
						return;
					}
					}
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
			return $"{bigPrecision} {smallPrecision}";
		}
		if (bigPrecision != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return bigPrecision;
				}
			}
		}
		return "soon";
	}

	public static string ToReadableString(this TimeSpan timespan)
	{
		object[] array = new object[4];
		string text;
		if (timespan.Duration().Days > 0)
		{
			object arg = timespan.Days;
			object arg2;
			if (timespan.Days == 1)
			{
				arg2 = string.Empty;
			}
			else
			{
				arg2 = "s";
			}
			text = $"{arg:0} day{arg2}, ";
		}
		else
		{
			text = string.Empty;
		}
		array[0] = text;
		array[1] = ((timespan.Duration().Hours <= 0) ? string.Empty : string.Format("{0:0} hour{1}, ", timespan.Hours, (timespan.Hours != 1) ? "s" : string.Empty));
		string text2;
		if (timespan.Duration().Minutes > 0)
		{
			object arg3 = timespan.Minutes;
			object arg4;
			if (timespan.Minutes == 1)
			{
				arg4 = string.Empty;
			}
			else
			{
				arg4 = "s";
			}
			text2 = $"{arg3:0} minute{arg4}, ";
		}
		else
		{
			text2 = string.Empty;
		}
		array[2] = text2;
		array[3] = ((timespan.Duration().Seconds <= 0) ? string.Empty : string.Format("{0:0} second{1}", timespan.Seconds, (timespan.Seconds != 1) ? "s" : string.Empty));
		string text3 = string.Format("{0}{1}{2}{3}", array);
		if (text3.EndsWith(", "))
		{
			text3 = text3.Substring(0, text3.Length - 2);
		}
		if (string.IsNullOrEmpty(text3))
		{
			text3 = "0 seconds";
		}
		return text3;
	}

	public static string ToReadableString(this JsonSerializationException exception)
	{
		string text = exception.ToString();
		string[] array = text.Split('\n');
		string[] array2 = new string[array.Length];
		int num = 0;
		foreach (string text2 in array)
		{
			if (text2.Contains("System.Enum."))
			{
				continue;
			}
			if (text2.Contains("Newtonsoft.Json.Serialization.") || text2.Contains("Newtonsoft.Json.JsonSerializer."))
			{
				continue;
			}
			if (text2.Contains("Newtonsoft.Json.JsonConvert."))
			{
				continue;
			}
			if (text2.Contains("--- End of inner exception stack trace ---"))
			{
			}
			else
			{
				array2[num++] = text2;
			}
		}
		return string.Join("\n", array2).Trim();
	}

	public static string ToReadableString(this Exception exception)
	{
		string text = exception.ToString();
		string[] array = text.Split('\n');
		string[] array2 = new string[array.Length];
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = array[i];
			if (text2.Contains("System.Runtime.CompilerServices.TaskAwaiter"))
			{
				continue;
			}
			if (text2.Contains("System.Runtime.ExceptionServices.ExceptionDispatchInfo"))
			{
				continue;
			}
			if (text2.Contains("--- End of stack trace"))
			{
				continue;
			}
			int num2 = text2.IndexOf(".<");
			int num3 = text2.IndexOf(">d__");
			int num4 = text2.IndexOf("MoveNext()");
			if (num2 > 0)
			{
				if (num3 > 0 && num4 > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(text2.Substring(0, num2));
					stringBuilder.Append(".");
					stringBuilder.Append(text2.Substring(num2 + 2, num3 - num2 - 2));
					stringBuilder.Append("()");
					stringBuilder.Append(text2.Substring(num4 + 10));
					text2 = stringBuilder.ToString();
				}
			}
			array2[num++] = text2;
		}
		return string.Join("\n", array2).Trim();
	}

	public static string ToJson(this object obj)
	{
		if (!obj.GetType().IsSerializable)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					throw new Exception($"{obj.GetType().FullName} is not serializable");
				}
			}
		}
		return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, new StringEnumConverter());
	}

	public static IEnumerable<T> Descendants<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> DescendBy)
	{
		IEnumerator<T> enumerator = source.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				T value = enumerator.Current;
				yield return value;
				IEnumerator<T> enumerator2 = DescendBy(value).Descendants(DescendBy).GetEnumerator();
				try
				{
					if (enumerator2.MoveNext())
					{
						yield return enumerator2.Current;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				finally
				{
					if (enumerator2 != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								enumerator2.Dispose();
								goto end_IL_0118;
							}
						}
					}
					goto end_IL_0118;
					IL_011b:
					switch (7)
					{
					default:
						goto end_IL_0118;
					case 0:
						goto IL_011b;
					}
					end_IL_0118:;
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_0158;
					}
				}
			}
			goto end_IL_0158;
			IL_015b:
			switch (3)
			{
			default:
				goto end_IL_0158;
			case 0:
				goto IL_015b;
			}
			end_IL_0158:;
		}
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
		while (true)
		{
			return stringBuilder.ToString();
		}
	}

	public static string ToHexString(this ulong value)
	{
		return $"0x{value:x}";
	}

	public static byte[] FromHexString(this string s)
	{
		IEnumerable<int> source = Enumerable.Range(0, s.Length);
		
		return (from x in source.Where(((int x) => x % 2 == 0))
			select Convert.ToByte(s.Substring(x, 2), 16)).ToArray();
	}

	public static bool IsTimeSpanFormat(this string s)
	{
		string[] array = s.Split(':');
		int result4;
		if (array.Length == 3)
		{
			if (int.TryParse(array[0], out int _))
			{
				if (int.TryParse(array[1], out int _))
				{
					result4 = (int.TryParse(array[2], out int _) ? 1 : 0);
					goto IL_006a;
				}
			}
		}
		result4 = 0;
		goto IL_006a;
		IL_006a:
		return (byte)result4 != 0;
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
