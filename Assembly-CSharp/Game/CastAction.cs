// ROGUES
// SERVER
//using System;
//using Mirror;
using UnityEngine.Networking;

// server-only -- was empty in reactor
// TODO LOW verify that server and client messages match
// NOTE SERVER_ONLY SERIALIZABLE
public class CastAction
{
#if SERVER
	public ActorData m_caster;
	public Ability m_ability;
	private int m_techPointGain;
	private int m_techPointLoss;

	public CastAction(ActorData caster, Ability ability, int techPointGain, int techPointLoss)
	{
		m_caster = caster;
		m_ability = ability;
		m_techPointGain = techPointGain;
		m_techPointLoss = techPointLoss;
	}

	public void CastAction_SerializeToStream(ref NetworkWriter writer)
	{
		int actorIndex = m_caster.ActorIndex;
		sbyte abilityIndex = (sbyte)m_caster.GetAbilityData().GetActionTypeOfAbility(m_ability);
		writer.Write(actorIndex);
		writer.Write(abilityIndex);
		writer.Write(m_techPointGain);
		writer.Write(m_techPointLoss);
	}
#endif
}
