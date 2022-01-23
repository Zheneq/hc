using System.Collections.Generic;
using UnityEngine;

public class ClericBasicAttack : Ability
{
	[Header("-- Targeting")]
	public bool m_penetrateLineOfSight;

	public float m_coneAngle = 180f;

	public float m_coneLengthInner = 1.5f;

	public float m_coneLength = 2.5f;

	public float m_coneBackwardOffset;

	public int m_maxTargets = 1;

	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmountInner = 28;

	public int m_damageAmount = 20;

	public StandardEffectInfo m_targetHitEffectInner;

	public StandardEffectInfo m_targetHitEffect;

	public AbilityModCooldownReduction m_cooldownReduction;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private Cleric_SyncComponent m_syncComp;

	private AbilityMod_ClericBasicAttack m_abilityMod;

	private StandardEffectInfo m_cachedTargetHitEffectInner;

	private StandardEffectInfo m_cachedTargetHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Cleric Bash";
		}
		m_syncComp = GetComponent<Cleric_SyncComponent>();
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		float coneAngle = GetConeAngle();
		List<AbilityUtil_Targeter_MultipleCones.ConeDimensions> list = new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>();
		list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneAngle, GetConeLengthInner()));
		list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneAngle, GetConeLength()));
		base.Targeter = new AbilityUtil_Targeter_MultipleCones(this, list, m_coneBackwardOffset, PenetrateLineOfSight(), true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ClericBasicAttack))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ClericBasicAttack);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedTargetHitEffectInner;
		if ((bool)m_abilityMod)
		{
			cachedTargetHitEffectInner = m_abilityMod.m_targetHitEffectInnerMod.GetModifiedValue(m_targetHitEffectInner);
		}
		else
		{
			cachedTargetHitEffectInner = m_targetHitEffectInner;
		}
		m_cachedTargetHitEffectInner = cachedTargetHitEffectInner;
		StandardEffectInfo cachedTargetHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedTargetHitEffect = m_abilityMod.m_targetHitEffectMod.GetModifiedValue(m_targetHitEffect);
		}
		else
		{
			cachedTargetHitEffect = m_targetHitEffect;
		}
		m_cachedTargetHitEffect = cachedTargetHitEffect;
	}

	public bool PenetrateLineOfSight()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight);
		}
		else
		{
			result = m_penetrateLineOfSight;
		}
		return result;
	}

	public float GetConeAngle()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneAngleMod.GetModifiedValue(m_coneAngle);
		}
		else
		{
			result = m_coneAngle;
		}
		return result;
	}

	public float GetConeLengthInner()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneLengthInnerMod.GetModifiedValue(m_coneLengthInner);
		}
		else
		{
			result = m_coneLengthInner;
		}
		return result;
	}

	public float GetConeLength()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength);
		}
		else
		{
			result = m_coneLength;
		}
		return result;
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset);
		}
		else
		{
			result = m_coneBackwardOffset;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			result = m_maxTargets;
		}
		return result;
	}

	public int GetDamageAmountInner()
	{
		return (!m_abilityMod) ? m_damageAmountInner : m_abilityMod.m_damageAmountInnerMod.GetModifiedValue(m_damageAmountInner);
	}

	public int GetDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			result = m_damageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetTargetHitEffectInner()
	{
		return (m_cachedTargetHitEffectInner == null) ? m_targetHitEffectInner : m_cachedTargetHitEffectInner;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		return (m_cachedTargetHitEffect == null) ? m_targetHitEffect : m_cachedTargetHitEffect;
	}

	public int GetExtraDamageToTargetsWhoEvaded()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageToTargetsWhoEvaded.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public AbilityModCooldownReduction GetCooldownReduction()
	{
		if ((bool)m_abilityMod)
		{
			if (m_abilityMod.m_useCooldownReductionOverride)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return m_abilityMod.m_cooldownReductionOverrideMod;
					}
				}
			}
		}
		return m_cooldownReduction;
	}

	public int GetHitsToIgnoreForCooldownReductionMultiplier()
	{
		if ((bool)m_abilityMod)
		{
			if (m_abilityMod.m_useCooldownReductionOverride)
			{
				return m_abilityMod.m_hitsToIgnoreForCooldownReductionMultiplier.GetModifiedValue(0);
			}
		}
		return 0;
	}

	public int GetExtraTechPointGainInAreaBuff()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraTechPointGainInAreaBuff.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "DamageAmountInner", string.Empty, m_damageAmountInner);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetHitEffectInner, "TargetHitEffectInner", m_targetHitEffectInner);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetHitEffect, "TargetHitEffect", m_targetHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Near, GetDamageAmountInner()));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, GetDamageAmount()));
		return list;
	}

	private bool InsideNearRadius(ActorData targetActor, Vector3 damageOrigin)
	{
		float num = GetConeLengthInner() * Board.Get().squareSize;
		Vector3 vector = targetActor.GetFreePos() - damageOrigin;
		vector.y = 0f;
		float num2 = vector.magnitude;
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			num2 -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
		}
		return num2 <= num;
	}

	public override bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.Near)
		{
			if (subjectType != AbilityTooltipSubject.Far)
			{
				return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
			}
		}
		if (InsideNearRadius(targetActor, damageOrigin))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return subjectType == AbilityTooltipSubject.Near;
				}
			}
		}
		return subjectType == AbilityTooltipSubject.Far;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ClericAreaBuff)))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return GetExtraTechPointGainInAreaBuff() * base.Targeter.GetNumActorsInRange();
				}
			}
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}
}
