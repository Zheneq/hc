// ROGUES
// SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBrain_Dash : NPCBrain
{
	public GameObject[] m_dashLocations;
	
#if SERVER
	// added in rogues
	private int m_nextDashLocation;
#endif

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_Dash nPCBrain_Dash = bot.gameObject.AddComponent<NPCBrain_Dash>();
		nPCBrain_Dash.m_dashLocations = m_dashLocations;
		return nPCBrain_Dash;
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
		AbilityData.ActionType actionType = AbilityData.ActionType.ABILITY_0;
		if (abilityData.GetAbilityOfActionType(actionType) is TutorialDash && abilityData.ValidateActionIsRequestable(actionType))
		{
			AbilityTarget item = AbilityTarget.CreateAbilityTargetFromBoardSquare(Board.Get().GetSquareClosestToPos(m_dashLocations[m_nextDashLocation].transform.position.x, m_dashLocations[m_nextDashLocation].transform.position.z), actorData.GetFreePos());
			botController.RequestAbility(new List<AbilityTarget>
			{
				item
			}, actionType);
			m_nextDashLocation++;
			if (m_nextDashLocation >= m_dashLocations.Length)
			{
				m_nextDashLocation = 0;
			}
		}
	}
#endif
}
