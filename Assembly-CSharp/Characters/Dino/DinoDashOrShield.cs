using System.Collections.Generic;
using System.Linq;
using AbilityContextNamespace;
using UnityEngine;

// TODO DINO Cooldown is zero, which can break cooldown reductions
public class DinoDashOrShield : GenericAbility_Container
{
	[Separator("Targeting")]
	public Color m_iconColorWhileActive;
	[Separator("[Dash]: Target Select for the second turn if you use the ability again")]
	public GenericAbility_TargetSelectBase m_targetSelectForDash;
	[Separator("[Dash]: On Hit Data for second turn, if using ability again to dash", "yellow")]
	public OnHitAuthoredData m_dashOnHitData;
	[Separator("[Dash]: Energy Interactions")]
	public TechPointInteraction[] m_dashTechPointInteractions;
	[Separator("[Dash]: Movement Adjust")]
	public MovementAdjustment m_dashMovementAdjust = MovementAdjustment.NoMovement;
	[Separator("[Dash]: Shielding per enemy hit")]
	public int m_shieldPerEnemyHit;
	public int m_shieldDuration = 1;
	[Separator("For No Dash, applied on end of prep phase")]
	public StandardEffectInfo m_shieldEffect;
	public int m_healIfNoDash;
	public int m_cdrIfNoDash;
	[Separator("Cooldown, set on turn after initial cast")]
	public int m_delayedCooldown;
	[Separator("Powering up primary")]
	public bool m_fullyChargeUpLayerCone;
	[Separator("Animation Index")]
	public ActorModelData.ActionAnimationType m_dashAnimIndex;
	public ActorModelData.ActionAnimationType m_noDashShieldAnimIndex;
	[Separator("Sequences")]
	public GameObject m_onTriggerSequencePrefab;

	private Dino_SyncComponent m_syncComp;
	private AbilityMod_DinoDashOrShield m_abilityMod;
	private OnHitAuthoredData m_cachedDashOnHitData;
	private StandardEffectInfo m_cachedShieldEffect;
	
#if SERVER
	// custom
	private Passive_Dino m_passive;
	private DinoLayerCones m_primaryAbility;
#endif

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "\n-- On Hit Data for dash --\n" + m_dashOnHitData.GetInEditorDesc();
	}

	public override List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> relevantTargetSelectCompForEditor = base.GetRelevantTargetSelectCompForEditor();
		if (m_targetSelectForDash != null)
		{
			relevantTargetSelectCompForEditor.Add(m_targetSelectForDash);
		}
		return relevantTargetSelectCompForEditor;
	}

	public void ResetTargetersForStanceChange()
	{
		SetupTargetersAndCachedVars();
	}

	protected override void SetupTargetersAndCachedVars()
	{
#if SERVER
		// custom
		m_passive = GetPassiveOfType(typeof(Passive_Dino)) as Passive_Dino;
		AbilityData abilityData = GetComponent<AbilityData>();
		if (abilityData != null)
		{
			m_primaryAbility = abilityData.GetAbilityOfType<DinoLayerCones>();
		}
#endif
		
		m_syncComp = GetComponent<Dino_SyncComponent>();
		SetCachedFields();
		base.SetupTargetersAndCachedVars();
		if (Targeter is AbilityUtil_Targeter_LaserChargeReverseCones)
		{
			AbilityUtil_Targeter_LaserChargeReverseCones targeter = Targeter as AbilityUtil_Targeter_LaserChargeReverseCones;
			GenericAbility_TargetSelectBase targetSelectComp = GetTargetSelectComp();
			targeter.SetAffectedGroups(targetSelectComp.IncludeEnemies(), targetSelectComp.IncludeAllies(), true);
			targeter.m_includeCasterDelegate = TargeterIncludeCaster;
		}
	}

	private bool TargeterIncludeCaster(ActorData caster, List<ActorData> actorsSoFar)
	{
		return GetShieldPerEnemyHit() > 0 && actorsSoFar.Count > 0;
	}

	public override void PostProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargeterIndex,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		ContextVars abilityContext,
		ActorData caster,
		TargetingNumberUpdateScratch results)
	{
		if (IsInReadyStance() && GetShieldPerEnemyHit() > 0)
		{
			SetShieldPerEnemyHitTargetingNumbers(targetActor, caster, GetShieldPerEnemyHit(), actorHitContext, results);
		}
	}

	public static void SetShieldPerEnemyHitTargetingNumbers(
		ActorData targetActor,
		ActorData caster,
		int shieldPerEnemyHit,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		TargetingNumberUpdateScratch results)
	{
		if (shieldPerEnemyHit <= 0 || targetActor != caster)
		{
			return;
		}
		int enemiesHit = 0;
		foreach (KeyValuePair<ActorData, ActorHitContext> hitActor in actorHitContext)
		{
			if (hitActor.Key.GetTeam() != caster.GetTeam()
			    && hitActor.Value.m_inRangeForTargeter)
			{
				enemiesHit++;
			}
		}
		if (enemiesHit <= 0)
		{
			return;
		}
		int absorb = shieldPerEnemyHit * enemiesHit;
		if (results.m_absorb >= 0)
		{
			results.m_absorb += absorb;
		}
		else
		{
			results.m_absorb = absorb;
		}
	}

	public override bool ShouldUpdateDrawnTargetersOnQueueChange()
	{
		return FullyChargeUpLayerCone();
	}

	private void SetCachedFields()
	{
		m_cachedShieldEffect = m_abilityMod != null
			? m_abilityMod.m_shieldEffectMod.GetModifiedValue(m_shieldEffect)
			: m_shieldEffect;
		m_cachedDashOnHitData = m_abilityMod != null
			? m_abilityMod.m_dashOnHitDataMod.GetModdedOnHitData(m_dashOnHitData)
			: m_dashOnHitData;
	}

	public int GetShieldPerEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldPerEnemyHitMod.GetModifiedValue(m_shieldPerEnemyHit)
			: m_shieldPerEnemyHit;
	}

	public int GetShieldDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldDurationMod.GetModifiedValue(m_shieldDuration)
			: m_shieldDuration;
	}

	public StandardEffectInfo GetShieldEffect()
	{
		return m_cachedShieldEffect ?? m_shieldEffect;
	}

	public int GetHealIfNoDash()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healIfNoDashMod.GetModifiedValue(m_healIfNoDash)
			: m_healIfNoDash;
	}

	public int GetCdrIfNoDash()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfNoDashMod.GetModifiedValue(m_cdrIfNoDash)
			: m_cdrIfNoDash;
	}

	public int GetDelayedCooldown()
	{
		return m_abilityMod != null
			? m_abilityMod.m_delayedCooldownMod.GetModifiedValue(m_delayedCooldown)
			: m_delayedCooldown;
	}

	public bool FullyChargeUpLayerCone()
	{
		return m_abilityMod != null
			? m_abilityMod.m_fullyChargeUpLayerConeMod.GetModifiedValue(m_fullyChargeUpLayerCone)
			: m_fullyChargeUpLayerCone;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_dashOnHitData.AddTooltipTokens(tokens);
		AbilityMod.AddToken_EffectInfo(tokens, m_shieldEffect, "ShieldEffect", m_shieldEffect);
		AddTokenInt(tokens, "HealIfNoDash", string.Empty, m_healIfNoDash);
		AddTokenInt(tokens, "CdrIfNoDash", string.Empty, m_cdrIfNoDash);
		AddTokenInt(tokens, "DelayedCooldown", string.Empty, m_delayedCooldown);
	}

	public override bool UseCustomAbilityIconColor()
	{
		return m_syncComp != null && m_syncComp.m_dashOrShieldInReadyStance;
	}

	public override Color GetCustomAbilityIconColor(ActorData actor)
	{
		return UseCustomAbilityIconColor()
			? m_iconColorWhileActive
			: base.GetCustomAbilityIconColor(actor);
	}

	public override GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		return IsInReadyStance()
			? m_targetSelectForDash
			: base.GetTargetSelectComp();
	}

	public override AbilityPriority GetRunPriority()
	{
		return IsInReadyStance()
			? AbilityPriority.Evasion
			: base.GetRunPriority();
	}

	public override MovementAdjustment GetMovementAdjustment()
	{
		return IsInReadyStance()
			? m_dashMovementAdjust
			: base.GetMovementAdjustment();
	}

	public override TechPointInteraction[] GetBaseTechPointInteractions()
	{
		return IsInReadyStance()
			? m_dashTechPointInteractions
			: base.GetBaseTechPointInteractions();
	}

	public override OnHitAuthoredData GetOnHitAuthoredData()
	{
		return IsInReadyStance()
			? m_cachedDashOnHitData
			: base.GetOnHitAuthoredData();
	}

	public override bool IsFreeAction()
	{
		return !IsInReadyStance() && base.IsFreeAction();
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		return IsInReadyStance()
			? m_dashAnimIndex
			: base.GetActionAnimType();
	}

	public override int GetCooldownForUIDisplay()
	{
		return GetDelayedCooldown();
	}

	public bool IsInReadyStance()
	{
		return m_syncComp != null && m_syncComp.m_dashOrShieldInReadyStance;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_DinoDashOrShield;
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}

	protected override void SetTargetSelectModReference()
	{
		if (m_abilityMod != null)
		{
			m_targetSelectComp.SetTargetSelectMod(m_abilityMod.m_initialCastTargetSelectMod);
			m_targetSelectForDash.SetTargetSelectMod(m_abilityMod.m_dashTargetSelectMod);
		}
		else
		{
			m_targetSelectComp.ClearTargetSelectMod();
			m_targetSelectForDash.ClearTargetSelectMod();
		}
	}
	
#if SERVER
	// custom
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);

		if (m_syncComp != null)
		{
			m_syncComp.Networkm_dashOrShieldLastCastTurn = IsInReadyStance() ? -1 : GameFlowData.Get().CurrentTurn;
		}

		if (m_passive != null && IsInReadyStance())
		{
			m_passive.m_dashCooldownAdjust = 0;
		}
	}

	// custom
	protected override void ProcessGatheredHits(
		List<AbilityTarget> targets,
		ActorData caster,
		AbilityResults abilityResults,
		List<ActorHitResults> actorHitResults,
		List<PositionHitResults> positionHitResults,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		base.ProcessGatheredHits(targets, caster, abilityResults, actorHitResults, positionHitResults, nonActorTargetInfo);

		AbilityData.ActionType abilityActionType = caster.GetAbilityData().GetActionTypeOfAbility(this);
		ActorHitResults casterHitResults = actorHitResults
			.FirstOrDefault(ahr => ahr.m_hitParameters.Target == caster);
		if (casterHitResults == null)
		{
			casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetLoSCheckPos()));
			actorHitResults.Add(casterHitResults);
		}
		
		if (!IsInReadyStance())
		{
			casterHitResults.AddEffect(new DinoDashOrShieldEffect(
					AsEffectSource(),
					caster.GetCurrentBoardSquare(),
					caster,
					caster,
					GetShieldEffect(),
					GetHealIfNoDash(),
					m_noDashShieldAnimIndex,
					abilityActionType,
					GetDelayedCooldown() - GetCdrIfNoDash(),
					m_onTriggerSequencePrefab,
					m_passive));
		}
		else
		{
			int enemiesHit = actorHitResults
				.Count(ahr => ahr.m_hitParameters.Target.GetTeam() != caster.GetTeam());
			casterHitResults.AddEffect(CreateShieldEffect(
				this, caster, GetShieldPerEnemyHit() * enemiesHit, GetShieldDuration()));
			casterHitResults.AddMiscHitEvent(
				new MiscHitEventData_OverrideCooldown(abilityActionType, GetDelayedCooldown() + m_passive.m_dashCooldownAdjust));
		}

		if (m_passive != null
		    && m_primaryAbility != null
		    && !ServerActionBuffer.Get().HasStoredAbilityRequestOfType(caster, typeof(DinoLayerCones)))
		{
			casterHitResults.AddMiscHitEvent(
				new MiscHitEventData_UpdatePassive(
					m_passive, 
					new List<MiscHitEventPassiveUpdateParams> 
					{
						new Passive_Dino.SetPowerLevelParam(m_primaryAbility.GetLayerCount())
					}));
		}
	}
	
	// custom
	public override void OnEffectAbsorbedDamage(ActorData effectCaster, int damageAbsorbed)
	{
		effectCaster.GetFreelancerStats().AddToValueOfStat(
			FreelancerStats.DinoStats.DashOrShieldEffectiveShieldAndDamageEvaded,
			damageAbsorbed);
	}
	
	// custom
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(
				FreelancerStats.DinoStats.DashOrShieldEffectiveShieldAndDamageEvaded,
				results.FinalDamage);
		}
	}

	// custom
	public override void OnDodgedDamage(ActorData caster, int damageDodged)
	{
		caster.GetFreelancerStats().AddToValueOfStat(
			FreelancerStats.DinoStats.DashOrShieldEffectiveShieldAndDamageEvaded,
			damageDodged);
	}
#endif
}
