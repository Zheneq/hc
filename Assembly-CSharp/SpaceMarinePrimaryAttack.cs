using System;
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
	public int m_damageAmount = 0x12;

	public int m_extraDamageToClosestTarget;

	[Separator("Enemy Hit: Cone", true)]
	public int m_coneDamageAmount = 0x12;

	public StandardEffectInfo m_coneEnemyHitEffect;

	[Header("-- Whether length of targeter should ignore world geo")]
	public bool m_laserLengthIgnoreWorldGeo = true;

	private AbilityMod_SpaceMarinePrimaryAttack m_abilityMod;

	private LaserTargetingInfo m_cachedLaserTargetInfo;

	private ConeTargetingInfo m_cachedConeTargetInfo;

	private StandardEffectInfo m_cachedConeEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Piston Punch";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		LaserTargetingInfo laserTargetInfo = this.GetLaserTargetInfo();
		ConeTargetingInfo coneTargetInfo = this.GetConeTargetInfo();
		base.Targeter = new AbilityUtil_Targeter_SpaceMarineBasicAttack(this, laserTargetInfo.width, laserTargetInfo.range, laserTargetInfo.maxTargets, coneTargetInfo.m_widthAngleDeg, coneTargetInfo.m_radiusInSquares, laserTargetInfo.penetrateLos)
		{
			AddConeOnFirstLaserHit = this.AddConeOnFirstHitTarget(),
			LengthIgnoreWorldGeo = this.m_laserLengthIgnoreWorldGeo
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserTargetInfo().range;
	}

	private void SetCachedFields()
	{
		this.m_cachedLaserTargetInfo = ((!this.m_abilityMod) ? this.m_laserTargetInfo : this.m_abilityMod.m_laserTargetInfoMod.GetModifiedValue(this.m_laserTargetInfo));
		ConeTargetingInfo cachedConeTargetInfo;
		if (this.m_abilityMod)
		{
			cachedConeTargetInfo = this.m_abilityMod.m_coneTargetInfoMod.GetModifiedValue(this.m_coneTargetInfo);
		}
		else
		{
			cachedConeTargetInfo = this.m_coneTargetInfo;
		}
		this.m_cachedConeTargetInfo = cachedConeTargetInfo;
		StandardEffectInfo cachedConeEnemyHitEffect;
		if (this.m_abilityMod)
		{
			cachedConeEnemyHitEffect = this.m_abilityMod.m_coneEnemyHitEffectMod.GetModifiedValue(this.m_coneEnemyHitEffect);
		}
		else
		{
			cachedConeEnemyHitEffect = this.m_coneEnemyHitEffect;
		}
		this.m_cachedConeEnemyHitEffect = cachedConeEnemyHitEffect;
	}

	public LaserTargetingInfo GetLaserTargetInfo()
	{
		return (this.m_cachedLaserTargetInfo == null) ? this.m_laserTargetInfo : this.m_cachedLaserTargetInfo;
	}

	public bool AddConeOnFirstHitTarget()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_addConeOnFirstHitTargetMod.GetModifiedValue(this.m_addConeOnFirstHitTarget);
		}
		else
		{
			result = this.m_addConeOnFirstHitTarget;
		}
		return result;
	}

	public ConeTargetingInfo GetConeTargetInfo()
	{
		ConeTargetingInfo result;
		if (this.m_cachedConeTargetInfo != null)
		{
			result = this.m_cachedConeTargetInfo;
		}
		else
		{
			result = this.m_coneTargetInfo;
		}
		return result;
	}

	public int GetLaserDamage()
	{
		return (!this.m_abilityMod) ? this.m_damageAmount : this.m_abilityMod.m_baseDamageMod.GetModifiedValue(this.m_damageAmount);
	}

	public int GetExtraDamageToClosestTarget()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageOnClosestMod.GetModifiedValue(this.m_extraDamageToClosestTarget);
		}
		else
		{
			result = this.m_extraDamageToClosestTarget;
		}
		return result;
	}

	public int GetConeDamageAmount()
	{
		return (!this.m_abilityMod) ? this.m_coneDamageAmount : this.m_abilityMod.m_coneDamageAmountMod.GetModifiedValue(this.m_coneDamageAmount);
	}

	public StandardEffectInfo GetConeEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedConeEnemyHitEffect != null)
		{
			result = this.m_cachedConeEnemyHitEffect;
		}
		else
		{
			result = this.m_coneEnemyHitEffect;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_damageAmount);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_coneDamageAmount);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
		{
			if (base.Targeter is AbilityUtil_Targeter_SpaceMarineBasicAttack)
			{
				int num = this.GetLaserDamage();
				AbilityUtil_Targeter_SpaceMarineBasicAttack abilityUtil_Targeter_SpaceMarineBasicAttack = base.Targeter as AbilityUtil_Targeter_SpaceMarineBasicAttack;
				List<ActorData> lastLaserHitActors = abilityUtil_Targeter_SpaceMarineBasicAttack.GetLastLaserHitActors();
				if (lastLaserHitActors.Count > 0 && lastLaserHitActors[0] == targetActor)
				{
					num += this.GetExtraDamageToClosestTarget();
				}
				results.m_damage = num;
			}
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Secondary) > 0)
		{
			results.m_damage = this.GetConeDamageAmount();
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "BaseDamage", string.Empty, this.m_damageAmount, false);
		base.AddTokenInt(tokens, "TotalDamage_Closest", string.Empty, this.m_damageAmount + this.m_extraDamageToClosestTarget, false);
		base.AddTokenInt(tokens, "ExtraDamageOnClosest", string.Empty, this.m_extraDamageToClosestTarget, false);
		base.AddTokenInt(tokens, "ConeDamageAmount", string.Empty, this.m_coneDamageAmount, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_coneEnemyHitEffect, "ConeEnemyHitEffect", this.m_coneEnemyHitEffect, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SpaceMarinePrimaryAttack))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SpaceMarinePrimaryAttack);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
