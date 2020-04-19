using System;
using System.Collections.Generic;
using UnityEngine;

public class NanoSmithChainLightning : Ability
{
	[Separator("Laser/Primary Hit", true)]
	public int m_laserDamage = 0x14;

	public StandardEffectInfo m_laserEnemyHitEffect;

	public float m_laserRange = 5f;

	public float m_laserWidth = 1f;

	public bool m_penetrateLos;

	public int m_laserMaxHits = 1;

	[Separator("Chain Lightning", true)]
	public float m_chainRadius = 3f;

	public int m_chainMaxHits = -1;

	public int m_chainDamage = 0xA;

	public StandardEffectInfo m_chainEnemyHitEffect;

	public int m_energyGainPerChainHit;

	public bool m_chainCanHitInvisibleActors = true;

	[Separator("Extra Absob for Vacuum Bomb cast target", true)]
	public int m_extraAbsorbPerHitForVacuumBomb;

	public int m_maxExtraAbsorbForVacuumBomb = 0xA;

	[Header("-- Sequences")]
	public GameObject m_bounceLaserSequencePrefab;

	public GameObject m_selfHitSequencePrefab;

	private AbilityMod_NanoSmithChainLightning m_abilityMod;

	private StandardEffectInfo m_cachedLaserEnemyHitEffect;

	private StandardEffectInfo m_cachedChainEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Chain Lightning";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		base.ClearTargeters();
		if (this.GetExpectedNumberOfTargeters() < 2)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.SetupTargeter()).MethodHandle;
			}
			base.Targeter = new AbilityUtil_Targeter_ChainLightningLaser(this, this.GetLaserWidth(), this.GetLaserRange(), this.m_penetrateLos, this.GetLaserMaxTargets(), false, this.GetChainMaxHits(), this.GetChainRadius());
		}
		else
		{
			for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
			{
				base.Targeters.Add(new AbilityUtil_Targeter_ChainLightningLaser(this, this.GetLaserWidth(), this.GetLaserRange(), this.m_penetrateLos, this.GetLaserMaxTargets(), false, this.GetChainMaxHits(), this.GetChainRadius()));
				base.Targeters[i].SetUseMultiTargetUpdate(true);
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result = 1;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.GetExpectedNumberOfTargeters()).MethodHandle;
			}
			if (this.m_abilityMod.m_useTargetDataOverrides)
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
				if (this.m_abilityMod.m_targetDataOverrides.Length > 1)
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
					result = this.m_abilityMod.m_targetDataOverrides.Length;
				}
			}
		}
		return result;
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
		StandardEffectInfo cachedLaserEnemyHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.SetCachedFields()).MethodHandle;
			}
			cachedLaserEnemyHitEffect = this.m_abilityMod.m_laserEnemyHitEffectMod.GetModifiedValue(this.m_laserEnemyHitEffect);
		}
		else
		{
			cachedLaserEnemyHitEffect = this.m_laserEnemyHitEffect;
		}
		this.m_cachedLaserEnemyHitEffect = cachedLaserEnemyHitEffect;
		this.m_cachedChainEnemyHitEffect = ((!this.m_abilityMod) ? this.m_chainEnemyHitEffect : this.m_abilityMod.m_chainEnemyHitEffectMod.GetModifiedValue(this.m_chainEnemyHitEffect));
	}

	public int GetLaserDamage()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.GetLaserDamage()).MethodHandle;
			}
			result = this.m_laserDamage;
		}
		else
		{
			result = this.m_abilityMod.m_laserDamageMod.GetModifiedValue(this.m_laserDamage);
		}
		return result;
	}

	public StandardEffectInfo GetLaserEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedLaserEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.GetLaserEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedLaserEnemyHitEffect;
		}
		else
		{
			result = this.m_laserEnemyHitEffect;
		}
		return result;
	}

	public float GetLaserRange()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.GetLaserRange()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
		}
		else
		{
			result = this.m_laserRange;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		return (!this.m_abilityMod) ? this.m_laserWidth : this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
	}

	public bool PenetrateLos()
	{
		bool result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.PenetrateLos()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLosMod.GetModifiedValue(this.m_penetrateLos);
		}
		else
		{
			result = this.m_penetrateLos;
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
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.GetLaserMaxTargets()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserMaxHitsMod.GetModifiedValue(this.m_laserMaxHits);
		}
		else
		{
			result = this.m_laserMaxHits;
		}
		return result;
	}

	public float GetChainRadius()
	{
		float result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.GetChainRadius()).MethodHandle;
			}
			result = this.m_chainRadius;
		}
		else
		{
			result = this.m_abilityMod.m_chainRadiusMod.GetModifiedValue(this.m_chainRadius);
		}
		return result;
	}

	public int GetChainMaxHits()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.GetChainMaxHits()).MethodHandle;
			}
			result = this.m_chainMaxHits;
		}
		else
		{
			result = this.m_abilityMod.m_chainMaxHitsMod.GetModifiedValue(this.m_chainMaxHits);
		}
		return result;
	}

	public int GetChainDamage()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_chainDamageMod.GetModifiedValue(this.m_chainDamage) : this.m_chainDamage;
	}

	public int GetEnergyGainPerChainHit()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.GetEnergyGainPerChainHit()).MethodHandle;
			}
			result = this.m_energyGainPerChainHit;
		}
		else
		{
			result = this.m_abilityMod.m_energyPerChainHitMod.GetModifiedValue(this.m_energyGainPerChainHit);
		}
		return result;
	}

	public StandardEffectInfo GetChainEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedChainEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.GetChainEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedChainEnemyHitEffect;
		}
		else
		{
			result = this.m_chainEnemyHitEffect;
		}
		return result;
	}

	public bool ChainCanHitInvisibleActors()
	{
		return (!this.m_abilityMod) ? this.m_chainCanHitInvisibleActors : this.m_abilityMod.m_chainCanHitInvisibleActorsMod.GetModifiedValue(this.m_chainCanHitInvisibleActors);
	}

	public int GetExtraAbsorbPerHitForVacuumBomb()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.GetExtraAbsorbPerHitForVacuumBomb()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraAbsorbPerHitForVacuumBombMod.GetModifiedValue(this.m_extraAbsorbPerHitForVacuumBomb);
		}
		else
		{
			result = this.m_extraAbsorbPerHitForVacuumBomb;
		}
		return result;
	}

	public int GetMaxExtraAbsorbForVacuumBomb()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.GetMaxExtraAbsorbForVacuumBomb()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxExtraAbsorbForVacuumBombMod.GetModifiedValue(this.m_maxExtraAbsorbForVacuumBomb);
		}
		else
		{
			result = this.m_maxExtraAbsorbForVacuumBomb;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_laserDamage);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_chainDamage);
		this.m_laserEnemyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		this.m_chainEnemyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetLaserDamage());
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.GetChainDamage());
		return result;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.GetEnergyGainPerChainHit() > 0 && base.Targeters != null && currentTargeterIndex < base.Targeters.Count)
		{
			List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeters[currentTargeterIndex].GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Secondary);
			return this.GetEnergyGainPerChainHit() * visibleActorsInRangeByTooltipSubject.Count;
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithChainLightning abilityMod_NanoSmithChainLightning = modAsBase as AbilityMod_NanoSmithChainLightning;
		string name = "LaserDamage";
		string empty = string.Empty;
		int val;
		if (abilityMod_NanoSmithChainLightning)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_NanoSmithChainLightning.m_laserDamageMod.GetModifiedValue(this.m_laserDamage);
		}
		else
		{
			val = this.m_laserDamage;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_laserEnemyHitEffect, "LaserEnemyHitEffect", this.m_laserEnemyHitEffect, true);
		base.AddTokenInt(tokens, "LaserMaxHits", string.Empty, this.m_laserMaxHits, false);
		string name2 = "ChainMaxHits";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_NanoSmithChainLightning)
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
			val2 = abilityMod_NanoSmithChainLightning.m_chainMaxHitsMod.GetModifiedValue(this.m_chainMaxHits);
		}
		else
		{
			val2 = this.m_chainMaxHits;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "ChainDamage";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_NanoSmithChainLightning)
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
			val3 = abilityMod_NanoSmithChainLightning.m_chainDamageMod.GetModifiedValue(this.m_chainDamage);
		}
		else
		{
			val3 = this.m_chainDamage;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_chainEnemyHitEffect, "ChainEnemyHitEffect", this.m_chainEnemyHitEffect, true);
		string name4 = "EnergyGainPerChainHit";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_NanoSmithChainLightning)
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
			val4 = abilityMod_NanoSmithChainLightning.m_energyPerChainHitMod.GetModifiedValue(this.m_energyGainPerChainHit);
		}
		else
		{
			val4 = this.m_energyGainPerChainHit;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NanoSmithChainLightning))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithChainLightning.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_NanoSmithChainLightning);
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
