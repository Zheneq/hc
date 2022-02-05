// ROGUES
// SERVER
using UnityEngine.Networking;

// added in rogues
// TODO SERIALIZATION verify that server and client messages match
#if SERVER
public class ResolveKnockbacksForActor : MessageBase
{
	public int m_targetActorIndex;
	public int m_sourceActorIndex;

	public override void Serialize(NetworkWriter writer)
	{
		writer.Write(m_targetActorIndex);
		writer.Write(m_sourceActorIndex);
	}

	public override void Deserialize(NetworkReader reader)
	{
		m_targetActorIndex = reader.ReadInt32();
		m_sourceActorIndex = reader.ReadInt32();
	}
}
#endif
