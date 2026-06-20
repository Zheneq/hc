// ROGUES
// SERVER
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// added in rogues
#if SERVER // rogues?
public class NPCBrain_Constructed : NPCBrain_Adaptive
{
	public List<BrainBehavior> m_behaviors;
	public bool m_showMovementGizmos;
	public bool m_showTargetingGizmos;
	
	private BrainBehavior m_activeBehavior;
	private int m_distanceToNearestEnemy;
	private int m_distanceToFarthestEnemy;
	private int m_distanceToNearestAlly;
	private int m_distanceToFarthestAlly;

	private static float s_glanceScoreModifier = 0.05f;
	private static float s_hitScoreModifier = 0.3f;
	private static float s_critScoreModifier = 0.75f;
	private static float s_optimalRangeMovementScoreBase = 100f;

	private Dictionary<BoardSquare, float> m_squareScoresForGizmos;

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_Constructed npcbrain_Constructed = bot.gameObject.AddComponent<NPCBrain_Constructed>();
		npcbrain_Constructed.m_behaviors = m_behaviors;
		for (int i = 0; i < npcbrain_Constructed.m_behaviors.Count; i++)
		{
			npcbrain_Constructed.m_behaviors[i].DebugName = "Behavior " + i;
		}
		npcbrain_Constructed.m_showMovementGizmos = m_showMovementGizmos;
		npcbrain_Constructed.m_showTargetingGizmos = m_showTargetingGizmos;
		return npcbrain_Constructed;
	}

	public void Start()
	{
		m_logReasoning = true;
	}

	public void DecideBehavior()
	{
		ActorData component = GetComponent<ActorData>();
		if (component)
		{
			CacheCommonConditionalValuesForTurn(component);
			BrainBehavior brainBehavior = null;
			foreach (BrainBehavior brainBehavior2 in m_behaviors)
			{
				brainBehavior2.BehaviorDemandedTarget = null;
				bool flag = true;
				if (brainBehavior2.m_behaviorRequirements != null)
				{
					foreach (BrainBehaviorCondition brainBehaviorCondition in brainBehavior2.m_behaviorRequirements)
					{
						int currentCompareValue = 0;
						switch (brainBehaviorCondition.m_conditionType)
						{
						case BehaviorRequirementConditionType.Requirement_SelfHP:
							currentCompareValue = (int)(component.GetHitPointPercent() * 100f);
							break;
						case BehaviorRequirementConditionType.Requirement_AllyCount:
							currentCompareValue = GameFlowData.Get().GetAllTeamMembers(component.GetTeam()).Count;
							break;
						case BehaviorRequirementConditionType.Requirement_EnemyCount:
							currentCompareValue = component.GetOtherTeams().SelectMany(otherTeam => GameFlowData.Get().GetAllTeamMembers(otherTeam)).ToList().Count;
							break;
						case BehaviorRequirementConditionType.Requirement_TurnsInCombat:
						{
							BotController component2 = component.GetComponent<BotController>();
							if (component2 != null && component2.m_alertedTurn != -1)
							{
								currentCompareValue = GameFlowData.Get().CurrentTurn - component2.m_alertedTurn;
							}
							break;
						}
						case BehaviorRequirementConditionType.Requirement_NearbyEnemyHP_StrictTarget:
							using (List<ActorData>.Enumerator enumerator3 = AreaEffectUtils.GetActorsInRadius(component.GetLoSCheckPos(), brainBehavior2.m_nearbyDefinition, false, component, component.GetOtherTeams(), null).GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									ActorData actorData = enumerator3.Current;
									if (actorData.IsActorVisibleToSpecificClient(component))
									{
										currentCompareValue = (int)(actorData.GetHitPointPercent() * 100f);
										if (CompareCondition(brainBehaviorCondition.m_conditional, currentCompareValue, brainBehaviorCondition.m_conditionValue))
										{
											brainBehavior2.BehaviorDemandedTarget = actorData;
										}
									}
								}
								break;
							}
							goto IL_1C4;
						case BehaviorRequirementConditionType.Requirement_NearbyAllyHP_StrictTarget:
							goto IL_1C4;
						case BehaviorRequirementConditionType.Requirement_NearbyEnemyCount:
							goto IL_239;
						case BehaviorRequirementConditionType.Requirement_NearbyAllyCount:
							currentCompareValue = AreaEffectUtils.GetActorsInRadius(component.GetLoSCheckPos(), brainBehavior2.m_nearbyDefinition, false, component, component.GetTeam(), null).Count;
							break;
						}
						IL_27D:
						flag = CompareCondition(brainBehaviorCondition.m_conditional, currentCompareValue, brainBehaviorCondition.m_conditionValue);
						if (!flag)
						{
							break;
						}
						if (m_logReasoning)
						{
							Log.Info(Log.Category.AIBrain, "{0} considering Behavior {1}: {2} {3} {4}", component.name, brainBehavior2.DebugName, brainBehaviorCondition.m_conditionType.ToString(), brainBehaviorCondition.m_conditional.ToString(), brainBehaviorCondition.m_conditionValue);
						}
						continue;
						IL_1C4:
						using (List<ActorData>.Enumerator enumerator3 = AreaEffectUtils.GetActorsInRadius(component.GetLoSCheckPos(), brainBehavior2.m_nearbyDefinition, false, component, component.GetTeam(), null).GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								ActorData actorData2 = enumerator3.Current;
								currentCompareValue = (int)(actorData2.GetHitPointPercent() * 100f);
								if (CompareCondition(brainBehaviorCondition.m_conditional, currentCompareValue, brainBehaviorCondition.m_conditionValue))
								{
									brainBehavior2.BehaviorDemandedTarget = actorData2;
								}
							}
							goto IL_27D;
						}
						IL_239:
						currentCompareValue = AreaEffectUtils.GetActorsInRadius(component.GetLoSCheckPos(), brainBehavior2.m_nearbyDefinition, false, component, component.GetOtherTeams(), null).Count;
						goto IL_27D;
					}
				}
				if (flag)
				{
					if (m_logReasoning)
					{
						string text = (brainBehavior2.BehaviorDemandedTarget != null) ? (": " + brainBehavior2.BehaviorDemandedTarget.GetDisplayName()) : "";
						Log.Info(Log.Category.AIBrain, "{0} choosing Behavior {1} {2}", component.name, brainBehavior2.DebugName, text);
					}
					brainBehavior = brainBehavior2;
					if (!brainBehavior2.m_behaviorRequirements.IsNullOrEmpty())
					{
						break;
					}
				}
			}
			if (brainBehavior != null)
			{
				if (brainBehavior != m_activeBehavior)
				{
					m_activeBehavior = brainBehavior;
					OnEnterBehavior(brainBehavior);
				}
			}
			else
			{
				Debug.LogError("Brain could not pick a behavior. At least one behavior must be set with no requirements so it's always available.");
			}
		}
	}

	private void OnEnterBehavior(BrainBehavior b)
	{
		// rogues
		// if (b != null)
		// {
		// 	ActorTurnSM component = base.GetComponent<ActorTurnSM>();
		// 	if (component)
		// 	{
		// 		component.NumAbilityActionsPerTurn = (uint)b.m_abilityActionsPerTurn;
		// 	}
		// }
	}

	public override bool ShouldDoAbilityBeforeMovement()
	{
		return m_activeBehavior != null && m_activeBehavior.m_abilityBeforeMovement;
	}

	protected void CacheCommonConditionalValuesForTurn(ActorData actor)
	{
		m_distanceToNearestEnemy = int.MaxValue;
		m_distanceToFarthestEnemy = 0;
		m_distanceToNearestAlly = int.MaxValue;
		m_distanceToFarthestAlly = 0;
		if (actor.GetCurrentBoardSquare() != null)
		{
			foreach (ActorData actorData in GameFlowData.Get().GetActors())
			{
				if (actorData != actor && actorData.GetCurrentBoardSquare() != null)
				{
					float num = actor.GetCurrentBoardSquare().HorizontalDistanceInSquaresTo(actorData.GetCurrentBoardSquare());
					if (actorData.GetTeam() != actor.GetTeam())
					{
						if (num < m_distanceToNearestEnemy)
						{
							m_distanceToNearestEnemy = Mathf.RoundToInt(num);
						}
						if (num > m_distanceToFarthestEnemy)
						{
							m_distanceToFarthestEnemy = Mathf.RoundToInt(num);
						}
					}
					else
					{
						if (num < m_distanceToNearestAlly)
						{
							m_distanceToNearestAlly = Mathf.RoundToInt(num);
						}
						if (num > m_distanceToFarthestAlly)
						{
							m_distanceToFarthestAlly = Mathf.RoundToInt(num);
						}
					}
				}
			}
		}
	}

	public bool CompareCondition(Condition conditional, int currentCompareValue, int targetCompareValue)
	{
		bool result = false;
		switch (conditional)
		{
		case Condition.GreaterThan:
			result = (currentCompareValue > targetCompareValue);
			break;
		case Condition.GreaterThanOrEqualTo:
			result = (currentCompareValue >= targetCompareValue);
			break;
		case Condition.EqualTo:
			result = (currentCompareValue == targetCompareValue);
			break;
		case Condition.LessThanOrEqualTo:
			result = (currentCompareValue <= targetCompareValue);
			break;
		case Condition.LessThan:
			result = (currentCompareValue < targetCompareValue);
			break;
		case Condition.NotEqualTo:
			result = (currentCompareValue != targetCompareValue);
			break;
		}
		return result;
	}

	// rogues
	// public int[] EstimateHitChance(ActorData caster, ActorData target, Ability ability)
	// {
	// 	EquipmentStats equipmentStats = caster.GetEquipmentStats();
	// 	EquipmentStats equipmentStats2 = target.GetEquipmentStats();
	// 	int num = caster.GetBaseStatValue(GearStatType.AccuracyAdjustment);
	// 	int cachedActionType = (int)ability.CachedActionType;
	// 	num = Mathf.RoundToInt(equipmentStats.GetTotalStatValueForSlot(GearStatType.AccuracyAdjustment, (float)num, cachedActionType, target));
	// 	num += ability.GetAccuracyAdjust();
	// 	float dist = ability.CalcDistForAccuracyAdjust(target, caster);
	// 	int num2 = ability.GetProximityAccuAdjustData(caster).CalcAdjustAmount(dist, 0f);
	// 	num += num2;
	// 	int num3 = target.GetBaseStatValue(GearStatType.DefenseAdjustment);
	// 	num3 = Mathf.RoundToInt(equipmentStats2.GetTotalStatValueForSlot(GearStatType.DefenseAdjustment, (float)num3, -1, target));
	// 	int[] result = null;
	// 	if (!ability.IgnoreAccuracySystem() && !ability.ConvertMissToGlance())
	// 	{
	// 		HitChanceBracketType hitChanceBracketType = HitChanceBracketType.Default;
	// 		ActorCover component = target.GetComponent<ActorCover>();
	// 		if (component != null && !ability.ForceIgnoreCover(caster))
	// 		{
	// 			component.IsInCoverWrt(caster.GetLoSCheckPos(), out hitChanceBracketType);
	// 		}
	// 		int startAdjustGlance = EquipmentStats.CalcBracketGlanceStartAdjustment(equipmentStats, equipmentStats2, cachedActionType, target);
	// 		int startAdjustCrit = EquipmentStats.CalcBracketCriticalStartAdjustment(equipmentStats, equipmentStats2, cachedActionType, target);
	// 		int startAdjustDodge = EquipmentStats.CalcBracketDodgeStartAdjustment(equipmentStats, equipmentStats2, cachedActionType, target);
	// 		int startAdjustBlock = EquipmentStats.CalcBracketBlockStartAdjustment(equipmentStats, equipmentStats2, cachedActionType, hitChanceBracketType, target);
	// 		result = PveGameplayData.Get().GetTargeterPreviewChances(num, num3, hitChanceBracketType, startAdjustGlance, startAdjustCrit, startAdjustDodge, startAdjustBlock);
	// 	}
	// 	return result;
	// }

	public override IEnumerator DecideAbilities()
	{
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		ActorData component = GetComponent<ActorData>();
		bool useFastBotAI = hydrogenConfig.UseFastBotAI;
		AbilityData component2 = GetComponent<AbilityData>();
		ActorTurnSM component3 = GetComponent<ActorTurnSM>();
		BotController component4 = GetComponent<BotController>();
		if (!component2 || !component || !component3 || !component4 || m_activeBehavior == null)
		{
			yield break;
		}
		if (m_potentialChoices == null)
		{
			m_potentialChoices = new Dictionary<AbilityData.ActionType, PotentialChoice>();
		}
		for (int i = 4; i > -1; i--)
		{
			AbilityData.ActionType actionType = (AbilityData.ActionType)i;
			Ability abilityOfActionType = component2.GetAbilityOfActionType(actionType);
			if (!(abilityOfActionType == null))
			{
				if (m_activeBehavior.m_abilities.Length <= i)
				{
					Debug.LogError("Brain setup error for " + component.name + ". Make sure the Abilities list for behaviors has exactly 5 entries");
				}
				else if (m_activeBehavior.m_abilities[i] != null)
				{
					if (!component2.ValidateActionIsRequestable(actionType))
					{
						Log.Info(Log.Category.AIBrain, string.Concat(new object[]
						{
							"Brain action not requestable ",
							actionType,
							// ": ",
							// component.GetActorTurnSM().GetUsedActionsDebugString()
						}));
					}
					else
					{
						ScoreAbility(abilityOfActionType, m_activeBehavior.m_abilities[(int)abilityOfActionType.CachedActionType], m_activeBehavior.BehaviorDemandedTarget);
					}
				}
			}
		}
		if (m_potentialChoices.Count == 0)
		{
			Log.Warning(Log.Category.AIBrain, "[WARNING] {0} NPC Brain found no valid choices", component.name);
		}
		else
		{
			AbilityData.ActionType actionType2 = AbilityData.ActionType.INVALID_ACTION;
			ConstructedChoice constructedChoice = null;
			int num = 0;
			int num2 = GameplayRandom.Range(0, m_potentialChoices.Count);
			foreach (KeyValuePair<AbilityData.ActionType, PotentialChoice> keyValuePair in m_potentialChoices)
			{
				ConstructedChoice constructedChoice2 = keyValuePair.Value as ConstructedChoice;
				if (constructedChoice2 != null)
				{
					bool flag = false;
					if (constructedChoice == null)
					{
						flag = true;
					}
					else
					{
						switch (m_activeBehavior.m_tiebreakerRule)
						{
						case AbilityTiebreakerType.Random:
							flag = (num == num2);
							break;
						case AbilityTiebreakerType.HighestDamage:
							flag = (constructedChoice2.damageTotal > constructedChoice.damageTotal);
							break;
						case AbilityTiebreakerType.MostKills:
							flag = (constructedChoice2.lethalCount > constructedChoice.lethalCount);
							break;
						case AbilityTiebreakerType.HighestHealing:
							flag = (constructedChoice2.healingTotal > constructedChoice.healingTotal);
							break;
						case AbilityTiebreakerType.Score:
							flag = (constructedChoice2.score > constructedChoice.score);
							break;
						case AbilityTiebreakerType.HighestCooldown:
							flag = (component2.GetAbilityOfActionType(keyValuePair.Key).GetModdedCooldown() > component2.GetAbilityOfActionType(actionType2).GetModdedCooldown());
							break;
						case AbilityTiebreakerType.LowestCooldown:
							flag = (component2.GetAbilityOfActionType(keyValuePair.Key).GetModdedCooldown() < component2.GetAbilityOfActionType(actionType2).GetModdedCooldown());
							break;
						}
					}
					num++;
					if (flag)
					{
						actionType2 = keyValuePair.Key;
						constructedChoice = constructedChoice2;
					}
				}
			}
			BoardSquare dashTarget = null;
			if (actionType2 != AbilityData.ActionType.INVALID_ACTION && constructedChoice != null && constructedChoice.score > 0f)
			{
				Ability abilityOfActionType2 = component2.GetAbilityOfActionType(actionType2);
				dashTarget = constructedChoice.destinationSquare;
				int num3 = (int)(1 + actionType2);
				if (num3 > 5)
				{
					num3 -= 2;
				}
				if (m_logReasoning)
				{
					Log.Info(Log.Category.AIBrain, "{0} choosing ability {1} - {2} - Reasoning: {3} Final score: {4}", component.name, num3, abilityOfActionType2.m_abilityName, constructedChoice.reasoning, constructedChoice.score);
				}
				if (m_sendReasoningToTeamChat)
				{
					UIQueueListPanel.UIPhase uiphaseFromAbilityPriority = UIQueueListPanel.GetUIPhaseFromAbilityPriority(abilityOfActionType2.GetRunPriority());
					string text = "<color=red>";
					if (uiphaseFromAbilityPriority == UIQueueListPanel.UIPhase.Prep)
					{
						text = "<color=green>";
					}
					else if (uiphaseFromAbilityPriority == UIQueueListPanel.UIPhase.Evasion)
					{
						text = "<color=yellow>";
					}
					ServerGameManager.Get().SendUnlocalizedConsoleMessage(string.Format("<color=white>{0}</color> choosing ability {5}{1} - {2}</color> - Reasoning:\n{3}Final Score: <color=yellow>{4}</color>", component.DisplayName, num3, abilityOfActionType2.m_abilityName, constructedChoice.reasoning, constructedChoice.score, text), Team.Invalid, ConsoleMessageType.TeamChat, component.DisplayName);
				}
				if (abilityOfActionType2.IsSimpleAction())
				{
					GetComponent<ServerActorController>().ProcessCastSimpleActionRequest(actionType2, true);
				}
				else
				{
					component4.RequestAbility(constructedChoice.targetList, actionType2);
				}
			}
			else if (m_logReasoning)
			{
				Log.Info(Log.Category.AIBrain, "{0} failed choosing ability {1} out of {2} potential choices.", component.name, (constructedChoice == null) ? "null" : (constructedChoice.score + ": " + constructedChoice.reasoning), m_potentialChoices.Count);
				foreach (KeyValuePair<AbilityData.ActionType, PotentialChoice> keyValuePair2 in m_potentialChoices)
				{
					Log.Info(" -{0}: {1}", keyValuePair2.Key.ToString(), (keyValuePair2.Value == null) ? "null" : keyValuePair2.Value.ToString());
				}
			}
			BotManager.Get().BotAIAbilitySelected(component, dashTarget, 0f);
			m_potentialChoices.Clear();
		}
	}

	public override IEnumerator DecideMovement()
	{
		if (m_activeBehavior == null)
		{
			yield break;
		}
		ActorData component = GetComponent<ActorData>();
		yield return StartCoroutine(DoMovement(component, m_activeBehavior.BehaviorDemandedTarget, m_activeBehavior.m_movementOptimalRangeFromEnemy, m_activeBehavior.m_movementOptimalRangeFromAlly, m_activeBehavior.m_movementOptimalRangeWindowEnemy, m_activeBehavior.m_movementOptimalRangeWindowAlly, m_activeBehavior.m_movementTooCloseLinearPenalty, m_activeBehavior.m_movementTooFarLinearPenalty, m_activeBehavior.m_movementNoLoSToEnemyPenalty, m_activeBehavior.m_movementNoLoSToAllyPenalty, !m_activeBehavior.m_abilityBeforeMovement, m_activeBehavior.m_movementEnemyScoreMult, m_activeBehavior.m_movementAllyScoreMult, m_activeBehavior.m_movementLowerHealthScoreMult, m_activeBehavior.m_movementCoverScoreMult, m_activeBehavior.m_movementSprintScorePenalty, m_activeBehavior.m_movementStandStillScorePenalty, m_activeBehavior.m_engagementRange));
	}

	public bool HasLOSToTeamFromSquare(Team team, BoardSquare square)
	{
		bool result = false;
		if (square != null)
		{
			foreach (ActorData actorData in GameFlowData.Get().GetAllTeamMembers(team))
			{
				BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
				if (currentBoardSquare != null && square.GetLOS(currentBoardSquare.x, currentBoardSquare.y))
				{
					result = true;
					break;
				}
			}
		}
		return result;
	}

	private float EvaluateSquareForMovementRelatingToActor(BoardSquare square, ActorData target, float distance, float optimalRange, float optimalRangeWindow, float tooCloseLinearPenalty, float tooFarLinearPenalty, float scoreMult, ref bool hasTargetInPreferredRange, ref string reasoning)
	{
		float num = 0f;
		if (distance < optimalRange - optimalRangeWindow)
		{
			float num2 = optimalRange - optimalRangeWindow - distance;
			float num3 = s_optimalRangeMovementScoreBase - num2 * tooCloseLinearPenalty;
			num3 *= scoreMult;
			num += num3;
			hasTargetInPreferredRange = true;
			reasoning = string.Concat(reasoning, num3, " for closer than optimal ", target.name, ": ", distance, " ");
		}
		else if (distance > optimalRange + optimalRangeWindow)
		{
			float num4 = distance - (optimalRange + optimalRangeWindow);
			float num5 = s_optimalRangeMovementScoreBase - num4 * tooFarLinearPenalty;
			num5 *= scoreMult;
			num += num5;
			reasoning = string.Concat(reasoning, num5, " for farther than optimal ", target.name, ": ", distance, " ");
		}
		else
		{
			float num6 = s_optimalRangeMovementScoreBase;
			num6 *= scoreMult;
			num += num6;
			hasTargetInPreferredRange = true;
			reasoning = string.Concat(reasoning, num6, " for optimal range ", target.name, ": ", distance, " ");
		}
		return num;
	}

	private IEnumerator DoMovement(ActorData actorData, ActorData demandedTarget, float optimalRangeFromEnemy, float optimalRangeFromAlly, float optimalRangeWindowEnemy, float optimalRangeWindowAlly, float tooCloseLinearPenalty, float tooFarLinearPenalty, float noEnemyLoSPenalty, float noAllyLoSPenalty, bool avoidSprint, float enemyScoreMult, float allyScoreMult, float lowerHealthScoreMult, float coverScoreMult, float sprintScorePenalty, float standStillScorePenalty, float engagementRange)
	{
		ActorMovement actorMovement = actorData.GetActorMovement();
		ActorTurnSM turnSM = actorData.GetActorTurnSM();
		HydrogenConfig.Get();
		float remainingHorizontalMovement = actorData.RemainingHorizontalMovement;
		HashSet<BoardSquare> squares = actorMovement.SquaresCanMoveTo;
		HashSet<BoardSquare> nonSprintSquares = actorMovement.SquaresCanMoveToWithQueuedAbility;
		float bestSquareScore = -99999f;
		string bestReasoning = "";
		BoardSquare startingSquare = actorData.GetCurrentBoardSquare();
		BoardSquare bestSquare = startingSquare;
		List<ActorData> players = GameFlowData.Get().GetActors();
		bool anyActorsInEngagementRange = false;
		do
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			foreach (BoardSquare boardSquare in squares)
			{
				float num = 0f;
				int num2 = 0;
				string text = "";
				bool flag = false;
				if (demandedTarget != null && demandedTarget.GetCurrentBoardSquare() && !demandedTarget.IsDead())
				{
					float distance = boardSquare.HorizontalDistanceOnBoardTo(BotManager.Get().GetPendingDestinationOrCurrentSquare(demandedTarget, actorData));
					bool flag2 = actorData.GetOtherTeams().Contains(demandedTarget.GetTeam());
					float optimalRange = flag2 ? optimalRangeFromEnemy : optimalRangeFromAlly;
					float optimalRangeWindow = flag2 ? optimalRangeWindowEnemy : optimalRangeWindowAlly;
					float scoreMult = flag2 ? enemyScoreMult : allyScoreMult;
					float num3 = EvaluateSquareForMovementRelatingToActor(boardSquare, demandedTarget, distance, optimalRange, optimalRangeWindow, tooCloseLinearPenalty, tooFarLinearPenalty, scoreMult, ref flag, ref text);
					num += num3;
					num2 = 1;
				}
				else
				{
					foreach (ActorData actorData2 in players)
					{
						if (!(actorData2 == actorData) && actorData2 && actorData2.GetCurrentBoardSquare() && !actorData2.IsDead())
						{
							bool flag3 = actorData.GetOtherTeams().Contains(actorData2.GetTeam());
							float num4 = flag3 ? optimalRangeFromEnemy : optimalRangeFromAlly;
							float optimalRangeWindow2 = flag3 ? optimalRangeWindowEnemy : optimalRangeWindowAlly;
							float num5 = flag3 ? enemyScoreMult : allyScoreMult;
							float num6 = lowerHealthScoreMult * (1f - actorData2.GetHitPointPercent());
							num5 += num6;
							if (num4 >= 0f)
							{
								float num7 = boardSquare.HorizontalDistanceOnBoardTo(BotManager.Get().GetPendingDestinationOrCurrentSquare(actorData2, actorData));
								if (num7 <= engagementRange)
								{
									num2++;
									float num8 = EvaluateSquareForMovementRelatingToActor(boardSquare, actorData2, num7, num4, optimalRangeWindow2, tooCloseLinearPenalty, tooFarLinearPenalty, num5, ref flag, ref text);
									num += num8;
								}
							}
						}
					}
				}
				if (num2 > 0)
				{
					num /= num2;
					anyActorsInEngagementRange = true;
				}
				else
				{
					num -= 999f;
					text = string.Concat(text, "zero players considered within the ", engagementRange, " engagementRange ");
				}
				ActorCover component = actorData.GetComponent<ActorCover>();
				if (component != null)
				{
					float num9 = component.CoverRating(boardSquare) * coverScoreMult;  // CoverRating(boardSquare, engagementRange) in rogeus
					num += num9;
					text = text + num9 + " for cover\n";
				}
				if (boardSquare.OccupantActor != null && boardSquare.OccupantActor != actorData)
				{
					num -= 300f;
					text += "-300 for occupied ";
				}
				if (noEnemyLoSPenalty != 0f && !HasLOSToTeamFromSquare(actorData.GetEnemyTeam(), boardSquare))
				{
					num -= noEnemyLoSPenalty;
					text = string.Concat(text, "-", noEnemyLoSPenalty, " for no enemy LoS ");
				}
				if (noAllyLoSPenalty != 0f && !HasLOSToTeamFromSquare(actorData.GetTeam(), boardSquare))
				{
					num -= noAllyLoSPenalty;
					text = string.Concat(text, "-", noAllyLoSPenalty, " for no ally LoS ");
				}
				if (avoidSprint && !nonSprintSquares.Contains(boardSquare))
				{
					num -= sprintScorePenalty;
					text = text + -sprintScorePenalty + " for sprinting ";
				}
				if (boardSquare.GetGridPos().CoordsEqual(startingSquare.GetGridPos()))
				{
					num -= standStillScorePenalty;
					text = text + -standStillScorePenalty + " for standing still ";
				}
				else
				{
					num += standStillScorePenalty;
					text = text + standStillScorePenalty + " for not standing still ";
				}
				if (bestSquareScore < num)
				{
					bestReasoning = string.Format("Bot {0} at {1},{2} moving to {3},{4} with score {5} that is better than our previous best {6}. {7}", actorData.name, startingSquare.GetGridPos().x, startingSquare.GetGridPos().y, boardSquare.GetGridPos().x, boardSquare.GetGridPos().y, num, bestSquareScore, text);
					bestSquareScore = num;
					bestSquare = boardSquare;
				}
				if (realtimeSinceStartup + 1f < Time.realtimeSinceStartup)
				{
					yield return null;
					realtimeSinceStartup = Time.realtimeSinceStartup;
				}
			}
			HashSet<BoardSquare>.Enumerator enumerator = default(HashSet<BoardSquare>.Enumerator);
			if (!anyActorsInEngagementRange)
			{
				if (engagementRange > 100f)
				{
					break;
				}
				engagementRange = 101f;
				bestSquareScore = -99999f;
				bestReasoning = "";
				bestSquare = startingSquare;
				if (m_showMovementGizmos && !m_showTargetingGizmos)
				{
					m_squareScoresForGizmos = new Dictionary<BoardSquare, float>();
				}
			}
		}
		while (!anyActorsInEngagementRange);
		if (m_logReasoning)
		{
			Log.Info(Log.Category.AIBrain, bestReasoning);
		}
		if (bestSquare != startingSquare)
		{
			turnSM.SelectMovementSquareForMovement(bestSquare); // , true in rogues
			BotManager.Get().SelectDestination(actorData, bestSquare);
		}
		yield break;
		yield break;
	}

	private void OnDrawGizmos()
	{
	}

	private TargetingPriority ScoreAbility(Ability ability, BrainAbility brainAbility, ActorData demandedTarget)
	{
		ActorData component = GetComponent<ActorData>();
		float currentRangeInSquares = AbilityUtils.GetCurrentRangeInSquares(ability, component, 0);
		int num = 0;
		int num2 = 0;
		List<AbilityTarget> list = GeneratePotentialAbilityTargetLocations(currentRangeInSquares, brainAbility.m_includeEnemies, brainAbility.m_includeAllies, brainAbility.m_includeSelf);
		List<AbilityTarget> list2 = new List<AbilityTarget>();
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		TargetingPriority targetingPriority = TargetingPriority.Priority_Ignored;
		if (brainAbility.m_targetingConditions.Count == 0)
		{
			Log.Warning(Log.Category.AIBrain, "[WARNING] {0} has no targeting conditions for ability {1} in {2} and will never be used!", component.name, ability.m_abilityName, m_activeBehavior.name);
		}
		foreach (AbilityTarget item in list)
		{
			list2.Clear();
			list2.Add(item);
			List<ActorData> hitActors = ability.GetHitActors(list2, component, nonActorTargetInfo);
			if (hitActors.Count > 0)
			{
				num++;
				foreach (BrainTargetingCondition brainTargetingCondition in brainAbility.m_targetingConditions)
				{
					if (targetingPriority <= brainTargetingCondition.m_priority)
					{
						ConstructedChoice constructedChoice = new ConstructedChoice();
						TargetingPriority targetingPriority2 = ScoreAbilityCondition(ability, brainTargetingCondition, hitActors, component, list2, ref constructedChoice);
						if (targetingPriority2 >= targetingPriority)
						{
							targetingPriority = targetingPriority2;
							constructedChoice.targetList = new List<AbilityTarget>();
							constructedChoice.targetList.Add(item);
							constructedChoice.score = (float)targetingPriority * 100 + hitActors.Count;
							if (hitActors.Contains(demandedTarget))
							{
								constructedChoice.score += 100f;
							}
							EvaluatePotentialChoice(ability.CachedActionType, constructedChoice);
							num2++;
						}
					}
				}
			}
		}
		Log.Info(Log.Category.AIBrain, "{0} evaluated {1} potential hits out of {2} possible targets for ability {3}. {4} valid choices considered.", component.name, num.ToString(), list.Count.ToString(), ability.m_abilityName, num2.ToString());
		return targetingPriority;
	}

	private TargetingPriority ScoreAbilityCondition(Ability ability, BrainTargetingCondition brainCondition, List<ActorData> hitActors, ActorData caster, List<AbilityTarget> abilityTargets, ref ConstructedChoice constructedChoice)
	{
		if (brainCondition.m_conditionType == TargetingConditionType.Condition_None)
		{
			return brainCondition.m_priority;
		}
		if (brainCondition.m_conditionType == TargetingConditionType.Condition_SelfHP)
		{
			if (CompareCondition(brainCondition.m_conditional, (int)(caster.GetHitPointPercent() * 100f), brainCondition.m_conditionValue))
			{
				return brainCondition.m_priority;
			}
		}
		else
		{
			if (brainCondition.m_conditionType != TargetingConditionType.Condition_TargetCount)
			{
				if (brainCondition.m_conditionType == TargetingConditionType.Condition_TargetHighestHP || brainCondition.m_conditionType == TargetingConditionType.Condition_TargetLowestHP)
				{
					using (List<ActorData>.Enumerator enumerator = hitActors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData actorData = enumerator.Current;
							if (CompareCondition(brainCondition.m_conditional, (int)(actorData.GetHitPointPercent() * 100f), brainCondition.m_conditionValue))
							{
								constructedChoice.highestTargetHP = (int)(actorData.GetHitPointPercent() * 100f);
								constructedChoice.lowestTargetHP = (int)(actorData.GetHitPointPercent() * 100f);
								return brainCondition.m_priority;
							}
						}
						return TargetingPriority.Priority_Ignored;
					}
				}
				if (brainCondition.m_conditionType == TargetingConditionType.Condition_TotalHealing || brainCondition.m_conditionType == TargetingConditionType.Condition_TotalDamage)
				{
					AbilityResults abilityResults = new AbilityResults(caster, ability, null, s_gatherRealResults, true);
					ability.GatherAbilityResults(abilityTargets, caster, ref abilityResults);
					int num = 0;
					if (brainCondition.m_conditionType == TargetingConditionType.Condition_TotalDamage)
					{
						using (Dictionary<ActorData, ActorHitResults>.Enumerator enumerator2 = abilityResults.m_actorToHitResults.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								KeyValuePair<ActorData, ActorHitResults> keyValuePair = enumerator2.Current;
								int num2 = keyValuePair.Value.m_hitParameters.Target.HitPoints;
								num2 = Mathf.Max(num2, keyValuePair.Value.FinalDamage);
								num += num2;
							}
							goto IL_22B;
						}
					}
					foreach (KeyValuePair<ActorData, ActorHitResults> keyValuePair2 in abilityResults.m_actorToHitResults)
					{
						int num3 = keyValuePair2.Value.m_hitParameters.Target.GetMaxHitPoints() - keyValuePair2.Value.m_hitParameters.Target.HitPoints;
						num3 = Mathf.Min(num3, keyValuePair2.Value.FinalHealing);
						num += num3;
					}
					IL_22B:
					if (CompareCondition(brainCondition.m_conditional, num, brainCondition.m_conditionValue))
					{
						if (brainCondition.m_conditionType == TargetingConditionType.Condition_TotalDamage)
						{
							constructedChoice.damageTotal = num;
						}
						else
						{
							constructedChoice.healingTotal = num;
						}
						return brainCondition.m_priority;
					}
					return TargetingPriority.Priority_Ignored;
				}

				if (brainCondition.m_conditionType >= TargetingConditionType.Condition_DistanceToNearestTarget && brainCondition.m_conditionType <= TargetingConditionType.Condition_DistanceToFarthestAlly)
				{
					using (List<ActorData>.Enumerator enumerator = hitActors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData actorData2 = enumerator.Current;
							if (((brainCondition.m_conditionType != TargetingConditionType.Condition_DistanceToFarthestAlly && brainCondition.m_conditionType != TargetingConditionType.Condition_DistanceToNearestAlly) || actorData2.GetTeam() == caster.GetTeam()) && ((brainCondition.m_conditionType != TargetingConditionType.Condition_DistanceToFarthestEnemy && brainCondition.m_conditionType != TargetingConditionType.Condition_DistanceToNearestEnemy) || actorData2.GetTeam() != caster.GetTeam()))
							{
								Vector3 vector = caster.GetFreePos() - actorData2.GetFreePos();
								if (CompareCondition(brainCondition.m_conditional, (int)(vector.magnitude / Board.Get().squareSize), brainCondition.m_conditionValue))
								{
									constructedChoice.distanceToFarthestTarget = (int)(vector.magnitude / Board.Get().squareSize);
									constructedChoice.distanceToNearestTarget = (int)(vector.magnitude / Board.Get().squareSize);
									return brainCondition.m_priority;
								}
							}
						}
						return TargetingPriority.Priority_Ignored;
					}
				}
				if (brainCondition.m_conditionType == TargetingConditionType.Condition_HitChance
				    || brainCondition.m_conditionType == TargetingConditionType.Condition_CritChance)
				{
					// rogues
					// HitChanceBracket.HitType hitType = (brainCondition.m_conditionType == TargetingConditionType.Condition_HitChance) ? HitChanceBracket.HitType.Normal : HitChanceBracket.HitType.Crit;
					// caster.GetBaseStatValue(GearStatType.AccuracyAdjustment);
					// caster.GetEquipmentStats();
					// foreach (ActorData target in hitActors)
					// {
					// 	int[] array = this.EstimateHitChance(caster, target, ability);
					// 	int bestHitChance = array[4] + array[3];
					// 	if (this.CompareCondition(brainCondition.m_conditional, array[(int)hitType], brainCondition.m_conditionValue))
					// 	{
					// 		constructedChoice.bestCritChance = array[4];
					// 		constructedChoice.bestHitChance = bestHitChance;
					// 		return brainCondition.m_priority;
					// 	}
					// }
					return TargetingPriority.Priority_Ignored;
				}
				if (brainCondition.m_conditionType != TargetingConditionType.Condition_LethalCount)
				{
					return TargetingPriority.Priority_Ignored;
				}
				AbilityResults abilityResults2 = new AbilityResults(caster, ability, null, s_gatherRealResults, true);
				ability.GatherAbilityResults(abilityTargets, caster, ref abilityResults2);
				int num4 = 0;
				foreach (KeyValuePair<ActorData, ActorHitResults> keyValuePair3 in abilityResults2.m_actorToHitResults)
				{
					if (keyValuePair3.Value.FinalDamage > keyValuePair3.Value.m_hitParameters.Target.HitPoints)
					{
						num4++;
					}
				}
				if (CompareCondition(brainCondition.m_conditional, num4, brainCondition.m_conditionValue))
				{
					constructedChoice.lethalCount = num4;
					return brainCondition.m_priority;
				}
				return TargetingPriority.Priority_Ignored;
				TargetingPriority result;
				return result;
			}
			if (CompareCondition(brainCondition.m_conditional, hitActors.Count, brainCondition.m_conditionValue))
			{
				return brainCondition.m_priority;
			}
		}
		return TargetingPriority.Priority_Ignored;
	}

	private void EvaluatePotentialChoice(AbilityData.ActionType abilityActionType, ConstructedChoice choice)
	{
		if (m_potentialChoices.Count == 0)
		{
			m_potentialChoices.Add(abilityActionType, choice);
			return;
		}
		float num = 0f;
		foreach (KeyValuePair<AbilityData.ActionType, PotentialChoice> keyValuePair in m_potentialChoices)
		{
			if (keyValuePair.Value.score > num)
			{
				num = keyValuePair.Value.score;
				AbilityData.ActionType key = keyValuePair.Key;
			}
		}
		if (choice.score > num)
		{
			m_potentialChoices.Clear();
			m_potentialChoices.Add(abilityActionType, choice);
			return;
		}
		if (choice.score == num && !m_potentialChoices.ContainsKey(abilityActionType))
		{
			m_potentialChoices.Add(abilityActionType, choice);
		}
	}
}
#endif
