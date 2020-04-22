using System.Collections.Generic;
using UnityEngine;

public class FishManSplittingLaser : Ability
{
	[Header("-- Primary Laser")]
	public bool m_primaryLaserCanHitEnemies = true;

	public bool m_primaryLaserCanHitAllies;

	public int m_primaryTargetDamageAmount = 5;

	public int m_primaryTargetHealingAmount;

	public StandardEffectInfo m_primaryTargetEnemyHitEffect;

	public StandardEffectInfo m_primaryTargetAllyHitEffect;

	public LaserTargetingInfo m_primaryTargetingInfo;

	[Header("-- Secondary Lasers")]
	public bool m_secondaryLasersCanHitEnemies = true;

	public bool m_secondaryLasersCanHitAllies;

	public int m_secondaryTargetDamageAmount = 5;

	public int m_secondaryTargetHealingAmount;

	public StandardEffectInfo m_secondaryTargetEnemyHitEffect;

	public StandardEffectInfo m_secondaryTargetAllyHitEffect;

	public LaserTargetingInfo m_secondaryTargetingInfo;

	[Header("-- Split Data")]
	public bool m_alwaysSplit;

	public float m_minSplitAngle = 60f;

	public float m_maxSplitAngle = 120f;

	public float m_lengthForMinAngle = 3f;

	public float m_lengthForMaxAngle = 9f;

	public int m_numSplitBeamPairs = 1;

	[Header("-- Sequences")]
	public GameObject m_castSequence;

	public GameObject m_splitProjectileSequence;

	private StandardEffectInfo m_cachedPrimaryTargetEnemyHitEffect;

	private StandardEffectInfo m_cachedPrimaryTargetAllyHitEffect;

	private LaserTargetingInfo m_cachedPrimaryTargetingInfo;

	private StandardEffectInfo m_cachedSecondaryTargetEnemyHitEffect;

	private StandardEffectInfo m_cachedSecondaryTargetAllyHitEffect;

	private LaserTargetingInfo m_cachedSecondaryTargetingInfo;

	private AbilityMod_FishManSplittingLaser m_abilityMod;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_SplittingLaser(this, GetMinSplitAngle(), GetMaxSplitAngle(), GetLengthForMinAngle(), GetLengthForMaxAngle(), GetNumSplitBeamPairs(), AlwaysSplit(), GetPrimaryTargetingInfo().range, GetPrimaryTargetingInfo().width, GetPrimaryTargetingInfo().penetrateLos, GetPrimaryTargetingInfo().maxTargets, m_primaryLaserCanHitEnemies, m_primaryLaserCanHitAllies, GetSecondaryTargetingInfo().range, GetSecondaryTargetingInfo().width, GetSecondaryTargetingInfo().penetrateLos, GetSecondaryTargetingInfo().maxTargets, m_secondaryLasersCanHitEnemies, m_secondaryLasersCanHitAllies);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedPrimaryTargetEnemyHitEffect;
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
			cachedPrimaryTargetEnemyHitEffect = m_abilityMod.m_primaryTargetEnemyHitEffectMod.GetModifiedValue(m_primaryTargetEnemyHitEffect);
		}
		else
		{
			cachedPrimaryTargetEnemyHitEffect = m_primaryTargetEnemyHitEffect;
		}
		m_cachedPrimaryTargetEnemyHitEffect = cachedPrimaryTargetEnemyHitEffect;
		StandardEffectInfo cachedPrimaryTargetAllyHitEffect;
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
			cachedPrimaryTargetAllyHitEffect = m_abilityMod.m_primaryTargetAllyHitEffectMod.GetModifiedValue(m_primaryTargetAllyHitEffect);
		}
		else
		{
			cachedPrimaryTargetAllyHitEffect = m_primaryTargetAllyHitEffect;
		}
		m_cachedPrimaryTargetAllyHitEffect = cachedPrimaryTargetAllyHitEffect;
		m_cachedPrimaryTargetingInfo = ((!m_abilityMod) ? m_primaryTargetingInfo : m_abilityMod.m_primaryTargetingInfoMod.GetModifiedValue(m_primaryTargetingInfo));
		m_cachedSecondaryTargetEnemyHitEffect = ((!m_abilityMod) ? m_secondaryTargetEnemyHitEffect : m_abilityMod.m_secondaryTargetEnemyHitEffectMod.GetModifiedValue(m_secondaryTargetEnemyHitEffect));
		StandardEffectInfo cachedSecondaryTargetAllyHitEffect;
		if ((bool)m_abilityMod)
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
			cachedSecondaryTargetAllyHitEffect = m_abilityMod.m_secondaryTargetAllyHitEffectMod.GetModifiedValue(m_secondaryTargetAllyHitEffect);
		}
		else
		{
			cachedSecondaryTargetAllyHitEffect = m_secondaryTargetAllyHitEffect;
		}
		m_cachedSecondaryTargetAllyHitEffect = cachedSecondaryTargetAllyHitEffect;
		LaserTargetingInfo cachedSecondaryTargetingInfo;
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
			cachedSecondaryTargetingInfo = m_abilityMod.m_secondaryTargetingInfoMod.GetModifiedValue(m_secondaryTargetingInfo);
		}
		else
		{
			cachedSecondaryTargetingInfo = m_secondaryTargetingInfo;
		}
		m_cachedSecondaryTargetingInfo = cachedSecondaryTargetingInfo;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_FishManSplittingLaser))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_FishManSplittingLaser);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	public bool PrimaryLaserCanHitEnemies()
	{
		return (!m_abilityMod) ? m_primaryLaserCanHitEnemies : m_abilityMod.m_primaryLaserCanHitEnemiesMod.GetModifiedValue(m_primaryLaserCanHitEnemies);
	}

	public bool PrimaryLaserCanHitAllies()
	{
		bool result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_primaryLaserCanHitAlliesMod.GetModifiedValue(m_primaryLaserCanHitAllies);
		}
		else
		{
			result = m_primaryLaserCanHitAllies;
		}
		return result;
	}

	public int GetPrimaryTargetDamageAmount()
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
			result = m_abilityMod.m_primaryTargetDamageAmountMod.GetModifiedValue(m_primaryTargetDamageAmount);
		}
		else
		{
			result = m_primaryTargetDamageAmount;
		}
		return result;
	}

	public int GetPrimaryTargetHealingAmount()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_primaryTargetHealingAmountMod.GetModifiedValue(m_primaryTargetHealingAmount);
		}
		else
		{
			result = m_primaryTargetHealingAmount;
		}
		return result;
	}

	public StandardEffectInfo GetPrimaryTargetEnemyHitEffect()
	{
		return (m_cachedPrimaryTargetEnemyHitEffect == null) ? m_primaryTargetEnemyHitEffect : m_cachedPrimaryTargetEnemyHitEffect;
	}

	public StandardEffectInfo GetPrimaryTargetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedPrimaryTargetAllyHitEffect != null)
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
			result = m_cachedPrimaryTargetAllyHitEffect;
		}
		else
		{
			result = m_primaryTargetAllyHitEffect;
		}
		return result;
	}

	public LaserTargetingInfo GetPrimaryTargetingInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedPrimaryTargetingInfo != null)
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
			result = m_cachedPrimaryTargetingInfo;
		}
		else
		{
			result = m_primaryTargetingInfo;
		}
		return result;
	}

	public bool SecondaryLasersCanHitEnemies()
	{
		return (!m_abilityMod) ? m_secondaryLasersCanHitEnemies : m_abilityMod.m_secondaryLasersCanHitEnemiesMod.GetModifiedValue(m_secondaryLasersCanHitEnemies);
	}

	public bool SecondaryLasersCanHitAllies()
	{
		return (!m_abilityMod) ? m_secondaryLasersCanHitAllies : m_abilityMod.m_secondaryLasersCanHitAlliesMod.GetModifiedValue(m_secondaryLasersCanHitAllies);
	}

	public int GetSecondaryTargetDamageAmount()
	{
		return (!m_abilityMod) ? m_secondaryTargetDamageAmount : m_abilityMod.m_secondaryTargetDamageAmountMod.GetModifiedValue(m_secondaryTargetDamageAmount);
	}

	public int GetSecondaryTargetHealingAmount()
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
			result = m_abilityMod.m_secondaryTargetHealingAmountMod.GetModifiedValue(m_secondaryTargetHealingAmount);
		}
		else
		{
			result = m_secondaryTargetHealingAmount;
		}
		return result;
	}

	public StandardEffectInfo GetSecondaryTargetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedSecondaryTargetEnemyHitEffect != null)
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
			result = m_cachedSecondaryTargetEnemyHitEffect;
		}
		else
		{
			result = m_secondaryTargetEnemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetSecondaryTargetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedSecondaryTargetAllyHitEffect != null)
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
			result = m_cachedSecondaryTargetAllyHitEffect;
		}
		else
		{
			result = m_secondaryTargetAllyHitEffect;
		}
		return result;
	}

	public LaserTargetingInfo GetSecondaryTargetingInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedSecondaryTargetingInfo != null)
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
			result = m_cachedSecondaryTargetingInfo;
		}
		else
		{
			result = m_secondaryTargetingInfo;
		}
		return result;
	}

	public bool AlwaysSplit()
	{
		return (!m_abilityMod) ? m_alwaysSplit : m_abilityMod.m_alwaysSplitMod.GetModifiedValue(m_alwaysSplit);
	}

	public float GetMinSplitAngle()
	{
		return (!m_abilityMod) ? m_minSplitAngle : m_abilityMod.m_minSplitAngleMod.GetModifiedValue(m_minSplitAngle);
	}

	public float GetMaxSplitAngle()
	{
		float result;
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
			result = m_abilityMod.m_maxSplitAngleMod.GetModifiedValue(m_maxSplitAngle);
		}
		else
		{
			result = m_maxSplitAngle;
		}
		return result;
	}

	public float GetLengthForMinAngle()
	{
		return (!m_abilityMod) ? m_lengthForMinAngle : m_abilityMod.m_lengthForMinAngleMod.GetModifiedValue(m_lengthForMinAngle);
	}

	public float GetLengthForMaxAngle()
	{
		float result;
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
			result = m_abilityMod.m_lengthForMaxAngleMod.GetModifiedValue(m_lengthForMaxAngle);
		}
		else
		{
			result = m_lengthForMaxAngle;
		}
		return result;
	}

	public int GetNumSplitBeamPairs()
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
			result = m_abilityMod.m_numSplitBeamPairsMod.GetModifiedValue(m_numSplitBeamPairs);
		}
		else
		{
			result = m_numSplitBeamPairs;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManSplittingLaser abilityMod_FishManSplittingLaser = modAsBase as AbilityMod_FishManSplittingLaser;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_FishManSplittingLaser)
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
			val = abilityMod_FishManSplittingLaser.m_primaryTargetDamageAmountMod.GetModifiedValue(m_primaryTargetDamageAmount);
		}
		else
		{
			val = m_primaryTargetDamageAmount;
		}
		AddTokenInt(tokens, "PrimaryTargetDamageAmount", empty, val);
		AddTokenInt(tokens, "PrimaryTargetHealingAmount", string.Empty, (!abilityMod_FishManSplittingLaser) ? m_primaryTargetHealingAmount : abilityMod_FishManSplittingLaser.m_primaryTargetHealingAmountMod.GetModifiedValue(m_primaryTargetHealingAmount));
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_FishManSplittingLaser)
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
			effectInfo = abilityMod_FishManSplittingLaser.m_primaryTargetEnemyHitEffectMod.GetModifiedValue(m_primaryTargetEnemyHitEffect);
		}
		else
		{
			effectInfo = m_primaryTargetEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "PrimaryTargetEnemyHitEffect", m_primaryTargetEnemyHitEffect);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_FishManSplittingLaser)
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
			effectInfo2 = abilityMod_FishManSplittingLaser.m_primaryTargetAllyHitEffectMod.GetModifiedValue(m_primaryTargetAllyHitEffect);
		}
		else
		{
			effectInfo2 = m_primaryTargetAllyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "PrimaryTargetAllyHitEffect", m_primaryTargetAllyHitEffect);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_FishManSplittingLaser)
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
			val2 = abilityMod_FishManSplittingLaser.m_secondaryTargetDamageAmountMod.GetModifiedValue(m_secondaryTargetDamageAmount);
		}
		else
		{
			val2 = m_secondaryTargetDamageAmount;
		}
		AddTokenInt(tokens, "SecondaryTargetDamageAmount", empty2, val2);
		AddTokenInt(tokens, "SecondaryTargetHealingAmount", string.Empty, (!abilityMod_FishManSplittingLaser) ? m_secondaryTargetHealingAmount : abilityMod_FishManSplittingLaser.m_secondaryTargetHealingAmountMod.GetModifiedValue(m_secondaryTargetHealingAmount));
		StandardEffectInfo effectInfo3;
		if ((bool)abilityMod_FishManSplittingLaser)
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
			effectInfo3 = abilityMod_FishManSplittingLaser.m_secondaryTargetEnemyHitEffectMod.GetModifiedValue(m_secondaryTargetEnemyHitEffect);
		}
		else
		{
			effectInfo3 = m_secondaryTargetEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "SecondaryTargetEnemyHitEffect", m_secondaryTargetEnemyHitEffect);
		StandardEffectInfo effectInfo4;
		if ((bool)abilityMod_FishManSplittingLaser)
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
			effectInfo4 = abilityMod_FishManSplittingLaser.m_secondaryTargetAllyHitEffectMod.GetModifiedValue(m_secondaryTargetAllyHitEffect);
		}
		else
		{
			effectInfo4 = m_secondaryTargetAllyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo4, "SecondaryTargetAllyHitEffect", m_secondaryTargetAllyHitEffect);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_FishManSplittingLaser)
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
			val3 = abilityMod_FishManSplittingLaser.m_numSplitBeamPairsMod.GetModifiedValue(m_numSplitBeamPairs);
		}
		else
		{
			val3 = m_numSplitBeamPairs;
		}
		AddTokenInt(tokens, "NumSplitBeamPairs", empty3, val3);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetPrimaryTargetDamageAmount()));
		GetPrimaryTargetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, GetSecondaryTargetDamageAmount()));
		GetSecondaryTargetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Tertiary, GetPrimaryTargetHealingAmount()));
		GetPrimaryTargetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Tertiary);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Quaternary, GetSecondaryTargetHealingAmount()));
		GetSecondaryTargetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Quaternary);
		return numbers;
	}
}
