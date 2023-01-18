// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ExoPunch : Ability
{
	[Header("-- Targeting")]
	public float m_coneWidthAngle = 180f;
	public float m_coneBackwardOffset;
	public float m_coneLength = 2.5f;
	public bool m_penetrateLineOfSight;
	public int m_maxTargets = 5;
	[Header("-- Knockback")]
	public float m_knockbackDistance;
	public KnockbackType m_knockbackType;
	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmount = 20;
	public StandardEffectInfo m_targetHitEffect;
	[Header("-- Nearby Hit Bonus")]
	public float m_nearDistThreshold;
	public int m_nearEnemyExtraDamage;
	public StandardEffectInfo m_nearEnemyExtraEffect;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ExoPunch m_abilityMod;
	private StandardEffectInfo m_cachedTargetHitEffect;
	private StandardEffectInfo m_cachedNearEnemyExtraEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Exollent Hit";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		AbilityUtil_Targeter_StretchCone targeter = new AbilityUtil_Targeter_StretchCone(
			this,
			GetConeLength(),
			GetConeLength(),
			GetConeWidthAngle(),
			GetConeWidthAngle(),
			AreaEffectUtils.StretchConeStyle.Linear,
			GetConeBackwardOffset(),
			PenetrateLineOfSight());
		targeter.InitKnockbackData(GetKnockbackDistance(), GetKnockbackType(), 0f, KnockbackType.AwayFromSource);
		Targeter = targeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeLength();
	}

	private void SetCachedFields()
	{
		m_cachedTargetHitEffect = m_abilityMod != null
			? m_abilityMod.m_targetHitEffectMod.GetModifiedValue(m_targetHitEffect)
			: m_targetHitEffect;
		m_cachedNearEnemyExtraEffect = m_abilityMod != null
			? m_abilityMod.m_nearEnemyExtraEffectMod.GetModifiedValue(m_nearEnemyExtraEffect)
			: m_nearEnemyExtraEffect;
	}

	public float GetConeWidthAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset)
			: m_coneBackwardOffset;
	}

	public float GetConeLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength)
			: m_coneLength;
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

	public float GetKnockbackDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance)
			: m_knockbackDistance;
	}

	public KnockbackType GetKnockbackType()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType)
			: m_knockbackType;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		return m_cachedTargetHitEffect ?? m_targetHitEffect;
	}

	public float GetNearDistThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_nearDistThresholdMod.GetModifiedValue(m_nearDistThreshold)
			: m_nearDistThreshold;
	}

	public int GetNearEnemyExtraDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_nearEnemyExtraDamageMod.GetModifiedValue(m_nearEnemyExtraDamage)
			: m_nearEnemyExtraDamage;
	}

	public StandardEffectInfo GetNearEnemyExtraEffect()
	{
		return m_cachedNearEnemyExtraEffect ?? m_nearEnemyExtraEffect;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetDamageAmount())
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		float nearDistThreshold = GetNearDistThreshold() * Board.Get().squareSize;
		if (nearDistThreshold > 0f)
		{
			Vector3 vector = targetActor.GetFreePos() - ActorData.GetFreePos();
			vector.y = 0f;
			if (vector.magnitude < nearDistThreshold)
			{
				List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
				if (tooltipSubjectTypes != null
				    && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy)
				    && ActorData != null)
				{
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					int damage = GetDamageAmount();
					if (GetNearEnemyExtraDamage() > 0)
					{
						damage += GetNearEnemyExtraDamage();
					}
					dictionary[AbilityTooltipSymbol.Damage] = damage;
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Damage", "damage in the cone", GetDamageAmount());
		AddTokenInt(tokens, "Knockback_Distance", "range of knockback for hit enemies", Mathf.RoundToInt(GetKnockbackDistance()));
		AddTokenInt(tokens, "Cone_Angle", "angle of the damage cone", (int)GetConeWidthAngle());
		AddTokenInt(tokens, "Cone_Length", "range of the damage cone", Mathf.RoundToInt(GetConeLength()));
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ExoPunch))
		{
			m_abilityMod = abilityMod as AbilityMod_ExoPunch;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// added in rogues
	private List<ActorData> GetHitTargets(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return AreaEffectUtils.GetActorsInCone(
			caster.GetLoSCheckPos(),
			VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection),
			GetConeWidthAngle(),
			GetConeLength(),
			GetConeBackwardOffset(),
			PenetrateLineOfSight(),
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			caster.GetCurrentBoardSquare(),
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitTargets = GetHitTargets(targets, caster, nonActorTargetInfo);
		int damageAmount = GetDamageAmount();
		float knockbackDistance = GetKnockbackDistance();
		float num = GetNearDistThreshold() * Board.Get().squareSize;
		foreach (ActorData actorData in hitTargets)
		{
			bool flag = false;
			int num2 = 0;
			if (num > 0f)
			{
				Vector3 vector = actorData.GetFreePos() - caster.GetFreePos();
				vector.y = 0f;
				if (vector.magnitude < num)
				{
					num2 = GetNearEnemyExtraDamage();
					flag = true;
				}
			}
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			actorHitResults.SetBaseDamage(damageAmount + num2);
			actorHitResults.AddStandardEffectInfo(GetTargetHitEffect());
			if (flag)
			{
				actorHitResults.AddStandardEffectInfo(GetNearEnemyExtraEffect());
			}
			if (knockbackDistance != 0f)
			{
				KnockbackHitData knockbackData = new KnockbackHitData(
					actorData,
					caster,
					GetKnockbackType(),
					targets[0].AimDirection,
					caster.GetFreePos(),
					knockbackDistance);
				actorHitResults.AddKnockbackData(knockbackData);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif
}
