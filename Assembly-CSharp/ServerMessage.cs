using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class ServerMessage
{
	public static implicit operator ServerMessage(string value)
	{
		return new ServerMessage
		{
			EN = value,
			FR = value,
			DE = value,
			RU = value,
			ES = value,
			IT = value,
			PL = value,
			PT = value,
			KO = value,
			ZH = value
		};
	}

	public string EN { get; set; }
	public string FR { get; set; }
	public string DE { get; set; }
	public string RU { get; set; }
	public string ES { get; set; }
	public string IT { get; set; }
	public string PL { get; set; }
	public string PT { get; set; }
	public string KO { get; set; }
	public string ZH { get; set; }

	public string GetValue(ServerMessageLanguage language)
	{
		return (string)GetType().GetProperty(language.ToString()).GetValue(this, null);
	}

	public string GetValue(string languageCode)
	{
		ServerMessageLanguage language = (ServerMessageLanguage)Enum.Parse(typeof(ServerMessageLanguage), languageCode, true);
		return GetValue(language);
	}

	public Dictionary<string, string> GetAllLanguageValues()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (ServerMessageLanguage language in Enum.GetValues(typeof(ServerMessageLanguage)))
		{
			if (!GetValue(language).IsNullOrEmpty())
			{
				dictionary[language.ToString().ToLower()] = GetValue(language);
			}
		}
		return dictionary;
	}

	public void SetValue(ServerMessageLanguage language, string value)
	{
		GetType().GetProperty(language.ToString()).SetValue(this, value, null);
	}

	public void SetValue(string languageCode, string value)
	{
		ServerMessageLanguage language = (ServerMessageLanguage)Enum.Parse(typeof(ServerMessageLanguage), languageCode, true);
		SetValue(language, value);
	}

	[JsonIgnore]
	public IEnumerable<string> Languages
	{
		get
		{
			foreach (ServerMessageLanguage language in Enum.GetValues(typeof(ServerMessageLanguage)))
			{
				if (!GetValue(language).IsNullOrEmpty())
				{
					yield return language.ToString();
				}
			}
		}
	}
}
