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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LobbySessionInfo.get_Name()).MethodHandle;
				}
				return string.Format("[{0} {1:x}]", this.AccountId, this.SessionToken);
			}
			if (!this.ProcessCode.IsNullOrEmpty())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(LobbySessionInfo.get_HandleWithoutNumber()).MethodHandle;
				}
				string handle = this.Handle;
				int num = handle.LastIndexOf('#');
				string result;
				if (num == -1)
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
