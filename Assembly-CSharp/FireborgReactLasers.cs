using AbilityContextNamespace;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class FireborgReactLasers : GenericAbility_Container
{
	[Serializable]
	public class HitEffectApplySetting
	{
		public bool m_firstNormal;

		public bool m_secondNormal;

		public bool m_firstSuperheated;

		public bool m_secondSuperheated;

		public bool ShouldApply(bool isFirst, bool superheated)
		{
			if (isFirst && !superheated)
			{
				if (m_firstNormal)
				{
					goto IL_008d;
				}
			}
			if (!isFirst)
			{
				if (!superheated)
				{
					if (m_secondNormal)
					{
						goto IL_008d;
					}
				}
			}
			if (isFirst)
			{
				if (superheated)
				{
					if (m_firstSuperheated)
					{
						goto IL_008d;
					}
				}
			}
			int result;
			if (!isFirst)
			{
				if (superheated)
				{
					result = (m_secondSuperheated ? 1 : 0);
					goto IL_008e;
				}
			}
			result = 0;
			goto IL_008e;
			IL_008e:
			return (byte)result != 0;
			IL_008d:
			result = 1;
			goto IL_008e;
		}
	}

	[Separator("On Hit Data - For Second Laser", "yellow")]
	public OnHitAuthoredData m_onHitDataForSecondLaser;

	[Separator("When to apply ignite and ground fire effects?", true)]
	public HitEffectApplySetting m_ignitedApplySetting;

	public HitEffectApplySetting m_groundFireApplySetting;

	[Separator("Extra Shield", true)]
	public int m_extraShieldIfLowHealth;

	public int m_lowHealthThresh;

	[Header("-- shield per damaging hit, applied on next turn")]
	public int m_shieldPerHitReceivedForNextTurn;

	[Header("-- shield applied on next turn if depleted this turn")]
	public int m_earlyDepleteShieldOnNextTurn;

	[Separator("Sequences", true)]
	public GameObject m_persistentSeqPrefab;

	public GameObject m_onTriggerSeqPrefab;

	public GameObject m_reactionAnimTriggerSeqPrefab;

	[Header("-- Superheated Sequences")]
	public GameObject m_superheatedCastSeqPrefab;

	public GameObject m_superheatedOnTriggerSeqPrefab;

	public float m_onTriggerProjectileSeqStartDelay;

	[Separator("Animation", true)]
	public int m_mainLaserAnimationIndex = 4;

	private Fireborg_SyncComponent m_syncComp;

	private AbilityData.ActionType m_myActionType;

	private AbilityMod_FireborgReactLasers m_abilityMod;

	private OnHitAuthoredData m_cachedOnHitDataForSecondLaser;

	public override string GetUsageForEditor()
	{
		string usageForEditor = base.GetUsageForEditor();
		return new StringBuilder().Append(usageForEditor).Append(Fireborg_SyncComponent.GetSuperheatedCvarUsage()).ToString();
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(Fireborg_SyncComponent.s_cvarSuperheated.GetName());
		return contextNamesForEditor;
	}

	public override string GetOnHitDataDesc()
	{
		return new StringBuilder().Append(base.GetOnHitDataDesc()).Append("-- On Hit Data for Reaction --\n").Append(m_onHitDataForSecondLaser.GetInEditorDesc()).ToString();
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Fireborg_SyncComponent>();
		m_myActionType = GetActionTypeOfAbility(this);
		SetCachedFields();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_onHitDataForSecondLaser.AddTooltipTokens(tokens);
		AddTokenInt(tokens, "ExtraShieldIfLowHealth", string.Empty, m_extraShieldIfLowHealth);
		AddTokenInt(tokens, "LowHealthThresh", string.Empty, m_lowHealthThresh);
		AddTokenInt(tokens, "ShieldPerHitReceivedForNextTurn", string.Empty, m_shieldPerHitReceivedForNextTurn);
		AddTokenInt(tokens, "EarlyDepleteShieldOnNextTurn", string.Empty, m_earlyDepleteShieldOnNextTurn);
	}

	private void SetCachedFields()
	{
		m_cachedOnHitDataForSecondLaser = ((!(m_abilityMod != null)) ? m_onHitDataForSecondLaser : m_abilityMod.m_onHitDataForSecondLaserMod.GetModdedOnHitData(m_onHitDataForSecondLaser));
	}

	public OnHitAuthoredData GetOnHitDataForSecondLaser()
	{
		OnHitAuthoredData result;
		if (m_cachedOnHitDataForSecondLaser != null)
		{
			result = m_cachedOnHitDataForSecondLaser;
		}
		else
		{
			result = m_onHitDataForSecondLaser;
		}
		return result;
	}

	public int GetExtraShieldIfLowHealth()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_extraShieldIfLowHealthMod.GetModifiedValue(m_extraShieldIfLowHealth);
		}
		else
		{
			result = m_extraShieldIfLowHealth;
		}
		return result;
	}

	public int GetLowHealthThresh()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_lowHealthThreshMod.GetModifiedValue(m_lowHealthThresh);
		}
		else
		{
			result = m_lowHealthThresh;
		}
		return result;
	}

	public int GetShieldPerHitReceivedForNextTurn()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_shieldPerHitReceivedForNextTurnMod.GetModifiedValue(m_shieldPerHitReceivedForNextTurn);
		}
		else
		{
			result = m_shieldPerHitReceivedForNextTurn;
		}
		return result;
	}

	public int GetEarlyDepleteShieldOnNextTurn()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_earlyDepleteShieldOnNextTurnMod.GetModifiedValue(m_earlyDepleteShieldOnNextTurn);
		}
		else
		{
			result = m_earlyDepleteShieldOnNextTurn;
		}
		return result;
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (!(targetActor == caster) || GetExtraShieldIfLowHealth() <= 0)
		{
			return;
		}
		while (true)
		{
			if (caster.HitPoints >= GetLowHealthThresh())
			{
				return;
			}
			if (results.m_absorb >= 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						results.m_absorb += GetExtraShieldIfLowHealth();
						return;
					}
				}
			}
			results.m_absorb = GetExtraShieldIfLowHealth();
			return;
		}
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (m_groundFireApplySetting.ShouldApply(true, m_syncComp.InSuperheatMode()))
		{
			if (!m_syncComp.m_actorsInGroundFireOnTurnStart.Contains((uint)targetActor.ActorIndex))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
					}
				}
			}
		}
		return null;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = (abilityMod as AbilityMod_FireborgReactLasers);
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}

	public override void OnClientCombatPhasePlayDataReceived(List<ClientResolutionAction> resolutionActions, ActorData caster)
	{
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			ClientResolutionAction clientResolutionAction = null;
			ClientResolutionAction clientResolutionAction2 = null;
			foreach (ClientResolutionAction resolutionAction in resolutionActions)
			{
				if (clientResolutionAction == null)
				{
					if (resolutionAction.GetSourceAbilityActionType() == m_myActionType)
					{
						if (resolutionAction.GetCaster() == caster)
						{
							if (resolutionAction.IsResolutionActionType(ResolutionActionType.EffectAnimation))
							{
								clientResolutionAction = resolutionAction;
							}
						}
					}
				}
				if (clientResolutionAction2 == null)
				{
					if (resolutionAction.HasReactionHitByCaster(caster))
					{
						clientResolutionAction2 = resolutionAction;
					}
				}
			}
			if (clientResolutionAction == null)
			{
				return;
			}
			while (true)
			{
				if (clientResolutionAction2 == null)
				{
					return;
				}
				while (true)
				{
					int playOrderOfClientAction = TheatricsManager.Get().GetPlayOrderOfClientAction(clientResolutionAction, AbilityPriority.Combat_Damage);
					int playOrderOfFirstDamagingHitOnActor = TheatricsManager.Get().GetPlayOrderOfFirstDamagingHitOnActor(caster, AbilityPriority.Combat_Damage);
					if (playOrderOfFirstDamagingHitOnActor < 0)
					{
						return;
					}
					while (true)
					{
						if (playOrderOfFirstDamagingHitOnActor >= playOrderOfClientAction)
						{
							return;
						}
						while (true)
						{
							clientResolutionAction.GetHitResults(out Dictionary<ActorData, ClientActorHitResults> actorHitResList, out Dictionary<Vector3, ClientPositionHitResults> posHitResList);
							clientResolutionAction2.GetReactionHitResultsByCaster(caster, out Dictionary<ActorData, ClientActorHitResults> actorHitResList2, out Dictionary<Vector3, ClientPositionHitResults> posHitResList2);
							if (actorHitResList != null && posHitResList != null)
							{
								if (actorHitResList2 != null)
								{
									if (posHitResList2 != null)
									{
										List<ActorData> list = new List<ActorData>(actorHitResList.Keys);
										List<Vector3> list2 = new List<Vector3>(posHitResList.Keys);
										using (List<ActorData>.Enumerator enumerator2 = list.GetEnumerator())
										{
											while (enumerator2.MoveNext())
											{
												ActorData current2 = enumerator2.Current;
												if (actorHitResList2.ContainsKey(current2))
												{
													ClientActorHitResults value = actorHitResList[current2];
													actorHitResList[current2] = actorHitResList2[current2];
													actorHitResList2[current2] = value;
													ClientActorHitResults clientActorHitResults = actorHitResList2[current2];
													ClientActorHitResults clientActorHitResults2 = actorHitResList[current2];
													if (m_syncComp.InSuperheatMode())
													{
														if (m_ignitedApplySetting.m_firstSuperheated && clientActorHitResults.GetNumEffectsToStart() < clientActorHitResults2.GetNumEffectsToStart())
														{
															clientActorHitResults.SwapEffectsToStart(clientActorHitResults2);
														}
													}
													else
													{
														if (!m_ignitedApplySetting.m_firstNormal)
														{
															if (clientActorHitResults.GetNumEffectsToStart() > clientActorHitResults2.GetNumEffectsToStart())
															{
																clientActorHitResults.SwapEffectsToStart(clientActorHitResults2);
																continue;
															}
														}
														if (m_ignitedApplySetting.m_firstNormal)
														{
															if (clientActorHitResults.GetNumEffectsToStart() < clientActorHitResults2.GetNumEffectsToStart())
															{
																clientActorHitResults.SwapEffectsToStart(clientActorHitResults2);
															}
														}
													}
												}
											}
										}
										using (List<Vector3>.Enumerator enumerator3 = list2.GetEnumerator())
										{
											while (enumerator3.MoveNext())
											{
												Vector3 current3 = enumerator3.Current;
												if (posHitResList2.ContainsKey(current3))
												{
													ClientPositionHitResults value2 = posHitResList[current3];
													posHitResList[current3] = posHitResList2[current3];
													posHitResList2[current3] = value2;
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
							}
							Debug.LogError(string.Concat(GetType(), " has empty hit results when trying to swap them on client"));
							return;
						}
					}
				}
			}
		}
	}
}
