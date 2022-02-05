// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
//using Mirror;
using UnityEngine.Networking;

// added in rogues
// TODO SERIALIZATION verify that server and client messages match
#if SERVER
public class Failsafe_HurryResolutionPhase : MessageBase
{
	public int m_currentTurn;
	public sbyte m_currentAbilityPhaseSbyte;
	public List<int> m_playersStillResolving;

	public override void Serialize(NetworkWriter writer)
	{
		writer.Write(m_currentTurn);
		writer.Write(m_currentAbilityPhaseSbyte);
		writer.Write(m_playersStillResolving.Count);
		for (int i = 0; i < m_playersStillResolving.Count; i++)
		{
			writer.Write(m_playersStillResolving[i]);
		}
	}

	public override void Deserialize(NetworkReader reader)
	{
		m_currentTurn = reader.ReadInt32();
		m_currentAbilityPhaseSbyte = reader.ReadSByte();
		sbyte b = reader.ReadSByte();
		m_playersStillResolving = new List<int>(b);
		for (int i = 0; i < b; i++)
		{
			m_playersStillResolving.Add(reader.ReadInt32());
		}
	}
}
#endif

