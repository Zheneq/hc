using System;
using System.Collections.Generic;
using UnityEngine;

public class FishManRoamingDebuff : Ability
{
	[Header("-- Laser Targeting")]
	public LaserTargetingInfo m_laserInfo;

	[Header("-- Effect Data")]
	public StandardEffectInfo m_effectWhileOnEnemy;

	public StandardEffectInfo m_effectWhileOnAlly;

	[Header("-- Jump Criteria")]
	public float m_jumpRadius;

	public bool m_jumpIgnoresLineOfSight;

	public int m_numJumps = 2;

	public bool m_canJumpToEnemies = true;

	public bool m_canJumpToAllies;

	public bool m_canJumpToInvisibleTargets;

	public FishManRoamingDebuff.RoamingDebuffJumpPreference m_primaryJumpPreference;

	public FishManRoamingDebuff.RoamingDebuffJumpPreference m_secondaryJumpPreference = FishManRoamingDebuff.RoamingDebuffJumpPreference.DontCare;

	public FishManRoamingDebuff.RoamingDebuffJumpPreference m_tiebreakerJumpPreference = FishManRoamingDebuff.RoamingDebuffJumpPreference.DontCare;

	[Header("-- Damage/Healing on initial hit")]
	public int m_damageToEnemyOnInitialHit;

	public int m_healingToAllyOnInitialHit;

	[Header("-- Damage/Healing on jump")]
	public int m_damageToEnemiesOnJump;

	public int m_healingToAlliesOnJump;

	public int m_damageIncreasePerJump;

	[Header("-- Sequences")]
	public GameObject m_castSequence;

	public GameObject m_jumpSequence;

	[Header("-- Anim")]
	public int m_jumpAnimationIndex;

	private AbilityMod_FishManRoamingDebuff m_abilityMod;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardEffectInfo m_cachedEffectWhileOnEnemy;

	private StandardEffectInfo m_cachedEffectWhileOnAlly;

	private void Start()
	{
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Laser(this, this.m_cachedLaserInfo);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserInfo().range;
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo cachedLaserInfo;
		if (this.m_abilityMod)
		{
			cachedLaserInfo = this.m_abilityMod.m_laserInfoMod.GetModifiedValue(this.m_laserInfo);
		}
		else
		{
			cachedLaserInfo = this.m_laserInfo;
		}
		this.m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedEffectWhileOnEnemy;
		if (this.m_abilityMod)
		{
			cachedEffectWhileOnEnemy = this.m_abilityMod.m_effectWhileOnEnemyMod.GetModifiedValue(this.m_effectWhileOnEnemy);
		}
		else
		{
			cachedEffectWhileOnEnemy = this.m_effectWhileOnEnemy;
		}
		this.m_cachedEffectWhileOnEnemy = cachedEffectWhileOnEnemy;
		StandardEffectInfo cachedEffectWhileOnAlly;
		if (this.m_abilityMod)
		{
			cachedEffectWhileOnAlly = this.m_abilityMod.m_effectWhileOnAllyMod.GetModifiedValue(this.m_effectWhileOnAlly);
		}
		else
		{
			cachedEffectWhileOnAlly = this.m_effectWhileOnAlly;
		}
		this.m_cachedEffectWhileOnAlly = cachedEffectWhileOnAlly;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedLaserInfo != null)
		{
			result = this.m_cachedLaserInfo;
		}
		else
		{
			result = this.m_laserInfo;
		}
		return result;
	}

	public StandardEffectInfo GetEffectWhileOnEnemy()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectWhileOnEnemy != null)
		{
			result = this.m_cachedEffectWhileOnEnemy;
		}
		else
		{
			result = this.m_effectWhileOnEnemy;
		}
		return result;
	}

	public StandardEffectInfo GetEffectWhileOnAlly()
	{
		return (this.m_cachedEffectWhileOnAlly == null) ? this.m_effectWhileOnAlly : this.m_cachedEffectWhileOnAlly;
	}

	public float GetJumpRadius()
	{
		return (!this.m_abilityMod) ? this.m_jumpRadius : this.m_abilityMod.m_jumpRadiusMod.GetModifiedValue(this.m_jumpRadius);
	}

	public bool GetJumpIgnoresLoS()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_jumpIgnoresLineOfSightMod.GetModifiedValue(this.m_jumpIgnoresLineOfSight);
		}
		else
		{
			result = this.m_jumpIgnoresLineOfSight;
		}
		return result;
	}

	public int GetNumJumps()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_numJumpsMod.GetModifiedValue(this.m_numJumps);
		}
		else
		{
			result = this.m_numJumps;
		}
		return result;
	}

	public bool CanJumpToEnemies()
	{
		return (!this.m_abilityMod) ? this.m_canJumpToEnemies : this.m_abilityMod.m_canJumpToEnemiesMod.GetModifiedValue(this.m_canJumpToEnemies);
	}

	public bool CanJumpToAllies()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_canJumpToAlliesMod.GetModifiedValue(this.m_canJumpToAllies);
		}
		else
		{
			result = this.m_canJumpToAllies;
		}
		return result;
	}

	public bool CanJumpToInvisibleTargets()
	{
		return (!this.m_abilityMod) ? this.m_canJumpToInvisibleTargets : this.m_abilityMod.m_canJumpToInvisibleTargetsMod.GetModifiedValue(this.m_canJumpToInvisibleTargets);
	}

	public int GetDamageToEnemiesOnJump()
	{
		return (!this.m_abilityMod) ? this.m_damageToEnemiesOnJump : this.m_abilityMod.m_damageToEnemiesOnJumpMod.GetModifiedValue(this.m_damageToEnemiesOnJump);
	}

	public int GetHealingToAlliesOnJump()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_healingToAlliesOnJumpMod.GetModifiedValue(this.m_healingToAlliesOnJump);
		}
		else
		{
			result = this.m_healingToAlliesOnJump;
		}
		return result;
	}

	public int GetDamageIncreasePerJump()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageIncreasePerJumpMod.GetModifiedValue(this.m_damageIncreasePerJump);
		}
		else
		{
			result = this.m_damageIncreasePerJump;
		}
		return result;
	}

	public int GetDamageToEnemyOnInitialHit()
	{
		return (!this.m_abilityMod) ? this.m_damageToEnemyOnInitialHit : this.m_abilityMod.m_damageToEnemyOnInitialHitMod.GetModifiedValue(this.m_damageToEnemyOnInitialHit);
	}

	public int GetHealingToAllyOnInitialHit()
	{
		return (!this.m_abilityMod) ? this.m_healingToAllyOnInitialHit : this.m_abilityMod.m_healingToAllyOnInitialHitMod.GetModifiedValue(this.m_healingToAllyOnInitialHit);
	}

	public int GetJumpAnimationIndex()
	{
		return (!this.m_abilityMod) ? this.m_jumpAnimationIndex : this.m_abilityMod.m_jumpAnimationIndexMod.GetModifiedValue(this.m_jumpAnimationIndex);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManRoamingDebuff))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_FishManRoamingDebuff);
			this.Setup();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManRoamingDebuff abilityMod_FishManRoamingDebuff = modAsBase as AbilityMod_FishManRoamingDebuff;
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_FishManRoamingDebuff) ? this.m_effectWhileOnEnemy : abilityMod_FishManRoamingDebuff.m_effectWhileOnEnemyMod.GetModifiedValue(this.m_effectWhileOnEnemy), "EffectWhileOnEnemy", this.m_effectWhileOnEnemy, true);
		StandardEffectInfo effectInfo;
		if (abilityMod_FishManRoamingDebuff)
		{
			effectInfo = abilityMod_FishManRoamingDebuff.m_effectWhileOnAllyMod.GetModifiedValue(this.m_effectWhileOnAlly);
		}
		else
		{
			effectInfo = this.m_effectWhileOnAlly;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectWhileOnAlly", this.m_effectWhileOnAlly, true);
		base.AddTokenInt(tokens, "DamageToEnemiesOnJump", string.Empty, (!abilityMod_FishManRoamingDebuff) ? this.m_damageToEnemiesOnJump : abilityMod_FishManRoamingDebuff.m_damageToEnemiesOnJumpMod.GetModifiedValue(this.m_damageToEnemiesOnJump), false);
		string name = "HealingToAlliesOnJump";
		string empty = string.Empty;
		int val;
		if (abilityMod_FishManRoamingDebuff)
		{
			val = abilityMod_FishManRoamingDebuff.m_healingToAlliesOnJumpMod.GetModifiedValue(this.m_healingToAlliesOnJump);
		}
		else
		{
			val = this.m_healingToAlliesOnJump;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "DamageToEnemyOnInitialHit";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_FishManRoamingDebuff)
		{
			val2 = abilityMod_FishManRoamingDebuff.m_damageToEnemyOnInitialHitMod.GetModifiedValue(this.m_damageToEnemyOnInitialHit);
		}
		else
		{
			val2 = this.m_damageToEnemyOnInitialHit;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "HealingToAllyOnInitialHit";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_FishManRoamingDebuff)
		{
			val3 = abilityMod_FishManRoamingDebuff.m_healingToAllyOnInitialHitMod.GetModifiedValue(this.m_healingToAllyOnInitialHit);
		}
		else
		{
			val3 = this.m_healingToAllyOnInitialHit;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "JumpRadius_Rounded";
		string empty4 = string.Empty;
		float val4;
		if (abilityMod_FishManRoamingDebuff)
		{
			val4 = abilityMod_FishManRoamingDebuff.m_jumpRadiusMod.GetModifiedValue(this.m_jumpRadius);
		}
		else
		{
			val4 = this.m_jumpRadius;
		}
		base.AddTokenFloatRounded(tokens, name4, empty4, val4, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.GetDamageToEnemyOnInitialHit() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamageToEnemyOnInitialHit());
		}
		if (this.GetHealingToAllyOnInitialHit() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Ally, this.GetHealingToAllyOnInitialHit());
		}
		this.GetEffectWhileOnEnemy().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		this.GetEffectWhileOnAlly().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		return result;
	}

	public enum RoamingDebuffJumpPreference
	{
		Closest,
		Farthest,
		Enemy,
		Ally,
		MostHP,
		LeastHP,
		DontCare
	}
}
