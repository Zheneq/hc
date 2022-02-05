// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
//using Mirror;
using UnityEngine.Networking;

// TODO SERIALIZATION verify that server and client messages match
#if SERVER
public class CastAbility : MessageBase
{
	public int m_actorIndex;
	public AbilityData.ActionType m_actionType;
	public List<AbilityTarget> m_targets;

	public override void Serialize(NetworkWriter writer)
	{
		writer.Write(m_actorIndex);
		writer.Write((int)m_actionType);
		AbilityTarget.SerializeAbilityTargetList(m_targets, writer);
	}

	public override void Deserialize(NetworkReader reader)
	{
		m_actorIndex = reader.ReadInt32();
		m_actionType = (AbilityData.ActionType)reader.ReadInt32();
		m_targets = AbilityTarget.DeSerializeAbilityTargetList(reader);
	}
}
#endif

