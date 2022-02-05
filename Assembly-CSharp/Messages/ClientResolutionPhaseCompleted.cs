// ROGUES
// SERVER
//using System;
//using Mirror;
using UnityEngine.Networking;

// added in rogues
// TODO SERIALIZATION verify that server and client messages match
#if SERVER
public class ClientResolutionPhaseCompleted : MessageBase
{
	public sbyte m_abilityPhase;
	public int m_actorIndex;
	public bool m_asFailsafe;
	public bool m_asResend;

	public override void Serialize(NetworkWriter writer)
	{
		writer.Write(m_abilityPhase);
		writer.Write(m_actorIndex);
		writer.Write(m_asFailsafe);
		writer.Write(m_asResend);
	}

	public override void Deserialize(NetworkReader reader)
	{
		m_abilityPhase = reader.ReadSByte();
		m_actorIndex = reader.ReadInt32();
		m_asFailsafe = reader.ReadBoolean();
		m_asResend = reader.ReadBoolean();
	}
}
#endif

