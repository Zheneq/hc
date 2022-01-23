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

	[Separator("Heal Per Target Hit", true)]
	public int m_healPerEnemyHit;

	[Separator("Cooldown Reduction", true)]
	public int m_cdrOnAbility;

	public AbilityData.ActionType m_cdrAbilityTarget = AbilityData.ActionType.ABILITY_1;

	public int m_cdrMinTriggerHitCount = 3;

	[Separator("Shielding on turn start per enemy hit", true)]
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
		ConeTargetingInfo coneTargetingInfo = new ConeTargetingInfo();
		coneTargetingInfo.m_affectsAllies = GetLaserInfo().affectsAllies;
		coneTargetingInfo.m_affectsEnemies = GetLaserInfo().affectsEnemies;
		int affectsCaster;
		if (GetHealPerEnemyHit() <= 0)
		{
			affectsCaster = ((GetAbsorbAmountIfTriggeredHitCount() > 0) ? 1 : 0);
		}
		else
		{
			affectsCaster = 1;
		}
		coneTargetingInfo.m_affectsCaster = ((byte)affectsCaster != 0);
		coneTargetingInfo.m_penetrateLos = false;
		coneTargetingInfo.m_radiusInSquares = GetCircleRadius();
		coneTargetingInfo.m_widthAngleDeg = 360f;
		AbilityUtil_Targeter_ConeOrLaser abilityUtil_Targeter_ConeOrLaser = new AbilityUtil_Targeter_ConeOrLaser(this, coneTargetingInfo, GetLaserInfo(), m_circleDistThreshold);
		abilityUtil_Targeter_ConeOrLaser.m_customShouldAddCasterDelegate = ShouldAddCasterForTargeter;
		base.Targeter = abilityUtil_Targeter_ConeOrLaser;
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
		int result;
		if (GetHealPerEnemyHit() > 0)
		{
			if (actorsSoFar.Count > 0)
			{
				result = 1;
				goto IL_0061;
			}
		}
		if (GetAbsorbAmountIfTriggeredHitCount() > 0)
		{
			result = ((actorsSoFar.Count >= GetCdrMinTriggerHitCount()) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		goto IL_0061;
		IL_0061:
		return (byte)result != 0;
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo cachedLaserInfo;
		if ((bool)m_abilityMod)
		{
			cachedLaserInfo = m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo);
		}
		else
		{
			cachedLaserInfo = m_laserInfo;
		}
		m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedCircleEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedCircleEnemyHitEffect = m_abilityMod.m_circleEnemyHitEffectMod.GetModifiedValue(m_circleEnemyHitEffect);
		}
		else
		{
			cachedCircleEnemyHitEffect = m_circleEnemyHitEffect;
		}
		m_cachedCircleEnemyHitEffect = cachedCircleEnemyHitEffect;
		StandardEffectInfo cachedLaserEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedLaserEnemyHitEffect = m_abilityMod.m_laserEnemyHitEffectMod.GetModifiedValue(m_laserEnemyHitEffect);
		}
		else
		{
			cachedLaserEnemyHitEffect = m_laserEnemyHitEffect;
		}
		m_cachedLaserEnemyHitEffect = cachedLaserEnemyHitEffect;
	}

	public float GetCircleDistThreshold()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_circleDistThresholdMod.GetModifiedValue(m_circleDistThreshold);
		}
		else
		{
			result = m_circleDistThreshold;
		}
		return result;
	}

	public float GetCircleRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_circleRadiusMod.GetModifiedValue(m_circleRadius);
		}
		else
		{
			result = m_circleRadius;
		}
		return result;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedLaserInfo != null)
		{
			result = m_cachedLaserInfo;
		}
		else
		{
			result = m_laserInfo;
		}
		return result;
	}

	public int GetCircleDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_circleDamageMod.GetModifiedValue(m_circleDamage);
		}
		else
		{
			result = m_circleDamage;
		}
		return result;
	}

	public StandardEffectInfo GetCircleEnemyHitEffect()
	{
		return (m_cachedCircleEnemyHitEffect == null) ? m_circleEnemyHitEffect : m_cachedCircleEnemyHitEffect;
	}

	public int GetLaserDamage()
	{
		return (!m_abilityMod) ? m_laserDamage : m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamage);
	}

	public StandardEffectInfo GetLaserEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedLaserEnemyHitEffect != null)
		{
			result = m_cachedLaserEnemyHitEffect;
		}
		else
		{
			result = m_laserEnemyHitEffect;
		}
		return result;
	}

	public int GetExtraDamageForAlternating()
	{
		return (!m_abilityMod) ? m_extraDamageForAlternating : m_abilityMod.m_extraDamageForAlternatingMod.GetModifiedValue(m_extraDamageForAlternating);
	}

	public int GetExtraDamageForFarTarget()
	{
		return (!m_abilityMod) ? m_extraDamageForFarTarget : m_abilityMod.m_extraDamageForFarTargetMod.GetModifiedValue(m_extraDamageForFarTarget);
	}

	public float GetLaserFarDistThresh()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserFarDistThreshMod.GetModifiedValue(m_laserFarDistThresh);
		}
		else
		{
			result = m_laserFarDistThresh;
		}
		return result;
	}

	public float GetCircleFarDistThresh()
	{
		return (!m_abilityMod) ? m_circleFarDistThresh : m_abilityMod.m_circleFarDistThreshMod.GetModifiedValue(m_circleFarDistThresh);
	}

	public int GetHealPerEnemyHit()
	{
		return (!m_abilityMod) ? m_healPerEnemyHit : m_abilityMod.m_healPerEnemyHitMod.GetModifiedValue(m_healPerEnemyHit);
	}

	public int GetCdrOnAbility()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cdrOnAbilityMod.GetModifiedValue(m_cdrOnAbility);
		}
		else
		{
			result = m_cdrOnAbility;
		}
		return result;
	}

	public int GetCdrMinTriggerHitCount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cdrMinTriggerHitCountMod.GetModifiedValue(m_cdrMinTriggerHitCount);
		}
		else
		{
			result = m_cdrMinTriggerHitCount;
		}
		return result;
	}

	public int GetAbsorbPerEnemyHitOnTurnStart()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_absorbPerEnemyHitOnTurnStartMod.GetModifiedValue(m_absorbPerEnemyHitOnTurnStart);
		}
		else
		{
			result = m_absorbPerEnemyHitOnTurnStart;
		}
		return result;
	}

	public int GetAbsorbAmountIfTriggeredHitCount()
	{
		return (!(m_abilityMod != null)) ? m_absorbAmountIfTriggeredHitCount : m_abilityMod.m_absorbAmountIfTriggeredHitCountMod.GetModifiedValue(m_absorbAmountIfTriggeredHitCount);
	}

	public int GetExtraDamageForFarTarget(ActorData targetActor, ActorData caster, bool forCone)
	{
		float num;
		if (forCone)
		{
			num = GetCircleFarDistThresh();
		}
		else
		{
			num = GetLaserFarDistThresh();
		}
		float num2 = num;
		if (num2 > 0f)
		{
			if (GetExtraDamageForFarTarget() > 0)
			{
				Vector3 vector = targetActor.GetFreePos() - caster.GetFreePos();
				vector.y = 0f;
				float magnitude = vector.magnitude;
				if (magnitude >= num2 * Board.Get().squareSize)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return GetExtraDamageForFarTarget();
						}
					}
				}
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
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			bool flag = base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0;
			int num = 0;
			if (flag)
			{
				num = GetCircleDamage();
				if (GetExtraDamageForAlternating() > 0)
				{
					if ((bool)m_syncComp && m_syncComp.m_lastPrimaryUsedMode == 2)
					{
						num += GetExtraDamageForAlternating();
					}
				}
			}
			else
			{
				num = GetLaserDamage();
				if (GetExtraDamageForAlternating() > 0)
				{
					if ((bool)m_syncComp)
					{
						if (m_syncComp.m_lastPrimaryUsedMode == 1)
						{
							num += GetExtraDamageForAlternating();
						}
					}
				}
			}
			int extraDamageForFarTarget = GetExtraDamageForFarTarget(targetActor, base.ActorData, flag);
			num = (results.m_damage = num + extraDamageForFarTarget);
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
		{
			int healing = 0;
			if (GetHealPerEnemyHit() > 0)
			{
				int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				healing = visibleActorsCountByTooltipSubject * GetHealPerEnemyHit();
			}
			results.m_healing = healing;
			if (GetAbsorbAmountIfTriggeredHitCount() > 0)
			{
				int visibleActorsCountByTooltipSubject2 = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				if (visibleActorsCountByTooltipSubject2 >= GetCdrMinTriggerHitCount())
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
		float magnitude = vector.magnitude;
		return magnitude <= m_circleDistThreshold;
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
		if (targets != null)
		{
			if (caster != null)
			{
				int result;
				if (ShouldUseCircle(targets[0].FreePos, caster))
				{
					result = m_onCastCircleAnimIndex;
				}
				else
				{
					result = m_onCastLaserAnimIndex;
				}
				return (ActorModelData.ActionAnimationType)result;
			}
		}
		return base.GetActionAnimType();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SenseiBasicAttack))
		{
			m_abilityMod = (abilityMod as AbilityMod_SenseiBasicAttack);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
