using System;
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
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Laser(this, this.m_cachedLaserInfo);
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo cachedLaserInfo;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.SetCachedFields()).MethodHandle;
			}
			cachedLaserInfo = this.m_abilityMod.m_laserInfoMod.GetModifiedValue(this.m_laserInfo);
		}
		else
		{
			cachedLaserInfo = this.m_laserInfo;
		}
		this.m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedEffectOnAllies;
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
			cachedEffectOnAllies = this.m_abilityMod.m_effectOnAlliesMod.GetModifiedValue(this.m_effectOnAllies);
		}
		else
		{
			cachedEffectOnAllies = this.m_effectOnAllies;
		}
		this.m_cachedEffectOnAllies = cachedEffectOnAllies;
		StandardEffectInfo cachedEffectOnEnemies;
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
			cachedEffectOnEnemies = this.m_abilityMod.m_effectOnEnemiesMod.GetModifiedValue(this.m_effectOnEnemies);
		}
		else
		{
			cachedEffectOnEnemies = this.m_effectOnEnemies;
		}
		this.m_cachedEffectOnEnemies = cachedEffectOnEnemies;
		this.m_cachedExplosionEffectToAllies = ((!this.m_abilityMod) ? this.m_explosionEffectToAllies : this.m_abilityMod.m_explosionEffectToAlliesMod.GetModifiedValue(this.m_explosionEffectToAllies));
		StandardEffectInfo cachedExplosionEffectToEnemies;
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
			cachedExplosionEffectToEnemies = this.m_abilityMod.m_explosionEffectToEnemiesMod.GetModifiedValue(this.m_explosionEffectToEnemies);
		}
		else
		{
			cachedExplosionEffectToEnemies = this.m_explosionEffectToEnemies;
		}
		this.m_cachedExplosionEffectToEnemies = cachedExplosionEffectToEnemies;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return (this.m_cachedLaserInfo == null) ? this.m_laserInfo : this.m_cachedLaserInfo;
	}

	public StandardEffectInfo GetEffectOnAllies()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnAllies != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.GetEffectOnAllies()).MethodHandle;
			}
			result = this.m_cachedEffectOnAllies;
		}
		else
		{
			result = this.m_effectOnAllies;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnEnemies != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.GetEffectOnEnemies()).MethodHandle;
			}
			result = this.m_cachedEffectOnEnemies;
		}
		else
		{
			result = this.m_effectOnEnemies;
		}
		return result;
	}

	public int GetInitialHitHealingToAllies()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.GetInitialHitHealingToAllies()).MethodHandle;
			}
			result = this.m_abilityMod.m_initialHitHealingToAlliesMod.GetModifiedValue(this.m_initialHitHealingToAllies);
		}
		else
		{
			result = this.m_initialHitHealingToAllies;
		}
		return result;
	}

	public int GetInitialHitDamageToEnemies()
	{
		return (!this.m_abilityMod) ? this.m_initialHitDamageToEnemies : this.m_abilityMod.m_initialHitDamageToEnemiesMod.GetModifiedValue(this.m_initialHitDamageToEnemies);
	}

	public int GetNumTurnsBeforeFirstExplosion()
	{
		return (!this.m_abilityMod) ? this.m_numTurnsBeforeFirstExplosion : this.m_abilityMod.m_numTurnsBeforeFirstExplosionMod.GetModifiedValue(this.m_numTurnsBeforeFirstExplosion);
	}

	public int GetNumExplosionsBeforeEnding()
	{
		return (!this.m_abilityMod) ? this.m_numExplosionsBeforeEnding : this.m_abilityMod.m_numExplosionsBeforeEndingMod.GetModifiedValue(this.m_numExplosionsBeforeEnding);
	}

	public AbilityAreaShape GetExplosionShape()
	{
		AbilityAreaShape result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.GetExplosionShape()).MethodHandle;
			}
			result = this.m_abilityMod.m_explosionShapeMod.GetModifiedValue(this.m_explosionShape);
		}
		else
		{
			result = this.m_explosionShape;
		}
		return result;
	}

	public bool ExplosionIgnoresLineOfSight()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.ExplosionIgnoresLineOfSight()).MethodHandle;
			}
			result = this.m_abilityMod.m_explosionIgnoresLineOfSightMod.GetModifiedValue(this.m_explosionIgnoresLineOfSight);
		}
		else
		{
			result = this.m_explosionIgnoresLineOfSight;
		}
		return result;
	}

	public bool ExplosionCanAffectEffectHolder()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.ExplosionCanAffectEffectHolder()).MethodHandle;
			}
			result = this.m_abilityMod.m_explosionCanAffectEffectHolderMod.GetModifiedValue(this.m_explosionCanAffectEffectHolder);
		}
		else
		{
			result = this.m_explosionCanAffectEffectHolder;
		}
		return result;
	}

	public int GetExplosionHealingToAllies()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.GetExplosionHealingToAllies()).MethodHandle;
			}
			result = this.m_abilityMod.m_explosionHealingToAlliesMod.GetModifiedValue(this.m_explosionHealingToAllies);
		}
		else
		{
			result = this.m_explosionHealingToAllies;
		}
		return result;
	}

	public int GetExplosionDamageToEnemies()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.GetExplosionDamageToEnemies()).MethodHandle;
			}
			result = this.m_abilityMod.m_explosionDamageToEnemiesMod.GetModifiedValue(this.m_explosionDamageToEnemies);
		}
		else
		{
			result = this.m_explosionDamageToEnemies;
		}
		return result;
	}

	public StandardEffectInfo GetExplosionEffectToAllies()
	{
		StandardEffectInfo result;
		if (this.m_cachedExplosionEffectToAllies != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.GetExplosionEffectToAllies()).MethodHandle;
			}
			result = this.m_cachedExplosionEffectToAllies;
		}
		else
		{
			result = this.m_explosionEffectToAllies;
		}
		return result;
	}

	public StandardEffectInfo GetExplosionEffectToEnemies()
	{
		StandardEffectInfo result;
		if (this.m_cachedExplosionEffectToEnemies != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.GetExplosionEffectToEnemies()).MethodHandle;
			}
			result = this.m_cachedExplosionEffectToEnemies;
		}
		else
		{
			result = this.m_explosionEffectToEnemies;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManBubbleLaser))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_FishManBubbleLaser);
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
		AbilityMod_FishManBubbleLaser abilityMod_FishManBubbleLaser = modAsBase as AbilityMod_FishManBubbleLaser;
		StandardEffectInfo effectInfo;
		if (abilityMod_FishManBubbleLaser)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			effectInfo = abilityMod_FishManBubbleLaser.m_effectOnAlliesMod.GetModifiedValue(this.m_effectOnAllies);
		}
		else
		{
			effectInfo = this.m_effectOnAllies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnAllies", this.m_effectOnAllies, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_FishManBubbleLaser)
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
			effectInfo2 = abilityMod_FishManBubbleLaser.m_effectOnEnemiesMod.GetModifiedValue(this.m_effectOnEnemies);
		}
		else
		{
			effectInfo2 = this.m_effectOnEnemies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnEnemies", this.m_effectOnEnemies, true);
		base.AddTokenInt(tokens, "InitialHitHealingToAllies", string.Empty, (!abilityMod_FishManBubbleLaser) ? this.m_initialHitHealingToAllies : abilityMod_FishManBubbleLaser.m_initialHitHealingToAlliesMod.GetModifiedValue(this.m_initialHitHealingToAllies), false);
		string name = "InitialHitDamageToEnemies";
		string empty = string.Empty;
		int val;
		if (abilityMod_FishManBubbleLaser)
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
			val = abilityMod_FishManBubbleLaser.m_initialHitDamageToEnemiesMod.GetModifiedValue(this.m_initialHitDamageToEnemies);
		}
		else
		{
			val = this.m_initialHitDamageToEnemies;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "NumTurnsBeforeFirstExplosion";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_FishManBubbleLaser)
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
			val2 = abilityMod_FishManBubbleLaser.m_numTurnsBeforeFirstExplosionMod.GetModifiedValue(this.m_numTurnsBeforeFirstExplosion);
		}
		else
		{
			val2 = this.m_numTurnsBeforeFirstExplosion;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		base.AddTokenInt(tokens, "NumExplosionsBeforeEnding", string.Empty, (!abilityMod_FishManBubbleLaser) ? this.m_numExplosionsBeforeEnding : abilityMod_FishManBubbleLaser.m_numExplosionsBeforeEndingMod.GetModifiedValue(this.m_numExplosionsBeforeEnding), false);
		string name3 = "ExplosionHealingToAllies";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_FishManBubbleLaser)
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
			val3 = abilityMod_FishManBubbleLaser.m_explosionHealingToAlliesMod.GetModifiedValue(this.m_explosionHealingToAllies);
		}
		else
		{
			val3 = this.m_explosionHealingToAllies;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "ExplosionDamageToEnemies";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_FishManBubbleLaser)
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
			val4 = abilityMod_FishManBubbleLaser.m_explosionDamageToEnemiesMod.GetModifiedValue(this.m_explosionDamageToEnemies);
		}
		else
		{
			val4 = this.m_explosionDamageToEnemies;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		StandardEffectInfo effectInfo3;
		if (abilityMod_FishManBubbleLaser)
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
			effectInfo3 = abilityMod_FishManBubbleLaser.m_explosionEffectToAlliesMod.GetModifiedValue(this.m_explosionEffectToAllies);
		}
		else
		{
			effectInfo3 = this.m_explosionEffectToAllies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "ExplosionEffectToAllies", this.m_explosionEffectToAllies, true);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_FishManBubbleLaser) ? this.m_explosionEffectToEnemies : abilityMod_FishManBubbleLaser.m_explosionEffectToEnemiesMod.GetModifiedValue(this.m_explosionEffectToEnemies), "ExplosionEffectToEnemies", this.m_explosionEffectToEnemies, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.GetInitialHitDamageToEnemies() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManBubbleLaser.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetInitialHitDamageToEnemies());
		}
		if (this.GetInitialHitHealingToAllies() > 0)
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
			AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetInitialHitHealingToAllies());
		}
		this.GetEffectOnEnemies().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		this.GetEffectOnAllies().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		return result;
	}
}
