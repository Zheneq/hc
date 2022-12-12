using System.Collections.Generic;
using UnityEngine;

public class SpaceMarinePrimaryAttack : Ability
{
	[Separator("Targeting")]
	public LaserTargetingInfo m_laserTargetInfo;
	[Header("-- Cone on initial target hit --")]
	public bool m_addConeOnFirstHitTarget = true;
	public ConeTargetingInfo m_coneTargetInfo;
	[Separator("Enemy Hit: Laser")]
	public int m_damageAmount = 18;
	public int m_extraDamageToClosestTarget;
	[Separator("Enemy Hit: Cone")]
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
		Targeter = new AbilityUtil_Targeter_SpaceMarineBasicAttack(
			this,
			laserTargetInfo.width,
			laserTargetInfo.range,
			laserTargetInfo.maxTargets,
			coneTargetInfo.m_widthAngleDeg,
			coneTargetInfo.m_radiusInSquares,
			laserTargetInfo.penetrateLos)
		{
			AddConeOnFirstLaserHit = AddConeOnFirstHitTarget(),
			LengthIgnoreWorldGeo = m_laserLengthIgnoreWorldGeo
		};
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
		m_cachedLaserTargetInfo = m_abilityMod != null
			? m_abilityMod.m_laserTargetInfoMod.GetModifiedValue(m_laserTargetInfo)
			: m_laserTargetInfo;
		m_cachedConeTargetInfo = m_abilityMod != null
			? m_abilityMod.m_coneTargetInfoMod.GetModifiedValue(m_coneTargetInfo)
			: m_coneTargetInfo;
		m_cachedConeEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_coneEnemyHitEffectMod.GetModifiedValue(m_coneEnemyHitEffect)
			: m_coneEnemyHitEffect;
	}

	public LaserTargetingInfo GetLaserTargetInfo()
	{
		return m_cachedLaserTargetInfo ?? m_laserTargetInfo;
	}

	public bool AddConeOnFirstHitTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_addConeOnFirstHitTargetMod.GetModifiedValue(m_addConeOnFirstHitTarget)
			: m_addConeOnFirstHitTarget;
	}

	public ConeTargetingInfo GetConeTargetInfo()
	{
		return m_cachedConeTargetInfo ?? m_coneTargetInfo;
	}

	public int GetLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_baseDamageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetExtraDamageToClosestTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageOnClosestMod.GetModifiedValue(m_extraDamageToClosestTarget)
			: m_extraDamageToClosestTarget;
	}

	public int GetConeDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneDamageAmountMod.GetModifiedValue(m_coneDamageAmount)
			: m_coneDamageAmount;
	}

	public StandardEffectInfo GetConeEnemyHitEffect()
	{
		return m_cachedConeEnemyHitEffect ?? m_coneEnemyHitEffect;
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
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
		{
			if (Targeter is AbilityUtil_Targeter_SpaceMarineBasicAttack)
			{
				int damage = GetLaserDamage();
				AbilityUtil_Targeter_SpaceMarineBasicAttack targeter = Targeter as AbilityUtil_Targeter_SpaceMarineBasicAttack;
				List<ActorData> lastLaserHitActors = targeter.GetLastLaserHitActors();
				if (lastLaserHitActors.Count > 0 && lastLaserHitActors[0] == targetActor)
				{
					damage += GetExtraDamageToClosestTarget();
				}
				results.m_damage = damage;
			}
		}
		else if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Secondary) > 0)
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
		if (abilityMod.GetType() != typeof(AbilityMod_SpaceMarinePrimaryAttack))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}

		m_abilityMod = abilityMod as AbilityMod_SpaceMarinePrimaryAttack;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
