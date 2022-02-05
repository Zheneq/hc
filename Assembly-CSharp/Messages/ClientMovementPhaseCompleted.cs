// ROGUES
// SERVER
//using Mirror;
using UnityEngine.Networking;

// server-only, missing in reactor
// TODO SERIALIZATION verify that server and client messages match
#if SERVER
public class ClientMovementPhaseCompleted : MessageBase
{
	public int m_actorIndex;
	public bool m_asFailsafe;

	public override void Serialize(NetworkWriter writer)
	{
		writer.Write(m_actorIndex);
		writer.Write(m_asFailsafe);
	}

	public override void Deserialize(NetworkReader reader)
	{
		m_actorIndex = reader.ReadInt32();
		m_asFailsafe = reader.ReadBoolean();
	}
}
#endif

