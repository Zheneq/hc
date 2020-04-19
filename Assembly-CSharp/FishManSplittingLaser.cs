using System;
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
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_SplittingLaser(this, this.GetMinSplitAngle(), this.GetMaxSplitAngle(), this.GetLengthForMinAngle(), this.GetLengthForMaxAngle(), this.GetNumSplitBeamPairs(), this.AlwaysSplit(), this.GetPrimaryTargetingInfo().range, this.GetPrimaryTargetingInfo().width, this.GetPrimaryTargetingInfo().penetrateLos, this.GetPrimaryTargetingInfo().maxTargets, this.m_primaryLaserCanHitEnemies, this.m_primaryLaserCanHitAllies, this.GetSecondaryTargetingInfo().range, this.GetSecondaryTargetingInfo().width, this.GetSecondaryTargetingInfo().penetrateLos, this.GetSecondaryTargetingInfo().maxTargets, this.m_secondaryLasersCanHitEnemies, this.m_secondaryLasersCanHitAllies);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedPrimaryTargetEnemyHitEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.SetCachedFields()).MethodHandle;
			}
			cachedPrimaryTargetEnemyHitEffect = this.m_abilityMod.m_primaryTargetEnemyHitEffectMod.GetModifiedValue(this.m_primaryTargetEnemyHitEffect);
		}
		else
		{
			cachedPrimaryTargetEnemyHitEffect = this.m_primaryTargetEnemyHitEffect;
		}
		this.m_cachedPrimaryTargetEnemyHitEffect = cachedPrimaryTargetEnemyHitEffect;
		StandardEffectInfo cachedPrimaryTargetAllyHitEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedPrimaryTargetAllyHitEffect = this.m_abilityMod.m_primaryTargetAllyHitEffectMod.GetModifiedValue(this.m_primaryTargetAllyHitEffect);
		}
		else
		{
			cachedPrimaryTargetAllyHitEffect = this.m_primaryTargetAllyHitEffect;
		}
		this.m_cachedPrimaryTargetAllyHitEffect = cachedPrimaryTargetAllyHitEffect;
		this.m_cachedPrimaryTargetingInfo = ((!this.m_abilityMod) ? this.m_primaryTargetingInfo : this.m_abilityMod.m_primaryTargetingInfoMod.GetModifiedValue(this.m_primaryTargetingInfo));
		this.m_cachedSecondaryTargetEnemyHitEffect = ((!this.m_abilityMod) ? this.m_secondaryTargetEnemyHitEffect : this.m_abilityMod.m_secondaryTargetEnemyHitEffectMod.GetModifiedValue(this.m_secondaryTargetEnemyHitEffect));
		StandardEffectInfo cachedSecondaryTargetAllyHitEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedSecondaryTargetAllyHitEffect = this.m_abilityMod.m_secondaryTargetAllyHitEffectMod.GetModifiedValue(this.m_secondaryTargetAllyHitEffect);
		}
		else
		{
			cachedSecondaryTargetAllyHitEffect = this.m_secondaryTargetAllyHitEffect;
		}
		this.m_cachedSecondaryTargetAllyHitEffect = cachedSecondaryTargetAllyHitEffect;
		LaserTargetingInfo cachedSecondaryTargetingInfo;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedSecondaryTargetingInfo = this.m_abilityMod.m_secondaryTargetingInfoMod.GetModifiedValue(this.m_secondaryTargetingInfo);
		}
		else
		{
			cachedSecondaryTargetingInfo = this.m_secondaryTargetingInfo;
		}
		this.m_cachedSecondaryTargetingInfo = cachedSecondaryTargetingInfo;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManSplittingLaser))
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_FishManSplittingLaser);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	public bool PrimaryLaserCanHitEnemies()
	{
		return (!this.m_abilityMod) ? this.m_primaryLaserCanHitEnemies : this.m_abilityMod.m_primaryLaserCanHitEnemiesMod.GetModifiedValue(this.m_primaryLaserCanHitEnemies);
	}

	public bool PrimaryLaserCanHitAllies()
	{
		bool result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.PrimaryLaserCanHitAllies()).MethodHandle;
			}
			result = this.m_abilityMod.m_primaryLaserCanHitAlliesMod.GetModifiedValue(this.m_primaryLaserCanHitAllies);
		}
		else
		{
			result = this.m_primaryLaserCanHitAllies;
		}
		return result;
	}

	public int GetPrimaryTargetDamageAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.GetPrimaryTargetDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_primaryTargetDamageAmountMod.GetModifiedValue(this.m_primaryTargetDamageAmount);
		}
		else
		{
			result = this.m_primaryTargetDamageAmount;
		}
		return result;
	}

	public int GetPrimaryTargetHealingAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.GetPrimaryTargetHealingAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_primaryTargetHealingAmountMod.GetModifiedValue(this.m_primaryTargetHealingAmount);
		}
		else
		{
			result = this.m_primaryTargetHealingAmount;
		}
		return result;
	}

	public StandardEffectInfo GetPrimaryTargetEnemyHitEffect()
	{
		return (this.m_cachedPrimaryTargetEnemyHitEffect == null) ? this.m_primaryTargetEnemyHitEffect : this.m_cachedPrimaryTargetEnemyHitEffect;
	}

	public StandardEffectInfo GetPrimaryTargetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedPrimaryTargetAllyHitEffect != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.GetPrimaryTargetAllyHitEffect()).MethodHandle;
			}
			result = this.m_cachedPrimaryTargetAllyHitEffect;
		}
		else
		{
			result = this.m_primaryTargetAllyHitEffect;
		}
		return result;
	}

	public LaserTargetingInfo GetPrimaryTargetingInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedPrimaryTargetingInfo != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.GetPrimaryTargetingInfo()).MethodHandle;
			}
			result = this.m_cachedPrimaryTargetingInfo;
		}
		else
		{
			result = this.m_primaryTargetingInfo;
		}
		return result;
	}

	public bool SecondaryLasersCanHitEnemies()
	{
		return (!this.m_abilityMod) ? this.m_secondaryLasersCanHitEnemies : this.m_abilityMod.m_secondaryLasersCanHitEnemiesMod.GetModifiedValue(this.m_secondaryLasersCanHitEnemies);
	}

	public bool SecondaryLasersCanHitAllies()
	{
		return (!this.m_abilityMod) ? this.m_secondaryLasersCanHitAllies : this.m_abilityMod.m_secondaryLasersCanHitAlliesMod.GetModifiedValue(this.m_secondaryLasersCanHitAllies);
	}

	public int GetSecondaryTargetDamageAmount()
	{
		return (!this.m_abilityMod) ? this.m_secondaryTargetDamageAmount : this.m_abilityMod.m_secondaryTargetDamageAmountMod.GetModifiedValue(this.m_secondaryTargetDamageAmount);
	}

	public int GetSecondaryTargetHealingAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.GetSecondaryTargetHealingAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_secondaryTargetHealingAmountMod.GetModifiedValue(this.m_secondaryTargetHealingAmount);
		}
		else
		{
			result = this.m_secondaryTargetHealingAmount;
		}
		return result;
	}

	public StandardEffectInfo GetSecondaryTargetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedSecondaryTargetEnemyHitEffect != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.GetSecondaryTargetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedSecondaryTargetEnemyHitEffect;
		}
		else
		{
			result = this.m_secondaryTargetEnemyHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetSecondaryTargetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedSecondaryTargetAllyHitEffect != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.GetSecondaryTargetAllyHitEffect()).MethodHandle;
			}
			result = this.m_cachedSecondaryTargetAllyHitEffect;
		}
		else
		{
			result = this.m_secondaryTargetAllyHitEffect;
		}
		return result;
	}

	public LaserTargetingInfo GetSecondaryTargetingInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedSecondaryTargetingInfo != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.GetSecondaryTargetingInfo()).MethodHandle;
			}
			result = this.m_cachedSecondaryTargetingInfo;
		}
		else
		{
			result = this.m_secondaryTargetingInfo;
		}
		return result;
	}

	public bool AlwaysSplit()
	{
		return (!this.m_abilityMod) ? this.m_alwaysSplit : this.m_abilityMod.m_alwaysSplitMod.GetModifiedValue(this.m_alwaysSplit);
	}

	public float GetMinSplitAngle()
	{
		return (!this.m_abilityMod) ? this.m_minSplitAngle : this.m_abilityMod.m_minSplitAngleMod.GetModifiedValue(this.m_minSplitAngle);
	}

	public float GetMaxSplitAngle()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.GetMaxSplitAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxSplitAngleMod.GetModifiedValue(this.m_maxSplitAngle);
		}
		else
		{
			result = this.m_maxSplitAngle;
		}
		return result;
	}

	public float GetLengthForMinAngle()
	{
		return (!this.m_abilityMod) ? this.m_lengthForMinAngle : this.m_abilityMod.m_lengthForMinAngleMod.GetModifiedValue(this.m_lengthForMinAngle);
	}

	public float GetLengthForMaxAngle()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.GetLengthForMaxAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_lengthForMaxAngleMod.GetModifiedValue(this.m_lengthForMaxAngle);
		}
		else
		{
			result = this.m_lengthForMaxAngle;
		}
		return result;
	}

	public int GetNumSplitBeamPairs()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.GetNumSplitBeamPairs()).MethodHandle;
			}
			result = this.m_abilityMod.m_numSplitBeamPairsMod.GetModifiedValue(this.m_numSplitBeamPairs);
		}
		else
		{
			result = this.m_numSplitBeamPairs;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManSplittingLaser abilityMod_FishManSplittingLaser = modAsBase as AbilityMod_FishManSplittingLaser;
		string name = "PrimaryTargetDamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_FishManSplittingLaser)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManSplittingLaser.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_FishManSplittingLaser.m_primaryTargetDamageAmountMod.GetModifiedValue(this.m_primaryTargetDamageAmount);
		}
		else
		{
			val = this.m_primaryTargetDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "PrimaryTargetHealingAmount", string.Empty, (!abilityMod_FishManSplittingLaser) ? this.m_primaryTargetHealingAmount : abilityMod_FishManSplittingLaser.m_primaryTargetHealingAmountMod.GetModifiedValue(this.m_primaryTargetHealingAmount), false);
		StandardEffectInfo effectInfo;
		if (abilityMod_FishManSplittingLaser)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo = abilityMod_FishManSplittingLaser.m_primaryTargetEnemyHitEffectMod.GetModifiedValue(this.m_primaryTargetEnemyHitEffect);
		}
		else
		{
			effectInfo = this.m_primaryTargetEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "PrimaryTargetEnemyHitEffect", this.m_primaryTargetEnemyHitEffect, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_FishManSplittingLaser)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo2 = abilityMod_FishManSplittingLaser.m_primaryTargetAllyHitEffectMod.GetModifiedValue(this.m_primaryTargetAllyHitEffect);
		}
		else
		{
			effectInfo2 = this.m_primaryTargetAllyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "PrimaryTargetAllyHitEffect", this.m_primaryTargetAllyHitEffect, true);
		string name2 = "SecondaryTargetDamageAmount";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_FishManSplittingLaser)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = abilityMod_FishManSplittingLaser.m_secondaryTargetDamageAmountMod.GetModifiedValue(this.m_secondaryTargetDamageAmount);
		}
		else
		{
			val2 = this.m_secondaryTargetDamageAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		base.AddTokenInt(tokens, "SecondaryTargetHealingAmount", string.Empty, (!abilityMod_FishManSplittingLaser) ? this.m_secondaryTargetHealingAmount : abilityMod_FishManSplittingLaser.m_secondaryTargetHealingAmountMod.GetModifiedValue(this.m_secondaryTargetHealingAmount), false);
		StandardEffectInfo effectInfo3;
		if (abilityMod_FishManSplittingLaser)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo3 = abilityMod_FishManSplittingLaser.m_secondaryTargetEnemyHitEffectMod.GetModifiedValue(this.m_secondaryTargetEnemyHitEffect);
		}
		else
		{
			effectInfo3 = this.m_secondaryTargetEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "SecondaryTargetEnemyHitEffect", this.m_secondaryTargetEnemyHitEffect, true);
		StandardEffectInfo effectInfo4;
		if (abilityMod_FishManSplittingLaser)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo4 = abilityMod_FishManSplittingLaser.m_secondaryTargetAllyHitEffectMod.GetModifiedValue(this.m_secondaryTargetAllyHitEffect);
		}
		else
		{
			effectInfo4 = this.m_secondaryTargetAllyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo4, "SecondaryTargetAllyHitEffect", this.m_secondaryTargetAllyHitEffect, true);
		string name3 = "NumSplitBeamPairs";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_FishManSplittingLaser)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			val3 = abilityMod_FishManSplittingLaser.m_numSplitBeamPairsMod.GetModifiedValue(this.m_numSplitBeamPairs);
		}
		else
		{
			val3 = this.m_numSplitBeamPairs;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.GetPrimaryTargetDamageAmount()));
		this.GetPrimaryTargetEnemyHitEffect().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Primary);
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, this.GetSecondaryTargetDamageAmount()));
		this.GetSecondaryTargetEnemyHitEffect().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Secondary);
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Tertiary, this.GetPrimaryTargetHealingAmount()));
		this.GetPrimaryTargetAllyHitEffect().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Tertiary);
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Quaternary, this.GetSecondaryTargetHealingAmount()));
		this.GetSecondaryTargetAllyHitEffect().ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Quaternary);
		return list;
	}
}
