using System;
using Newtonsoft.Json;

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
			if (!this.Handle.IsNullOrEmpty())
			{
				return string.Format("{0} [{1} {2:x}]", this.Handle, this.AccountId, this.SessionToken);
			}
			if (this.SessionToken != 0L)
			{
				return string.Format("[{0} {1:x}]", this.AccountId, this.SessionToken);
			}
			if (!this.ProcessCode.IsNullOrEmpty())
			{
				return this.ProcessCode;
			}
			return "unknown";
		}
	}

	[JsonIgnore]
	public string HandleWithoutNumber
	{
		get
		{
			if (this.Handle != null)
			{
				string handle = this.Handle;
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
			return null;
		}
	}

	public override string ToString()
	{
		return this.Name;
	}
}
