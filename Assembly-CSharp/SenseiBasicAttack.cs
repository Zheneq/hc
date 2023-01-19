using System.Collections.Generic;
using UnityEngine;

public class SenseiBasicAttack : Ability
{
	public enum LastUsedModeFlag
	{
		None,
		Cone,
		Laser
	}

	[Separator("Targeting Info", "cyan")]
	public float m_circleDistThreshold = 2f;
	[Header("  Targeting: For Circle")]
	public float m_circleRadius = 1.5f;
	[Header("  Targeting: For Laser")]
	public LaserTargetingInfo m_laserInfo;
	[Separator("On Hit Stuff", "cyan")]
	public int m_circleDamage = 15;
	public StandardEffectInfo m_circleEnemyHitEffect;
	[Space(10f)]
	public int m_laserDamage = 20;
	public StandardEffectInfo m_laserEnemyHitEffect;
	[Header("-- Extra Damage: alternate use")]
	public int m_extraDamageForAlternating;
	[Header("-- Extra Damage: far away target hits")]
	public int m_extraDamageForFarTarget;
	public float m_laserFarDistThresh;
	public float m_circleFarDistThresh;
	[Separator("Heal Per Target Hit")]
	public int m_healPerEnemyHit;
	[Separator("Cooldown Reduction")]
	public int m_cdrOnAbility;
	public AbilityData.ActionType m_cdrAbilityTarget = AbilityData.ActionType.ABILITY_1;
	public int m_cdrMinTriggerHitCount = 3;
	[Separator("Shielding on turn start per enemy hit")]
	public int m_absorbPerEnemyHitOnTurnStart;
	public int m_absorbShieldDuration = 1;
	public int m_absorbAmountIfTriggeredHitCount;
	[Header("-- Animation Indices --")]
	public int m_onCastLaserAnimIndex = 1;
	public int m_onCastCircleAnimIndex = 6;
	[Header("-- Sequences --")]
	public GameObject m_circleSequencePrefab;
	public GameObject m_laserSequencePrefab;

	private AbilityMod_SenseiBasicAttack m_abilityMod;
	private Sensei_SyncComponent m_syncComp;
	private LaserTargetingInfo m_cachedLaserInfo;
	private StandardEffectInfo m_cachedCircleEnemyHitEffect;
	private StandardEffectInfo m_cachedLaserEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Sensei Circle Or Laser";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Sensei_SyncComponent>();
		}
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_ConeOrLaser(
			this,
			new ConeTargetingInfo
			{
				m_affectsAllies = GetLaserInfo().affectsAllies,
				m_affectsEnemies = GetLaserInfo().affectsEnemies,
				m_affectsCaster = GetHealPerEnemyHit() > 0 || GetAbsorbAmountIfTriggeredHitCount() > 0,
				m_penetrateLos = false,
				m_radiusInSquares = GetCircleRadius(),
				m_widthAngleDeg = 360f
			},
			GetLaserInfo(),
			m_circleDistThreshold)
		{
			m_customShouldAddCasterDelegate = ShouldAddCasterForTargeter
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	private bool ShouldAddCasterForTargeter(ActorData caster, List<ActorData> actorsSoFar)
	{
		return GetHealPerEnemyHit() > 0 && actorsSoFar.Count > 0
		       || GetAbsorbAmountIfTriggeredHitCount() > 0 && actorsSoFar.Count >= GetCdrMinTriggerHitCount();
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_abilityMod != null
			? m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo)
			: m_laserInfo;
		m_cachedCircleEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_circleEnemyHitEffectMod.GetModifiedValue(m_circleEnemyHitEffect)
			: m_circleEnemyHitEffect;
		m_cachedLaserEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_laserEnemyHitEffectMod.GetModifiedValue(m_laserEnemyHitEffect)
			: m_laserEnemyHitEffect;
	}

	public float GetCircleDistThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_circleDistThresholdMod.GetModifiedValue(m_circleDistThreshold)
			: m_circleDistThreshold;
	}

	public float GetCircleRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_circleRadiusMod.GetModifiedValue(m_circleRadius)
			: m_circleRadius;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public int GetCircleDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_circleDamageMod.GetModifiedValue(m_circleDamage)
			: m_circleDamage;
	}

	public StandardEffectInfo GetCircleEnemyHitEffect()
	{
		return m_cachedCircleEnemyHitEffect ?? m_circleEnemyHitEffect;
	}

	public int GetLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamage)
			: m_laserDamage;
	}

	public StandardEffectInfo GetLaserEnemyHitEffect()
	{
		return m_cachedLaserEnemyHitEffect ?? m_laserEnemyHitEffect;
	}

	public int GetExtraDamageForAlternating()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForAlternatingMod.GetModifiedValue(m_extraDamageForAlternating)
			: m_extraDamageForAlternating;
	}

	public int GetExtraDamageForFarTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForFarTargetMod.GetModifiedValue(m_extraDamageForFarTarget)
			: m_extraDamageForFarTarget;
	}

	public float GetLaserFarDistThresh()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserFarDistThreshMod.GetModifiedValue(m_laserFarDistThresh)
			: m_laserFarDistThresh;
	}

	public float GetCircleFarDistThresh()
	{
		return m_abilityMod != null
			? m_abilityMod.m_circleFarDistThreshMod.GetModifiedValue(m_circleFarDistThresh)
			: m_circleFarDistThresh;
	}

	public int GetHealPerEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healPerEnemyHitMod.GetModifiedValue(m_healPerEnemyHit)
			: m_healPerEnemyHit;
	}

	public int GetCdrOnAbility()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrOnAbilityMod.GetModifiedValue(m_cdrOnAbility)
			: m_cdrOnAbility;
	}

	public int GetCdrMinTriggerHitCount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrMinTriggerHitCountMod.GetModifiedValue(m_cdrMinTriggerHitCount)
			: m_cdrMinTriggerHitCount;
	}

	public int GetAbsorbPerEnemyHitOnTurnStart()
	{
		return m_abilityMod != null
			? m_abilityMod.m_absorbPerEnemyHitOnTurnStartMod.GetModifiedValue(m_absorbPerEnemyHitOnTurnStart)
			: m_absorbPerEnemyHitOnTurnStart;
	}

	public int GetAbsorbAmountIfTriggeredHitCount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_absorbAmountIfTriggeredHitCountMod.GetModifiedValue(m_absorbAmountIfTriggeredHitCount)
			: m_absorbAmountIfTriggeredHitCount;
	}

	public int GetExtraDamageForFarTarget(ActorData targetActor, ActorData caster, bool forCone)
	{
		float threshold = forCone ? GetCircleFarDistThresh() : GetLaserFarDistThresh();
		if (threshold > 0f && GetExtraDamageForFarTarget() > 0)
		{
			Vector3 vector = targetActor.GetFreePos() - caster.GetFreePos();
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (magnitude >= threshold * Board.Get().squareSize)
			{
				return GetExtraDamageForFarTarget();
			}
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "CircleDamage", string.Empty, m_circleDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_circleEnemyHitEffect, "CircleEnemyHitEffect", m_circleEnemyHitEffect);
		AddTokenInt(tokens, "LaserDamage", string.Empty, m_laserDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserEnemyHitEffect, "LaserEnemyHitEffect", m_laserEnemyHitEffect);
		AddTokenInt(tokens, "ExtraDamageForAlternating", string.Empty, m_extraDamageForAlternating);
		AddTokenInt(tokens, "ExtraDamageForFarTarget", string.Empty, m_extraDamageForFarTarget);
		AddTokenInt(tokens, "HealPerEnemyHit", string.Empty, m_healPerEnemyHit);
		AddTokenInt(tokens, "CdrOnAbility", string.Empty, m_cdrOnAbility);
		AddTokenInt(tokens, "CdrMinTriggerHitCount", string.Empty, m_cdrMinTriggerHitCount);
		AddTokenInt(tokens, "AbsorbPerEnemyHitOnTurnStart", string.Empty, m_absorbPerEnemyHitOnTurnStart);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetCircleDamage());
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetLaserDamage());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetHealPerEnemyHit());
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			bool hasPrimary = Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0;
			int damage;
			if (hasPrimary)
			{
				damage = GetCircleDamage();
				if (GetExtraDamageForAlternating() > 0 && m_syncComp != null && m_syncComp.m_lastPrimaryUsedMode == 2)
				{
					damage += GetExtraDamageForAlternating();
				}
			}
			else
			{
				damage = GetLaserDamage();
				if (GetExtraDamageForAlternating() > 0 && m_syncComp != null && m_syncComp.m_lastPrimaryUsedMode == 1)
				{
					damage += GetExtraDamageForAlternating();
				}
			}
			int extraDamageForFarTarget = GetExtraDamageForFarTarget(targetActor, ActorData, hasPrimary);
			results.m_damage = damage + extraDamageForFarTarget;
		}
		else if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
		{
			int healing = 0;
			if (GetHealPerEnemyHit() > 0)
			{
				int enemyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				healing = enemyNum * GetHealPerEnemyHit();
			}
			results.m_healing = healing;
			if (GetAbsorbAmountIfTriggeredHitCount() > 0)
			{
				int enemyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				if (enemyNum >= GetCdrMinTriggerHitCount())
				{
					results.m_absorb = GetAbsorbAmountIfTriggeredHitCount();
				}
			}
		}
		return true;
	}

	private bool ShouldUseCircle(Vector3 freePos, ActorData caster)
	{
		Vector3 vector = freePos - caster.GetFreePos();
		vector.y = 0f;
		return vector.magnitude <= m_circleDistThreshold;
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = m_circleDistThreshold - 0.1f;
		max = m_circleDistThreshold + 0.1f;
		return true;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return animIndex == m_onCastCircleAnimIndex || animIndex == m_onCastLaserAnimIndex;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType(List<AbilityTarget> targets, ActorData caster)
	{
		if (targets != null && caster != null)
		{
			return ShouldUseCircle(targets[0].FreePos, caster)
				? (ActorModelData.ActionAnimationType)m_onCastCircleAnimIndex
				: (ActorModelData.ActionAnimationType)m_onCastLaserAnimIndex;
		}
		return base.GetActionAnimType();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SenseiBasicAttack))
		{
			m_abilityMod = abilityMod as AbilityMod_SenseiBasicAttack;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
