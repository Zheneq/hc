using System;
using UnityEngine;

public class NPCBrain_GotoLocationAndPerformAbility : NPCBrain
{
	private BoardSquare m_destination;

	public AbilityData.ActionType m_abilityId;

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_GotoLocationAndPerformAbility npcbrain_GotoLocationAndPerformAbility = bot.gameObject.AddComponent<NPCBrain_GotoLocationAndPerformAbility>();
		npcbrain_GotoLocationAndPerformAbility.m_abilityId = this.m_abilityId;
		npcbrain_GotoLocationAndPerformAbility.m_destination = Board.\u000E().\u000E(destination);
		return npcbrain_GotoLocationAndPerformAbility;
	}
}
