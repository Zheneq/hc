// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScampDashAndAoe : GenericAbility_Container
{
	[Separator("Shield Cost on Cast")]
	public int m_shieldCost = 20;
	[Separator("Target Select Component for when shield is down")]
	public GenericAbility_TargetSelectBase m_shieldDownTargetSelect;
	[Separator("On Hit Data for when shield is down", "yellow")]
	public OnHitAuthoredData m_shieldDownOnHitData;
	[Separator("Cooldown for Shield Down mode. If <= 0, use same cooldown for both modes")]
	public int m_shieldDownCooldown = -1;
	[Header("-- Cdr on suit dash when going into suit form")]
	public int m_cdrOnSuitApply;
	[Header("-- if > 0 and health below threshold, shield down form of dash has no cooldowns")]
	public int m_shieldDownNoCooldownHealthThresh;
	[Separator("Extra Energy for dashing through or onto orb")]
	public int m_extraEnergyForDashOnOrb;
	[Separator("Whether we can move after dashing when out of suit")]
	public bool m_shieldDownAllowMoveAfterEvade = true;

	private AbilityMod_ScampDashAndAoe m_abilityMod;
	private Scamp_SyncComponent m_syncComp;
	private OnHitAuthoredData m_cachedShieldDownOnHitData;
#if SERVER
	private Passive_Scamp m_passive; // custom
#endif

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "\n-- On Hit Data when shields are down --\n" + m_shieldDownOnHitData.GetInEditorDesc();
	}

	public override List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> relevantTargetSelectCompForEditor = base.GetRelevantTargetSelectCompForEditor();
		if (m_shieldDownTargetSelect != null)
		{
			relevantTargetSelectCompForEditor.Add(m_shieldDownTargetSelect);
		}
		return relevantTargetSelectCompForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
#if SERVER
		m_passive = GetPassiveOfType<Passive_Scamp>(); // custom
#endif
		base.SetupTargetersAndCachedVars();
		m_cachedShieldDownOnHitData = m_abilityMod != null
			? m_abilityMod.m_shieldDownOnHitDataMod.GetModdedOnHitData(m_shieldDownOnHitData)
			: m_shieldDownOnHitData;
	}

	public void ResetTargetersForShielding(bool hasShield)
	{
		ClearTargeters();
		Targeters.AddRange(!hasShield && m_shieldDownTargetSelect != null
			? m_shieldDownTargetSelect.CreateTargeters(this)
			: m_targetSelectComp.CreateTargeters(this));
	}

	public int GetShieldCost()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldCostMod.GetModifiedValue(m_shieldCost)
			: m_shieldCost;
	}

	public int GetShieldDownCooldown()
	{
#if SERVER
		if (ActorData.HitPoints < GetShieldDownNoCooldownHealthThresh())
		{
			return 0;
		}
#endif
		return m_abilityMod != null
			? m_abilityMod.m_shieldDownCooldownMod.GetModifiedValue(m_shieldDownCooldown)
			: m_shieldDownCooldown;
	}

	public int GetCdrOnSuitApply()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrOnSuitApplyMod.GetModifiedValue(m_cdrOnSuitApply)
			: m_cdrOnSuitApply;
	}

	public int GetShieldDownNoCooldownHealthThresh()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldDownNoCooldownHealthThreshMod.GetModifiedValue(m_shieldDownNoCooldownHealthThresh)
			: m_shieldDownNoCooldownHealthThresh;
	}

	public int GetExtraEnergyForDashOnOrb()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraEnergyForDashOnOrbMod.GetModifiedValue(m_extraEnergyForDashOnOrb)
			: m_extraEnergyForDashOnOrb;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		m_shieldDownOnHitData.AddTooltipTokens(tokens);
		AddTokenInt(tokens, "ShieldCost", string.Empty, m_shieldCost);
		AddTokenInt(tokens, "ShieldDownCooldown", string.Empty, m_shieldDownCooldown);
		AddTokenInt(tokens, "CdrOnSuitApply", string.Empty, m_cdrOnSuitApply);
		AddTokenInt(tokens, "ShidleDownNoCooldownHealthThresh", string.Empty, m_shieldDownNoCooldownHealthThresh);
		AddTokenInt(tokens, "ExtraEnergyForDashOnOrb", string.Empty, m_extraEnergyForDashOnOrb);
	}

	public bool IsInSuit()
	{
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart;
	}

	public override GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		return IsInSuit()
			? base.GetTargetSelectComp()
			: m_shieldDownTargetSelect;
	}

	public override OnHitAuthoredData GetOnHitAuthoredData()
	{
		return IsInSuit()
			? base.GetOnHitAuthoredData()
			: m_cachedShieldDownOnHitData ?? m_shieldDownOnHitData;
	}

	public override int GetBaseCooldown()
	{
		return !IsInSuit() && GetShieldDownCooldown() >= 0
			? GetShieldDownCooldown()
			: base.GetBaseCooldown();
	}

	// TODO SCAMP unused
	public int CalcCurrentMaxCooldown(bool inSuit)
	{
		return GetShieldDownCooldown() < 0 || inSuit
			? m_cooldown
			: GetShieldDownCooldown();
	}

	public override MovementAdjustment GetMovementAdjustment()
	{
		return CanOverrideMoveStartSquare()
			? MovementAdjustment.ReducedMovement
			: base.GetMovementAdjustment();
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return !IsInSuit() && m_shieldDownAllowMoveAfterEvade;
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_ScampDashAndAoe;
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}

	protected override void SetTargetSelectModReference()
	{
		if (m_abilityMod != null)
		{
			m_targetSelectComp.SetTargetSelectMod(m_abilityMod.m_inSuitTargetSelectMod);
			m_shieldDownTargetSelect.SetTargetSelectMod(m_abilityMod.m_shieldDownTargetSelectMod);
		}
		else
		{
			m_targetSelectComp.ClearTargetSelectMod();
			m_shieldDownTargetSelect.ClearTargetSelectMod();
		}
	}
	
#if SERVER
	// custom
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);

		if (IsInSuit() && GetShieldCost() > 0)
		{
			int shieldCost = (int)Math.Min(GetShieldCost(), m_syncComp.m_suitShieldingOnTurnStart);
			List<StandardActorEffect> shieldEffects = m_passive.GetShieldEffects();
			if (!shieldEffects.IsNullOrEmpty())
			{
				foreach (StandardActorEffect shieldEffect in shieldEffects)
				{
					shieldEffect.ReduceAbsorptionRemaining(ref shieldCost);
					ServerEffectManager.Get().UpdateAbsorbPoints(shieldEffect);
					if (shieldCost <= 0)
					{
						break;
					}
				}
			}
		}
		m_passive.OnDash();
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

		if (GetExtraEnergyForDashOnOrb() > 0)
		{
			BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
			if (m_passive.GetOrbs().Any(e => e.TargetSquare == targetSquare))
			{
				GetOrAddHitResults(caster, actorHitResults)
					.AddTechPointGain(GetExtraEnergyForDashOnOrb());
			}
		}
	}
	
	// custom
	public override BoardSquare GetModifiedMoveStartSquare(ActorData caster, List<AbilityTarget> targets)
	{
		if (CanOverrideMoveStartSquare())
		{
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			if (square != null)
			{
				return square;
			}
		}
		return base.GetModifiedMoveStartSquare(caster, targets);
	}
	
	// custom
	public override void OnDodgedDamage(ActorData caster, int damageDodged)
	{
		caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.ScampStats.DashDamageDoneAndAvoided, damageDodged);
	}
	
	// custom
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(
				FreelancerStats.ScampStats.DashDamageDoneAndAvoided,
				results.FinalDamage);
		}
	}
#endif
}
