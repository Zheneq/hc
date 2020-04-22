using System.Collections.Generic;
using UnityEngine;

public class FishManBubbleLaser : Ability
{
	[Header("-- Targeting")]
	[Space(10f)]
	public LaserTargetingInfo m_laserInfo;

	[Header("-- Initial Hit")]
	public StandardEffectInfo m_effectOnAllies;

	public StandardEffectInfo m_effectOnEnemies;

	public int m_initialHitHealingToAllies;

	public int m_initialHitDamageToEnemies;

	[Header("-- Explosion Data")]
	public int m_numTurnsBeforeFirstExplosion = 1;

	public int m_numExplosionsBeforeEnding = 1;

	public AbilityAreaShape m_explosionShape;

	public bool m_explosionIgnoresLineOfSight;

	public bool m_explosionCanAffectEffectHolder;

	[Header("-- Explosion Results")]
	public int m_explosionHealingToAllies;

	public int m_explosionDamageToEnemies;

	public StandardEffectInfo m_explosionEffectToAllies;

	public StandardEffectInfo m_explosionEffectToEnemies;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_bubbleSequencePrefab;

	public GameObject m_explosionSequencePrefab;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardEffectInfo m_cachedEffectOnAllies;

	private StandardEffectInfo m_cachedEffectOnEnemies;

	private StandardEffectInfo m_cachedExplosionEffectToAllies;

	private StandardEffectInfo m_cachedExplosionEffectToEnemies;

	private AbilityMod_FishManBubbleLaser m_abilityMod;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Laser(this, m_cachedLaserInfo);
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
		StandardEffectInfo cachedEffectOnAllies;
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
			cachedEffectOnAllies = m_abilityMod.m_effectOnAlliesMod.GetModifiedValue(m_effectOnAllies);
		}
		else
		{
			cachedEffectOnAllies = m_effectOnAllies;
		}
		m_cachedEffectOnAllies = cachedEffectOnAllies;
		StandardEffectInfo cachedEffectOnEnemies;
		if ((bool)m_abilityMod)
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
			cachedEffectOnEnemies = m_abilityMod.m_effectOnEnemiesMod.GetModifiedValue(m_effectOnEnemies);
		}
		else
		{
			cachedEffectOnEnemies = m_effectOnEnemies;
		}
		m_cachedEffectOnEnemies = cachedEffectOnEnemies;
		m_cachedExplosionEffectToAllies = ((!m_abilityMod) ? m_explosionEffectToAllies : m_abilityMod.m_explosionEffectToAlliesMod.GetModifiedValue(m_explosionEffectToAllies));
		StandardEffectInfo cachedExplosionEffectToEnemies;
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
			cachedExplosionEffectToEnemies = m_abilityMod.m_explosionEffectToEnemiesMod.GetModifiedValue(m_explosionEffectToEnemies);
		}
		else
		{
			cachedExplosionEffectToEnemies = m_explosionEffectToEnemies;
		}
		m_cachedExplosionEffectToEnemies = cachedExplosionEffectToEnemies;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return (m_cachedLaserInfo == null) ? m_laserInfo : m_cachedLaserInfo;
	}

	public StandardEffectInfo GetEffectOnAllies()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnAllies != null)
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
			result = m_cachedEffectOnAllies;
		}
		else
		{
			result = m_effectOnAllies;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnEnemies != null)
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
			result = m_cachedEffectOnEnemies;
		}
		else
		{
			result = m_effectOnEnemies;
		}
		return result;
	}

	public int GetInitialHitHealingToAllies()
	{
		int result;
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
			result = m_abilityMod.m_initialHitHealingToAlliesMod.GetModifiedValue(m_initialHitHealingToAllies);
		}
		else
		{
			result = m_initialHitHealingToAllies;
		}
		return result;
	}

	public int GetInitialHitDamageToEnemies()
	{
		return (!m_abilityMod) ? m_initialHitDamageToEnemies : m_abilityMod.m_initialHitDamageToEnemiesMod.GetModifiedValue(m_initialHitDamageToEnemies);
	}

	public int GetNumTurnsBeforeFirstExplosion()
	{
		return (!m_abilityMod) ? m_numTurnsBeforeFirstExplosion : m_abilityMod.m_numTurnsBeforeFirstExplosionMod.GetModifiedValue(m_numTurnsBeforeFirstExplosion);
	}

	public int GetNumExplosionsBeforeEnding()
	{
		return (!m_abilityMod) ? m_numExplosionsBeforeEnding : m_abilityMod.m_numExplosionsBeforeEndingMod.GetModifiedValue(m_numExplosionsBeforeEnding);
	}

	public AbilityAreaShape GetExplosionShape()
	{
		AbilityAreaShape result;
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
			result = m_abilityMod.m_explosionShapeMod.GetModifiedValue(m_explosionShape);
		}
		else
		{
			result = m_explosionShape;
		}
		return result;
	}

	public bool ExplosionIgnoresLineOfSight()
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
			result = m_abilityMod.m_explosionIgnoresLineOfSightMod.GetModifiedValue(m_explosionIgnoresLineOfSight);
		}
		else
		{
			result = m_explosionIgnoresLineOfSight;
		}
		return result;
	}

	public bool ExplosionCanAffectEffectHolder()
	{
		bool result;
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
			result = m_abilityMod.m_explosionCanAffectEffectHolderMod.GetModifiedValue(m_explosionCanAffectEffectHolder);
		}
		else
		{
			result = m_explosionCanAffectEffectHolder;
		}
		return result;
	}

	public int GetExplosionHealingToAllies()
	{
		int result;
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
			result = m_abilityMod.m_explosionHealingToAlliesMod.GetModifiedValue(m_explosionHealingToAllies);
		}
		else
		{
			result = m_explosionHealingToAllies;
		}
		return result;
	}

	public int GetExplosionDamageToEnemies()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_explosionDamageToEnemiesMod.GetModifiedValue(m_explosionDamageToEnemies);
		}
		else
		{
			result = m_explosionDamageToEnemies;
		}
		return result;
	}

	public StandardEffectInfo GetExplosionEffectToAllies()
	{
		StandardEffectInfo result;
		if (m_cachedExplosionEffectToAllies != null)
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
			result = m_cachedExplosionEffectToAllies;
		}
		else
		{
			result = m_explosionEffectToAllies;
		}
		return result;
	}

	public StandardEffectInfo GetExplosionEffectToEnemies()
	{
		StandardEffectInfo result;
		if (m_cachedExplosionEffectToEnemies != null)
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
			result = m_cachedExplosionEffectToEnemies;
		}
		else
		{
			result = m_explosionEffectToEnemies;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManBubbleLaser))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_abilityMod = (abilityMod as AbilityMod_FishManBubbleLaser);
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
		AbilityMod_FishManBubbleLaser abilityMod_FishManBubbleLaser = modAsBase as AbilityMod_FishManBubbleLaser;
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_FishManBubbleLaser)
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
			effectInfo = abilityMod_FishManBubbleLaser.m_effectOnAlliesMod.GetModifiedValue(m_effectOnAllies);
		}
		else
		{
			effectInfo = m_effectOnAllies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnAllies", m_effectOnAllies);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_FishManBubbleLaser)
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
			effectInfo2 = abilityMod_FishManBubbleLaser.m_effectOnEnemiesMod.GetModifiedValue(m_effectOnEnemies);
		}
		else
		{
			effectInfo2 = m_effectOnEnemies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnEnemies", m_effectOnEnemies);
		AddTokenInt(tokens, "InitialHitHealingToAllies", string.Empty, (!abilityMod_FishManBubbleLaser) ? m_initialHitHealingToAllies : abilityMod_FishManBubbleLaser.m_initialHitHealingToAlliesMod.GetModifiedValue(m_initialHitHealingToAllies));
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_FishManBubbleLaser)
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
			val = abilityMod_FishManBubbleLaser.m_initialHitDamageToEnemiesMod.GetModifiedValue(m_initialHitDamageToEnemies);
		}
		else
		{
			val = m_initialHitDamageToEnemies;
		}
		AddTokenInt(tokens, "InitialHitDamageToEnemies", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_FishManBubbleLaser)
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
			val2 = abilityMod_FishManBubbleLaser.m_numTurnsBeforeFirstExplosionMod.GetModifiedValue(m_numTurnsBeforeFirstExplosion);
		}
		else
		{
			val2 = m_numTurnsBeforeFirstExplosion;
		}
		AddTokenInt(tokens, "NumTurnsBeforeFirstExplosion", empty2, val2);
		AddTokenInt(tokens, "NumExplosionsBeforeEnding", string.Empty, (!abilityMod_FishManBubbleLaser) ? m_numExplosionsBeforeEnding : abilityMod_FishManBubbleLaser.m_numExplosionsBeforeEndingMod.GetModifiedValue(m_numExplosionsBeforeEnding));
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_FishManBubbleLaser)
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
			val3 = abilityMod_FishManBubbleLaser.m_explosionHealingToAlliesMod.GetModifiedValue(m_explosionHealingToAllies);
		}
		else
		{
			val3 = m_explosionHealingToAllies;
		}
		AddTokenInt(tokens, "ExplosionHealingToAllies", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_FishManBubbleLaser)
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
			val4 = abilityMod_FishManBubbleLaser.m_explosionDamageToEnemiesMod.GetModifiedValue(m_explosionDamageToEnemies);
		}
		else
		{
			val4 = m_explosionDamageToEnemies;
		}
		AddTokenInt(tokens, "ExplosionDamageToEnemies", empty4, val4);
		StandardEffectInfo effectInfo3;
		if ((bool)abilityMod_FishManBubbleLaser)
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
			effectInfo3 = abilityMod_FishManBubbleLaser.m_explosionEffectToAlliesMod.GetModifiedValue(m_explosionEffectToAllies);
		}
		else
		{
			effectInfo3 = m_explosionEffectToAllies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "ExplosionEffectToAllies", m_explosionEffectToAllies);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_FishManBubbleLaser) ? m_explosionEffectToEnemies : abilityMod_FishManBubbleLaser.m_explosionEffectToEnemiesMod.GetModifiedValue(m_explosionEffectToEnemies), "ExplosionEffectToEnemies", m_explosionEffectToEnemies);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		if (GetInitialHitDamageToEnemies() > 0)
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
			AbilityTooltipHelper.ReportDamage(ref number, AbilityTooltipSubject.Enemy, GetInitialHitDamageToEnemies());
		}
		if (GetInitialHitHealingToAllies() > 0)
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
			AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Ally, GetInitialHitHealingToAllies());
		}
		GetEffectOnEnemies().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Enemy);
		GetEffectOnAllies().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Ally);
		return number;
	}
}
