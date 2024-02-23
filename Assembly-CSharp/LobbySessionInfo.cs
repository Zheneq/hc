using Newtonsoft.Json;
using System;
using System.Text;

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
				return new StringBuilder().Append(Handle).Append(" [").Append(AccountId).Append(" ").AppendFormat("{0:x}", SessionToken).Append("]").ToString();
			}
			if (SessionToken != 0)
			{
				return new StringBuilder().Append("[").Append(AccountId).Append(" ").AppendFormat("{0:x}", SessionToken).Append("]").ToString();
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
