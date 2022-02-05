// ROGUES
// SERVER
using UnityEngine.Networking;

// added in rogues
// TODO SERIALIZATION verify that server and client messages match
#if SERVER
public class SingleResolutionAction : MessageBase
{
	public int m_turn;

	public AbilityPriority m_abilityPhase;

	public ResolutionAction m_actionServer;

	public ClientResolutionAction m_actionClient;

	public override void Serialize(NetworkWriter writer)
	{
		writer.WritePackedUInt32((uint)m_turn);  // Write(int) in rogues
		writer.Write((sbyte)m_abilityPhase);
		m_actionServer.ResolutionAction_SerializeToStream(writer);
	}

	public override void Deserialize(NetworkReader reader)
	{
		m_turn = (int)reader.ReadPackedUInt32();  // ReadInt32 in rogues
		m_abilityPhase = (AbilityPriority)reader.ReadSByte();
		m_actionClient = ClientResolutionAction.ClientResolutionAction_DeSerializeFromStream(reader);
	}
}
#endif
