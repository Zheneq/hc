using System;
using System.Collections.Generic;
using UnityEngine;

public class MantaConeDirtyFighting : Ability
{
	[Header("-- Targeting")]
	public float m_coneRange = 4f;

	public float m_coneWidth = 60f;

	public bool m_penetrateLoS;

	public int m_maxTargets = 5;

	public float m_coneBackwardOffset;

	[Header("-- Hit Damage/Effects")]
	public int m_onCastDamageAmount;

	public StandardActorEffectData m_dirtyFightingEffectData;

	public StandardEffectInfo m_enemyHitEffectData;

	public StandardEffectInfo m_effectOnTargetFromExplosion;

	[Header("-- On Reaction Hit/Explosion Triggered")]
	public int m_effectExplosionDamage = 0x1E;

	[Tooltip("whether allies other than yourself should be able to trigger the explosion")]
	public bool m_explodeOnlyFromSelfDamage;

	public int m_techPointGainPerExplosion = 5;

	public int m_healAmountPerExplosion;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_effectOnExplosionSequencePrefab;

	private AbilityMod_MantaConeDirtyFighting m_abilityMod;

	private StandardActorEffectData m_cachedDirtyFightingEffectData;

	private StandardEffectInfo m_cachedEnemyHitEffectData;

	private StandardEffectInfo m_cachedEffectOnTargetFromExplosion;

	private StandardEffectInfo m_cachedEffectOnTargetWhenExpiresWithoutExplosion;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.Start()).MethodHandle;
			}
			this.m_abilityName = "Dirty Fighting Cone";
		}
		this.SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetConeRange();
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedDirtyFightingEffectData;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.SetCachedFields()).MethodHandle;
			}
			cachedDirtyFightingEffectData = this.m_abilityMod.m_dirtyFightingEffectDataMod.GetModifiedValue(this.m_dirtyFightingEffectData);
		}
		else
		{
			cachedDirtyFightingEffectData = this.m_dirtyFightingEffectData;
		}
		this.m_cachedDirtyFightingEffectData = cachedDirtyFightingEffectData;
		StandardEffectInfo cachedEnemyHitEffectData;
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
			cachedEnemyHitEffectData = this.m_abilityMod.m_enemyHitEffectDataMod.GetModifiedValue(this.m_enemyHitEffectData);
		}
		else
		{
			cachedEnemyHitEffectData = this.m_enemyHitEffectData;
		}
		this.m_cachedEnemyHitEffectData = cachedEnemyHitEffectData;
		StandardEffectInfo cachedEffectOnTargetFromExplosion;
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
			cachedEffectOnTargetFromExplosion = this.m_abilityMod.m_effectOnTargetFromExplosionMod.GetModifiedValue(this.m_effectOnTargetFromExplosion);
		}
		else
		{
			cachedEffectOnTargetFromExplosion = this.m_effectOnTargetFromExplosion;
		}
		this.m_cachedEffectOnTargetFromExplosion = cachedEffectOnTargetFromExplosion;
		this.m_cachedEffectOnTargetWhenExpiresWithoutExplosion = ((!this.m_abilityMod) ? null : this.m_abilityMod.m_effectOnTargetWhenExpiresWithoutExplosionMod.GetModifiedValue(null));
	}

	public float GetConeRange()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.GetConeRange()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneRangeMod.GetModifiedValue(this.m_coneRange);
		}
		else
		{
			result = this.m_coneRange;
		}
		return result;
	}

	public float GetConeWidth()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.GetConeWidth()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneWidthMod.GetModifiedValue(this.m_coneWidth);
		}
		else
		{
			result = this.m_coneWidth;
		}
		return result;
	}

	public bool PenetrateLoS()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.PenetrateLoS()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLoSMod.GetModifiedValue(this.m_penetrateLoS);
		}
		else
		{
			result = this.m_penetrateLoS;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		return (!this.m_abilityMod) ? this.m_maxTargets : this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
	}

	public float GetConeBackwardOffset()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.GetConeBackwardOffset()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(this.m_coneBackwardOffset);
		}
		else
		{
			result = this.m_coneBackwardOffset;
		}
		return result;
	}

	public int GetOnCastDamageAmount()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.GetOnCastDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_onCastDamageAmountMod.GetModifiedValue(this.m_onCastDamageAmount);
		}
		else
		{
			result = this.m_onCastDamageAmount;
		}
		return result;
	}

	public StandardActorEffectData GetDirtyFightingEffectData()
	{
		StandardActorEffectData result;
		if (this.m_cachedDirtyFightingEffectData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.GetDirtyFightingEffectData()).MethodHandle;
			}
			result = this.m_cachedDirtyFightingEffectData;
		}
		else
		{
			result = this.m_dirtyFightingEffectData;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffectData()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffectData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.GetEnemyHitEffectData()).MethodHandle;
			}
			result = this.m_cachedEnemyHitEffectData;
		}
		else
		{
			result = this.m_enemyHitEffectData;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnTargetFromExplosion()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnTargetFromExplosion != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.GetEffectOnTargetFromExplosion()).MethodHandle;
			}
			result = this.m_cachedEffectOnTargetFromExplosion;
		}
		else
		{
			result = this.m_effectOnTargetFromExplosion;
		}
		return result;
	}

	public StandardActorEffectData GetEffectOnTargetWhenExpiresWithoutExplosion()
	{
		if (this.m_cachedEffectOnTargetWhenExpiresWithoutExplosion != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.GetEffectOnTargetWhenExpiresWithoutExplosion()).MethodHandle;
			}
			if (this.m_cachedEffectOnTargetWhenExpiresWithoutExplosion.m_applyEffect)
			{
				return this.m_cachedEffectOnTargetWhenExpiresWithoutExplosion.m_effectData;
			}
		}
		return null;
	}

	public int GetEffectExplosionDamage()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.GetEffectExplosionDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_effectExplosionDamageMod.GetModifiedValue(this.m_effectExplosionDamage);
		}
		else
		{
			result = this.m_effectExplosionDamage;
		}
		return result;
	}

	public bool ExplodeOnlyFromSelfDamage()
	{
		return (!this.m_abilityMod) ? this.m_explodeOnlyFromSelfDamage : this.m_abilityMod.m_explodeOnlyFromSelfDamageMod.GetModifiedValue(this.m_explodeOnlyFromSelfDamage);
	}

	public int GetTechPointGainPerExplosion()
	{
		return (!this.m_abilityMod) ? this.m_techPointGainPerExplosion : this.m_abilityMod.m_techPointGainPerExplosionMod.GetModifiedValue(this.m_techPointGainPerExplosion);
	}

	public int GetHealAmountPerExplosion()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.GetHealAmountPerExplosion()).MethodHandle;
			}
			result = this.m_abilityMod.m_healPerExplosionMod.GetModifiedValue(this.m_healAmountPerExplosion);
		}
		else
		{
			result = this.m_healAmountPerExplosion;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MantaConeDirtyFighting))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MantaConeDirtyFighting.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_MantaConeDirtyFighting);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, this.GetConeWidth(), this.GetConeRange(), this.m_coneBackwardOffset, this.PenetrateLoS(), true, true, false, false, this.GetMaxTargets(), false);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "OnCastDamageAmount", string.Empty, this.m_onCastDamageAmount, false);
		this.m_dirtyFightingEffectData.AddTooltipTokens(tokens, "DirtyFightingEffectData", false, null);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffectData, "EnemyHitEffectData", this.m_enemyHitEffectData, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnTargetFromExplosion, "EffectOnTargetFromExplosion", this.m_effectOnTargetFromExplosion, true);
		base.AddTokenInt(tokens, "EffectExplosionDamage", string.Empty, this.m_effectExplosionDamage, false);
		base.AddTokenInt(tokens, "TechPointGainPerExplosion", string.Empty, this.m_techPointGainPerExplosion, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetOnCastDamageAmount());
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Tertiary, this.GetEffectExplosionDamage());
		this.GetEnemyHitEffectData().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Tertiary);
		return result;
	}
}
