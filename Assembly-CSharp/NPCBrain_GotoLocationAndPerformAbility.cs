using UnityEngine;

public class NPCBrain_GotoLocationAndPerformAbility : NPCBrain
{
	private BoardSquare m_destination;

	public AbilityData.ActionType m_abilityId;

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_GotoLocationAndPerformAbility nPCBrain_GotoLocationAndPerformAbility = bot.gameObject.AddComponent<NPCBrain_GotoLocationAndPerformAbility>();
		nPCBrain_GotoLocationAndPerformAbility.m_abilityId = m_abilityId;
		nPCBrain_GotoLocationAndPerformAbility.m_destination = Board.Get().GetSquare(destination);
		return nPCBrain_GotoLocationAndPerformAbility;
	}
}
