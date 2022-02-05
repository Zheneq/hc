// ROGUES
// SERVER
using UnityEngine.Networking;

// added in rogues -- server-only
#if SERVER
public class AllianceResponseBase : AllianceMessageBase
{
	public bool Success;
	public string ErrorMessage;

	public override void Serialize(NetworkWriter writer)
	{
		base.Serialize(writer);
		writer.Write(this.Success);
		writer.Write(this.ErrorMessage);
	}

	public override void Deserialize(NetworkReader reader)
	{
		base.Deserialize(reader);
		this.Success = reader.ReadBoolean();
		this.ErrorMessage = reader.ReadString();
	}
}
#endif
