// ROGUES
// SERVER
using System;
//using Mirror;
using Newtonsoft.Json;
using UnityEngine.Networking;

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

	// added in rogues
    //public string ExternalConnectionAddress;

	public string Handle;
	public bool IsBinary;
	public string FakeEntitlements;
	public Region Region;
	public string LanguageCode;

	// added in rogues
#if SERVER
	public void Serialize(NetworkWriter writer)
    {
        writer.Write(AccountId);
        writer.Write(UserName);
        writer.Write(BuildVersion);
        writer.Write(ProtocolVersion);
        writer.Write(SessionToken);
        writer.Write(ReconnectSessionToken);
        writer.Write(ProcessCode);
        writer.Write((short)ProcessType);
        writer.Write(ConnectionAddress);
        //writer.Write(ExternalConnectionAddress);
        writer.Write(Handle);
        writer.Write(IsBinary);
        writer.Write(FakeEntitlements);
        writer.Write((short)Region);
        writer.Write(LanguageCode);
    }
#endif

	// added in rogues
#if SERVER
	public void Deserialize(NetworkReader reader)
    {
        AccountId = reader.ReadInt64();
        UserName = reader.ReadString();
        BuildVersion = reader.ReadString();
        ProtocolVersion = reader.ReadString();
        SessionToken = reader.ReadInt64();
        ReconnectSessionToken = reader.ReadInt64();
        ProcessCode = reader.ReadString();
        ProcessType = (ProcessType)reader.ReadInt16();
        ConnectionAddress = reader.ReadString();
        //ExternalConnectionAddress = reader.ReadString();
        Handle = reader.ReadString();
        IsBinary = reader.ReadBoolean();
        FakeEntitlements = reader.ReadString();
        Region = (Region)reader.ReadInt16();
        LanguageCode = reader.ReadString();
    }
#endif

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
