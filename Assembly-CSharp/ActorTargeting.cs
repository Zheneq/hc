using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ActorTargeting : NetworkBehaviour, IGameEventListener
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
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return 1;
					}
				}
			}
			AbilityRequestData abilityRequestData = obj as AbilityRequestData;
			if (abilityRequestData != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return m_actionType.CompareTo(abilityRequestData.m_actionType);
					}
				}
			}
			throw new ArgumentException("Object is not an AbilityRequestData");
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

	private void Start()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReconnectReplayStateChanged);
			return;
		}
	}

	private void ResetTargeters(bool clearInstantly)
	{
		if (m_actorData == null)
		{
			m_actorData = GetComponent<ActorData>();
		}
		ActorTurnSM actorTurnSM = m_actorData.GetActorTurnSM();
		AbilityData abilityData = m_actorData.GetAbilityData();
		object obj;
		if (GameFlowData.Get() != null)
		{
			obj = GameFlowData.Get().activeOwnedActorData;
		}
		else
		{
			obj = null;
		}
		ActorData y = (ActorData)obj;
		for (int i = 0; i < 14; i++)
		{
			AbilityData.ActionType type = (AbilityData.ActionType)i;
			Ability abilityOfActionType = abilityData.GetAbilityOfActionType(type);
			if (abilityOfActionType == null)
			{
				continue;
			}
			if (m_actorData == y)
			{
				if (actorTurnSM != null)
				{
					if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION && abilityOfActionType.IsAbilitySelected())
					{
						continue;
					}
				}
			}
			foreach (AbilityUtil_Targeter targeter in abilityOfActionType.Targeters)
			{
				targeter?.ResetTargeter(clearInstantly);
			}
		}
		while (true)
		{
			if (HUD_UI.Get() != null)
			{
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.TurnOffTargetingAbilityIndicator(m_actorData, 0);
			}
			m_targetersBeingDrawn = false;
			return;
		}
	}

	public void OnRequestDataDeserialized()
	{
		ResetTargeters(false);
		if (!ShouldDrawTargeters())
		{
			return;
		}
		while (true)
		{
			DrawTargeters();
			CalculateTargetedActors();
			return;
		}
	}

	public List<AbilityRequestData> GetAbilityRequestDataForClient()
	{
		if (m_actorData != null)
		{
			if (m_actorData.TeamSensitiveData_authority != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return m_actorData.TeamSensitiveData_authority.GetAbilityRequestData();
					}
				}
			}
		}
		return s_emptyRequestDataList;
	}

	private void Update()
	{
		bool flag = ShouldDrawTargeters();
		if (m_markedForForceRedraw)
		{
			ResetTargeters(false);
			m_targetersBeingDrawn = false;
			m_markedForForceRedraw = false;
		}
		if (flag)
		{
			if (!m_targetersBeingDrawn)
			{
				DrawTargeters();
				CalculateTargetedActors();
				return;
			}
		}
		if (flag)
		{
			return;
		}
		while (true)
		{
			if (m_targetersBeingDrawn)
			{
				while (true)
				{
					ResetTargeters(false);
					return;
				}
			}
			return;
		}
	}

	private void LateUpdate()
	{
		if (ShouldDrawTargeters())
		{
			if (m_targetersBeingDrawn)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						UpdateDrawnTargeters();
						return;
					}
				}
			}
		}
		if (ShouldDrawAbilityCooldowns())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					UpdateAbilityCooldowns(0, true);
					return;
				}
			}
		}
		ClearAbilityCooldowns();
	}

	public List<AbilityTarget> GetAbilityTargetsInRequest(AbilityData.ActionType actionType)
	{
		List<AbilityRequestData> abilityRequestDataForClient = GetAbilityRequestDataForClient();
		if (abilityRequestDataForClient != null)
		{
			for (int i = 0; i < abilityRequestDataForClient.Count; i++)
			{
				AbilityRequestData abilityRequestData = abilityRequestDataForClient[i];
				if (abilityRequestData == null)
				{
					continue;
				}
				if (abilityRequestData.m_actionType != actionType)
				{
					continue;
				}
				while (true)
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
		List<AbilityRequestData> abilityRequestDataForClient = GetAbilityRequestDataForClient();
		using (List<AbilityRequestData>.Enumerator enumerator = abilityRequestDataForClient.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityRequestData current = enumerator.Current;
				Ability abilityOfActionType = abilityData.GetAbilityOfActionType(current.m_actionType);
				if (!(abilityOfActionType == null))
				{
					if (actorData == activeOwnedActorData)
					{
						if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromSelf))
						{
							continue;
						}
					}
					if (actorData != activeOwnedActorData)
					{
						if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromAllies))
						{
							continue;
						}
					}
					if (abilityOfActionType.Targeter != null)
					{
						for (int i = 0; i < abilityOfActionType.Targeters.Count && i < abilityOfActionType.GetExpectedNumberOfTargeters(); i++)
						{
							if (i >= current.m_targets.Count)
							{
								break;
							}
							AbilityUtil_Targeter abilityUtil_Targeter = abilityOfActionType.Targeters[i];
							if (abilityUtil_Targeter != null)
							{
								List<AbilityUtil_Targeter.ActorTarget> actorsInRange = abilityUtil_Targeter.GetActorsInRange();
								using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator2 = actorsInRange.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										AbilityUtil_Targeter.ActorTarget current2 = enumerator2.Current;
										Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
										GetNameplateNumbersForTargeter(actorData, current2.m_actor, abilityOfActionType, i, dictionary);
										using (Dictionary<AbilityTooltipSymbol, int>.Enumerator enumerator3 = dictionary.GetEnumerator())
										{
											while (enumerator3.MoveNext())
											{
												KeyValuePair<AbilityTooltipSymbol, int> current3 = enumerator3.Current;
												AbilityTooltipSymbol key = current3.Key;
												if (!m_currentTargetedActors.ContainsKey(current2.m_actor))
												{
													m_currentTargetedActors[current2.m_actor] = new Dictionary<AbilityTooltipSymbol, int>();
												}
												if (!m_currentTargetedActors[current2.m_actor].ContainsKey(key))
												{
													m_currentTargetedActors[current2.m_actor][key] = 0;
												}
												m_currentTargetedActors[current2.m_actor][key] += current3.Value;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

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
			if (!abilityTargeting.Targeters[i].IsActorInTargetRange(target, out bool inCover))
			{
				continue;
			}
			hasTargetsInRange = true;
			flag = (i == 0 || flag) && inCover;
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
		bool result = false;
		if (m_currentTargetedActors.ContainsKey(target))
		{
			if (m_currentTargetedActors[target].ContainsKey(symbol))
			{
				targetedActorValue += m_currentTargetedActors[target][symbol];
				result = true;
			}
		}
		return result;
	}

	private bool InSpectatorModeAndHideTargeting()
	{
		int result;
		if (ClientGameManager.Get().PlayerInfo != null && ClientGameManager.Get().PlayerInfo.IsSpectator)
		{
			result = (ClientGameManager.Get().SpectatorHideAbilityTargeter ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private bool ShouldDrawTargeters()
	{
		if (m_actorData == null)
		{
			m_actorData = GetComponent<ActorData>();
		}
		ActorData actorData = m_actorData;
		if (!(GameFlowData.Get() == null))
		{
			if (!(ClientGameManager.Get() == null))
			{
				if (GameFlowData.Get().LocalPlayerData == null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (ClientGameManager.Get().IsFastForward)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (InSpectatorModeAndHideTargeting())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				if (teamViewing != Team.Invalid && actorData.GetTeam() != teamViewing)
				{
					return false;
				}
				if (!GameplayUtils.IsPlayerControlled(this))
				{
					if (!GameplayData.Get().m_npcsShowTargeters)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return false;
							}
						}
					}
				}
				if (!GameFlowData.Get().IsInDecisionState())
				{
					return false;
				}
				if (actorData.IsDead())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (actorData.GetCurrentBoardSquare() == null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				return true;
			}
		}
		return false;
	}

	private bool ShouldDrawAbilityCooldowns()
	{
		if (GameFlowData.Get() == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (GameFlowData.Get().LocalPlayerData == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (!(ClientGameManager.Get() == null))
		{
			if (!ClientGameManager.Get().IsFastForward)
			{
				if (SinglePlayerManager.Get() != null)
				{
					if (!SinglePlayerManager.Get().EnableCooldownIndicators())
					{
						return false;
					}
				}
				ActorData actorData = m_actorData;
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (actorData == activeOwnedActorData)
				{
					if (GameFlowData.Get().m_ownedActorDatas.Count <= 1)
					{
						return false;
					}
				}
				if (!actorData.IsVisibleToClient())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (!GameplayData.Get().m_showActorAbilityCooldowns)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (!GameFlowData.Get().IsInDecisionState() && !InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (actorData.IsDead())
				{
					return false;
				}
				if (actorData.ShouldPickRespawn_zq())
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (actorData.GetCurrentBoardSquare() == null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				return true;
			}
		}
		return false;
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
			if (!(abilityOfActionType == null))
			{
				if (item.m_targets.Count == 0)
				{
				}
				else if (!(actorData == activeOwnedActorData) || !AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromSelf))
				{
					if (actorData != activeOwnedActorData && AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromAllies))
					{
					}
					else if (abilityOfActionType.Targeter != null)
					{
						if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.UseTeleportUIEffect))
						{
							abilityOfActionType.Targeter.UpdateEffectOnCaster(item.m_targets[0], actorData);
							abilityOfActionType.Targeter.UpdateTargetAreaEffect(item.m_targets[0], actorData);
							abilityOfActionType.Targeter.UpdateTargeting(item.m_targets[0], actorData);
							abilityOfActionType.Targeter.StartConfirmedTargeting(item.m_targets[0], actorData);
							abilityOfActionType.Targeter.UpdateFadeOutHighlights(actorData);
						}
						else if (abilityOfActionType.GetExpectedNumberOfTargeters() < 2)
						{
							abilityOfActionType.Targeter.SetLastUpdateCursorState(item.m_targets[0]);
							abilityOfActionType.Targeter.UpdateTargeting(item.m_targets[0], actorData);
							abilityOfActionType.Targeter.StartConfirmedTargeting(item.m_targets[0], actorData);
							abilityOfActionType.Targeter.UpdateFadeOutHighlights(actorData);
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
								abilityOfActionType.Targeters[i].UpdateFadeOutHighlights(actorData);
							}
						}
						abilityOfActionType.Targeter.UpdateArrowsForUI();
						if (actorData == activeOwnedActorData && abilityOfActionType != null && abilityOfActionType.ShouldAutoQueueIfValid())
						{
							if (abilityRequestDataForClient.Count == 1 && actorData.GetAbilityData().GetLastSelectedAbility() == null)
							{
								actorData.GetAbilityData().SetLastSelectedAbility(abilityOfActionType);
							}
						}
					}
				}
			}
		}
		m_targetersBeingDrawn = true;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (args == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (eventType != GameEventManager.EventType.ReconnectReplayStateChanged)
		{
			return;
		}
		while (true)
		{
			ClearAbilityCooldowns();
			MarkForForceRedraw();
			return;
		}
	}

	private void OnDestroy()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReconnectReplayStateChanged);
			return;
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
		ActorData y = (!(Board.Get().PlayerFreeSquare != null)) ? null : Board.Get().PlayerFreeSquare.OccupantActor;
		AbilityData abilityData = actorData.GetAbilityData();
		List<AbilityRequestData> abilityRequestDataForClient = GetAbilityRequestDataForClient();
		int num = 0;
		for (int i = 0; i < abilityRequestDataForClient.Count; i++)
		{
			AbilityRequestData abilityRequestData = abilityRequestDataForClient[i];
			Ability abilityOfActionType = abilityData.GetAbilityOfActionType(abilityRequestData.m_actionType);
			if (abilityOfActionType == null)
			{
				continue;
			}
			if (actorData == activeOwnedActorData)
			{
				if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromSelf))
				{
					continue;
				}
			}
			if (actorData != activeOwnedActorData)
			{
				if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromAllies))
				{
					continue;
				}
			}
			if (abilityOfActionType.Targeter != null)
			{
				for (int j = 0; j < abilityOfActionType.Targeters.Count; j++)
				{
					if (j >= abilityOfActionType.GetExpectedNumberOfTargeters())
					{
						break;
					}
					AbilityUtil_Targeter abilityUtil_Targeter;
					if (j < abilityRequestData.m_targets.Count)
					{
						abilityUtil_Targeter = abilityOfActionType.Targeters[j];
						if (abilityUtil_Targeter == null)
						{
							continue;
						}
						if (!(actorData == y))
						{
							if (!InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo))
							{
								actorData.ForceDisplayTargetHighlight = false;
								goto IL_019f;
							}
						}
						actorData.ForceDisplayTargetHighlight = true;
						abilityUtil_Targeter.StartConfirmedTargeting(abilityRequestData.m_targets[j], actorData);
						goto IL_019f;
					}
					break;
					IL_019f:
					abilityUtil_Targeter.UpdateConfirmedTargeting(abilityRequestData.m_targets[j], actorData);
				}
			}
			if (!(actorData != activeOwnedActorData))
			{
				if (GameFlowData.Get().m_ownedActorDatas.Count <= 1)
				{
					continue;
				}
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateTargetingAbilityIndicator(actorData, abilityOfActionType, abilityRequestData.m_actionType, num);
			num++;
		}
		if (!(actorData != activeOwnedActorData))
		{
			if (GameFlowData.Get().m_ownedActorDatas.Count <= 1)
			{
				goto IL_02bc;
			}
		}
		UpdateAbilityCooldowns(num, false);
		goto IL_02bc;
		IL_02bc:
		m_targetersBeingDrawn = true;
	}

	public void UpdateAbilityCooldowns(int targetingAbilityIndicatorIndex, bool showRequestedAbilities)
	{
		AbilityData abilityData = m_actorData.GetAbilityData();
		AbilityData.ActionType actionTargeting = abilityData.GetSelectedActionTypeForTargeting();
		bool flag = InSpectatorModeAndHideTargeting();
		bool flag2 = false;
		if (actionTargeting != AbilityData.ActionType.INVALID_ACTION)
		{
			if (!flag)
			{
				if (GameFlowData.Get().LocalPlayerData.IsViewingTeam(m_actorData.GetTeam()))
				{
					flag2 = true;
				}
			}
		}
		List<AbilityRequestData> abilityRequestDataForClient = GetAbilityRequestDataForClient();
		object obj;
		if (Board.Get().PlayerFreeSquare != null)
		{
			obj = Board.Get().PlayerFreeSquare.OccupantActor;
		}
		else
		{
			obj = null;
		}
		ActorData y = (ActorData)obj;
		if (!(m_actorData == y))
		{
			if (!InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo))
			{
				s_updatedAbilityCooldownsActors.Clear();
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetCatalystPipsVisible(m_actorData, false);
				goto IL_0310;
			}
		}
		if (Time.time != s_lastTimeAddedAbilityCooldownsActor)
		{
			if (!s_updatedAbilityCooldownsActors.Contains(m_actorData))
			{
				s_updatedAbilityCooldownsActors.Add(m_actorData);
				s_lastTimeAddedAbilityCooldownsActor = Time.time;
			}
			int actualMaxTechPoints = m_actorData.GetActualMaxTechPoints();
			List<Ability> abilitiesAsList = abilityData.GetAbilitiesAsList();
			using (List<Ability>.Enumerator enumerator = abilitiesAsList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ability current = enumerator.Current;
					if (current == null)
					{
					}
					else
					{
						AbilityData.ActionType abilityAction = abilityData.GetActionTypeOfAbility(current);
						int moddedCost = current.GetModdedCost();
						bool flag3 = abilityAction == AbilityData.ActionType.ABILITY_0;
						int num;
						if (abilityAction == AbilityData.ActionType.ABILITY_4)
						{
							num = ((moddedCost >= actualMaxTechPoints) ? 1 : 0);
						}
						else
						{
							num = 0;
						}
						bool flag4 = (byte)num != 0;
						if (!flag3)
						{
							if (!flag4)
							{
								if (flag2)
								{
									if (abilityAction == actionTargeting)
									{
										continue;
									}
								}
								if (GameFlowData.Get().LocalPlayerData.IsViewingTeam(m_actorData.GetTeam()))
								{
									if (!showRequestedAbilities)
									{
										if (abilityRequestDataForClient.FirstOrDefault((AbilityRequestData a) => a.m_actionType == abilityAction) != null)
										{
											continue;
										}
									}
								}
								HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateTargetingAbilityIndicator(m_actorData, current, abilityAction, targetingAbilityIndicatorIndex);
								targetingAbilityIndicatorIndex++;
							}
						}
					}
				}
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetCatalystPipsVisible(m_actorData, true);
		}
		goto IL_0310;
		IL_0310:
		if (flag2)
		{
			if (abilityRequestDataForClient.FirstOrDefault((AbilityRequestData a) => a.m_actionType == actionTargeting) == null)
			{
				if (actionTargeting != AbilityData.ActionType.CARD_0)
				{
					if (actionTargeting != AbilityData.ActionType.CARD_1)
					{
						if (actionTargeting != AbilityData.ActionType.CARD_2)
						{
							HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateTargetingAbilityIndicator(m_actorData, abilityData.GetAbilityOfActionType(actionTargeting), actionTargeting, targetingAbilityIndicatorIndex);
							targetingAbilityIndicatorIndex++;
						}
					}
				}
			}
		}
		HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.TurnOffTargetingAbilityIndicator(m_actorData, targetingAbilityIndicatorIndex);
	}

	public void ClearAbilityCooldowns()
	{
		if (!(HUD_UI.Get() != null))
		{
			return;
		}
		while (true)
		{
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.TurnOffTargetingAbilityIndicator(m_actorData, 0);
			return;
		}
	}

	public BoardSquare GetEvadeDestinationForTargeter()
	{
		BoardSquare boardSquare = null;
		List<AbilityRequestData> abilityRequestDataForClient = GetAbilityRequestDataForClient();
		if (abilityRequestDataForClient != null)
		{
			if (abilityRequestDataForClient.Count > 0)
			{
				if (m_actorData != null)
				{
					AbilityData abilityData = m_actorData.GetAbilityData();
					for (int i = 0; i < abilityRequestDataForClient.Count; i++)
					{
						if (boardSquare == null)
						{
							AbilityRequestData abilityRequestData = abilityRequestDataForClient[i];
							if (abilityRequestData == null)
							{
								continue;
							}
							Ability abilityOfActionType = abilityData.GetAbilityOfActionType(abilityRequestData.m_actionType);
							if (abilityOfActionType != null && abilityOfActionType.GetRunPriority() == AbilityPriority.Evasion)
							{
								boardSquare = abilityOfActionType.GetEvadeDestinationForTargeter(abilityRequestData.m_targets, m_actorData);
							}
							continue;
						}
						break;
					}
				}
			}
		}
		return boardSquare;
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
