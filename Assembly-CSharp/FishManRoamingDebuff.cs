using System.Collections.Generic;
using UnityEngine;

public class FishManRoamingDebuff : Ability
{
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
	public RoamingDebuffJumpPreference m_primaryJumpPreference;
	public RoamingDebuffJumpPreference m_secondaryJumpPreference = RoamingDebuffJumpPreference.DontCare;
	public RoamingDebuffJumpPreference m_tiebreakerJumpPreference = RoamingDebuffJumpPreference.DontCare;
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
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_Laser(this, m_cachedLaserInfo);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_abilityMod != null
			? m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo)
			: m_laserInfo;
		m_cachedEffectWhileOnEnemy = m_abilityMod != null
			? m_abilityMod.m_effectWhileOnEnemyMod.GetModifiedValue(m_effectWhileOnEnemy)
			: m_effectWhileOnEnemy;
		m_cachedEffectWhileOnAlly = m_abilityMod != null
			? m_abilityMod.m_effectWhileOnAllyMod.GetModifiedValue(m_effectWhileOnAlly)
			: m_effectWhileOnAlly;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public StandardEffectInfo GetEffectWhileOnEnemy()
	{
		return m_cachedEffectWhileOnEnemy ?? m_effectWhileOnEnemy;
	}

	public StandardEffectInfo GetEffectWhileOnAlly()
	{
		return m_cachedEffectWhileOnAlly ?? m_effectWhileOnAlly;
	}

	public float GetJumpRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_jumpRadiusMod.GetModifiedValue(m_jumpRadius)
			: m_jumpRadius;
	}

	public bool GetJumpIgnoresLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_jumpIgnoresLineOfSightMod.GetModifiedValue(m_jumpIgnoresLineOfSight)
			: m_jumpIgnoresLineOfSight;
	}

	public int GetNumJumps()
	{
		return m_abilityMod != null
			? m_abilityMod.m_numJumpsMod.GetModifiedValue(m_numJumps)
			: m_numJumps;
	}

	public bool CanJumpToEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canJumpToEnemiesMod.GetModifiedValue(m_canJumpToEnemies)
			: m_canJumpToEnemies;
	}

	public bool CanJumpToAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canJumpToAlliesMod.GetModifiedValue(m_canJumpToAllies)
			: m_canJumpToAllies;
	}

	public bool CanJumpToInvisibleTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canJumpToInvisibleTargetsMod.GetModifiedValue(m_canJumpToInvisibleTargets)
			: m_canJumpToInvisibleTargets;
	}

	public int GetDamageToEnemiesOnJump()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageToEnemiesOnJumpMod.GetModifiedValue(m_damageToEnemiesOnJump)
			: m_damageToEnemiesOnJump;
	}

	public int GetHealingToAlliesOnJump()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healingToAlliesOnJumpMod.GetModifiedValue(m_healingToAlliesOnJump)
			: m_healingToAlliesOnJump;
	}

	public int GetDamageIncreasePerJump()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageIncreasePerJumpMod.GetModifiedValue(m_damageIncreasePerJump)
			: m_damageIncreasePerJump;
	}

	public int GetDamageToEnemyOnInitialHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageToEnemyOnInitialHitMod.GetModifiedValue(m_damageToEnemyOnInitialHit)
			: m_damageToEnemyOnInitialHit;
	}

	public int GetHealingToAllyOnInitialHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healingToAllyOnInitialHitMod.GetModifiedValue(m_healingToAllyOnInitialHit)
			: m_healingToAllyOnInitialHit;
	}

	public int GetJumpAnimationIndex()
	{
		return m_abilityMod != null
			? m_abilityMod.m_jumpAnimationIndexMod.GetModifiedValue(m_jumpAnimationIndex)
			: m_jumpAnimationIndex;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_FishManRoamingDebuff))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		
		m_abilityMod = abilityMod as AbilityMod_FishManRoamingDebuff;
		Setup();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManRoamingDebuff abilityMod_FishManRoamingDebuff = modAsBase as AbilityMod_FishManRoamingDebuff;
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManRoamingDebuff != null
			? abilityMod_FishManRoamingDebuff.m_effectWhileOnEnemyMod.GetModifiedValue(m_effectWhileOnEnemy)
			: m_effectWhileOnEnemy, "EffectWhileOnEnemy", m_effectWhileOnEnemy);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManRoamingDebuff != null
			? abilityMod_FishManRoamingDebuff.m_effectWhileOnAllyMod.GetModifiedValue(m_effectWhileOnAlly)
			: m_effectWhileOnAlly, "EffectWhileOnAlly", m_effectWhileOnAlly);
		AddTokenInt(tokens, "DamageToEnemiesOnJump", string.Empty, abilityMod_FishManRoamingDebuff != null
			? abilityMod_FishManRoamingDebuff.m_damageToEnemiesOnJumpMod.GetModifiedValue(m_damageToEnemiesOnJump)
			: m_damageToEnemiesOnJump);
		AddTokenInt(tokens, "HealingToAlliesOnJump", string.Empty, abilityMod_FishManRoamingDebuff != null
			? abilityMod_FishManRoamingDebuff.m_healingToAlliesOnJumpMod.GetModifiedValue(m_healingToAlliesOnJump)
			: m_healingToAlliesOnJump);
		AddTokenInt(tokens, "DamageToEnemyOnInitialHit", string.Empty, abilityMod_FishManRoamingDebuff != null
			? abilityMod_FishManRoamingDebuff.m_damageToEnemyOnInitialHitMod.GetModifiedValue(m_damageToEnemyOnInitialHit)
			: m_damageToEnemyOnInitialHit);
		AddTokenInt(tokens, "HealingToAllyOnInitialHit", string.Empty, abilityMod_FishManRoamingDebuff != null
			? abilityMod_FishManRoamingDebuff.m_healingToAllyOnInitialHitMod.GetModifiedValue(m_healingToAllyOnInitialHit)
			: m_healingToAllyOnInitialHit);
		AddTokenFloatRounded(tokens, "JumpRadius_Rounded", string.Empty, abilityMod_FishManRoamingDebuff != null
			? abilityMod_FishManRoamingDebuff.m_jumpRadiusMod.GetModifiedValue(m_jumpRadius)
			: m_jumpRadius);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (GetDamageToEnemyOnInitialHit() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageToEnemyOnInitialHit());
		}
		if (GetHealingToAllyOnInitialHit() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Ally, GetHealingToAllyOnInitialHit());
		}
		GetEffectWhileOnEnemy().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		GetEffectWhileOnAlly().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		return numbers;
	}
}
