using System;
using System.Collections.Generic;
using UnityEngine;

public class ThiefSpoilLaserUlt : Ability
{
	[Header("-- Targeter")]
	public bool m_targeterMultiTarget;

	public float m_targeterMaxAngle = 120f;

	public float m_targeterMinInterpDistance = 1.5f;

	public float m_targeterMaxInterpDistance = 6f;

	[Header("-- Damage")]
	public int m_laserDamageAmount = 3;

	public int m_laserSubsequentDamageAmount = 3;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Laser Properties")]
	public float m_laserRange = 5f;

	public float m_laserWidth = 0.5f;

	public int m_laserMaxTargets = 1;

	public int m_laserCount = 2;

	public bool m_laserPenetrateLos;

	[Header("-- Spoil Spawn Data On Enemy Hit")]
	public SpoilsSpawnData m_spoilSpawnData;

	[Header("-- PowerUp/Spoils Interaction")]
	public bool m_hitPowerups;

	public bool m_stopOnPowerupHit = true;

	public bool m_includeSpoilsPowerups = true;

	public bool m_ignorePickupTeamRestriction;

	public int m_maxPowerupsHit;

	[Header("-- Buffs Copy --")]
	public bool m_copyBuffsOnEnemyHit;

	public int m_copyBuffDuration = 2;

	public List<StatusType> m_buffsToCopy;

	[Header("-- Sequences")]
	public GameObject m_laserSequencePrefab;

	public GameObject m_powerupReturnPrefab;

	public GameObject m_onBuffCopyAudioSequencePrefab;

	private AbilityMod_ThiefSpoilLaserUlt m_abilityMod;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private SpoilsSpawnData m_cachedSpoilSpawnData;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.Start()).MethodHandle;
			}
			this.m_abilityName = "Ult 2";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		if (this.m_targeterMultiTarget)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.Setup()).MethodHandle;
			}
			base.ClearTargeters();
			for (int i = 0; i < this.GetLaserCount(); i++)
			{
				base.Targeters.Add(new AbilityUtil_Targeter_ThiefFanLaser(this, 0f, this.GetTargeterMaxAngle(), this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance, this.GetLaserRange(), this.GetLaserWidth(), this.GetLaserMaxTargets(), this.GetLaserCount(), this.LaserPenetrateLos(), this.HitPowerups(), this.StopOnPowerupHit(), this.IncludeSpoilsPowerups(), this.IgnorePickupTeamRestriction(), this.GetMaxPowerupsHit(), 0f, 0f));
				base.Targeters[i].SetUseMultiTargetUpdate(true);
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_ThiefFanLaser(this, 0f, this.GetTargeterMaxAngle(), this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance, this.GetLaserRange(), this.GetLaserWidth(), this.GetLaserMaxTargets(), this.GetLaserCount(), this.LaserPenetrateLos(), this.HitPowerups(), this.StopOnPowerupHit(), this.IncludeSpoilsPowerups(), this.IgnorePickupTeamRestriction(), this.GetMaxPowerupsHit(), 0f, 0f);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result;
		if (this.m_targeterMultiTarget)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.GetExpectedNumberOfTargeters()).MethodHandle;
			}
			result = this.GetLaserCount();
		}
		else
		{
			result = 1;
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
		StandardEffectInfo cachedEnemyHitEffect;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.SetCachedFields()).MethodHandle;
			}
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		SpoilsSpawnData cachedSpoilSpawnData;
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
			cachedSpoilSpawnData = this.m_abilityMod.m_spoilSpawnDataMod.GetModifiedValue(this.m_spoilSpawnData);
		}
		else
		{
			cachedSpoilSpawnData = this.m_spoilSpawnData;
		}
		this.m_cachedSpoilSpawnData = cachedSpoilSpawnData;
	}

	public float GetTargeterMaxAngle()
	{
		return (!this.m_abilityMod) ? this.m_targeterMaxAngle : this.m_abilityMod.m_targeterMaxAngleMod.GetModifiedValue(this.m_targeterMaxAngle);
	}

	public int GetLaserDamageAmount()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.GetLaserDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			result = this.m_laserDamageAmount;
		}
		return result;
	}

	public int GetLaserSubsequentDamageAmount()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.GetLaserSubsequentDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserSubsequentDamageAmountMod.GetModifiedValue(this.m_laserSubsequentDamageAmount);
		}
		else
		{
			result = this.m_laserSubsequentDamageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.GetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
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
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.GetLaserRange()).MethodHandle;
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

	public int GetLaserMaxTargets()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.GetLaserMaxTargets()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserMaxTargetsMod.GetModifiedValue(this.m_laserMaxTargets);
		}
		else
		{
			result = this.m_laserMaxTargets;
		}
		return result;
	}

	public int GetLaserCount()
	{
		return (!this.m_abilityMod) ? this.m_laserCount : this.m_abilityMod.m_laserCountMod.GetModifiedValue(this.m_laserCount);
	}

	public bool LaserPenetrateLos()
	{
		return (!this.m_abilityMod) ? this.m_laserPenetrateLos : this.m_abilityMod.m_laserPenetrateLosMod.GetModifiedValue(this.m_laserPenetrateLos);
	}

	public SpoilsSpawnData GetSpoilSpawnData()
	{
		return (this.m_cachedSpoilSpawnData == null) ? this.m_spoilSpawnData : this.m_cachedSpoilSpawnData;
	}

	public bool HitPowerups()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.HitPowerups()).MethodHandle;
			}
			result = this.m_abilityMod.m_hitPowerupsMod.GetModifiedValue(this.m_hitPowerups);
		}
		else
		{
			result = this.m_hitPowerups;
		}
		return result;
	}

	public bool StopOnPowerupHit()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.StopOnPowerupHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_stopOnPowerupHitMod.GetModifiedValue(this.m_stopOnPowerupHit);
		}
		else
		{
			result = this.m_stopOnPowerupHit;
		}
		return result;
	}

	public bool IncludeSpoilsPowerups()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.IncludeSpoilsPowerups()).MethodHandle;
			}
			result = this.m_abilityMod.m_includeSpoilsPowerupsMod.GetModifiedValue(this.m_includeSpoilsPowerups);
		}
		else
		{
			result = this.m_includeSpoilsPowerups;
		}
		return result;
	}

	public bool IgnorePickupTeamRestriction()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.IgnorePickupTeamRestriction()).MethodHandle;
			}
			result = this.m_abilityMod.m_ignorePickupTeamRestrictionMod.GetModifiedValue(this.m_ignorePickupTeamRestriction);
		}
		else
		{
			result = this.m_ignorePickupTeamRestriction;
		}
		return result;
	}

	public int GetMaxPowerupsHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.GetMaxPowerupsHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_maxPowerupsHitMod.GetModifiedValue(this.m_maxPowerupsHit);
		}
		else
		{
			result = this.m_maxPowerupsHit;
		}
		return result;
	}

	public bool CopyBuffsOnEnemyHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.CopyBuffsOnEnemyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_copyBuffsOnEnemyHitMod.GetModifiedValue(this.m_copyBuffsOnEnemyHit);
		}
		else
		{
			result = this.m_copyBuffsOnEnemyHit;
		}
		return result;
	}

	public int GetCopyBuffDuration()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.GetCopyBuffDuration()).MethodHandle;
			}
			result = this.m_abilityMod.m_copyBuffDurationMod.GetModifiedValue(this.m_copyBuffDuration);
		}
		else
		{
			result = this.m_copyBuffDuration;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_laserDamageAmount);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (this.GetExpectedNumberOfTargeters() < 2)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			this.AccumulateDamageFromTargeter(targetActor, base.Targeter, dictionary);
		}
		else
		{
			for (int i = 0; i <= currentTargeterIndex; i++)
			{
				this.AccumulateDamageFromTargeter(targetActor, base.Targeters[i], dictionary);
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
		return dictionary;
	}

	private void AccumulateDamageFromTargeter(ActorData targetActor, AbilityUtil_Targeter targeter, Dictionary<AbilityTooltipSymbol, int> symbolToDamage)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.AccumulateDamageFromTargeter(ActorData, AbilityUtil_Targeter, Dictionary<AbilityTooltipSymbol, int>)).MethodHandle;
			}
			foreach (AbilityTooltipSubject abilityTooltipSubject in tooltipSubjectTypes)
			{
				if (abilityTooltipSubject == AbilityTooltipSubject.Primary)
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
					if (!symbolToDamage.ContainsKey(AbilityTooltipSymbol.Damage))
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
						symbolToDamage[AbilityTooltipSymbol.Damage] = this.GetLaserDamageAmount();
					}
					else
					{
						symbolToDamage[AbilityTooltipSymbol.Damage] = symbolToDamage[AbilityTooltipSymbol.Damage] + this.GetLaserSubsequentDamageAmount();
					}
				}
			}
		}
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefSpoilLaserUlt abilityMod_ThiefSpoilLaserUlt = modAsBase as AbilityMod_ThiefSpoilLaserUlt;
		string name = "LaserDamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_ThiefSpoilLaserUlt)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_ThiefSpoilLaserUlt.m_laserDamageAmountMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			val = this.m_laserDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "LaserSubsequentDamageAmount";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ThiefSpoilLaserUlt)
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
			val2 = abilityMod_ThiefSpoilLaserUlt.m_laserSubsequentDamageAmountMod.GetModifiedValue(this.m_laserSubsequentDamageAmount);
		}
		else
		{
			val2 = this.m_laserSubsequentDamageAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		base.AddTokenInt(tokens, "LaserDamageTotalCombined", string.Empty, this.m_laserDamageAmount + this.m_laserSubsequentDamageAmount, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_ThiefSpoilLaserUlt)
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
			effectInfo = abilityMod_ThiefSpoilLaserUlt.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", this.m_enemyHitEffect, true);
		string name3 = "LaserMaxTargets";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_ThiefSpoilLaserUlt)
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
			val3 = abilityMod_ThiefSpoilLaserUlt.m_laserMaxTargetsMod.GetModifiedValue(this.m_laserMaxTargets);
		}
		else
		{
			val3 = this.m_laserMaxTargets;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "LaserCount";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_ThiefSpoilLaserUlt)
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
			val4 = abilityMod_ThiefSpoilLaserUlt.m_laserCountMod.GetModifiedValue(this.m_laserCount);
		}
		else
		{
			val4 = this.m_laserCount;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		base.AddTokenInt(tokens, "CopyBuffDuration", string.Empty, (!abilityMod_ThiefSpoilLaserUlt) ? this.m_copyBuffDuration : abilityMod_ThiefSpoilLaserUlt.m_copyBuffDurationMod.GetModifiedValue(this.m_copyBuffDuration), false);
		base.AddTokenInt(tokens, "MaxPowerupsHit", string.Empty, (!abilityMod_ThiefSpoilLaserUlt) ? this.m_maxPowerupsHit : abilityMod_ThiefSpoilLaserUlt.m_maxPowerupsHitMod.GetModifiedValue(this.m_maxPowerupsHit), false);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = this.m_targeterMinInterpDistance * Board.\u000E().squareSize;
		max = this.m_targeterMaxInterpDistance * Board.\u000E().squareSize;
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ThiefSpoilLaserUlt))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ThiefSpoilLaserUlt.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ThiefSpoilLaserUlt);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
