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
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		AbilityUtil_Targeter.AffectsActor affectsCaster = HasSelfEffectFromBaseMod() ? AbilityUtil_Targeter.AffectsActor.Always : AbilityUtil_Targeter.AffectsActor.Never;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, m_castExplosionShape, m_penetrateLineOfSight, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, affectsCaster);
		base.Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedExtraEffectOnSelf;
		if ((bool)m_abilityMod)
		{
			cachedExtraEffectOnSelf = m_abilityMod.m_extraEffectOnSelfMod.GetModifiedValue(m_extraEffectOnSelf);
		}
		else
		{
			cachedExtraEffectOnSelf = m_extraEffectOnSelf;
		}
		m_cachedExtraEffectOnSelf = cachedExtraEffectOnSelf;
	}

	public int GetSelfHealOnCast()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfHealOnCastMod.GetModifiedValue(m_selfHealOnCast);
		}
		else
		{
			result = m_selfHealOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEffectOnSelf()
	{
		StandardEffectInfo result;
		if (m_cachedExtraEffectOnSelf != null)
		{
			result = m_cachedExtraEffectOnSelf;
		}
		else
		{
			result = m_extraEffectOnSelf;
		}
		return result;
	}

	private int GetPlasmaDamage()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_plasmaDamage;
		}
		else
		{
			result = m_abilityMod.m_plasmaDamageMod.GetModifiedValue(m_plasmaDamage);
		}
		return result;
	}

	private int GetPlasmaDuration()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_plasmaDurationMod.GetModifiedValue(m_plasmaDuration) : m_plasmaDuration;
	}

	private int GetPlasmaHealing()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_plasmaHealingMod.GetModifiedValue(0);
		}
		return result;
	}

	private int GetPlasmaTechPointGain()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_plasmaTechPointGainMod.GetModifiedValue(0) : 0;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetPlasmaDamage());
		AppendTooltipNumbersFromBaseModEffects(ref numbers, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetSelfHealOnCast());
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_RageBeastUltimate abilityMod_RageBeastUltimate = modAsBase as AbilityMod_RageBeastUltimate;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_RageBeastUltimate)
		{
			val = abilityMod_RageBeastUltimate.m_plasmaDamageMod.GetModifiedValue(m_plasmaDamage);
		}
		else
		{
			val = m_plasmaDamage;
		}
		AddTokenInt(tokens, "PlasmaDamage", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_RageBeastUltimate)
		{
			val2 = abilityMod_RageBeastUltimate.m_plasmaDurationMod.GetModifiedValue(m_plasmaDuration);
		}
		else
		{
			val2 = m_plasmaDuration;
		}
		AddTokenInt(tokens, "PlasmaDuration", empty2, val2);
		AddTokenInt(tokens, "SelfHealOnCast", string.Empty, m_selfHealOnCast);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_RageBeastUltimate)
		{
			effectInfo = abilityMod_RageBeastUltimate.m_extraEffectOnSelfMod.GetModifiedValue(m_extraEffectOnSelf);
		}
		else
		{
			effectInfo = m_extraEffectOnSelf;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "ExtraEffectOnSelf", m_extraEffectOnSelf);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_RageBeastUltimate))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_RageBeastUltimate);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
