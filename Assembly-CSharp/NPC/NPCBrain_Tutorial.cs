// ROGUES
// SERVER
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBrain_Tutorial : NPCBrain
{
	public enum AttackPattern
	{
		AlwaysAttackPlayer,
		AttackPlayerIfInLoS,
		AttackNearestEnemy,
		NeverAttack
	}

	public enum MovementPattern
	{
		StayStill,
		MoveOncePlayerInLoS,
		MoveAsap
	}

	public AttackPattern m_attackPattern = AttackPattern.AttackPlayerIfInLoS;
	private Transform m_destination;
	public MovementPattern m_movementPattern;
	public float m_attackRange = 5f;

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_Tutorial nPCBrain_Tutorial = bot.gameObject.AddComponent<NPCBrain_Tutorial>();
		nPCBrain_Tutorial.m_attackPattern = m_attackPattern;
		nPCBrain_Tutorial.m_movementPattern = m_movementPattern;
		nPCBrain_Tutorial.m_destination = destination;
		nPCBrain_Tutorial.m_attackRange = m_attackRange;
		return nPCBrain_Tutorial;
	}

	public bool IsPlayerInLoS()
	{
		bool isVisibleToEnemy = GetComponent<ActorData>().IsActorVisibleToAnyEnemy();
		ActorData localPlayer = SinglePlayerManager.Get().GetLocalPlayer();
		if (localPlayer == null)
		{
			return false;
		}
		ActorStatus actorStatus = localPlayer.GetComponent<ActorStatus>();
		bool isNotHidden = !localPlayer.IsInBrush()
		             || actorStatus.HasStatus(StatusType.CantHideInBrush)
		             || actorStatus.HasStatus(StatusType.Revealed);
		return isVisibleToEnemy && isNotHidden;
	}
	
#if SERVER
	// added in rogues
	private void AttackActor(ActorData playerActor)
	{
		AbilityData abilityData = GetComponent<AbilityData>();
		ActorData actorData = GetComponent<ActorData>();
		BotController botController = GetComponent<BotController>();
		if (abilityData == null || actorData == null || botController == null || playerActor == null)
		{
			return;
		}
		float num = actorData.GetCurrentBoardSquare().HorizontalDistanceOnBoardTo(playerActor.GetCurrentBoardSquare());
		if (m_attackRange < num)
		{
			return;
		}
		AbilityData.ActionType actionType = AbilityData.ActionType.ABILITY_0;
		Ability abilityOfActionType = abilityData.GetAbilityOfActionType(actionType);
		if (abilityOfActionType == null)
		{
			return;
		}
		if (!abilityData.ValidateActionIsRequestable(actionType))
		{
			return;
		}
		if (abilityOfActionType.IsSimpleAction())
		{
			GetComponent<ServerActorController>().ProcessCastSimpleActionRequest(actionType, true);
			return;
		}
		bool flag = true;
		List<AbilityTarget> list = new List<AbilityTarget>();
		if (abilityOfActionType.IsAutoSelect())
		{
			AbilityTarget item = AbilityTarget.CreateSimpleAbilityTarget(actorData);
			list.Add(item);
		}
		else
		{
			for (int i = 0; i < abilityOfActionType.GetNumTargets(); i++)
			{
				AbilityTarget abilityTarget = null;
				BoardSquare currentBoardSquare = playerActor.GetCurrentBoardSquare();
				if (currentBoardSquare != null)
				{
					AbilityTarget abilityTarget2 = AbilityTarget.CreateAbilityTargetFromBoardSquare(currentBoardSquare, actorData.GetFreePos());
					if (abilityData.ValidateAbilityOnTarget(abilityOfActionType, abilityTarget2, i))
					{
						abilityTarget = abilityTarget2;
					}
				}
				if (abilityTarget == null)
				{
					flag = false;
					break;
				}
				list.Add(abilityTarget);
			}
		}
		if (flag)
		{
			botController.RequestAbility(list, actionType);
		}
	}
#endif
	
	public ActorData FindNearestEnemy()
	{
		ActorData nearestEnemy = null;
		ActorData actorData = GetComponent<ActorData>();
		BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
		float minDistToEnemy = float.MaxValue;
		List<ActorData> actors = GameFlowData.Get().GetActors();
		foreach (ActorData otherActor in actors)
		{
			if (otherActor.GetTeam() == actorData.GetTeam())
			{
				continue;
			}
			BoardSquare enemySquare = otherActor.GetCurrentBoardSquare();
			if (enemySquare == null)
			{
				continue;
			}
			float distToEnemy = currentBoardSquare.HorizontalDistanceOnBoardTo(enemySquare);
			if (distToEnemy < minDistToEnemy)
			{
				minDistToEnemy = distToEnemy;
				nearestEnemy = otherActor;
			}
		}
		return nearestEnemy;
	}

	public void MoveToDestination()
	{
		if (m_destination == null) return;
		ActorData actorData = GetComponent<ActorData>();
		ActorMovement actorMovement = GetComponent<ActorMovement>();
		ActorTurnSM actorTurnSM = GetComponent<ActorTurnSM>();
		BoardSquare targetSquare = Board.Get().GetSquareFromTransform(m_destination);
		if (actorData != null
		    && actorMovement != null
		    && actorTurnSM != null
		    && targetSquare != null)
		{
			BoardSquare closestMoveableSquareTo = actorMovement.GetClosestMoveableSquareTo(targetSquare, true);
			if (closestMoveableSquareTo != null
			    && closestMoveableSquareTo != actorData.GetCurrentBoardSquare())
			{
				actorTurnSM.SelectMovementSquareForMovement(closestMoveableSquareTo); // , true in rogues
			}
		}
	}

#if SERVER
	// added in rogues
	public override IEnumerator DecideAbilities()
	{
		switch (m_attackPattern)
		{
		case AttackPattern.AlwaysAttackPlayer:
			AttackActor(SinglePlayerManager.Get().GetLocalPlayer());
			break;
		case AttackPattern.AttackPlayerIfInLoS:
			if (IsPlayerInLoS())
			{
				AttackActor(SinglePlayerManager.Get().GetLocalPlayer());
			}
			break;
		case AttackPattern.AttackNearestEnemy:
		{
			ActorData actorData = FindNearestEnemy();
			if (actorData != null)
			{
				AttackActor(actorData);
			}
			break;
		}
		}
		switch (m_movementPattern)
		{
		case MovementPattern.MoveOncePlayerInLoS:
			if (IsPlayerInLoS())
			{
				MoveToDestination();
			}
			break;
		case MovementPattern.MoveAsap:
			MoveToDestination();
			break;
		}
		yield break;
	}
#endif
}
