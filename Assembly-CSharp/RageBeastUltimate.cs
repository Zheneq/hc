using System;
using System.Collections.Generic;
using UnityEngine;

public class RageBeastUltimate : Ability
{
	[Space(10f)]
	public bool m_resetCooldowns = true;

	public AbilityAreaShape m_castExplosionShape = AbilityAreaShape.Five_x_Five_NoCorners;

	public AbilityAreaShape m_walkingSpillShape;

	[Separator("Plasma", true)]
	public int m_plasmaDamage = 5;

	public int m_plasmaDuration = 2;

	public int m_spillingStateDuration = 2;

	public bool m_penetrateLineOfSight;

	[Separator("Direct vs Indirect Damage", true)]
	public bool m_isDirectDamageOnCast;

	[Separator("Self Hit on Cast", true)]
	public int m_selfHealOnCast;

	public StandardEffectInfo m_extraEffectOnSelf;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	public GameObject m_plasmaSequencePrefab;

	public GameObject m_hitByPlasmaSequencePrefab;

	public GameObject m_buffSequencePrefab;

	private AbilityMod_RageBeastUltimate m_abilityMod;

	private StandardEffectInfo m_cachedExtraEffectOnSelf;

	private void Start()
	{
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		bool flag = base.HasSelfEffectFromBaseMod();
		AbilityUtil_Targeter.AffectsActor affectsCaster = (!flag) ? AbilityUtil_Targeter.AffectsActor.Never : AbilityUtil_Targeter.AffectsActor.Always;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, this.m_castExplosionShape, this.m_penetrateLineOfSight, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedExtraEffectOnSelf;
		if (this.m_abilityMod)
		{
			cachedExtraEffectOnSelf = this.m_abilityMod.m_extraEffectOnSelfMod.GetModifiedValue(this.m_extraEffectOnSelf);
		}
		else
		{
			cachedExtraEffectOnSelf = this.m_extraEffectOnSelf;
		}
		this.m_cachedExtraEffectOnSelf = cachedExtraEffectOnSelf;
	}

	public int GetSelfHealOnCast()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_selfHealOnCastMod.GetModifiedValue(this.m_selfHealOnCast);
		}
		else
		{
			result = this.m_selfHealOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEffectOnSelf()
	{
		StandardEffectInfo result;
		if (this.m_cachedExtraEffectOnSelf != null)
		{
			result = this.m_cachedExtraEffectOnSelf;
		}
		else
		{
			result = this.m_extraEffectOnSelf;
		}
		return result;
	}

	private int GetPlasmaDamage()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_plasmaDamage;
		}
		else
		{
			result = this.m_abilityMod.m_plasmaDamageMod.GetModifiedValue(this.m_plasmaDamage);
		}
		return result;
	}

	private int GetPlasmaDuration()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_plasmaDurationMod.GetModifiedValue(this.m_plasmaDuration) : this.m_plasmaDuration;
	}

	private int GetPlasmaHealing()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_plasmaHealingMod.GetModifiedValue(0);
		}
		return result;
	}

	private int GetPlasmaTechPointGain()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_plasmaTechPointGainMod.GetModifiedValue(0) : 0;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetPlasmaDamage());
		base.AppendTooltipNumbersFromBaseModEffects(ref result, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetSelfHealOnCast());
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastUltimate abilityMod_RageBeastUltimate = modAsBase as AbilityMod_RageBeastUltimate;
		string name = "PlasmaDamage";
		string empty = string.Empty;
		int val;
		if (abilityMod_RageBeastUltimate)
		{
			val = abilityMod_RageBeastUltimate.m_plasmaDamageMod.GetModifiedValue(this.m_plasmaDamage);
		}
		else
		{
			val = this.m_plasmaDamage;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "PlasmaDuration";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_RageBeastUltimate)
		{
			val2 = abilityMod_RageBeastUltimate.m_plasmaDurationMod.GetModifiedValue(this.m_plasmaDuration);
		}
		else
		{
			val2 = this.m_plasmaDuration;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		base.AddTokenInt(tokens, "SelfHealOnCast", string.Empty, this.m_selfHealOnCast, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_RageBeastUltimate)
		{
			effectInfo = abilityMod_RageBeastUltimate.m_extraEffectOnSelfMod.GetModifiedValue(this.m_extraEffectOnSelf);
		}
		else
		{
			effectInfo = this.m_extraEffectOnSelf;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "ExtraEffectOnSelf", this.m_extraEffectOnSelf, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RageBeastUltimate))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_RageBeastUltimate);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
