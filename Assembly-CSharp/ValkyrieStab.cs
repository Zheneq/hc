using System.Collections.Generic;
using UnityEngine;

public class ValkyrieStab : Ability
{
	[Header("-- Targeting")]
	public float m_coneWidthMinAngle = 10f;
	public float m_coneWidthMaxAngle = 70f;
	public float m_coneBackwardOffset;
	public float m_coneMinLength = 2.5f;
	public float m_coneMaxLength = 5f;
	public AreaEffectUtils.StretchConeStyle m_coneStretchStyle;
	public bool m_penetrateLineOfSight;
	public int m_maxTargets = 5;
	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmount = 20;
	public int m_lessDamagePerTarget = 3;
	public StandardEffectInfo m_targetHitEffect;
	[Header("-- Sequences")]
	public GameObject m_centerProjectileSequencePrefab;
	public GameObject m_sideProjectileSequencePrefab;

	private Valkyrie_SyncComponent m_syncComp;
	private AbilityMod_ValkyrieStab m_abilityMod;
	private StandardEffectInfo m_cachedTargetHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Spear Poke";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_syncComp = GetComponent<Valkyrie_SyncComponent>();
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_ReverseStretchCone(
			this,
			GetConeMinLength(),
			GetConeMaxLength(),
			GetConeWidthMinAngle(),
			GetConeWidthMaxAngle(),
			m_coneStretchStyle,
			GetConeBackwardOffset(),
			PenetrateLineOfSight());
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeMaxLength() + GetConeBackwardOffset();
	}

	private void SetCachedFields()
	{
		m_cachedTargetHitEffect = m_abilityMod != null
			? m_abilityMod.m_targetHitEffectMod.GetModifiedValue(m_targetHitEffect)
			: m_targetHitEffect;
	}

	public float GetConeWidthMinAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthMinAngleMod.GetModifiedValue(m_coneWidthMinAngle)
			: m_coneWidthMinAngle;
	}

	public float GetConeWidthMaxAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthMaxAngleMod.GetModifiedValue(m_coneWidthMaxAngle)
			: m_coneWidthMaxAngle;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset)
			: m_coneBackwardOffset;
	}

	public float GetConeMinLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneMinLengthMod.GetModifiedValue(m_coneMinLength)
			: m_coneMinLength;
	}

	public float GetConeMaxLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneMaxLengthMod.GetModifiedValue(m_coneMaxLength)
			: m_coneMaxLength;
	}

	public bool PenetrateLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight)
			: m_penetrateLineOfSight;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetLessDamagePerTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lessDamagePerTargetMod.GetModifiedValue(m_lessDamagePerTarget)
			: m_lessDamagePerTarget;
	}

	public int GetExtraDamageOnSpearTip()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageOnSpearTip.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraDamageFirstTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageFirstTarget.GetModifiedValue(0)
			: 0;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		return m_cachedTargetHitEffect ?? m_targetHitEffect;
	}

	public int GetExtraAbsorbNextShieldBlockPerHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_perHitExtraAbsorbNextShieldBlock.GetModifiedValue(0)
			: 0;
	}

	public int GetMaxExtraAbsorbNextShieldBlock()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxExtraAbsorbNextShieldBlock.GetModifiedValue(0)
			: 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyrieStab))
		{
			m_abilityMod = abilityMod as AbilityMod_ValkyrieStab;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetDamageAmount()));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = Targeter.GetActorsInRange();
		List<ActorData> hitActors = new List<ActorData>();
		int extraDamage = 0;
		foreach (AbilityUtil_Targeter.ActorTarget target in actorsInRange)
		{
			hitActors.Add(target.m_actor);
			if (target.m_actor == targetActor && target.m_subjectTypes.Contains(AbilityTooltipSubject.Far))
			{
				extraDamage = GetExtraDamageOnSpearTip();
			}
		}
		int damageAmount = GetDamageAmount();
		bool reducedDamage = true;
		damageAmount += GetExtraDamageFirstTarget();
		foreach (ActorData item in hitActors)
		{
			if (item == targetActor)
			{
				dictionary[AbilityTooltipSymbol.Damage] = damageAmount + extraDamage;
				break;
			}
			if (m_syncComp == null || !m_syncComp.m_skipDamageReductionForNextStab)
			{
				damageAmount = Mathf.Max(0, damageAmount - GetLessDamagePerTarget());
			}
			if (reducedDamage)
			{
				reducedDamage = false;
				damageAmount -= GetExtraDamageFirstTarget();
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Damage", "damage in the cone", GetDamageAmount());
		AddTokenInt(tokens, "LessDamagePerTarget", string.Empty, m_lessDamagePerTarget);
		AddTokenInt(tokens, "Cone_MinAngle", "smallest angle of the damage cone", (int)GetConeWidthMinAngle());
		AddTokenInt(tokens, "Cone_MaxAngle", "largest angle of the damage cone", (int)GetConeWidthMaxAngle());
		AddTokenInt(tokens, "Cone_MinLength", "shortest range of the damage cone", Mathf.RoundToInt(GetConeMinLength()));
		AddTokenInt(tokens, "Cone_MaxLength", "longest range of the damage cone", Mathf.RoundToInt(GetConeMaxLength()));
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetHitEffect, "TargetHitEffect", m_targetHitEffect);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = GetConeMinLength() * Board.Get().squareSize;
		max = GetConeMaxLength() * Board.Get().squareSize;
		return true;
	}
}
