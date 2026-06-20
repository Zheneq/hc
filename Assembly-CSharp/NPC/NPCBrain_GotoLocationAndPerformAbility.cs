// ROGUES
// SERVER
using System.Collections;
using UnityEngine;

public class NPCBrain_GotoLocationAndPerformAbility : NPCBrain
{
	private BoardSquare m_destination;

	public AbilityData.ActionType m_abilityId;

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_GotoLocationAndPerformAbility nPCBrain_GotoLocationAndPerformAbility = bot.gameObject.AddComponent<NPCBrain_GotoLocationAndPerformAbility>();
		nPCBrain_GotoLocationAndPerformAbility.m_abilityId = m_abilityId;
		nPCBrain_GotoLocationAndPerformAbility.m_destination = Board.Get().GetSquareFromTransform(destination);
		return nPCBrain_GotoLocationAndPerformAbility;
	}

#if SERVER
// added in rogues
	public override IEnumerator DecideAbilities()
	{
		AbilityData abilityData = GetComponent<AbilityData>();
		ActorData actorData = GetComponent<ActorData>();
		ActorTurnSM actorTurnSm = GetComponent<ActorTurnSM>();
		BotController botController = GetComponent<BotController>();
		if (!abilityData || !actorData || !actorTurnSm || !botController)
		{
			yield break;
		}
		if (m_abilityId != AbilityData.ActionType.INVALID_ACTION && actorData.GetCurrentBoardSquare() == m_destination)
		{
			botController.RequestAbility(null, m_abilityId);
		}
		else
		{
			BoardSquare closestMoveableSquareTo = actorData.GetActorMovement().GetClosestMoveableSquareTo(m_destination, true, false);
			actorTurnSm.SelectMovementSquareForMovement(closestMoveableSquareTo); // , true in rogues
		}
	}
#endif
}
