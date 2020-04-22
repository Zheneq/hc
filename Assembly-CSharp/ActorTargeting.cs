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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
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
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReconnectReplayStateChanged);
			return;
		}
	}

	private void ResetTargeters(bool clearInstantly)
	{
		if (m_actorData == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_actorData = GetComponent<ActorData>();
		}
		ActorTurnSM actorTurnSM = m_actorData.GetActorTurnSM();
		AbilityData abilityData = m_actorData.GetAbilityData();
		object obj;
		if (GameFlowData.Get() != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
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
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				continue;
			}
			if (m_actorData == y)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (actorTurnSM != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION && abilityOfActionType.IsAbilitySelected())
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
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
			switch (6)
			{
			case 0:
				continue;
			}
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
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			DrawTargeters();
			CalculateTargetedActors();
			return;
		}
	}

	public List<AbilityRequestData> GetAbilityRequestDataForClient()
	{
		if (m_actorData != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ResetTargeters(false);
			m_targetersBeingDrawn = false;
			m_markedForForceRedraw = false;
		}
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
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
			switch (5)
			{
			case 0:
				continue;
			}
			if (m_targetersBeingDrawn)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < abilityRequestDataForClient.Count; i++)
			{
				AbilityRequestData abilityRequestData = abilityRequestDataForClient[i];
				if (abilityRequestData == null)
				{
					continue;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (abilityRequestData.m_actionType != actionType)
				{
					continue;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					return abilityRequestData.m_targets;
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
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
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromSelf))
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							continue;
						}
					}
					if (actorData != activeOwnedActorData)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromAllies))
						{
							continue;
						}
					}
					if (abilityOfActionType.Targeter != null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						for (int i = 0; i < abilityOfActionType.Targeters.Count && i < abilityOfActionType.GetExpectedNumberOfTargeters(); i++)
						{
							if (i >= current.m_targets.Count)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
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
													while (true)
													{
														switch (7)
														{
														case 0:
															continue;
														}
														break;
													}
													m_currentTargetedActors[current2.m_actor] = new Dictionary<AbilityTooltipSymbol, int>();
												}
												if (!m_currentTargetedActors[current2.m_actor].ContainsKey(key))
												{
													while (true)
													{
														switch (3)
														{
														case 0:
															continue;
														}
														break;
													}
													m_currentTargetedActors[current2.m_actor][key] = 0;
												}
												m_currentTargetedActors[current2.m_actor][key] += current3.Value;
											}
											while (true)
											{
												switch (3)
												{
												case 0:
													continue;
												}
												break;
											}
										}
									}
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		bool flag2 = false;
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			if (!abilityTargeting.Targeters[i].IsActorInTargetRange(target, out bool inCover))
			{
				continue;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			flag2 = true;
			if (i == 0)
			{
				flag = inCover;
				continue;
			}
			int num;
			if (flag)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				num = (inCover ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			flag = ((byte)num != 0);
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (!flag2)
			{
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			HashSet<AbilityTooltipSubject> hashSet = new HashSet<AbilityTooltipSubject>();
			if (abilityTargeting.GetExpectedNumberOfTargeters() > 1)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int j = 0; j <= currentTargeterIndex; j++)
				{
					abilityTargeting.Targeters[j].AppendToTooltipSubjectSet(target, hashSet);
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			else
			{
				abilityTargeting.Targeter.AppendToTooltipSubjectSet(target, hashSet);
			}
			List<AbilityTooltipNumber> nameplateTargetingNumbers = abilityTargeting.GetNameplateTargetingNumbers();
			int num2;
			if (!AbilityUtils.AbilityHasTag(abilityTargeting, AbilityTags.NameplateTargetNumberIgnoreCover))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				num2 = (abilityTargeting.ForceIgnoreCover(target) ? 1 : 0);
			}
			else
			{
				num2 = 1;
			}
			bool flag3 = (byte)num2 != 0;
			bool reducedCoverEffectiveness = AbilityUtils.AbilityReduceCoverEffectiveness(abilityTargeting, target);
			Dictionary<AbilityTooltipSymbol, int> dictionary = null;
			s_targetingNumberUpdateScratch.ResetForCalc();
			bool customTargeterNumbers = abilityTargeting.GetCustomTargeterNumbers(target, currentTargeterIndex, s_targetingNumberUpdateScratch);
			if (customTargeterNumbers)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (abilityTargeting is GenericAbility_Container)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							if (s_targetingNumberUpdateScratch.m_damage > 0)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								int value = AbilityUtils.CalculateDamageForTargeter(caster, target, abilityTargeting, s_targetingNumberUpdateScratch.m_damage, flag && !flag3);
								AddSymbolToValuePair(symbolToValueMap, AbilityTooltipSymbol.Damage, value);
							}
							if (s_targetingNumberUpdateScratch.m_healing > 0)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								int value2 = AbilityUtils.CalculateHealingForTargeter(caster, target, abilityTargeting, s_targetingNumberUpdateScratch.m_healing);
								AddSymbolToValuePair(symbolToValueMap, AbilityTooltipSymbol.Healing, value2);
							}
							if (s_targetingNumberUpdateScratch.m_absorb > 0)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								int value3 = AbilityUtils.CalculateAbsorbForTargeter(caster, target, abilityTargeting, s_targetingNumberUpdateScratch.m_absorb);
								AddSymbolToValuePair(symbolToValueMap, AbilityTooltipSymbol.Absorb, value3);
							}
							if (s_targetingNumberUpdateScratch.m_energy > 0)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
										if (caster != target)
										{
											while (true)
											{
												switch (4)
												{
												case 0:
													break;
												default:
												{
													int value4 = AbilityUtils.CalculateTechPointsForTargeter(target, abilityTargeting, s_targetingNumberUpdateScratch.m_energy);
													AddSymbolToValuePair(symbolToValueMap, AbilityTooltipSymbol.Energy, value4);
													return;
												}
												}
											}
										}
										return;
									}
								}
							}
							return;
						}
					}
				}
			}
			if (!customTargeterNumbers)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				dictionary = abilityTargeting.GetCustomNameplateItemTooltipValues(target, currentTargeterIndex);
			}
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			using (HashSet<AbilityTooltipSubject>.Enumerator enumerator = hashSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityTooltipSubject current = enumerator.Current;
					using (List<AbilityTooltipNumber>.Enumerator enumerator2 = nameplateTargetingNumbers.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							AbilityTooltipNumber current2 = enumerator2.Current;
							if (current2.m_subject != current)
							{
								continue;
							}
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							int num3 = current2.m_value;
							if (customTargeterNumbers)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								if (s_targetingNumberUpdateScratch.HasOverride(current2.m_symbol))
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
									num3 = s_targetingNumberUpdateScratch.GetOverrideValue(current2.m_symbol);
									goto IL_0358;
								}
							}
							if (dictionary != null && dictionary.ContainsKey(current2.m_symbol))
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								num3 = dictionary[current2.m_symbol];
							}
							goto IL_0358;
							IL_0358:
							switch (current2.m_symbol)
							{
							case AbilityTooltipSymbol.Damage:
								if (flag4)
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									num3 = 0;
								}
								if (num3 > 0)
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									if (m_showStatusAdjustedTargetingNumbers)
									{
										num3 = AbilityUtils.CalculateDamageForTargeter(caster, target, abilityTargeting, num3, flag && !flag3);
									}
									else if (flag)
									{
										while (true)
										{
											switch (1)
											{
											case 0:
												continue;
											}
											break;
										}
										if (!flag3)
										{
											while (true)
											{
												switch (4)
												{
												case 0:
													continue;
												}
												break;
											}
											num3 = AbilityUtils.ApplyCoverDamageReduction(target.GetActorStats(), num3, reducedCoverEffectiveness);
										}
									}
									flag4 = true;
								}
								break;
							case AbilityTooltipSymbol.Healing:
								if (flag5)
								{
									num3 = 0;
								}
								if (num3 > 0)
								{
									if (m_showStatusAdjustedTargetingNumbers)
									{
										num3 = AbilityUtils.CalculateHealingForTargeter(caster, target, abilityTargeting, num3);
									}
									flag5 = true;
								}
								break;
							case AbilityTooltipSymbol.Absorb:
								if (flag7)
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									num3 = 0;
								}
								if (num3 > 0)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (m_showStatusAdjustedTargetingNumbers)
									{
										while (true)
										{
											switch (1)
											{
											case 0:
												continue;
											}
											break;
										}
										num3 = AbilityUtils.CalculateAbsorbForTargeter(caster, target, abilityTargeting, num3);
									}
									flag7 = true;
								}
								break;
							case AbilityTooltipSymbol.Energy:
								if (flag6)
								{
									num3 = 0;
								}
								if (caster != target)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (num3 > 0)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										if (m_showStatusAdjustedTargetingNumbers)
										{
											while (true)
											{
												switch (5)
												{
												case 0:
													continue;
												}
												break;
											}
											num3 = AbilityUtils.CalculateTechPointsForTargeter(target, abilityTargeting, num3);
										}
										flag6 = true;
									}
								}
								break;
							}
							if (num3 > 0)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!symbolToValueMap.ContainsKey(current2.m_symbol))
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									symbolToValueMap.Add(current2.m_symbol, num3);
								}
								else
								{
									symbolToValueMap[current2.m_symbol] += num3;
								}
							}
						}
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				while (true)
				{
					switch (6)
					{
					default:
						return;
					case 0:
						break;
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
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_currentTargetedActors[target].ContainsKey(symbol))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_actorData = GetComponent<ActorData>();
		}
		ActorData actorData = m_actorData;
		if (!(GameFlowData.Get() == null))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!ClientGameManager.Get().IsFastForward)
			{
				if (SinglePlayerManager.Get() != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!SinglePlayerManager.Get().EnableCooldownIndicators())
					{
						return false;
					}
				}
				ActorData actorData = m_actorData;
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (actorData == activeOwnedActorData)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
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
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
				}
				else if (!(actorData == activeOwnedActorData) || !AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromSelf))
				{
					if (actorData != activeOwnedActorData && AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromAllies))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					else if (abilityOfActionType.Targeter != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.UseTeleportUIEffect))
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							abilityOfActionType.Targeter.UpdateEffectOnCaster(item.m_targets[0], actorData);
							abilityOfActionType.Targeter.UpdateTargetAreaEffect(item.m_targets[0], actorData);
							abilityOfActionType.Targeter.UpdateTargeting(item.m_targets[0], actorData);
							abilityOfActionType.Targeter.StartConfirmedTargeting(item.m_targets[0], actorData);
							abilityOfActionType.Targeter.UpdateFadeOutHighlights(actorData);
						}
						else if (abilityOfActionType.GetExpectedNumberOfTargeters() < 2)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
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
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								num = abilityOfActionType.GetNumTargets();
							}
							if (num > abilityOfActionType.Targeters.Count)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								num = abilityOfActionType.Targeters.Count;
							}
							if (num > item.m_targets.Count)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								num = item.m_targets.Count;
							}
							for (int i = 0; i < num; i++)
							{
								if (abilityOfActionType.Targeters[i].IsUsingMultiTargetUpdate())
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
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
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						abilityOfActionType.Targeter.UpdateArrowsForUI();
						if (actorData == activeOwnedActorData && abilityOfActionType != null && abilityOfActionType.ShouldAutoQueueIfValid())
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							if (abilityRequestDataForClient.Count == 1 && actorData.GetAbilityData().GetLastSelectedAbility() == null)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
			switch (2)
			{
			case 0:
				continue;
			}
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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				continue;
			}
			if (actorData == activeOwnedActorData)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromSelf))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					continue;
				}
			}
			if (actorData != activeOwnedActorData)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.HideConfirmedTargeterFromAllies))
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					continue;
				}
			}
			if (abilityOfActionType.Targeter != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int j = 0; j < abilityOfActionType.Targeters.Count; j++)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (j >= abilityOfActionType.GetExpectedNumberOfTargeters())
					{
						break;
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
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
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!(actorData == y))
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo))
							{
								actorData.ForceDisplayTargetHighlight = false;
								goto IL_019f;
							}
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						actorData.ForceDisplayTargetHighlight = true;
						abilityUtil_Targeter.StartConfirmedTargeting(abilityRequestData.m_targets[j], actorData);
						goto IL_019f;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					break;
					IL_019f:
					abilityUtil_Targeter.UpdateConfirmedTargeting(abilityRequestData.m_targets[j], actorData);
				}
			}
			if (!(actorData != activeOwnedActorData))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameFlowData.Get().m_ownedActorDatas.Count <= 1)
				{
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateTargetingAbilityIndicator(actorData, abilityOfActionType, abilityRequestData.m_actionType, num);
			num++;
		}
		if (!(actorData != activeOwnedActorData))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (GameFlowData.Get().m_ownedActorDatas.Count <= 1)
			{
				goto IL_02bc;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!flag)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameFlowData.Get().LocalPlayerData.IsViewingTeam(m_actorData.GetTeam()))
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					flag2 = true;
				}
			}
		}
		List<AbilityRequestData> abilityRequestDataForClient = GetAbilityRequestDataForClient();
		object obj;
		if (Board.Get().PlayerFreeSquare != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			obj = Board.Get().PlayerFreeSquare.OccupantActor;
		}
		else
		{
			obj = null;
		}
		ActorData y = (ActorData)obj;
		if (!(m_actorData == y))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo))
			{
				s_updatedAbilityCooldownsActors.Clear();
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetCatalystPipsVisible(m_actorData, false);
				goto IL_0310;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (Time.time != s_lastTimeAddedAbilityCooldownsActor)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!s_updatedAbilityCooldownsActors.Contains(m_actorData))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
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
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					else
					{
						AbilityData.ActionType abilityAction = abilityData.GetActionTypeOfAbility(current);
						int moddedCost = current.GetModdedCost();
						bool flag3 = abilityAction == AbilityData.ActionType.ABILITY_0;
						int num;
						if (abilityAction == AbilityData.ActionType.ABILITY_4)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							num = ((moddedCost >= actualMaxTechPoints) ? 1 : 0);
						}
						else
						{
							num = 0;
						}
						bool flag4 = (byte)num != 0;
						if (!flag3)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!flag4)
							{
								if (flag2)
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									if (abilityAction == actionTargeting)
									{
										continue;
									}
								}
								if (GameFlowData.Get().LocalPlayerData.IsViewingTeam(m_actorData.GetTeam()))
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!showRequestedAbilities)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
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
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetCatalystPipsVisible(m_actorData, true);
		}
		goto IL_0310;
		IL_0310:
		if (flag2)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (abilityRequestDataForClient.FirstOrDefault((AbilityRequestData a) => a.m_actionType == actionTargeting) == null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (actionTargeting != AbilityData.ActionType.CARD_0)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (actionTargeting != AbilityData.ActionType.CARD_1)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (actionTargeting != AbilityData.ActionType.CARD_2)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
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
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (abilityRequestDataForClient.Count > 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_actorData != null)
				{
					AbilityData abilityData = m_actorData.GetAbilityData();
					for (int i = 0; i < abilityRequestDataForClient.Count; i++)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (boardSquare == null)
						{
							AbilityRequestData abilityRequestData = abilityRequestDataForClient[i];
							if (abilityRequestData == null)
							{
								continue;
							}
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							Ability abilityOfActionType = abilityData.GetAbilityOfActionType(abilityRequestData.m_actionType);
							if (abilityOfActionType != null && abilityOfActionType.GetRunPriority() == AbilityPriority.Evasion)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								boardSquare = abilityOfActionType.GetEvadeDestinationForTargeter(abilityRequestData.m_targets, m_actorData);
							}
							continue;
						}
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
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
