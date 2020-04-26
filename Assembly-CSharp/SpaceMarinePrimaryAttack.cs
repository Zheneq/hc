using System.Collections.Generic;
using UnityEngine;

public class SpaceMarinePrimaryAttack : Ability
{
	[Separator("Targeting", true)]
	public LaserTargetingInfo m_laserTargetInfo;

	[Header("-- Cone on initial target hit --")]
	public bool m_addConeOnFirstHitTarget = true;

	public ConeTargetingInfo m_coneTargetInfo;

	[Separator("Enemy Hit: Laser", true)]
	public int m_damageAmount = 18;

	public int m_extraDamageToClosestTarget;

	[Separator("Enemy Hit: Cone", true)]
	public int m_coneDamageAmount = 18;

	public StandardEffectInfo m_coneEnemyHitEffect;

	[Header("-- Whether length of targeter should ignore world geo")]
	public bool m_laserLengthIgnoreWorldGeo = true;

	private AbilityMod_SpaceMarinePrimaryAttack m_abilityMod;

	private LaserTargetingInfo m_cachedLaserTargetInfo;

	private ConeTargetingInfo m_cachedConeTargetInfo;

	private StandardEffectInfo m_cachedConeEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Piston Punch";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		LaserTargetingInfo laserTargetInfo = GetLaserTargetInfo();
		ConeTargetingInfo coneTargetInfo = GetConeTargetInfo();
		AbilityUtil_Targeter_SpaceMarineBasicAttack abilityUtil_Targeter_SpaceMarineBasicAttack = new AbilityUtil_Targeter_SpaceMarineBasicAttack(this, laserTargetInfo.width, laserTargetInfo.range, laserTargetInfo.maxTargets, coneTargetInfo.m_widthAngleDeg, coneTargetInfo.m_radiusInSquares, laserTargetInfo.penetrateLos);
		abilityUtil_Targeter_SpaceMarineBasicAttack.AddConeOnFirstLaserHit = AddConeOnFirstHitTarget();
		abilityUtil_Targeter_SpaceMarineBasicAttack.LengthIgnoreWorldGeo = m_laserLengthIgnoreWorldGeo;
		base.Targeter = abilityUtil_Targeter_SpaceMarineBasicAttack;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserTargetInfo().range;
	}

	private void SetCachedFields()
	{
		m_cachedLaserTargetInfo = ((!m_abilityMod) ? m_laserTargetInfo : m_abilityMod.m_laserTargetInfoMod.GetModifiedValue(m_laserTargetInfo));
		ConeTargetingInfo cachedConeTargetInfo;
		if ((bool)m_abilityMod)
		{
			cachedConeTargetInfo = m_abilityMod.m_coneTargetInfoMod.GetModifiedValue(m_coneTargetInfo);
		}
		else
		{
			cachedConeTargetInfo = m_coneTargetInfo;
		}
		m_cachedConeTargetInfo = cachedConeTargetInfo;
		StandardEffectInfo cachedConeEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedConeEnemyHitEffect = m_abilityMod.m_coneEnemyHitEffectMod.GetModifiedValue(m_coneEnemyHitEffect);
		}
		else
		{
			cachedConeEnemyHitEffect = m_coneEnemyHitEffect;
		}
		m_cachedConeEnemyHitEffect = cachedConeEnemyHitEffect;
	}

	public LaserTargetingInfo GetLaserTargetInfo()
	{
		return (m_cachedLaserTargetInfo == null) ? m_laserTargetInfo : m_cachedLaserTargetInfo;
	}

	public bool AddConeOnFirstHitTarget()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_addConeOnFirstHitTargetMod.GetModifiedValue(m_addConeOnFirstHitTarget);
		}
		else
		{
			result = m_addConeOnFirstHitTarget;
		}
		return result;
	}

	public ConeTargetingInfo GetConeTargetInfo()
	{
		ConeTargetingInfo result;
		if (m_cachedConeTargetInfo != null)
		{
			result = m_cachedConeTargetInfo;
		}
		else
		{
			result = m_coneTargetInfo;
		}
		return result;
	}

	public int GetLaserDamage()
	{
		return (!m_abilityMod) ? m_damageAmount : m_abilityMod.m_baseDamageMod.GetModifiedValue(m_damageAmount);
	}

	public int GetExtraDamageToClosestTarget()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageOnClosestMod.GetModifiedValue(m_extraDamageToClosestTarget);
		}
		else
		{
			result = m_extraDamageToClosestTarget;
		}
		return result;
	}

	public int GetConeDamageAmount()
	{
		return (!m_abilityMod) ? m_coneDamageAmount : m_abilityMod.m_coneDamageAmountMod.GetModifiedValue(m_coneDamageAmount);
	}

	public StandardEffectInfo GetConeEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedConeEnemyHitEffect != null)
		{
			result = m_cachedConeEnemyHitEffect;
		}
		else
		{
			result = m_coneEnemyHitEffect;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_coneDamageAmount);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
		{
			if (base.Targeter is AbilityUtil_Targeter_SpaceMarineBasicAttack)
			{
				int num = GetLaserDamage();
				AbilityUtil_Targeter_SpaceMarineBasicAttack abilityUtil_Targeter_SpaceMarineBasicAttack = base.Targeter as AbilityUtil_Targeter_SpaceMarineBasicAttack;
				List<ActorData> lastLaserHitActors = abilityUtil_Targeter_SpaceMarineBasicAttack.GetLastLaserHitActors();
				if (lastLaserHitActors.Count > 0 && lastLaserHitActors[0] == targetActor)
				{
					num += GetExtraDamageToClosestTarget();
				}
				results.m_damage = num;
			}
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Secondary) > 0)
		{
			results.m_damage = GetConeDamageAmount();
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "BaseDamage", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "TotalDamage_Closest", string.Empty, m_damageAmount + m_extraDamageToClosestTarget);
		AddTokenInt(tokens, "ExtraDamageOnClosest", string.Empty, m_extraDamageToClosestTarget);
		AddTokenInt(tokens, "ConeDamageAmount", string.Empty, m_coneDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_coneEnemyHitEffect, "ConeEnemyHitEffect", m_coneEnemyHitEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SpaceMarinePrimaryAttack))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_SpaceMarinePrimaryAttack);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
