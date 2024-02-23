using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class JsonUtil
{
	private static void TraverseNode(JToken node, Action<JProperty> propertyAction = null)
	{
		if (node.Type == JTokenType.Object)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					IEnumerator<JProperty> enumerator = node.Children<JProperty>().GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							JProperty current = enumerator.Current;
							if (propertyAction != null)
							{
								propertyAction(current);
							}
							TraverseNode(current.Value, propertyAction);
						}
						while (true)
						{
							switch (2)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									enumerator.Dispose();
									goto end_IL_0075;
								}
							}
						}
						end_IL_0075:;
					}
				}
				}
			}
		}
		if (node.Type != JTokenType.Array)
		{
			return;
		}
		while (true)
		{
			IEnumerator<JToken> enumerator2 = node.Children().GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					JToken current2 = enumerator2.Current;
					TraverseNode(current2, propertyAction);
				}
			}
			finally
			{
				if (enumerator2 != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							enumerator2.Dispose();
							goto end_IL_00d3;
						}
					}
				}
				end_IL_00d3:;
			}
			return;
		}
	}

	public static bool IsValidJson(this string s, out string formatError)
	{
		try
		{
			JToken node = JToken.Parse(s);
			
			TraverseNode(node, delegate(JProperty prop)
				{
					if (prop.Value.Type == JTokenType.String)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
							{
								string s2 = prop.Value.ToString();
								if (s2.IsTimeSpanFormat())
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											break;
										default:
											try
											{
												TimeSpan timeSpan = TimeSpan.Parse(s2);
											}
											catch (Exception arg)
											{
												throw new Exception(new StringBuilder().Append(prop.Name).Append(" ").Append(prop.Value).Append(" | ").Append(arg).ToString());
											}
											return;
										}
									}
								}
								return;
							}
							}
						}
					}
				});
			formatError = null;
			return true;
		}
		catch (Exception ex)
		{
			formatError = ex.Message;
			return false;
		}
	}

	public static List<string> CompareJson(string jsonString1, string jsonString2)
	{
		List<string> list = new List<string>();
		List<JProperty> jsonProps1 = JObject.Parse(jsonString1).Properties().ToList();
		List<JProperty> jsonProps2 = JObject.Parse(jsonString2).Properties().ToList();
		List<JProperty> list2 = jsonProps1.Where((JProperty x) => jsonProps2.Count((JProperty y) => x.Name == y.Name && JToken.DeepEquals(x, y)) == 0).ToList();
		List<JProperty> list3 = jsonProps2.Where((JProperty x) => jsonProps1.Count((JProperty y) => x.Name == y.Name) == 0).ToList();
		using (List<JProperty>.Enumerator enumerator = list2.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				JProperty p2 = enumerator.Current;
				JProperty jProperty = jsonProps2.SingleOrDefault((JProperty p) => p.Name == p2.Name);
				string item;
				if (jProperty != null)
				{
					item = new StringBuilder().Append("(UPDATED) ").Append(p2.ToString()).Append(" -> ").Append(jProperty.ToString()).ToString();
				}
				else
				{
					item = new StringBuilder().Append("(DELETED) ").Append(p2.ToString()).ToString();
				}
				list.Add(item);
			}
		}
		using (List<JProperty>.Enumerator enumerator2 = list3.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				JProperty current = enumerator2.Current;
				string item2 = new StringBuilder().Append("(ADDED) ").Append(current.ToString()).ToString();
				list.Add(item2);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
	}

	public static T DeepClone<T>(this object obj)
	{
		string value = JsonConvert.SerializeObject(obj);
		return JsonConvert.DeserializeObject<T>(value);
	}
}
