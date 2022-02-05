// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine.Networking;

// server-only, missing in reactor
// TODO SERIALIZATION verify that server and client messages match
#if SERVER
public class ServerMovementStarting : MessageBase
{
	public sbyte m_currentMovementType;
	public List<int> m_actorIndices;
	public List<bool> m_doomed;

	public override void Serialize(NetworkWriter writer)
	{
		sbyte currentMovementType = m_currentMovementType;
		writer.Write(currentMovementType);
		int count = m_actorIndices.Count;
		sbyte b = (sbyte)count;
		writer.Write(b);
		for (int i = 0; i < count; i++)
		{
			int num = m_actorIndices[i];
			bool flag = m_doomed[i];
			writer.Write(num);
			writer.Write(flag);
		}
	}

	public override void Deserialize(NetworkReader reader)
	{
		m_currentMovementType = reader.ReadSByte();
		int num = (int)reader.ReadSByte();
		m_actorIndices = new List<int>(num);
		m_doomed = new List<bool>(num);
		for (int i = 0; i < num; i++)
		{
			int item = reader.ReadInt32();
			bool item2 = reader.ReadBoolean();
			m_actorIndices.Add(item);
			m_doomed.Add(item2);
		}
	}
}
#endif
