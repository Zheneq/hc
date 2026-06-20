// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScampSuitToggle : Ability
{
	[Separator("Whether shield down mode is free action")]
	public bool m_shieldDownModeFreeAction;
	[Separator("Cooldowns")]
	public int m_cooldownCreateSuit = 2;
	public int m_cooldownRefillShield = 2;
	[Header("-- Cooldown override for when suit is destroyed")]
	public int m_cooldownOverrideOnSuitDestroy = 2;
	[Separator("Energy to Shield (shield = energy x multiplier)")]
	public float m_energyToShieldMult = 1f;
	[Separator("Clear Energy Orbs on cast")]
	public bool m_clearEnergyOrbsOnCast = true;
	[Separator("Extra Orbs to spawn on suit lost")]
	public int m_extraOrbsToSpawnOnSuitLost;
	[Separator("Passive Energy Regen")]
	public int m_passiveEnergyRegen;
	[Separator("Effect to apply when suit is gained or lost (applied on start of turn)")]
	public bool m_considerRespawnForSuitGainEffect; // TODO SCAMP unused, always false
	public StandardEffectInfo m_effectForSuitGained;
	public StandardEffectInfo m_effectForSuitLost;
	[Separator("Sequences")]
	public GameObject m_castSequencePrefab;
	[Header("-- for setting anim param, only spawned when adding new suit")]
	public GameObject m_addSuitAnimSeqPrefab;

	private AbilityMod_ScampSuitToggle m_abilityMod;
	private Scamp_SyncComponent m_syncComp;
	private Passive_Scamp m_passive;
	private StandardEffectInfo m_cachedEffectForSuitGained;
	private StandardEffectInfo m_cachedEffectForSuitLost;
	
#if SERVER
	// custom
	private AbilityData.ActionType m_actionType;
	private ScampDashAndAoe m_dashAbility;
	private AbilityData.ActionType m_dashAbilityActionType;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "ScampSuitToggle";
		}
		Setup();
	}

	private void Setup()
	{
		m_passive = GetPassiveOfType<Passive_Scamp>();
		SetCachedFields();
		m_syncComp = GetComponent<Scamp_SyncComponent>();
#if SERVER
		// custom
		m_actionType = GetActionTypeOfAbility(this);
		m_dashAbility = GetAbilityOfType<ScampDashAndAoe>();
		m_dashAbilityActionType = GetActionTypeOfAbility(m_dashAbility);
#endif
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos,
			false,
			false,
			AbilityUtil_Targeter.AffectsActor.Always);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "CooldownCreateSuit", string.Empty, m_cooldownCreateSuit);
		AddTokenInt(tokens, "CooldownRefillShield", string.Empty, m_cooldownRefillShield);
		AddTokenInt(tokens, "CooldownOverrideOnSuitDestroy", string.Empty, m_cooldownOverrideOnSuitDestroy);
		AddTokenInt(tokens, "ExtraOrbsToSpawnOnSuitLost", string.Empty, m_extraOrbsToSpawnOnSuitLost);
		AddTokenInt(tokens, "PassiveEnergyRegen", string.Empty, m_passiveEnergyRegen);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectForSuitGained, "EffectForSuitGained", m_effectForSuitGained);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectForSuitLost, "EffectForSuitLost", m_effectForSuitLost);
	}

	private void SetCachedFields()
	{
		m_cachedEffectForSuitGained = m_abilityMod != null
			? m_abilityMod.m_effectForSuitGainedMod.GetModifiedValue(m_effectForSuitGained)
			: m_effectForSuitGained;
		m_cachedEffectForSuitLost = m_abilityMod != null
			? m_abilityMod.m_effectForSuitLostMod.GetModifiedValue(m_effectForSuitLost)
			: m_effectForSuitLost;
	}

	public bool ShieldDownModeFreeAction()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldDownModeFreeActionMod.GetModifiedValue(m_shieldDownModeFreeAction)
			: m_shieldDownModeFreeAction;
	}

	public int GetCooldownCreateSuit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownCreateSuitMod.GetModifiedValue(m_cooldownCreateSuit)
			: m_cooldownCreateSuit;
	}

	public int GetCooldownRefillShield()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownRefillShieldMod.GetModifiedValue(m_cooldownRefillShield)
			: m_cooldownRefillShield;
	}

	public int GetCooldownOverrideOnSuitDestroy()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownOverrideOnSuitDestroyMod.GetModifiedValue(m_cooldownOverrideOnSuitDestroy)
			: m_cooldownOverrideOnSuitDestroy;
	}

	public float GetEnergyToShieldMult()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyToShieldMultMod.GetModifiedValue(m_energyToShieldMult)
			: m_energyToShieldMult;
	}

	public bool ClearEnergyOrbsOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_clearEnergyOrbsOnCastMod.GetModifiedValue(m_clearEnergyOrbsOnCast)
			: m_clearEnergyOrbsOnCast;
	}

	public int GetExtraOrbsToSpawnOnSuitLost()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraOrbsToSpawnOnSuitLostMod.GetModifiedValue(m_extraOrbsToSpawnOnSuitLost)
			: m_extraOrbsToSpawnOnSuitLost;
	}

	public int GetPassiveEnergyRegen()
	{
		return m_abilityMod != null
			? m_abilityMod.m_passiveEnergyRegenMod.GetModifiedValue(m_passiveEnergyRegen)
			: m_passiveEnergyRegen;
	}
	
	public bool ConsiderRespawnForSuitGainEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_considerRespawnForSuitGainEffectMod.GetModifiedValue(m_considerRespawnForSuitGainEffect)
			: m_considerRespawnForSuitGainEffect;
	}

	public StandardEffectInfo GetEffectForSuitGained()
	{
		return m_cachedEffectForSuitGained ?? m_effectForSuitGained;
	}

	public StandardEffectInfo GetEffectForSuitLost()
	{
		return m_cachedEffectForSuitLost ?? m_effectForSuitLost;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		int techPoints = ActorData.TechPoints + ActorData.ReservedTechPoints;
		int absorb = Mathf.RoundToInt(techPoints * GetEnergyToShieldMult());
		results.m_absorb = Mathf.Clamp(absorb, 1, m_passive.GetMaxSuitShield());
		return true;
	}

	public override bool IsFreeAction()
	{
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart
			? base.IsFreeAction()
			: ShieldDownModeFreeAction();
	}

	public override int GetModdedCost()
	{
		int cost = 0;
		if (ActorData != null)
		{
			cost = ActorData.TechPoints + ActorData.ReservedTechPoints;
		}
		return Mathf.Max(1, cost);
	}

	public override int GetTechPointRegenContribution()
	{
		int passiveEnergyRegen = GetPassiveEnergyRegen();
		return passiveEnergyRegen > 0
			? passiveEnergyRegen
			: 0;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return m_syncComp != null
			? caster.TechPoints + caster.ReservedTechPoints > 0
			: base.CustomCanCastValidation(caster);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScampSuitToggle))
		{
			m_abilityMod = abilityMod as AbilityMod_ScampSuitToggle;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

#if SERVER
	// custom
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		var list = new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				caster.GetFreePos(),
				caster.AsArray(),
				caster,
				additionalData.m_sequenceSource)
		};

		if (!m_syncComp.m_suitWasActiveOnTurnStart)
		{
			list.Add(
				new ServerClientUtils.SequenceStartData(
					m_addSuitAnimSeqPrefab,
					caster.GetFreePos(),
					null,
					caster,
					additionalData.m_sequenceSource));
		}

		if (ClearEnergyOrbsOnCast())
		{
			foreach (ScampOrbEffect orbEffect in m_passive.GetOrbs())
			{
				list.Add(orbEffect.GetEffectEndSeqData(additionalData.m_sequenceSource));
			}
		}

		return list;
	}

	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);

		m_passive.OnSuitToggle();
	}

	// custom
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitParameters actorHitParameters = new ActorHitParameters(caster, caster.GetFreePos());
		ActorHitResults actorHitResults = new ActorHitResults(actorHitParameters);

		int absorb = Mathf.RoundToInt((ActorData.TechPoints + ActorData.ReservedTechPoints) * GetEnergyToShieldMult()); // all TP were reserved for cast but not for bot aiming
		absorb = Mathf.Clamp(absorb, 1, m_passive.GetMaxSuitShield());

		int permanentAbsorb = Math.Min(absorb, m_passive.GetMaxSuitShield() - (int)m_syncComp.m_suitShieldingOnTurnStart);
		int temporaryAbsorb = Math.Max(absorb - permanentAbsorb, 0);
		
		StandardActorEffectData effectData = m_passive.m_shieldEffectData.GetShallowCopy();
		effectData.m_absorbAmount = permanentAbsorb;
		actorHitResults.AddStandardEffectInfo(
			new StandardEffectInfo
			{
				m_applyEffect = true,
				m_effectData = effectData
			});

		if (temporaryAbsorb > 0)
		{
			actorHitResults.AddEffect(
				GenericAbility_Container.CreateShieldEffect(
					this,
					caster,
					temporaryAbsorb,
					1));
		}

		if (!m_syncComp.m_suitWasActiveOnTurnStart)
		{
			actorHitResults.AddMiscHitEvent(
				new MiscHitEventData_OverrideCooldown(
					m_actionType,
					GetCooldownCreateSuit()));
			actorHitResults.AddMiscHitEvent(
				new MiscHitEventData_OverrideCooldown(
					m_dashAbilityActionType,
					m_passive.FuryBallDashCooldownRemaining - 1));
			if (m_dashAbility.GetCdrOnSuitApply() > 0)
			{
				actorHitResults.AddMiscHitEvent(
					new MiscHitEventData_AddToCasterCooldown(
						m_dashAbilityActionType,
						-m_dashAbility.GetCdrOnSuitApply()));
			}
		}
		else
		{
			actorHitResults.AddMiscHitEvent(
				new MiscHitEventData_OverrideCooldown(
					m_actionType,
					GetCooldownRefillShield()));
		}

		if (ClearEnergyOrbsOnCast())
		{
			foreach (ScampOrbEffect orbEffect in m_passive.GetOrbs())
			{
				actorHitResults.AddEffectForRemoval(orbEffect);
			}
		}

		actorHitResults.AddMiscHitEvent(
			new MiscHitEventData_UpdateFreelancerStat(
				(int)FreelancerStats.ScampStats.UltShieldGenerated,
				absorb,
				caster));
		
		abilityResults.StoreActorHit(actorHitResults);
	}
#endif
}
