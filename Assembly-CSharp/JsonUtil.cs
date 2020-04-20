using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class JsonUtil
{
	private static void TraverseNode(JToken node, Action<JProperty> propertyAction = null)
	{
		if (node.Type == JTokenType.Object)
		{
			IEnumerator<JProperty> enumerator = node.Children<JProperty>().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					JProperty jproperty = enumerator.Current;
					if (propertyAction != null)
					{
						propertyAction(jproperty);
					}
					JsonUtil.TraverseNode(jproperty.Value, propertyAction);
				}
			}
			finally
			{
				if (enumerator != null)
				{
					enumerator.Dispose();
				}
			}
		}
		else if (node.Type == JTokenType.Array)
		{
			IEnumerator<JToken> enumerator2 = node.Children().GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					JToken node2 = enumerator2.Current;
					JsonUtil.TraverseNode(node2, propertyAction);
				}
			}
			finally
			{
				if (enumerator2 != null)
				{
					enumerator2.Dispose();
				}
			}
		}
	}

	public unsafe static bool IsValidJson(this string s, out string formatError)
	{
		bool result;
		try
		{
			JToken jtoken = JToken.Parse(s);
			JToken node = jtoken;
			
			JsonUtil.TraverseNode(node, delegate(JProperty prop)
				{
					if (prop.Value.Type == JTokenType.String)
					{
						string s2 = prop.Value.ToString();
						if (s2.IsTimeSpanFormat())
						{
							try
							{
								TimeSpan timeSpan = TimeSpan.Parse(s2);
							}
							catch (Exception arg)
							{
								throw new Exception(string.Format("{0} {1} | {2}", prop.Name, prop.Value, arg));
							}
						}
					}
				});
			formatError = null;
			result = true;
		}
		catch (Exception ex)
		{
			formatError = ex.Message;
			result = false;
		}
		return result;
	}

	public static List<string> CompareJson(string jsonString1, string jsonString2)
	{
		List<string> list = new List<string>();
		List<JProperty> jsonProps1 = JObject.Parse(jsonString1).Properties().ToList<JProperty>();
		List<JProperty> jsonProps2 = JObject.Parse(jsonString2).Properties().ToList<JProperty>();
		List<JProperty> list2 = (from x in jsonProps1
		where jsonProps2.Count((JProperty y) => x.Name == y.Name && JToken.DeepEquals(x, y)) == 0
		select x).ToList<JProperty>();
		List<JProperty> list3 = (from x in jsonProps2
		where jsonProps1.Count((JProperty y) => x.Name == y.Name) == 0
		select x).ToList<JProperty>();
		using (List<JProperty>.Enumerator enumerator = list2.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				JProperty p1 = enumerator.Current;
				JProperty jproperty = jsonProps2.SingleOrDefault((JProperty p) => p.Name == p1.Name);
				string item;
				if (jproperty != null)
				{
					item = string.Format("(UPDATED) {0} -> {1}", p1.ToString(), jproperty.ToString());
				}
				else
				{
					item = string.Format("(DELETED) {0}", p1.ToString());
				}
				list.Add(item);
			}
		}
		using (List<JProperty>.Enumerator enumerator2 = list3.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				JProperty jproperty2 = enumerator2.Current;
				string item2 = string.Format("(ADDED) {0}", jproperty2.ToString());
				list.Add(item2);
			}
		}
		return list;
	}

	public static T DeepClone<T>(this object obj)
	{
		string value = JsonConvert.SerializeObject(obj);
		return JsonConvert.DeserializeObject<T>(value);
	}
}
