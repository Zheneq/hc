// ROGUES
// SERVER
using System.Collections.Generic;
//using Mirror;
using UnityEngine.Networking;

// server-only, missing in reactor
// TODO SERIALIZATION verify that server and client messages match
#if SERVER
public class Failsafe_HurryMovementPhase : MessageBase
{
	public int m_currentTurn;
	public List<int> m_actorsNotDone;

	public override void Serialize(NetworkWriter writer)
	{
		writer.Write(m_currentTurn);
		sbyte b = (sbyte)m_actorsNotDone.Count;
		writer.Write(b);
		for (sbyte b2 = 0; b2 < b; b2 += 1)
		{
			writer.Write(m_actorsNotDone[b2]);
		}
	}

	public override void Deserialize(NetworkReader reader)
	{
		m_currentTurn = reader.ReadInt32();
		sbyte b = reader.ReadSByte();
		m_actorsNotDone = new List<int>(b);
		for (sbyte b2 = 0; b2 < b; b2 += 1)
		{
			m_actorsNotDone.Add(reader.ReadInt32());
		}
	}
}
#endif

