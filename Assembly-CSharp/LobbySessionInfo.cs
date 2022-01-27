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
				return $"[{AccountId} {SessionToken:x}]";
			}
			if (!ProcessCode.IsNullOrEmpty())
			{
				return ProcessCode;
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
				int num = Handle.LastIndexOf('#');
				return num == -1 ? Handle : Handle.Remove(num);
			}
			return null;
		}
	}

	public override string ToString()
	{
		return Name;
	}
}
