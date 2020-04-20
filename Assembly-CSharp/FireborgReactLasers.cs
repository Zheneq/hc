using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;
using UnityEngine.Networking;

public class FireborgReactLasers : GenericAbility_Container
{
	[Separator("On Hit Data - For Second Laser", "yellow")]
	public OnHitAuthoredData m_onHitDataForSecondLaser;

	[Separator("When to apply ignite and ground fire effects?", true)]
	public FireborgReactLasers.HitEffectApplySetting m_ignitedApplySetting;

	public FireborgReactLasers.HitEffectApplySetting m_groundFireApplySetting;

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
		return usageForEditor + Fireborg_SyncComponent.GetSuperheatedCvarUsage();
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(Fireborg_SyncComponent.s_cvarSuperheated.GetName());
		return contextNamesForEditor;
	}

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "-- On Hit Data for Reaction --\n" + this.m_onHitDataForSecondLaser.GetInEditorDesc();
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Fireborg_SyncComponent>();
		this.m_myActionType = base.GetActionTypeOfAbility(this);
		this.SetCachedFields();
		base.SetupTargetersAndCachedVars();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		this.m_onHitDataForSecondLaser.AddTooltipTokens(tokens);
		base.AddTokenInt(tokens, "ExtraShieldIfLowHealth", string.Empty, this.m_extraShieldIfLowHealth, false);
		base.AddTokenInt(tokens, "LowHealthThresh", string.Empty, this.m_lowHealthThresh, false);
		base.AddTokenInt(tokens, "ShieldPerHitReceivedForNextTurn", string.Empty, this.m_shieldPerHitReceivedForNextTurn, false);
		base.AddTokenInt(tokens, "EarlyDepleteShieldOnNextTurn", string.Empty, this.m_earlyDepleteShieldOnNextTurn, false);
	}

	private void SetCachedFields()
	{
		this.m_cachedOnHitDataForSecondLaser = ((!(this.m_abilityMod != null)) ? this.m_onHitDataForSecondLaser : this.m_abilityMod.m_onHitDataForSecondLaserMod.symbol_001D(this.m_onHitDataForSecondLaser));
	}

	public OnHitAuthoredData GetOnHitDataForSecondLaser()
	{
		OnHitAuthoredData result;
		if (this.m_cachedOnHitDataForSecondLaser != null)
		{
			result = this.m_cachedOnHitDataForSecondLaser;
		}
		else
		{
			result = this.m_onHitDataForSecondLaser;
		}
		return result;
	}

	public int GetExtraShieldIfLowHealth()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_extraShieldIfLowHealthMod.GetModifiedValue(this.m_extraShieldIfLowHealth);
		}
		else
		{
			result = this.m_extraShieldIfLowHealth;
		}
		return result;
	}

	public int GetLowHealthThresh()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_lowHealthThreshMod.GetModifiedValue(this.m_lowHealthThresh);
		}
		else
		{
			result = this.m_lowHealthThresh;
		}
		return result;
	}

	public int GetShieldPerHitReceivedForNextTurn()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_shieldPerHitReceivedForNextTurnMod.GetModifiedValue(this.m_shieldPerHitReceivedForNextTurn);
		}
		else
		{
			result = this.m_shieldPerHitReceivedForNextTurn;
		}
		return result;
	}

	public int GetEarlyDepleteShieldOnNextTurn()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_earlyDepleteShieldOnNextTurnMod.GetModifiedValue(this.m_earlyDepleteShieldOnNextTurn);
		}
		else
		{
			result = this.m_earlyDepleteShieldOnNextTurn;
		}
		return result;
	}

	public override void PostProcessTargetingNumbers(ActorData targetActor, int currentTargeterIndex, Dictionary<ActorData, ActorHitContext> actorHitContext, ContextVars abilityContext, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (targetActor == caster && this.GetExtraShieldIfLowHealth() > 0)
		{
			if (caster.HitPoints < this.GetLowHealthThresh())
			{
				if (results.m_absorb >= 0)
				{
					results.m_absorb += this.GetExtraShieldIfLowHealth();
				}
				else
				{
					results.m_absorb = this.GetExtraShieldIfLowHealth();
				}
			}
		}
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (this.m_groundFireApplySetting.ShouldApply(true, this.m_syncComp.InSuperheatMode()))
		{
			if (!this.m_syncComp.m_actorsInGroundFireOnTurnStart.Contains((uint)targetActor.ActorIndex))
			{
				return this.m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
			}
		}
		return null;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		this.m_abilityMod = (abilityMod as AbilityMod_FireborgReactLasers);
	}

	protected override void GenModImpl_ClearModRef()
	{
		this.m_abilityMod = null;
	}

	public override void OnClientCombatPhasePlayDataReceived(List<ClientResolutionAction> resolutionActions, ActorData caster)
	{
		if (NetworkClient.active)
		{
			ClientResolutionAction clientResolutionAction = null;
			ClientResolutionAction clientResolutionAction2 = null;
			foreach (ClientResolutionAction clientResolutionAction3 in resolutionActions)
			{
				if (clientResolutionAction == null)
				{
					if (clientResolutionAction3.GetSourceAbilityActionType() == this.m_myActionType)
					{
						if (clientResolutionAction3.GetCaster() == caster)
						{
							if (clientResolutionAction3.IsResolutionActionType(ResolutionActionType.EffectAnimation))
							{
								clientResolutionAction = clientResolutionAction3;
							}
						}
					}
				}
				if (clientResolutionAction2 == null)
				{
					if (clientResolutionAction3.HasReactionHitByCaster(caster))
					{
						clientResolutionAction2 = clientResolutionAction3;
					}
				}
			}
			if (clientResolutionAction != null)
			{
				if (clientResolutionAction2 != null)
				{
					int playOrderOfClientAction = TheatricsManager.Get().GetPlayOrderOfClientAction(clientResolutionAction, AbilityPriority.Combat_Damage);
					int playOrderOfFirstDamagingHitOnActor = TheatricsManager.Get().GetPlayOrderOfFirstDamagingHitOnActor(caster, AbilityPriority.Combat_Damage);
					if (playOrderOfFirstDamagingHitOnActor >= 0)
					{
						if (playOrderOfFirstDamagingHitOnActor < playOrderOfClientAction)
						{
							Dictionary<ActorData, ClientActorHitResults> dictionary;
							Dictionary<Vector3, ClientPositionHitResults> dictionary2;
							clientResolutionAction.GetHitResults(out dictionary, out dictionary2);
							Dictionary<ActorData, ClientActorHitResults> dictionary3;
							Dictionary<Vector3, ClientPositionHitResults> dictionary4;
							clientResolutionAction2.GetReactionHitResultsByCaster(caster, out dictionary3, out dictionary4);
							if (dictionary != null && dictionary2 != null)
							{
								if (dictionary3 != null)
								{
									if (dictionary4 != null)
									{
										List<ActorData> list = new List<ActorData>(dictionary.Keys);
										List<Vector3> list2 = new List<Vector3>(dictionary2.Keys);
										using (List<ActorData>.Enumerator enumerator2 = list.GetEnumerator())
										{
											while (enumerator2.MoveNext())
											{
												ActorData key = enumerator2.Current;
												if (dictionary3.ContainsKey(key))
												{
													ClientActorHitResults value = dictionary[key];
													dictionary[key] = dictionary3[key];
													dictionary3[key] = value;
													ClientActorHitResults clientActorHitResults = dictionary3[key];
													ClientActorHitResults clientActorHitResults2 = dictionary[key];
													if (this.m_syncComp.InSuperheatMode())
													{
														if (this.m_ignitedApplySetting.m_firstSuperheated && clientActorHitResults.GetNumEffectsToStart() < clientActorHitResults2.GetNumEffectsToStart())
														{
															clientActorHitResults.SwapEffectsToStart(clientActorHitResults2);
														}
													}
													else
													{
														if (!this.m_ignitedApplySetting.m_firstNormal)
														{
															if (clientActorHitResults.GetNumEffectsToStart() > clientActorHitResults2.GetNumEffectsToStart())
															{
																clientActorHitResults.SwapEffectsToStart(clientActorHitResults2);
																continue;
															}
														}
														if (this.m_ignitedApplySetting.m_firstNormal)
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
												Vector3 key2 = enumerator3.Current;
												if (dictionary4.ContainsKey(key2))
												{
													ClientPositionHitResults value2 = dictionary2[key2];
													dictionary2[key2] = dictionary4[key2];
													dictionary4[key2] = value2;
												}
											}
										}
										return;
									}
								}
							}
							Debug.LogError(base.GetType() + " has empty hit results when trying to swap them on client");
						}
					}
				}
			}
		}
	}

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
				if (this.m_firstNormal)
				{
					goto IL_8D;
				}
			}
			if (!isFirst)
			{
				if (!superheated)
				{
					if (this.m_secondNormal)
					{
						goto IL_8D;
					}
				}
			}
			if (isFirst)
			{
				if (superheated)
				{
					if (this.m_firstSuperheated)
					{
						goto IL_8D;
					}
				}
			}
			int result;
			if (!isFirst)
			{
				if (superheated)
				{
					result = (this.m_secondSuperheated ? 1 : 0);
					goto IL_8B;
				}
			}
			result = 0;
			IL_8B:
			return result != 0;
			IL_8D:
			result = 1;
			return result != 0;
		}
	}
}
