using Newtonsoft.Json;
using System;

[Serializable]
public class LobbySessionInfo
{
	public long AccountId;

	public string UserName;

	public string BuildVersion;

	public string ProtocolVersion;

	public long SessionToken;

	public long ReconnectSessionToken;

	public string ProcessCode;

	public ProcessType ProcessType;

	public string ConnectionAddress;

	public string Handle;

	public bool IsBinary;

	public string FakeEntitlements;

	public Region Region;

	public string LanguageCode;

	[JsonIgnore]
	public string Name
	{
		get
		{
			if (!Handle.IsNullOrEmpty())
			{
				return $"{Handle} [{AccountId} {SessionToken:x}]";
			}
			if (SessionToken != 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return $"[{AccountId} {SessionToken:x}]";
					}
				}
			}
			if (!ProcessCode.IsNullOrEmpty())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return ProcessCode;
					}
				}
			}
			return "unknown";
		}
	}

	[JsonIgnore]
	public string HandleWithoutNumber
	{
		get
		{
			if (Handle != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						string handle = Handle;
						int num = handle.LastIndexOf('#');
						string result;
						if (num == -1)
						{
							result = handle;
						}
						else
						{
							result = handle.Remove(num);
						}
						return result;
					}
					}
				}
			}
			return null;
		}
	}

	public override string ToString()
	{
		return Name;
	}
}
