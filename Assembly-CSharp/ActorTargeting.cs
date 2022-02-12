// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ActorTargeting : NetworkBehaviour,
	// removed in rogues
	IGameEventListener
{
	public class AbilityRequestData : IComparable
	{
		public AbilityData.ActionType m_actionType;
		public List<AbilityTarget> m_targets;

		public AbilityRequestData(AbilityData.ActionType actionType, List<AbilityTarget> targets)
		{
			m_actionType = actionType;
			m_targets = targets;
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			AbilityRequestData abilityRequestData = obj as AbilityRequestData;
			if (abilityRequestData == null)
			{
				throw new ArgumentException("Object is not an AbilityRequestData");
			}
			return m_actionType.CompareTo(abilityRequestData.m_actionType);
		}
	}

	private static List<AbilityRequestData> s_emptyRequestDataList = new List<AbilityRequestData>();

	private ActorData m_actorData;
	private bool m_targetersBeingDrawn;
	private Dictionary<ActorData, Dictionary<AbilityTooltipSymbol, int>> m_currentTargetedActors;

	public static bool m_showStatusAdjustedTargetingNumbers = true;
	private static TargetingNumberUpdateScratch s_targetingNumberUpdateScratch = new TargetingNumberUpdateScratch();

	private bool m_markedForForceRedraw;

	private static float s_lastTimeAddedAbilityCooldownsActor = 0f;
	private static List<ActorData> s_updatedAbilityCooldownsActors = new List<ActorData>();

	private void Awake()
	{
		m_actorData = GetComponent<ActorData>();
		m_currentTargetedActors = new Dictionary<ActorData, Dictionary<AbilityTooltipSymbol, int>>();
	}

	// removed in rogues
	private void Start()
	{
		if (NetworkClient.active)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReconnectReplayStateChanged);
		}
	}

	private void ResetTargeters(bool clearInstantly)  // no params in rogues
	{
		if (m_actorData == null)
		{
			m_actorData = GetComponent<ActorData>();
		}
		ActorTurnSM actorTurnSM = m_actorData.GetActorTurnSM();
		AbilityData abilityData = m_actorData.GetAbilityData();
		ActorData activeOwnedActorData = GameFlowData.Get()?.activeOwnedActorData;
		for (int i = 0; i < AbilityData.NUM_ACTIONS; i++)
		{
			AbilityData.ActionType type = (AbilityData.ActionType)i;
			Ability abilityOfActionType = abilityData.GetAbilityOfActionType(type);
			if (abilityOfActionType != null
				&& (m_actorData != activeOwnedActorData
					|| actorTurnSM == null
					|| actorTurnSM.CurrentState != TurnStateEnum.TARGETING_ACTION
					|| !abilityOfActionType.IsAbilitySelected()))
			{
				foreach (AbilityUtil_Targeter targeter in abilityOfActionType.Targeters)
				{
					targeter?.ResetTargeter(clearInstantly);  // no params in rogues
				}
			}
		}
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.TurnOffTargetingAbilityIndicator(m_actorData, 0);
		}
		m_targetersBeingDrawn = false;
	}

	public void OnRequestDataDeserialized()
	{
		ResetTargeters(false);  // no params in rogues
		if (ShouldDrawTargeters())
		{
			DrawTargeters();
			CalculateTargetedActors();
		}
	}

	public List<AbilityRequestData> GetAbilityRequestDataForClient()
	{
		if (m_actorData != null && m_actorData.TeamSensitiveData_authority != null)
		{
			return m_actorData.TeamSensitiveData_authority.GetAbilityRequestData();
		}
		return s_emptyRequestDataList;
	}

	private void Update()
	{
		// server-only
#if SERVER
		if (NetworkServer.active)
		{
			SynchWithActionBuffer();
		}
#endif
		bool flag = ShouldDrawTargeters();
		if (m_markedForForceRedraw)
		{
			ResetTargeters(false);  // no params in rogues
			m_targetersBeingDrawn = false;
			m_markedForForceRedraw = false;
		}
		if (flag && !m_targetersBeingDrawn)
		{
			DrawTargeters();
			CalculateTargetedActors();
		}
		else if (!flag && m_targetersBeingDrawn)
		{
			ResetTargeters(false);  // no params in rogues
		}
	}

	private void LateUpdate()
	{
		if (ShouldDrawTargeters() && m_targetersBeingDrawn)
		{
			UpdateDrawnTargeters();
		}
		else if (ShouldDrawAbilityCooldowns())
		{
			UpdateAbilityCooldowns(0, true);
		}
		else
		{
			ClearAbilityCooldowns();
		}
	}

	// added in rogues
#if SERVER
	private void SynchWithActionBuffer()
	{
		ActorData actorData = m_actorData;
		if (actorData == null || actorData.TeamSensitiveData_friendly == null)
		{
			return;
		}
		List<AbilityRequestData> abilityRequestData = actorData.TeamSensitiveData_friendly.GetAbilityRequestData();
		List<AbilityRequestData> pendingAbilityRequestsForTargeting = ServerActionBuffer.Get().GetPendingAbilityRequestsForTargeting(actorData);
		if (pendingAbilityRequestsForTargeting.Count > 1)
		{
			pendingAbilityRequestsForTargeting.Sort();
		}
		bool flag = false;
		if (pendingAbilityRequestsForTargeting.Count != abilityRequestData.Count)
		{
			flag = true;
		}
		else
		{
			for (int i = 0; i < pendingAbilityRequestsForTargeting.Count; i++)
			{
				AbilityRequestData abilityRequestData2 = abilityRequestData[i];
				AbilityRequestData abilityRequestData3 = pendingAbilityRequestsForTargeting[i];
				if (abilityRequestData2.m_actionType != abilityRequestData3.m_actionType || abilityRequestData2.m_targets.Count != abilityRequestData3.m_targets.Count)
				{
					flag = true;
					break;
				}
				for (int j = 0; j < abilityRequestData3.m_targets.Count; j++)
				{
					AbilityTarget abilityTarget = abilityRequestData[i].m_targets[j];
					AbilityTarget abilityTarget2 = pendingAbilityRequestsForTargeting[i].m_targets[j];
					if (abilityTarget.AimDirection != abilityTarget2.AimDirection || abilityTarget.FreePos != abilityTarget2.FreePos || abilityTarget.GridPos.x != abilityTarget2.GridPos.x || abilityTarget.GridPos.y != abilityTarget2.GridPos.y)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		if (flag)
		{
			ResetTargeters(false);  // no params in rogues
			m_actorData.TeamSensitiveData_friendly.SetAbilityRequestData(pendingAbilityRequestsForTargeting);
			m_actorData.TeamSensitiveData_friendly.MarkAsDirty(ActorTeamSensitiveData.DirtyBit.AbilityRequestDataForTargeter);
		}
	}
#endif

	public List<AbilityTarget> GetAbilityTargetsInRequest(AbilityData.ActionType actionType)
	{
		List<AbilityRequestData> abilityRequestDataForClient = GetAbilityRequestDataForClient();
		if (abilityRequestDataForClient != null)
		{
			for (int i = 0; i < abilityRequestDataForClient.Count; i++)
			{
				AbilityRequestData abilityRequestData = abilityRequestDataForClient[i];
				if (abilityRequestData != null && abilityRequestData.m_actionType == actionType)
				{
					return abilityRequestData.m_targets;
				}
			}
		}
		return null;
	}

	private void CalculateTargetedActors()
	{
		m_currentTargetedActors.Clear();
		ActorData actorData = m_actorData;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		AbilityData abilityData = actorData.GetAbilityData();
		foreach (AbilityRequestData current in GetAbilityRequestDataForClient())
		{
			Ability abilityOfActionType = abilityData.GetAbilityOfActionType(current.m_actionType);
			if (abilityOfActionType != null
				&& (actorData != activeOwnedActorData || !AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromSelf))
				&& (actorData == activeOwnedActorData || !AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromAllies))
				&& abilityOfActionType.Targeter != null)
			{
				for (int i = 0;
					i < abilityOfActionType.Targeters.Count
					&& i < abilityOfActionType.GetExpectedNumberOfTargeters()
					&& i < current.m_targets.Count;
					i++)
				{
					AbilityUtil_Targeter abilityUtil_Targeter = abilityOfActionType.Targeters[i];
					if (abilityUtil_Targeter != null)
					{
						foreach (AbilityUtil_Targeter.ActorTarget actorInRange in abilityUtil_Targeter.GetActorsInRange())
						{
							Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
							GetNameplateNumbersForTargeter(actorData, actorInRange.m_actor, abilityOfActionType, i, dictionary);
							foreach (KeyValuePair<AbilityTooltipSymbol, int> nameplateNumber in dictionary)
							{
								AbilityTooltipSymbol key = nameplateNumber.Key;
								if (!m_currentTargetedActors.ContainsKey(actorInRange.m_actor))
								{
									m_currentTargetedActors[actorInRange.m_actor] = new Dictionary<AbilityTooltipSymbol, int>();
								}
								if (!m_currentTargetedActors[actorInRange.m_actor].ContainsKey(key))
								{
									m_currentTargetedActors[actorInRange.m_actor][key] = 0;
								}
								m_currentTargetedActors[actorInRange.m_actor][key] += nameplateNumber.Value;
							}
						}
					}
				}
			}
		}
	}

	// reactor
	public static void GetNameplateNumbersForTargeter(ActorData caster, ActorData target, Ability abilityTargeting, int currentTargeterIndex, Dictionary<AbilityTooltipSymbol, int> symbolToValueMap)
	{
		bool flag = true;
		if (currentTargeterIndex >= abilityTargeting.Targeters.Count)
		{
			return;
		}
		bool hasTargetsInRange = false;
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			if (abilityTargeting.Targeters[i].IsActorInTargetRange(target, out bool inCover))
			{
				hasTargetsInRange = true;
				flag = (i == 0 || flag) && inCover;
			}
		}
		if (!hasTargetsInRange)
		{
			return;
		}
		HashSet<AbilityTooltipSubject> tooltipSubjectSet = new HashSet<AbilityTooltipSubject>();
		if (abilityTargeting.GetExpectedNumberOfTargeters() > 1)
		{
			for (int j = 0; j <= currentTargeterIndex; j++)
			{
				abilityTargeting.Targeters[j].AppendToTooltipSubjectSet(target, tooltipSubjectSet);
			}
		}
		else
		{
			abilityTargeting.Targeter.AppendToTooltipSubjectSet(target, tooltipSubjectSet);
		}
		List<AbilityTooltipNumber> nameplateTargetingNumbers = abilityTargeting.GetNameplateTargetingNumbers();
		bool isIgnoringCover = AbilityUtils.AbilityHasTag(abilityTargeting, AbilityTags.NameplateTargetNumberIgnoreCover) || abilityTargeting.ForceIgnoreCover(target);
		bool reducedCoverEffectiveness = AbilityUtils.AbilityReduceCoverEffectiveness(abilityTargeting, target);
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		s_targetingNumberUpdateScratch.ResetForCalc();
		bool customTargeterNumbers = abilityTargeting.GetCustomTargeterNumbers(target, currentTargeterIndex, s_targetingNumberUpdateScratch);
		if (customTargeterNumbers && abilityTargeting is GenericAbility_Container)
		{
			if (s_targetingNumberUpdateScratch.m_damage > 0)
			{
				int value = AbilityUtils.CalculateDamageForTargeter(caster, target, abilityTargeting, s_targetingNumberUpdateScratch.m_damage, flag && !isIgnoringCover);
				AddSymbolToValuePair(symbolToValueMap, AbilityTooltipSymbol.Damage, value);
			}
			if (s_targetingNumberUpdateScratch.m_healing > 0)
			{
				int value2 = AbilityUtils.CalculateHealingForTargeter(caster, target, abilityTargeting, s_targetingNumberUpdateScratch.m_healing);
				AddSymbolToValuePair(symbolToValueMap, AbilityTooltipSymbol.Healing, value2);
			}
			if (s_targetingNumberUpdateScratch.m_absorb > 0)
			{
				int value3 = AbilityUtils.CalculateAbsorbForTargeter(caster, target, abilityTargeting, s_targetingNumberUpdateScratch.m_absorb);
				AddSymbolToValuePair(symbolToValueMap, AbilityTooltipSymbol.Absorb, value3);
			}
			if (s_targetingNumberUpdateScratch.m_energy > 0 && caster != target)
			{
				int value4 = AbilityUtils.CalculateTechPointsForTargeter(target, abilityTargeting, s_targetingNumberUpdateScratch.m_energy);
				AddSymbolToValuePair(symbolToValueMap, AbilityTooltipSymbol.Energy, value4);
			}
			return;
		}
		if (!customTargeterNumbers)
		{
			dictionary = abilityTargeting.GetCustomNameplateItemTooltipValues(target, currentTargeterIndex);
		}
		bool hadDamage = false;
		bool hadHealing = false;
		bool hadEnergy = false;
		bool hadAbsorb = false;
		foreach (AbilityTooltipSubject tooltipSubject in tooltipSubjectSet)
		{
			foreach (AbilityTooltipNumber targetingNumber in nameplateTargetingNumbers)
			{
				if (targetingNumber.m_subject != tooltipSubject)
				{
					continue;
				}
				int value = targetingNumber.m_value;
				if (customTargeterNumbers && s_targetingNumberUpdateScratch.HasOverride(targetingNumber.m_symbol))
				{
					value = s_targetingNumberUpdateScratch.GetOverrideValue(targetingNumber.m_symbol);
				}
				else if (dictionary != null && dictionary.ContainsKey(targetingNumber.m_symbol))
				{
					value = dictionary[targetingNumber.m_symbol];
				}
				switch (targetingNumber.m_symbol)
				{
					case AbilityTooltipSymbol.Damage:
						if (hadDamage)
						{
							value = 0;
						}
						if (value > 0)
						{
							if (m_showStatusAdjustedTargetingNumbers)
							{
								value = AbilityUtils.CalculateDamageForTargeter(caster, target, abilityTargeting, value, flag && !isIgnoringCover);
							}
							else if (flag)
							{
								if (!isIgnoringCover)
								{
									value = AbilityUtils.ApplyCoverDamageReduction(target.GetActorStats(), value, reducedCoverEffectiveness);
								}
							}
							hadDamage = true;
						}
						break;
					case AbilityTooltipSymbol.Healing:
						if (hadHealing)
						{
							value = 0;
						}
						if (value > 0)
						{
							if (m_showStatusAdjustedTargetingNumbers)
							{
								value = AbilityUtils.CalculateHealingForTargeter(caster, target, abilityTargeting, value);
							}
							hadHealing = true;
						}
						break;
					case AbilityTooltipSymbol.Absorb:
						if (hadAbsorb)
						{
							value = 0;
						}
						if (value > 0)
						{
							if (m_showStatusAdjustedTargetingNumbers)
							{
								value = AbilityUtils.CalculateAbsorbForTargeter(caster, target, abilityTargeting, value);
							}
							hadAbsorb = true;
						}
						break;
					case AbilityTooltipSymbol.Energy:
						if (hadEnergy)
						{
							value = 0;
						}
						if (caster != target && value > 0)
						{
							if (m_showStatusAdjustedTargetingNumbers)
							{
								value = AbilityUtils.CalculateTechPointsForTargeter(target, abilityTargeting, value);
							}
							hadEnergy = true;
						}
						break;
				}
				if (value > 0)
				{
					if (!symbolToValueMap.ContainsKey(targetingNumber.m_symbol))
					{
						symbolToValueMap.Add(targetingNumber.m_symbol, value);
					}
					else
					{
						symbolToValueMap[targetingNumber.m_symbol] += value;
					}
				}
			}
		}
	}

	private static void AddSymbolToValuePair(Dictionary<AbilityTooltipSymbol, int> symbolToValue, AbilityTooltipSymbol symbol, int value)
	{
		if (value > 0)
		{
			if (!symbolToValue.ContainsKey(symbol))
			{
				symbolToValue.Add(symbol, value);
			}
			else
			{
				symbolToValue[symbol] += value;
			}
		}
	}

	internal bool IsTargetingActor(ActorData target, AbilityTooltipSymbol symbol, ref int targetedActorValue)
	{
		if (m_currentTargetedActors.ContainsKey(target) && m_currentTargetedActors[target].ContainsKey(symbol))
		{
			targetedActorValue += m_currentTargetedActors[target][symbol];
			return true;
		}
		return false;
	}

	private bool InSpectatorModeAndHideTargeting()
	{
		return ClientGameManager.Get().PlayerInfo != null
			&& ClientGameManager.Get().PlayerInfo.IsSpectator
			&& ClientGameManager.Get().SpectatorHideAbilityTargeter;
	}

	private bool ShouldDrawTargeters()
	{
		if (m_actorData == null)
		{
			m_actorData = GetComponent<ActorData>();
		}
		ActorData actorData = m_actorData;
		if (GameFlowData.Get() == null)
		{
			return false;
		}
		if (ClientGameManager.Get() == null)
		{
			return false;
		}
		if (GameFlowData.Get().LocalPlayerData == null)
		{
			return false;
		}
		if (ClientGameManager.Get().IsFastForward)  // removed in rogues
		{
			return false;
		}
		if (InSpectatorModeAndHideTargeting())
		{
			return false;
		}
		Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
		if (teamViewing != Team.Invalid && actorData.GetTeam() != teamViewing)
		{
			return false;
		}
		if (!GameplayUtils.IsPlayerControlled(this) && !GameplayData.Get().m_npcsShowTargeters)
		{
			return false;
		}
		if (!GameFlowData.Get().IsInDecisionState())
		{
			return false;
		}
		if (actorData.IsDead())
		{
			return false;
		}
		if (actorData.GetCurrentBoardSquare() == null)
		{
			return false;
		}
		// added in rogues
		//if (actorData.GetActorMovement().AmMoving())
		//{
		//	return false;
		//}
		return true;
	}

	private bool ShouldDrawAbilityCooldowns()
	{
		if (GameFlowData.Get() == null)
		{
			return false;
		}
		if (GameFlowData.Get().LocalPlayerData == null)
		{
			return false;
		}
		if (ClientGameManager.Get() == null)
		{
			return false;
		}
		// removed in rogues
		if (ClientGameManager.Get().IsFastForward)
		{
			return false;
		}
		if (SinglePlayerManager.Get() != null && !SinglePlayerManager.Get().EnableCooldownIndicators())
		{
			return false;
		}
		ActorData actorData = m_actorData;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (actorData == activeOwnedActorData && GameFlowData.Get().m_ownedActorDatas.Count <= 1)
		{
			return false;
		}
		if (!actorData.IsActorVisibleToClient())
		{
			return false;
		}
		if (!GameplayData.Get().m_showActorAbilityCooldowns)
		{
			return false;
		}
		if (!GameFlowData.Get().IsInDecisionState()
			&& !InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo))
		{
			return false;
		}
		if (actorData.IsDead())
		{
			return false;
		}
		if (actorData.IsActorInvisibleForRespawn())
		{
			return false;
		}
		if (actorData.GetCurrentBoardSquare() == null)
		{
			return false;
		}
		// added in rogues
		//if (actorData.GetActorMovement().AmMoving())
		//{
		//	return false;
		//}
		return true;
	}

	private void DrawTargeters()
	{
		ActorData actorData = m_actorData;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		AbilityData abilityData = actorData.GetAbilityData();
		List<AbilityRequestData> abilityRequestDataForClient = GetAbilityRequestDataForClient();
		foreach (AbilityRequestData item in abilityRequestDataForClient)
		{
			Ability abilityOfActionType = abilityData.GetAbilityOfActionType(item.m_actionType);
			if (abilityOfActionType != null
				&& item.m_targets.Count != 0
				&& (actorData != activeOwnedActorData || !AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromSelf))
				&& (actorData == activeOwnedActorData || !AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromAllies))
				&& abilityOfActionType.Targeter != null)
			{
				if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.UseTeleportUIEffect))
				{
					abilityOfActionType.Targeter.UpdateEffectOnCaster(item.m_targets[0], actorData);
					abilityOfActionType.Targeter.UpdateTargetAreaEffect(item.m_targets[0], actorData);
					abilityOfActionType.Targeter.UpdateTargeting(item.m_targets[0], actorData);
					abilityOfActionType.Targeter.StartConfirmedTargeting(item.m_targets[0], actorData);
					abilityOfActionType.Targeter.UpdateFadeOutHighlights(actorData);  // removed in rogues
				}
				else if (abilityOfActionType.GetExpectedNumberOfTargeters() < 2)
				{
					abilityOfActionType.Targeter.SetLastUpdateCursorState(item.m_targets[0]);
					abilityOfActionType.Targeter.UpdateTargeting(item.m_targets[0], actorData);
					abilityOfActionType.Targeter.StartConfirmedTargeting(item.m_targets[0], actorData);
					abilityOfActionType.Targeter.UpdateFadeOutHighlights(actorData);  // removed in rogues
				}
				else
				{
					int num = abilityOfActionType.GetExpectedNumberOfTargeters();
					if (num > abilityOfActionType.GetNumTargets())
					{
						num = abilityOfActionType.GetNumTargets();
					}
					if (num > abilityOfActionType.Targeters.Count)
					{
						num = abilityOfActionType.Targeters.Count;
					}
					if (num > item.m_targets.Count)
					{
						num = item.m_targets.Count;
					}
					for (int i = 0; i < num; i++)
					{
						if (abilityOfActionType.Targeters[i].IsUsingMultiTargetUpdate())
						{
							abilityOfActionType.Targeters[i].SetLastUpdateCursorState(item.m_targets[i]);
							abilityOfActionType.Targeters[i].UpdateTargetingMultiTargets(item.m_targets[i], actorData, i, item.m_targets);
						}
						else
						{
							abilityOfActionType.Targeters[i].SetLastUpdateCursorState(item.m_targets[i]);
							abilityOfActionType.Targeters[i].UpdateTargeting(item.m_targets[i], actorData);
						}
						abilityOfActionType.Targeters[i].StartConfirmedTargeting(item.m_targets[i], actorData);
						abilityOfActionType.Targeters[i].UpdateFadeOutHighlights(actorData);  // removed in rogues
					}
				}
				abilityOfActionType.Targeter.UpdateArrowsForUI();
				if (actorData == activeOwnedActorData && abilityOfActionType != null && abilityOfActionType.ShouldAutoQueueIfValid() && abilityRequestDataForClient.Count == 1 && actorData.GetAbilityData().GetLastSelectedAbility() == null)
				{
					actorData.GetAbilityData().SetLastSelectedAbility(abilityOfActionType);
				}
			}
		}
		m_targetersBeingDrawn = true;
	}

	// removed in rogues
	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (args != null && eventType == GameEventManager.EventType.ReconnectReplayStateChanged)
		{
			ClearAbilityCooldowns();
			MarkForForceRedraw();
		}
	}

	// removed in rogues
	private void OnDestroy()
	{
		if (NetworkClient.active)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReconnectReplayStateChanged);
		}
	}

	public void MarkForForceRedraw()
	{
		m_markedForForceRedraw = true;
	}

	private void UpdateDrawnTargeters()
	{
		ActorData actorData = m_actorData;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		ActorData occupantActor = Board.Get().PlayerFreeSquare?.OccupantActor;
		AbilityData abilityData = actorData.GetAbilityData();
		List<AbilityRequestData> abilityRequestDataForClient = GetAbilityRequestDataForClient();
		int num = 0;
		for (int i = 0; i < abilityRequestDataForClient.Count; i++)
		{
			AbilityRequestData abilityRequestData = abilityRequestDataForClient[i];
			Ability abilityOfActionType = abilityData.GetAbilityOfActionType(abilityRequestData.m_actionType);
			if (abilityOfActionType != null
				&& (actorData != activeOwnedActorData || !AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromSelf))
				&& (actorData == activeOwnedActorData || !AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromAllies)))
			{
				if (abilityOfActionType.Targeter != null)
				{
					for (int j = 0; j < abilityOfActionType.Targeters.Count; j++)
					{
						if (j >= abilityOfActionType.GetExpectedNumberOfTargeters() || j >= abilityRequestData.m_targets.Count)
						{
							break;
						}
						AbilityUtil_Targeter abilityUtil_Targeter = abilityOfActionType.Targeters[j];
						if (abilityUtil_Targeter != null)
						{
							if (actorData != occupantActor && !InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo))
							{
								actorData.ForceDisplayTargetHighlight = false;
							}
							else
							{
								actorData.ForceDisplayTargetHighlight = true;
								abilityUtil_Targeter.StartConfirmedTargeting(abilityRequestData.m_targets[j], actorData);
							}
							abilityUtil_Targeter.UpdateConfirmedTargeting(abilityRequestData.m_targets[j], actorData);
						}
					}
				}
				if (actorData != activeOwnedActorData || GameFlowData.Get().m_ownedActorDatas.Count > 1)
				{
					HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateTargetingAbilityIndicator(actorData, abilityOfActionType, abilityRequestData.m_actionType, num);
					num++;
				}
			}
		}
		if (actorData != activeOwnedActorData || GameFlowData.Get().m_ownedActorDatas.Count > 1)
		{
			UpdateAbilityCooldowns(num, false);
		}
		m_targetersBeingDrawn = true;
	}

	public void UpdateAbilityCooldowns(int targetingAbilityIndicatorIndex, bool showRequestedAbilities)
	{
		AbilityData abilityData = m_actorData.GetAbilityData();
		AbilityData.ActionType actionTargeting = abilityData.GetSelectedActionTypeForTargeting();
		bool isShowingTargeting = false;
		if (actionTargeting != AbilityData.ActionType.INVALID_ACTION
			&& !InSpectatorModeAndHideTargeting()
			&& GameFlowData.Get().LocalPlayerData.IsViewingTeam(m_actorData.GetTeam()))
		{
			isShowingTargeting = true;
		}
		List<AbilityRequestData> abilityRequestDataForClient = GetAbilityRequestDataForClient();
		ActorData y = Board.Get().PlayerFreeSquare?.OccupantActor;
		if (m_actorData != y && !InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo))
		{
			s_updatedAbilityCooldownsActors.Clear();
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetCatalystPipsVisible(m_actorData, false);
		}
		else if (Time.time != s_lastTimeAddedAbilityCooldownsActor)
		{
			if (!s_updatedAbilityCooldownsActors.Contains(m_actorData))
			{
				s_updatedAbilityCooldownsActors.Add(m_actorData);
				s_lastTimeAddedAbilityCooldownsActor = Time.time;
			}
			int actualMaxTechPoints = m_actorData.GetMaxTechPoints();
			foreach (Ability current in abilityData.GetAbilitiesAsList())
			{
				if (current != null)
				{
					AbilityData.ActionType abilityAction = abilityData.GetActionTypeOfAbility(current);
					int moddedCost = current.GetModdedCost();

					// reactor
					bool isBasicAbility = abilityAction == AbilityData.ActionType.ABILITY_0;
					bool isImpossibleUlt = abilityAction == AbilityData.ActionType.ABILITY_4 && moddedCost >= actualMaxTechPoints;
					if (isBasicAbility || isImpossibleUlt)
					{
						continue;
					}
					// rogues
					//            if (current.GetModdedCooldown() <= 0
					//	&& current.GetModdedMaxStocks() <= 0
					//	&& (moddedCost <= 0 || moddedCost >= actualMaxTechPoints))
					//{
					//	continue;
					//}


					if ((!isShowingTargeting || abilityAction != actionTargeting)
						&& (!GameFlowData.Get().LocalPlayerData.IsViewingTeam(m_actorData.GetTeam())
							|| showRequestedAbilities
							|| abilityRequestDataForClient.FirstOrDefault((AbilityRequestData a) => a.m_actionType == abilityAction) == null))
					{
						HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateTargetingAbilityIndicator(m_actorData, current, abilityAction, targetingAbilityIndicatorIndex);
						targetingAbilityIndicatorIndex++;
					}
				}
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetCatalystPipsVisible(m_actorData, true);
		}
		if (isShowingTargeting
			&& abilityRequestDataForClient.FirstOrDefault((AbilityRequestData a) => a.m_actionType == actionTargeting) == null
			&& actionTargeting != AbilityData.ActionType.CARD_0
			&& actionTargeting != AbilityData.ActionType.CARD_1
			&& actionTargeting != AbilityData.ActionType.CARD_2)
		{
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateTargetingAbilityIndicator(m_actorData, abilityData.GetAbilityOfActionType(actionTargeting), actionTargeting, targetingAbilityIndicatorIndex);
			targetingAbilityIndicatorIndex++;
		}
		HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.TurnOffTargetingAbilityIndicator(m_actorData, targetingAbilityIndicatorIndex);
	}

	public void ClearAbilityCooldowns()
	{
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.TurnOffTargetingAbilityIndicator(m_actorData, 0);
		}
	}

	public BoardSquare GetEvadeDestinationForTargeter()
	{
		List<AbilityRequestData> abilityRequestDataForClient = GetAbilityRequestDataForClient();
		if (abilityRequestDataForClient == null
			|| abilityRequestDataForClient.Count <= 0
			|| m_actorData == null)
		{
			return null;
		}
		AbilityData abilityData = m_actorData.GetAbilityData();
		BoardSquare boardSquare = null;
		for (int i = 0; i < abilityRequestDataForClient.Count; i++)
		{
			if (boardSquare != null)
			{
				break;
			}
			AbilityRequestData abilityRequestData = abilityRequestDataForClient[i];
			if (abilityRequestData != null)
			{
				Ability abilityOfActionType = abilityData.GetAbilityOfActionType(abilityRequestData.m_actionType);
				if (abilityOfActionType != null && abilityOfActionType.GetRunPriority() == AbilityPriority.Evasion)
				{
					boardSquare = abilityOfActionType.GetEvadeDestinationForTargeter(abilityRequestData.m_targets, m_actorData);
				}
			}
		}
		return boardSquare;
	}

	// removed in rogues
	private void UNetVersion()
	{
	}

	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	// removed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
