using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ActorTargeting : NetworkBehaviour, IGameEventListener
{
	private static List<ActorTargeting.AbilityRequestData> s_emptyRequestDataList = new List<ActorTargeting.AbilityRequestData>();

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
		this.m_actorData = base.GetComponent<ActorData>();
		this.m_currentTargetedActors = new Dictionary<ActorData, Dictionary<AbilityTooltipSymbol, int>>();
	}

	private void Start()
	{
		if (NetworkClient.active)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReconnectReplayStateChanged);
		}
	}

	private void ResetTargeters(bool clearInstantly)
	{
		if (this.m_actorData == null)
		{
			this.m_actorData = base.GetComponent<ActorData>();
		}
		ActorTurnSM actorTurnSM = this.m_actorData.GetActorTurnSM();
		AbilityData abilityData = this.m_actorData.GetAbilityData();
		ActorData actorData;
		if (GameFlowData.Get() != null)
		{
			actorData = GameFlowData.Get().activeOwnedActorData;
		}
		else
		{
			actorData = null;
		}
		ActorData y = actorData;
		for (int i = 0; i < 0xE; i++)
		{
			AbilityData.ActionType type = (AbilityData.ActionType)i;
			Ability abilityOfActionType = abilityData.GetAbilityOfActionType(type);
			if (abilityOfActionType == null)
			{
			}
			else
			{
				if (this.m_actorData == y)
				{
					if (actorTurnSM != null)
					{
						if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION && abilityOfActionType.IsAbilitySelected())
						{
							goto IL_13C;
						}
					}
				}
				foreach (AbilityUtil_Targeter abilityUtil_Targeter in abilityOfActionType.Targeters)
				{
					if (abilityUtil_Targeter != null)
					{
						abilityUtil_Targeter.ResetTargeter(clearInstantly);
					}
				}
			}
			IL_13C:;
		}
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.TurnOffTargetingAbilityIndicator(this.m_actorData, 0);
		}
		this.m_targetersBeingDrawn = false;
	}

	public void OnRequestDataDeserialized()
	{
		this.ResetTargeters(false);
		if (this.ShouldDrawTargeters())
		{
			this.DrawTargeters();
			this.CalculateTargetedActors();
		}
	}

	public List<ActorTargeting.AbilityRequestData> GetAbilityRequestDataForClient()
	{
		if (this.m_actorData != null)
		{
			if (this.m_actorData.TeamSensitiveData_authority != null)
			{
				return this.m_actorData.TeamSensitiveData_authority.GetAbilityRequestData();
			}
		}
		return ActorTargeting.s_emptyRequestDataList;
	}

	private void Update()
	{
		bool flag = this.ShouldDrawTargeters();
		if (this.m_markedForForceRedraw)
		{
			this.ResetTargeters(false);
			this.m_targetersBeingDrawn = false;
			this.m_markedForForceRedraw = false;
		}
		if (flag)
		{
			if (!this.m_targetersBeingDrawn)
			{
				this.DrawTargeters();
				this.CalculateTargetedActors();
				return;
			}
		}
		if (!flag)
		{
			if (this.m_targetersBeingDrawn)
			{
				this.ResetTargeters(false);
			}
		}
	}

	private void LateUpdate()
	{
		bool flag = this.ShouldDrawTargeters();
		if (flag)
		{
			if (this.m_targetersBeingDrawn)
			{
				this.UpdateDrawnTargeters();
				return;
			}
		}
		if (this.ShouldDrawAbilityCooldowns())
		{
			this.UpdateAbilityCooldowns(0, true);
		}
		else
		{
			this.ClearAbilityCooldowns();
		}
	}

	public List<AbilityTarget> GetAbilityTargetsInRequest(AbilityData.ActionType actionType)
	{
		List<ActorTargeting.AbilityRequestData> abilityRequestDataForClient = this.GetAbilityRequestDataForClient();
		if (abilityRequestDataForClient != null)
		{
			for (int i = 0; i < abilityRequestDataForClient.Count; i++)
			{
				ActorTargeting.AbilityRequestData abilityRequestData = abilityRequestDataForClient[i];
				if (abilityRequestData != null)
				{
					if (abilityRequestData.m_actionType == actionType)
					{
						return abilityRequestData.m_targets;
					}
				}
			}
		}
		return null;
	}

	private void CalculateTargetedActors()
	{
		this.m_currentTargetedActors.Clear();
		ActorData actorData = this.m_actorData;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		AbilityData abilityData = actorData.GetAbilityData();
		List<ActorTargeting.AbilityRequestData> abilityRequestDataForClient = this.GetAbilityRequestDataForClient();
		using (List<ActorTargeting.AbilityRequestData>.Enumerator enumerator = abilityRequestDataForClient.GetEnumerator())
		{
			IL_2A4:
			while (enumerator.MoveNext())
			{
				ActorTargeting.AbilityRequestData abilityRequestData = enumerator.Current;
				Ability abilityOfActionType = abilityData.GetAbilityOfActionType(abilityRequestData.m_actionType);
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
						int num = 0;
						while (num < abilityOfActionType.Targeters.Count && num < abilityOfActionType.GetExpectedNumberOfTargeters())
						{
							if (num >= abilityRequestData.m_targets.Count)
							{
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									goto IL_2A4;
								}
							}
							else
							{
								AbilityUtil_Targeter abilityUtil_Targeter = abilityOfActionType.Targeters[num];
								if (abilityUtil_Targeter != null)
								{
									List<AbilityUtil_Targeter.ActorTarget> actorsInRange = abilityUtil_Targeter.GetActorsInRange();
									using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator2 = actorsInRange.GetEnumerator())
									{
										while (enumerator2.MoveNext())
										{
											AbilityUtil_Targeter.ActorTarget actorTarget = enumerator2.Current;
											Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
											ActorTargeting.GetNameplateNumbersForTargeter(actorData, actorTarget.m_actor, abilityOfActionType, num, dictionary);
											using (Dictionary<AbilityTooltipSymbol, int>.Enumerator enumerator3 = dictionary.GetEnumerator())
											{
												while (enumerator3.MoveNext())
												{
													KeyValuePair<AbilityTooltipSymbol, int> keyValuePair = enumerator3.Current;
													AbilityTooltipSymbol key = keyValuePair.Key;
													if (!this.m_currentTargetedActors.ContainsKey(actorTarget.m_actor))
													{
														this.m_currentTargetedActors[actorTarget.m_actor] = new Dictionary<AbilityTooltipSymbol, int>();
													}
													if (!this.m_currentTargetedActors[actorTarget.m_actor].ContainsKey(key))
													{
														this.m_currentTargetedActors[actorTarget.m_actor][key] = 0;
													}
													Dictionary<AbilityTooltipSymbol, int> dictionary2;
													AbilityTooltipSymbol key2;
													(dictionary2 = this.m_currentTargetedActors[actorTarget.m_actor])[key2 = key] = dictionary2[key2] + keyValuePair.Value;
												}
											}
										}
									}
								}
								num++;
							}
						}
					}
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
		bool flag2 = false;
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			bool flag4;
			bool flag3 = abilityTargeting.Targeters[i].IsActorInTargetRange(target, out flag4);
			if (flag3)
			{
				flag2 = true;
				if (i == 0)
				{
					flag = flag4;
				}
				else
				{
					bool flag5;
					if (flag)
					{
						flag5 = flag4;
					}
					else
					{
						flag5 = false;
					}
					flag = flag5;
				}
			}
		}
		if (!flag2)
		{
			return;
		}
		HashSet<AbilityTooltipSubject> hashSet = new HashSet<AbilityTooltipSubject>();
		if (abilityTargeting.GetExpectedNumberOfTargeters() > 1)
		{
			for (int j = 0; j <= currentTargeterIndex; j++)
			{
				abilityTargeting.Targeters[j].AppendToTooltipSubjectSet(target, hashSet);
			}
		}
		else
		{
			abilityTargeting.Targeter.AppendToTooltipSubjectSet(target, hashSet);
		}
		List<AbilityTooltipNumber> nameplateTargetingNumbers = abilityTargeting.GetNameplateTargetingNumbers();
		bool flag6;
		if (!AbilityUtils.AbilityHasTag(abilityTargeting, AbilityTags.NameplateTargetNumberIgnoreCover))
		{
			flag6 = abilityTargeting.ForceIgnoreCover(target);
		}
		else
		{
			flag6 = true;
		}
		bool flag7 = flag6;
		bool reducedCoverEffectiveness = AbilityUtils.AbilityReduceCoverEffectiveness(abilityTargeting, target);
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		ActorTargeting.s_targetingNumberUpdateScratch.ResetForCalc();
		bool customTargeterNumbers = abilityTargeting.GetCustomTargeterNumbers(target, currentTargeterIndex, ActorTargeting.s_targetingNumberUpdateScratch);
		if (customTargeterNumbers)
		{
			if (abilityTargeting is GenericAbility_Container)
			{
				if (ActorTargeting.s_targetingNumberUpdateScratch.m_damage > 0)
				{
					int value = AbilityUtils.CalculateDamageForTargeter(caster, target, abilityTargeting, ActorTargeting.s_targetingNumberUpdateScratch.m_damage, flag && !flag7);
					ActorTargeting.AddSymbolToValuePair(symbolToValueMap, AbilityTooltipSymbol.Damage, value);
				}
				if (ActorTargeting.s_targetingNumberUpdateScratch.m_healing > 0)
				{
					int value2 = AbilityUtils.CalculateHealingForTargeter(caster, target, abilityTargeting, ActorTargeting.s_targetingNumberUpdateScratch.m_healing);
					ActorTargeting.AddSymbolToValuePair(symbolToValueMap, AbilityTooltipSymbol.Healing, value2);
				}
				if (ActorTargeting.s_targetingNumberUpdateScratch.m_absorb > 0)
				{
					int value3 = AbilityUtils.CalculateAbsorbForTargeter(caster, target, abilityTargeting, ActorTargeting.s_targetingNumberUpdateScratch.m_absorb);
					ActorTargeting.AddSymbolToValuePair(symbolToValueMap, AbilityTooltipSymbol.Absorb, value3);
				}
				if (ActorTargeting.s_targetingNumberUpdateScratch.m_energy > 0)
				{
					if (caster != target)
					{
						int value4 = AbilityUtils.CalculateTechPointsForTargeter(target, abilityTargeting, ActorTargeting.s_targetingNumberUpdateScratch.m_energy);
						ActorTargeting.AddSymbolToValuePair(symbolToValueMap, AbilityTooltipSymbol.Energy, value4);
					}
				}
				return;
			}
		}
		if (!customTargeterNumbers)
		{
			dictionary = abilityTargeting.GetCustomNameplateItemTooltipValues(target, currentTargeterIndex);
		}
		bool flag8 = false;
		bool flag9 = false;
		bool flag10 = false;
		bool flag11 = false;
		using (HashSet<AbilityTooltipSubject>.Enumerator enumerator = hashSet.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityTooltipSubject abilityTooltipSubject = enumerator.Current;
				using (List<AbilityTooltipNumber>.Enumerator enumerator2 = nameplateTargetingNumbers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						AbilityTooltipNumber abilityTooltipNumber = enumerator2.Current;
						if (abilityTooltipNumber.m_subject == abilityTooltipSubject)
						{
							int num = abilityTooltipNumber.m_value;
							if (!customTargeterNumbers)
							{
								goto IL_326;
							}
							if (!ActorTargeting.s_targetingNumberUpdateScratch.HasOverride(abilityTooltipNumber.m_symbol))
							{
								goto IL_326;
							}
							num = ActorTargeting.s_targetingNumberUpdateScratch.GetOverrideValue(abilityTooltipNumber.m_symbol);
							IL_358:
							switch (abilityTooltipNumber.m_symbol)
							{
							case AbilityTooltipSymbol.Damage:
								if (flag8)
								{
									num = 0;
								}
								if (num > 0)
								{
									if (ActorTargeting.m_showStatusAdjustedTargetingNumbers)
									{
										num = AbilityUtils.CalculateDamageForTargeter(caster, target, abilityTargeting, num, flag && !flag7);
									}
									else if (flag)
									{
										if (!flag7)
										{
											num = AbilityUtils.ApplyCoverDamageReduction(target.GetActorStats(), num, reducedCoverEffectiveness);
										}
									}
									flag8 = true;
								}
								break;
							case AbilityTooltipSymbol.Healing:
								if (flag9)
								{
									num = 0;
								}
								if (num > 0)
								{
									if (ActorTargeting.m_showStatusAdjustedTargetingNumbers)
									{
										num = AbilityUtils.CalculateHealingForTargeter(caster, target, abilityTargeting, num);
									}
									flag9 = true;
								}
								break;
							case AbilityTooltipSymbol.Absorb:
								if (flag11)
								{
									num = 0;
								}
								if (num > 0)
								{
									if (ActorTargeting.m_showStatusAdjustedTargetingNumbers)
									{
										num = AbilityUtils.CalculateAbsorbForTargeter(caster, target, abilityTargeting, num);
									}
									flag11 = true;
								}
								break;
							case AbilityTooltipSymbol.Energy:
								if (flag10)
								{
									num = 0;
								}
								if (caster != target)
								{
									if (num > 0)
									{
										if (ActorTargeting.m_showStatusAdjustedTargetingNumbers)
										{
											num = AbilityUtils.CalculateTechPointsForTargeter(target, abilityTargeting, num);
										}
										flag10 = true;
									}
								}
								break;
							}
							if (num <= 0)
							{
								continue;
							}
							if (!symbolToValueMap.ContainsKey(abilityTooltipNumber.m_symbol))
							{
								symbolToValueMap.Add(abilityTooltipNumber.m_symbol, num);
								continue;
							}
							AbilityTooltipSymbol symbol;
							symbolToValueMap[symbol = abilityTooltipNumber.m_symbol] = symbolToValueMap[symbol] + num;
							continue;
							IL_326:
							if (dictionary != null && dictionary.ContainsKey(abilityTooltipNumber.m_symbol))
							{
								num = dictionary[abilityTooltipNumber.m_symbol];
								goto IL_358;
							}
							goto IL_358;
						}
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

	internal unsafe bool IsTargetingActor(ActorData target, AbilityTooltipSymbol symbol, ref int targetedActorValue)
	{
		bool result = false;
		if (this.m_currentTargetedActors.ContainsKey(target))
		{
			if (this.m_currentTargetedActors[target].ContainsKey(symbol))
			{
				targetedActorValue += this.m_currentTargetedActors[target][symbol];
				result = true;
			}
		}
		return result;
	}

	private bool InSpectatorModeAndHideTargeting()
	{
		bool result;
		if (ClientGameManager.Get().PlayerInfo != null && ClientGameManager.Get().PlayerInfo.IsSpectator)
		{
			result = ClientGameManager.Get().SpectatorHideAbilityTargeter;
		}
		else
		{
			result = false;
		}
		return result;
	}

	private bool ShouldDrawTargeters()
	{
		if (this.m_actorData == null)
		{
			this.m_actorData = base.GetComponent<ActorData>();
		}
		ActorData actorData = this.m_actorData;
		if (!(GameFlowData.Get() == null))
		{
			if (ClientGameManager.Get() == null)
			{
			}
			else
			{
				if (GameFlowData.Get().LocalPlayerData == null)
				{
					return false;
				}
				if (ClientGameManager.Get().IsFastForward)
				{
					return false;
				}
				if (this.InSpectatorModeAndHideTargeting())
				{
					return false;
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
						return false;
					}
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
				return true;
			}
		}
		return false;
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
		if (!(ClientGameManager.Get() == null))
		{
			if (ClientGameManager.Get().IsFastForward)
			{
			}
			else
			{
				if (SinglePlayerManager.Get() != null)
				{
					if (!SinglePlayerManager.Get().EnableCooldownIndicators())
					{
						return false;
					}
				}
				ActorData actorData = this.m_actorData;
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
					return false;
				}
				if (!GameplayData.Get().m_showActorAbilityCooldowns)
				{
					return false;
				}
				if (!GameFlowData.Get().IsInDecisionState() && !InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo))
				{
					return false;
				}
				if (actorData.IsDead())
				{
					return false;
				}
				if (actorData.ShouldPickRespawn_zq())
				{
					return false;
				}
				if (actorData.GetCurrentBoardSquare() == null)
				{
					return false;
				}
				return true;
			}
		}
		return false;
	}

	private void DrawTargeters()
	{
		ActorData actorData = this.m_actorData;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		AbilityData abilityData = actorData.GetAbilityData();
		List<ActorTargeting.AbilityRequestData> abilityRequestDataForClient = this.GetAbilityRequestDataForClient();
		foreach (ActorTargeting.AbilityRequestData abilityRequestData in abilityRequestDataForClient)
		{
			Ability abilityOfActionType = abilityData.GetAbilityOfActionType(abilityRequestData.m_actionType);
			if (!(abilityOfActionType == null))
			{
				if (abilityRequestData.m_targets.Count == 0)
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
							abilityOfActionType.Targeter.UpdateEffectOnCaster(abilityRequestData.m_targets[0], actorData);
							abilityOfActionType.Targeter.UpdateTargetAreaEffect(abilityRequestData.m_targets[0], actorData);
							abilityOfActionType.Targeter.UpdateTargeting(abilityRequestData.m_targets[0], actorData);
							abilityOfActionType.Targeter.StartConfirmedTargeting(abilityRequestData.m_targets[0], actorData);
							abilityOfActionType.Targeter.UpdateFadeOutHighlights(actorData);
						}
						else if (abilityOfActionType.GetExpectedNumberOfTargeters() < 2)
						{
							abilityOfActionType.Targeter.SetLastUpdateCursorState(abilityRequestData.m_targets[0]);
							abilityOfActionType.Targeter.UpdateTargeting(abilityRequestData.m_targets[0], actorData);
							abilityOfActionType.Targeter.StartConfirmedTargeting(abilityRequestData.m_targets[0], actorData);
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
							if (num > abilityRequestData.m_targets.Count)
							{
								num = abilityRequestData.m_targets.Count;
							}
							for (int i = 0; i < num; i++)
							{
								if (abilityOfActionType.Targeters[i].IsUsingMultiTargetUpdate())
								{
									abilityOfActionType.Targeters[i].SetLastUpdateCursorState(abilityRequestData.m_targets[i]);
									abilityOfActionType.Targeters[i].UpdateTargetingMultiTargets(abilityRequestData.m_targets[i], actorData, i, abilityRequestData.m_targets);
								}
								else
								{
									abilityOfActionType.Targeters[i].SetLastUpdateCursorState(abilityRequestData.m_targets[i]);
									abilityOfActionType.Targeters[i].UpdateTargeting(abilityRequestData.m_targets[i], actorData);
								}
								abilityOfActionType.Targeters[i].StartConfirmedTargeting(abilityRequestData.m_targets[i], actorData);
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
		this.m_targetersBeingDrawn = true;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (args == null)
		{
			return;
		}
		if (eventType == GameEventManager.EventType.ReconnectReplayStateChanged)
		{
			this.ClearAbilityCooldowns();
			this.MarkForForceRedraw();
		}
	}

	private void OnDestroy()
	{
		if (NetworkClient.active)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReconnectReplayStateChanged);
		}
	}

	public void MarkForForceRedraw()
	{
		this.m_markedForForceRedraw = true;
	}

	private void UpdateDrawnTargeters()
	{
		ActorData actorData = this.m_actorData;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		ActorData y = (!(Board.Get().PlayerFreeSquare != null)) ? null : Board.Get().PlayerFreeSquare.OccupantActor;
		AbilityData abilityData = actorData.GetAbilityData();
		List<ActorTargeting.AbilityRequestData> abilityRequestDataForClient = this.GetAbilityRequestDataForClient();
		int num = 0;
		for (int i = 0; i < abilityRequestDataForClient.Count; i++)
		{
			ActorTargeting.AbilityRequestData abilityRequestData = abilityRequestDataForClient[i];
			Ability abilityOfActionType = abilityData.GetAbilityOfActionType(abilityRequestData.m_actionType);
			if (abilityOfActionType == null)
			{
			}
			else
			{
				if (actorData == activeOwnedActorData)
				{
					if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromSelf))
					{
						goto IL_26A;
					}
				}
				if (actorData != activeOwnedActorData)
				{
					if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromAllies))
					{
						goto IL_26A;
					}
				}
				if (abilityOfActionType.Targeter != null)
				{
					int j = 0;
					while (j < abilityOfActionType.Targeters.Count)
					{
						if (j >= abilityOfActionType.GetExpectedNumberOfTargeters())
						{
							break;
						}
						if (j >= abilityRequestData.m_targets.Count)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								goto IL_20F;
							}
						}
						else
						{
							AbilityUtil_Targeter abilityUtil_Targeter = abilityOfActionType.Targeters[j];
							if (abilityUtil_Targeter != null)
							{
								if (actorData == y)
								{
									goto IL_177;
								}
								if (InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo))
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										goto IL_177;
									}
								}
								else
								{
									actorData.ForceDisplayTargetHighlight = false;
								}
								IL_19F:
								abilityUtil_Targeter.UpdateConfirmedTargeting(abilityRequestData.m_targets[j], actorData);
								goto IL_1B7;
								IL_177:
								actorData.ForceDisplayTargetHighlight = true;
								abilityUtil_Targeter.StartConfirmedTargeting(abilityRequestData.m_targets[j], actorData);
								goto IL_19F;
							}
							IL_1B7:
							j++;
						}
					}
				}
				IL_20F:
				if (!(actorData != activeOwnedActorData))
				{
					if (GameFlowData.Get().m_ownedActorDatas.Count <= 1)
					{
						goto IL_26A;
					}
				}
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateTargetingAbilityIndicator(actorData, abilityOfActionType, abilityRequestData.m_actionType, num);
				num++;
			}
			IL_26A:;
		}
		if (!(actorData != activeOwnedActorData))
		{
			if (GameFlowData.Get().m_ownedActorDatas.Count <= 1)
			{
				goto IL_2BC;
			}
		}
		this.UpdateAbilityCooldowns(num, false);
		IL_2BC:
		this.m_targetersBeingDrawn = true;
	}

	public void UpdateAbilityCooldowns(int targetingAbilityIndicatorIndex, bool showRequestedAbilities)
	{
		AbilityData abilityData = this.m_actorData.GetAbilityData();
		AbilityData.ActionType actionTargeting = abilityData.GetSelectedActionTypeForTargeting();
		bool flag = this.InSpectatorModeAndHideTargeting();
		bool flag2 = false;
		if (actionTargeting != AbilityData.ActionType.INVALID_ACTION)
		{
			if (!flag)
			{
				if (GameFlowData.Get().LocalPlayerData.IsViewingTeam(this.m_actorData.GetTeam()))
				{
					flag2 = true;
				}
			}
		}
		List<ActorTargeting.AbilityRequestData> abilityRequestDataForClient = this.GetAbilityRequestDataForClient();
		ActorData actorData;
		if (Board.Get().PlayerFreeSquare != null)
		{
			actorData = Board.Get().PlayerFreeSquare.OccupantActor;
		}
		else
		{
			actorData = null;
		}
		ActorData y = actorData;
		if (!(this.m_actorData == y))
		{
			if (!InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo))
			{
				ActorTargeting.s_updatedAbilityCooldownsActors.Clear();
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetCatalystPipsVisible(this.m_actorData, false);
				goto IL_310;
			}
		}
		if (Time.time != ActorTargeting.s_lastTimeAddedAbilityCooldownsActor)
		{
			if (!ActorTargeting.s_updatedAbilityCooldownsActors.Contains(this.m_actorData))
			{
				ActorTargeting.s_updatedAbilityCooldownsActors.Add(this.m_actorData);
				ActorTargeting.s_lastTimeAddedAbilityCooldownsActor = Time.time;
			}
			int actualMaxTechPoints = this.m_actorData.GetActualMaxTechPoints();
			List<Ability> abilitiesAsList = abilityData.GetAbilitiesAsList();
			using (List<Ability>.Enumerator enumerator = abilitiesAsList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ability ability = enumerator.Current;
					if (ability == null)
					{
					}
					else
					{
						AbilityData.ActionType abilityAction = abilityData.GetActionTypeOfAbility(ability);
						int moddedCost = ability.GetModdedCost();
						bool flag3 = abilityAction == AbilityData.ActionType.ABILITY_0;
						bool flag4;
						if (abilityAction == AbilityData.ActionType.ABILITY_4)
						{
							flag4 = (moddedCost >= actualMaxTechPoints);
						}
						else
						{
							flag4 = false;
						}
						bool flag5 = flag4;
						if (!flag3)
						{
							if (!flag5)
							{
								if (flag2)
								{
									if (abilityAction == actionTargeting)
									{
										continue;
									}
								}
								if (GameFlowData.Get().LocalPlayerData.IsViewingTeam(this.m_actorData.GetTeam()))
								{
									if (!showRequestedAbilities)
									{
										if (abilityRequestDataForClient.FirstOrDefault((ActorTargeting.AbilityRequestData a) => a.m_actionType == abilityAction) != null)
										{
											continue;
										}
									}
								}
								HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateTargetingAbilityIndicator(this.m_actorData, ability, abilityAction, targetingAbilityIndicatorIndex);
								targetingAbilityIndicatorIndex++;
							}
						}
					}
				}
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetCatalystPipsVisible(this.m_actorData, true);
		}
		IL_310:
		if (flag2)
		{
			if (abilityRequestDataForClient.FirstOrDefault((ActorTargeting.AbilityRequestData a) => a.m_actionType == actionTargeting) == null)
			{
				if (actionTargeting != AbilityData.ActionType.CARD_0)
				{
					if (actionTargeting != AbilityData.ActionType.CARD_1)
					{
						if (actionTargeting != AbilityData.ActionType.CARD_2)
						{
							HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateTargetingAbilityIndicator(this.m_actorData, abilityData.GetAbilityOfActionType(actionTargeting), actionTargeting, targetingAbilityIndicatorIndex);
							targetingAbilityIndicatorIndex++;
						}
					}
				}
			}
		}
		HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.TurnOffTargetingAbilityIndicator(this.m_actorData, targetingAbilityIndicatorIndex);
	}

	public void ClearAbilityCooldowns()
	{
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.TurnOffTargetingAbilityIndicator(this.m_actorData, 0);
		}
	}

	public BoardSquare GetEvadeDestinationForTargeter()
	{
		BoardSquare boardSquare = null;
		List<ActorTargeting.AbilityRequestData> abilityRequestDataForClient = this.GetAbilityRequestDataForClient();
		if (abilityRequestDataForClient != null)
		{
			if (abilityRequestDataForClient.Count > 0)
			{
				if (this.m_actorData != null)
				{
					AbilityData abilityData = this.m_actorData.GetAbilityData();
					int i = 0;
					while (i < abilityRequestDataForClient.Count)
					{
						if (!(boardSquare == null))
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								return boardSquare;
							}
						}
						else
						{
							ActorTargeting.AbilityRequestData abilityRequestData = abilityRequestDataForClient[i];
							if (abilityRequestData != null)
							{
								Ability abilityOfActionType = abilityData.GetAbilityOfActionType(abilityRequestData.m_actionType);
								if (abilityOfActionType != null && abilityOfActionType.GetRunPriority() == AbilityPriority.Evasion)
								{
									boardSquare = abilityOfActionType.GetEvadeDestinationForTargeter(abilityRequestData.m_targets, this.m_actorData);
								}
							}
							i++;
						}
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
		return false;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}

	public class AbilityRequestData : IComparable
	{
		public AbilityData.ActionType m_actionType;

		public List<AbilityTarget> m_targets;

		public AbilityRequestData(AbilityData.ActionType actionType, List<AbilityTarget> targets)
		{
			this.m_actionType = actionType;
			this.m_targets = targets;
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			ActorTargeting.AbilityRequestData abilityRequestData = obj as ActorTargeting.AbilityRequestData;
			if (abilityRequestData != null)
			{
				return this.m_actionType.CompareTo(abilityRequestData.m_actionType);
			}
			throw new ArgumentException("Object is not an AbilityRequestData");
		}
	}
}
