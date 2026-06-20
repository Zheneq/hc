// ROGUES
// SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBrain_Scripted : NPCBrain
{
	public string m_name;
	public ActionsForTurn[] m_actions;
	public NPCBrain m_brainAfterActions;
	
#if SERVER
	// added in rogues
	private int m_nextAction;
#endif

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_Scripted nPCBrain_Scripted = bot.gameObject.AddComponent<NPCBrain_Scripted>();
		nPCBrain_Scripted.m_actions = m_actions;
		nPCBrain_Scripted.m_brainAfterActions = m_brainAfterActions;
		return nPCBrain_Scripted;
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
		if (m_actions != null && m_nextAction < m_actions.GetLength(0))
		{
			AbilityData.ActionType actionToDo = m_actions[m_nextAction].m_actionToDo;
			if (actionToDo != AbilityData.ActionType.INVALID_ACTION)
			{
				if (abilityData.GetAbilityOfActionType(actionToDo) == null || !abilityData.ValidateActionIsRequestable(actionToDo))
				{
					Log.Error("Invalid ability specified in NPCBrain_Scripted script.  Index {0}, Bot name: {1}", m_nextAction, actorData.DisplayName);
					yield break;
				}
				List<AbilityTarget> list = null;
				foreach (GameObject gameObject in m_actions[m_nextAction].m_targets)
				{
					if (list == null)
					{
						list = new List<AbilityTarget>();
					}
					AbilityTarget item = AbilityTarget.CreateAbilityTargetFromWorldPos(gameObject.transform.position, actorData.GetFreePos());
					list.Add(item);
				}
				botController.RequestAbility(list, actionToDo);
			}
			if (m_actions[m_nextAction].m_chase)
			{
				BoardSquare closestValidForGameplaySquareTo = Board.Get().GetClosestValidForGameplaySquareTo(m_actions[m_nextAction].m_moveDestinations[0].transform.position.x, m_actions[m_nextAction].m_moveDestinations[0].transform.position.z);
				actorTurnSm.SelectMovementSquareForChasing(closestValidForGameplaySquareTo);
			}
			else if (m_actions[m_nextAction].m_moveDestinations != null && m_actions[m_nextAction].m_moveDestinations.Length != 0)
			{
				List<BoardSquare> list2 = new List<BoardSquare>();
				foreach (GameObject gameObject2 in m_actions[m_nextAction].m_moveDestinations)
				{
					BoardSquare closestValidForGameplaySquareTo2 = Board.Get().GetClosestValidForGameplaySquareTo(gameObject2.transform.position.x, gameObject2.transform.position.z);
					list2.Add(closestValidForGameplaySquareTo2);
				}
				actorTurnSm.SelectMovementSquaresForMovement(list2); // , true in rogues
			}
			m_nextAction++;
			yield break;
		}
		NPCBrain npcbrain = null;
		if (m_brainAfterActions != null)
		{
			npcbrain = m_brainAfterActions.Create(botController, null);
		}
		if (npcbrain != null)
		{
			yield return botController.StartCoroutine(npcbrain.DecideAbilities());
			Destroy(this);
			yield break;
		}
	}
#endif
}
