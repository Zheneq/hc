// ROGUES
// SERVER
using UnityEngine.Networking;

// added in rogues
// TODO SERIALIZATION verify that server and client messages match
#if SERVER
public class StartResolutionPhase : MessageBase
{
	public int m_currentTurn;
	public sbyte m_currentAbilityPhaseSbyte;
	public sbyte m_currentResolutionActionsCountSbyte;
	public sbyte m_numAnimEntries;

	public override void Serialize(NetworkWriter writer)
	{
		writer.Write(m_currentTurn);
		writer.Write(m_currentAbilityPhaseSbyte);
		writer.Write(m_currentResolutionActionsCountSbyte);
		writer.Write(m_numAnimEntries);
	}

	public override void Deserialize(NetworkReader reader)
	{
		m_currentTurn = reader.ReadInt32();
		m_currentAbilityPhaseSbyte = reader.ReadSByte();
		m_currentResolutionActionsCountSbyte = reader.ReadSByte();
		m_numAnimEntries = reader.ReadSByte();
	}
}
#endif
