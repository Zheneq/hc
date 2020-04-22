using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ServerMessage
{
	public string EN
	{
		get;
		set;
	}

	public string FR
	{
		get;
		set;
	}

	public string DE
	{
		get;
		set;
	}

	public string RU
	{
		get;
		set;
	}

	public string ES
	{
		get;
		set;
	}

	public string IT
	{
		get;
		set;
	}

	public string PL
	{
		get;
		set;
	}

	public string PT
	{
		get;
		set;
	}

	public string KO
	{
		get;
		set;
	}

	public string ZH
	{
		get;
		set;
	}

	[JsonIgnore]
	public IEnumerable<string> Languages
	{
		get
		{
			using (IEnumerator<ServerMessageLanguage> enumerator = Enum.GetValues(typeof(ServerMessageLanguage)).Cast<ServerMessageLanguage>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ServerMessageLanguage language = enumerator.Current;
					if (!GetValue(language).IsNullOrEmpty())
					{
						yield return language.ToString();
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				while (true)
				{
					switch (3)
					{
					default:
						yield break;
					case 0:
						break;
					}
				}
			}
		}
	}

	public static implicit operator ServerMessage(string value)
	{
		ServerMessage serverMessage = new ServerMessage();
		serverMessage.EN = value;
		serverMessage.FR = value;
		serverMessage.DE = value;
		serverMessage.RU = value;
		serverMessage.ES = value;
		serverMessage.IT = value;
		serverMessage.PL = value;
		serverMessage.PT = value;
		serverMessage.KO = value;
		serverMessage.ZH = value;
		return serverMessage;
	}

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
		IEnumerator<ServerMessageLanguage> enumerator = Enum.GetValues(typeof(ServerMessageLanguage)).Cast<ServerMessageLanguage>().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ServerMessageLanguage current = enumerator.Current;
				if (!GetValue(current).IsNullOrEmpty())
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					dictionary[current.ToString().ToLower()] = GetValue(current);
				}
			}
			return dictionary;
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_0083;
					}
				}
			}
			end_IL_0083:;
		}
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
}
