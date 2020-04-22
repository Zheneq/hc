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
		base.Targeter = new AbilityUtil_Targeter_Laser(this, m_cachedLaserInfo);
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
		LaserTargetingInfo cachedLaserInfo;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			cachedLaserInfo = m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo);
		}
		else
		{
			cachedLaserInfo = m_laserInfo;
		}
		m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedEffectWhileOnEnemy;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedEffectWhileOnEnemy = m_abilityMod.m_effectWhileOnEnemyMod.GetModifiedValue(m_effectWhileOnEnemy);
		}
		else
		{
			cachedEffectWhileOnEnemy = m_effectWhileOnEnemy;
		}
		m_cachedEffectWhileOnEnemy = cachedEffectWhileOnEnemy;
		StandardEffectInfo cachedEffectWhileOnAlly;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedEffectWhileOnAlly = m_abilityMod.m_effectWhileOnAllyMod.GetModifiedValue(m_effectWhileOnAlly);
		}
		else
		{
			cachedEffectWhileOnAlly = m_effectWhileOnAlly;
		}
		m_cachedEffectWhileOnAlly = cachedEffectWhileOnAlly;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedLaserInfo != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedLaserInfo;
		}
		else
		{
			result = m_laserInfo;
		}
		return result;
	}

	public StandardEffectInfo GetEffectWhileOnEnemy()
	{
		StandardEffectInfo result;
		if (m_cachedEffectWhileOnEnemy != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedEffectWhileOnEnemy;
		}
		else
		{
			result = m_effectWhileOnEnemy;
		}
		return result;
	}

	public StandardEffectInfo GetEffectWhileOnAlly()
	{
		return (m_cachedEffectWhileOnAlly == null) ? m_effectWhileOnAlly : m_cachedEffectWhileOnAlly;
	}

	public float GetJumpRadius()
	{
		return (!m_abilityMod) ? m_jumpRadius : m_abilityMod.m_jumpRadiusMod.GetModifiedValue(m_jumpRadius);
	}

	public bool GetJumpIgnoresLoS()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_jumpIgnoresLineOfSightMod.GetModifiedValue(m_jumpIgnoresLineOfSight);
		}
		else
		{
			result = m_jumpIgnoresLineOfSight;
		}
		return result;
	}

	public int GetNumJumps()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_numJumpsMod.GetModifiedValue(m_numJumps);
		}
		else
		{
			result = m_numJumps;
		}
		return result;
	}

	public bool CanJumpToEnemies()
	{
		return (!m_abilityMod) ? m_canJumpToEnemies : m_abilityMod.m_canJumpToEnemiesMod.GetModifiedValue(m_canJumpToEnemies);
	}

	public bool CanJumpToAllies()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_canJumpToAlliesMod.GetModifiedValue(m_canJumpToAllies);
		}
		else
		{
			result = m_canJumpToAllies;
		}
		return result;
	}

	public bool CanJumpToInvisibleTargets()
	{
		return (!m_abilityMod) ? m_canJumpToInvisibleTargets : m_abilityMod.m_canJumpToInvisibleTargetsMod.GetModifiedValue(m_canJumpToInvisibleTargets);
	}

	public int GetDamageToEnemiesOnJump()
	{
		return (!m_abilityMod) ? m_damageToEnemiesOnJump : m_abilityMod.m_damageToEnemiesOnJumpMod.GetModifiedValue(m_damageToEnemiesOnJump);
	}

	public int GetHealingToAlliesOnJump()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_healingToAlliesOnJumpMod.GetModifiedValue(m_healingToAlliesOnJump);
		}
		else
		{
			result = m_healingToAlliesOnJump;
		}
		return result;
	}

	public int GetDamageIncreasePerJump()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_damageIncreasePerJumpMod.GetModifiedValue(m_damageIncreasePerJump);
		}
		else
		{
			result = m_damageIncreasePerJump;
		}
		return result;
	}

	public int GetDamageToEnemyOnInitialHit()
	{
		return (!m_abilityMod) ? m_damageToEnemyOnInitialHit : m_abilityMod.m_damageToEnemyOnInitialHitMod.GetModifiedValue(m_damageToEnemyOnInitialHit);
	}

	public int GetHealingToAllyOnInitialHit()
	{
		return (!m_abilityMod) ? m_healingToAllyOnInitialHit : m_abilityMod.m_healingToAllyOnInitialHitMod.GetModifiedValue(m_healingToAllyOnInitialHit);
	}

	public int GetJumpAnimationIndex()
	{
		return (!m_abilityMod) ? m_jumpAnimationIndex : m_abilityMod.m_jumpAnimationIndexMod.GetModifiedValue(m_jumpAnimationIndex);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManRoamingDebuff))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityMod = (abilityMod as AbilityMod_FishManRoamingDebuff);
					Setup();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManRoamingDebuff abilityMod_FishManRoamingDebuff = modAsBase as AbilityMod_FishManRoamingDebuff;
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_FishManRoamingDebuff) ? m_effectWhileOnEnemy : abilityMod_FishManRoamingDebuff.m_effectWhileOnEnemyMod.GetModifiedValue(m_effectWhileOnEnemy), "EffectWhileOnEnemy", m_effectWhileOnEnemy);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_FishManRoamingDebuff)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			effectInfo = abilityMod_FishManRoamingDebuff.m_effectWhileOnAllyMod.GetModifiedValue(m_effectWhileOnAlly);
		}
		else
		{
			effectInfo = m_effectWhileOnAlly;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectWhileOnAlly", m_effectWhileOnAlly);
		AddTokenInt(tokens, "DamageToEnemiesOnJump", string.Empty, (!abilityMod_FishManRoamingDebuff) ? m_damageToEnemiesOnJump : abilityMod_FishManRoamingDebuff.m_damageToEnemiesOnJumpMod.GetModifiedValue(m_damageToEnemiesOnJump));
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_FishManRoamingDebuff)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			val = abilityMod_FishManRoamingDebuff.m_healingToAlliesOnJumpMod.GetModifiedValue(m_healingToAlliesOnJump);
		}
		else
		{
			val = m_healingToAlliesOnJump;
		}
		AddTokenInt(tokens, "HealingToAlliesOnJump", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_FishManRoamingDebuff)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = abilityMod_FishManRoamingDebuff.m_damageToEnemyOnInitialHitMod.GetModifiedValue(m_damageToEnemyOnInitialHit);
		}
		else
		{
			val2 = m_damageToEnemyOnInitialHit;
		}
		AddTokenInt(tokens, "DamageToEnemyOnInitialHit", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_FishManRoamingDebuff)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			val3 = abilityMod_FishManRoamingDebuff.m_healingToAllyOnInitialHitMod.GetModifiedValue(m_healingToAllyOnInitialHit);
		}
		else
		{
			val3 = m_healingToAllyOnInitialHit;
		}
		AddTokenInt(tokens, "HealingToAllyOnInitialHit", empty3, val3);
		string empty4 = string.Empty;
		float val4;
		if ((bool)abilityMod_FishManRoamingDebuff)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			val4 = abilityMod_FishManRoamingDebuff.m_jumpRadiusMod.GetModifiedValue(m_jumpRadius);
		}
		else
		{
			val4 = m_jumpRadius;
		}
		AddTokenFloatRounded(tokens, "JumpRadius_Rounded", empty4, val4);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (GetDamageToEnemyOnInitialHit() > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
