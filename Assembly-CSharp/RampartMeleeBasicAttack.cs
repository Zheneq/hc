using System;
using System.Collections.Generic;
using UnityEngine;

public class RampartMeleeBasicAttack : Ability
{
	[Header("-- Laser Targeting")]
	public float m_laserRange = 4f;

	public float m_laserWidth = 1f;

	public int m_laserMaxTargets;

	public bool m_penetrateLos;

	[Header("-- Cone Targeting")]
	public float m_coneWidthAngle = 90f;

	public float m_coneRange = 2.5f;

	[Header("-- Hit Damage/Effects")]
	public int m_laserDamage = 0x14;

	public StandardEffectInfo m_laserEnemyHitEffect;

	public int m_coneDamage = 0xA;

	public StandardEffectInfo m_coneEnemyHitEffect;

	public int m_bonusDamageForOverlap;

	[Header("-- Sequences")]
	public GameObject m_laserSequencePrefab;

	public GameObject m_coneSequencePrefab;

	private AbilityMod_RampartMeleeBasicAttack m_abilityMod;

	private StandardEffectInfo m_cachedLaserEnemyHitEffect;

	private StandardEffectInfo m_cachedConeEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Overhead Slam";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_ClaymoreSlam(this, this.GetLaserRange(), this.GetLaserWidth(), this.GetLaserMaxTargets(), this.GetConeAngle(), this.GetConeRange(), 0f, this.PenetrateLos(), true, false, false, this.GetBonusDamageForOverlap() > 0);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserRange();
	}

	private void SetCachedFields()
	{
		this.m_cachedLaserEnemyHitEffect = ((!this.m_abilityMod) ? this.m_laserEnemyHitEffect : this.m_abilityMod.m_laserEnemyHitEffectMod.GetModifiedValue(this.m_laserEnemyHitEffect));
		StandardEffectInfo cachedConeEnemyHitEffect;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartMeleeBasicAttack.SetCachedFields()).MethodHandle;
			}
			cachedConeEnemyHitEffect = this.m_abilityMod.m_coneEnemyHitEffectMod.GetModifiedValue(this.m_coneEnemyHitEffect);
		}
		else
		{
			cachedConeEnemyHitEffect = this.m_coneEnemyHitEffect;
		}
		this.m_cachedConeEnemyHitEffect = cachedConeEnemyHitEffect;
	}

	public float GetLaserRange()
	{
		return (!this.m_abilityMod) ? this.m_laserRange : this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
	}

	public float GetLaserWidth()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartMeleeBasicAttack.GetLaserWidth()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
		}
		else
		{
			result = this.m_laserWidth;
		}
		return result;
	}

	public int GetLaserMaxTargets()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartMeleeBasicAttack.GetLaserMaxTargets()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(this.m_laserMaxTargets);
		}
		else
		{
			result = this.m_laserMaxTargets;
		}
		return result;
	}

	public bool PenetrateLos()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartMeleeBasicAttack.PenetrateLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
		}
		else
		{
			result = this.m_penetrateLos;
		}
		return result;
	}

	public float GetConeAngle()
	{
		return (!this.m_abilityMod) ? this.m_coneWidthAngle : this.m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(this.m_coneWidthAngle);
	}

	public float GetConeRange()
	{
		return (!this.m_abilityMod) ? this.m_coneRange : this.m_abilityMod.m_coneRangeMod.GetModifiedValue(this.m_coneRange);
	}

	public int GetLaserDamage()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartMeleeBasicAttack.GetLaserDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserDamageMod.GetModifiedValue(this.m_laserDamage);
		}
		else
		{
			result = this.m_laserDamage;
		}
		return result;
	}

	public StandardEffectInfo GetLaserEnemyHitEffect()
	{
		return (this.m_cachedLaserEnemyHitEffect == null) ? this.m_laserEnemyHitEffect : this.m_cachedLaserEnemyHitEffect;
	}

	public int GetConeDamage()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartMeleeBasicAttack.GetConeDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneDamageMod.GetModifiedValue(this.m_coneDamage);
		}
		else
		{
			result = this.m_coneDamage;
		}
		return result;
	}

	public StandardEffectInfo GetConeEnemyHitEffect()
	{
		return (this.m_cachedConeEnemyHitEffect == null) ? this.m_coneEnemyHitEffect : this.m_cachedConeEnemyHitEffect;
	}

	public int GetBonusDamageForOverlap()
	{
		return (!this.m_abilityMod) ? this.m_bonusDamageForOverlap : this.m_abilityMod.m_bonusDamageForOverlapMod.GetModifiedValue(this.m_bonusDamageForOverlap);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartMeleeBasicAttack))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartMeleeBasicAttack.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_RampartMeleeBasicAttack);
		}
		this.SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetLaserDamage());
		this.m_laserEnemyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartMeleeBasicAttack.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			bool flag = tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary);
			bool flag2 = tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary);
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (flag)
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
				if (flag2)
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
					dictionary[AbilityTooltipSymbol.Damage] = this.GetLaserDamage() + this.GetBonusDamageForOverlap();
					return dictionary;
				}
			}
			if (flag)
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
				dictionary[AbilityTooltipSymbol.Damage] = this.GetLaserDamage();
			}
			else if (flag2)
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
				dictionary[AbilityTooltipSymbol.Damage] = this.GetConeDamage();
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartMeleeBasicAttack abilityMod_RampartMeleeBasicAttack = modAsBase as AbilityMod_RampartMeleeBasicAttack;
		string name = "LaserMaxTargets";
		string empty = string.Empty;
		int val;
		if (abilityMod_RampartMeleeBasicAttack)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartMeleeBasicAttack.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_RampartMeleeBasicAttack.m_laserMaxTargetsMod.GetModifiedValue(this.m_laserMaxTargets);
		}
		else
		{
			val = this.m_laserMaxTargets;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "LaserDamage";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_RampartMeleeBasicAttack)
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
			val2 = abilityMod_RampartMeleeBasicAttack.m_laserDamageMod.GetModifiedValue(this.m_laserDamage);
		}
		else
		{
			val2 = this.m_laserDamage;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_RampartMeleeBasicAttack)
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
			effectInfo = abilityMod_RampartMeleeBasicAttack.m_laserEnemyHitEffectMod.GetModifiedValue(this.m_laserEnemyHitEffect);
		}
		else
		{
			effectInfo = this.m_laserEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "LaserEnemyHitEffect", this.m_laserEnemyHitEffect, true);
		string name3 = "ConeDamage";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_RampartMeleeBasicAttack)
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
			val3 = abilityMod_RampartMeleeBasicAttack.m_coneDamageMod.GetModifiedValue(this.m_coneDamage);
		}
		else
		{
			val3 = this.m_coneDamage;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_RampartMeleeBasicAttack)
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
			effectInfo2 = abilityMod_RampartMeleeBasicAttack.m_coneEnemyHitEffectMod.GetModifiedValue(this.m_coneEnemyHitEffect);
		}
		else
		{
			effectInfo2 = this.m_coneEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "ConeEnemyHitEffect", this.m_coneEnemyHitEffect, true);
	}
}
